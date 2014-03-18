-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.6.11


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema project
--

CREATE DATABASE IF NOT EXISTS project;
USE project;

--
-- Definition of table `employees`
--

DROP TABLE IF EXISTS `employees`;
CREATE TABLE `employees` (
  `empid` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'employees ID number',
  `emp_firstname` varchar(45) NOT NULL,
  `emp_lastname` varchar(45) NOT NULL,
  `emp_address` varchar(150) NOT NULL,
  `emp_phone` varchar(45) NOT NULL,
  PRIMARY KEY (`empid`)
) ENGINE=InnoDB AUTO_INCREMENT=123456790 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `employees`
--

/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` (`empid`,`emp_firstname`,`emp_lastname`,`emp_address`,`emp_phone`) VALUES 
 (555,'דור','חזן','נהריה','סלולארי'),
 (39785878,'שוקי','פורת','כתובת','טלפון'),
 (123456789,'דובי','בלום','מוצקין','04555333');
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;


--
-- Definition of table `superuser`
--

DROP TABLE IF EXISTS `superuser`;
CREATE TABLE `superuser` (
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

--
-- Dumping data for table `superuser`
--

/*!40000 ALTER TABLE `superuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `superuser` ENABLE KEYS */;


--
-- Definition of table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
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

--
-- Dumping data for table `users`
--

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`userid`,`empid`,`user_name`,`password`,`role`,`connected`,`email`) VALUES 
 (1,555,'dor','1','מנהל','לא מחובר','dorhzn@gmail.com'),
 (2,39785878,'שוקי','2','מזכירה','לא מחובר','max5452@gmail.com'),
 (3,123456789,'דב','3','איכות','לא מחובר','max5452@gmail.com');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
