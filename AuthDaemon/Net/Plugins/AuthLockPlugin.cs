using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.Net.Plugins
{
    public class AuthLockPlugin : Plugin
    {
        public override string ConfigBlock
        {
            get
            {
                return "AuthLock";
            }
        }
        public override string Name
        {
            get
            {
                return "authlock";
            }
        }

        public int LockInterval { get; set; }
        public byte Retcode { get; set; }

        private Queue<int> lockQueue = new Queue<int>();
        private Dictionary<int, DateTime> lockStart = new Dictionary<int, DateTime>();

        public override void Initialize()
        {
            LockInterval = int.Parse(GetVar("interval").Safe("0"));
            Retcode = byte.Parse(GetVar("retcode").Safe("10"));

            Session.Handler.AddHandler(0x0F, OnUserLogin, 10);
            Session.Handler.AddHandler(33, OnUserLogout, 10);
        }

        private bool CheckTime(DateTime dt)
        {
            return (DateTime.Now - dt).TotalMilliseconds < LockInterval;
        }
        public void Flush()
        {
            lock (lockStart)
            {
                while (lockQueue.Count > 0 && !CheckTime(lockStart[lockQueue.Peek()]))
                {
                    var id = lockQueue.Dequeue();
                    lockStart.Remove(id);
                }
            }
        }
        public bool CheckLock(int userId)
        {
            lock (lockStart)
            {
                Flush();
                return lockStart.ContainsKey(userId);
            }
        }
        public void UpdateLock(int userId)
        {
            lock (lockStart)
            {
                Flush();
                if (lockStart.ContainsKey(userId)) return;

                lockStart[userId] = DateTime.Now;
                lockQueue.Enqueue(userId);
            }
        }

        private void OnUserLogin(object sender, PacketEventArgs e)
        {
            int id = e.Packet.id;
            var arg = e.Packet.arg;

            int userId = arg.userId;

            Log.Trace("Auth lock userId = {0}, check = {1}, interval = {2}", userId, CheckLock(userId), LockInterval);

            if (CheckLock(userId))
            {
                e.Handled = true;

                dynamic res = new DynamicStructure();
                res.retcode = Retcode;

                Session.Send(0x0F, id, arg, res);
            }
            else
            {
                UpdateLock(userId);
            }
        }
        private void OnUserLogout(object sender, PacketEventArgs e)
        {
            int id = e.Packet.id;
            var arg = e.Packet.arg;

            int userId = arg.userId;
            UpdateLock(userId);
        }
    }
}
