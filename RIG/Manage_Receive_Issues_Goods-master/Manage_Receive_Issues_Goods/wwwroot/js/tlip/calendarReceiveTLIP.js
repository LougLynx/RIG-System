document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    
   const notificationConnection = new signalR.HubConnectionBuilder()
       .withUrl("/updateReceiveTLIPHub")
        .build();
    notificationConnection.start().then(function () {
        console.log("SignalR connected.");
    }).catch(function (err) {
        console.error("Failed to connect SignalR: ", err.toString());
    });


    notificationConnection.on("UpdateCalendar", function (actualReceived) {
        /*console.log("Received update:", actualReceived);
        const start = new Date(actualReceived.actualDeliveryTime);
        const end = new Date(start);

        if (actualReceived.actualLeadTime) {
            end.setMinutes(end.getMinutes() + actualReceived.actualLeadTime);
        } else {
            end.setHours(end.getHours() + 1);
        }

        const formattedStart = formatDateTime(start);
        const formattedEnd = formatDateTime(end);
        let existingEvent = calendar.getEventById(actualReceived.actualReceivedId);
        if (!existingEvent) {
            calendar.addEvent({
                id: actualReceived.actualReceivedId,  
                title: actualReceived.supplierName,
                start: formattedStart,
                end: formattedEnd,
                resourceId: 2,
                extendedProps: {
                    actualReceivedId: actualReceived.actualReceivedId,
                    supplierCode: actualReceived.supplierCode
                }
            });
        }*/

        console.log("Received update via SignalR:", actualReceived);
        updateCalendarEvent(actualReceived);

    });

    function updateCalendarEvent(actualReceived) {
        const start = new Date(actualReceived.actualDeliveryTime);
        const end = new Date(start);

        if (actualReceived.actualLeadTime) {
            end.setMinutes(end.getMinutes() + actualReceived.actualLeadTime);
        } else {
            end.setHours(end.getHours() + 1);
        }

        const formattedStart = formatDateTime(start);
        const formattedEnd = formatDateTime(end);

        let existingEvent = calendar.getEventById(actualReceived.actualReceivedId);
        if (!existingEvent) {   
            calendar.addEvent({
                id: actualReceived.actualReceivedId,
                title: actualReceived.supplierName,
                start: formattedStart,
                end: formattedEnd,
                resourceId: 2,
                extendedProps: {
                    actualReceivedId: actualReceived.actualReceivedId,
                    supplierCode: actualReceived.supplierCode
                }
            });
        } else {
            existingEvent.setStart(formattedStart);
            existingEvent.setEnd(formattedEnd);
        }
    }

    // Periodically update event end time if completion percentage is less than 100%
    setInterval(function () {
        // Fetch the latest data to update events
        getActualReceived().then(data => {
            data.forEach(actualReceived => {
                // Only update if completion percentage is less than 100%
                if (actualReceived.completionPercentage < 100) {
                    updateCalendarEvent(actualReceived);
                }
            });
        }).catch(error => {
            console.error("Error fetching actual received data:", error);
        });
    }, 5000); // Every 5 seconds

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

    function updateEventEndInDatabase(actualReceivedId, end) {
        fetch(`/TLIPWarehouse/UpdateActualLeadTime`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                actualReceivedId: actualReceivedId,
                actualDeliveryTime: end // Truyền thời gian kết thúc, API sẽ tính toán ActualLeadTime
            })
        }).then(response => {
            if (!response.ok) {
                console.error('Failed to update ActualLeadTime:', response.statusText);
            }
        }).catch(function (err) {
            console.error('Error updating ActualLeadTime:', err.toString());
        });
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
        resources: '/api/ResourcesReceivedTLIP',
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
                        let end = new Date(start);

                        const completedItems = actualReceived.ActualDetails.filter(detail => detail.QuantityRemain === 0).length;
                        const totalItems = actualReceived.ActualDetails.length;
                        const completionPercentage = (completedItems / totalItems) * 100;

                        let eventColor = completionPercentage === 100 ? '#32AB25' : '#C7B44F';

                        // Nếu phần trăm hoàn thành là 0, set thời gian kết thúc mặc định +1 tiếng
                        if (completionPercentage === 0) {
                            end.setHours(end.getHours() + 1);
                        } else {
                            // Nếu phần trăm không phải 0, set thời gian kết thúc là thời gian hiện tại
                            end = new Date();
                        }

                        const formattedStart = formatDateTime(start);
                        const formattedEnd = formatDateTime(end);

                        events.push({
                            id: actualReceived.ActualReceivedId,
                            title: actualReceived.SupplierName,
                            start: formattedStart,
                            end: formattedEnd,
                            resourceId: resourceIdCounter,
                            backgroundColor: eventColor,  // Đặt màu sắc cho sự kiện
                            extendedProps: {
                                actualReceivedId: actualReceived.ActualReceivedId,
                                supplierCode: actualReceived.SupplierCode,
                                completionPercentage: completionPercentage
                            }
                        });

                        // Sau khi cập nhật thời gian kết thúc (end), cập nhật vào database
                        if (completionPercentage !== 0) {
                            const formattedEnd = formatDateTime(new Date());  // Thời gian kết thúc là thời gian hiện tại
                            updateEventEndInDatabase(actualReceived.ActualReceivedId, formattedEnd);
                        }

                        resourceIdCounter += 2;
                    });

                    successCallback(events);
                })
                .catch(error => failureCallback(error));
        }
