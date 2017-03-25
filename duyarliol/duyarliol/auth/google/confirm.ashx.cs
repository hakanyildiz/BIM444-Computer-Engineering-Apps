using System;
using System.Collections.Generic;
using System.Web;

namespace duyarliol.auth.google
{
    public class confirm : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            
                if (context.Request.HttpMethod == "POST")
                {
                    string method = context.Request.Form["method"],
                      email = context.Request.Form["email"],
                      username = context.Request.Form["username"],
                      accesstoken = context.Request.Form["accesstoken"],
                      password = context.Request.Form["password"];
                    if (!string.IsNullOrEmpty(method) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        var data = new core.data();
                        var authmethod = 1;
                        if (method == "new")
                        {
                            if (data.checkdb("users", new List<core.db>() {
                                new core.db() { column = "fullname", value = username }
                              }) > 0)
                            {
                                //user already registered
                                context.Session["auth-message"] = "Yazdığın kullanıcı adı zaten kayıtlı.";
                                context.Session["auth-method"] = "google";
                                context.Session["auth-email"] = email;
                                context.Session["auth-accesstoken"] = accesstoken;
                                context.Response.Redirect("/authenticate#new");
                            }
                            else
                            {
                            //register new user
                            if (data.insertdb("users", new List<core.db>() {
                                  new core.db() { column = "apikey", value = data.createrandomstring(30) },
                                  new core.db() { column = "apisecret", value = data.createapisecret() },
                                  new core.db() { column = "fullname", value = username },
                                  new core.db() { column = "password", value = data.createpassword(password, "SHA512", null) },
                                  new core.db() { column = "registerdate", value = DateTime.Now },
                                  new core.db() { column = "status", value = 1 },
                                  new core.db() { column = "url", value = data.makeurl(username) },

                                }))
                            {
                                    int userid = Convert.ToInt32(data.getsinglecolumndb("users", "id", new List<core.db>() {
                                        new core.db() { column = "fullname", value = username }
                                      }) ?? 0);
                                    if (userid > 0)
                                    {
                                    if (data.insertdb("auths", new List<core.db>() {
                                          new core.db() { column = "authtype", value = authmethod },
                                          new core.db() { column = "userid", value = userid },
                                          new core.db() { column = "email", value = email },
                                           new core.db() {column = "accesstoken", value = accesstoken },
                                          new core.db() { column = "status", value = 1 },
                                          new core.db() { column = "date", value = DateTime.Now }
                                         }))
                                        {
                                            //registered successfully
                                            context.Session["user"] = data.getuserinfo(userid);
                                            context.Session["site-message"] = "Hoşgeldin.";
                                            context.Session["auth-method"] = null;
                                            context.Session["auth-email"] = null;
                                            context.Session["auth-accesstoken"] = null;
                                            data.updatedb("users", new List<core.db>() {
                                                new core.db() { column = "lastlogindate", value = DateTime.Now }
                                              }, new List<core.db>() {
                                                new core.db() { column = "id", value = userid }
                                              });
                                            string redirect = context.Session["auth-redirect"] != null ? context.Session["auth-redirect"].ToString() : "/";
                                            context.Response.Redirect(redirect);
                                        }
                                        else
                                        {
                                            //cannot insert table: auths
                                            data.removedb("users", new List<core.db>() { new core.db() { column = "id", value = userid } });
                                            context.Session["auth-message"] = "Bağlantı hatası.";
                                            context.Session["auth-method"] = "google";
                                            context.Session["auth-email"] = email;
                                            context.Session["auth-accesstoken"] = accesstoken;
                                            context.Response.Redirect("/authenticate#new");
                                        }
                                    }
                                    else
                                    {
                                        //cannot insert table: users
                                        context.Session["auth-message"] = "Bağlantı hatası.";
                                        context.Session["auth-method"] = "google";
                                        context.Session["auth-email"] = email;
                                        context.Session["auth-accesstoken"] = accesstoken;
                                        context.Response.Redirect("/authenticate#new");
                                    }
                                }
                                else
                                {
                                    //cannot insert table: users
                                    context.Session["auth-message"] = "Bağlantı hatası.";
                                    context.Session["auth-method"] = "google";
                                    context.Session["auth-email"] = email;
                                    context.Session["auth-accesstoken"] = accesstoken;
                                    context.Response.Redirect("/authenticate#new");
                                }
                            }
                        }
                        else if (method == "link")
                        {
                            int userid = Convert.ToInt32(data.getsinglecolumndb("users", "id", new List<core.db>() {
                            new core.db() { column = "fullname", value = username }
                          }) ?? 0);
                            if (userid > 0)
                            {
                            //user registered
                            if (data.insertdb("auths", new List<core.db>() {
                                      new core.db() { column = "authtype", value = authmethod },
                                      new core.db() { column = "userid", value = userid },
                                      new core.db() { column = "email", value = email },
                                      new core.db() {column = "accesstoken", value = accesstoken },
                                      new core.db() { column = "status", value = 1 },
                                      new core.db() { column = "date", value = DateTime.Now }
                            }))
                            {
                                    //linked successfully
                                    context.Session["user"] = data.getuserinfo(userid);
                                    context.Session["site-message"] = "Hoşgeldin.";
                                    context.Session["auth-method"] = null;
                                    context.Session["auth-email"] = null;
                                    context.Session["auth-accesstoken"] = null;
                                    data.updatedb("users", new List<core.db>() {
                                        new core.db() { column = "lastlogindate", value = DateTime.Now }
                                      }, new List<core.db>() {
                                        new core.db() { column = "id", value = userid }
                                      });
                                    string redirect = context.Session["auth-redirect"] != null ? context.Session["auth-redirect"].ToString() : "/";
                                    context.Response.Redirect(redirect);
                                }
                                else
                                {
                                    //cannot insert table: auths
                                    context.Session["auth-message"] = "Bağlantı hatası.";
                                    context.Session["auth-method"] = "google";
                                    context.Session["auth-email"] = email;
                                    context.Session["auth-accesstoken"] = accesstoken;
                                    context.Response.Redirect("/authenticate#link");
                                }
                            }
                            else
                            {
                                //user not registered
                                context.Session["auth-message"] = "Yazdığın kullanıcı adı için bir kayıt bulamadım.";
                                context.Session["auth-method"] = "google";
                                context.Session["auth-email"] = email;
                                context.Session["auth-accesstoken"] = accesstoken;
                                context.Response.Redirect("/authenticate#link");
                            }
                        }
                        else
                        {
                            //invalid method
                            context.Session["auth-message"] = "Bağlantı hatası.";
                            context.Session["auth-method"] = "google";
                            context.Session["auth-email"] = email;
                            context.Session["auth-accesstoken"] = accesstoken;
                            context.Response.Redirect("/authenticate");
                        }
                    }
                    else
                    {
                        //some required fields are empty
                        context.Session["auth-message"] = "Lütfen gerekli yerleri doldur.";
                        context.Session["auth-method"] = "google";
                        context.Session["auth-email"] = email;
                        context.Session["auth-accesstoken"] = accesstoken;
                        context.Response.Redirect("/authenticate");
                    }
                }
                else
                {
                    //invalid http method
                    context.Session["site-message"] = "Bağlantı hatası.";
                    context.Session["auth-method"] = null;
                    context.Session["auth-email"] = null;
                    context.Session["auth-accesstoken"] = null;
                    context.Response.Redirect("/");
                }
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