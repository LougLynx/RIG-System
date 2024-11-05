-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: rig
-- ------------------------------------------------------
-- Server version	8.0.36

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('00000000000000_CreateIdentitySchema','8.0.8');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actualdetailtlip`
--

DROP TABLE IF EXISTS `actualdetailtlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actualdetailtlip` (
  `ActualDetailID` int NOT NULL AUTO_INCREMENT,
  `PartNo` varchar(100) NOT NULL,
  `Quantity` int DEFAULT NULL,
  `QuantityRemain` int DEFAULT NULL,
  `ActualReceivedID` int NOT NULL,
  PRIMARY KEY (`ActualDetailID`),
  KEY `ActualReceivedID` (`ActualReceivedID`),
  CONSTRAINT `actualdetailtlip_ibfk_1` FOREIGN KEY (`ActualReceivedID`) REFERENCES `actualreceivedtlip` (`ActualReceivedID`)
) ENGINE=InnoDB AUTO_INCREMENT=196 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actualdetailtlip`
--

LOCK TABLES `actualdetailtlip` WRITE;
/*!40000 ALTER TABLE `actualdetailtlip` DISABLE KEYS */;
INSERT INTO `actualdetailtlip` VALUES (1,'VN012020-0380',2592,0,1),(2,'VN012020-0390',1760,0,1),(3,'VN012075-0040',4000,4000,1),(4,'VN079659-0090',7500,0,1),(5,'VN082028-0340',4000,4000,1),(6,'VN082042-0070',4000,4000,1),(7,'VN229739-0060',10000,0,1),(8,'VN198811-7450',2430,0,2),(9,'VN012020-0240',2520,0,3),(10,'VN197427-0090',450,0,3),(11,'VN198827-7000',2000,0,3),(12,'VN270135-0011',3000,0,3),(13,'VN012020-0290',825,0,4),(14,'VN012025-0101',1200,0,4),(15,'VN012025-0290',3024,0,4),(16,'VN270133-0010',400,0,4),(17,'VN150111-0381',300,0,5),(18,'VN012025-0340',3840,0,6),(19,'VN082311-0040',30,30,6),(20,'VN270111-1260',864,0,6),(21,'VN082330-0010',1216,0,7),(22,'VN082330-0030',1024,0,7),(23,'VN229740-0540',3024,0,7),(24,'VN229740-0680TS',9072,0,7),(25,'VN229740-0620',6048,0,8),(26,'VN012025-0170',1600,0,9),(27,'VN012036-0090',1600,0,9),(28,'VN012036-0100',1600,0,9),(29,'VN012036-0110',9600,0,9),(30,'VN012063-0350',1750,0,9),(31,'VN012063-0380',3600,0,9),(32,'VN082323-0010',1152,0,9),(33,'VN150141-0100',3584,0,9),(34,'VN150146-0020',3584,0,9),(35,'VN082021-0110',504,0,10),(36,'VN150111-0381',600,0,10),(37,'VN012026-0020',5500,5500,11),(38,'VN012026-0050',1500,1500,11),(39,'VN012075-0050',2400,2400,11),(40,'VN082066-1470',3200,3200,11),(41,'VN082066-1500',2700,2700,11),(42,'VN082066-1560',8000,8000,11),(43,'VN082341-0010',320,0,11),(44,'VN079651-2020',1500,0,12),(45,'VN079651-2180',2000,0,12),(46,'VN079651-2250',15360,0,12),(47,'VN230517-0370',6600,0,12),(48,'VN230517-0380',6600,0,12),(49,'VN230551-0170',7680,0,12),(50,'VN230551-0180',7680,0,12),(51,'VN082028-0350',2000,2000,13),(52,'VN082042-0070',4000,4000,13),(53,'VN229739-0060',10000,0,13),(54,'VN229767-0130',10000,0,13),(55,'VN150111-0381',300,0,14),(56,'VN012020-0380',1296,0,15),(57,'VN082042-0070',4000,4000,15),(58,'VN150111-0381',300,0,16),(59,'VN270133-0010',800,800,17),(60,'VN270135-0011',3000,3000,17),(61,'VN150111-0381',600,0,18),(62,'VN082066-1510',2700,2700,19),(63,'VN082066-1570',4500,4500,19),(64,'VN082066-1580',1800,1800,19),(65,'VN082341-0010',320,0,19),(66,'VN150155-0120',1200,1200,19),(67,'VN012035-0040',25600,0,20),(68,'VN012035-0060',3200,0,20),(69,'VN012035-0080',57600,0,20),(70,'VN079667-0290',6000,0,20),(71,'VN079667-0300',96000,0,20),(72,'VN079667-0310',96000,0,20),(73,'VN082041-0150',17000,0,20),(74,'VN082041-0160',14280,0,20),(75,'VN229751-0280',43200,0,20),(76,'VN229761-0120',72000,0,20),(77,'VN012025-0290',3024,0,21),(78,'VN270133-0010',400,400,21),(79,'VN012020-0380',1296,0,22),(80,'VN012075-0040',2000,2000,22),(81,'VN079634-0120',1500,1500,22),(82,'VN079659-0090',7500,7500,22),(83,'VN082028-0350',2000,2000,22),(84,'VN082042-0070',4000,4000,22),(85,'VN150163-0010',5000,5000,22),(86,'VN229739-0060',10000,10000,22),(87,'VN229767-0130',10000,10000,22),(88,'VN150111-0381',300,0,23),(89,'HV012020-0420',3456,0,24),(90,'HV079640-0730',3840,3840,24),(91,'HV079640-0830',3840,3840,24),(92,'HV079640-0850',3840,0,24),(93,'HV079640-0870',7680,0,24),(94,'HV079640-1140',3840,3840,24),(95,'HV079640-1190',6720,6720,24),(96,'HV192300-5010',576,576,24),(97,'HV192300-5050',2304,0,24),(98,'HV192300-7041',4608,0,24),(99,'HV230440-0020',9720,9720,24),(100,'HV270150-0010',4320,0,24),(101,'VN079610-1380',13440,0,25),(102,'VN079653-0070',18000,0,26),(103,'VN079659-0100',20000,0,26),(104,'VN082364-0010',8000,0,26),(105,'VN230539-0060',10000,0,26),(106,'VN012025-0310',1440,0,27),(107,'VN012025-0340',3840,0,27),(108,'VN012064-0150',2688,0,27),(109,'VN012064-0160',2688,0,27),(110,'VN270111-1620',2880,0,27),(111,'VN198812-8750',1134,0,28),(112,'VN198812-8880',378,0,28),(113,'VN198812-8961',378,0,28),(114,'VN198812-9010',1512,0,28),(115,'VN198812-9060',324,0,28),(116,'VN198812-9100',324,0,28),(117,'VN198812-9110',378,0,28),(118,'VN270112-1510',378,0,28),(119,'VN079617-1990',6720,0,29),(120,'VN079617-2030',840,0,29),(121,'VN079617-2250',1680,0,29),(122,'VN229717-0760',4480,0,29),(123,'VN229732-0450',9216,0,29),(124,'VN229781-0080',14400,0,30),(125,'VN082035-0050',5000,0,31),(126,'VN198827-7000',2000,0,31),(127,'VN270133-0010',400,0,31),(128,'VN198811-7450',2430,0,32),(129,'VN270133-0010',400,0,33),(130,'VN150111-0381',600,0,34),(131,'VN082031-0080',12000,0,35),(132,'VN082031-0090',14400,0,35),(133,'VN082031-0101',7200,0,35),(134,'VN229861-3680',3072,0,36),(135,'VN079649-0290',180000,0,37),(136,'VN082351-0010',14872,0,37),(137,'VN082352-0010',16000,0,37),(138,'VN082352-0020',16800,0,37),(139,'VN082353-0010',14080,0,37),(140,'VN082353-0020',8960,0,37),(141,'VN139729-0140',2500,0,37),(142,'VN229771-0040',174000,0,37),(143,'VN229771-0080',168000,0,37),(144,'VN229771-0090',342000,0,37),(145,'VN012031-0040',12000,0,38),(146,'VN012031-0130',6912,0,38),(147,'VN012031-0140',25344,0,38),(148,'VN079610-1230',16170,0,39),(149,'VN079651-2430',14560,0,39),(150,'VN079651-2440',10080,0,39),(151,'VN082053-0160',9600,0,39),(152,'VN082054-0090',9216,0,39),(153,'VN082055-0200',11520,0,39),(154,'VN082056-0120',21600,0,39),(155,'VN082056-0131',8640,0,39),(156,'VN150151-0220',10320,0,39),(157,'VN150151-0350',960,0,39),(158,'VN198814-7100',6000,0,39),(159,'VN198814-7110',7500,0,39),(160,'VN230411-0020',27648,0,39),(161,'VN230517-0431',8640,0,39),(162,'VN230517-0441',6480,0,39),(163,'VN234056-2360',21000,0,39),(164,'VN150151-0150',1260,0,40),(165,'VN012020-0290',300,300,41),(166,'VN012025-0090',3000,3000,41),(167,'VN270133-0010',400,400,41),(168,'VN198811-7450',2430,0,42),(169,'VN012020-0290',1500,1500,43),(170,'VN012025-0101',1600,1600,43),(171,'VN012025-0290',3024,0,43),(172,'VN082035-0050',10000,10000,43),(173,'VN082081-0010',5000,5000,43),(174,'VN198827-7000',2000,2000,43),(175,'VN270133-0010',400,400,43),(176,'VN198811-7450',4860,0,44),(177,'VN150111-0381',600,0,45),(178,'VN012020-0380',2592,0,46),(179,'VN012075-0040',2000,2000,46),(180,'VN082028-0340',4000,4000,46),(181,'VN082042-0070',8000,8000,46),(182,'VN230431-0010',5400,0,46),(183,'VN012036-0110',9600,0,47),(184,'VN012063-0350',1750,0,47),(185,'VN012063-0380',3600,0,47),(186,'VN012063-0430',4800,0,47),(187,'VN082323-0010',576,0,47),(188,'VN150141-0100',3584,0,47),(189,'VN150141-0221',896,0,47),(190,'VN150146-0020',3584,0,47),(191,'VN150146-0070',896,0,47),(192,'VN082330-0010',576,0,48),(193,'VN229740-0680TS',9072,0,48),(194,'VN229740-0730',3024,0,48),(195,'VN229740-0620',6048,0,49);
/*!40000 ALTER TABLE `actualdetailtlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actualreceivedtlip`
--

DROP TABLE IF EXISTS `actualreceivedtlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actualreceivedtlip` (
  `ActualReceivedID` int NOT NULL AUTO_INCREMENT,
  `ActualDeliveryTime` datetime NOT NULL,
  `ActualLeadTime` time DEFAULT NULL,
  `SupplierCode` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `AsnNumber` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `DoNumber` varchar(100) DEFAULT NULL,
  `Invoice` varchar(100) DEFAULT NULL,
  `IsCompleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`ActualReceivedID`),
  KEY `SupplierCode` (`SupplierCode`),
  CONSTRAINT `actualreceivedtlip_ibfk_1` FOREIGN KEY (`SupplierCode`) REFERENCES `supplier` (`SupplierCode`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actualreceivedtlip`
--

LOCK TABLES `actualreceivedtlip` WRITE;
/*!40000 ALTER TABLE `actualreceivedtlip` DISABLE KEYS */;
INSERT INTO `actualreceivedtlip` VALUES (1,'2024-11-04 15:39:00','00:06:37','A02','M4Y04K1M02A02','41104088','',1),(2,'2024-11-04 15:40:00','00:00:45','A01','A4Y04K1M03A01','41104087','',1),(3,'2024-11-04 15:40:00','00:04:04','A01','O4Y04K1M04A01','41104087','',1),(4,'2024-11-04 15:40:00','00:03:07','A01','O4Y04K1M05A01','41104090','',1),(5,'2024-11-04 15:40:00','00:00:30','A03','O4Y01K1M07A03','41104050','',1),(6,'2024-11-04 18:31:00','00:40:22','E25','M4Y02K1M02E25','41104045','',1),(7,'2024-11-04 18:33:00','00:40:34','A12','M4Y04K1M01A12','41104018','',1),(8,'2024-11-04 18:33:00','00:38:54','A12','S4Y04K1M01A12','41104018','',1),(9,'2024-11-04 18:33:00','00:44:14','A06','M4Y04K1M01A06','41104015','',1),(10,'2024-11-04 19:20:00','00:02:21','A03','O4Y01K1M01A03','41104058','',1),(11,'2024-11-04 19:21:00','00:02:24','A02','N4Y04K1M02A02','41104089','',1),(12,'2024-11-04 20:23:00','00:02:45','E23','M4Y04K1M01E23','41104031','',1),(13,'2024-11-04 22:57:00','00:04:50','A02','M4Y04K1M03A02','41104095','',1),(14,'2024-11-04 23:09:00','00:01:03','A03','O4Y01K1M02A03','41104053','',1),(15,'2024-11-05 05:03:00','00:02:47','A02','M4Y05K1M01A02','41105060','',1),(16,'2024-11-05 05:04:00','00:01:00','A03','O4Y04K1M02A03','41105031','',1),(17,'2024-11-05 08:20:00','02:43:09','A01','O4Y05K1M01A01','41105059','',1),(18,'2024-11-05 08:20:00','00:00:49','A03','O4Y04K1M03A03','41105033','',1),(19,'2024-11-05 08:20:00','02:42:58','A02','N4Y05K1M01A02','41105062','',1),(20,'2024-11-05 09:21:00','01:36:02','I05','','','24100524',1),(21,'2024-11-05 11:02:00','00:22:36','A01','O4Y05K1M02A01','41105061','',1),(22,'2024-11-05 11:02:00','00:23:02','A02','M4Y05K1M02A02','41105064','',1),(23,'2024-11-05 11:02:00','00:00:42','A03','O4Y04K1M04A03','41105036','',1),(24,'2024-11-05 11:26:00','00:30:32','D04','','41105014','',1),(25,'2024-11-05 11:56:00','00:34:23','E12','S4Y04K1M01E12','41105018','',1),(26,'2024-11-05 11:57:00','00:34:39','E09','M4Y01K1M01E09','41105017','',1),(27,'2024-11-05 11:57:00','00:40:40','E25','M4Y04K1M04E25','41105029','',1),(28,'2024-11-05 11:58:00','00:36:33','E22','M4Y05K1M01E22','41105020','',1),(29,'2024-11-05 12:38:00','00:02:21','E15','S4Y05K1M01E15','41105037','',1),(30,'2024-11-05 12:38:00','00:01:05','E19','M4Y04K1M04E19','41105032','',1),(31,'2024-11-05 14:18:00','00:01:21','A01','O4Y05K1M03A01','41105063','',1),(32,'2024-11-05 14:18:00','00:00:43','A01','A4Y05K1M02A01','41105069','',1),(33,'2024-11-05 14:18:00','00:02:48','A01','O4Y05K1M05A01','41105069','',1),(34,'2024-11-05 14:18:00','00:00:22','A03','O4Y04K1M05A03','41105038','',1),(35,'2024-11-05 14:28:00','00:08:58','E14','M4Y01K1M01E14','41105002','',1),(36,'2024-11-05 14:28:00','00:09:36','E27','S4Y01K1M01E27','41105004','',1),(37,'2024-11-05 14:31:00','02:16:17','E30','M4X22K1M01E30','41105005','',1),(38,'2024-11-05 14:31:00','00:05:07','E26','M4X31K1M01E26','41105003','',1),(39,'2024-11-05 15:41:00','00:47:50','E02','M4Y01K1M01E02','41105015','',1),(40,'2024-11-05 15:41:00','00:50:21','E02','S4Y01K1M01E02','41105015','',1),(41,'2024-11-05 16:23:00','00:07:31','A01','O4Y05K1M07A01','41105078','',1),(42,'2024-11-05 16:23:00','00:15:31','A01','A4Y05K1M04A01','41105078','',1),(43,'2024-11-05 16:23:00','00:05:22','A01','O4Y05K1M06A01','41105075','',1),(44,'2024-11-05 16:24:00','00:15:15','A01','A4Y05K1M03A01','41105074','',1),(45,'2024-11-05 16:24:00','00:07:49','A03','O4Y04K1M06A03','41105040','',1),(46,'2024-11-05 16:25:00','00:04:41','A02','M4Y05K1M03A02','41105070','',1),(47,'2024-11-05 16:49:00','00:05:37','A06','M4Y05K1M01A06','41105039','',1),(48,'2024-11-05 16:50:00','00:02:28','A12','M4Y05K1M01A12','41105010','',1),(49,'2024-11-05 16:50:00','00:01:26','A12','S4Y05K1M01A12','41105010','',1);
/*!40000 ALTER TABLE `actualreceivedtlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actualsissuestlip`
--

DROP TABLE IF EXISTS `actualsissuestlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actualsissuestlip` (
  `ActualID` int NOT NULL AUTO_INCREMENT,
  `PlanDetailID` int NOT NULL,
  `ActualTime` datetime NOT NULL,
  PRIMARY KEY (`ActualID`),
  KEY `PlanDetailID` (`PlanDetailID`),
  CONSTRAINT `actualsissuestlip_ibfk_1` FOREIGN KEY (`PlanDetailID`) REFERENCES `planritddetails` (`PlanDetailID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actualsissuestlip`
--

LOCK TABLES `actualsissuestlip` WRITE;
/*!40000 ALTER TABLE `actualsissuestlip` DISABLE KEYS */;
/*!40000 ALTER TABLE `actualsissuestlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actualsreceivedenso`
--

DROP TABLE IF EXISTS `actualsreceivedenso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actualsreceivedenso` (
  `ActualID` int NOT NULL AUTO_INCREMENT,
  `PlanDetailID` int NOT NULL,
  `ActualTime` datetime NOT NULL,
  PRIMARY KEY (`ActualID`),
  KEY `PlanDetailID` (`PlanDetailID`),
  CONSTRAINT `actualsreceivedenso_ibfk_1` FOREIGN KEY (`PlanDetailID`) REFERENCES `planritddetails` (`PlanDetailID`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actualsreceivedenso`
--

LOCK TABLES `actualsreceivedenso` WRITE;
/*!40000 ALTER TABLE `actualsreceivedenso` DISABLE KEYS */;
INSERT INTO `actualsreceivedenso` VALUES (1,1,'2024-10-12 01:00:00'),(2,2,'2024-10-12 02:30:00'),(3,3,'2024-10-12 04:00:00'),(4,4,'2024-10-12 05:00:00'),(5,5,'2024-10-12 06:30:00'),(6,6,'2024-10-12 08:30:00'),(7,7,'2024-10-12 09:00:00'),(8,8,'2024-10-12 10:00:00'),(9,9,'2024-10-12 11:00:00'),(10,10,'2024-10-12 12:00:00'),(11,11,'2024-10-12 13:30:00'),(12,12,'2024-10-12 14:00:00'),(13,13,'2024-10-12 15:00:00'),(14,14,'2024-10-12 16:30:00'),(15,15,'2024-10-12 18:30:00'),(16,16,'2024-10-12 19:00:00'),(17,17,'2024-10-12 19:00:00'),(18,18,'2024-10-12 20:00:00'),(19,19,'2024-10-12 21:00:00'),(20,20,'2024-10-12 22:00:00'),(21,1,'2024-10-13 01:00:00'),(22,2,'2024-10-13 02:30:00'),(23,3,'2024-10-13 04:00:00'),(24,4,'2024-10-13 05:00:00'),(25,5,'2024-10-13 06:30:00'),(26,6,'2024-10-13 08:30:00'),(27,7,'2024-10-13 09:00:00'),(28,8,'2024-10-13 10:00:00'),(29,9,'2024-10-13 11:00:00'),(30,10,'2024-10-13 12:00:00'),(31,11,'2024-10-13 13:30:00'),(32,12,'2024-10-13 14:00:00'),(33,13,'2024-10-13 15:00:00'),(34,14,'2024-10-13 16:30:00'),(35,15,'2024-10-13 18:30:00'),(36,16,'2024-10-13 19:00:00'),(37,17,'2024-10-13 19:00:00'),(38,18,'2024-10-13 20:00:00'),(39,19,'2024-10-13 21:00:00'),(40,20,'2024-10-13 22:00:00');
/*!40000 ALTER TABLE `actualsreceivedenso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('f1e6d175-cf9e-4344-ac6e-c6fbdc9c6a55','123@123.com','123@123.COM','123@123.com','123@123.COM',1,'AQAAAAIAAYagAAAAEO/7wKPwuZQS3CMxQKTxsu8oC0f1ywyTk4KFrRp4MtlId6hmvA6q5kkTRQa62wJqkA==','TCZAS6ODQ423BBIOWAJBKCLGGAU7SRX4','d6175e19-6772-4244-9b84-7abd6725e974',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `historyplanreceivedtlip`
--

DROP TABLE IF EXISTS `historyplanreceivedtlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `historyplanreceivedtlip` (
  `HistoryID` int NOT NULL AUTO_INCREMENT,
  `PlanDetailID` int DEFAULT NULL,
  `ActualReceivedID` int DEFAULT NULL,
  `HistoryDate` date DEFAULT NULL,
  PRIMARY KEY (`HistoryID`),
  KEY `PlanDetailID` (`PlanDetailID`),
  KEY `ActualReceivedID` (`ActualReceivedID`),
  CONSTRAINT `historyplanreceivedtlip_ibfk_1` FOREIGN KEY (`PlanDetailID`) REFERENCES `plandetailreceivedtlip` (`PlanDetailID`),
  CONSTRAINT `historyplanreceivedtlip_ibfk_2` FOREIGN KEY (`ActualReceivedID`) REFERENCES `actualreceivedtlip` (`ActualReceivedID`)
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `historyplanreceivedtlip`
--

LOCK TABLES `historyplanreceivedtlip` WRITE;
/*!40000 ALTER TABLE `historyplanreceivedtlip` DISABLE KEYS */;
INSERT INTO `historyplanreceivedtlip` VALUES (1,NULL,1,'2024-11-04'),(2,NULL,2,'2024-11-04'),(3,NULL,3,'2024-11-04'),(4,NULL,4,'2024-11-04'),(5,NULL,5,'2024-11-04'),(6,NULL,6,'2024-11-04'),(7,NULL,7,'2024-11-04'),(8,NULL,8,'2024-11-04'),(9,NULL,9,'2024-11-04'),(10,NULL,10,'2024-11-04'),(11,NULL,11,'2024-11-04'),(12,NULL,12,'2024-11-04'),(13,NULL,13,'2024-11-04'),(14,NULL,14,'2024-11-04'),(15,NULL,15,'2024-11-05'),(16,NULL,16,'2024-11-05'),(17,1,NULL,'2024-11-04'),(18,2,NULL,'2024-11-04'),(19,3,NULL,'2024-11-04'),(20,4,NULL,'2024-11-04'),(21,5,NULL,'2024-11-04'),(22,6,NULL,'2024-11-04'),(23,7,NULL,'2024-11-04'),(24,8,NULL,'2024-11-04'),(25,9,NULL,'2024-11-04'),(26,10,NULL,'2024-11-04'),(27,11,NULL,'2024-11-04'),(28,12,NULL,'2024-11-04'),(29,13,NULL,'2024-11-04'),(30,14,NULL,'2024-11-04'),(31,15,NULL,'2024-11-04'),(32,16,NULL,'2024-11-04'),(33,17,NULL,'2024-11-04'),(34,18,NULL,'2024-11-04'),(35,19,NULL,'2024-11-04'),(36,20,NULL,'2024-11-04'),(37,21,NULL,'2024-11-04'),(38,22,NULL,'2024-11-04'),(39,23,NULL,'2024-11-04'),(40,24,NULL,'2024-11-04'),(41,25,NULL,'2024-11-04'),(42,26,NULL,'2024-11-04'),(43,27,NULL,'2024-11-04'),(44,28,NULL,'2024-11-04'),(45,29,NULL,'2024-11-04'),(46,30,NULL,'2024-11-04'),(47,31,NULL,'2024-11-04'),(48,32,NULL,'2024-11-04'),(49,33,NULL,'2024-11-04'),(50,34,NULL,'2024-11-04'),(51,35,NULL,'2024-11-04'),(52,36,NULL,'2024-11-04'),(53,37,NULL,'2024-11-04'),(54,38,NULL,'2024-11-04'),(55,39,NULL,'2024-11-04'),(56,40,NULL,'2024-11-04'),(57,41,NULL,'2024-11-04'),(58,42,NULL,'2024-11-04'),(59,43,NULL,'2024-11-04'),(60,44,NULL,'2024-11-04'),(61,45,NULL,'2024-11-04'),(62,46,NULL,'2024-11-04'),(63,47,NULL,'2024-11-04'),(64,48,NULL,'2024-11-04'),(65,49,NULL,'2024-11-04'),(66,50,NULL,'2024-11-04'),(67,51,NULL,'2024-11-04'),(68,52,NULL,'2024-11-04'),(69,53,NULL,'2024-11-04'),(70,NULL,17,'2024-11-05'),(71,NULL,18,'2024-11-05'),(72,NULL,19,'2024-11-05'),(73,NULL,20,'2024-11-05'),(74,NULL,21,'2024-11-05'),(75,NULL,22,'2024-11-05'),(76,NULL,23,'2024-11-05'),(77,NULL,24,'2024-11-05'),(78,NULL,25,'2024-11-05'),(79,NULL,26,'2024-11-05'),(80,NULL,27,'2024-11-05'),(81,NULL,28,'2024-11-05'),(82,NULL,29,'2024-11-05'),(83,NULL,30,'2024-11-05'),(84,NULL,31,'2024-11-05'),(85,NULL,32,'2024-11-05'),(86,NULL,33,'2024-11-05'),(87,NULL,34,'2024-11-05'),(88,NULL,35,'2024-11-05'),(89,NULL,36,'2024-11-05'),(90,NULL,37,'2024-11-05'),(91,NULL,38,'2024-11-05'),(92,NULL,39,'2024-11-05'),(93,NULL,40,'2024-11-05'),(94,NULL,41,'2024-11-05'),(95,NULL,42,'2024-11-05'),(96,NULL,43,'2024-11-05'),(97,NULL,44,'2024-11-05'),(98,NULL,45,'2024-11-05'),(99,NULL,46,'2024-11-05'),(100,NULL,47,'2024-11-05'),(101,NULL,48,'2024-11-05'),(102,NULL,49,'2024-11-05');
/*!40000 ALTER TABLE `historyplanreceivedtlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plandetailreceivedtlip`
--

DROP TABLE IF EXISTS `plandetailreceivedtlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plandetailreceivedtlip` (
  `PlanDetailID` int NOT NULL AUTO_INCREMENT,
  `PlanID` int NOT NULL,
  `SupplierCode` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `DeliveryTime` time NOT NULL,
  `WeekdayID` int NOT NULL,
  `LeadTime` time NOT NULL,
  `PlanType` enum('Weekly','Monthly','Bi-Monthly') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `WeekOfMonth` int DEFAULT NULL,
  `OccurrencesPerMonth` int DEFAULT NULL,
  PRIMARY KEY (`PlanDetailID`),
  KEY `SupplierCode` (`SupplierCode`),
  KEY `WeekdayID` (`WeekdayID`),
  KEY `PlanID` (`PlanID`),
  CONSTRAINT `plandetailreceivedtlip_ibfk_1` FOREIGN KEY (`SupplierCode`) REFERENCES `supplier` (`SupplierCode`),
  CONSTRAINT `plandetailreceivedtlip_ibfk_2` FOREIGN KEY (`WeekdayID`) REFERENCES `weekday` (`WeekdayID`),
  CONSTRAINT `plandetailreceivedtlip_ibfk_3` FOREIGN KEY (`PlanID`) REFERENCES `planreceivetlip` (`PlanID`)
) ENGINE=InnoDB AUTO_INCREMENT=300 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plandetailreceivedtlip`
--

LOCK TABLES `plandetailreceivedtlip` WRITE;
/*!40000 ALTER TABLE `plandetailreceivedtlip` DISABLE KEYS */;
INSERT INTO `plandetailreceivedtlip` VALUES (1,1,'A01','07:35:00',1,'01:00:00','Weekly',NULL,NULL),(2,1,'A02','07:35:00',1,'01:00:00','Weekly',NULL,NULL),(3,1,'A03','07:35:00',1,'01:00:00','Weekly',NULL,NULL),(4,1,'G66/G68','06:00:00',1,'04:00:00','Weekly',NULL,NULL),(5,1,'E25','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(6,1,'D04','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(7,1,'TD60','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(8,1,'E12','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(9,1,'A08','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(10,1,'E01','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(11,1,'E22','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(12,1,'A07','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(13,1,'B35','10:35:00',1,'02:00:00','Weekly',NULL,NULL),(14,1,'A01','11:05:00',1,'00:30:00','Weekly',NULL,NULL),(15,1,'A02','11:05:00',1,'00:30:00','Weekly',NULL,NULL),(16,1,'A03','11:05:00',1,'00:30:00','Weekly',NULL,NULL),(17,1,'E15','13:00:00',1,'01:00:00','Weekly',NULL,NULL),(18,1,'E19','13:00:00',1,'01:00:00','Weekly',NULL,NULL),(19,1,'E05','13:00:00',1,'01:00:00','Weekly',NULL,NULL),(20,1,'E20','13:00:00',1,'01:00:00','Weekly',NULL,NULL),(21,1,'A01','13:30:00',1,'00:30:00','Weekly',NULL,NULL),(22,1,'A02','13:30:00',1,'00:30:00','Weekly',NULL,NULL),(23,1,'A03','13:30:00',1,'00:30:00','Weekly',NULL,NULL),(24,1,'I04','14:30:00',1,'02:00:00','Monthly',2,NULL),(25,1,'A01','15:30:00',1,'00:30:00','Weekly',NULL,NULL),(26,1,'A02','15:30:00',1,'00:30:00','Weekly',NULL,NULL),(27,1,'A03','15:30:00',1,'00:30:00','Weekly',NULL,NULL),(28,1,'A01','16:30:00',1,'00:30:00','Weekly',NULL,NULL),(29,1,'A02','16:30:00',1,'00:30:00','Weekly',NULL,NULL),(30,1,'A03','16:30:00',1,'00:30:00','Weekly',NULL,NULL),(31,1,'E15','16:30:00',1,'01:00:00','Weekly',NULL,NULL),(32,1,'D04','16:30:00',1,'01:00:00','Weekly',NULL,NULL),(33,1,'TD60','16:30:00',1,'01:00:00','Weekly',NULL,NULL),(34,1,'D04','18:00:00',1,'01:30:00','Weekly',3,NULL),(35,1,'TD60','18:00:00',1,'01:30:00','Weekly',3,NULL),(36,1,'A01','19:00:00',1,'00:30:00','Weekly',NULL,NULL),(37,1,'A02','19:00:00',1,'00:30:00','Weekly',NULL,NULL),(38,1,'A03','19:00:00',1,'00:30:00','Weekly',NULL,NULL),(39,1,'A01','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(40,1,'A02','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(41,1,'A03','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(42,1,'D04','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(43,1,'TD60','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(44,1,'E25','19:30:00',1,'01:00:00','Weekly',NULL,NULL),(45,1,'A01','23:30:00',1,'00:30:00','Weekly',NULL,NULL),(46,1,'A02','23:30:00',1,'00:30:00','Weekly',NULL,NULL),(47,1,'A03','23:30:00',1,'00:30:00','Weekly',NULL,NULL),(48,1,'A01','02:30:00',1,'00:30:00','Weekly',NULL,NULL),(49,1,'A02','02:30:00',1,'00:30:00','Weekly',NULL,NULL),(50,1,'A03','02:30:00',1,'00:30:00','Weekly',NULL,NULL),(51,1,'A01','05:20:00',1,'00:30:00','Weekly',NULL,NULL),(52,1,'A02','05:20:00',1,'00:30:00','Weekly',NULL,NULL),(53,1,'A03','05:20:00',1,'00:30:00','Weekly',NULL,NULL),(54,1,'I24','06:00:00',2,'02:00:00','Monthly',1,NULL),(55,1,'I06','06:00:00',2,'02:00:00','Monthly',1,NULL),(56,1,'I14','06:00:00',2,'02:00:00','Monthly',1,NULL),(57,1,'I18','06:00:00',2,'02:00:00','Monthly',1,NULL),(58,1,'A01','07:30:00',2,'01:00:00','Weekly',NULL,NULL),(59,1,'A02','07:00:00',2,'01:00:00','Weekly',NULL,NULL),(60,1,'A03','07:00:00',2,'01:00:00','Weekly',NULL,NULL),(61,1,'I05','08:00:00',2,'02:00:00','Weekly',NULL,NULL),(62,1,'E25','10:30:00',2,'02:00:00','Weekly',NULL,NULL),(63,1,'D04','10:30:00',2,'02:00:00','Weekly',NULL,NULL),(64,1,'E12','10:30:00',2,'02:00:00','Weekly',NULL,NULL),(65,1,'E22','10:30:00',2,'02:00:00','Weekly',NULL,NULL),(66,1,'A01','11:00:00',2,'00:30:00','Weekly',NULL,NULL),(67,1,'A02','11:00:00',2,'00:30:00','Weekly',NULL,NULL),(68,1,'A03','11:00:00',2,'00:30:00','Weekly',NULL,NULL),(69,1,'E15','13:00:00',2,'01:00:00','Weekly',NULL,NULL),(70,1,'E19','13:00:00',2,'01:00:00','Weekly',NULL,NULL),(71,1,'A01','13:30:00',2,'00:30:00','Weekly',NULL,NULL),(72,1,'A02','13:30:00',2,'00:30:00','Weekly',NULL,NULL),(73,1,'A03','13:30:00',2,'00:30:00','Weekly',NULL,NULL),(74,1,'E27','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(75,1,'E13','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(76,1,'E14','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(77,1,'E18','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(78,1,'E03','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(79,1,'E02','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(80,1,'E04','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(81,1,'E30','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(82,1,'E26','14:30:00',2,'01:30:00','Weekly',NULL,NULL),(83,1,'E09','15:00:00',2,'01:00:00','Weekly',NULL,NULL),(84,1,'E24','15:00:00',2,'01:00:00','Weekly',NULL,NULL),(85,1,'A01','15:30:00',2,'00:30:00','Weekly',NULL,NULL),(86,1,'A02','15:30:00',2,'00:30:00','Weekly',NULL,NULL),(87,1,'A03','15:30:00',2,'00:30:00','Weekly',NULL,NULL),(88,1,'D04','16:00:00',2,'01:00:00','Weekly',NULL,NULL),(89,1,'E15','16:00:00',2,'01:00:00','Weekly',NULL,NULL),(90,1,'A01','16:30:00',2,'00:30:00','Weekly',NULL,NULL),(91,1,'A02','16:30:00',2,'00:30:00','Weekly',NULL,NULL),(92,1,'A03','16:30:00',2,'00:30:00','Weekly',NULL,NULL),(93,1,'I15','18:00:00',2,'01:30:00','Monthly',2,NULL),(94,1,'A01','19:00:00',2,'00:30:00','Weekly',NULL,NULL),(95,1,'A02','19:00:00',2,'00:30:00','Weekly',NULL,NULL),(96,1,'A03','19:00:00',2,'00:30:00','Weekly',NULL,NULL),(97,1,'A01','19:30:00',2,'01:00:00','Weekly',NULL,NULL),(98,1,'A02','19:30:00',2,'01:00:00','Weekly',NULL,NULL),(99,1,'A03','19:30:00',2,'01:00:00','Weekly',NULL,NULL),(100,1,'D04','19:30:00',2,'01:00:00','Weekly',NULL,NULL),(101,1,'E25','19:30:00',2,'01:00:00','Weekly',NULL,NULL),(102,1,'A01','23:30:00',2,'00:30:00','Weekly',NULL,NULL),(103,1,'A02','23:30:00',2,'00:30:00','Weekly',NULL,NULL),(104,1,'A03','23:30:00',2,'00:30:00','Weekly',NULL,NULL),(105,1,'G66/G68','03:00:00',2,'03:00:00','Weekly',NULL,NULL),(106,1,'A01','02:30:00',2,'00:30:00','Weekly',NULL,NULL),(107,1,'A02','02:30:00',2,'00:30:00','Weekly',NULL,NULL),(108,1,'A03','02:30:00',2,'00:30:00','Weekly',NULL,NULL),(109,1,'A01','05:20:00',2,'00:30:00','Weekly',NULL,NULL),(110,1,'A02','05:20:00',2,'00:30:00','Weekly',NULL,NULL),(111,1,'A03','05:20:00',2,'00:30:00','Weekly',NULL,NULL),(112,1,'A01','07:30:00',3,'01:00:00','Weekly',NULL,NULL),(113,1,'A02','07:30:00',3,'01:00:00','Weekly',NULL,NULL),(114,1,'A03','07:30:00',3,'01:00:00','Weekly',NULL,NULL),(115,1,'E25','10:30:00',3,'02:00:00','Weekly',NULL,NULL),(116,1,'D04','10:30:00',3,'02:00:00','Weekly',NULL,NULL),(117,1,'E12','10:30:00',3,'02:00:00','Weekly',NULL,NULL),(118,1,'E22','10:30:00',3,'02:00:00','Weekly',NULL,NULL),(119,1,'A01','11:00:00',3,'00:30:00','Weekly',NULL,NULL),(120,1,'A02','11:00:00',3,'00:30:00','Weekly',NULL,NULL),(121,1,'A03','11:00:00',3,'00:30:00','Weekly',NULL,NULL),(122,1,'E15','13:00:00',3,'01:00:00','Weekly',NULL,NULL),(123,1,'E19','13:30:00',3,'01:00:00','Weekly',NULL,NULL),(124,1,'A01','13:30:00',3,'00:30:00','Weekly',NULL,NULL),(125,1,'A02','13:30:00',3,'00:30:00','Weekly',NULL,NULL),(126,1,'A03','13:30:00',3,'00:30:00','Weekly',NULL,NULL),(127,1,'A01','15:30:00',3,'00:30:00','Weekly',NULL,NULL),(128,1,'A02','15:30:00',3,'00:30:00','Weekly',NULL,NULL),(129,1,'A03','15:30:00',3,'00:30:00','Weekly',NULL,NULL),(130,1,'D04','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(131,1,'B372','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(132,1,'E23','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(133,1,'E06','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(134,1,'E29','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(135,1,'E15','16:00:00',3,'01:30:00','Weekly',NULL,NULL),(136,1,'A01','19:00:00',3,'00:30:00','Weekly',NULL,NULL),(137,1,'A02','19:00:00',3,'00:30:00','Weekly',NULL,NULL),(138,1,'A03','19:00:00',3,'00:30:00','Weekly',NULL,NULL),(139,1,'H91','19:30:00',3,'02:30:00','Monthly',3,NULL),(140,1,'H96','19:30:00',3,'02:30:00','Monthly',2,NULL),(141,1,'G03','19:30:00',3,'02:30:00','Monthly',3,NULL),(142,1,'A01','19:30:00',3,'01:00:00','Weekly',NULL,NULL),(143,1,'A02','19:30:00',3,'01:00:00','Weekly',NULL,NULL),(144,1,'A03','19:30:00',3,'01:00:00','Weekly',NULL,NULL),(145,1,'D04','19:30:00',3,'01:00:00','Weekly',NULL,NULL),(146,1,'E25','19:30:00',3,'01:00:00','Weekly',NULL,NULL),(147,1,'I17','22:30:00',3,'02:30:00','Monthly',1,NULL),(148,1,'H93','22:30:00',3,'02:00:00','Monthly',3,NULL),(149,1,'A01','23:30:00',3,'00:30:00','Weekly',NULL,NULL),(150,1,'A02','23:30:00',3,'00:30:00','Weekly',NULL,NULL),(151,1,'A03','23:30:00',3,'00:30:00','Weekly',NULL,NULL),(152,1,'I10','00:30:00',3,'01:00:00','Monthly',3,NULL),(153,1,'G66/G68','02:00:00',3,'03:30:00','Weekly',NULL,NULL),(154,1,'A01','02:30:00',3,'00:30:00','Weekly',NULL,NULL),(155,1,'A02','02:30:00',3,'00:30:00','Weekly',NULL,NULL),(156,1,'A03','02:30:00',3,'00:30:00','Weekly',NULL,NULL),(157,1,'A01','05:20:00',3,'00:30:00','Weekly',NULL,NULL),(158,1,'A02','05:20:00',3,'00:30:00','Weekly',NULL,NULL),(159,1,'A03','05:20:00',3,'00:30:00','Weekly',NULL,NULL),(160,1,'A01','07:30:00',4,'00:30:00','Weekly',NULL,NULL),(161,1,'A02','07:30:00',4,'00:30:00','Weekly',NULL,NULL),(162,1,'A03','07:30:00',4,'00:30:00','Weekly',NULL,NULL),(163,1,'E25','10:30:00',4,'02:00:00','Weekly',NULL,NULL),(164,1,'D04','10:30:00',4,'02:00:00','Weekly',NULL,NULL),(165,1,'E12','10:30:00',4,'02:00:00','Weekly',NULL,NULL),(166,1,'E22','10:30:00',4,'02:00:00','Weekly',NULL,NULL),(167,1,'A01','11:00:00',4,'00:30:00','Weekly',NULL,NULL),(168,1,'A02','11:00:00',4,'00:30:00','Weekly',NULL,NULL),(169,1,'A03','11:00:00',4,'00:30:00','Weekly',NULL,NULL),(170,1,'E15','13:00:00',4,'01:00:00','Weekly',NULL,NULL),(171,1,'E19','13:30:00',4,'01:00:00','Weekly',NULL,NULL),(172,1,'A01','13:30:00',4,'00:30:00','Weekly',NULL,NULL),(173,1,'A02','13:30:00',4,'00:30:00','Weekly',NULL,NULL),(174,1,'A03','13:30:00',4,'00:30:00','Weekly',NULL,NULL),(175,1,'A01','15:30:00',4,'00:30:00','Weekly',NULL,NULL),(176,1,'A02','15:30:00',4,'00:30:00','Weekly',NULL,NULL),(177,1,'A03','15:30:00',4,'00:30:00','Weekly',NULL,NULL),(178,1,'D04','16:00:00',4,'01:30:00','Weekly',NULL,NULL),(179,1,'E21','16:00:00',4,'01:30:00','Weekly',NULL,NULL),(180,1,'E15','16:00:00',4,'01:30:00','Weekly',NULL,NULL),(181,1,'A01','16:30:00',4,'00:30:00','Weekly',NULL,NULL),(182,1,'A02','16:30:00',4,'00:30:00','Weekly',NULL,NULL),(183,1,'A03','16:30:00',4,'00:30:00','Weekly',NULL,NULL),(184,1,'A01','19:00:00',4,'00:30:00','Weekly',NULL,NULL),(185,1,'A02','19:00:00',4,'00:30:00','Weekly',NULL,NULL),(186,1,'A03','19:00:00',4,'00:30:00','Weekly',NULL,NULL),(187,1,'A01','19:30:00',4,'01:00:00','Weekly',NULL,NULL),(188,1,'A02','19:30:00',4,'01:00:00','Weekly',NULL,NULL),(189,1,'A03','19:30:00',4,'01:00:00','Weekly',NULL,NULL),(190,1,'D04','19:30:00',4,'01:00:00','Weekly',NULL,NULL),(191,1,'E25','19:30:00',4,'01:00:00','Weekly',NULL,NULL),(192,1,'A01','23:00:00',4,'00:30:00','Weekly',NULL,NULL),(193,1,'A02','23:00:00',4,'00:30:00','Weekly',NULL,NULL),(194,1,'A03','23:00:00',4,'00:30:00','Weekly',NULL,NULL),(195,1,'A01','02:30:00',4,'00:30:00','Weekly',NULL,NULL),(196,1,'A02','02:30:00',4,'00:30:00','Weekly',NULL,NULL),(197,1,'A03','02:30:00',4,'00:30:00','Weekly',NULL,NULL),(198,1,'A01','05:20:00',4,'00:30:00','Weekly',NULL,NULL),(199,1,'A02','05:20:00',4,'00:30:00','Weekly',NULL,NULL),(200,1,'A03','05:20:00',4,'00:30:00','Weekly',NULL,NULL),(201,1,'G05','06:00:00',5,'02:30:00','Weekly',NULL,NULL),(202,1,'A01','07:30:00',5,'01:00:00','Weekly',NULL,NULL),(203,1,'A02','07:30:00',5,'01:00:00','Weekly',NULL,NULL),(204,1,'A03','07:30:00',5,'01:00:00','Weekly',NULL,NULL),(205,1,'E25','10:30:00',5,'02:00:00','Weekly',NULL,NULL),(206,1,'D04','10:30:00',5,'02:00:00','Weekly',NULL,NULL),(207,1,'TD60','10:30:00',5,'02:00:00','Weekly',NULL,NULL),(208,1,'E12','10:30:00',5,'02:00:00','Weekly',NULL,NULL),(209,1,'E22','10:30:00',5,'02:00:00','Weekly',NULL,NULL),(210,1,'A01','11:00:00',5,'00:30:00','Weekly',NULL,NULL),(211,1,'A02','11:00:00',5,'00:30:00','Weekly',NULL,NULL),(212,1,'A03','11:00:00',5,'00:30:00','Weekly',NULL,NULL),(213,1,'E15','13:00:00',5,'01:00:00','Weekly',NULL,NULL),(214,1,'E19','13:30:00',5,'01:00:00','Weekly',NULL,NULL),(215,1,'A01','13:30:00',5,'00:30:00','Weekly',NULL,NULL),(216,1,'A02','13:30:00',5,'00:30:00','Weekly',NULL,NULL),(217,1,'A03','13:30:00',5,'00:30:00','Weekly',NULL,NULL),(218,1,'E27','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(219,1,'E13','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(220,1,'E14','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(221,1,'E18','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(222,1,'E03','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(223,1,'E02','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(224,1,'E04','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(225,1,'E30','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(226,1,'E26','14:30:00',5,'01:30:00','Weekly',NULL,NULL),(227,1,'A01','15:30:00',5,'00:30:00','Weekly',NULL,NULL),(228,1,'A02','15:30:00',5,'00:30:00','Weekly',NULL,NULL),(229,1,'A03','15:30:00',5,'00:30:00','Weekly',NULL,NULL),(230,1,'D04','16:00:00',5,'01:30:00','Weekly',NULL,NULL),(231,1,'TD60','16:00:00',5,'01:30:00','Weekly',NULL,NULL),(232,1,'E15','16:00:00',5,'01:30:00','Weekly',NULL,NULL),(233,1,'E23','16:00:00',5,'01:30:00','Weekly',NULL,NULL),(234,1,'A01','16:30:00',5,'00:30:00','Weekly',NULL,NULL),(235,1,'A02','16:30:00',5,'00:30:00','Weekly',NULL,NULL),(236,1,'A03','16:30:00',5,'00:30:00','Weekly',NULL,NULL),(237,1,'A01','19:00:00',5,'00:30:00','Weekly',NULL,NULL),(238,1,'A02','19:00:00',5,'00:30:00','Weekly',NULL,NULL),(239,1,'A03','19:00:00',5,'00:30:00','Weekly',NULL,NULL),(240,1,'A01','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(241,1,'A02','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(242,1,'A03','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(243,1,'D04','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(244,1,'TD60','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(245,1,'E25','19:30:00',5,'01:00:00','Weekly',NULL,NULL),(246,1,'B222','20:00:00',5,'02:00:00','Monthly',3,NULL),(247,1,'G02','20:00:00',5,'02:00:00','Monthly',3,NULL),(248,1,'G06','20:00:00',5,'02:00:00','Weekly',NULL,NULL),(249,1,'H98','22:30:00',5,'01:30:00','Monthly',2,NULL),(250,1,'A01','23:30:00',5,'00:30:00','Weekly',NULL,NULL),(251,1,'A02','23:30:00',5,'00:30:00','Weekly',NULL,NULL),(252,1,'A03','23:30:00',5,'00:30:00','Weekly',NULL,NULL),(253,1,'D08','00:00:00',5,'01:30:00','Weekly',NULL,NULL),(254,1,'G66/G68','00:00:00',5,'06:00:00','Weekly',NULL,NULL),(255,1,'A01','02:30:00',5,'00:30:00','Weekly',NULL,NULL),(256,1,'A02','02:30:00',5,'00:30:00','Weekly',NULL,NULL),(257,1,'A03','02:30:00',5,'00:30:00','Weekly',NULL,NULL),(258,1,'A01','05:20:00',5,'00:30:00','Weekly',NULL,NULL),(259,1,'A02','05:20:00',5,'00:30:00','Weekly',NULL,NULL),(260,1,'A03','05:20:00',5,'00:30:00','Weekly',NULL,NULL),(261,1,'A01','07:30:00',6,'01:00:00','Weekly',NULL,NULL),(262,1,'A02','07:30:00',6,'01:00:00','Weekly',NULL,NULL),(263,1,'A03','07:30:00',6,'01:00:00','Weekly',NULL,NULL),(264,1,'E25','10:30:00',6,'02:00:00','Weekly',NULL,NULL),(265,1,'D04','10:30:00',6,'02:00:00','Weekly',NULL,NULL),(266,1,'E12','10:30:00',6,'02:00:00','Weekly',NULL,NULL),(267,1,'A01','11:00:00',6,'00:30:00','Weekly',NULL,NULL),(268,1,'A02','11:00:00',6,'00:30:00','Weekly',NULL,NULL),(269,1,'A03','11:00:00',6,'00:30:00','Weekly',NULL,NULL),(270,1,'E15','13:00:00',6,'01:00:00','Weekly',NULL,NULL),(271,1,'E19','13:30:00',6,'01:00:00','Weekly',NULL,NULL),(272,1,'A01','13:30:00',6,'00:30:00','Weekly',NULL,NULL),(273,1,'A02','13:30:00',6,'00:30:00','Weekly',NULL,NULL),(274,1,'A03','13:30:00',6,'00:30:00','Weekly',NULL,NULL),(275,1,'A01','15:30:00',6,'00:30:00','Weekly',NULL,NULL),(276,1,'A02','15:30:00',6,'00:30:00','Weekly',NULL,NULL),(277,1,'A03','15:30:00',6,'00:30:00','Weekly',NULL,NULL),(278,1,'D04','16:00:00',6,'01:30:00','Weekly',NULL,NULL),(279,1,'E15','16:00:00',6,'01:30:00','Weekly',NULL,NULL),(280,1,'A01','16:30:00',6,'00:30:00','Weekly',NULL,NULL),(281,1,'A02','16:30:00',6,'00:30:00','Weekly',NULL,NULL),(282,1,'A03','16:30:00',6,'00:30:00','Weekly',NULL,NULL),(283,1,'A01','19:00:00',6,'00:30:00','Weekly',NULL,NULL),(284,1,'A02','19:00:00',6,'00:30:00','Weekly',NULL,NULL),(285,1,'A03','19:00:00',6,'00:30:00','Weekly',NULL,NULL),(286,1,'A01','19:30:00',6,'01:00:00','Weekly',NULL,NULL),(287,1,'A02','19:30:00',6,'01:00:00','Weekly',NULL,NULL),(288,1,'A03','19:30:00',6,'01:00:00','Weekly',NULL,NULL),(289,1,'D04','19:30:00',6,'01:00:00','Weekly',NULL,NULL),(290,1,'E25','19:30:00',6,'01:00:00','Weekly',NULL,NULL),(291,1,'A01','23:30:00',6,'00:30:00','Weekly',NULL,NULL),(292,1,'A02','23:30:00',6,'00:30:00','Weekly',NULL,NULL),(293,1,'A03','23:30:00',6,'00:30:00','Weekly',NULL,NULL),(294,1,'A01','02:30:00',6,'00:30:00','Weekly',NULL,NULL),(295,1,'A02','02:30:00',6,'00:30:00','Weekly',NULL,NULL),(296,1,'A03','02:30:00',6,'00:30:00','Weekly',NULL,NULL),(297,1,'A01','05:00:00',6,'00:30:00','Weekly',NULL,NULL),(298,1,'A02','05:00:00',6,'00:30:00','Weekly',NULL,NULL),(299,1,'A03','05:00:00',6,'00:30:00','Weekly',NULL,NULL);
/*!40000 ALTER TABLE `plandetailreceivedtlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `planreceivetlip`
--

DROP TABLE IF EXISTS `planreceivetlip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `planreceivetlip` (
  `PlanID` int NOT NULL AUTO_INCREMENT,
  `PlanName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EffectiveDate` date NOT NULL,
  PRIMARY KEY (`PlanID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `planreceivetlip`
--

LOCK TABLES `planreceivetlip` WRITE;
/*!40000 ALTER TABLE `planreceivetlip` DISABLE KEYS */;
INSERT INTO `planreceivetlip` VALUES (1,'Kế hoạch nhận hàng 2024','2024-10-01');
/*!40000 ALTER TABLE `planreceivetlip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `planritd`
--

DROP TABLE IF EXISTS `planritd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `planritd` (
  `PlanID` int NOT NULL AUTO_INCREMENT,
  `PlanName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PlanType` enum('Received','Issued') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TotalShipment` int NOT NULL DEFAULT '0',
  `EffectiveDate` date NOT NULL,
  PRIMARY KEY (`PlanID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `planritd`
--

LOCK TABLES `planritd` WRITE;
/*!40000 ALTER TABLE `planritd` DISABLE KEYS */;
INSERT INTO `planritd` VALUES (1,'Plan RITD','Received',20,'2024-10-03'),(2,'Plan RITD','Received',20,'2024-10-09');
/*!40000 ALTER TABLE `planritd` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `planritddetails`
--

DROP TABLE IF EXISTS `planritddetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `planritddetails` (
  `PlanDetailID` int NOT NULL AUTO_INCREMENT,
  `PlanID` int NOT NULL,
  `PlanTime` time NOT NULL,
  `PlanDetailName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StatusReceiveID` int DEFAULT NULL,
  `StatusIssueID` int DEFAULT NULL,
  PRIMARY KEY (`PlanDetailID`),
  KEY `PlanID` (`PlanID`),
  KEY `StatusReceiveID` (`StatusReceiveID`),
  KEY `StatusIssueID` (`StatusIssueID`),
  CONSTRAINT `planritddetails_ibfk_1` FOREIGN KEY (`PlanID`) REFERENCES `planritd` (`PlanID`),
  CONSTRAINT `planritddetails_ibfk_2` FOREIGN KEY (`StatusReceiveID`) REFERENCES `statusesritd` (`StatusID`),
  CONSTRAINT `planritddetails_ibfk_3` FOREIGN KEY (`StatusIssueID`) REFERENCES `statusesritd` (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `planritddetails`
--

LOCK TABLES `planritddetails` WRITE;
/*!40000 ALTER TABLE `planritddetails` DISABLE KEYS */;
INSERT INTO `planritddetails` VALUES (1,1,'00:30:00','Số 1',NULL,NULL),(2,1,'02:00:00','Số 2',NULL,NULL),(3,1,'03:40:00','Số 3',NULL,NULL),(4,1,'04:30:00','Số 4',NULL,NULL),(5,1,'06:00:00','Số 5',NULL,NULL),(6,1,'07:30:00','Số 6',NULL,NULL),(7,1,'08:30:00','Số 7',NULL,NULL),(8,1,'10:00:00','Số 8',NULL,NULL),(9,1,'10:10:00','Số 9',NULL,NULL),(10,1,'12:30:00','Số 10',NULL,NULL),(11,1,'13:30:00','Số 11',NULL,NULL),(12,1,'14:30:00','Số 12',NULL,NULL),(13,1,'15:30:00','Số 13',NULL,NULL),(14,1,'17:00:00','Số 14',NULL,NULL),(15,1,'18:30:00','Số 15',NULL,NULL),(16,1,'19:30:00','Số 16',NULL,NULL),(17,1,'20:30:00','Số 17',NULL,NULL),(18,1,'21:30:00','Số 18',NULL,NULL),(19,1,'22:30:00','Số 19',NULL,NULL),(20,1,'23:30:00','Số 20',NULL,NULL),(21,2,'00:30:00','Số 1',NULL,NULL),(22,2,'02:00:00','Số 2',NULL,NULL),(23,2,'03:40:00','Số 3',NULL,NULL),(24,2,'04:30:00','Số 4',NULL,NULL),(25,2,'06:00:00','Số 5',NULL,NULL),(26,2,'07:30:00','Số 6',NULL,NULL),(27,2,'08:30:00','Số 7',NULL,NULL),(28,2,'10:00:00','Số 8',NULL,NULL),(29,2,'10:10:00','Số 9',NULL,NULL),(30,2,'12:30:00','Số 10',NULL,NULL),(31,2,'13:30:00','Số 11',NULL,NULL),(32,2,'14:30:00','Số 12',NULL,NULL),(33,2,'15:30:00','Số 13',NULL,NULL),(34,2,'17:00:00','Số 14',NULL,NULL),(35,2,'18:30:00','Số 15',NULL,NULL),(36,2,'19:30:00','Số 16',NULL,NULL),(37,2,'20:30:00','Số 17',NULL,NULL),(38,2,'21:30:00','Số 18',NULL,NULL),(39,2,'22:30:00','Số 19',NULL,NULL),(40,2,'23:30:00','Số 20',NULL,NULL);
/*!40000 ALTER TABLE `planritddetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statusesritd`
--

DROP TABLE IF EXISTS `statusesritd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statusesritd` (
  `StatusID` int NOT NULL AUTO_INCREMENT,
  `Status` enum('Pending','In Transit','Delivered','Received') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statusesritd`
--

LOCK TABLES `statusesritd` WRITE;
/*!40000 ALTER TABLE `statusesritd` DISABLE KEYS */;
INSERT INTO `statusesritd` VALUES (1,'Pending'),(2,'In Transit'),(3,'Delivered'),(4,'Received');
/*!40000 ALTER TABLE `statusesritd` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `supplier`
--

DROP TABLE IF EXISTS `supplier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `supplier` (
  `SupplierCode` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `SupplierName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`SupplierCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `supplier`
--

LOCK TABLES `supplier` WRITE;
/*!40000 ALTER TABLE `supplier` DISABLE KEYS */;
INSERT INTO `supplier` VALUES ('A01','OHARA VIETNAM'),('A02','MATSUO VIETNAM'),('A03','HAL VIETNAM'),('A04','KEINHING VIETNAM'),('A06','NIPPO MECHATRONIC VIETNAM'),('A07','NSK VIETNAM'),('A08','FUKOKU VIETNAM'),('A09','RYHTHM KYOSHIN HN'),('A10','TOYOTA TSUSHO VN'),('A12','NIPPO MECHATRONIC VIETNAM'),('A14','Parker Processing VN'),('B04','CTY QUỐC TUẤN'),('B08','CTY THUẬN AN'),('B10','CTY TNHH THUONG MAI PHUC MA'),('B105','CTY TNHH HTECH'),('B11','CTY TNHH HOA HOC UNG DỤNG'),('B12','CTY TNHH AIR LIQUIDE VIET NAM'),('B13','CTY TNHH NGUYEN VAN THA'),('B132','CTY TNHH ANH MINH'),('B134','CTY TNHH ĐỨC MINH'),('B139','CTY TNHH VIỆT ANH'),('B159','Kura'),('B17','CTY TNHH THUONG MAI HA ANH'),('B18','CTY CP SX & DV CONG NGHIEP GMF'),('B190','IDEMITSU'),('B20','CTY TNHH DUC ANH'),('B205','TOPSUN'),('B206','TANAKA KIKINZOKU'),('B214','NISHI TOKYO CHEMIX CORPORATION'),('B222','ARMOR WEATHYSHINE'),('B228','SHINJIKI'),('B237','SUNRISE'),('B24','CTY CP CHUYEN GIAO CN TECH VINA'),('B269','Công ty TNHH PT'),('B28','CTY TNHH TM & CONG NGHE HONG DUONG'),('B30','CONG TY TNHH KSMC'),('B318','UENO'),('B32','Longson JSC'),('B35','HA SON'),('B369','CHI NHANH CTY NHẤT PHÁT TẠI HN'),('B372','CTY TNHH SERTIM'),('B71','JFE'),('D02','DENSO KOREA CORPORATION'),('D04','HDVN'),('D08','AINE'),('D09/TD20/TD23/TD24','DENSO THAILAND'),('D10/D03/D05/TD12/TD15','DNIA'),('D11','DMAT'),('D12','DENSO MANUFACTURING HUNGA'),('D13','DNMX ( Denso Mexico )'),('Di261401-5200','null'),('E01','KYOSHIN VIETNAM'),('E02','MARUEI VIETNAM PRECISION Co.,Ltd'),('E03','MEINAN VIETNAM PRECISION Co.,Ltd'),('E04','HARIKI'),('E05','ADVANEX VIETNAM'),('E06','ATARIH PRECISION'),('E09','VINA TAIYO'),('E10','FUJIKURA VN/ FUJIKURA TRADING(HCM BRANCH)'),('E12','SANWA VN'),('E13','KAWASAKI VN'),('E14','SANYO VN'),('E15','SURTECKARIYA VN'),('E16','SHOEI VN'),('E17','TOPY FASTERNER VN'),('E18','KURABE INDUCTRIES VN'),('E19','NOGUCHI VIETNAM --> Fertile VN'),('E20','CURIOUS SEIKI VN'),('E21','(TOYO) DRILIBE VN'),('E22','OHASHI TEKKO VIETNAM'),('E23','Micro Techno'),('E24','DAITO RUBBER'),('E25','KURODA VN'),('E26','MABUCHI VN'),('E27','CHUBURIKA VIETNAM'),('E28','FUJIKURA VN/ FUJIKURA TRADING(HCM BRANCH)'),('E29','Vietinak VN'),('E30','Viet nam Nok co, LTD'),('E31','null'),('E32','Toyo Brazing VN'),('FT01','DENSO SALE THAI (TRADING APM6)'),('FT03','NIPPO'),('G01','NOK THAI'),('G02','HITACHI METALS THAI'),('G03','SWS THAILAND'),('G05','YAMADA SOMBOON'),('G06','DAIKI THAILAND'),('G66/G68','DENSO SALE THAI'),('H78/H84/H88','TOYOTA TSUSHO JAPAN 1 ( FIRMED PERIOD 2 WEEK)'),('H79','TOYOTA TSUSHO JAPAN 2 ( FIRMED PERIOD 4 WEEK)'),('H82','TOYOTA TSUSHO JAPAN 3 ( FIRMED PERIOD 6 WEEK)'),('H85','NAGASE CO.,Ltd'),('H87','TSUCHIYA'),('H89','TOYOTA TSUSHO JAPAN 7 (MASUYASU)'),('H91','OKAYA CO , . LTD'),('H92','IDAKA'),('H93','SANKO'),('H94','TSUCHIYA PARTS'),('H95','KONISHI Co.,LTD'),('H96','ITOMOL'),('H98','MITSUBISHI'),('I02','DAIMET KLANG'),('I03','NOK SINGAPORE'),('I04','HOKURIKU DENSO SINGAPORE'),('I05','NTA MACHINING STD BHD'),('I06','HOINAK LIMITTED'),('I07','ASAHI KOSEI (M) SDN.BHD.CO'),('I10','SCHAEFFLER (SINGAPORE)'),('I12','NITTO SHOJI LIMITTED'),('I14','HIRATA (SHENZHEN) CO.,LTD'),('I15','SHANGHAI MIKURO SPRING'),('I17','NISSIN KOGYO'),('I18','TDK HONGKONG'),('I20','SUMITOMO ELECTRIC ASIA PA'),('I21','GSE CAMBODIA'),('I23','Scherdel'),('I24','JONHSON ELECTRIC WORLD TRADE COMPANY'),('J01','DNJP'),('J01.1','DNJP'),('T08','MINEBEA'),('T14','YAHAGI THAILAND'),('TD11','DNIA'),('TD22','ADTH'),('TD30','DNMY'),('TD42','DNWX'),('TD50','DIAS'),('TD51','DIAS'),('TD60','HDVN Trading'),('TD61','DMVN'),('TD70','SKD'),('TD71','SKD');
/*!40000 ALTER TABLE `supplier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `weekday`
--

DROP TABLE IF EXISTS `weekday`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `weekday` (
  `WeekdayID` int NOT NULL AUTO_INCREMENT,
  `DayName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`WeekdayID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `weekday`
--

LOCK TABLES `weekday` WRITE;
/*!40000 ALTER TABLE `weekday` DISABLE KEYS */;
INSERT INTO `weekday` VALUES (1,'Monday'),(2,'Tuesday'),(3,'Wednesday'),(4,'Thursday'),(5,'Friday'),(6,'Saturday'),(7,'Sunday');
/*!40000 ALTER TABLE `weekday` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-05 17:09:52
