using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace duyarliol.auth.google
{
    public class gateway : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string state = context.Request.QueryString["state"], code = context.Request.QueryString["code"], error = context.Request.QueryString["error"];
            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(code))
            {
                string tokeninfo = "";
                try
                {
                    tokeninfo = granttoken("https://www.googleapis.com/oauth2/v4/token", string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}", code, "1025271688593-hjuu8h3rjk1jsqs26vl043hha8shabpk.apps.googleusercontent.com", "pKbIPhe4A_UfD__8kh0lbrBz", "http://duyarliol1.azurewebsites.net/auth/google/gateway", "authorization_code"));
                }
                catch (Exception)
                {
                    //
                }
                if (!string.IsNullOrEmpty(tokeninfo))
                {
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue };

                    var token = jss.Deserialize<googletoken>(tokeninfo);
                    if (token != null && !string.IsNullOrEmpty(token.access_token))
                    {
                        string userinfo = "";
                        try
                        {
                            userinfo = getuserinfo(token.access_token);
                        }
                        catch (Exception)
                        {
                            //
                        }
                        if (!string.IsNullOrEmpty(userinfo))
                        {
                            var googleuser = jss.Deserialize<googleuser>(userinfo);
                            if (googleuser != null && !string.IsNullOrEmpty(googleuser.email))
                            {
                                if (googleuser.audience == "1025271688593-hjuu8h3rjk1jsqs26vl043hha8shabpk.apps.googleusercontent.com")
                                {
                                    if (googleuser.verified_email)
                                    {
                                        var data = new core.data();

                                        var userid = Convert.ToInt32(data.getsinglecolumndb("auths", "userid", new List<core.db>() {
                                          new core.db() { column = "email", value = googleuser.email },
                                          new core.db() { column = "authtype", value = 1 },
                                          new core.db() { column = "status", value = 1 }
                                        }) ?? 0);
                                        if (userid > 0)
                                        {
                                            //user authenticated with google
                                            var user = data.getuserinfo(userid);
                                            if (user == null || user.id == 0)
                                            {
                                                //cannot get user info
                                                context.Session["site-message"] = "Bağlantı hatası.";
                                                context.Response.Redirect("/");
                                            }

                                            context.Session["user"] = user;
                                            context.Session["site-message"] = "Hoşgeldin.";
                                            string redirect = context.Session["auth-redirect"] != null ? context.Session["auth-redirect"].ToString() : "/";
                                            context.Response.Redirect(redirect.Contains("uye/profil") ? string.Format("{0}#auths", redirect) : redirect);
                                        }
                                        else
                                        {
                                            //user not authenticated with google
                                            if (context.Session["auth-current-user"] != null)
                                            {
                                                userid = Convert.ToInt32(context.Session["auth-current-user"]);
                                                if (userid > 0)
                                                {
                                                    if (data.insertdb("auths", new List<core.db>() {
                                                      new core.db() { column = "authtype", value = 1 },
                                                      new core.db() { column = "userid", value = userid },
                                                      new core.db() { column = "email", value = googleuser.email },
                                                      new core.db() { column = "status", value = 1 },
                                                      new core.db() { column = "date", value = DateTime.Now }
                                                    }))
                                                    {
                                                        //linked successfully
                                                        context.Session["user"] = data.getuserinfo(userid);
                                                        context.Session["site-message"] = "Google hesabın bağlandı.";
                                                        context.Session["auth-method"] = null;
                                                        context.Session["auth-email"] = null;
                                                        context.Session["auth-accesstoken"] = null;
                                                        string redirect = context.Session["auth-redirect"] != null ? context.Session["auth-redirect"].ToString() : "/";
                                                        context.Response.Redirect(redirect.Contains("uye/profil") ? string.Format("{0}#auths", redirect) : redirect);
                                                    }
                                                    else
                                                    {
                                                        //cannot insert table: auths
                                                        context.Session["auth-message"] = "Bağlantı hatası. Lütfen kullanıcı adı ve şifreni girerek dene.";
                                                        context.Session["auth-method"] = "google";
                                                        context.Session["auth-accesstoken"] = token.access_token;
                                                        context.Session["auth-email"] = googleuser.email;
                                                        context.Response.Redirect("/authenticate#link");
                                                    }
                                                }
                                                else
                                                {
                                                    context.Session["auth-message"] = "Sitemize ilk kez giriş yaptığını görüyoruz. Kaydını tamamlamak için son 1 adım kaldı.";
                                                    context.Session["auth-method"] = "google";
                                                    context.Session["auth-accesstoken"] = token.access_token;
                                                    context.Session["auth-email"] = googleuser.email;
                                                    context.Response.Redirect("/authenticate");
                                                }
                                            }
                                            else
                                            {
                                                context.Session["auth-message"] = "Sitemize ilk kez giriş yaptığını görüyoruz. Kaydını tamamlamak için son 1 adım kaldı.";
                                                context.Session["auth-method"] = "google";
                                                context.Session["auth-accesstoken"] = token.access_token;
                                                context.Session["auth-email"] = googleuser.email;
                                                context.Response.Redirect("/authenticate");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string granttoken(string url, string parameters)
        {
            string result = "";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(parameters);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;

            Stream requestStream = req.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream responseStream = res.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
            result = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            responseStream.Close();
            res.Close();

            return result;
        }

        public string getuserinfo(string token)
        {
            string result = "";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", token));
            req.Method = "GET";
            req.ContentType = "application/json";

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream responseStream = res.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
            result = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            responseStream.Close();
            res.Close();

            return result;
        }

        class googletoken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
        }

        class googleuser
        {
            public string issued_to { get; set; }
            public string audience { get; set; }
            public string user_id { get; set; }
            public string scope { get; set; }
            public int expires_in { get; set; }
            public string email { get; set; }
            public bool verified_email { get; set; }
            public string access_type { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}