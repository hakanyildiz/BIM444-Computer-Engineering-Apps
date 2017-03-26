using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class data
    {
        public const string connstr = "Server=tcp:duyarliol1.database.windows.net,1433;Initial Catalog=duyarliol1;Persist Security Info=False;User ID=duyarliol1;Password=a2gf2a424gfk.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //public const string connstr = @"Data Source=duyarliol1.database.windows.net;Initial Catalog=duyarliol1;Integrated Security=False;User ID=duyarliol1;Password=a2gf2a424gfk;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        SqlConnection conn;
        SqlCommand cmd;
        public data() { conn = new SqlConnection(connstr); cmd = conn.CreateCommand(); }

        #region site functions

        public user getuserinfo(int id)
        {
            try
            {
                using (conn)
                {

                    cmd.CommandText = "select fullname,apikey,apisecret,status,password,url,namesurname,birthday,avatar,background,registerdate,lastlogindate from users where id=@a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            return new user()
                            {
                                id = id,
                                username = !r.IsDBNull(0) ? r.GetString(0) : "",
                                apikey = !r.IsDBNull(1) ? r.GetString(1) : "",
                                apisecret = !r.IsDBNull(2) ? r.GetString(2) : "",
                                status = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                password = !r.IsDBNull(4) ? r.GetString(4) : "",
                                url = !r.IsDBNull(5) ? r.GetString(5) : "",
                                namesurname = !r.IsDBNull(6) ? r.GetString(6) : "",
                                birthday = !r.IsDBNull(7) ? r.GetDateTime(7).ToString("dd/MM/yyyy") : "",
                                avatar = !r.IsDBNull(8) ? r.GetString(8) : "",
                                background = !r.IsDBNull(9) ? r.GetString(9) : "",
                                registerdate = !r.IsDBNull(10) ? r.GetDateTime(10) : DateTime.Now,
                                lastlogindate = !r.IsDBNull(11) ? r.GetDateTime(11) : DateTime.Now

                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public user getuserinfo(string username)
        {
            try
            {
                using (conn)
                {
                    cmd.CommandText = "select id,apikey,apisecret,status,password,url,namesurname,birthday,avatar,background,registerdate,lastlogindate from users where fullname like @a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", username);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            return new user()
                            {
                                username = username,
                                id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                apikey = !r.IsDBNull(1) ? r.GetString(1) : "",
                                apisecret = !r.IsDBNull(2) ? r.GetString(2) : "",
                                status = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                password = !r.IsDBNull(4) ? r.GetString(4) : "",
                                url = !r.IsDBNull(5) ? r.GetString(5) : "",
                                namesurname = !r.IsDBNull(6) ? r.GetString(6) : "",
                                birthday = !r.IsDBNull(7) ? r.GetDateTime(7).ToString("dd/MM/yyyy") : "",
                                avatar = !r.IsDBNull(8) ? r.GetString(8) : "",
                                background = !r.IsDBNull(9) ? r.GetString(9) : "",
                                registerdate = !r.IsDBNull(10) ? r.GetDateTime(10) : DateTime.Now,
                                lastlogindate = !r.IsDBNull(11) ? r.GetDateTime(11) : DateTime.Now
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<user> getfriendsfollowingchannelId(int channelId, int currentUserId)
        {
            try
            {
                List<user> list = new List<user>();
                using (conn)
                {
                    cmd.CommandText = string.Format("select users.fullname,users.url,users.avatar from users inner join auths on users.id = auths.userid where auths.authtype = 2 and auths.followlist like '%{0}%' and users.id != {1}", channelId, currentUserId);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@channel", channelId);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new user()
                            {
                                username = !r.IsDBNull(0) ? r.GetString(0) : "",
                                url = !r.IsDBNull(1) ? r.GetString(1) : "",
                                avatar = !r.IsDBNull(2) ? r.GetString(2) : ""
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public income getuserincome(int id)
        {
            try
            {
                using (conn)
                {

                    cmd.CommandText = "select jobtype, monthlyincome, monthlyadditionalincome, uploaddate from userincome where userid=@a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            return new income()
                            {
                                jobtype = !r.IsDBNull(0) ? r.GetString(0) : "",
                                monthlyincome = !r.IsDBNull(1) ? r.GetInt32(1) : 0,
                                monthlyadditionalincome = !r.IsDBNull(2) ? r.GetInt32(2) : 0,
                                date = !r.IsDBNull(3) ? r.GetDateTime(3) : DateTime.Now
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public outcome getuseroutcome(int id)
        {
            try
            {
                using (conn)
                {

                    cmd.CommandText = "select houserent,electricbill,waterbill,gasbill,internetbill,gsmbill,otherbills,individualexpense,marketexpense,updatedate from useroutcome where userid=@a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            return new outcome()
                            {
                                houserent = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                electricbill = !r.IsDBNull(1) ? r.GetInt32(1) : 0,
                                waterbill = !r.IsDBNull(2) ? r.GetInt32(2) : 0,
                                gasbill = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                internetbill = !r.IsDBNull(4) ? r.GetInt32(4) : 0,
                                gsmbill = !r.IsDBNull(5) ? r.GetInt32(5) : 0,
                                otherbills = !r.IsDBNull(6) ? r.GetInt32(6) : 0,
                                individualexpense = !r.IsDBNull(7) ? r.GetInt32(7) : 0,
                                marketexpense = !r.IsDBNull(8) ? r.GetInt32(8) : 0,
                                date = !r.IsDBNull(9) ? r.GetDateTime(9) : DateTime.Now
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<creditcard> getusercreditcards(int id)
        {
            try
            {
                List<creditcard> list = new List<creditcard>();
                using (conn)
                {
                    cmd.CommandText = "select id, bankname, cardlimit, carddebt, updatedate from usercreditcards where userid=@a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new creditcard()
                            {
                                id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                bankname = !r.IsDBNull(1) ? r.GetString(1) : "",
                                cardlimit = !r.IsDBNull(2) ? r.GetInt32(2) : 0,
                                carddebt = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                date = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now 
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
        #region api functions

        public async Task<user> getuserinfoasyncforapi(string apikey)
        {
            try
            {
                using (conn)
                {
                    cmd.CommandText = "select id,apisecret,status from users where apikey like @a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", apikey);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = await cmd.ExecuteReaderAsync())
                    {
                        while (await r.ReadAsync())
                        {
                            return new user()
                            {
                                apikey = apikey,
                                id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                apisecret = !r.IsDBNull(1) ? r.GetString(1) : "",
                                status = !r.IsDBNull(2) ? r.GetInt32(2) : 0
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<basic>> getgamesasync(int top, string query)
        {
            try
            {
                List<basic> list = new List<basic>();
                using (conn)
                {
                    cmd.CommandText = string.Format("select {0} name,url,picture,background,video from kalgame {1} {2}", top > 0 ? "top(@a)" : "", !string.IsNullOrEmpty(query) ? "where (name like '%'+@term1+'%' or searchkeywords like '%'+@term2+'%')" : "", !string.IsNullOrEmpty(query) ? "order by name" : "order by id desc");
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", top);
                    if (!string.IsNullOrEmpty(query))
                    {
                        cmd.Parameters.AddWithValue("@term1", query.ToLower());
                        cmd.Parameters.AddWithValue("@term2", string.Format("({0})", query.ToLower()));
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = await cmd.ExecuteReaderAsync())
                    {
                        while (await r.ReadAsync())
                        {
                            list.Add(new basic()
                            {
                                id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                value = !r.IsDBNull(1) ? r.GetString(1) : "",
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region helpers

        public int checkdb(string table, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table)) return 0;
                using (conn)
                {
                    cmd.Parameters.Clear();

                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@term{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@term{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("select count(*) from {0} {1}", table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool updatedb(string table, List<db> valuelist, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || valuelist == null || valuelist.Count == 0) return false;
                using (conn)
                {
                    cmd.Parameters.Clear();

                    StringBuilder query = new StringBuilder(), terms = new StringBuilder();
                    for (int i = 0; i < valuelist.Count; i++)
                    {
                        query.AppendFormat("{0}=@value{1},", valuelist[i].column, i);
                        cmd.Parameters.AddWithValue(string.Format("@value{0}", i), valuelist[i].value);
                    }
                    if (query.Length > 0) query.Remove(query.Length - 1, 1);

                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@term{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@term{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("update {0} set {1} {2}", table, query.ToString(), terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool insertdb(string table, List<db> valuelist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || valuelist == null || valuelist.Count == 0) return false;
                using (conn)
                {
                    StringBuilder cols = new StringBuilder(), vals = new StringBuilder();

                    cmd.Parameters.Clear();

                    for (int i = 0; i < valuelist.Count; i++)
                    {
                        cols.AppendFormat("{0},", valuelist[i].column);
                        vals.AppendFormat("@value{0},", i);
                        cmd.Parameters.AddWithValue(string.Format("@value{0}", i), valuelist[i].value);
                    }
                    if (cols.Length > 0) cols.Remove(cols.Length - 1, 1);
                    if (vals.Length > 0) vals.Remove(vals.Length - 1, 1);

                    cmd.CommandText = string.Format("insert into {0} ({1}) values ({2})", table, cols.ToString(), vals.ToString());
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool removedb(string table, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table)) return false;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@value{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("delete from {0} {1}", table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object getsinglecolumndb(string table, string column, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column)) return null;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@value{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("select {0} from {1} {2}", column, table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object getsinglecolumndbcount(string table, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table)) return null;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@value{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }
                    string column = "count(*)";

                    cmd.CommandText = string.Format("select {0} from {1} {2}", column, table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }



                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object getDoubleColumndb(string table, string column1, string column2, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column1) || string.IsNullOrEmpty(column2)) return null;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0}=@value{1} and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("select {0}, {1} from {2} {3}", column1, column2, table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object getsinglecolumndbViaLikeParam(string table, string column, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column)) return null;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0} like '%'+@value{1}+'%' and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("select {0} from {1} {2}", column, table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object getDoubleColumndbViaLikeParam(string table, string column1, string column2, List<db> termlist)
        {
            try
            {
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column1) || string.IsNullOrEmpty(column2)) return null;
                using (conn)
                {
                    cmd.Parameters.Clear();
                    StringBuilder terms = new StringBuilder();
                    if (termlist != null && termlist.Count > 0)
                    {
                        terms.Append("where ");
                        for (int i = 0; i < termlist.Count; i++)
                        {
                            terms.AppendFormat("{0} like '%'+@value{1}+'%' and ", termlist[i].column, i);
                            cmd.Parameters.AddWithValue(string.Format("@value{0}", i), termlist[i].value);
                        }
                        if (terms.Length > 0) terms.Remove(terms.Length - 4, 4);
                    }

                    cmd.CommandText = string.Format("select {0} + ' ' + {1} from {2} {3}", column1, column2, table, terms.Length > 0 ? terms.ToString() : "");
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }

                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string createhash(string value)
        {
            using (System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256Managed.Create())
            {
                return string.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
        public string createrandomstring(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public string createpassword(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                int minSaltSize = 4;
                int maxSaltSize = 8;
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);
                saltBytes = new byte[saltSize];
                System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);
            }
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];
            for (int i = 0; i < plainTextBytes.Length; i++) plainTextWithSaltBytes[i] = plainTextBytes[i];
            for (int i = 0; i < saltBytes.Length; i++) plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            System.Security.Cryptography.HashAlgorithm hash;
            if (hashAlgorithm == null) hashAlgorithm = "";
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA384":
                    hash = new System.Security.Cryptography.SHA384Managed();
                    break;
                case "SHA512":
                    hash = new System.Security.Cryptography.SHA512Managed();
                    break;
                default:
                    hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    break;
            }
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];
            for (int i = 0; i < saltBytes.Length; i++) hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);
            return hashValue;
        }
        public bool verifypassword(string plainText, string hashAlgorithm, string hashValue)
        {
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);
            int hashSizeInBits, hashSizeInBytes;
            if (hashAlgorithm == null) hashAlgorithm = "";
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default:
                    hashSizeInBits = 128;
                    break;
            }
            hashSizeInBytes = hashSizeInBits / 8;
            if (hashWithSaltBytes.Length < hashSizeInBytes) return false;
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
            for (int i = 0; i < saltBytes.Length; i++) saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];
            string expectedHashString = createpassword(plainText, hashAlgorithm, saltBytes);
            return (hashValue == expectedHashString);
        }
        public string makeurl(string term)
        {
            return term.ToLower().Replace("&", "-").Replace("(", "-").Replace(")", "-").Replace("/", "-").Replace("\\", "-").Replace("\"", "-").Replace("'", "-").Replace("+", "-").Replace("-", "-").Replace(":", "-").Replace(",", "-").Replace(".", "-").Replace("?", "-").Replace("!", "-").Replace("[", "-").Replace("]", "-").Replace("%", "-").Replace("#", "-").Replace("<", "-").Replace(">", "-").Replace("*", "-").Replace("|", "-").Replace("=", "-").Replace("~", "-").Replace("¨", "-").Replace("@", "-").Replace("æ", "-").Replace("ß", "-").Replace("½", "-").Replace("$", "-").Replace("£", "-").Replace("é", "-").Replace("ö", "o").Replace("ç", "c").Replace("ş", "s").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Trim().Replace(' ', '-');
        }
        public string createapisecret()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32];
                cryptoProvider.GetBytes(secretKeyByteArray);
                return Convert.ToBase64String(secretKeyByteArray);
            }
        }

        #endregion
    }

    public class db
    {
        public string column { get; set; }
        public object value { get; set; }
    }

    [DataContract]
    public class item
    {
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public string text { get; set; }
    }
    public class basic
    {
        public int id { get; set; }
        public string value { get; set; }
    }
    public class user
    {
        public int id { get; set; }
        public string password { get; set; }
        public string apikey { get; set; }
        public string apisecret { get; set; }
        public DateTime registerdate { get; set; }
        [DataMember]
        public DateTime lastlogindate { get; set; }
        public string birthday { get; set; }
        public string username { get; set; }
        public int status { get; set; }
        public string namesurname { get; set; }
        public string birthdaystr { get; set; }
        public string avatar { get; set; }
        public string background { get; set; }
        public string url { get; set; }
        public userauth userauth { get; set; }
    }
    public class profile
    {
        public string namesurname { get; set; }
        public string about { get; set; }
        public int birthday { get; set; }
        public int birthmonth { get; set; }
        public int birthyear { get; set; }
    }
    public class userauth
    {
        public auth google { get; set; }
        public auth twitch { get; set; }
        public auth facebook { get; set; }
    }
    public class auth
    {
        public bool linked { get; set; }
        public string email { get; set; }
    }

    public class income
    {
        public string jobtype { get; set; }
        public int monthlyincome { get; set; }
        public int monthlyadditionalincome { get; set; }
        public DateTime date { get; set; }
    }

    public class outcome
    {
        public int houserent { get; set; }
        public int waterbill { get; set; }
        public int gasbill { get; set; }
        public int gsmbill { get; set; }
        public int internetbill { get; set; }
        public int electricbill { get; set; }
        public int otherbills { get; set; }
        public int individualexpense { get; set; }
        public int marketexpense { get; set; }
        public DateTime date { get; set; }
    }
    public class creditcard
    {
        public int id { get; set; }
        public string bankname { get; set; }
        public int cardlimit { get; set; }
        public int carddebt { get; set; }
        public DateTime date { get; set; }
    }

}
