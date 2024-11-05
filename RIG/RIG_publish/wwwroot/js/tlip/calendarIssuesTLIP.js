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
    function generateDailyEvents(planDetails, currentPlanEffectiveDate, nextPlanEffectiveDate) {
        const events = [];
        const today = new Date();
        const todayString = today.toISOString().split('T')[0]; // Ngày hôm nay ở định dạng YYYY-MM-DD
        const currentPlanStartDate = new Date(currentPlanEffectiveDate);
        const nextPlanStartDate = nextPlanEffectiveDate ? new Date(nextPlanEffectiveDate) : null;

        planDetails.forEach(detail => {
            // Hiển thị các sự kiện dựa trên ActualTime cho các PlanDetail trong quá khứ
            if (detail.Actuals && detail.Actuals.length > 0) {
                detail.Actuals.forEach(actual => {
                    const actualDate = new Date(actual.ActualTime);
                    const actualYear = actualDate.getFullYear();
                    const actualMonth = String(actualDate.getMonth() + 1).padStart(2, '0');
                    const actualDay = String(actualDate.getDate()).padStart(2, '0');
                    const actualDateString = `${actualYear}-${actualMonth}-${actualDay}`;

                    // Kiểm tra xem ngày của Actual có trùng với hôm nay hay không
                    if (actualDateString !== todayString) {
                        // Hiển thị sự kiện PlanDetail dựa trên ActualTime nếu không phải ngày hôm nay
                        const planStartTime = `${actualDateString}T${detail.PlanTime}`;
                        const [hours, minutes] = detail.PlanTime.split(':').map(Number);
                        const planEndTime = `${actualDateString}T${String(hours + 1).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:00`;

                        events.push({
                            id: `plan-${detail.PlanDetailId}-actual-${actualDateString}`,
                            title: `Plan ${detail.PlanDetailName}`,
                            start: planStartTime,
                            end: planEndTime,
                            resourceId: `${detail.PlanDetailId * 2 - 1}`,
                            extendedProps: {
                                hasActual: true,
                                planDetailId: detail.PlanDetailId
                            }
                        });
                    }

                    // Hiển thị sự kiện Actual dựa trên ActualTime
                    const actualStartTime = `${actualDateString}T${actualDate.toTimeString().split(' ')[0]}`;
                    const actualEndDate = new Date(actualDate.getTime() + 60 * 60000); // Thêm 1 giờ cho thời gian kết thúc
                    const actualEndTime = `${actualEndDate.getFullYear()}-${String(actualEndDate.getMonth() + 1).padStart(2, '0')}-${String(actualEndDate.getDate()).padStart(2, '0')}T${actualEndDate.toTimeString().split(' ')[0]}`;

                    events.push({
                        id: `actual-${actual.ActualId}`,
                        title: `Actual ${detail.PlanDetailName}`,
                        start: actualStartTime,
                        end: actualEndTime,
                        resourceId: `${detail.PlanDetailId * 2}`,
                        extendedProps: { hasActual: true }
                    });
                });
            }

            // Hiển thị các PlanDetail từ hiện tại đến trước ngày của kế hoạch tiếp theo
            if (!nextPlanStartDate || today < nextPlanStartDate) {
                let startDate = new Date(Math.max(today.getTime(), currentPlanStartDate.getTime()));
                const endDate = nextPlanStartDate || new Date(today.getFullYear() + 1, today.getMonth(), today.getDate());

                while (startDate < endDate) {
                    const year = startDate.getFullYear();
                    const month = String(startDate.getMonth() + 1).padStart(2, '0');
                    const day = String(startDate.getDate()).padStart(2, '0');
                    const dateString = `${year}-${month}-${day}`;
                    const start = `${dateString}T${detail.PlanTime}`;

                    const [hours, minutes] = detail.PlanTime.split(':').map(Number);
                    const endHours = hours + 1;
                    const end = `${dateString}T${String(endHours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:00`;

                    events.push({
                        id: `plan-${detail.PlanDetailId}-${dateString}`,
                        title: `Plan ${detail.PlanDetailName}`,
                        start: start,
                        end: end,
                        resourceId: `${detail.PlanDetailId * 2 - 1}`,
                        extendedProps: {
                            hasActual: false,
                            planDetailId: detail.PlanDetailId
                        }
                    });

                    // Tăng ngày để lặp lại sự kiện
                    startDate.setDate(startDate.getDate() + 1);
                }
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
                        end: new Date(new Date(formattedNow).getTime() + 60 * 60000).toISOString(),
                        resourceId: `${planDetailId * 2}`,
                        extendedProps: { hasActual: true }
                    };

                    // Thêm sự kiện actual vào lịch
                    calendar.addEvent(actualEvent);

                    // Cập nhật extendedProps của sự kiện PlanDetail để đánh dấu đã có Actual
                    const todayString = formattedNow.split('T')[0];
                    var planEvent = calendar.getEventById(`plan-${planDetailId}-${todayString}`);
                    if (planEvent) {
                        planEvent.setExtendedProp('hasActual', true); // Cập nhật prop để xác nhận đã có Actual
                        console.log('Updated PlanDetail with hasActual:', planEvent);
                    } else {
                        console.log('PlanDetail event not found for update.');
                    }

                    // Ẩn nút Confirm sau khi xác nhận
                    var confirmButton = document.getElementById('confirmButton');
                    if (confirmButton) {
                        confirmButton.style.display = 'none';
                    }

                    // Đóng modal chi tiết hiện tại
                    var detailModal = document.getElementById('eventModal');
                    var bootstrapModal = bootstrap.Modal.getInstance(detailModal);
                    bootstrapModal.hide();

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

                        // Reload lại trang
                        location.reload();
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
        events: generateDailyEvents(planDetails, currentPlanEffectiveDate, nextPlanEffectiveDate),
        eventClick: function (info) {
            // Lấy ngày hôm nay ở định dạng YYYY-MM-DD
            const today = new Date();
            const todayString = today.toISOString().split('T')[0];

            // Lấy ngày từ thời gian bắt đầu của sự kiện được click
            const eventDate = new Date(info.event.start).toISOString().split('T')[0];
            console.log("Infor Start:", eventDate);
            // Lấy các thông tin cần thiết từ sự kiện
            const hasActual = info.event.extendedProps.hasActual;
            console.log("Has Actual:", hasActual);
            const planDetailId = info.event.extendedProps.planDetailId;

            // Hiển thị chi tiết sự kiện trong modal
            document.getElementById('eventDetails').innerHTML = `${info.event.title}<br><i><strong>Nhận lúc: ${new Date(info.event.start).toLocaleString('vi-VN', { timeZone: 'UTC', hour: '2-digit', minute: '2-digit' })}</strong></i>`;

            // Điều chỉnh nút Confirm dựa trên điều kiện
            const confirmButton = document.getElementById('confirmButton');
            if (confirmButton) {
                // Chỉ hiển thị nút Confirm nếu sự kiện thuộc về ngày hôm nay và chưa có Actual
                if (eventDate === todayString && !hasActual) {
                    confirmButton.style.display = 'block';
                    confirmButton.onclick = function () {
                        confirmActual(planDetailId);
                    };
                } else {
                    confirmButton.style.display = 'none';
                }
            }

            // Điều chỉnh nút Delete nếu sự kiện là Actual
            const deleteButton = document.getElementById('deleteButton');
            if (deleteButton) {
                if (info.event.id.startsWith('actual')) {
                    deleteButton.style.display = 'block';
                    deleteButton.onclick = function () {
                        confirmDelete(info.event.id.split('-')[1]);
                    };
                } else {
                    deleteButton.style.display = 'none';
                }
            }

            // Hiển thị modal chi tiết sự kiện
            const modalElement = document.getElementById('eventModal');
            if (modalElement) {
                const myModal = new bootstrap.Modal(modalElement);
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
