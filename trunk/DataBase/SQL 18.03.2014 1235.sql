-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.6.11 - MySQL Community Server (GPL)
-- Server OS:                    Win64
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for project
CREATE DATABASE IF NOT EXISTS `project` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `project`;


-- Dumping structure for table project.employees
CREATE TABLE IF NOT EXISTS `employees` (
  `empid` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'employees ID number',
  `emp_firstname` varchar(45) NOT NULL,
  `emp_lastname` varchar(45) NOT NULL,
  `emp_address` varchar(150) NOT NULL,
  `emp_phone` varchar(45) NOT NULL,
  PRIMARY KEY (`empid`)
) ENGINE=InnoDB AUTO_INCREMENT=39785879 DEFAULT CHARSET=utf8;

-- Dumping data for table project.employees: ~2 rows (approximately)
DELETE FROM `employees`;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` (`empid`, `emp_firstname`, `emp_lastname`, `emp_address`, `emp_phone`) VALUES
	(555, 'דור', 'חזן', 'נהריה', 'סלולארי'),
	(39785878, 'שוקי', 'פורת', 'כתובת', 'טלפון');
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;


-- Dumping structure for table project.superuser
CREATE TABLE IF NOT EXISTS `superuser` (
  `userid` int(1) unsigned NOT NULL AUTO_INCREMENT,
  `empid` int(15) unsigned DEFAULT NULL COMMENT 'Fkey to employees',
  `user_name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `role` varchar(45) NOT NULL,
  `connected` varchar(45) NOT NULL DEFAULT 'false',
  `email` varchar(45) NOT NULL,
  PRIMARY KEY (`userid`),
  KEY `empid_fk_superuser` (`empid`),
  CONSTRAINT `empid_fk_superuser` FOREIGN KEY (`empid`) REFERENCES `employees` (`empid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Dumping data for table project.superuser: ~0 rows (approximately)
DELETE FROM `superuser`;
/*!40000 ALTER TABLE `superuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `superuser` ENABLE KEYS */;


-- Dumping structure for table project.users
CREATE TABLE IF NOT EXISTS `users` (
  `userid` int(15) unsigned NOT NULL AUTO_INCREMENT,
  `empid` int(15) unsigned DEFAULT NULL COMMENT 'Fkey to employees',
  `user_name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `role` varchar(45) NOT NULL,
  `connected` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  PRIMARY KEY (`userid`),
  KEY `empid_FK` (`empid`),
  CONSTRAINT `empid_FK` FOREIGN KEY (`empid`) REFERENCES `employees` (`empid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- Dumping data for table project.users: ~2 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`userid`, `empid`, `user_name`, `password`, `role`, `connected`, `email`) VALUES
	(1, 555, 'dor', '1', 'manager', 'false', 'dorhzn@gmail.com'),
	(2, 39785878, 'שוקי', '2', 'manager', 'false', 'max5452@gmail.com');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
