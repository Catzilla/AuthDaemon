using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Threading.Tasks;
using IniParser.Parser;
using IniParser.Model;
using IniParser.Exceptions;
using System.IO;
using AuthDaemon.Net;
using System.Net;
using System.Threading;
using AuthDaemon.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using AuthDaemon.IO;
using AuthDaemon.IO.Serialization;

namespace AuthDaemon
{
    class Program
    {
        public static Logger Log = LogManager.GetLogger("main");
        public static IniData Config;
        public static AuthServer<AuthSession> AuthServer;

        static void Test2()
        {

            var login = "freepvps";
            var password = "qwerty";
            var hashs = "ubOwiUSg/BgLm8zrL/7ffA==";


            var bytes = Convert.FromBase64String(hashs);
            var md5 = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(login + password));

            Console.WriteLine(BitConverter.ToString(bytes));
            Console.WriteLine(BitConverter.ToString(md5));
        }
        static void Test()
        {
            MySqlConnectionStringBuilder mysqlCSB;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Port = 3306;
            mysqlCSB.Database = "pw";
            mysqlCSB["uid"] = "root";
            mysqlCSB.Password = "aliceabcqwertyalpha";
            Log.Info("ConnectionString = {0}", mysqlCSB.ConnectionString);
        }
        static void Test3()
        {
            var serializer = new DynamicSerializer();

            var pair = new StructureDeclaration();
            pair.Fields.Add(new KeyValuePair<string, string>("name", "byte[]"));
            pair.Fields.Add(new KeyValuePair<string, string>("id", "int"));
            serializer.KnownFields["pair"] = pair;


            var structure = new StructureDeclaration();
            structure.Fields.Add(new KeyValuePair<string, string>("global", "uint"));
            structure.Fields.Add(new KeyValuePair<string, string>("pairs", "pair"));

            dynamic d = new DynamicStructure();
            d.name = new byte[] { 1, 2, 3 };
            d.id = 20;

            dynamic dd = new DynamicStructure();
            dd.global = 1;
            //dd.pairs = d;

            var ds = new DataStream();
            structure.Serialize(serializer, ds, dd);

            Log.Info(BitConverter.ToString(ds.GetBytes()));
        }
        static void Main(string[] args)
        {
            Test3();
            if (args.Length == 0)
            {
                args = new []{ "auth.conf" };
            }

            Log.Trace("Started");

            var configPath = args[0];
            var encoding = Encoding.UTF8;

            if (!File.Exists(configPath))
            {
                Log.Error("{0} not found", configPath);
                return;
            }

            try
            {
                var configContent = File.ReadAllText(configPath);
                var parser = new IniDataParser();

                Config = parser.Parse(configContent);

                Log.Trace("Complete load config"); 
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
            var port = int.Parse(Config.Sections["GAuthServer"]["port"]);

            AuthServer = new AuthServer<AuthSession>();
            AuthServer.State = Config;
            AuthServer.Start(new IPEndPoint(IPAddress.Any, port));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
