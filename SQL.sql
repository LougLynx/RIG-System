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
(1, 9, 3, 120), 
-- Supplier B, giao hàng vào Thứ Ba, lúc 10:00:00, thời gian chuẩn bị 90 phút
(2, 11, 3, 90);

INSERT INTO `ScheduleReceived` (`SupplierID`, `DeliveryTimeID`, `WeekdayID`, `LeadTime`)
VALUES 
-- Supplier A, giao hàng vào Thứ Ba, lúc 12:00:00, thời gian chuẩn bị 90 phút
(3, 13, 3, 90), 
-- Supplier B, giao hàng vào Thứ Ba, lúc 14:00:00, thời gian chuẩn bị 120 phút
(6, 15, 3, 120),
-- Supplier C, giao hàng vào Thứ Ba, lúc 16:00:00, thời gian chuẩn bị 125 phút
(4, 17, 3, 90),
-- Supplier D, giao hàng vào Thứ Ba, lúc 20:00:00, thời gian chuẩn bị 160 phút
(5, 21, 3, 160);

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
-- Dumping structure for table Customer
-- --------------------------------------------------------
DROP TABLE IF EXISTS `Customer`;
CREATE TABLE IF NOT EXISTS `Customer` (
  `CustomerId` int NOT NULL AUTO_INCREMENT,
  `CustomerName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Dumping structure for table ScheduleIssuedDenso
-- --------------------------------------------------------
DROP TABLE IF EXISTS `ScheduleIssuedDenso`;
CREATE TABLE IF NOT EXISTS `ScheduleIssuedDenso` (
  `ScheduleDensoID` int NOT NULL AUTO_INCREMENT,
  `CustomerID` int NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  PRIMARY KEY (`ScheduleDensoID`),
  FOREIGN KEY (`CustomerID`) REFERENCES `Customer`(`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Dumping structure for table Stage
-- --------------------------------------------------------
DROP TABLE IF EXISTS `Stage`;
CREATE TABLE IF NOT EXISTS `Stage` (
  `StageId` int NOT NULL AUTO_INCREMENT,
  `ScheduleDensoID` int NOT NULL,
  `StageName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`StageId`),
  FOREIGN KEY (`ScheduleDensoID`) REFERENCES `ScheduleIssuedDenso`(`ScheduleDensoID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Dumping structure for table StageDelay
-- --------------------------------------------------------
DROP TABLE IF EXISTS `StageDelay`;
CREATE TABLE IF NOT EXISTS `StageDelay` (
  `DelayID` int NOT NULL AUTO_INCREMENT,
  `StageID` int NOT NULL,
  `OldStartTime` datetime NOT NULL,
  `OldEndTime` datetime NOT NULL,
  `NewStartTime` datetime NOT NULL,
  `NewEndTime` datetime NOT NULL,
  `DelayReason` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TimeStampDelay` datetime NOT NULL,
  PRIMARY KEY (`DelayID`),
  FOREIGN KEY (`StageID`) REFERENCES `Stage`(`StageId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Insert dữ liệu vào bảng Customer
INSERT INTO `Customer` (`CustomerName`)
VALUES 
('Customer A'),
('Customer B'),
('Customer C'),
('Customer D'),
('Customer E'),
('Customer F');

-- Insert dữ liệu vào bảng ScheduleIssuedDenso
INSERT INTO `ScheduleIssuedDenso` (`CustomerID`, `StartTime`, `EndTime`)
VALUES 
(1, '2023-10-01 08:00:00', '2023-10-01 10:00:00'),
(2, '2023-10-02 09:00:00', '2023-10-02 11:00:00'),
(3, '2023-10-03 10:00:00', '2023-10-03 12:00:00');

-- Insert dữ liệu vào bảng Stage
INSERT INTO `Stage` (`ScheduleDensoID`, `StageName`, `StartTime`, `EndTime`, `Status`)
VALUES 
(1, 'Stage 1', '2023-10-01 08:00:00', '2023-10-01 08:30:00', 'Completed'),
(1, 'Stage 2', '2023-10-01 08:30:00', '2023-10-01 09:00:00', 'Pending'),
(2, 'Stage 1', '2023-10-02 09:00:00', '2023-10-02 09:30:00', 'Completed'),
(2, 'Stage 2', '2023-10-02 09:30:00', '2023-10-02 10:00:00', 'Pending'),
(3, 'Stage 1', '2023-10-03 10:00:00', '2023-10-03 10:30:00', 'Completed'),
(3, 'Stage 2', '2023-10-03 10:30:00', '2023-10-03 11:00:00', 'Pending');

-- Insert dữ liệu vào bảng StageDelay
INSERT INTO `StageDelay` (`StageID`, `OldStartTime`, `OldEndTime`, `NewStartTime`, `NewEndTime`, `DelayReason`, `TimeStampDelay`)
VALUES 
(2, '2023-10-01 08:30:00', '2023-10-01 09:00:00', '2023-10-01 09:00:00', '2023-10-01 09:30:00', 'Technical Issue', '2023-10-01 08:45:00'),
(4, '2023-10-02 09:30:00', '2023-10-02 10:00:00', '2023-10-02 10:00:00', '2023-10-02 10:30:00', 'Resource Unavailable', '2023-10-02 09:45:00');




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
