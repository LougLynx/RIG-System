document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    // Lấy thời gian thực cho Indicator
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

    var now = getFormattedNow();

    // Tạo sự kiện dựa trên dữ liệu được truyền từ Razor View
    function generateDailyEvents(planDetails) {
        const events = [];

        console.log("Plan Details Received:", planDetails);

        planDetails.forEach(detail => {
            // Log detail for each PlanDetail
            console.log("Processing Plan Detail:", detail);

            // Tạo sự kiện cho các kế hoạch (PlanDetails)
            const date = new Date();
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const dateString = `${year}-${month}-${day}`;
            const start = `${dateString}T${detail.PlanTime}`;

            // Calculate the end time manually without using the Date object to avoid timezone issues
            const [hours, minutes] = detail.PlanTime.split(':').map(Number); // Extract hours and minutes from PlanTime 
            const endHours = hours + 1; // Add 1 hour for the event duration

            // Construct end time in the same format as start time, adjusting only hours (and wrap around if necessary)
            const end = `${dateString}T${String(endHours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:00`;

            console.log(`Generated Plan Event - ID: plan-${detail.PlanDetailId}, Start: ${start}, End: ${end}`);

            events.push({
                id: `plan-${detail.PlanDetailId}`,
                title: `Plan ${detail.PlanDetailName}`,
                start: start,
                end: end,
                resourceId: `${detail.PlanDetailId * 2 - 1}`,
                extendedProps: {
                    hasActual: detail.Actuals && detail.Actuals.length > 0,
                    planDetailId: detail.PlanDetailId
                }
            });

            if (detail.Actuals && detail.Actuals.length > 0) {
                console.log(`Actuals Found for Plan Detail ID: ${detail.PlanDetailId}`);

                // Lấy chỉ Actual đầu tiên
                const actual = detail.Actuals[0];
                console.log("Processing First Actual:", actual);

                const actualDate = new Date(actual.ActualTime);

                const actualYear = actualDate.getFullYear();
                const actualMonth = String(actualDate.getMonth() + 1).padStart(2, '0');
                const actualDay = String(actualDate.getDate()).padStart(2, '0');
                const actualDateString = `${actualYear}-${actualMonth}-${actualDay}`;

                const actualTimeString = actualDate.toTimeString().split(' ')[0];
                const actualStartTime = `${actualDateString}T${actualTimeString}`;

                console.log("Actual Time Start:", actualStartTime);

                const actualStartDate = new Date(actualStartTime);
                const actualEndDate = new Date(actualStartDate.getTime() + 60 * 60000);
                const actualEndTime = `${actualEndDate.getFullYear()}-${String(actualEndDate.getMonth() + 1).padStart(2, '0')}-${String(actualEndDate.getDate()).padStart(2, '0')}T${actualEndDate.toTimeString().split(' ')[0]}`;

                console.log("Actual Time End:", actualEndTime);

                events.push({
                    id: `actual-${actual.ActualId}`,
                    title: `Actual ${detail.PlanDetailName}`,
                    start: actualStartTime,
                    end: actualEndTime,
                    resourceId: `${detail.PlanDetailId * 2}`,
                    extendedProps: { hasActual: true }
                });
            } else {
                console.log(`No Actuals Found for Plan Detail ID: ${detail.PlanDetailId}`);
            }
        });

        console.log("All Events Generated:", events);
        return events;
    }

    function confirmActual(planDetailId) {
        var now = new Date();
        var formattedNow = now.getFullYear() + '-'
            + String(now.getMonth() + 1).padStart(2, '0') + '-'
            + String(now.getDate()).padStart(2, '0') + 'T'
            + String(now.getHours()).padStart(2, '0') + ':'
            + String(now.getMinutes()).padStart(2, '0') + ':'
            + String(now.getSeconds()).padStart(2, '0');

        fetch('/DensoWarehouse/AddActual', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                PlanDetailId: planDetailId,
                ActualTime: formattedNow
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log('Response from server:', data);

                    // Thêm actual vào lịch mà không cần tải lại trang
                    const actualEvent = {
                        id: `actual-${planDetailId}`,
                        title: `Actual for Plan ${planDetailId}`,
                        start: formattedNow,
                        end: new Date(new Date(formattedNow).getTime() + 60 * 60000),
                        resourceId: `${planDetailId * 2}`,
                        extendedProps: { hasActual: true }
                    };

                    // Thêm sự kiện actual vào lịch
                    calendar.addEvent(actualEvent);

                    // Ẩn nút Confirm sau khi xác nhận
                    var confirmButton = document.getElementById('confirmButton');
                    if (confirmButton) {
                        confirmButton.style.display = 'none'; // Ẩn nút Confirm
                    }

                    // Cập nhật extendedProps của sự kiện hiện tại để đánh dấu đã có Actual
                    var event = calendar.getEventById(`plan-${planDetailId}`);
                    if (event) {
                        event.setExtendedProp('hasActual', true); // Cập nhật prop để xác nhận đã có Actual
                    }

                    // Đóng modal chi tiết hiện tại
                    var detailModal = document.getElementById('eventModal');
                    var bootstrapModal = bootstrap.Modal.getInstance(detailModal); // Lấy instance của modal hiện tại
                    bootstrapModal.hide(); // Đóng modal chi tiết

                    // Hiển thị modal thành công với tích xanh
                    var successModal = new bootstrap.Modal(document.getElementById('successModal'));
                    successModal.show();
                } else {
                    console.log('Error from server:', data);
                    alert('Error confirming actual.');
                }
            })
            .catch(error => console.error('Error:', error));
    }

    let deleteActualId = null; // Biến lưu trữ ID của Actual cần xóa

    function confirmDelete(actualId) {
        // Lưu lại Actual ID để xóa sau khi người dùng xác nhận
        deleteActualId = actualId;

        // Hiển thị modal xác nhận xóa
        var deleteModalElement = document.getElementById('deleteConfirmModal');
        var deleteModal = new bootstrap.Modal(deleteModalElement);
        deleteModal.show();
    }

    // Khi người dùng nhấn nút "Xóa" trong modal xác nhận
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (deleteActualId) {
            fetch(`/DensoWarehouse/DeleteActual/${deleteActualId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        console.log('Actual deleted successfully.');

                        // Xóa sự kiện khỏi lịch
                        calendar.getEventById(`actual-${deleteActualId}`).remove();

                        // Ẩn modal xác nhận xóa
                        var deleteModalElement = document.getElementById('deleteConfirmModal');
                        var deleteModal = bootstrap.Modal.getInstance(deleteModalElement);
                        deleteModal.hide();

                        // Ẩn modal chi tiết sự kiện (eventModal)
                        var eventModalElement = document.getElementById('eventModal');
                        var eventModal = bootstrap.Modal.getInstance(eventModalElement);
                        eventModal.hide();

                    } else {
                        alert('Error deleting actual.');
                    }
                })
                .catch(error => console.error('Error:', error));
        }
    });




    var calendar = new FullCalendar.Calendar(calendarEl, {
        schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
        initialView: 'resourceTimelineDay',
        slotDuration: '01:00',
        slotLabelInterval: '00:30',
        slotLabelFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        },
        slotMinTime: '00:00:00',
        slotMaxTime: '24:00:00',
        stickyFooterScrollbar: 'auto',
        resourceAreaWidth: '110px',
        height: 1100,
        nowIndicator: true,
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
        resources: '/api/Resources',
        resourceOrder: 'order',
        events: generateDailyEvents(planDetails),
        eventClick: function (info) {
            var start = new Date(info.event.start);
            var end = new Date(info.event.end);

            var formattedStart = start.toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit'
            });

            document.getElementById('eventDetails').innerHTML = `${info.event.title}<br><i><strong>Nhận lúc: ${formattedStart}</strong></i>`;

            var confirmButton = document.getElementById('confirmButton');
            var deleteButton = document.getElementById('deleteButton');

            if (confirmButton) {
                if (!info.event.extendedProps.hasActual) {
                    confirmButton.style.display = 'block';
                    confirmButton.onclick = function () {
                        console.log("Plan Detail ID:", info.event.extendedProps.planDetailId );
                         confirmActual(info.event.extendedProps.planDetailId);
                    };
                } else {
                    confirmButton.style.display = 'none';
                }
            }

            if (info.event.id.startsWith('actual')) {
                deleteButton.style.display = 'block';
                deleteButton.onclick = function () {
                    confirmDelete(info.event.id.split('-')[1]);
                };
            } else {
                deleteButton.style.display = 'none';
            }
             
            var modalElement = document.getElementById('eventModal');
            if (modalElement) {
                var myModal = new bootstrap.Modal(modalElement);
                myModal.show();
            }
        },
        views: {
            resourceTimelineDay: {
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' }
            },
            resourceTimelineWeek: {
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' }
            }
        },
        resourceLaneClassNames: function (arg) {
            if (arg.resource.title === "Actual") {
                return ['gray-background'];
            }
            return [];
        },
        resourceLabelClassNames: function (arg) {
            if (arg.resource.title === "Actual") {
                return ['gray-background'];
            }
            return [];
        },
        eventContent: function (arg) {
            let content = document.createElement('div');
            content.classList.add('centered-event');
            content.innerHTML = arg.event.title;
            return { domNodes: [content] };
        },
        slotLabelContent: function (arg) {
            return { html: `<i style="color: blue;">${arg.text}</i>` };
        },
        slotLabelClassNames: function (arg) {
            return ['custom-slot-label'];
        }
    });

    calendar.render();
});

// HIỂN THỊ THỜI GIAN THỰC
document.addEventListener('DOMContentLoaded', function () {
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
});
