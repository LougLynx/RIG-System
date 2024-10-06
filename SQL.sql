	/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
	/*!40101 SET NAMES utf8 */;
	/*!50503 SET NAMES utf8mb4 */;
	/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
	/*!40103 SET TIME_ZONE='+00:00' */;
	/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
	/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
	/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

	-- --------------------------------------------------------
	-- Host:                         127.0.0.1
	-- Server version:               8.0.37 - MySQL Community Server - GPL
	-- Server OS:                    Win64
	-- HeidiSQL Version:             12.7.0.6850
	-- --------------------------------------------------------
			
	-- Dumping database structure for receive_issues_goods
	-- --------------------------------------------------------
	DROP DATABASE IF EXISTS `rig`;
	CREATE DATABASE IF NOT EXISTS `rig` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
	USE `rig`;

	-- --------------------------------------------------------
	-- Dumping structure for table Supplier
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `Supplier`;
	CREATE TABLE IF NOT EXISTS `Supplier` (
	  `SupplierID` int NOT NULL AUTO_INCREMENT,
	  `SupplierName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
	  PRIMARY KEY (`SupplierID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	INSERT INTO `Supplier` (`SupplierName`)
	VALUES 
	('Supplier A'),
	('Supplier B'),
	('Supplier C'),
	('Supplier D'),
	('Supplier E'),
	('Supplier F'),
	('Supplier G');

	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `Time`;
	CREATE TABLE IF NOT EXISTS `Time` (
	  `TimeID` int NOT NULL AUTO_INCREMENT,
	  `Time` time NOT NULL,
	  PRIMARY KEY (`TimeID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert 24 hours into the Time table
	INSERT INTO `Time` (`Time`)
	VALUES 
	('00:00:00'), ('01:00:00'), ('02:00:00'), ('03:00:00'), ('04:00:00'), ('05:00:00'),
	('06:00:00'), ('07:00:00'), ('08:00:00'), ('09:00:00'), ('10:00:00'), ('11:00:00'),
	('12:00:00'), ('13:00:00'), ('14:00:00'), ('15:00:00'), ('16:00:00'), ('17:00:00'),
	('18:00:00'), ('19:00:00'), ('20:00:00'), ('21:00:00'), ('22:00:00'), ('23:00:00');

	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `Weekday`;
	CREATE TABLE IF NOT EXISTS `Weekday` (
	  `WeekdayID` int NOT NULL AUTO_INCREMENT,
	  `DayName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
	  PRIMARY KEY (`WeekdayID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert days of the week into the Weekday table
	INSERT INTO `Weekday` (`DayName`)
	VALUES 
	('Monday'), ('Tuesday'), ('Wednesday'), ('Thursday'), ('Friday'), ('Saturday'), ('Sunday');

	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ScheduleReceived`;
	CREATE TABLE IF NOT EXISTS `ScheduleReceived` (
	  `ScheduleID` int NOT NULL AUTO_INCREMENT,
	  `SupplierID` int NOT NULL,
	  `DeliveryTimeID` int NOT NULL,
	  `WeekdayID` int NOT NULL,
	  `LeadTime` int NOT NULL,
	  PRIMARY KEY (`ScheduleID`),
	  FOREIGN KEY (`SupplierID`) REFERENCES `Supplier`(`SupplierID`),
	  FOREIGN KEY (`DeliveryTimeID`) REFERENCES `Time`(`TimeID`),
	  FOREIGN KEY (`WeekdayID`) REFERENCES `Weekday`(`WeekdayID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert dữ liệu vào bảng ScheduleReceived cho các kế hoạch Plan
	INSERT INTO `ScheduleReceived` (`SupplierID`, `DeliveryTimeID`, `WeekdayID`, `LeadTime`)
	VALUES 
	-- Supplier A, giao hàng vào Thứ Ba, lúc 08:00:00, thời gian chuẩn bị 120 phút
	(1, 9, 6, 120), 
	-- Supplier B, giao hàng vào Thứ Ba, lúc 10:00:00, thời gian chuẩn bị 90 phút
	(2, 11, 6, 90);

	INSERT INTO `ScheduleReceived` (`SupplierID`, `DeliveryTimeID`, `WeekdayID`, `LeadTime`)
	VALUES 
	-- Supplier A, giao hàng vào Thứ Ba, lúc 12:00:00, thời gian chuẩn bị 90 phút
	(3, 13, 6, 90), 
	-- Supplier B, giao hàng vào Thứ Ba, lúc 14:00:00, thời gian chuẩn bị 120 phút
	(6, 15, 6, 120),
	-- Supplier C, giao hàng vào Thứ Ba, lúc 16:00:00, thời gian chuẩn bị 125 phút
	(4, 17, 6, 90),
	-- Supplier D, giao hàng vào Thứ Ba, lúc 20:00:00, thời gian chuẩn bị 160 phút
	(5, 21, 6, 160);

	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ActualReceived`;
	CREATE TABLE IF NOT EXISTS `ActualReceived` (
	  `ActualReceivedID` int NOT NULL AUTO_INCREMENT,
	  `ActualDeliveryTime` datetime NOT NULL,
	  `ActualLeadTime` int NOT NULL,
	  `ScheduleID` int NOT NULL,
	  PRIMARY KEY (`ActualReceivedID`),
	  FOREIGN KEY (`ScheduleID`) REFERENCES `ScheduleReceived`(`ScheduleID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert dữ liệu vào bảng ActualReceived cho các lần nhận hàng thực tế (Actual)
	INSERT INTO `ActualReceived` (`ScheduleID`, `ActualDeliveryTime`, `ActualLeadTime`)
	VALUES 
	-- Thực nhận hàng cho Supplier A, lúc 08:15:00 ngày 29/09/2024, thời gian xử lý 130 phút
	(1, '2024-10-02 08:15:00', 130),
	-- Thực nhận hàng cho Supplier B, lúc 10:15:00 ngày 30/09/2024, thời gian xử lý 95 phút
	(2, '2024-10-02 10:15:00', 95);


	-- --------------------------------------------------------
	-- Dumping structure for table Plans
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `PlanRITD`;
	CREATE TABLE IF NOT EXISTS `PlanRITD` (
		`PlanID` int NOT NULL AUTO_INCREMENT,
		`PlanName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
		`PlanType` ENUM('Received', 'Issued') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
		`TotalShipment` int NOT NULL DEFAULT 0,
		`EffectiveDate` date NOT NULL, -- New field to indicate when the plan should start being applied
		PRIMARY KEY (`PlanID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	DROP TABLE IF EXISTS `PlanRITDDetails`;
	CREATE TABLE IF NOT EXISTS `PlanRITDDetails` (
		`PlanDetailID` int NOT NULL AUTO_INCREMENT,
		`PlanID` int NOT NULL,
		`PlanTime` time NOT NULL,
		`PlanDetailName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
		PRIMARY KEY (`PlanDetailID`),
		FOREIGN KEY (`PlanID`) REFERENCES `PlanRITD`(`PlanID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


	-- --------------------------------------------------------
	-- Dumping structure for table Actuals
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ActualsRITD`;
	CREATE TABLE IF NOT EXISTS `ActualsRITD` (
		`ActualID` int NOT NULL AUTO_INCREMENT,
		`PlanDetailID` int NOT NULL,
		`ActualTime` datetime NOT NULL,
		PRIMARY KEY (`ActualID`),
		FOREIGN KEY (`PlanDetailID`) REFERENCES `PlanRITDDetails`(`PlanDetailID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
	-- --------------------------------------------------------
	-- Dumping structure for table Statuses
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `Statuses`;
	CREATE TABLE IF NOT EXISTS `Statuses` (
		`StatusID` int NOT NULL AUTO_INCREMENT,
		`PlanDetailID` int NOT NULL,
		`Status` ENUM('Pending', 'In Transit', 'Delivered') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
		PRIMARY KEY (`StatusID`),
		FOREIGN KEY (`PlanDetailID`) REFERENCES `PlanRITDDetails`(`PlanDetailID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert dữ liệu mẫu vào bảng PlanRITD
	INSERT INTO `PlanRITD` (`PlanName`, `PlanType`,`TotalShipment`,`EffectiveDate`)
	VALUES 
	('Plan Issued', 'Issued', 20,'2024-10-03');

	-- Insert dữ liệu mẫu vào bảng PlanRITDDetails
	INSERT INTO `PlanRITDDetails` (`PlanID`,  `PlanTime`,`PlanDetailName`)
	VALUES 
	(1, '00:30:00', 'Số 1'),
	(1,  '02:00:00', 'Số 2'),
	(1,  '03:40:00', 'Số 3'),
	(1,  '04:30:00', 'Số 4'),
	(1,  '06:00:00', 'Số 5'),
	(1,  '07:30:00', 'Số 6'),
	(1,  '08:30:00', 'Số 7'),
	(1,  '10:00:00', 'Số 8'),
	(1,  '10:10:00', 'Số 9'),
	(1,  '12:30:00', 'Số 10'),
	(1,  '13:30:00', 'Số 11'),
	(1,  '14:30:00', 'Số 12'),
	(1,  '15:30:00', 'Số 13'),
	(1,  '17:00:00', 'Số 14'),
	(1,  '18:30:00', 'Số 15'),
	(1,  '19:30:00', 'Số 16'),
	(1,  '20:30:00', 'Số 17'),
	(1,  '21:30:00', 'Số 18'),
	(1,  '22:30:00', 'Số 19'),
	(1,  '23:30:00', 'Số 20');

	-- Insert dữ liệu mẫu vào bảng ActualsRITD
	INSERT INTO `ActualsRITD` (`PlanDetailID`, `ActualTime`)
	VALUES 
	(1, '2024-10-06 01:00:00'),
	(2, '2024-10-06 02:30:00'),
	(3, '2024-10-06 04:00:00'),
	(4, '2024-10-06 05:00:00'),
	(5, '2024-10-06 06:30:00'),
	(6, '2024-10-06 08:30:00'),	
	(7, '2024-10-06 09:00:00'),
	(8, '2024-10-06 10:00:00'),
	(9, '2024-10-06 11:00:00');

	-- Insert dữ liệu mẫu vào bảng Statuses
	INSERT INTO `Statuses` (`PlanDetailID`, `Status`)
	VALUES 
	(1, 'In Transit'),
	(2, 'Pending'),
	(3, 'Confirm');

	/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
	/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
	/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
	/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
	/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
