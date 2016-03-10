// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.Data
{
    public abstract class Database
    {
        public virtual bool AcquireUserPassword(string name, out int uid, out string password)
        {
            throw new NotImplementedException();
        }
        public virtual bool AddForbid(int userId, int type, int forbidTime, byte[] reason, int gmRoleId)
        {
            throw new NotImplementedException();
        }
        public virtual bool AddGM(int userId, int zoneId)
        {
            throw new NotImplementedException();
        }
        public virtual bool AddUser(
            string name, 
            byte[] password, 
            string prompt, 
            string answer, 
            string trueName, 
            string idNumber,
            string email, 
            string mobileNumber, 
            string province,
            string city,
            string phoneNumber, 
            string address, 
            string postalCode,
            int gender,
            string birthday,
            string qq, 
            byte[] password2)
        {
            throw new NotImplementedException();
        }
        public virtual bool AddUserPoint(int uid, int aid, int time)
        {
            throw new NotImplementedException();
        }
        public virtual bool AddUserPrivilege(int userId, int zoneId, int rid)
        {
            throw new NotImplementedException();
        }
        public virtual bool ChangePassword(string name, byte[] password)
        {
            throw new NotImplementedException();
        }
        public virtual bool ChangePassword2(string name, byte[] password2)
        {
            throw new NotImplementedException();
        }
        public virtual bool ClearOnlineRecords(int zoneId, int aid)
        {
            throw new NotImplementedException();
        }
        public virtual bool DeleteTimeoutForbid(int userId)
        {
            throw new NotImplementedException();
        }
        public virtual bool DeleteUserPrivilege(int userId, int zoneId, int rid, int deleteType)
        {
            throw new NotImplementedException();
        }
        public virtual bool EnableIpLimit(int uid, bool status)
        {
            throw new NotImplementedException();
        }
        public virtual bool LockUser(int uid, bool lockStatus)
        {
            throw new NotImplementedException();
        }
        public virtual bool RecordOffline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            throw new NotImplementedException();
        }
        public virtual bool RecordOnline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            throw new NotImplementedException();
        }
        public virtual bool RemainTime(int uid, int aid, out int remain, out int freeTimeLeft)
        {
            throw new NotImplementedException();
        }
        public virtual bool SetIpLimit(
            int uid, 
            int ipaddr1, string ipmask1, 
            int ipaddr2, string ipmask2, 
            int ipaddr3, string ipmask3, 
            bool enable)
        {
            throw new NotImplementedException();
        }
        public virtual bool UpdateUserInfo(
            string name,
            string prompt,
            string answer,
            string trueName,
            string idNumber,
            string email,
            string mobileNumber,
            string province,
            string city,
            string phoneNumber,
            string address,
            string postalCode,
            int gender,
            string birthday,
            string qq)
        {
            throw new NotImplementedException();
        }
        public virtual bool UseCash(
            int userId, 
            int zoneId, 
            int sn, 
            int aid, 
            int point, 
            int cash,
            int status,
            out int error)
        {
            throw new NotImplementedException();
        }
        public virtual IEnumerable<dynamic> Query(string name, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
