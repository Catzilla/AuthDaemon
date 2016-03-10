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
using System.Net;

namespace AuthDaemon.Net.Plugins
{
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
        }
        private void OnMatrixPasswdArg(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var arg = e.Packet.Arg;
                int id = e.Packet.Id;
                byte[] account = arg.Account;
                byte[] challenge = arg.Challenge;
                byte[] loginip = arg.loginip;

                string login = Encoding.ASCII.GetString(account);

                Log.Debug("OnMatrixPasswdArg, ip = {2}, login = {0}, challenge = {1}",
                    login,
                    BitConverter.ToString(challenge),
                    Ext.GetIp(loginip));

                if (e.Handled) return;

                dynamic res = new DynamicStructure();
                try
                {
                    Log.Trace("Auth with login: {0}", login);
                    int uid;
                    string hashstr;
                    Session.Database.AcquireUserPassword(login, out uid, out hashstr);

                    var reshash = Convert.FromBase64String(hashstr);

                    res.retcode = 0;
                    res.userId = uid;
                    res.algorithm = 0;
                    res.response = reshash;
                    res.matrix = Ext.EmptyBytes;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Log.Trace("Auth failure: {0}", ex.Message);

                    res.retcode = 3;
                    res.userId = 0;
                    res.algorithm = 0;
                    res.response = Ext.EmptyBytes;
                    res.matrix = Ext.EmptyBytes;
                }

                Session.Send(0x226, id, arg, res);
            }
        }

    }
}
