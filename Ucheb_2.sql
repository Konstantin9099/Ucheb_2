CREATE DATABASE Ucheb_2;

USE Ucheb_2;

CREATE TABLE `clients` (
  `client_id` int PRIMARY KEY AUTO_INCREMENT,
  `client_fio` varchar(80),
  `client_phone` varchar(12),
  `client_birth` date
);

CREATE TABLE `orders` (
  `order_id` int PRIMARY KEY AUTO_INCREMENT,
  `order_date` date,
  `order_client` int,
  `order_comment` text,
  `order_response` int
);

CREATE TABLE `auth` (
  `auth_id` int PRIMARY KEY AUTO_INCREMENT,
  `auth_log` varchar(20),
  `auth_pwd` text,
  `auth_fio` varchar(80)
);

ALTER TABLE `orders` ADD FOREIGN KEY (`order_response`) REFERENCES `auth` (`auth_id`);

ALTER TABLE `orders` ADD FOREIGN KEY (`order_client`) REFERENCES `clients` (`client_id`);

INSERT INTO `auth` VALUES (1,'111','111','Иванов Иван Иванович'),(2,'222','222','Васильева Вера Дмитриевна'),(3,'333','333','Чичиков Павел Иванович'),(4,'444','444','Орлов Максим Андреевич');

INSERT INTO `clients` VALUES (1,'Иванов Иван Иванович','+75624458745','1999-01-05'),(2,'Васильева Вера Дмитриевна','+74313216548','1983-05-25'),(3,'Чичиков Павел Иванович','+78645413100','1965-06-07'),(4,'Орлов Максим Андреевич','+74658751223','1978-10-15');

INSERT INTO `orders` VALUES (2,'2023-02-15',1,'Бумага для принтера (2 упаковки)',1),(3,'2023-02-17',2,'Папки (3 шт.), линейка (1 шт.), ручка гелевая черная (1 шт.)',2),(4,'2023-02-16',3,'Тетрадь общая в клетку 96 лист. (3 шт.)',3),(6,'2023-02-16',4,'Лампа настольная (1 шт.)',4),(7,'2023-02-17',4,'Корректор (1 шт.)',4),(8,'2023-02-18',4,'Текстовыделители (набор из 4 шт. разн. цветов)',4),(9,'2023-02-17',4,'Папка-скоросшиватель пластик. (5 шт.)',4);
