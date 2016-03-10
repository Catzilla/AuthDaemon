// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

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
                var config = Session.Config;

                var sql = new MySqlConnectionStringBuilder();
                foreach(var pair in config[ConfigBlock])
                {
                    sql[pair.KeyName] = pair.Value;
                }
                var queries = MySqlQuery.Load(config["SqlQueries"]["queries"]);
                var mysqlDatabase = new MySqlDatabase(sql.ConnectionString);


                foreach(var query in queries)
                {
                    mysqlDatabase.Queries[query.Name] = query;
                }

                Session.Database = mysqlDatabase;
            }
        }
    }
}
