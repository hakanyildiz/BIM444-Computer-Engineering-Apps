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
        //public const string connstr = "Server=tcp:duyarliol1.database.windows.net,1433;Initial Catalog=duyarliol1;Persist Security Info=False;User ID=duyarliol1;Password=a2gf2a424gfk.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string connstr = "Server=tcp:duyarliol.database.windows.net,1433;Initial Catalog=duyarliol;Persist Security Info=False;User ID=duyarliol;Password=A2gf2a424gfk.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
            catch (Exception ex)
            { 
                return null;
            }
        }

        public List<creditcard> getcreditcards(int userid)
        {
            try
            {
                List<creditcard> list = new List<creditcard>();
                using (conn)
                {
                    cmd.CommandText = "select bankname, cardlimit, carddebt,createddate,updatedate from usercreditcards where userid = @userid";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userid", userid);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new creditcard()
                            {
                                bankname = !r.IsDBNull(0) ? r.GetString(0) : "",
                                cardlimit = !r.IsDBNull(1) ? r.GetDouble(1) : 0,
                                carddebt = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                createddate = !r.IsDBNull(3) ? r.GetDateTime(3): DateTime.Now,
                                updatedate = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now,
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
            catch (Exception ex)
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

                    cmd.CommandText = "select jobtype, monthlyincome, monthlyadditionalincome, createddate, updatedate from userincome where userid=@a";
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
                                monthlyincome = !r.IsDBNull(1) ? r.GetDouble(1) : 0,
                                monthlyadditionalincome = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                createddate = !r.IsDBNull(3) ? r.GetDateTime(3) : DateTime.Now,
                                updatedate = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now
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

                    cmd.CommandText = "select houserent,electricbill,waterbill,gasbill,internetbill,gsmbill,otherbills,individualexpense,marketexpense,createddate,updatedate from useroutcome where userid=@a";
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
                                houserent = !r.IsDBNull(0) ? r.GetDouble(0) : 0,
                                electricbill = !r.IsDBNull(1) ? r.GetDouble(1) : 0,
                                waterbill = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                gasbill = !r.IsDBNull(3) ? r.GetDouble(3) : 0,
                                internetbill = !r.IsDBNull(4) ? r.GetDouble(4) : 0,
                                gsmbill = !r.IsDBNull(5) ? r.GetDouble(5) : 0,
                                otherbills = !r.IsDBNull(6) ? r.GetDouble(6) : 0,
                                individualexpense = !r.IsDBNull(7) ? r.GetDouble(7) : 0,
                                marketexpense = !r.IsDBNull(8) ? r.GetDouble(8) : 0,
                                createddate = !r.IsDBNull(9) ? r.GetDateTime(9) : DateTime.Now,
                                updatedate = !r.IsDBNull(10) ? r.GetDateTime(10) : DateTime.Now

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
                    cmd.CommandText = "select id, bankname, cardlimit, carddebt, createddate,updatedate from usercreditcards where userid=@a";
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
                                cardlimit = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                carddebt = !r.IsDBNull(3) ? r.GetDouble(3) : 0,
                                createddate = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now,
                                updatedate = !r.IsDBNull(5) ? r.GetDateTime(5) : DateTime.Now,
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

        public List<wishlist> getuserwishlist(int id)
        {
            try
            {
                List<wishlist> list = new List<wishlist>();
                using (conn)
                {
                    cmd.CommandText = "select ordername,ordercount,orderprice,userid,orderdate,pending,id,sitename from wishlist where userid=@a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new wishlist()
                            {
                                ordername = !r.IsDBNull(0) ? r.GetString(0) : "",
                                ordercount = !r.IsDBNull(1) ? r.GetInt32(1) : 0,
                                orderprice = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                userid = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                orderdate = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now,
                                pending = !r.IsDBNull(5) ? r.GetInt32(5) : 0,
                                id = !r.IsDBNull(6) ? r.GetInt32(6) : 0,
                                sitename = !r.IsDBNull(7) ? r.GetString(7): "default"
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

        public List<answerlist> getuseranswerlist(int id)
        {
            try
            {
                List<answerlist> list = new List<answerlist>();
                using (conn)
                {
                    cmd.CommandText = "select id,question,answer,date,userid from answerlist where userid = @a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", id);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new answerlist()
                            {
                                id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                question = !r.IsDBNull(1) ? r.GetString(1) : "",
                                answer = !r.IsDBNull(2) ? r.GetString(2) : "",
                                date = !r.IsDBNull(3) ? r.GetDateTime(3) : DateTime.Now,
                                userid = id,
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

        public List<wishlist> getuserwishlist(int userid, int pending, int itemperpage, int page)
        {
            try
            {
                List<wishlist> list = new List<wishlist>();
                using (conn)
                {
                    string pendingClause = "";
                    if(pending == 1)
                    {
                        pendingClause += " and pending = 1";
                    }
                    else if(pending == 0)
                    {
                        pendingClause += " and pending = 0";
                    }

                    cmd.CommandText = string.Format("select * from (select ordername,ordercount,orderprice,userid,orderdate,pending,id,sitename,row_number() over (order by orderdate desc) as rn from wishlist where userid=@userid {0}) a where a.rn between @start and @end", pendingClause);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@start", ((page - 1) * itemperpage) + 1);
                    cmd.Parameters.AddWithValue("@end", page * itemperpage);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new wishlist()
                            {
                                ordername = !r.IsDBNull(0) ? r.GetString(0) : "",
                                ordercount = !r.IsDBNull(1) ? r.GetInt32(1) : 0,
                                orderprice = !r.IsDBNull(2) ? r.GetDouble(2) : 0,
                                userid = !r.IsDBNull(3) ? r.GetInt32(3) : 0,
                                orderdate = !r.IsDBNull(4) ? r.GetDateTime(4) : DateTime.Now,
                                pending = !r.IsDBNull(5) ? r.GetInt32(5) : 0,
                                id = !r.IsDBNull(6) ? r.GetInt32(6) : 0,
                                sitename = !r.IsDBNull(7) ? r.GetString(7) : "default"

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

        public double getuserincometotal(int userid)
        {
            double total = 0;
            try
            {
                income income = new income();
                using (conn)
                {
                    cmd.CommandText = "select monthlyincome, monthlyadditionalincome from userincome where userid = @a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", userid);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            income.monthlyincome = !r.IsDBNull(0) ? r.GetDouble(0) : 0;
                            income.monthlyadditionalincome = !r.IsDBNull(1) ? r.GetDouble(1) : 0;
                        }
                    }
                }
                total = income.monthlyadditionalincome + income.monthlyincome;
            }
            catch (Exception)
            {

            }
            return total;
        }
        public double getuseroutcometotal(int userid)
        {
            double total = 0;
            try
            {
                outcome outcome = new outcome();
                using (conn)
                {
                    cmd.CommandText = "select electricbill, waterbill, gasbill, internetbill, gsmbill, otherbills, individualexpense, marketexpense,houserent from useroutcome where userid = @a";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@a", userid);
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            outcome.electricbill = !r.IsDBNull(0) ? r.GetDouble(0) : 0;
                            outcome.waterbill = !r.IsDBNull(1) ? r.GetDouble(1) : 0;
                            outcome.gasbill = !r.IsDBNull(2) ? r.GetDouble(2) : 0;
                            outcome.internetbill = !r.IsDBNull(3) ? r.GetDouble(3) : 0;
                            outcome.gsmbill = !r.IsDBNull(4) ? r.GetDouble(4) : 0;
                            outcome.otherbills = !r.IsDBNull(5) ? r.GetDouble(5) : 0;
                            outcome.individualexpense = !r.IsDBNull(6) ? r.GetDouble(6) : 0;
                            outcome.marketexpense = !r.IsDBNull(7) ? r.GetDouble(7) : 0;
                            outcome.houserent = !r.IsDBNull(8) ? r.GetDouble(8) : 0;
                        }
                    }
                }
                total = outcome.electricbill + outcome.waterbill + outcome.gasbill + outcome.internetbill +
                    outcome.gsmbill + outcome.otherbills + outcome.individualexpense + outcome.marketexpense + outcome.houserent;
            }
            catch (Exception)
            {

            }
            return total;
        }

        public List<interactions> getuserinteractions(int userid, string query, string orderby, int itemperpage, int page, string criter, string classtype)
        {

            try
            {
                List<interactions> list = new List<interactions>();
                string titleClass = "";
                using (conn)
                {

                    
                        if (classtype.Contains("all"))
                        {
                            cmd.CommandText = string.Format("select * from (select id,userid,status,title,subtitle,date,row_number() over (order by {0} {1}) as rn from userinteractions {2}) a where a.rn between @start and @end", orderby, criter, !string.IsNullOrEmpty(query) ? "where (name like '%'+@term+'%')" : "");
                        }
                        else
                        {
                             titleClass = string.Format("(title like'%'+@titleClass+'%')", classtype);
                            //query boş ise
                            if (string.IsNullOrEmpty(query))
                            {
                              titleClass = string.Format("where {0}", titleClass);
                            }
                            else
                            {
                                //query boş değilse
                                titleClass = string.Format(" and {0}", titleClass);
                            }
                            cmd.CommandText = string.Format("select * from (select id,userid,status,title,subtitle,date,row_number() over (order by {0} {1}) as rn from userinteractions {2} {3}) a where a.rn between @start and @end", orderby, criter, !string.IsNullOrEmpty(query) ? "where (name like '%'+@term+'%')" : "", titleClass);

                        }

                        cmd.Parameters.AddWithValue("@start", ((page - 1) * itemperpage) + 1);
                        cmd.Parameters.AddWithValue("@end", page * itemperpage);
                        if (!string.IsNullOrEmpty(query)) cmd.Parameters.AddWithValue("@term", query);
                        if (!classtype.Contains("all") && !string.IsNullOrEmpty(classtype)) cmd.Parameters.AddWithValue("@titleClass", classtype);
                        if (conn.State == ConnectionState.Open) conn.Close();
                        if (conn.State == ConnectionState.Closed) { conn.ConnectionString = connstr; conn.Open(); }
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                list.Add(new interactions()
                                {
                                    id = !r.IsDBNull(0) ? r.GetInt32(0) : 0,
                                    userid = !r.IsDBNull(1) ? r.GetInt32(1) : 0,
                                    status = !r.IsDBNull(2) ? r.GetInt32(2) : 0,
                                    title = !r.IsDBNull(3) ? r.GetString(3) : "",
                                    subtitle = !r.IsDBNull(4) ? r.GetString(4) : "",
                                    date = !r.IsDBNull(5) ? r.GetDateTime(5) : DateTime.Now
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
        public double monthlyincome { get; set; }
        public double monthlyadditionalincome { get; set; }
        public DateTime createddate { get; set; }
        public DateTime updatedate { get; set; }
    }

    public class outcome
    {
        public double houserent { get; set; }
        public double waterbill { get; set; }
        public double gasbill { get; set; }
        public double gsmbill { get; set; }
        public double internetbill { get; set; }
        public double electricbill { get; set; }
        public double otherbills { get; set; }
        public double individualexpense { get; set; }
        public double marketexpense { get; set; }
        public DateTime createddate { get; set; }
        public DateTime updatedate { get; set; }
    }
    public class creditcard
    {
        public int id { get; set; }
        public string bankname { get; set; }
        public double cardlimit { get; set; }
        public double carddebt { get; set; }
        public DateTime createddate { get; set; }
        public DateTime updatedate { get; set; }

    }

    public class wishlist
    {
        public int id { get; set; }
        public string ordername { get; set; }
        public int ordercount { get; set; }
        public double orderprice { get; set; }
        public DateTime orderdate { get; set; }
        public int pending { get; set; }
        public int userid { get; set; }
        public string  sitename { get; set; }
    }

    public class answerlist
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }

        public DateTime date { get; set; }

        public int userid { get; set; }

    }

    public class interactions
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public DateTime date { get; set; }
    }
}
