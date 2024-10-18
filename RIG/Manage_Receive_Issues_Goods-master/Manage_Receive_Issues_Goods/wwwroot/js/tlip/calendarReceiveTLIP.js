document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    
   const notificationConnection = new signalR.HubConnectionBuilder()
       .withUrl("/updateReceiveTLIPHub")
        .build();
    notificationConnection.start().then(function () {
        console.log("SignalR connected.");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    notificationConnection.on("UpdateCalendar", function (actualReceived) {
        console.log("Received update:", actualReceived);
        const start = new Date(actualReceived.actualDeliveryTime);
        const end = new Date(start);

        if (actualReceived.actualLeadTime) {
            end.setMinutes(end.getMinutes() + actualReceived.actualLeadTime);
        } else {
            end.setHours(end.getHours() + 1);
        }

        const formattedStart = formatDateTime(start);
        const formattedEnd = formatDateTime(end);
        calendar.addEvent({
            title: actualReceived.supplierName,
            start: formattedStart,
            end: formattedEnd,
            resourceId: 2,
            extendedProps: {
                actualReceivedId: actualReceived.actualReceivedId
            }
        });
    });


    function formatDateToLocalString(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    function formatDateTime(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
    }

    function getFormattedNow() {
        var now = new Date();
        var year = now.getFullYear();
        var month = String(now.getMonth() + 1).padStart(2, '0');
        var day = String(now.getDate()).padStart(2, '0');
        var hours = String(now.getHours()).padStart(2, '0');
        var minutes = String(now.getMinutes()).padStart(2, '0');
        var seconds = String(now.getSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
    }
    //Lấy thời gian thực cho Indicator
    var now = getFormattedNow();

    /*//Hàm để lấy nhà cung cấp cho hôm nay
    function loadSuppliersForToday() {
        fetch('/TLIPWarehouse/GetSuppliersForToday')
            .then(response => response.json())
            .then(data => {
                var suppliersHTML = '';
                data.forEach(supplier => {
                    suppliersHTML += `<div class="supplier-block">${supplier.supplierName}</div>`;
                });
                document.getElementById('suppliersListContent').innerHTML = suppliersHTML;
            });
    }*/

    function getActualReceived() {
        return fetch('/TLIPWarehouse/GetActualReceived')
            .then(response => response.json());
    }


  

    //KHỞI TẠO LỊCH
    var calendar = new FullCalendar.Calendar(calendarEl, {
        schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
        initialView: 'resourceTimelineDay',
        // Mỗi slot là 30 phút
        slotDuration: '00:30',
        // Mỗi slot cách nhau 1 tiếng
        slotLabelInterval: '00:30',
        slotLabelFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false // Sử dụng định dạng 24 giờ
        },
        // Hiển thị các ô thời gian từ 6 giờ sáng hôm trước đến 6 giờ sáng hôm sau
        slotMinTime: '06:00:00',
        slotMaxTime: '30:00:00',
        stickyFooterScrollbar: 'auto',
        resourceAreaWidth: '110px',
        //Gọi Indicator
        nowIndicator: true,
        //set Indicator với thời gian thực
        now: now,
        timeZone: 'Asia/Bangkok',
        locale: 'en-GB',
        aspectRatio: 2.0,
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'resourceTimelineDay,resourceTimelineWeek'
        },
        editable: false,
        resourceAreaHeaderContent: 'Details/Hour',
        //Lấy API để hiển thị cột Actual và Plan
        resources: '/api/Resources',
        //Sắp xếp theo thứ tự theo order(Plan trước Actual sau)
        resourceOrder: 'order',
        events: function (fetchInfo, successCallback, failureCallback) {
            getActualReceived()
                .then(data => {
                    console.log("data:", data);
                    const events = [];
                    let resourceIdCounter = 2;
                    data.forEach(actualReceived => {
                        console.log("actualReceived:", actualReceived);
                        const start = new Date(actualReceived.ActualDeliveryTime);
                        const end = new Date(start);

                        if (actualReceived.ActualLeadTime) {
                            end.setMinutes(end.getMinutes() + actualReceived.ActualLeadTime);
                        } else {
                            end.setHours(end.getHours() + 1);
                        }

                        const formattedStart = formatDateTime(start);
                        console.log("formattedStart:", formattedStart);
                        const formattedEnd = formatDateTime(end);
                        console.log("formattedEnd:", formattedEnd);
                        events.push({
                            title: actualReceived.SupplierName,
                            start: formattedStart,
                            end: formattedEnd,
                            resourceId: resourceIdCounter,
                            extendedProps: {
                                actualReceivedId: actualReceived.ActualReceivedId
                            }
                        });
                        resourceIdCounter += 2;
                    });
                    successCallback(events);
                })
                .catch(error => failureCallback(error));
        },
        // Không cho phép kéo sự kiện để thay đổi thời gian bắt đầu
        eventStartEditable: false,
        // Không cho phép thay đổi độ dài (thời lượng) sự kiện
        eventDurationEditable: false,
        //Hover viền khi di chuột 
        eventMouseEnter: function (info) {
            info.el.classList.add('highlighted-event');
        },
        eventMouseLeave: function (info) {
            info.el.classList.remove('highlighted-event');
        },
        resourceLabelClassNames: function (arg) {
            return ['custom-resource-label'];
        },

        //HÀM XỬ LÝ KHI CLICK VÀO SỰ KIỆN
        eventClick: function (info) {
            const actualReceivedId = info.event.extendedProps.actualReceivedId;
            fetchAndPopulateStagesTable(actualReceivedId);
        },
        //FOMAT NGÀY THÁNG
        views: {
            resourceTimelineDay: {
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' }
            },
            resourceTimelineWeek: {
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' }
            }
        },
        eventContent: function (arg) {
            let content = document.createElement('div');
            content.classList.add('centered-event');
            content.innerHTML = arg.event.title;
            return { domNodes: [content] };
        },
        resourceLaneClassNames: function (arg) {
            if (arg.resource.title === "Actual") {
                return ['gray-background'];
            }
            return ['white-background'];
        },
        resourceLabelClassNames: function (arg) {
            if (arg.resource.title === "Actual") {
                return ['gray-background'];
            }
            return ['white-background'];
        },
        slotLabelContent: function (arg) {
            return { html: `<i style="color: blue; text-decoration: none;">${arg.text}</i>` };
        },
        slotLabelClassNames: function (arg) {
            return ['custom-slot-label'];
        }

    });

    function getAsnDetail(asnNumber, doNumber, invoice) {
        return fetch(`/TLIPWarehouse/GetAsnDetail?asnNumber=${asnNumber}&doNumber=${doNumber}&invoice=${invoice}`)
            .then(response => response.json());
    }
    function getActualReceivedEntry(supplierCode, actualDeliveryTime) {
        return fetch(`/TLIPWarehouse/GetActualReceivedEntry?supplierCode=${supplierCode}&actualDeliveryTime=${actualDeliveryTime}`)
            .then(response => response.json());
    }

    var previousData = [];
    function fetchData() {
        console.log('fetchData called at', new Date().toLocaleTimeString());
        //fetch('/TLIPWarehouse/GetAsnInformation')
        fetch('/TLIPWarehouse/ParseAsnInformationFromFile')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(text => {
                if (!text) {
                    throw new Error('Empty response');
                }
                return JSON.parse(text);
            })
            .then(newData => {
                console.log('Data fetched:', newData);
                var now = new Date();
                newData.forEach(newItem => {
                    var previousItem = previousData.find(item =>
                        (item.AsnNumber && item.AsnNumber === newItem.AsnNumber) ||
                        (item.DoNumber && item.DoNumber === newItem.DoNumber) ||
                        (item.Invoice && item.Invoice === newItem.Invoice)
                    );
                    console.log('previousItem:', previousItem);
                    if (previousItem && !previousItem.ReceiveStatus && newItem.ReceiveStatus) {
                        var actualReceived = {
                            ActualDeliveryTime: formatDateTime(now),
                            SupplierCode: newItem.SupplierCode,
                            AsnNumber: newItem.AsnNumber,
                            DoNumber: newItem.DoNumber,
                            Invoice: newItem.Invoice
                        };
                        console.log('Posting data:', actualReceived);
                        fetch('/TLIPWarehouse/AddActualReceived', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(actualReceived)
                        })
                            .then(data => {
                                console.log('Data successfully posted:', data);
                                return fetch(`/TLIPWarehouse/GetActualReceivedEntry?supplierCode=${actualReceived.SupplierCode}&actualDeliveryTime=${actualReceived.ActualDeliveryTime}`);
                            })
                            .then(response => response.json())
                            .then(actualReceivedEntry => {
                                console.log('Retrieved actualReceivedEntry:', actualReceivedEntry);
                                const updateData = { ...actualReceivedEntry };
                                console.log('Update data:', updateData);
                                notificationConnection.invoke("UpdateCalendar", updateData)
                                    .catch(function (err) {
                                        return console.error(err.toString());
                                    });

                                return getAsnDetail(
                                    actualReceivedEntry.AsnNumber ? actualReceivedEntry.AsnNumber : '',
                                    actualReceivedEntry.DoNumber ? actualReceivedEntry.DoNumber : '',
                                    actualReceivedEntry.Invoice ? actualReceivedEntry.Invoice : ''
                                )
                                    .then (asnDetails => {
                                        console.log('ASN Details:', asnDetails);

                                        asnDetails.forEach(asnDetail => {
                                            const actualDetail = {
                                                ActualReceivedId: actualReceivedEntry.ActualReceivedId,
                                                PartNo: asnDetail.PartNo,
                                                Quantity: asnDetail.Quantity,
                                                QuantityRemain: asnDetail.QuantityRemain
                                            };
                                            console.log('Posting ActualDetailTLIP:', actualDetail);
                                            

                                            fetch('/TLIPWarehouse/AddActualDetail', {
                                                method: 'POST',
                                                headers: {
                                                    'Content-Type': 'application/json'
                                                },
                                                body: JSON.stringify(actualDetail)
                                            }).then(response => {
                                                if (!response.ok) {
                                                    console.error('Failed to post ActualDetailTLIP:', response.statusText);
                                                }
                                            }).catch(function (err) {
                                                console.error('Error posting ActualDetailTLIP:', err.toString());
                                            });
                                            
                                        });
                                        // Update the stages table
                                        populateStagesTable(asnDetails);
                                    })
                                    .then(result => {
                                        console.log('ActualDetailTLIP successfully added:', result);
                                    })
                                    .catch(error => {
                                        console.error('Error adding ActualDetailTLIP:', error);
                                    });
                            });
                    }
                });
                previousData = newData;
            })
            .catch(error => console.error('Error fetching data:', error));
    }
    setInterval(fetchData, 5000);


    let previousDataDetail = [];
    function fetchDataDetail() {
        getActualReceived().then(data => {
            console.log("Data actual nè: ", data);
            data.forEach(actualReceived => {
                console.log('Fetching data for:', actualReceived);
                getActualReceivedEntry(actualReceived.SupplierCode, actualReceived.ActualDeliveryTime)
                    .then(actualReceivedEntry => {
                        getAsnDetail(
                            actualReceivedEntry.AsnNumber ? actualReceivedEntry.AsnNumber : '',
                            actualReceivedEntry.DoNumber ? actualReceivedEntry.DoNumber : '',
                            actualReceivedEntry.Invoice ? actualReceivedEntry.Invoice : ''
                        ).then(asnDetails => {
                            asnDetails.forEach(asnDetail => {
                                const previousDetail = previousDataDetail.find(d => d.PartNo === asnDetail.PartNo);
                                if (previousDetail && previousDetail.QuantityRemain !== 0 && asnDetail.QuantityRemain === 0) {
                                    // Update the database
                                    const url = `/TLIPWarehouse/UpdateActualDetailTLIP?partNo=${asnDetail.PartNo}&actualReceivedId=${actualReceivedEntry.ActualReceivedId}&quantityRemain=0`;
                                    fetch(url, {
                                        method: 'POST'
                                    }).then(response => {
                                        if (!response.ok) {
                                            console.error('Failed to update ActualDetailTLIP:', response.statusText);
                                        }
                                    }).catch(function (err) {
                                        console.error('Error updating ActualDetailTLIP:', err.toString());
                                    });
                                    populateStagesTable(asnDetail);
                                }
                            });
                            // Update previousData with the latest data
                            previousDataDetail = asnDetails;
                        });
                    });
            });
        }).catch(error => {
            console.error('Error fetching data:', error);
        });
    }

    // Start polling every 5 seconds
    setInterval(fetchDataDetail, 5000);


     function populateStagesTable(asnDetail) {
        const stagesTableBody = document.getElementById('stagesTableBody');
        stagesTableBody.innerHTML = ''; // Clear existing rows

        asnDetail.forEach(detail => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${detail.PartNo}</td>
                <td>${detail.Quantity}</td>
                <td>${detail.QuantityRemain !== 0 ? 'Pending...' : 'Done'}</td>
            `;
            stagesTableBody.appendChild(row);
        });
    }

    function fetchAndPopulateStagesTable(actualReceivedId) {
        fetch(`/TLIPWarehouse/GetActualDetailsByReceivedId?actualReceivedId=${actualReceivedId}`)
            .then(response => response.json())
            .then(data => {
                console.log('Actual details:', data);
                populateStagesTable(data);
                var eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
                eventModal.show();
            })
            .catch(error => console.error('Error fetching actual details:', error));
    }
  /*  function loadAsnInformation() {
        console.log('loadAsnInformation called at', new Date().toLocaleTimeString());
        var currentDate = getCurrentDate();
        fetch(`/api/tlipwarehouse/getAsnInformation?inputDate=${currentDate}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                if (data.success === false) {
                    console.error('Error fetching ASN information:', data.message);
                } else {
                    console.log('ASN Information:', data);
                    // Process the ASN information as needed
                }
            })
            .catch(error => console.error('Error fetching ASN information:', error));
    }
    setInterval(loadAsnInformation, 5000);*/

    // Call loadAsnInformation when the calendar is rendered
   /* calendar.on('datesSet', function () {
        loadAsnInformation();
    });*/
    
    calendar.render();
 /*   function getCurrentDate() {
        var view = calendar.view;
        console.log(view);
        var startDate = view.currentStart;
        return startDate.toISOString().split('T')[0]; // Format as 'YYYY-MM-DD'
    }*/




    //Tải nhà cung cấp lên khi tải trang 
   // loadSuppliersForToday();

    //HIỂN THỊ THỜI GIAN THỰC
    function updateTime() {
        const now = new Date();
        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        const seconds = String(now.getSeconds()).padStart(2, '0');
        const currentTime = `${hours}:${minutes}:${seconds}`;
        document.getElementById('currentTime').textContent = currentTime;
    }
    setInterval(updateTime, 1000);
    updateTime();

/*    //HÀM XỬ LÝ SỰ KIỆN KHI ẤN DELAY
    document.getElementById('delayButton').addEventListener('click', async function () {
        const supplierId = document.getElementById('eventModal').getAttribute('data-actual');
        console.log("supplierId:", supplierId);
        const response = await fetch('/api/tlipwarehouse/delaySupplier', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(supplierId)
        });

        const data = await response.json();
        if (data.success) {
            console.log('Delay processed successfully.');
        } else {
            console.error('Failed to process delay.');
        }
    });*/
});
