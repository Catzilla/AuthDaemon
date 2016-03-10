// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

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
        
        static void Main(string[] args)
        {
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
