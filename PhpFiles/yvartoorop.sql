-- phpMyAdmin SQL Dump
-- version 4.9.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Gegenereerd op: 18 jun 2023 om 21:31
-- Serverversie: 10.6.12-MariaDB
-- PHP-versie: 7.4.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `yvartoorop`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `History`
--

CREATE TABLE `History` (
  `id` int(10) UNSIGNED NOT NULL,
  `winner_user_id` int(10) UNSIGNED DEFAULT NULL,
  `second_user_id` int(10) UNSIGNED DEFAULT NULL,
  `game_date` datetime NOT NULL DEFAULT current_timestamp(),
  `game_duration` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `History`
--

INSERT INTO `History` (`id`, `winner_user_id`, `second_user_id`, `game_date`, `game_duration`) VALUES
(71, 17, 17, '2023-05-17 21:58:27', 29.7952),
(72, 17, 15, '2020-07-13 22:09:26', 26.1794),
(73, 10, 11, '2023-03-07 22:25:35', 70),
(77, 15, 17, '2023-06-17 23:24:39', 66.6483),
(80, 15, 17, '2023-06-17 23:39:36', 35.2308),
(81, 17, 17, '2023-06-17 23:48:31', 46.6909),
(82, 15, 17, '2023-06-17 23:53:50', 32.8418),
(85, 15, 17, '2023-06-18 00:03:04', 56.4735),
(86, 17, 15, '2023-06-18 00:05:37', 28.1),
(92, 15, 17, '2023-06-18 14:10:04', 77.7783);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `Scores`
--

CREATE TABLE `Scores` (
  `id` int(10) UNSIGNED NOT NULL,
  `history_id` int(11) UNSIGNED NOT NULL,
  `user_id` int(10) UNSIGNED NOT NULL,
  `score` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `Scores`
--

INSERT INTO `Scores` (`id`, `history_id`, `user_id`, `score`) VALUES
(7, 15, 15, 6.65125),
(8, 17, 15, 9.10501),
(9, 17, 15, 9.10501),
(10, 15, 17, 11.2065),
(11, 17, 17, 8.3133),
(12, 15, 17, 11.2065),
(13, 17, 17, 8.51506),
(14, 15, 17, 13.8459),
(15, 15, 17, 13.8459),
(16, 15, 17, 36.8679),
(17, 17, 17, 7.19865),
(18, 15, 17, 16.284),
(19, 17, 17, 6.65174),
(20, 17, 17, 6.61464),
(21, 15, 17, 10.3604),
(22, 72, 15, 8.15434),
(23, 72, 15, 7.69542),
(24, 77, 15, 13.046),
(25, 77, 15, 43.1653),
(26, 80, 15, 10.4863),
(27, 80, 15, 14.4729),
(28, 81, 15, 14.0883),
(29, 81, 15, 22.1951),
(30, 82, 15, 11.5564),
(31, 82, 15, 11.0103),
(32, 85, 15, 29.9803),
(33, 85, 15, 16.2616),
(34, 86, 15, 9.79138),
(35, 86, 15, 7.97761),
(36, 92, 17, 31.1772),
(37, 92, 17, 51.4489),
(38, 92, 19, 60),
(39, 92, 19, 60),
(40, 92, 79, 60),
(41, 92, 79, 60),
(42, 92, 79, 60),
(43, 92, 79, 60),
(44, 92, 79, 60),
(45, 92, 79, 60),
(46, 92, 20, 60),
(47, 92, 20, 60);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `Servers`
--

CREATE TABLE `Servers` (
  `id` int(10) UNSIGNED NOT NULL,
  `ip` varchar(32) NOT NULL,
  `port` int(10) UNSIGNED NOT NULL,
  `local` tinyint(1) NOT NULL,
  `server_name` varchar(16) NOT NULL,
  `password` varchar(64) NOT NULL,
  `connected_users` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `Servers`
--

INSERT INTO `Servers` (`id`, `ip`, `port`, `local`, `server_name`, `password`, `connected_users`) VALUES
(166, '192.168.2.30', 25566, 1, 'macserver1', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 10),
(167, '192.168.2.30', 25566, 1, 'macserver2', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 10),
(168, '192.168.2.30', 25566, 1, 'macserver3', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 10),
(169, '192.168.2.30', 25566, 1, 'macserver4', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 10),
(170, '192.168.2.30', 25566, 1, 'macserver4', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 10);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `Users`
--

CREATE TABLE `Users` (
  `id` int(10) UNSIGNED NOT NULL,
  `server_id` int(10) NOT NULL DEFAULT -1,
  `email` varchar(64) NOT NULL,
  `username` varchar(16) NOT NULL,
  `password` varchar(64) NOT NULL,
  `games_played` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `Users`
--

INSERT INTO `Users` (`id`, `server_id`, `email`, `username`, `password`, `games_played`) VALUES
(15, 161, 'yvarftoorop@hotmail.com', 'McYvar', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 0),
(17, -1, 'mac@hotmail.com', 'Mac', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 0),
(18, 85, 'mier2@mier.nl', 'Mier2', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 0),
(19, 86, 'mier@mier.nl', 'Mier', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 0),
(20, 79, 'macy@hotmail.com', 'macy', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 2),
(21, -1, 'ton.marcus@macserver.com', 'ton', 'password', 0),
(22, -1, 'vincent.booman@macserver.com', 'vincent', 'password', 0);

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `History`
--
ALTER TABLE `History`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `Scores`
--
ALTER TABLE `Scores`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `Servers`
--
ALTER TABLE `Servers`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `Users`
--
ALTER TABLE `Users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `History`
--
ALTER TABLE `History`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=101;

--
-- AUTO_INCREMENT voor een tabel `Scores`
--
ALTER TABLE `Scores`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=48;

--
-- AUTO_INCREMENT voor een tabel `Servers`
--
ALTER TABLE `Servers`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=171;

--
-- AUTO_INCREMENT voor een tabel `Users`
--
ALTER TABLE `Users`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
