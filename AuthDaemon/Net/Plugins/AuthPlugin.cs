// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDaemon.IO.Serialization;
using AuthDaemon.Net.Security;

namespace AuthDaemon.Net.Plugins
{
    public class AuthHandler
    {
        public AuthPlugin AuthPlugin { get; private set; }
        public MD5Hash MD5 { get; private set; }

        public uint Id { get; set; }
        public byte[] Account { get; set; }
        public byte[] Challenge { get; set; }
        public AuthHandler(AuthPlugin authPlugin)
        {
            AuthPlugin = authPlugin;
            MD5 = new MD5Hash();
        }

        public void MatrixPasswd()
        {
            dynamic res = new DynamicStructure();
            try
            {
                var login = Encoding.ASCII.GetString(Account);
                AuthPlugin.Log.Trace("Auth with login: {0}", login);
                int uid;
                string hashstr;
                AuthPlugin.Session.Database.AcquireUserPassword(login, out uid, out hashstr);

                var reshash = Convert.FromBase64String(hashstr);

                AuthPlugin.Log.Debug("AuthHandler.MatrixPasswd, \r\n   login = {0}\r\n   challenge = {1}\r\n   reshash = {2}\r\n   uid = {3}",
                    login,
                    BitConverter.ToString(Challenge),
                    BitConverter.ToString(reshash),
                    uid);

                res.retcode = 0;
                res.userId = uid;
                res.algorithm = 0;
                res.response = reshash;
                res.matrix = Ext.EmptyBytes;
            }
            catch(Exception ex)
            {
                AuthPlugin.Log.Warn(ex);
                res.retcode = 1;
                res.userId = 0;
                res.algorithm = 0;
                res.response = Ext.EmptyBytes;
                res.matrix = Ext.EmptyBytes;
            }
            dynamic packet = new DynamicStructure();
            packet.id = Id;
            packet.res = res;

            AuthPlugin.Session.Send(0x226, packet);
        }
    }
    public class AuthPlugin : Plugin
    {
        public override string Name
        {
            get
            {
                return "auth";
            }
        }
        public override void Initialize()
        {
            Session.Handler.AddHandler(0x226, OnMatrixPasswdArg);
            Session.Handler.AddHandler(0x0F, OnUserLogin);
        }
        private void OnMatrixPasswdArg(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var arg = e.Packet.Arg;
                var id = (uint)e.Packet.Id & 0x7FFFFFFF;
                var account = (byte[])arg.Account;
                var challenge = (byte[])arg.Challenge;

                var handler = new AuthHandler(this);
                handler.Id = id;
                handler.Account = account;
                handler.Challenge = challenge;
                
                handler.MatrixPasswd();
            }
        }
        private void OnUserLogin(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var arg = e.Packet.Arg;
                var id = (uint)e.Packet.Id & 0x7FFFFFFF;
                
                dynamic res = new DynamicStructure();
                res.retcode = 0;
                res.remainplaytime = 0;
                res.func = 0;
                res.funcparm = 0;
                res.blisgm = 0;
                res.freetimeleft = 0;
                res.freetimeend = 0;
                res.creatime = 0;
                res.adduppoint = 0;
                res.soldpoint = 0;

                dynamic packet = new DynamicStructure();
                packet.Id = id;
                packet.res = res;

                Session.Send(0x0f, packet);
                Log.Trace("Empty 0x0F sended");
            }
        }
    }
}
