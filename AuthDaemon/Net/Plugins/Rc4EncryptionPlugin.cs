// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDaemon.Net.Security.Base;
using IniParser.Model;

namespace AuthDaemon.Net.Plugins
{
    public class Rc4EncryptionPlugin : Plugin
    {
        public override string Name
        {
            get
            {
                return "rc4encryption";
            }
        }
        public override void Initialize()
        {
            if (Enabled)
            {
                /*
                
iseckey                 =       baxi...
osec                    =       2
oseckey                 =       baxi...
shared_key              =       baxi...
                */
                var config = Session.Config;

                var iseckey = GetVar("iseckey");
                var oseckey = GetVar("oseckey");

                Log.Debug("iseckey = " + iseckey);
                Log.Debug("oseckey = " + oseckey);

                var enc = new Rc4Security();
                enc.InitializeDec(Encoding.ASCII.GetBytes(iseckey));
                enc.InitializeEnc(Encoding.ASCII.GetBytes(oseckey));

                Session.ClientState.Encryptor = enc;
            }
        }
    }
}
