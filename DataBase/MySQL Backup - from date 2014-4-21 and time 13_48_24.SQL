--
-- Create schema project by Shuki Porat
-- 

CREATE DATABASE IF NOT EXISTS project; 
USE project;

-- MySqlBackup.NET 2.0.2
-- Dump Time: 2014-04-21 13:48:31
-- --------------------------------------
-- Server version 5.6.11 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of costumers
-- 

DROP TABLE IF EXISTS `costumers`;
CREATE TABLE IF NOT EXISTS `costumers` (
  `costumerid` int(10) unsigned NOT NULL,
  `contactid` int(10) unsigned NOT NULL,
  `costumerName` varchar(45) NOT NULL,
  `contactName` varchar(45) NOT NULL,
  `contactEmail` varchar(45) NOT NULL,
  `contactPhone` varchar(45) NOT NULL,
  `costumerAddress` varchar(45) NOT NULL,
  `contactDepartment` varchar(45) NOT NULL,
  PRIMARY KEY (`contactid`,`costumerid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table costumers
-- 

/*!40000 ALTER TABLE `costumers` DISABLE KEYS */;
INSERT INTO `costumers`(`costumerid`,`contactid`,`costumerName`,`contactName`,`contactEmail`,`contactPhone`,`costumerAddress`,`contactDepartment`) VALUES
(5,0,'חברת חשמל','דור חזן','dor@gmail.com','05555555','aaa','בלה בלה'),
(1,1,'בזק','4','5','6','65','9'),
(5,1,'חברת חשמל','aaa','aaa@gmail.com','טלפון','aaa','aaa'),
(1,2,'בזק','qwe','sssf','zvxxzv','65','fregerg'),
(1,3,'בזק','wdwqd','dwe@fdwef.com','wdewqr','65','efef'),
(1,4,'בזק','343525','dwd@sfewf.com','2324er3f','65','f43tr4vt43');
/*!40000 ALTER TABLE `costumers` ENABLE KEYS */;

-- 
-- Definition of employees
-- 

DROP TABLE IF EXISTS `employees`;
CREATE TABLE IF NOT EXISTS `employees` (
  `empid` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'employees ID number',
  `emp_firstname` varchar(45) NOT NULL,
  `emp_lastname` varchar(45) NOT NULL,
  `emp_address` varchar(150) NOT NULL,
  `emp_phone` varchar(45) NOT NULL,
  PRIMARY KEY (`empid`)
) ENGINE=InnoDB AUTO_INCREMENT=123456790 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table employees
-- 

/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees`(`empid`,`emp_firstname`,`emp_lastname`,`emp_address`,`emp_phone`) VALUES
(23,'שם פרטי','משפחה','מקום','גלקסי'),
(555,'דור','חזן','נהריה','סלולארי'),
(39785878,'שוקי','פורת','כתובת','טלפון'),
(123456789,'דובי','בלום','מוצקין','04555333');
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;

-- 
-- Definition of item
-- 

DROP TABLE IF EXISTS `item`;
CREATE TABLE IF NOT EXISTS `item` (
  `itemid` varchar(45) NOT NULL COMMENT 'מספר הפריט',
  `itemStatus` varchar(45) NOT NULL COMMENT 'עבודה-תיקון-תקול-הסתיים --- עבור יצירת 2 דפוסים עבור אותו פריט, דפוס אחד לפריט בעבודה ודפוס שני לפריט בתיקון',
  `itemStageOrder` varchar(45) NOT NULL COMMENT 'the order of the stages for the itemtid',
  `itemName` varchar(45) NOT NULL COMMENT 'שם הפריט',
  `stageName` varchar(45) NOT NULL COMMENT 'שם השלב',
  `discription` varchar(200) DEFAULT NULL COMMENT 'תיאור השלב',
  PRIMARY KEY (`itemid`,`itemStageOrder`,`itemStatus`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table item
-- 

/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item`(`itemid`,`itemStatus`,`itemStageOrder`,`itemName`,`stageName`,`discription`) VALUES
('1','עבודה','1','efrefre','bkla','dsa'),
('1','בעבודה','2','two','sfsaf','vdfvfdb'),
('2','עבודה','1','gregreg','bla','fewf'),
('3','עבודה','1','rgreg','bla','grg');
/*!40000 ALTER TABLE `item` ENABLE KEYS */;

-- 
-- Definition of jobs
-- 

DROP TABLE IF EXISTS `jobs`;
CREATE TABLE IF NOT EXISTS `jobs` (
  `jobid` varchar(45) NOT NULL COMMENT 'מספר עבודה',
  `itemid` varchar(45) NOT NULL COMMENT 'מספר פריט - גם מפתח זר לטבלת פריט',
  `itemNum` varchar(45) NOT NULL COMMENT 'מאפשר שיהיה בעבודה יותר מפריט אחד מאותו הסוג',
  `itemStatus` varchar(45) NOT NULL COMMENT 'the status of the item- נרשם-בעבודה-בתיקון-תקול-הסתיים',
  `itemStageOrder` varchar(45) NOT NULL COMMENT 'the current Stage of the item in the items table also a FK to items table',
  `expectedItemQuantity` varchar(45) NOT NULL COMMENT 'הכמות שהוזמנה מפריט זה עבור עבודה זו יהיה אפשר רק בהוספת פריט ראשון או בעדכון ספציפי',
  `costumerid` varchar(45) NOT NULL COMMENT 'מס לקוח',
  `itemDescription` varchar(45) DEFAULT 'לא נרשם תיאור' COMMENT 'פירוט לגבי כל פריט בודד בקבוצה',
  `itemsDescription` varchar(45) DEFAULT 'לא נרשם תיאור' COMMENT 'פירוט לגבי קבוצה של פריטים באותו הסוג - יהיה ניתון רק בהוספת פריט ראשון או בעדכון ספציפי',
  `itemToFixStageOrder` varchar(45) DEFAULT 'הכל תקין' COMMENT 'מספר סדר השלב שבו זוהה הפריט כתקול',
  `job_status` varchar(45) NOT NULL COMMENT 'נרשם-בעבודה-מושהה-הסתיים-מבוטל',
  `jobdescription` varchar(45) NOT NULL COMMENT 'תיאור העבודה',
  `startDate` date NOT NULL,
  `expectedFinishDate` date NOT NULL,
  `actualFinishDate` date DEFAULT NULL,
  PRIMARY KEY (`jobid`,`itemid`,`itemNum`) USING BTREE,
  KEY `FK_jobs_1` (`itemid`,`itemStageOrder`,`itemStatus`),
  CONSTRAINT `FK_jobs_1` FOREIGN KEY (`itemid`, `itemStageOrder`, `itemStatus`) REFERENCES `item` (`itemid`, `itemStageOrder`, `itemStatus`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table jobs
-- 

/*!40000 ALTER TABLE `jobs` DISABLE KEYS */;
INSERT INTO `jobs`(`jobid`,`itemid`,`itemNum`,`itemStatus`,`itemStageOrder`,`expectedItemQuantity`,`costumerid`,`itemDescription`,`itemsDescription`,`itemToFixStageOrder`,`job_status`,`jobdescription`,`startDate`,`expectedFinishDate`,`actualFinishDate`) VALUES
('1','1','1','עבודה','1','1','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','bfdsfbisdgbijfdgbijfgbiaops','2014-01-01 00:00:00','2014-02-01 00:00:00','2014-02-01 00:00:00'),
('1','1','2','בעבודה','2','1','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','ניסיון לפריט נוסף','2014-01-03 00:00:00','2015-09-01 00:00:00','2017-02-01 00:00:00'),
('1','1','3','עבודה','1','1','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','דגדשגדשכ','2014-01-03 00:00:00','2014-09-01 00:00:00','2015-02-01 00:00:00'),
('1','1','4','בעבודה','2','1','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','עהרקע','2014-01-03 00:00:00','2014-09-01 00:00:00','2014-09-01 00:00:00'),
('1','2','1','עבודה','1','4','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','דגדשגכקכ','2014-01-03 00:00:00','2014-09-01 00:00:00','2015-02-01 00:00:00'),
('2','2','1','עבודה','1','1','5','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','job2 fdsafsadf dsfsdfsdf dsfdfsf','2014-01-04 00:00:00','2014-11-01 00:00:00','2016-02-01 00:00:00'),
('3','3','1','עבודה','1','3','1','לא נרשם תיאור','לא נרשם תיאור','הכל תקין','בעבודה','JOB 3 !!!!!!!!!','2014-01-03 00:00:00','2014-09-01 00:00:00','2015-02-01 00:00:00');
/*!40000 ALTER TABLE `jobs` ENABLE KEYS */;

-- 
-- Definition of superuser
-- 

DROP TABLE IF EXISTS `superuser`;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table superuser
-- 

/*!40000 ALTER TABLE `superuser` DISABLE KEYS */;

/*!40000 ALTER TABLE `superuser` ENABLE KEYS */;

-- 
-- Definition of users
-- 

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `userid` int(15) unsigned NOT NULL AUTO_INCREMENT,
  `empid` int(15) unsigned DEFAULT NULL COMMENT 'Fkey to employees',
  `user_name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `role` varchar(45) NOT NULL,
  `connected` varchar(45) NOT NULL DEFAULT 'לא מחובר',
  `email` varchar(45) NOT NULL,
  PRIMARY KEY (`userid`),
  KEY `empid_FK` (`empid`),
  CONSTRAINT `empid_FK` FOREIGN KEY (`empid`) REFERENCES `employees` (`empid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table users
-- 

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users`(`userid`,`empid`,`user_name`,`password`,`role`,`connected`,`email`) VALUES
(1,555,'dor','1','מנהל','לא מחובר','dorhzn@gmail.com'),
(2,39785878,'שוקי','2','מזכירה','לא מחובר','max5452@gmail.com'),
(3,123456789,'דב','3','איכות','לא מחובר','test1@test.com');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2014-04-21 13:48:32
-- Total time: 0:0:0:1:329 (d:h:m:s:ms)
