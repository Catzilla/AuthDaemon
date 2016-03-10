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
        public Dictionary<string, MySqlQuery> Queries { get; private set; }

        public MySqlDatabase(string connectionString)
        {
            MySqlConnection = new MySqlConnection(connectionString);
            Queries = new Dictionary<string, MySqlQuery>();
        }

        /*
        
<procedure name="acquireuserpasswd" connection="auth0" operate="replaceA">
 <parameter name="name1"     sql-type="varchar(64)"  java-type="java.lang.String"  in="true"  out="false" />
 <parameter name="uid1"      sql-type="integer"      java-type="java.lang.Integer" in="false" out="true" />
 <parameter name="passwd1"   sql-type="varchar(64)"  java-type="java.lang.String"  in="false" out="true" />
</procedure>
        */
        public override bool AcquireUserPassword(string name, out int uid, out string password)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name1",      MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("uid1",       MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("passwd1",    MySqlDbType.VarChar, 64) { Direction = ParameterDirection.Output }
            };
            var result = CallFunction("acquireuserpasswd", args);

            uid = (int)args[1].Value;
            password = (string)args[2].Value;
            return result;
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
        public override bool AddForbid(int userId, int type, int forbidTime, byte[] reason, int gmRoleId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("usedid1",        MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("type1",          MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("forbid_time1",   MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("reason1",        MySqlDbType.VarBinary, 255) { Value = reason },
                new MySqlParameter("gmroleid1",      MySqlDbType.Int32) { Value = userId }
            };
            var result = CallFunction("addForbid", args);
            return result;
        }
        public override bool AddUser(string name, byte[] password, string prompt, string answer, string trueName, string idNumber, string email, string mobileNumber, string province, string city, string phoneNumber, string address, string postalCode, int gender, string birthday, string qq, byte[] password2)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name1",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd1",        MySqlDbType.Binary, 16) { Value = password },
                new MySqlParameter("prompt1",        MySqlDbType.VarChar, 32) { Value = prompt },
                new MySqlParameter("answer1",        MySqlDbType.VarChar, 32) { Value = answer },
                new MySqlParameter("truename1",      MySqlDbType.VarChar, 32) { Value = trueName },
                new MySqlParameter("idnumber1",      MySqlDbType.VarChar, 32) { Value = idNumber },
                new MySqlParameter("email1",         MySqlDbType.VarChar, 64) { Value = email },
                new MySqlParameter("mobilenumber1",  MySqlDbType.VarChar, 32) { Value = mobileNumber },
                new MySqlParameter("province1",      MySqlDbType.VarChar, 32) { Value = province },
                new MySqlParameter("city1",          MySqlDbType.VarChar, 32) { Value = city },
                new MySqlParameter("phonenumber1",   MySqlDbType.VarChar, 32) { Value = phoneNumber },
                new MySqlParameter("address1",       MySqlDbType.VarChar, 64) { Value = address },
                new MySqlParameter("postalcode1",    MySqlDbType.VarChar, 8) { Value = postalCode },
                new MySqlParameter("gender1",        MySqlDbType.Int32) { Value = gender },
                new MySqlParameter("birthday1",      MySqlDbType.VarChar, 32) { Value = birthday },
                new MySqlParameter("qq1",            MySqlDbType.VarChar, 32) { Value = qq },
                new MySqlParameter("passwd21",       MySqlDbType.Binary, 16) { Value = password2 },
            };                      
            var result = CallFunction("adduser", args);
            return result;
        }
        public override bool UpdateUserInfo(string name, string prompt, string answer, string trueName, string idNumber, string email, string mobileNumber, string province, string city, string phoneNumber, string address, string postalCode, int gender, string birthday, string qq)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name1",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("prompt1",        MySqlDbType.VarChar, 32) { Value = prompt },
                new MySqlParameter("answer1",        MySqlDbType.VarChar, 32) { Value = answer },
                new MySqlParameter("truename1",      MySqlDbType.VarChar, 32) { Value = trueName },
                new MySqlParameter("idnumber1",      MySqlDbType.VarChar, 32) { Value = idNumber },
                new MySqlParameter("email1",         MySqlDbType.VarChar, 64) { Value = email },
                new MySqlParameter("mobilenumber1",  MySqlDbType.VarChar, 32) { Value = mobileNumber },
                new MySqlParameter("province1",      MySqlDbType.VarChar, 32) { Value = province },
                new MySqlParameter("city1",          MySqlDbType.VarChar, 32) { Value = city },
                new MySqlParameter("phonenumber1",   MySqlDbType.VarChar, 32) { Value = phoneNumber },
                new MySqlParameter("address1",       MySqlDbType.VarChar, 64) { Value = address },
                new MySqlParameter("postalcode1",    MySqlDbType.VarChar, 8) { Value = postalCode },
                new MySqlParameter("gender1",        MySqlDbType.Int32) { Value = gender },
                new MySqlParameter("birthday1",      MySqlDbType.VarChar, 32) { Value = birthday },
                new MySqlParameter("qq1",            MySqlDbType.VarChar, 32) { Value = qq },
            };
            var result = CallFunction("updateUserInfo", args);
            return result;
        }
        /*

        <procedure name="changePasswd" connection="auth0" operate="replaceA">
            <parameter name="name"   sql-type="varchar(64)" java-type="java.lang.String" in="true" out="false" />
            <parameter name="passwd" sql-type="binary(16)" java-type="java.lang.reflect.Array" in="true" out="false" />
        </procedure>
        */
        public override bool ChangePassword(string name, byte[] password)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name1",          MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd1",        MySqlDbType.Binary, 16) { Value = password },
            };
            var result = CallFunction("changePasswd", args);
            return result;
        }
        /*
    <procedure name="changePasswd2" connection="auth0" operate="replaceA">
        <parameter name="name" sql-type="varchar(64)" java-type="java.lang.String" in="true" out="false" />
        <parameter name="passwd2" sql-type="binary(16)" java-type="java.lang.reflect.Array" in="true" out="false" />
    </procedure>
        */
        public override bool ChangePassword2(string name, byte[] password2)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("name1",           MySqlDbType.VarChar, 64) { Value = name },
                new MySqlParameter("passwd21",        MySqlDbType.Binary, 16) { Value = password2 },
            };
            var result = CallFunction("changePasswd2", args);
            return result;
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
        public override bool SetIpLimit(int uid, int ipaddr1, string ipmask1, int ipaddr2, string ipmask2, int ipaddr3, string ipmask3, bool enable)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("ipaddr11",        MySqlDbType.Int32) { Value = ipaddr1 },
                new MySqlParameter("ipmask11",        MySqlDbType.VarChar, 2) { Value = ipmask1 },
                new MySqlParameter("ipaddr21",        MySqlDbType.Int32) { Value = ipaddr2 },
                new MySqlParameter("ipmask21",        MySqlDbType.VarChar, 2) { Value = ipmask2 },
                new MySqlParameter("ipaddr31",        MySqlDbType.Int32) { Value = ipaddr3 },
                new MySqlParameter("ipmask31",        MySqlDbType.VarChar, 2) { Value = ipmask3 },
            };
            var result = CallFunction("setiplimit", args);
            return result;
        }
        /*
<procedure name="enableiplimit" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="enable"   sql-type="char(1)"     java-type="java.lang.String" in="true" out="false"/>
</procedure>
        */
        public override bool EnableIpLimit(int uid, bool status)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("enable1",         MySqlDbType.Byte) { Value = (byte)(status ? 1 : 0)},
            };
            var result = CallFunction("setiplimit", args);
            return result;
        }
        /*
<procedure name="lockuser" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"     java-type="java.lang.Integer" in="true" out="false"/>
    <parameter name="lockstatus"   sql-type="char(1)"     java-type="java.lang.String" in="true" out="false"/>
</procedure>
        */
        public override bool LockUser(int uid, bool lockStatus)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",            MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("lockstatus1",     MySqlDbType.Byte) { Value = (byte)(lockStatus ? 1 : 0)},
            };
            var result = CallFunction("lockuser", args);
            return result;
        }
        /*

<procedure name="addUserPriv" connection="auth0" operate="replaceA">
    <parameter name="userid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="rid"      sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
</procedure>
        */
        public override bool AddUserPrivilege(int userId, int zoneId, int rid)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid1",            MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid1",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("rid1",               MySqlDbType.Int32) { Value = rid },
            };
            var result = CallFunction("addUserPriv", args);
            return result;
        }
        /*
<procedure name="delUserPriv" connection="auth0" operate="replaceA">
    <parameter name="userid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="rid"      sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
    <parameter name="deltype"  sql-type="integer"  java-type="java.lang.Integer"   in="true" out="false" />
</procedure>
        */
        public override bool DeleteUserPrivilege(int userId, int zoneId, int rid, int deleteType)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid1",            MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid1",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("rid1",               MySqlDbType.Int32) { Value = rid },
                new MySqlParameter("deltype1",           MySqlDbType.Int32) { Value = deleteType },
            };
            var result = CallFunction("delUserPriv", args);
            return result;
        }
        /*
<procedure name="deleteTimeoutForbid" connection="auth0" operate="replaceA">
    <parameter name="userid"	sql-type="integer" java-type="java.lang.Integer" in="true" out="false"/>
</procedure>
        */
        public override bool DeleteTimeoutForbid(int userId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid1",            MySqlDbType.Int32) { Value = userId },
            };
            var result = CallFunction("deleteTimeoutForbid", args);
            return result;
        }
        /*

<procedure name="clearonlinerecords" connection="auth0" operate="replaceA">
    <parameter name="zoneid"   sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"     in="true"  out="false" />
</procedure>
        */
        public override bool ClearOnlineRecords(int zoneId, int aid)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("zoneid1",            MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("aid1",               MySqlDbType.Int32) { Value = aid }
            };
            var result = CallFunction("clearonlinerecords", args);
            return result;
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
        public override bool RecordOnline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid1",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("zoneid1",            MySqlDbType.Int32) { Value = zoneId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("zonelocalid1",       MySqlDbType.Int32) { Value = zoneLocalId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("overwrite",         MySqlDbType.Int32) { Value = overwrite, Direction = ParameterDirection.InputOutput },
            };
            var result = CallFunction("recordonline", args);

            zoneId = (int)args[2].Value;
            zoneLocalId = (int)args[3].Value;
            overwrite = (int)args[4].Value;
            return result;
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
        public override bool RecordOffline(int uid, int aid, ref int zoneId, ref int zoneLocalId, ref int overwrite)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid1",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("zoneid1",            MySqlDbType.Int32) { Value = zoneId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("zonelocalid1",       MySqlDbType.Int32) { Value = zoneLocalId, Direction = ParameterDirection.InputOutput },
                new MySqlParameter("overwrite1",         MySqlDbType.Int32) { Value = overwrite, Direction = ParameterDirection.InputOutput },
            };
            var result = CallFunction("recordoffline", args);

            zoneId = args[2].Value.Safe<int>();
            zoneLocalId = args[3].Value.Safe<int>();
            overwrite = args[4].Value.Safe<int>();
            return result;
        }
        /*
<procedure name="remaintime" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="remain"   sql-type="integer"  java-type="java.lang.Integer"    in="false" out="true" />
    <parameter name="freetimeleft" sql-type="integer" java-type="java.lang.Integer" in="false" out="true" />
</procedure>
        */
        public override bool RemainTime(int uid, int aid, out int remain, out int freeTimeLeft)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid1",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("remain1",            MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("freetimeleft1",      MySqlDbType.Int32) { Direction = ParameterDirection.Output },
            };
            var result = CallFunction("remaintime", args);

            remain = (int)args[1].Value;
            freeTimeLeft = (int)args[2].Value;
            return result;
        }
        /*
<procedure name="adduserpoint" connection="auth0" operate="replaceA">
    <parameter name="uid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="aid"      sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
    <parameter name="time"     sql-type="integer"  java-type="java.lang.Integer"    in="true"  out="false" />
</procedure>
        */
        public override bool AddUserPoint(int uid, int aid, int time)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("uid1",               MySqlDbType.Int32) { Value = uid },
                new MySqlParameter("aid1",               MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("time1",              MySqlDbType.Int32) { Value = time },
            };
            var result = CallFunction("adduserpoint", args);
            return result;
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
        public override bool UseCash(int userId, int zoneId, int sn, int aid, int point, int cash, int status, out int error)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid1",                MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid1",                MySqlDbType.Int32) { Value = zoneId },
                new MySqlParameter("sn1",                    MySqlDbType.Int32) { Value = sn },
                new MySqlParameter("aid1",                   MySqlDbType.Int32) { Value = aid },
                new MySqlParameter("point1",                 MySqlDbType.Int32) { Value = point },
                new MySqlParameter("cash1",                  MySqlDbType.Int32) { Value = cash },
                new MySqlParameter("status1",                MySqlDbType.Int32) { Value = status },
                new MySqlParameter("error1",                 MySqlDbType.Int32) { Direction = ParameterDirection.Output },
            };
            var result = CallFunction("usecash", args);

            error = (int)args[7].Value;
            return result;
        }
        public override bool AddGM(int userId, int zoneId)
        {
            var args = new MySqlParameter[]
            {
                new MySqlParameter("userid1",                MySqlDbType.Int32) { Value = userId },
                new MySqlParameter("zoneid1",                MySqlDbType.Int32) { Value = zoneId },
            };
            var result = CallFunction("addGM", args);
            return result;
        }

        public override IEnumerable<dynamic> Query(string name, params object[] args)
        {
            if (MySqlConnection.State != ConnectionState.Open)
            {
                Connect();
            }
            var index = name.IndexOf('.');
            var baseName = name;
            if (index != -1)
            {
                baseName = name.Substring(0, index);
            }

            var query = Queries[baseName];
            using (var command = new MySqlCommand(query.Select(name), MySqlConnection))
            {
                command.CommandType = CommandType.Text;

                for (var i = 0; i < args.Length; i++)
                {
                    command.Parameters.Add(new MySqlParameter("argument" + i, args[i]));
                }

                using (var reader = command.ExecuteReader())
                {

                    var schema = reader.GetSchemaTable();
                    var rows = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        rows.Add(row.Field<string>("ColumnName"));
                    }
                    while (reader.Read())
                    {
                        var obj = new DynamicStructure();
                        foreach (var row in rows)
                        {
                            obj.Dictionary[row.ToLower()] = reader[row];
                        }
                        yield return obj;
                    }
                }
            }
        }
        public bool CallFunction(string functionName, params MySqlParameter[] parameters)
        {
            if (MySqlConnection.State != ConnectionState.Open)
            {
                Connect();
            }
            using (var command = new MySqlCommand(functionName, MySqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery() != 0;
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
