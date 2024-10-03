//// testGenerateDailyEvents.js

//// Hàm generateDailyEvents từ tệp gốc
//function generateDailyEvents(planDetails) {
//    const events = [];
//    const titles = planDetails.map(detail => detail.PlanDetailName);
//    const times = planDetails.map(detail => detail.PlanTime);

//    for (let i = 0; i < 7; i++) { // Generate for 7 days
//        const date = new Date();
//        date.setDate(date.getDate() + i);
//        const dateString = date.toISOString().split('T')[0];

//        titles.forEach((title, index) => {
//            events.push({
//                id: `${i}-${index}`,
//                title: title,
//                start: `${dateString}T${times[index]}`,
//                resourceId: index % 2 === 0 ? '1' : '2'
//            });
//        });
//    }

//    return events;
//}

//// Dữ liệu mẫu để kiểm thử
//const planDetails = [
//    { PlanDetailName: 'Event 1', PlanTime: '08:00:00' },
//    { PlanDetailName: 'Event 2', PlanTime: '09:00:00' }
//];

//// Gọi hàm và in kết quả ra bảng điều khiển
//const events = generateDailyEvents(planDetails);
//console.log(events);
