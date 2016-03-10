using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDaemon.Data;
using MySql.Data.MySqlClient;

namespace AuthDaemon.Net.Plugins
{
    public class MySqlDatabasePlugin : Plugin
    {
        public override string ConfigBlock
        {
            get
            {
                return "MySqlDatabase";
            }
        }
        public override string Name
        {
            get
            {
                return "mysqldatabase";
            }
        }

        public override void Initialize()
        {
            if (Enabled)
            {
                var sql = new MySqlConnectionStringBuilder();
                foreach(var pair in Session.Config[ConfigBlock])
                {
                    sql[pair.KeyName] = pair.Value;
                }
                Session.Database = new MySqlDatabase(sql.ConnectionString);
            }
        }
    }
}
