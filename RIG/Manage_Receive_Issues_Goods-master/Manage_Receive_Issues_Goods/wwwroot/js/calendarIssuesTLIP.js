document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

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

    //KHỞI TẠO LỊCH
    var calendar = new FullCalendar.Calendar(calendarEl, {
        schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
        initialView: 'resourceTimeGridFourDay',
        slotDuration: '00:30',
        datesAboveResources: true,
        // Mỗi slot cách nhau 1 tiếng
        slotLabelInterval: '01:00',
        slotLabelFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false // Sử dụng định dạng 24 giờ
        },
        // Hiển thị các ô thời gian theo ý muốn
        slotMinTime: '00:00:00',
        slotMaxTime: '24:00:00',
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
            right: 'resourceTimeGridDay,resourceTimeGridFourDay'
        },
        editable: false,
        allDaySlot: false,
        //Lấy API để hiển thị cột Actual và Plan
        resources: [
            { id: '1', title: 'Plan', eventColor: '#1E2B37' },
            { id: '2', title: 'Actual', eventColor: '#3E7D3E' }
        ],
        //Sắp xếp theo thứ tự theo order(Plan trước Actual sau)
        resourceOrder: 'order',
        events: '/TLIPWarehouse/GetPlanAndActualEvents',
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
            document.getElementById('eventDetails').innerText = `Nhà cung cấp: ${info.event.title}\nBắt đầu: ${formattedStart}\nKết thúc: ${formattedEnd}`;

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
        //FOMAT NGÀY THÁNG và phần view
        views: {
            resourceTimeGridFourDay: {
                type: 'resourceTimeGrid',
                duration: { days: 7 },
                buttonText: '7 days',
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' },
            },
            resourceTimeGridDay: {
                titleFormat: { day: '2-digit', month: '2-digit', year: 'numeric' }
            }
        },
        resourceLaneContent: function (arg) {
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
