// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Threading;

namespace AuthDaemon.Data
{
    public class MySqlDatabase : Database
    {
        public MySqlConnection MySqlConnection { get; private set; }

        public MySqlDatabase(string connectionString)
        {
            MySqlConnection = new MySqlConnection(connectionString);
        }

        /*
        
<procedure name="acquireuserpasswd" connection="auth0" operate="replaceA">
 <parameter name="name1"     sql-type="varchar(64)"  java-type="java.lang.String"  in="true"  out="false" />
 <parameter name="uid1"      sql-type="integer"      java-type="java.lang.Integer" in="false" out="true" />
 <parameter name="passwd1"   sql-type="varchar(64)"  java-type="java.lang.String"  in="false" out="true" />
</procedure>
        */
        public override void AcquireUserPassword(string name, out int uid, out string password)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name",      MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("uid",       MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("passwd",    MySqlDbType.VarChar, 64) { Direction = ParameterDirection.Output }
            };
            CallFunction("acquireuserpasswd", args);

            uid = (int)args[1].Value;
            password = (string)args[2].Value;
        }
        /*

<procedure name="addForbid" connection="auth0" operate="replaceA">
    <parameter name="userid"   sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="type"     sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <!--parameter name="ctime" sql-type="datetime"    java-type="java.util.Date" 	in="true" out="false"/-->
    <parameter name="forbid_time" sql-type="integer"  java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="reason"   sql-type="varbinary(255)" java-type="java.lang.reflect.Array"  in="true" out="false"/>
    <parameter name="gmroleid" sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
</procedure>
        */
        public override void AddForbid(int userId, int type, int forbidTime, byte[] reason, int gmRoleId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("usedid",        MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("type",          MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("forbid_time",   MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("reason",        MySqlDbType.VarBinary, 255) { Value = reason },
                new MySqlParameter("gmroleid",      MySqlDbType.Int32) { Value = userId }
            };
            CallFunction("addForbid", args);
        }
        public override void AddUser(string name, byte[] password, string prompt, string answer, string trueName, string idNumber, string email, string mobileNumber, string province, string city, string phoneNumber, string address, string postalCode, int gender, string birthday, string qq, byte[] password2)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd",        MySqlDbType.Binary, 16) { Value = password },
                new MySqlParameter("prompt",        MySqlDbType.VarChar, 32) { Value = prompt },
                new MySqlParameter("answer",        MySqlDbType.VarChar, 32) { Value = answer },
                new MySqlParameter("truename",      MySqlDbType.VarChar, 32) { Value = trueName },
                new MySqlParameter("idnumber",      MySqlDbType.VarChar, 32) { Value = idNumber },
                new MySqlParameter("email",         MySqlDbType.VarChar, 64) { Value = email },
                new MySqlParameter("mobilenumber",  MySqlDbType.VarChar, 32) { Value = mobileNumber },
                new MySqlParameter("province",      MySqlDbType.VarChar, 32) { Value = province },
                new MySqlParameter("city",          MySqlDbType.VarChar, 32) { Value = city },
                new MySqlParameter("phonenumber",   MySqlDbType.VarChar, 32) { Value = phoneNumber },
                new MySqlParameter("address",       MySqlDbType.VarChar, 64) { Value = address },
                new MySqlParameter("postalcode",    MySqlDbType.VarChar, 8) { Value = postalCode },
                new MySqlParameter("gender",        MySqlDbType.Int32) { Value = gender },
                new MySqlParameter("birthday",      MySqlDbType.VarChar, 32) { Value = birthday },
                new MySqlParameter("qq",            MySqlDbType.VarChar, 32) { Value = qq },
                new MySqlParameter("passwd2",       MySqlDbType.Binary, 16) { Value = password2 },
            };                      
            CallFunction("adduser", args);
        }
        public override void UpdateUserInfo(string name, string prompt, string answer, string trueName, string idNumber, string email, string mobileNumber, string province, string city, string phoneNumber, string address, string postalCode, int gender, string birthday, string qq)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("prompt",        MySqlDbType.VarChar, 32) { Value = prompt },
                new MySqlParameter("answer",        MySqlDbType.VarChar, 32) { Value = answer },
                new MySqlParameter("truename",      MySqlDbType.VarChar, 32) { Value = trueName },
                new MySqlParameter("idnumber",      MySqlDbType.VarChar, 32) { Value = idNumber },
                new MySqlParameter("email",         MySqlDbType.VarChar, 64) { Value = email },
                new MySqlParameter("mobilenumber",  MySqlDbType.VarChar, 32) { Value = mobileNumber },
                new MySqlParameter("province",      MySqlDbType.VarChar, 32) { Value = province },
                new MySqlParameter("city",          MySqlDbType.VarChar, 32) { Value = city },
                new MySqlParameter("phonenumber",   MySqlDbType.VarChar, 32) { Value = phoneNumber },
                new MySqlParameter("address",       MySqlDbType.VarChar, 64) { Value = address },
                new MySqlParameter("postalcode",    MySqlDbType.VarChar, 8) { Value = postalCode },
                new MySqlParameter("gender",        MySqlDbType.Int32) { Value = gender },
                new MySqlParameter("birthday",      MySqlDbType.VarChar, 32) { Value = birthday },
                new MySqlParameter("qq",            MySqlDbType.VarChar, 32) { Value = qq },
            };
            CallFunction("updateUserInfo", args);
        }
        /*

        <procedure name="changePasswd" connection="auth0" operate="replaceA">
            <parameter name="name"   sql-type="varchar(64)" java-type="java.lang.String" in="true" out="false" />
            <parameter name="passwd" sql-type="binary(16)" java-type="java.lang.reflect.Array" in="true" out="false" />
        </procedure>
        */
        public override void ChangePassword(string name, byte[] password)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd",        MySqlDbType.Binary, 16) { Value = password },
            };
            CallFunction("changePasswd", args);
        }
        /*
    <procedure name="changePasswd2" connection="auth0" operate="replaceA">
        <parameter name="name" sql-type="varchar(64)" java-type="java.lang.String" in="true" out="false" />
        <parameter name="passwd2" sql-type="binary(16)" java-type="java.lang.reflect.Array" in="true" out="false" />
    </procedure>
        */
        public override void ChangePassword2(string name, byte[] password2)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd2",        MySqlDbType.Binary, 16) { Value = password2 },
            };
            CallFunction("changePasswd2", args);
        }
        /*
<procedure name="setiplimit" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="ipaddr1"  sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="ipmask1"  sql-type="varchar(2)"  java-type="java.lang.String" in="true" out="false"/>
    <parameter name="ipaddr2"  sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="ipmask2"  sql-type="varchar(2)"  java-type="java.lang.String" in="true" out="false"/>
    <parameter name="ipaddr3"  sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="ipmask3"  sql-type="varchar(2)"  java-type="java.lang.String" in="true" out="false"/>
</procedure>
        */
        public override void SetIpLimit(int uid, int ipaddr1, string ipmask1, int ipaddr2, string ipmask2, int ipaddr3, string ipmask3, bool enable)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("ipaddr1",        MySqlDbType.Int32) { Value = ipaddr1 },
                new MySqlParameter("ipmask1",        MySqlDbType.VarChar, 2) { Value = ipmask1 },
                new MySqlParameter("ipaddr2",        MySqlDbType.Int32) { Value = ipaddr2 },
                new MySqlParameter("ipmask2",        MySqlDbType.VarChar, 2) { Value = ipmask2 },
                new MySqlParameter("ipaddr3",        MySqlDbType.Int32) { Value = ipaddr3 },
                new MySqlParameter("ipmask3",        MySqlDbType.VarChar, 2) { Value = ipmask3 },
            };
            CallFunction("setiplimit", args);
        }
        /*
<procedure name="enableiplimit" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="enable"   sql-type="char(1)"     java-type="java.lang.String" in="true" out="false"/>
</procedure>
        */
        public override void EnableIpLimit(int uid, bool status)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("enable",         MySqlDbType.Byte) { Value = (byte)(status ? 1 : 0)},
            };
            CallFunction("setiplimit", args);
        }
        /*
<procedure name="lockuser" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="lockstatus"   sql-type="char(1)"     java-type="java.lang.String" in="true" out="false"/>
</procedure>
        */
        public override void LockUser(int uid, bool lockStatus)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("lockstatus",     MySqlDbType.Byte) { Value = (byte)(lockStatus ? 1 : 0)},
            };
            CallFunction("lockuser", args);
        }
        /*

<procedure name="addUserPriv" connection="auth0" operate="replaceA">
    <parameter name="userid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="rid"      sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
</procedure>
        */
        public override void AddUserPrivilege(int userId, int zoneId, int rid)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid",            MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("rid",               MySqlDbType.Int32) { Value = rid },
            };
            CallFunction("addUserPriv", args);
        }
        /*
<procedure name="delUserPriv" connection="auth0" operate="replaceA">
    <parameter name="userid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="rid"      sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="deltype"  sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
</procedure>
        */
        public override void DeleteUserPrivilege(int userId, int zoneId, int rid, int deleteType)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid",            MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("rid",               MySqlDbType.Int32) { Value = rid },
                new MySqlParameter("deltype",           MySqlDbType.Int32) { Value = deleteType },
            };
            CallFunction("delUserPriv", args);
        }
        /*
<procedure name="deleteTimeoutForbid" connection="auth0" operate="replaceA">
    <parameter name="userid"	sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
</procedure>
        */
        public override void DeleteTimeoutForbid(int userId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid",            MySqlDbType.Int32) { Value = userId },
            };
            CallFunction("deleteTimeoutForbid", args);
        }
        /*

<procedure name="clearonlinerecords" connection="auth0" operate="replaceA">
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
</procedure>
        */
        public override void ClearOnlineRecords(int zoneId, int aid)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("zoneid",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("aid",               MySqlDbType.Int32) { Value = aid }
            };
            CallFunction("clearonlinerecords", args);
        }
        /*

<procedure name="recordonline" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="true" />
    <parameter name="zonelocalid" sql-type="integer"  java-type="java.lang.Integer"  in="true"  out="true" />
    <parameter name="overwrite" sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="true"  />
</procedure>
        */
        public override void RecordOnline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("zoneid",            MySqlDbType.Int32) { Value = zoneId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("zonelocalid",       MySqlDbType.Int32) { Value = zoneLocalId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("overwrite",         MySqlDbType.Int32) { Value = overwrite, Direction = ParameterDirection.InputOutput },
            };
            CallFunction("recordonline", args);

            zoneId = (int)args[2].Value;
            zoneLocalId = (int)args[3].Value;
            overwrite = (int)args[4].Value;
        }
        /*
<procedure name="recordoffline" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="true" />
    <parameter name="zonelocalid" sql-type="integer"  java-type="java.lang.Integer"  in="true"  out="true" />
    <parameter name="overwrite" sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="true" />
</procedure>
        */
        public override void RecordOffline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("zoneid",            MySqlDbType.Int32) { Value = zoneId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("zonelocalid",       MySqlDbType.Int32) { Value = zoneLocalId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("overwrite",         MySqlDbType.Int32) { Value = overwrite, Direction = ParameterDirection.InputOutput },
            };
            CallFunction("recordoffline", args);

            zoneId = (int)args[2].Value;
            zoneLocalId = (int)args[3].Value;
            overwrite = (int)args[4].Value;
        }
        /*
<procedure name="remaintime" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="remain"   sql-type="integer"  java-type="java.lang.Integer"    in="false" out="true" />
    <parameter name="freetimeleft" sql-type="integer" java-type="java.lang.Integer" in="false" out="true" />
</procedure>
        */
        public override void RemainTime(int uid, int aid, out int remain, out int freeTimeLeft)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("remain",            MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("freetimeleft",      MySqlDbType.Int32) { Direction = ParameterDirection.Output },
            };
            CallFunction("remaintime", args);

            remain = (int)args[1].Value;
            freeTimeLeft = (int)args[2].Value;
        }
        /*
<procedure name="adduserpoint" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="time"     sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
</procedure>
        */
        public override void AddUserPoint(int uid, int aid, int time)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("time",              MySqlDbType.Int32) { Value = time },
            };
            CallFunction("adduserpoint", args);
        }
        /*

<procedure name="usecash" connection="auth0" operate="replaceA">
    <parameter name="userid" sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="zoneid" sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="sn"     sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="aid"    sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="point"  sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="cash"   sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="status" sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="error"  sql-type="integer" java-type="java.lang.Integer" in="false" out="true"/>
</procedure>
        */
        public override void UseCash(int userId, int zoneId, int sn, int aid, int point, int cash, int status, out int error)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid",                MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid",                MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("sn",                    MySqlDbType.Int32) { Value = sn },
                new MySqlParameter("aid",                   MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("point",                 MySqlDbType.Int32) { Value = point },
                new MySqlParameter("cash",                  MySqlDbType.Int32) { Value = cash },
                new MySqlParameter("status",                MySqlDbType.Int32) { Value = status },
                new MySqlParameter("error",                 MySqlDbType.Int32) { Direction = ParameterDirection.Output },
            };
            CallFunction("usecash", args);

            error = (int)args[7].Value;
        }
        public override void AddGM(int userId, int zoneId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid",                MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid",                MySqlDbType.Int32) { Value = zoneId },
            };
            CallFunction("addGM", args);
        }
        public void CallFunction(string functionName, params MySqlParameter[] parameters)
        {
            foreach(var param in parameters)
            {
                param.ParameterName += "1";
            }
            if (MySqlConnection.State != ConnectionState.Open)
            {
                Connect();
            }
            using (var command = new MySqlCommand(functionName, MySqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }
        public void Connect()
        {
            lock (MySqlConnection)
            {
                while(MySqlConnection.State == ConnectionState.Connecting)
                {
                    Thread.Sleep(10);
                }
                if (MySqlConnection.State == ConnectionState.Open)
                {
                    return;
                }
                MySqlConnection.Open();
            }
        }
    }
}
