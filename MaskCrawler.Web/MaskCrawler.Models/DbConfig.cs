namespace MaskCrawler.Models
{
    public class DbConfig
    {
        public DbType Type { get; set; }
        public int? Timeout { get; set; }
        public string DataBaseName { get; set; }
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
    }

    public enum DbType
    {
        Mysql,
        SqlServer,
        Sqlite
    }
}
