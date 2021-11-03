using Dapper;

using MaskCrawler.Models;
using MaskCrawler.Utils;

using Microsoft.Extensions.Configuration;

using MySql.Data.MySqlClient;

using System;
using System.Data;


namespace MaskCrawler.Persistent.Infrastructure.MySql
{
    public class MySqlDataCentre : IDatabaseAdapter
    {
        private IDbConnection connection;
        public IDbConnection Connection { get => GetConnection(); }
        public IConfiguration Configuration { get; }
        public DbConfig DbConfig { get; }

        public MySqlDataCentre(DbConfig dbConfig)
        {
            this.DbConfig = dbConfig ?? throw new ArgumentNullException(nameof(dbConfig));
            if (dbConfig.Type == Models.DbType.Mysql)
            {
                if (string.IsNullOrWhiteSpace(dbConfig.ConnectionString))
                    dbConfig.ConnectionString = $"server={dbConfig.Server};database={dbConfig.DataBaseName};user={dbConfig.UserName};pwd={dbConfig.Password};";

                SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            }
            else
                throw new ArgumentException(nameof(Models.DbType));
        }

        public void InitDataBase()
        {
            try
            {
                return;

                using (var conn = GetConnection())
                using (var dbs = conn.BeginTransaction())
                {
                    var sqls = SqlTablesCons.CreateMySqlTables.SplitGets(",");
                    int num = 0, tableCount = sqls.Length;

                    PrintUtil.DW($"共获取到 {tableCount} 条数据表!", ConsoleColor.Yellow);
                    foreach (var sql in sqls)
                    {
                        int n = 0;
                        try
                        {
                            n = conn.Execute(sql, transaction: dbs, commandTimeout: DbConfig.Timeout);
                        }
                        catch (MySqlException ex)
                        {
#if DEBUG
                            throw ex;
#else
                            // TODO Log Write
#endif
                        }

                        if (n > 0)
                        {
                            num++;
                        }
                    }
                    dbs.Commit();
                    PrintUtil.DW($"成功添加 {num} 条表!", ConsoleColor.Yellow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Deletes(string tableName)
        {
            using (var conn = GetConnection())
            {
                var sql = $"drop table {tableName}";
                var num = conn.Execute(sql);
                return num.GetBool();
            }

        }

        private string GetTableName(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException($"“{nameof(sql)}”不能为 null 或空白。", nameof(sql));
            }

            var pattern = "insert (.*?) (";
            var name = sql.RegexGet(pattern);

            return name;
        }

        private bool Exist(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException($"“{nameof(tableName)}”不能为 null 或空白。", nameof(tableName));
            }

            using (var conn = GetConnection())
            {
                var qsql = "select count(*) from master where tablename=" + tableName;
                var val = conn.Execute(qsql);
                return val.GetBool();
            }
        }

        public IDbConnection GetConnection()
        {
            if (connection == null)
                connection = new MySqlConnection(DbConfig.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }
    }
}
