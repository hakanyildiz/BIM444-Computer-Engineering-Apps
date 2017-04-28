using System;
using System.Collections.Generic;
using System.Web;

namespace duyarliol.auth.facebook
{
    public class gateway : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public string CLIENT_ID = "1776520979342984";
        public string CLIENT_SECRET = "9504ef7cf7f06506eed6f1d2459fcc51";
        public string REDIRECT_URL = "http://duyarliol.azurewebsites.net/auth/facebook/gateway";
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                context.Session["site-message"] = "Bağlantı hatası. 1";
                context.Response.Redirect("/");
            }

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            string tt = getaccesstoken(string.Format("client_id={0}&client_secret={1}&redirect_uri={2}&code={3}", CLIENT_ID,CLIENT_SECRET,REDIRECT_URL, code));
            if (string.IsNullOrEmpty(tt))
            {
                //token object is empty
                context.Session["site-message"] = "Bağlantı hatası. 2";
                context.Response.Redirect("/");
            }
            var facebooktokeninfo = jss.Deserialize<facebooktoken>(tt);
            if (facebooktokeninfo == null || string.IsNullOrEmpty(facebooktokeninfo.access_token))
            {
                //cannot deserialize token object
                context.Session["site-message"] = "Bağlantı hatası. 3";
                context.Response.Redirect("/");
            }


            var facebookuserinfo = getuserinfo(facebooktokeninfo.access_token);
            if (!string.IsNullOrEmpty(facebookuserinfo))
            {
                var facebookuser = jss.Deserialize<facebookuser>(facebookuserinfo);
                if (facebookuser != null && !string.IsNullOrEmpty(facebookuser.email))
                {
                    var data = new core.data();

                    var userid = Convert.ToInt32(data.getsinglecolumndb("auths", "userid", new List<core.db>() {
                        new core.db() { column = "email", value = facebookuser.email },
                        new core.db() { column = "authtype", value = 3 },
                        new core.db() { column = "status", value = 1 }
                      }) ?? 0);
                    if (userid > 0)
                    {
                        var user = data.getuserinfo(userid);
                        if (user == null || user.id == 0)
                        {
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
                        if (context.Session["auth-current-user"] != null)
                        {
                            userid = Convert.ToInt32(context.Session["auth-current-user"]);
                            if (userid > 0)
                            {
                                if (data.insertdb("auths", new List<core.db>() {
                                      new core.db() { column = "authtype", value = 3 },
                                      new core.db() { column = "userid", value = userid },
                                      new core.db() { column = "email", value = facebookuser.email },
                                      new core.db() { column = "accesstoken", value = facebooktokeninfo.access_token },
                                      new core.db() { column = "status", value = 1 },
                                      new core.db() { column = "date", value = DateTime.Now }
                                    }))
                                {
                                    context.Session["user"] = data.getuserinfo(userid);
                                    context.Session["site-message"] = "Facebook hesabın bağlandı.";
                                    context.Session["auth-method"] = null;
                                    context.Session["auth-email"] = null;
                                    string redirect = context.Session["auth-redirect"] != null ? context.Session["auth-redirect"].ToString() : "/";
                                    context.Response.Redirect(redirect.Contains("uye/profil") ? string.Format("{0}#auths", redirect) : redirect);
                                }
                                else
                                {
                                    //cannot insert table: auths
                                    context.Session["auth-message"] = "Bağlantı hatası. Lütfen kullanıcı adı ve şifreni girerek dene";
                                    context.Session["auth-method"] = "facebook";
                                    context.Session["auth-accesstoken"] = facebooktokeninfo.access_token;
                                    context.Session["auth-email"] = facebookuser.email;
                                    context.Response.Redirect("/authenticate#link");
                                }
                            }
                            else
                            {
                                context.Session["auth-message"] = "Sitemize ilk kez giriş yaptığını görüyoruz. Kaydını tamamlamak için son 1 adım kaldı.";
                                context.Session["auth-method"] = "facebook";
                                context.Session["auth-accesstoken"] = facebooktokeninfo.access_token;
                                context.Session["auth-email"] = facebookuser.email;
                                context.Response.Redirect("/authenticate");
                            }
                        }
                        else
                        {
                            context.Session["auth-message"] = "Sitemize ilk kez giriş yaptığını görüyoruz. Kaydını tamamlamak için son 1 adım kaldı.";
                            context.Session["auth-method"] = "facebook";
                            context.Session["auth-accesstoken"] = facebooktokeninfo.access_token;
                            context.Session["auth-email"] = facebookuser.email;
                            context.Response.Redirect("/authenticate");
                        }
                    }
                }
            }
        }

        private static string getaccesstoken(string parameters)
        {
            try
            {
                string result = "";

                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(string.Format("https://graph.facebook.com/v2.6/oauth/access_token?{0}", parameters));
                req.Method = "GET";

                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.Stream responseStream = res.GetResponseStream();
                System.IO.StreamReader myStreamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                res.Close();

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string getuserinfo(string accesstoken)
        {
            try
            {
                string result = "";

                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(string.Format("https://graph.facebook.com/me?access_token={0}&fields=email", accesstoken));
                req.Method = "GET";
                req.ContentType = "application/json";

                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.Stream responseStream = res.GetResponseStream();
                System.IO.StreamReader myStreamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                res.Close();

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        class facebooktoken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        class facebookuser
        {
            public string email { get; set; }
            public string id { get; set; }
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