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
        if (actualReceived && typeof actualReceived === 'object' && !Array.isArray(actualReceived)) {
            console.log("Received update via SignalR:", actualReceived);

            // Convert properties to PascalCase
            actualReceived = convertToPascalCase(actualReceived);
            console.log("actualReceived after parse:", actualReceived);

            try {
                updateActualReceivedList();
                console.log("updateActualReceivedList executed successfully.");
            } catch (error) {
                console.error("Error in updateActualReceivedList:", error);
            }

            try {
                updateCalendarEvent(actualReceived);
                console.log("updateCalendarEvent executed successfully.");
            } catch (error) {
                console.error("Error in updateCalendarEvent:", error);
            }
        } else {
            console.error("actualReceived is not a valid object:", actualReceived);
        }
    });

    function convertToPascalCase(obj) {
        if (Array.isArray(obj)) {
            return obj.map(item => convertToPascalCase(item));
        } else if (obj !== null && obj.constructor === Object) {
            return Object.keys(obj).reduce((acc, key) => {
                const pascalCaseKey = key.charAt(0).toUpperCase() + key.slice(1);
                acc[pascalCaseKey] = convertToPascalCase(obj[key]);
                return acc;
            }, {});
        }
        return obj;
    }


    function updateActualReceivedList() {
        getActualReceived().then(function (data) {
            var modalsContainer = $('#modalsContainer');
            modalsContainer.empty();

            data.forEach(function (actualReceived) {
                var modalId = 'eventModal-' + actualReceived.ActualReceivedId;
                var modalHtml = `
                    <div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="eventModalLabel" aria-hidden="true" data-actual="">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="eventModalLabel">Chi tiết chuyến hàng</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <p id="eventDetails-${actualReceived.ActualReceivedId}"></p>
                                    <h5 class="modal-title" id="completionPercentage-${actualReceived.ActualReceivedId}"></h5>
                                    <table id="stagesTable-${actualReceived.ActualReceivedId}" class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Mã sản phẩm</th>
                                                <th>Số lượng</th>
                                                <th>Trạng thái</th>
                                            </tr>
                                        </thead>
                                        <tbody id="stagesTableBody-${actualReceived.ActualReceivedId}">
                                        </tbody>
                                    </table>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-warning" id="delayButton-${actualReceived.ActualReceivedId}">Delay</button>
                                </div>
                            </div>
                        </div>
                    </div>`;
                modalsContainer.append(modalHtml);
            });
        });
    }

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

    //Hàm để lấy nhà cung cấp cho hôm nay
    function loadSuppliersForToday() {
        fetch('/TLIPWarehouse/GetSuppliersForToday')
            .then(response => response.json())
            .then(data => {
                //console.log("Supplier: ", data);
                var suppliersHTML = '';
                data.forEach(supplier => {
                    suppliersHTML += `<div class="supplier-block">${supplier.supplierName}</div>`;
                });
                document.getElementById('suppliersListContent').innerHTML = suppliersHTML;
            });
    }

    function getActualReceived() {
        return fetch('/TLIPWarehouse/GetActualReceived')
            .then(response => response.json());
    }
    function getPlanDetailReceived() {
        return fetch('/TLIPWarehouse/GetCurrentPlanDetailsWithDates')
            .then(response => response.json());
    }

    function getAllActualReceivedLast7Days() {
        return fetch('/TLIPWarehouse/GetAllActualReceivedLast7Days')
            .then(response => response.json());
    }
    function getIncompleteActualReceived() {
        return fetch('/TLIPWarehouse/GetActualReceived')
            .then(response => response.json())
            .then(data => {
                return data.filter(actualReceived => {
                    const completionPercentage = calculateCompletionPercentage(actualReceived.ActualDetails);
                    return completionPercentage < 100;
                });
            });
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
    function calculateCompletionPercentage(actualDetails) {
        const completedItems = actualDetails.filter(detail => detail.QuantityRemain === 0).length;
        const totalItems = actualDetails.length;
        return (completedItems / totalItems) * 100;
    }

    function fetchEvents(fetchInfo, successCallback, failureCallback) {
        Promise.all([getActualReceived(), getPlanDetailReceived()])
            .then(([actualReceivedData, planDetailData]) => {
                const events = [];

                // Process actual received data
                actualReceivedData.forEach(actualReceived => {
                    const start = new Date(actualReceived.ActualDeliveryTime);
                    let end = new Date(start);

                    const completionPercentage = calculateCompletionPercentage(actualReceived.ActualDetails);
                    let eventColor = completionPercentage === 100 ? '#3E7D3E' : '#C7B44F';

                    if (!actualReceived.IsCompleted) {
                        if (completionPercentage === 0) {
                            end.setHours(end.getHours() + 1);
                        } else if (completionPercentage < 100) {
                            end = new Date();
                        }
                    } else {
                        if (actualReceived.ActualLeadTime) {
                            const [hours, minutes, seconds] = actualReceived.ActualLeadTime.split(':').map(Number);
                            end.setHours(end.getHours() + hours);
                            end.setMinutes(end.getMinutes() + minutes);
                            end.setSeconds(end.getSeconds() + seconds);
                        } else {
                            end.setHours(end.getHours() + 1);
                        }
                    }


                    const formattedStart = formatDateTime(start);
                    const formattedEnd = formatDateTime(end);
                    const modalId = `eventModal-${actualReceived.ActualReceivedId}`;

                    events.push({
                        id: `actual-${actualReceived.ActualReceivedId}`,
                        title: actualReceived.SupplierName,
                        start: formattedStart,
                        end: formattedEnd,
                        backgroundColor: eventColor,
                        resourceId: `${actualReceived.SupplierCode}_Actual`,
                        extendedProps: {
                            actualReceivedId: actualReceived.ActualReceivedId,
                            supplierCode: actualReceived.SupplierCode,
                            completionPercentage: completionPercentage,
                            modalId: modalId
                        }
                    });
                });

                // Process plan detail data
                planDetailData.forEach(planDetail => {
                    const specificDate = new Date(planDetail.SpecificDate);
                    const deliveryTimeParts = planDetail.DeliveryTime.split(':');
                    const leadTimeParts = planDetail.LeadTime.split(':');

                    const start = new Date(specificDate);
                    start.setHours(parseInt(deliveryTimeParts[0], 10));
                    start.setMinutes(parseInt(deliveryTimeParts[1], 10));
                    start.setSeconds(parseInt(deliveryTimeParts[2], 10));

                    const end = new Date(start);
                    end.setHours(end.getHours() + parseInt(leadTimeParts[0], 10));
                    end.setMinutes(end.getMinutes() + parseInt(leadTimeParts[1], 10));
                    end.setSeconds(end.getSeconds() + parseInt(leadTimeParts[2], 10));

                    const formattedStart = formatDateTime(start);
                    const formattedEnd = formatDateTime(end);
                    const modalId = `planModal-${planDetail.PlanDetailId}`;

                    events.push({
                        id: `plan-${planDetail.PlanDetailId}`,
                        title: planDetail.SupplierName,
                        start: formattedStart,
                        end: formattedEnd,
                        resourceId: `${planDetail.SupplierCode}_Plan`,
                        extendedProps: {
                            planDetailId: planDetail.PlanDetailId,
                            supplierCode: planDetail.SupplierCode,
                            planType: planDetail.PlanType,
                            modalId: modalId
                        }
                    });
                });

                successCallback(events);
            })
            .catch(error => failureCallback(error));
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
        //resources: '/api/ResourcesReceivedTLIP',
        resources: function (fetchInfo, successCallback, failureCallback) {
            getCurrentDate(fetchInfo.startStr).then(weekdayId => {
                fetchResources(weekdayId)
                    .then(resources => successCallback(resources))
                    .catch(error => failureCallback(error));
            });
        },
        datesSet: function (dateInfo) {
            console.log('datesSet:', dateInfo.startStr);
            getCurrentDate(dateInfo.startStr).then(weekdayId => {
                fetchResources(weekdayId)
                    .then(resources => {
                        if (typeof calendar.refetchResources === 'function') {
                            calendar.refetchResources();
                        } else {
                            console.error('calendar.refetchResources is not defined or not a function');
                        }
                    })
                    .catch(error => {
                        console.error("Error fetching resources:", error);
                        if (typeof failureCallback === 'function') {
                            failureCallback(error);
                        } else {
                            console.error('failureCallback is not defined or not a function');
                        }
                    });
            }).catch(error => {
                console.error("Error getting current date:", error);
                if (typeof failureCallback === 'function') {
                    failureCallback(error);
                } else {
                    console.error('failureCallback is not defined or not a function');
                }
            });
        },



        //Sắp xếp theo thứ tự theo order(Plan trước Actual sau)
        resourceOrder: 'order',
        events: fetchEvents,

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
            const planDetailReceiveId = info.event.extendedProps.planDetailId;

            const supplierCode = info.event.extendedProps.supplierCode;
            const supplierName = info.event.title;
            const start = info.event.start;
            const end = info.event.end;

            const formattedStart = new Date(start).toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit'
            });

            const formattedEnd = new Date(end).toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                second: '2-digit',
                minute: '2-digit'
            });

            const eventDetails = `
                <strong>Nhà cung cấp:</strong> ${supplierName} - ${supplierCode}<br>
                <i><strong>Nhận lúc:</strong></i> ${formattedStart}<br>
                <i><strong>Kết thúc lúc:</strong></i> ${formattedEnd}
            `;
            const eventModalElement = document.getElementById(`eventModal-${actualReceivedId}`);
            const planModalElement = document.getElementById(`planModal-${planDetailReceiveId}`);

            if (eventModalElement) {
                fetchAndPopulateStagesTable(actualReceivedId);
                const eventDetailsElement = document.getElementById(`eventDetails-${actualReceivedId}`);
                if (eventDetailsElement) {
                    eventDetailsElement.innerHTML = eventDetails;
                } else {
                    console.error(`Element with ID eventDetails-${actualReceivedId} not found.`);
                }
                var eventModal = new bootstrap.Modal(eventModalElement);
                eventModal.show();
            } else {
                console.error(`Element with id 'eventModal-${actualReceivedId}' not found.`);
            }

            if (planModalElement) {
                var eventModal = new bootstrap.Modal(planModalElement);
                eventModal.show();
            } else {
                console.error(`Element with id 'planModal-${planDetailReceiveId}' not found.`);
            }

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
            if (arg.resource.title.endsWith("ACTUAL")) {
                return ['gray-background'];
            }
            return ['white-background'];
        },
        resourceLabelClassNames: function (arg) {
            if (arg.resource.title.endsWith("ACTUAL")) {
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
        //return fetch(`/TLIPWarehouse/GetAsnDetail?asnNumber=${asnNumber}&doNumber=${doNumber}&invoice=${invoice}`)
        return fetch(`/TLIPWarehouse/ParseAsnDetailFromFile?asnNumber=${asnNumber}&doNumber=${doNumber}&invoice=${invoice}`)
            .then(response => response.json());
    }
    function getActualReceivedEntry(supplierCode, actualDeliveryTime, ansNumber) {
        return fetch(`/TLIPWarehouse/GetActualReceivedEntry?supplierCode=${supplierCode}&actualDeliveryTime=${actualDeliveryTime}&ansNumber=${ansNumber}`)
            .then(response => response.json());
    }

    var previousData = [];
    async function fetchData() {
        console.log('fetchData called at', new Date().toLocaleTimeString());

        try {
            const response = await fetch('/TLIPWarehouse/ParseAsnInformationFromFile');
            //const response = await fetch('/TLIPWarehouse/GetAsnInformation');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const text = await response.text();
            if (!text) {
                throw new Error('Empty response');
            }
            const nextData = JSON.parse(text);
            //console.log('Data fetched:', nextData);
            var now = new Date();
            nextData.forEach(async (nextItem) => {
                var previousItem = previousData.find(item =>
                    (item.AsnNumber && item.AsnNumber === nextItem.AsnNumber) ||
                    (!item.AsnNumber && item.DoNumber && item.DoNumber === nextItem.DoNumber) ||
                    (!item.AsnNumber && !item.DoNumber && item.Invoice && item.Invoice === nextItem.Invoice)
                );
                //console.log('previousItem:', previousItem);

                if (previousItem && !previousItem.ReceiveStatus && nextItem.ReceiveStatus) {
                    var actualReceived = {
                        ActualDeliveryTime: formatDateTime(now),
                        SupplierCode: nextItem.SupplierCode,
                        AsnNumber: nextItem.AsnNumber,
                        DoNumber: nextItem.DoNumber,
                        Invoice: nextItem.Invoice,
                        IsCompleted: nextItem.IsCompleted
                    };
                    //console.log('Posting data:', actualReceived);
                    //console.log('ADD DATA ĐÂYYYYYYYY');

                    await fetch('/TLIPWarehouse/AddActualReceived', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(actualReceived)
                    });

                    const actualReceivedEntryResponse = await fetch(`/TLIPWarehouse/GetActualReceivedEntry?supplierCode=${actualReceived.SupplierCode}&actualDeliveryTime=${actualReceived.ActualDeliveryTime}&asnNumber=${actualReceived.AsnNumber}`);
                    const actualReceivedEntry = await actualReceivedEntryResponse.json();
                    //console.log('Retrieved actualReceivedEntry:', actualReceivedEntry);

                    const asnDetails = await getAsnDetail(
                        actualReceivedEntry.AsnNumber ? actualReceivedEntry.AsnNumber : '',
                        actualReceivedEntry.DoNumber ? actualReceivedEntry.DoNumber : '',
                        actualReceivedEntry.Invoice ? actualReceivedEntry.Invoice : ''
                    );
                    //console.log('ASN Details trong fetchData:', asnDetails);

                    const addActualDetailPromises = asnDetails.map(asnDetail => {
                        const actualDetail = {
                            ActualReceivedId: actualReceivedEntry.ActualReceivedId,
                            PartNo: asnDetail.PartNo,
                            Quantity: asnDetail.Quantity,
                            QuantityRemain: asnDetail.QuantityRemain
                        };
                        //console.log('ADD ASNDETAIL ĐÂYYYYYYYY');

                        return fetch('/TLIPWarehouse/AddActualDetail', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(actualDetail)
                        }).then(response => {
                            if (!response.ok) {
                                console.error('Failed to post ActualDetailTLIP:', response.statusText);
                            }
                        }).catch(err => {
                            console.error('Error posting ActualDetailTLIP:', err.toString());
                        });
                    });

                    await Promise.all(addActualDetailPromises);

                    const updatedActualReceivedEntryResponse = await fetch(`/TLIPWarehouse/GetActualReceivedById?actualReceivedId=${actualReceivedEntry.ActualReceivedId}`);
                    if (!updatedActualReceivedEntryResponse.ok) {
                        throw new Error('Network response was not ok ' + updatedActualReceivedEntryResponse.statusText);
                    }
                    const updatedActualReceivedEntry = await updatedActualReceivedEntryResponse.json();
                    if (!updatedActualReceivedEntry) {
                        console.error('No data returned from API');
                    } else {
                        console.log('Data received from API:', updatedActualReceivedEntry);
                    }
                }

                if (previousItem && !previousItem.IsCompleted && nextItem.IsCompleted) {
                    try {
                        const response = await fetch('/TLIPWarehouse/GetActualReceivedByDetails', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({
                                SupplierCode: previousItem.SupplierCode,
                                AsnNumber: previousItem.AsnNumber,
                                DoNumber: previousItem.DoNumber,
                                Invoice: previousItem.Invoice
                            })
                        });

                        if (!response.ok) {
                            throw new Error('Failed to fetch actual received item');
                        }

                        const actualReceived = await response.json();

                        /* // Update all ActualDetails' QuantityRemain to 0
                         const response1 = await fetch(`/TLIPWarehouse/UpdateActualDetailsQuantityRemain?actualReceivedId=${actualReceived.ActualReceivedId}&quantityRemain=0`, {
                             method: 'POST'
                         });
                         if (response1.ok) {
                             console.log("Updated ActualDetails' QuantityRemain to 0 successfully.");
                         } else {
                             console.error("Failed to update ActualDetails' QuantityRemain to 0:", response1.statusText);
                         }*/

                        // Update IsCompleted to true
                        const response2 = await fetch(`/TLIPWarehouse/UpdateActualReceivedCompletion?actualReceivedId=${actualReceived.ActualReceivedId}&isCompleted=true`, {
                            method: 'POST'
                        });
                        if (response2.ok) {
                            console.log("Updated ActualReceived IsCompleted to true successfully.");
                        } else {
                            console.error("Failed to update ActualReceived IsCompleted to true:", response2.statusText);
                        }

                    } catch (error) {
                        console.error("Failed to update ActualReceived and ActualDetails: ", error);
                    }
                }

            });
            previousData = nextData;
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }


    setInterval(fetchData, 5000);


    function fetchDataDetail() {
        getIncompleteActualReceived().then(data => {
            //console.log("Data actual nè: ", data);
            data.forEach(actualReceived => {
                console.log('Actual Received Chưa xong:', actualReceived);
                getAsnDetail(
                    actualReceived.AsnNumber ? actualReceived.AsnNumber : '',
                    actualReceived.DoNumber ? actualReceived.DoNumber : '',
                    actualReceived.Invoice ? actualReceived.Invoice : ''
                    //actualReceived.AsnNumber ? actualReceived.AsnNumber : null,
                    //actualReceived.DoNumber ? actualReceived.DoNumber : null,
                    //actualReceived.Invoice ? actualReceived.Invoice : null
                ).then(asnDetails => {
                    //console.log('ASN Details trong fetchDataDetail:', asnDetails);
                    asnDetails.forEach(asnDetail => {

                        if (asnDetail.QuantityRemain === 0) {
                            const url = `/TLIPWarehouse/UpdateActualDetailTLIP?partNo=${asnDetail.PartNo}&actualReceivedId=${actualReceived.ActualReceivedId}&quantityRemain=0`;
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

                });
            });
        }).catch(error => {
            console.error('Error fetching data:', error);
        });
    }


    // Start polling every 5 seconds
    setInterval(fetchDataDetail, 5000);


    function populateStagesTable(asnDetails, actualReceivedId) {
        const stagesTableBodyId = `stagesTableBody-${actualReceivedId}`;
        const stagesTableBody = document.getElementById(stagesTableBodyId);
        let totalItems = asnDetails.length;
        let completedItems = 0;

        if (stagesTableBody) {
            stagesTableBody.innerHTML = '';
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
        }


        // Tính toán phần trăm hoàn thành
        let completionPercentage = (completedItems / totalItems) * 100;

        // Cập nhật giao diện để hiển thị phần trăm hoàn thành
        const completionElement = document.getElementById(`completionPercentage-${actualReceivedId}`);
        let color = completionPercentage < 100 ? '#C7B44F' : '#3E7D3E';

        if (completionElement) {
            completionElement.textContent = `Đã hoàn thành được: ${completionPercentage.toFixed(2)}%`;
            completionElement.style.color = color; // Cập nhật màu sắc
        } else {
            // Nếu phần tử hiển thị chưa tồn tại, tạo mới
            const newCompletionElement = document.createElement('div');
            newCompletionElement.id = `completionPercentage-${actualReceivedId}`;
            newCompletionElement.textContent = `Đã hoàn thành được: ${completionPercentage.toFixed(2)}%`;
            newCompletionElement.style.color = color;
            document.body.appendChild(newCompletionElement);
        }
    }



    function fetchAndPopulateStagesTable(actualReceivedId) {
        fetch(`/TLIPWarehouse/GetActualDetailsByReceivedId?actualReceivedId=${actualReceivedId}`)
            .then(response => response.json())
            .then(asnDetails => {
                //console.log('Actual details:', data);
                populateStagesTable(asnDetails, actualReceivedId);
            })
            .catch(error => console.error('Error fetching actual details:', error));

    }


    function updateCalendarEvent(actualReceived) {
        const start = new Date(actualReceived.ActualDeliveryTime);
        const end = new Date(start);

        if (actualReceived.ActualLeadTime) {
            //console.log("ActualLeadTime:", actualReceived.ActualLeadTime);
            const leadTimeParts = actualReceived.ActualLeadTime.split(':');
            const hours = parseInt(leadTimeParts[0], 10);
            const minutes = parseInt(leadTimeParts[1], 10);
            const seconds = parseInt(leadTimeParts[2], 10);

            end.setHours(end.getHours() + hours);
            end.setMinutes(end.getMinutes() + minutes);
            end.setSeconds(end.getSeconds() + seconds);

        } else {
            end.setHours(end.getHours() + 1);
        }
        //console.log("end 2:", end);


        const formattedStart = formatDateTime(start);
        const formattedEnd = formatDateTime(end);
        //console.log("formattedEnd:", formattedEnd);

        let existingEvent = calendar.getEventById(`actual-${actualReceived.ActualReceivedId}`);

        const modalId = `eventModal-${actualReceived.ActualReceivedId}`;
        const completionPercentage = calculateCompletionPercentage(actualReceived.ActualDetails);
        if (!actualReceived.IsCompleted) {
            if (completionPercentage < 100) {
                if (existingEvent) {
                    existingEvent.setEnd(formattedEnd);
                    //console.log("Event updated with new end:", formattedEnd);
                } else {
                    calendar.addEvent({
                        id: `actual-${actualReceived.ActualReceivedId}`,
                        title: actualReceived.SupplierName,
                        start: formattedStart,
                        end: formattedEnd,
                        resourceId: `${actualReceived.SupplierCode}_Actual`,
                        extendedProps: {
                            actualReceivedId: actualReceived.ActualReceivedId,
                            supplierCode: actualReceived.SupplierCode,
                            modalId: modalId
                        }
                    });
                    //console.log("Event added with start:", formattedStart, "and end:", formattedEnd);
                }
            }
        }
        let event = calendar.getEventById(`actual-${actualReceived.ActualReceivedId}`);
        if (event) {
            const completionPercentage = calculateCompletionPercentage(actualReceived.ActualDetails);
            if (completionPercentage === 100) {
                event.setProp('backgroundColor', '#3E7D3E');
            }
        }

        updateEventDetails(actualReceived);
    }
    function updateEventDetails(actualReceived) {
        const start = new Date(actualReceived.ActualDeliveryTime);
        const end = new Date(start);
        if (actualReceived.ActualLeadTime) {
            const leadTimeParts = actualReceived.ActualLeadTime.split(':');
            const hours = parseInt(leadTimeParts[0], 10);
            const minutes = parseInt(leadTimeParts[1], 10);
            const seconds = parseInt(leadTimeParts[2], 10);

            end.setHours(end.getHours() + hours);
            end.setMinutes(end.getMinutes() + minutes);
            end.setSeconds(end.getSeconds() + seconds);

        } else {
            end.setHours(end.getHours() + 1);
        }
        const formattedStart = formatDateTime(start);
        const formattedEnd = formatDateTime(end);

        const actualReceivedId = actualReceived.ActualReceivedId;
        const supplierCode = actualReceived.SupplierCode;
        const supplierName = actualReceived.SupplierName;

        const formattedStartEvent = new Date(formattedStart).toLocaleString('vi-VN', {
            timeZone: 'Asia/Bangkok',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });

        const formattedEndEvent = new Date(formattedEnd).toLocaleString('vi-VN', {
            timeZone: 'Asia/Bangkok',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });

        const eventDetails = `
            <strong>Nhà cung cấp:</strong> ${supplierName} - ${supplierCode}<br>
            <i><strong>Nhận lúc:</strong></i> ${formattedStartEvent}<br>
            <i><strong>Kết thúc lúc:</strong></i> ${formattedEndEvent}
        `;

        if (calculateCompletionPercentage(actualReceived.ActualDetails) < 100) {
            const eventDetailsElement = document.getElementById(`eventDetails-${actualReceivedId}`);
            if (eventDetailsElement) {
                eventDetailsElement.innerHTML = eventDetails;
            } else {
                console.error(`Element with ID eventDetails-${actualReceivedId} not found.`);
            }
        }
    }

    setInterval(function () {
        getIncompleteActualReceived().then(data => {
            data.forEach(actualReceived => {
                //console.log("actualReceived skrtttt:", actualReceived);
                if (!actualReceived.IsCompleted) {
                    if (actualReceived.CompletionPercentage < 100) {
                        const formattedEnd = formatDateTime(new Date());
                        updateEventEndInDatabase(actualReceived.ActualReceivedId, formattedEnd);
                        updateCalendarEvent(actualReceived);
                        //console.log("actualReceived: Update tiếp nè");
                    }
                }
            });
        }).catch(error => {
            console.error("Error fetching actual received data:", error);
        });

        getAllActualReceivedLast7Days().then(data => {
            data.forEach(actualReceived => {
                if (actualReceived.CompletionPercentage === 100) {
                    let event = calendar.getEventById(`actual-${actualReceived.ActualReceivedId}`);
                    event.setProp('backgroundColor', '#3E7D3E');
                }
            });
        }).catch(error => {
            console.error("Error fetching actual received data:", error);
        });
    }, 5000);


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

    /* function fetchResources(date) {
         fetch(`/api/ResourcesReceivedTLIP?date=${date}`)
             .then(response => response.json())
             .then(resources => {
                 calendar.refetchResources(resources);
             });
     }*/

    //function getCurrentDate(dateNow) {
    //    console.log("dateNow: ", dateNow);
    //    fetch('/api/ResourcesReceivedTLIP/GetWeekday', {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json'
    //        },
    //        body: JSON.stringify({ date: dateNow })
    //    })
    //        .then(response => response.json())
    //        .then(data => {
    //            console.log("Weekday: ", data.WeekdayId);
    //            fetchResources(data.WeekdayId);
    //        })
    //        .catch(error => {
    //            console.error("Error fetching weekday: ", error);
    //        });
    //}
    //function fetchResources(weekdayId) {
    //    fetch(`/api/ResourcesReceivedTLIP?weekdayId=${weekdayId}`)
    //        .then(response => response.json())
    //        .then(data => {
    //            console.log("Fetched resources:", data); // Log the fetched data
    //            calendar.refetchResources(data);
    //            console.log("Resources refetched"); // Log after refetching resources

    //        })
    //        .catch(error => {
    //            console.error("Error fetching resources: ", error);
    //        });
    //}
    function getCurrentDate(dateNow) {

        const today = new Date().toISOString().split('T')[0];
        const dateToUse = dateNow ? dateNow : today;
        console.log("dateToUse: ", dateToUse);
        return fetch('/api/ResourcesReceivedTLIP/GetWeekday', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Date: dateToUse })
        })
            .then(response => response.json())
            .then(data => data.WeekdayId);
    }


    /*  function getWeekdayId(date) {
          return fetch('/api/ResourcesReceivedTLIP/GetWeekday', {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify({ Date: date })
          })
              .then(response => response.json())
              .then(data => data.WeekdayId);
      }*/

    // Function to get resources based on weekday ID
    function fetchResources(weekdayId) {
        return fetch(`/api/ResourcesReceivedTLIP?weekdayId=${weekdayId}`)
            .then(response => response.json());
    }


    // Gọi hàm getCurrentDate khi DOMContentLoaded
    //getCurrentDate();



    //Tải nhà cung cấp lên khi tải trang 
    loadSuppliersForToday();

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
