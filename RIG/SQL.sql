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
	 -- DROP DATABASE IF EXISTS `rig`;
	 -- CREATE DATABASE IF NOT EXISTS `rig` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
	USE `rig`;

	-- --------------------------------------------------------
	-- Dumping structure for table Supplier
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `Supplier`;
	CREATE TABLE IF NOT EXISTS `Supplier` (
  `SupplierCode` nvarchar(100) NOT NULL,
  `SupplierName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ,
  PRIMARY KEY (`SupplierCode`)
	) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


	INSERT INTO `Supplier` (`SupplierCode`, `SupplierName`) VALUES
('A01', 'OHARA VIETNAM'),
('A02', 'MATSUO VIETNAM'),
('A03', 'HAL VIETNAM'),
('A04', 'KEINHING VIETNAM'),
('A06', 'NIPPO MECHATRONIC VIETNAM'),
('A07', 'NSK VIETNAM'),
('A08', 'FUKOKU VIETNAM'),
('A09', 'RYHTHM KYOSHIN HN'),
('A10', 'TOYOTA TSUSHO VN'),
('B04', 'CTY QUỐC TUẤN'),
('B08', 'CTY THUẬN AN'),
('B10', 'CTY TNHH THUONG MAI PHUC MA'),
('B105', 'CTY TNHH HTECH'),
('B11', 'CTY TNHH HOA HOC UNG DỤNG'),
('B12', 'CTY TNHH AIR LIQUIDE VIET NAM'),
('B13', 'CTY TNHH NGUYEN VAN THA'),
('B132', 'CTY TNHH ANH MINH'),
('B134', 'CTY TNHH ĐỨC MINH'),
('B139', 'CTY TNHH VIỆT ANH'),
('B17', 'CTY TNHH THUONG MAI HA ANH'),
('B18', 'CTY CP SX & DV CONG NGHIEP GMF'),
('B20', 'CTY TNHH DUC ANH'),
('B24', 'CTY CP CHUYEN GIAO CN TECH VINA'),
('B35', 'HA SON'),
('B190', 'IDEMITSU'),
('B205', 'TOPSUN'),
('B206', 'TANAKA KIKINZOKU'),
('B228', 'SHINJIKI'),
('B32', 'Longson JSC'),
('B237', 'SUNRISE'),
('B269', 'Công ty TNHH PT'),
('B214', 'NISHI TOKYO CHEMIX CORPORATION'),
('B318', 'UENO'),
('B369', 'CHI NHANH CTY NHẤT PHÁT TẠI HN'),
('B372', 'CTY TNHH SERTIM'),
('B71', 'JFE'),
('D02', 'DENSO KOREA CORPORATION'),
('D10/D03/D05/TD12/TD15', 'DNIA'),
('D11', 'DMAT'),
('E01', 'KYOSHIN VIETNAM'),
('E02', 'MARUEI VIETNAM PRECISION Co.,Ltd'),
('E03', 'MEINAN VIETNAM PRECISION Co.,Ltd'),
('E04', 'HARIKI'),
('E05', 'ADVANEX VIETNAM'),
('E06', 'ATARIH PRECISION'),
('E27', 'CHUBURIKA VIETNAM'),
('E09', 'VINA TAIYO'),
('E10', 'FUJIKURA VN/ FUJIKURA TRADING(HCM BRANCH)'),
('E28', 'FUJIKURA VN/ FUJIKURA TRADING(HCM BRANCH)'),
('E18', 'KURABE INDUCTRIES VN'),
('E14', 'SANYO VN'),
('E13', 'KAWASAKI VN'),
('E15', 'SURTECKARIYA VN'),
('E16', 'SHOEI VN'),
('E17', 'TOPY FASTERNER VN'),
('E21', '(TOYO) DRILIBE VN'),
('E12', 'SANWA VN'),
('E19', 'NOGUCHI VIETNAM --> Fertile VN'),
('E20', 'CURIOUS SEIKI VN'),
('E22', 'OHASHI TEKKO VIETNAM'),
('E26', 'MABUCHI VN'),
('E24', 'DAITO RUBBER'),
('E25', 'KURODA VN'),
('E23', 'Micro Techno'),
('E29', 'Vietinak VN'),
('G01', 'NOK THAI'),
('G66/G68', 'DENSO SALE THAI'),
('D09/TD20/TD23/TD24', 'DENSO THAILAND'),
('G03', 'SWS THAILAND'),
('G05', 'YAMADA SOMBOON'),
('G06', 'DAIKI THAILAND'),
('T14', 'YAHAGI THAILAND'),
('FT01', 'DENSO SALE THAI (TRADING APM6)'),
('G02', 'HITACHI METALS THAI'),
('H91', 'OKAYA CO , . LTD'),
('H93', 'SANKO'),
('H92', 'IDAKA'),
('H78/H84/H88', 'TOYOTA TSUSHO JAPAN 1 ( FIRMED PERIOD 2 WEEK)'),
('H79', 'TOYOTA TSUSHO JAPAN 2 ( FIRMED PERIOD 4 WEEK)'),
('H82', 'TOYOTA TSUSHO JAPAN 3 ( FIRMED PERIOD 6 WEEK)'),
('H85', 'NAGASE CO.,Ltd'),
('H87', 'TSUCHIYA'),
('H89', 'TOYOTA TSUSHO JAPAN 7 (MASUYASU)'),
('H94', 'TSUCHIYA PARTS'),
('H95', 'KONISHI Co.,LTD'),
('H96', 'ITOMOL'),
('H98', 'MITSUBISHI'),
('I17', 'NISSIN KOGYO'),
('I18', 'TDK HONGKONG'),
('I15', 'SHANGHAI MIKURO SPRING'),
('I02', 'DAIMET KLANG'),
('I03', 'NOK SINGAPORE'),
('I04', 'HOKURIKU DENSO SINGAPORE'),
('I05', 'NTA MACHINING STD BHD'),
('I06', 'HOINAK LIMITTED'),
('I07', 'ASAHI KOSEI (M) SDN.BHD.CO'),
('I10', 'SCHAEFFLER (SINGAPORE)'),
('I12', 'NITTO SHOJI LIMITTED'),
('I14', 'HIRATA (SHENZHEN) CO.,LTD'),
('I21', 'GSE CAMBODIA'),
('J01', 'DNJP'),
('TD30', 'DNMY'),
('TD50', 'DIAS'),
('TD60', 'HDVN'),
('TD11', 'DNIA'),
('TD51', 'DIAS'),
('TD70', 'SKD'),
('TD71', 'SKD'),
('TD22', 'ADTH'),
('I20', 'SUMITOMO ELECTRIC ASIA PA'),
('B28', 'CTY TNHH TM & CONG NGHE HONG DUONG'),
('B30', 'CONG TY TNHH KSMC'),
('D12', 'DENSO MANUFACTURING HUNGA'),
('A14', 'Parker Processing VN'),
('E32', 'Toyo Brazing VN'),
('E31', 'null'),
('I23', 'Scherdel'),
('I24', 'JONHSON ELECTRIC WORLD TRADE COMPANY'),
('B159', 'Kura'),
('D08', 'AINE'),
('D04', 'HDVN'),
('D13', 'DNMX ( Denso Mexico )'),
('E30', 'Viet nam Nok co, LTD'),
('B222', 'ARMOR WEATHYSHINE'),
('A12', 'NIPPO MECHATRONIC VIETNAM'),
('TD61', 'DMVN'),
('J01.1', 'DNJP'),
('TD42', 'DNWX'),
('Di261401-5200', 'null'),
('FT03', 'NIPPO'),
('T08', 'MINEBEA');

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
	-- Dumping structure for table Plans
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `PlanReceiveTLIP`;
	CREATE TABLE IF NOT EXISTS `PlanReceiveTLIP` (
		`PlanID` int NOT NULL AUTO_INCREMENT,
		`PlanName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
		`EffectiveDate` date NOT NULL, -- New field to indicate when the plan should start being applied
		PRIMARY KEY (`PlanID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
    
    
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `PlanDetailReceivedTLIP`;
	CREATE TABLE IF NOT EXISTS `PlanDetailReceivedTLIP` (
	  `PlanDetailID` int NOT NULL AUTO_INCREMENT,
	  `PlanID` int NOT NULL,
	  `SupplierCode` nvarchar(100) NOT NULL,
	  `DeliveryTime` time NOT NULL,
	  `WeekdayID` int NOT NULL,
	  `LeadTime` time NOT NULL,
	  PRIMARY KEY (`PlanDetailID`),
	  FOREIGN KEY (`SupplierCode`) REFERENCES `Supplier`(`SupplierCode`),
	  FOREIGN KEY (`WeekdayID`) REFERENCES `Weekday`(`WeekdayID`),
	  FOREIGN KEY (`PlanID`) REFERENCES `PlanReceiveTLIP`(`PlanID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



	-- Insert dữ liệu vào bảng ScheduleReceived cho các kế hoạch Plan
	INSERT INTO `PlanDetailReceivedTLIP` (`PlanID`, `SupplierCode`, `DeliveryTime`, `WeekdayID`, `LeadTime`) VALUES
	(1, 'SUP001', '08:00:00', 1, '00:30:00'),
	(1, 'SUP002', '09:00:00', 2, '01:00:00'),
	(1, 'SUP003', '10:00:00', 3, '01:30:00');

	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ActualReceivedTLIP`;
	CREATE TABLE IF NOT EXISTS `ActualReceivedTLIP` (
	  `ActualReceivedID` int NOT NULL AUTO_INCREMENT,
	  `ActualDeliveryTime` datetime NOT NULL,
	  `ActualLeadTime` time ,
	  `SupplierCode` nvarchar(100) NOT NULL,
      `AsnNumber` nvarchar(100) ,
      `DoNumber` varchar(100) ,
      `Invoice` varchar(100) ,
	  PRIMARY KEY (`ActualReceivedID`),
	 FOREIGN KEY (`SupplierCode`) REFERENCES `Supplier`(`SupplierCode`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
		DROP TABLE IF EXISTS `ActualDetailTLIP`;
	CREATE TABLE IF NOT EXISTS `ActualDetailTLIP` (
	  `ActualDetailID` int NOT NULL AUTO_INCREMENT,
	  `PartNo` varchar(100) NOT NULL,
	  `Quantity` int,
	  `QuantityRemain` int,
	  `ActualReceivedID` int NOT NULL,
	  PRIMARY KEY (`ActualDetailID`),
	  FOREIGN KEY (`ActualReceivedID`) REFERENCES `ActualReceivedTLIP`(`ActualReceivedID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------------------------------------


	-- Insert dữ liệu vào bảng ActualReceived cho các lần nhận hàng thực tế (Actual)
	INSERT INTO `ActualReceivedTLIP` (`ActualDeliveryTime`, `ActualLeadTime`, `SupplierCode`) VALUES
	('2024-10-15 08:00:00', 30, 'SUP001'),
	('2024-10-15 09:00:00', null, 'SUP002'),
	('2024-10-15 10:00:00', 90, 'SUP003');

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
-- -----------------------------------------------------------------------------------------------------------
	DROP TABLE IF EXISTS `PlanRITDDetails`;
CREATE TABLE IF NOT EXISTS `PlanRITDDetails` (
    `PlanDetailID` int NOT NULL AUTO_INCREMENT,
    `PlanID` int NOT NULL,
    `PlanTime` time NOT NULL,
    `PlanDetailName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `StatusReceiveID` int,
    `StatusIssueID` int,
    PRIMARY KEY (`PlanDetailID`),
    FOREIGN KEY (`PlanID`) REFERENCES `PlanRITD`(`PlanID`),
    FOREIGN KEY (`StatusReceiveID`) REFERENCES `StatusesRITD`(`StatusID`),
    FOREIGN KEY (`StatusIssueID`) REFERENCES `StatusesRITD`(`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


	-- --------------------------------------------------------
	-- Dumping structure for table ActualsReceiveDenso
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ActualsReceiveDenso`;
	CREATE TABLE IF NOT EXISTS `ActualsReceiveDenso` (
		`ActualID` int NOT NULL AUTO_INCREMENT,
		`PlanDetailID` int NOT NULL,
		`ActualTime` datetime NOT NULL,
		PRIMARY KEY (`ActualID`),
		FOREIGN KEY (`PlanDetailID`) REFERENCES `PlanRITDDetails`(`PlanDetailID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
    
    -- --------------------------------------------------------
	-- Dumping structure for table ActualsIssuesTLIP
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `ActualsIssuesTLIP`;
	CREATE TABLE IF NOT EXISTS `ActualsIssuesTLIP` (
		`ActualID` int NOT NULL AUTO_INCREMENT,
		`PlanDetailID` int NOT NULL,
		`ActualTime` datetime NOT NULL,
		PRIMARY KEY (`ActualID`),
		FOREIGN KEY (`PlanDetailID`) REFERENCES `PlanRITDDetails`(`PlanDetailID`)
	) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
	-- --------------------------------------------------------
	-- Dumping structure for table Statuses
	-- --------------------------------------------------------
	DROP TABLE IF EXISTS `StatusesRITD`;
CREATE TABLE IF NOT EXISTS `StatusesRITD` (
    `StatusID` int NOT NULL AUTO_INCREMENT,
    `Status` ENUM('Pending', 'In Transit', 'Delivered', 'Received') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	-- Insert dữ liệu mẫu vào bảng PlanRITD
	INSERT INTO `PlanRITD` (`PlanName`, `PlanType`,`TotalShipment`,`EffectiveDate`)
	VALUES 
	('Plan RITD', 'Received', 20,'2024-10-03'),
    ('Plan RITD', 'Received', 20,'2024-10-09');	

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
	(1,  '23:30:00', 'Số 20'),
	(2, '00:30:00', 'Số 1'),
	(2,  '02:00:00', 'Số 2'),
	(2,  '03:40:00', 'Số 3'),
	(2,  '04:30:00', 'Số 4'),
	(2,  '06:00:00', 'Số 5'),
	(2,  '07:30:00', 'Số 6'),
	(2,  '08:30:00', 'Số 7'),
	(2,  '10:00:00', 'Số 8'),
	(2,  '10:10:00', 'Số 9'),
	(2,  '12:30:00', 'Số 10'),
	(2,  '13:30:00', 'Số 11'),
	(2,  '14:30:00', 'Số 12'),
	(2,  '15:30:00', 'Số 13'),
	(2,  '17:00:00', 'Số 14'),
	(2,  '18:30:00', 'Số 15'),
	(2,  '19:30:00', 'Số 16'),
	(2,  '20:30:00', 'Số 17'),
	(2,  '21:30:00', 'Số 18'),
	(2,  '22:30:00', 'Số 19'),
	(2,  '23:30:00', 'Số 20');

	-- Insert dữ liệu mẫu vào bảng ActualsReceiveDenso
	INSERT INTO `ActualsReceiveDenso` (`PlanDetailID`, `ActualTime`)
	VALUES 
	(1, '2024-10-12 01:00:00'),
	(2, '2024-10-12 02:30:00'),
	(3, '2024-10-12 04:00:00'),
	(4, '2024-10-12 05:00:00'),
	(5, '2024-10-12 06:30:00'),
	(6, '2024-10-12 08:30:00'),	
	(7, '2024-10-12 09:00:00'),
	(8, '2024-10-12 10:00:00'),
	(9, '2024-10-12 11:00:00'),
    (10, '2024-10-12 12:00:00'),
	(11, '2024-10-12 13:30:00'),
	(12, '2024-10-12 14:00:00'),
	(13, '2024-10-12 15:00:00'),
	(14, '2024-10-12 16:30:00'),
	(15, '2024-10-12 18:30:00'),	
	(16, '2024-10-12 19:00:00'),
	(17, '2024-10-12 19:00:00'),
	(18, '2024-10-12 20:00:00'),
    (19, '2024-10-12 21:00:00'),
	(20, '2024-10-12 22:00:00'),
    
    (1, '2024-10-13 01:00:00'),
    (2, '2024-10-13 02:30:00'),
	(3, '2024-10-13 04:00:00'),
	(4, '2024-10-13 05:00:00'),
	(5, '2024-10-13 06:30:00'),
	(6, '2024-10-13 08:30:00'),	
	(7, '2024-10-13 09:00:00'),
	(8, '2024-10-13 10:00:00'),
	(9, '2024-10-13 11:00:00'),
    (10, '2024-10-13 12:00:00'),
	(11, '2024-10-13 13:30:00'),
	(12, '2024-10-13 14:00:00'),
	(13, '2024-10-13 15:00:00'),
	(14, '2024-10-13 16:30:00'),
	(15, '2024-10-13 18:30:00'),		
	(16, '2024-10-13 19:00:00'),
	(17, '2024-10-13 19:00:00'),
	(18, '2024-10-13 20:00:00'),
    (19, '2024-10-13 21:00:00'),
	(20, '2024-10-13 22:00:00');

INSERT INTO `StatusesRITD` (`Status`) VALUES ('Pending');
INSERT INTO `StatusesRITD` (`Status`) VALUES ('In Transit');
INSERT INTO `StatusesRITD` (`Status`) VALUES ('Delivered');
INSERT INTO `StatusesRITD` (`Status`) VALUES ('Received');


	/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
	/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
	/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
	/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
	/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
