using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.StaticSource
{
    public static class DBHelper
    {
        static string connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        static Lazy<IFreeSql> sqliteLazy = new Lazy<IFreeSql>(() => new FreeSql.FreeSqlBuilder()
         .UseConnectionString(FreeSql.DataType.SqlServer, connString)
         .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
         .Build());
        public static IFreeSql SqlEntity => sqliteLazy.Value;
    }
}
