-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.18 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for touristo
CREATE DATABASE IF NOT EXISTS `touristo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `touristo`;

-- Dumping structure for table touristo.attractions
CREATE TABLE IF NOT EXISTS `attractions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `type` int(11) NOT NULL,
  `cityid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Attractions_cityid` (`cityid`),
  CONSTRAINT `FK_Attractions_Cities_cityid` FOREIGN KEY (`cityid`) REFERENCES `cities` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table touristo.attractions: ~11 rows (approximately)
/*!40000 ALTER TABLE `attractions` DISABLE KEYS */;
REPLACE INTO `attractions` (`id`, `name`, `address`, `type`, `cityid`) VALUES
	(18, 'MOSIR Centrum Rekreacyjno Sportowe', 'Sulechowska 41', 7, 17),
	(19, 'Wino i Grono', 'Aleja Wojska Polskiego 79', 0, 17),
	(20, 'Tkalnia Zagadek Zielona Góra', 'Lisowskiego 9/2', 3, 17),
	(21, 'Muzeum Dawnych Tortur i Wina', ' Aleja Niepodległości 15', 5, 17),
	(22, 'Palmiarnia', ' Wrocławska 12a', 3, 17),
	(23, 'Ogród Botaniczny Uniwersytetu Zielonogórskiego', 'Botaniczna 50A', 2, 17),
	(24, 'Focus Park', 'Aleja 3 Maja', 0, 17),
	(25, 'Planetarium Wenus', ' Generała Władysława Sikorskiego 10', 3, 17),
	(26, 'Ratusz', 'Stary Rynek 1', 3, 17),
	(27, 'The Brandenburg Gate', 'Pariser Platz 10117', 3, 2),
	(28, 'The Rebuilt Reichstag', 'Platz der Republik 1, 11011', 3, 2);
/*!40000 ALTER TABLE `attractions` ENABLE KEYS */;

-- Dumping structure for table touristo.cities
CREATE TABLE IF NOT EXISTS `cities` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `countryid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Cities_countryid` (`countryid`),
  CONSTRAINT `FK_Cities_Countries_countryid` FOREIGN KEY (`countryid`) REFERENCES `countries` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table touristo.cities: ~5 rows (approximately)
/*!40000 ALTER TABLE `cities` DISABLE KEYS */;
REPLACE INTO `cities` (`id`, `name`, `countryid`) VALUES
	(1, 'Warszawa', 1),
	(2, 'Berlin', 2),
	(8, 'Wrocław', 1),
	(9, 'Lubsko', 1),
	(17, 'Zielona Góra', 1);
/*!40000 ALTER TABLE `cities` ENABLE KEYS */;

-- Dumping structure for table touristo.countries
CREATE TABLE IF NOT EXISTS `countries` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table touristo.countries: ~4 rows (approximately)
/*!40000 ALTER TABLE `countries` DISABLE KEYS */;
REPLACE INTO `countries` (`id`, `name`) VALUES
	(1, 'Poland'),
	(2, 'Germany'),
	(3, 'Hungary'),
	(10, 'Russia');
/*!40000 ALTER TABLE `countries` ENABLE KEYS */;

-- Dumping structure for table touristo.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table touristo.users: ~1 rows (approximately)
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
REPLACE INTO `users` (`id`, `username`, `password`, `role`) VALUES
	(1, 'KhatAdmin', 'SilneHasl@11', 'Admin');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

-- Dumping structure for table touristo.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table touristo.__efmigrationshistory: ~5 rows (approximately)
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
REPLACE INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
	('20200614144254_InitialMigration', '3.1.5'),
	('20200614144425_AddUserData', '3.1.5'),
	('20200614144543_UpdateCountriesData', '3.1.5'),
	('20200614150609_FinalMigration', '3.1.5'),
	('20200614223754_test', '3.1.5');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
