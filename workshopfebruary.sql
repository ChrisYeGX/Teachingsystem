-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: 2018-05-22 04:44:57
-- 服务器版本： 10.1.19-MariaDB
-- PHP Version: 5.6.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `workshopfebruary`
--

-- --------------------------------------------------------

--
-- 表的结构 `assignment`
--

CREATE TABLE `assignment` (
  `Assignment_id` int(11) NOT NULL,
  `AssignmentName` varchar(25) NOT NULL,
  `QuestionsID` varchar(255) DEFAULT NULL,
  `percentage` double DEFAULT NULL,
  `coursecode` varchar(25) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `assignment`
--

INSERT INTO `assignment` (`Assignment_id`, `AssignmentName`, `QuestionsID`, `percentage`, `coursecode`) VALUES
(1, 'Assignment1', '10001,10002,10003', 20, '1'),
(2, 'Assignment2', '20001,20002,20003,20004', 80, '1');

-- --------------------------------------------------------

--
-- 表的结构 `course`
--

CREATE TABLE `course` (
  `Course_code` int(11) NOT NULL,
  `courseName` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `course`
--

INSERT INTO `course` (`Course_code`, `courseName`) VALUES
(1, 'WorkshopIII(2017-2018)');

-- --------------------------------------------------------

--
-- 表的结构 `finalgrade`
--

CREATE TABLE `finalgrade` (
  `CourseName` varchar(255) NOT NULL,
  `StudentName` varchar(255) NOT NULL,
  `StudentId` varchar(255) NOT NULL,
  `AssignmentName` varchar(255) NOT NULL,
  `Mark` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `finalgrade`
--

INSERT INTO `finalgrade` (`CourseName`, `StudentName`, `StudentId`, `AssignmentName`, `Mark`) VALUES
('WorkshopIII(2017-2018)', 'Dengjie', '1530003004', 'Assignment1', '66'),
('WorkshopIII(2017-2018)', 'Dengjie', '1530003004', 'Assignment2', '74'),
('WorkshopIII(2017-2018)', 'Grayson', '1530003005', 'Assignment1', '71'),
('WorkshopIII(2017-2018)', 'Grayson', '1530003005', 'Assignment2', '66'),
('WorkshopIII(2017-2018)', 'Liam', '1530003009', 'Assignment1', '78'),
('WorkshopIII(2017-2018)', 'Liam', '1530003009', 'Assignment2', '79.5'),
('WorkshopIII(2017-2018)', 'xiange', '1530003051', 'Assignment1', '100'),
('WorkshopIII(2017-2018)', 'xiange', '1530003051', 'Assignment2', '93.5');

-- --------------------------------------------------------

--
-- 表的结构 `question`
--

CREATE TABLE `question` (
  `QuestionID` varchar(255) NOT NULL DEFAULT '',
  `RubricName` varchar(255) DEFAULT NULL,
  `percentage` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `question`
--

INSERT INTO `question` (`QuestionID`, `RubricName`, `percentage`) VALUES
('10001', 'rubric1', 20),
('10002', 'rubric1', 50),
('10003', 'rubric1', 30),
('20001', 'rubric2', 30),
('20002', 'rubric2', 60),
('20003', 'rubric1', 5),
('20004', 'rubric1', 5);

-- --------------------------------------------------------

--
-- 表的结构 `rubric`
--

CREATE TABLE `rubric` (
  `RubricsID` int(11) NOT NULL,
  `RubricName` varchar(225) NOT NULL,
  `RubricsItem` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `rubric`
--

INSERT INTO `rubric` (`RubricsID`, `RubricName`, `RubricsItem`) VALUES
(1, 'rubric1', '10001,10002,10003'),
(2, 'rubric2', '20001,20002,20003');

-- --------------------------------------------------------

--
-- 表的结构 `rubricitem`
--

CREATE TABLE `rubricitem` (
  `rubricitemid` int(20) NOT NULL,
  `name` varchar(50) NOT NULL,
  `percentage` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `rubricitem`
--

INSERT INTO `rubricitem` (`rubricitemid`, `name`, `percentage`) VALUES
(10001, 'aaaaaa', 50),
(10002, 'bbbbbb', 50),
(20001, 'cccccc', 50),
(20002, 'dddddd', 50),
(30001, 'eeeeee', 50),
(30002, 'fffff', 50),
(40001, 'gggggg', 50),
(40002, 'hhhhhh', 50),
(50001, 'iiiiii', 50),
(50002, 'jjjjjj', 50),
(60001, 'kkkkkk', 50),
(60002, 'llllll', 50),
(70001, 'mmmmmm', 50),
(70002, 'nnnnnn', 50),
(80001, 'oooooo', 50),
(80002, 'pppppp', 50);

-- --------------------------------------------------------

--
-- 表的结构 `student`
--

CREATE TABLE `student` (
  `Name` varchar(25) NOT NULL,
  `ID` int(20) NOT NULL,
  `FinalGrade` varchar(25) NOT NULL,
  `Course_code` varchar(25) NOT NULL,
  `groups` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `student`
--

INSERT INTO `student` (`Name`, `ID`, `FinalGrade`, `Course_code`, `groups`) VALUES
('Dengjie', 1530003004, '51.32', '1', '1'),
('Grayson', 1530003005, '52.5', '1', '1'),
('Liam', 1530003009, '54.44', '1', '1'),
('xiange', 1530003051, '35.2', '1', '1');

-- --------------------------------------------------------

--
-- 表的结构 `students_assignment`
--

CREATE TABLE `students_assignment` (
  `idconbom` varchar(100) NOT NULL,
  `mark` double NOT NULL,
  `Latency` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 转存表中的数据 `students_assignment`
--

INSERT INTO `students_assignment` (`idconbom`, `mark`, `Latency`) VALUES
('1+1530003004+10001+rubric1+10001+Dengjie+Assignment1', 100, '100'),
('1+1530003004+10001+rubric1+10002+Dengjie+Assignment1', 80, '100'),
('1+1530003004+10002+rubric1+10001+Dengjie+Assignment1', 80, '100'),
('1+1530003004+10002+rubric1+10002+Dengjie+Assignment1', 60, '100'),
('1+1530003004+10003+rubric1+10001+Dengjie+Assignment1', 100, '100'),
('1+1530003004+10003+rubric1+10002+Dengjie+Assignment1', 100, '100'),
('1+1530003005+10001+rubric1+10001+Grayson+Assignment1', 100, '70'),
('1+1530003005+10001+rubric1+10002+Grayson+Assignment1', 100, '70'),
('1+1530003005+10002+rubric1+10001+Grayson+Assignment1', 80, '70'),
('1+1530003005+10002+rubric1+10002+Grayson+Assignment1', 100, '70'),
('1+1530003005+10003+rubric1+10001+Grayson+Assignment1', 80, '70'),
('1+1530003005+10003+rubric1+10002+Grayson+Assignment1', 40, '70'),
('1+1530003009+10001+rubric1+10001+Liam+Assignment1', 80, '50'),
('1+1530003009+10001+rubric1+10002+Liam+Assignment1', 60, '50'),
('1+1530003009+10002+rubric1+10001+Liam+Assignment1', 80, '50'),
('1+1530003009+10002+rubric1+10002+Liam+Assignment1', 100, '50'),
('1+1530003009+10003+rubric1+10001+Liam+Assignment1', 40, '50'),
('1+1530003009+10003+rubric1+10002+Liam+Assignment1', 60, '50'),
('1+1530003051+10001+rubric1+10001+xiange+Assignment1', 80, '0'),
('1+1530003051+10001+rubric1+10002+xiange+Assignment1', 100, '0'),
('1+1530003051+10002+rubric1+10001+xiange+Assignment1', 60, '0'),
('1+1530003051+10002+rubric1+10002+xiange+Assignment1', 80, '0'),
('1+1530003051+10003+rubric1+10001+xiange+Assignment1', 40, '0'),
('1+1530003051+10003+rubric1+10002+xiange+Assignment1', 60, '0'),
('2+1530003004+20001+rubric2+20001+Dengjie+Assignment2', 80, '70'),
('2+1530003004+20001+rubric2+20002+Dengjie+Assignment2', 80, '70'),
('2+1530003004+20002+rubric2+20001+Dengjie+Assignment2', 40, '70'),
('2+1530003004+20002+rubric2+20002+Dengjie+Assignment2', 60, '70'),
('2+1530003004+20003+rubric1+10001+Dengjie+Assignment2', 80, '70'),
('2+1530003004+20003+rubric1+10002+Dengjie+Assignment2', 80, '70'),
('2+1530003004+20004+rubric1+10001+Dengjie+Assignment2', 80, '70'),
('2+1530003004+20004+rubric1+10002+Dengjie+Assignment2', 80, '70'),
('2+1530003005+20001+rubric2+20001+Grayson+Assignment2', 100, '70'),
('2+1530003005+20001+rubric2+20002+Grayson+Assignment2', 100, '70'),
('2+1530003005+20002+rubric2+20001+Grayson+Assignment2', 60, '70'),
('2+1530003005+20002+rubric2+20002+Grayson+Assignment2', 60, '70'),
('2+1530003005+20003+rubric1+10001+Grayson+Assignment2', 80, '70'),
('2+1530003005+20003+rubric1+10002+Grayson+Assignment2', 80, '70'),
('2+1530003005+20004+rubric1+10001+Grayson+Assignment2', 60, '70'),
('2+1530003005+20004+rubric1+10002+Grayson+Assignment2', 60, '70'),
('2+1530003009+20001+rubric2+20001+Liam+Assignment2', 100, '70'),
('2+1530003009+20001+rubric2+20002+Liam+Assignment2', 100, '70'),
('2+1530003009+20002+rubric2+20001+Liam+Assignment2', 80, '70'),
('2+1530003009+20002+rubric2+20002+Liam+Assignment2', 80, '70'),
('2+1530003009+20003+rubric1+10001+Liam+Assignment2', 80, '70'),
('2+1530003009+20003+rubric1+10002+Liam+Assignment2', 80, '70'),
('2+1530003009+20004+rubric1+10001+Liam+Assignment2', 40, '70'),
('2+1530003009+20004+rubric1+10002+Liam+Assignment2', 40, '70'),
('2+1530003051+20001+rubric2+20001+xiange+Assignment2', 100, '50'),
('2+1530003051+20001+rubric2+20002+xiange+Assignment2', 100, '50'),
('2+1530003051+20002+rubric2+20001+xiange+Assignment2', 80, '50'),
('2+1530003051+20002+rubric2+20002+xiange+Assignment2', 80, '50'),
('2+1530003051+20003+rubric1+10001+xiange+Assignment2', 100, '50'),
('2+1530003051+20003+rubric1+10002+xiange+Assignment2', 100, '50'),
('2+1530003051+20004+rubric1+10001+xiange+Assignment2', 100, '50'),
('2+1530003051+20004+rubric1+10002+xiange+Assignment2', 100, '50');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `assignment`
--
ALTER TABLE `assignment`
  ADD PRIMARY KEY (`Assignment_id`);

--
-- Indexes for table `course`
--
ALTER TABLE `course`
  ADD PRIMARY KEY (`Course_code`);

--
-- Indexes for table `finalgrade`
--
ALTER TABLE `finalgrade`
  ADD PRIMARY KEY (`CourseName`,`StudentName`,`StudentId`,`AssignmentName`);

--
-- Indexes for table `question`
--
ALTER TABLE `question`
  ADD PRIMARY KEY (`QuestionID`);

--
-- Indexes for table `rubric`
--
ALTER TABLE `rubric`
  ADD PRIMARY KEY (`RubricsID`);

--
-- Indexes for table `student`
--
ALTER TABLE `student`
  ADD PRIMARY KEY (`ID`,`Course_code`);

--
-- Indexes for table `students_assignment`
--
ALTER TABLE `students_assignment`
  ADD PRIMARY KEY (`idconbom`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `course`
--
ALTER TABLE `course`
  MODIFY `Course_code` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
