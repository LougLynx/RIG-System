document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    // Log the planDetails to verify data
    //console.log('Plan Details:', planDetails);

    // PARSE SANG ĐỊNH ISO 8601
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

    console.log('actualDetails:', actualDetails);
        //tạo sự kiện cho 7 ngày
        function generateDailyEvents(planDetails, actualDetails) {
            const events = [];
            const titles = planDetails.map(detail => detail.PlanDetailName);
            const times = planDetails.map(detail => detail.PlanTime);
            const ids = planDetails.map(detail => detail.PlanDetailId);

            for (let i = 0; i < 7; i++) {
                const date = new Date();
                date.setDate(date.getDate() + i);
                const dateString = date.toISOString().split('T')[0];
                console.log('DateString Plan:', dateString);

                titles.forEach((title, index) => {
                    events.push({
                        id: `plan-${i}-${index}`,
                        title: 'Chuyến ' + title,
                        start: `${dateString}T${times[index]}`,
                        end: (() => {
                            const startDate = new Date(`${dateString}T${times[index]}`);
                            const endDate = new Date(startDate.getTime() + 60 * 60000);
                            return endDate.toISOString().split('T')[0] + 'T' + endDate.toTimeString().split(' ')[0];
                        })(),
                        resourceId: `${ids[index] * 2 - 1}`
                    });
                });
            }

            actualDetails.forEach(detail => {
                console.log('Detail:', detail); // Log the detail object

                const actualDate = new Date(detail.ActualTime);
                console.log('Actual Date:', actualDate); // Log the actual date

                const year = actualDate.getFullYear();
                const month = String(actualDate.getMonth() + 1).padStart(2, '0');
                const day = String(actualDate.getDate()).padStart(2, '0');
                const dateString = `${year}-${month}-${day}`;
                console.log('DateString:', dateString);

                const timeString = actualDate.toTimeString().split(' ')[0];
                const actualTime = `${dateString}T${timeString}`;
                console.log('Actual Time:', actualTime); // Log the formatted actual time

                const startDate = new Date(actualTime);
                const endDate = new Date(startDate.getTime() + 60 * 60000);
                const endTime = endDate.toISOString().split('T')[0] + 'T' + endDate.toTimeString().split(' ')[0];
                console.log('Start Time:', startDate); // Log the start time
                console.log('End Time:', endDate); // Log the end time

                events.push({
                    id: `actual-${detail.ActualId}`,
                    title: 'Actual ',
                    start: actualTime,
                    end: endTime,
                    resourceId: `${detail.PlanDetailId * 2}`
                });
                console.log('Event:', events[events.length - 1]); // Log the event object
            });



            return events;
        }

    var calendar = new FullCalendar.Calendar(calendarEl, {
        schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
        initialView: 'resourceTimelineDay',
        // Mỗi slot là 30 phút
        slotDuration: '01:00',
        // Mỗi slot cách nhau 1 tiếng
        slotLabelInterval: '00:30',
        slotLabelFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false // Sử dụng định dạng 24 giờ
        },
        // Hiển thị các ô thời gian từ 6 giờ sáng hôm trước đến 6 giờ sáng hôm sau
        slotMinTime: '00:00:00',
        slotMaxTime: '24:00:00',
        stickyFooterScrollbar: 'auto',
        resourceAreaWidth: '110px',
        height: 1100,
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
        //Sắp xếp theo thứ tự theo order(Plan trước Actual sau)
        events: generateDailyEvents(planDetails, actualDetails),
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
            var start = new Date(info.event.start);
            var end = new Date(info.event.end);

            // Định dạng thời gian bắt đầu và kết thúc theo dạng ' HH:mm'
            var formattedStart = start.toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit'
            });

            var formattedEnd = end.toLocaleString('vi-VN', {
                timeZone: 'UTC',
                hour: '2-digit',
                minute: '2-digit'
            });

            // Đưa dữ liệu sự kiện vào modal
            document.getElementById('eventDetails').innerText = ` ${info.event.title}\nBắt đầu: ${formattedStart}`;

            // Kiểm tra nếu sự kiện thuộc "Actual" row
            if (info.event.getResources().some(resource => resource.title === "Actual")) {
                // Gọi API để tải các giai đoạn của sự kiện
                loadEventStages(info.event.id);
                //Nếu là Plan thì chỉ hiện Start và End
                document.getElementById('stagesTable').style.display = 'table';
            } else {
                //Nếu là Actua thì hiện bảng Stage
                document.getElementById('stagesTable').style.display = 'none';
            }

            // Hiển thị modal với các giai đoạn và nút điều khiển
            var myModal = new bootstrap.Modal(document.getElementById('eventModal'));
            myModal.show();
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
            return { html: `<i style="color: blue; text-decoration: none;">${arg.text}</i>` };
        },
        slotLabelClassNames: function (arg) {
            return ['custom-slot-label'];
        }

    });


    calendar.render();
});
//HIỂN THỊ THỜI GIAN THỰC
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
