using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.Net.Plugins
{
    public class OnlineRecordPlugin : Plugin
    {
        public override string Name
        {
            get
            {
                return "onlinerecord";
            }
        }
        public AnnounceZone Zone { get; private set; }
        public override void Initialize()
        {
            Session.Handler.AddHandler(0x0F, OnUserLogin);
            Session.Handler.AddHandler(33, OnUserLogout);

            Zone = Session.Plugins.Register<AnnounceZone>();
        }
        private void OnUserLogin(object sender, PacketEventArgs e)
        {
            if (Enabled && !e.Handled)
            {
                var arg = e.Packet.Arg;
                var id = e.Packet.Id;

                int userId = arg.userId;
                int localsid = arg.localsid;
                int blkickuser = arg.blkickuser;
                byte[] loginip = arg.loginip;
                byte[] account = arg.account;

                int zoneId = Zone.ZoneId;

                dynamic res = new DynamicStructure();
                try
                {
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

                    int overwrite = blkickuser;
                    Session.Database.RecordOnline(userId, Zone.Aid, ref zoneId, ref localsid, ref overwrite);
                    if (zoneId != Zone.ZoneId || (blkickuser == 0 && localsid != arg.localsid))
                    {
                        Log.Warn("(zoneId != Zone.ZoneId || localsid != arg.localsid <=> {0} != {1} || {2} != {3}", zoneId, Zone.ZoneId, localsid, (int)arg.localsid);

                        res.retcode = 10;
                    }
                    else
                    {
                        DateTime creatime = Session.Database.Query("acquireUserCreatime.byUid", userId).First().CreaTime;

                        Log.Trace("OnUserLogin login={0}, uid={1}, ip={2}", Encoding.ASCII.GetString(account), userId, Ext.GetIp(loginip));

                        res.creatime = Ext.ToUnixTime(creatime);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    res.retcode = 0xff;
                    /*
                        if ((arrayOfObject[0] != null) && (arrayOfObject[1] != null) && (arrayOfObject[2] != null) && ((((Integer)arrayOfObject[0]).intValue() != i) || (((Integer)arrayOfObject[1]).intValue() != localUserLoginArg.localsid)))
      if (((Integer)arrayOfObject[2]).intValue() == 1)
      {
        KickoutUser localObject = (KickoutUser)Rpc.Create("KICKOUTUSER");
        localObject.userid = localUserLoginArg.userid;
        localObject.localsid = ((Integer)arrayOfObject[1]).intValue();
        localObject.cause = 32;
        GAuthServer.GetLog().info("Send Kickout userid=" + localObject.userid + " sid=" + localObject.localsid);
        Session localSession = GetSessionbyZoneid((Integer)arrayOfObject[0]);
        if (localSession != null)
          localGAuthServer.Send(localSession, localObject);
        else
          GAuthServer.GetLog().info("Error: kickout user " + localObject.userid + " failed.");
      }
      else
      {
        localUserLoginRes.retcode = 10;
        return;
      }
                    */
                }

                Session.Send(0x0f, id, arg, res);
                Log.Trace("0x0F sended, retcode = {0}", (int)res.retcode);
            }
        }
        private void OnUserLogout(object sender, PacketEventArgs e)
        {
            if (Enabled && !e.Handled)
            {
                var arg = e.Packet.Arg;
                var id = e.Packet.Id;

                int userId = arg.userId;
                int localSid = arg.Localsid;

                int zoneId = Zone.ZoneId;
                int aid = Zone.Aid;
                int overwrite = aid;

                Session.Database.RecordOffline(userId, aid, ref zoneId, ref localSid, ref overwrite);

                Log.Trace("UserLogout userId = {0}, localsid = {1}", userId, localSid);

                dynamic res = new DynamicStructure();
                res.retcode = 0;

                Session.Send(33, id, arg, res);
            }
        }
    }
}
