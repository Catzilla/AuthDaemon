using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.Net.Plugins
{
    public class PrivilegePlugin : Plugin
    {
        public override string ConfigBlock
        {
            get
            {
                return "Privilege";
            }
        }
        public override string Name
        {
            get
            {
                return "privilege";
            }
        }
        public AnnounceZone Zone { get; private set; }
        public byte[] Auth { get; private set; }
        public override void Initialize()
        {
            Auth = Convert.FromBase64String(GetVar("auth").Safe());

            Session.SendHandler.AddHandler(0x0F, OnUserLogin);
            Session.Handler.AddHandler(506, OnQueryUserPrivilege);
            Session.Handler.AddHandler(507, OnQueryUserPrivilege);

            Zone = Session.Plugins.Register<AnnounceZone>();
        }
        private bool CheckPrivilege(int userId, int zoneId)
        {
            return Session.Database.Query("acquireUserPrivilege.byUidZid", userId, zoneId).Count() > 0;
        }
        private void OnUserLogin(object sender, PacketEventArgs e)
        {
            if (Enabled && !e.Handled)
            {
                var arg = e.Packet.Arg;
                var res = e.Packet.Res;
                var id = e.Packet.Id;
                if (res.Retcode == 0)
                {

                    int userId = arg.userId;
                    int zoneId = Zone.ZoneId;

                    try
                    {
                        res.blisgm = CheckPrivilege(userId, zoneId) ? 1 : 0;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
        }

        private void OnQueryUserPrivilege(object sender, PacketEventArgs e)
        {
            if (!Enabled && e.Handled) return;
            
            var arg = e.Packet;
            dynamic res = new DynamicStructure();

            int userId = arg.userId;
            int zoneId = arg.zoneId;

            Log.Trace("QueryUserPrivilege userId = {0}, zoneId = {1}", userId, zoneId);

            /*
            packet 507 QueryUserPrivilege_Re:
	int userId;
	byte[] auth;
            */

            res.userId = userId;
            res.auth = Auth;

            Session.Send(507, res);
        }
    }
}
