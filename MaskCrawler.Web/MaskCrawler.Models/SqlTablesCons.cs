namespace MaskCrawler.Models
{
    public static class SqlTablesCons
    {
        public static string CreateMySqlTables = @"CREATE TABLE `account` (
  `id` int NOT NULL AUTO_INCREMENT,
  `gid` char(36) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `password` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `salt` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `createDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_gid` (`gid`),
  UNIQUE KEY `idx_phone` (`phone`),
  UNIQUE KEY `idx_email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `task` (
  `tid` char(36) NOT NULL,
  `tpid` char(36) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `describe` varchar(255) DEFAULT NULL,
  `resType` varchar(20) DEFAULT NULL,
  `createDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`tid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
";
    }
}