,
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
            const supplierCode = info.event.extendedProps.supplierCode;
            const supplierName = info.event.title; 
            const start = info.event.start; 
            const end = info.event.end; 

            const formattedStart = new Date(start).toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit'
            });

            const formattedEnd = new Date(end).toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit'
            });

            const eventDetails = `
        <strong>Nhà cung cấp:</strong> ${supplierName} - ${supplierCode}<br>
        <i><strong>Nhận lúc:</strong></i> ${formattedStart}<br>
        <i><strong>Kết thúc lúc:</strong></i> ${formattedEnd}
    `;
            document.getElementById('eventDetails').innerHTML = eventDetails;

            // Gán actualReceivedId vào modal để sử dụng khi cần
            document.getElementById('eventModal').setAttribute('data-actual', actualReceivedId);

            // Lấy chi tiết giai đoạn cho actualReceivedId và hiển thị chúng
            fetchAndPopulateStagesTable(actualReceivedId);

            // Mở modal
            var eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
            eventModal.show();
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
       // return fetch(`/TLIPWarehouse/GetAsnDetail?asnNumber=${asnNumber}&doNumber=${doNumber}&invoice=${invoice}`)
        return fetch(`/TLIPWarehouse/ParseAsnDetailFromFile?asnNumber=${asnNumber}&doNumber=${doNumber}&invoice=${invoice}`)
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
                            .then(async actualReceivedEntry => {
                                console.log('Retrieved actualReceivedEntry:', actualReceivedEntry);

                                // Fetch ASN details and update stages table
                                const asnDetails = await getAsnDetail(
                                    /*actualReceivedEntry.AsnNumber ? actualReceivedEntry.AsnNumber : '',
                                    actualReceivedEntry.DoNumber ? actualReceivedEntry.DoNumber : '',   `
                                    actualReceivedEntry.Invoice ? actualReceivedEntry.Invoice : ''*/
                                    actualReceivedEntry.AsnNumber ? actualReceivedEntry.AsnNumber : null,
                                    actualReceivedEntry.DoNumber ? actualReceivedEntry.DoNumber : null,
                                    actualReceivedEntry.Invoice ? actualReceivedEntry.Invoice : null
                                );
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

                                // Ensure stages table is populated before invoking UpdateCalendar
                                //await populateStagesTable(asnDetails);

                                // Now invoke the update after stages table is updated
                              /*  const updateData = { ...actualReceivedEntry };
                                console.log('Update data:', updateData);
                                notificationConnection.invoke("UpdateCalendar", updateData)
                                    .catch(function (err) {
                                        return console.error(err.toString());
                                    });*/
                            })
                            .catch(error => {
                                console.error('Error adding ActualDetailTLIP:', error);
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
                                }
                            });
                            populateStagesTable(asnDetails);

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


    function populateStagesTable(asnDetails) {
        const stagesTableBody = document.getElementById('stagesTableBody');
        stagesTableBody.innerHTML = '';

        let totalItems = asnDetails.length;
        let completedItems = 0;

        asnDetails.forEach(detail => {
            const row = document.createElement('tr');
            const isDone = detail.QuantityRemain === 0;
            if (isDone) {
                completedItems++;
            }
            row.innerHTML = `
        <td>${detail.PartNo}</td>
        <td>${detail.Quantity}</td>
        <td>${isDone ? 'Done' : 'Pending...'}</td>
    `;
            stagesTableBody.appendChild(row);
        });

        // Tính toán phần trăm hoàn thành
        let completionPercentage = (completedItems / totalItems) * 100;

        // Cập nhật giao diện để hiển thị phần trăm hoàn thành
        const completionElement = document.getElementById('completionPercentage');
        let color = completionPercentage < 100 ? '#C7B44F' : '#32AB25'; 

        if (completionElement) {
            completionElement.textContent = `Đã hoàn thành được: ${completionPercentage.toFixed(2)}%`;
            completionElement.style.color = color; // Cập nhật màu sắc
        } else {
            // Nếu phần tử hiển thị chưa tồn tại, tạo mới
            const newCompletionElement = document.createElement('div');
            newCompletionElement.id = 'completionPercentage';
            newCompletionElement.textContent = `Đã hoàn thành được: ${completionPercentage.toFixed(2)}%`;
            newCompletionElement.style.color = color; 
            document.body.appendChild(newCompletionElement); 
        }
    }



    function fetchAndPopulateStagesTable(actualReceivedId) {
        fetch(`/TLIPWarehouse/GetActualDetailsByReceivedId?actualReceivedId=${actualReceivedId}`)
            .then(response => response.json())
            .then(data => {
                console.log('Actual details:', data);
                populateStagesTable(data);
                /*var eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
                eventModal.show();*/
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
