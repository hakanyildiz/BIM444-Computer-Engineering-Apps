using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace duyarliol.handlers
{
    public class main : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("cache-control", "no-cache, no-store, max-age=0, must-revalidate");
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Expires", "0");
            context.Response.ContentType = "application/json";

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            var res = new response() { success = false, message = "" };

            if (context.Request.HttpMethod == "POST")
            {
                core.data data = new core.data();
                string method = !string.IsNullOrEmpty(context.Request.Form["fm"]) ? context.Request.Form["fm"] : "";
                if (!string.IsNullOrEmpty(method))
                {
                    if (context.Session["user"] != null)
                    {
                        #region user post methods

                        if (method == "change-password")
                        {
                            #region change user password

                            string oldpass = context.Request.Form["oldpass"],
                              newpass = context.Request.Form["newpass"],
                              newpasscon = context.Request.Form["newpasscon"];

                            if (string.IsNullOrEmpty(oldpass) || string.IsNullOrEmpty(newpass) || string.IsNullOrEmpty(newpasscon))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen gerekli yerleri doldur." }));
                                context.Response.End();
                            }

                            if (newpass != newpasscon)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Yeni şifreler eşleşmiyor." }));
                                context.Response.End();
                            }

                            string currentpass = Convert.ToString(data.getsinglecolumndb("users", "password", new List<core.db>() { new core.db() { column = "id", value = ((core.user)context.Session["user"]).id } }) ?? "");

                            if (!data.verifypassword(oldpass, "SHA512", currentpass))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Eski şifreni hatalı girdin." }));
                                context.Response.End();
                            }

                            if (!data.updatedb("users", new List<core.db>() {
                                      new core.db() { column = "password", value = data.createpassword(newpass, "SHA512", null) }
                                    }, new List<core.db>() {
                                      new core.db() { column = "id", value = ((core.user)context.Session["user"]).id }
                                    }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "bağlantı hatası." }));
                                context.Response.End();
                            }

                            context.Response.Write(jss.Serialize(new response() { success = true, message = "Şifren değiştirildi. Artık yeni şifren ile giriş yapabilirsin." }));
                            context.Response.End();

                            #endregion
                        }
                        else if (method == "update-user-profile")
                        {
                            #region update user profile info

                            string name = context.Request.Form["namesurname"],
                              about = context.Request.Form["about"],
                              a = context.Request.Form["birthday"],
                              b = context.Request.Form["birthmonth"],
                              c = context.Request.Form["birthyear"];

                            int birthday = 0, birthmonth = 0, birthyear = 0;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out birthday);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out birthmonth);
                            if (!string.IsNullOrEmpty(c)) int.TryParse(c, out birthyear);

                            DateTime birth = DateTime.Now.AddYears(100);

                            if (birthday > 0 && birthmonth > 0 && birthyear > 0)
                            {
                                if (!DateTime.TryParse(string.Format("{0}.{1}.{2}", birthday, birthmonth, birthyear), out birth))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Geçersiz doğum tarihi girdin." }));
                                    context.Response.End();
                                }
                            }

                            if (data.updatedb("users", new List<core.db>() {
                                           new core.db() { column = "namesurname", value = name },
                                           new core.db() { column = "about", value = about },
                                           new core.db() { column = "birthday", value = birth }
                                        }, new List<core.db>() {
                                      new core.db() { column = "id", value = ((core.user)context.Session["user"]).id }
                                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Profil bilgilerin güncellendi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "update-user-avatar")
                        {
                            #region update user avatar

                            if (context.Request.Files.Count > 0)
                            {
                                HttpPostedFile avatar = context.Request.Files[0];
                                if (avatar != null && !string.IsNullOrEmpty(avatar.FileName))
                                {
                                    if (isvalidavatar(avatar))
                                    {
                                        string oldavatar = Convert.ToString(data.getsinglecolumndb("users", "avatar", new List<core.db>() { new core.db() { column = "id", value = ((core.user)context.Session["user"]).id } }) ?? "");
                                        string avatarfilename = string.Format("{0}-avatar-{1}{2}", ((core.user)context.Session["user"]).username, new Random().Next(1000000, 9999999), System.IO.Path.GetExtension(avatar.FileName).ToLower());
                                        string newavatar = "";
                                        try
                                        {
                                            avatar.SaveAs(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\avatar\{0}", avatarfilename));
                                            if (System.IO.File.Exists(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\avatar\{0}", avatarfilename)))
                                            {
                                                newavatar = avatarfilename;
                                                System.IO.File.Delete(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\avatar\{0}", oldavatar));
                                            }
                                            else
                                            {
                                                context.Session["site-message"] = "Bağlantı hatası.3";
                                                context.Response.Redirect("/uye/profil");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //
                                        }
                                        if (!string.IsNullOrEmpty(newavatar))
                                        {
                                            if (data.updatedb("users", new List<core.db>() {
                          new core.db() { column = "avatar", value = newavatar }
                        }, new List<core.db>() {
                          new core.db() { column = "id", value = ((core.user)context.Session["user"]).id }
                        }))
                                            {
                                                ((core.user)context.Session["user"]).avatar = newavatar;
                                                context.Session["site-message"] = "Avatarın güncellendi.";
                                                context.Response.Redirect("/uye/profil");
                                            }
                                            else
                                            {
                                                context.Session["site-message"] = "Bağlantı hatası.1";
                                                context.Response.Redirect("/uye/profil");
                                            }
                                        }
                                        else
                                        {
                                            context.Session["site-message"] = "Bağlantı hatası.2";
                                            context.Response.Redirect("/uye/profil");
                                        }
                                    }
                                    else
                                    {
                                        context.Session["site-message"] = "Seçtiğin resim geçersiz. Lütfen maksimum 200x200 piksel boyutlarında ve 1MB büyüklüğünde bir resim seç.";
                                        context.Response.Redirect("/uye/profil");
                                    }
                                }
                                else
                                {
                                    context.Session["site-message"] = "Lütfen bir resim seç.";
                                    context.Response.Redirect("/uye/profil");
                                }
                            }

                            #endregion
                        }
                        else if (method == "update-user-cover")
                        {
                            #region update user cover image

                            if (context.Request.Files.Count > 0)
                            {
                                HttpPostedFile cover = context.Request.Files[0];
                                if (cover != null && !string.IsNullOrEmpty(cover.FileName))
                                {
                                    if (isvalidcover(cover))
                                    {
                                        string oldcover = Convert.ToString(data.getsinglecolumndb("users", "background", new List<core.db>() { new core.db() { column = "id", value = ((core.user)context.Session["user"]).id } }) ?? "");
                                        string coverfilename = string.Format("{0}-cover-{1}{2}", ((core.user)context.Session["user"]).username, new Random().Next(1000000, 9999999), System.IO.Path.GetExtension(cover.FileName).ToLower());
                                        string newcover = "";
                                        try
                                        {
                                            cover.SaveAs(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\wallpaper\{0}", coverfilename));
                                            if (System.IO.File.Exists(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\wallpaper\{0}", coverfilename)))
                                            {
                                                newcover = coverfilename;
                                                System.IO.File.Delete(string.Format(@"C:\inetpub\vhosts\kaldirirmi.com\cdn\images\user\wallpaper\{0}", oldcover));
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //
                                        }
                                        if (!string.IsNullOrEmpty(newcover))
                                        {
                                            if (data.updatedb("users", new List<core.db>() {
                          new core.db() { column = "background", value = newcover }
                        }, new List<core.db>() {
                          new core.db() { column = "id", value = ((core.user)context.Session["user"]).id }
                        }))
                                            {
                                                ((core.user)context.Session["user"]).background = newcover;
                                                context.Session["site-message"] = "Cover resmin güncellendi.";
                                                context.Response.Redirect("/uye/profil");
                                            }
                                            else
                                            {
                                                context.Session["site-message"] = "Bağlantı hatası.";
                                                context.Response.Redirect("/uye/profil");
                                            }
                                        }
                                        else
                                        {
                                            context.Session["site-message"] = "Bağlantı hatası.";
                                            context.Response.Redirect("/uye/profil");
                                        }
                                    }
                                    else
                                    {
                                        context.Session["site-message"] = "Seçtiğin resim geçersiz. Lütfen maksimum 1920x1080 piksel boyutlarında ve 4MB büyüklüğünde bir resim seç.";
                                        context.Response.Redirect("/uye/profil");
                                    }
                                }
                                else
                                {
                                    context.Session["site-message"] = "Lütfen bir resim seç.";
                                    context.Response.Redirect("/uye/profil");
                                }
                            }

                            #endregion
                        }
                        else if (method == "new-user-system")
                        {
                            #region add user system

                            int ram = 0, cpu = 0, gpu = 0, os = 0, free_hdd = 0;
                            string a = context.Request.Form["cpu"],
                              b = context.Request.Form["gpu"],
                              c = context.Request.Form["os"],
                              d = context.Request.Form["ram"],
                              e = context.Request.Form["hdd"],
                              cpuname = context.Request.Form["cpuname"],
                              gpuname = context.Request.Form["gpuname"],
                              osname = context.Request.Form["osname"];

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out cpu);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out gpu);
                            if (!string.IsNullOrEmpty(c)) int.TryParse(c, out os);
                            if (!string.IsNullOrEmpty(d)) int.TryParse(d, out ram);
                            if (!string.IsNullOrEmpty(d)) int.TryParse(e, out free_hdd);

                            if (cpu == 0 || gpu == 0 || os == 0 || ram == 0 || free_hdd == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen gerekli yerleri doldur." }));
                                context.Response.End();
                            }

                            data.removedb("usersystems", new List<core.db>() { new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id } });

                            if (data.insertdb("usersystems", new List<core.db>() {
                                      new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                      new core.db() { column = "cpu_id", value = cpu },
                                      new core.db() { column = "cpu_name", value = cpuname },
                                      new core.db() { column = "gpu_id", value = gpu },
                                      new core.db() { column = "gpu_name", value = gpuname },
                                      new core.db() { column = "os_id", value = os },
                                      new core.db() { column = "os_name", value = osname },
                                      new core.db() { column = "ram", value = ram },
                                      new core.db() { column = "free_hdd", value = free_hdd },
                                      new core.db() { column = "last_scan_time", value = DateTime.Now},


                                    }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Sistemin tanımlandı." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "new-user-frame-rate")
                        {
                            #region add new user frame rate

                            int gameid = 0, resolutionid = 0, framerate = 0;
                            string a = context.Request.Form["game"],
                              b = context.Request.Form["resolution"],
                              c = context.Request.Form["framerate"];

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out gameid);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out resolutionid);
                            if (!string.IsNullOrEmpty(c)) int.TryParse(c, out framerate);

                            if (gameid == 0 || resolutionid == 0 || framerate == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen gerekli yerleri doldur." }));
                                context.Response.End();
                            }

                            if (data.insertdb("userframerates", new List<core.db>() {
                    new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                    new core.db() { column = "gameid", value = gameid },
                    new core.db() { column = "resolutionid", value = resolutionid },
                    new core.db() { column = "framerate", value = framerate },
                    new core.db() { column = "date", value = DateTime.Now }
                  }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "İşlem başarılı." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "new-forum-post")
                        {
                            #region add new forum post

                            string content = context.Request.Form["content"],
                              forumthreadurl = context.Request.Form["forumthread"];

                            if (string.IsNullOrEmpty(forumthreadurl))
                            {
                                context.Session["site-message"] = "Bağlantı hatası.";
                                context.Response.Redirect(context.Request.UrlReferrer.PathAndQuery);
                            }

                            int forumthreadid = Convert.ToInt32(data.getsinglecolumndb("forumthreads", "id", new List<core.db>() { new core.db() { column = "url", value = forumthreadurl } }) ?? 0);
                            if (forumthreadid == 0)
                            {
                                context.Session["site-message"] = "Bağlantı hatası.";
                                context.Response.Redirect(context.Request.UrlReferrer.PathAndQuery);
                            }

                            if (string.IsNullOrEmpty(content))
                            {
                                context.Session["site-message"] = "Lütfen yorum kısmını boş bırakma.";
                                context.Response.Redirect(context.Request.UrlReferrer.PathAndQuery);
                            }

                            if (!data.insertdb("forumposts", new List<core.db>() {
                                  new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                  new core.db() { column = "forumthreadid", value = forumthreadid },
                                  new core.db() { column = "postcontent", value = HttpUtility.HtmlEncode(content) },
                                  new core.db() { column = "upvote", value = 0 },
                                  new core.db() { column = "downvote", value = 0 },
                                  new core.db() { column = "date", value = DateTime.Now },
                                  new core.db() { column = "status", value = 1 }
                                }))
                            {
                                context.Session["site-message"] = "Bağlantı hatası.";
                                context.Response.Redirect(context.Request.UrlReferrer.PathAndQuery);
                            }



                            int currentpostcount = Convert.ToInt32(data.getsinglecolumndbcount(
                                "forumposts",
                                new List<core.db>(){
                                        new core.db() { column = "forumthreadid", value = forumthreadid }
                                      , new core.db() {column = "status" , value = 1 }
                            }));


                            data.updatedb("forumthreads", new List<core.db>() {
                                  new core.db() { column = "postcount", value = currentpostcount }
                                }, new List<core.db>() {
                                  new core.db() { column = "id", value = forumthreadid }
                                });
                            context.Session["site-message"] = "Yorumun eklendi.";
                            context.Response.Redirect(context.Request.UrlReferrer.PathAndQuery);

                            #endregion
                        }
                        #region Moderator Page
                        else if (method == "accept-waiting-thread")
                        {
                            #region accept waiting thread

                            string a = context.Request.Form["threadid"];

                            int threadid = 0;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out threadid);


                            if (data.updatedb("forumthreads", new List<core.db>() {
                  new core.db() { column = "status", value = 1 }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = threadid}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Konu onaylandı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "delete-waiting-thread")
                        {
                            #region delete waiting thread Moderator Operation

                            string a = context.Request.Form["threadid"];

                            int threadid = 0;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out threadid);


                            if (data.removedb("forumthreads", new List<core.db>() {
                  new core.db() { column = "id", value = threadid }
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Konu silindi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "mod-block-user")
                        {
                            #region mod-block-user Moderator Operation

                            string a = context.Request.Form["userid"];

                            int userid = 0;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);


                            if (data.updatedb("users", new List<core.db>() {
                  new core.db() { column = "status", value = 0}
                }, new List<core.db>() {
                  new core.db() { column = "id", value = userid}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Kullanıcı Engellendi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "mod-unblock-user")
                        {
                            #region mod-unblock-user Moderator Operation

                            string a = context.Request.Form["userid"];

                            int userid = 0;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);


                            if (data.updatedb("users", new List<core.db>() {
                  new core.db() { column = "status", value = 1}
                }, new List<core.db>() {
                  new core.db() { column = "id", value = userid}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Kullanıcı Engeli Kaldırıldı." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        #endregion

                        else if (method == "wizard-add-system")
                        {
                            #region add user system in SystemWizard page - autodetect system
                            int ram = 0, cpuId = 0, gpuId = 0, osId = 0, freeHdd = 0;
                            string a = context.Request.Form["cpuName"],
                              b = context.Request.Form["gpuName"],
                              c = context.Request.Form["osName"],
                              d = context.Request.Form["ram"],
                              e = context.Request.Form["freeHdd"],
                              cpuName = "",
                              gpuName = "",
                              osName = "";

                            if (!string.IsNullOrEmpty(a)) cpuName = a.Trim();
                            if (!string.IsNullOrEmpty(b)) gpuName = b.Trim();
                            if (!string.IsNullOrEmpty(c)) osName = c.Trim();
                            if (!string.IsNullOrEmpty(d)) int.TryParse(d, out ram);
                            if (!string.IsNullOrEmpty(e)) int.TryParse(e, out freeHdd);

                            if (cpuName == "" || gpuName == "" || osName == "" || ram == 0 || freeHdd == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Parametrelerde sıkıntı var." }));
                                context.Response.End();
                            }

                            //get cpu id via cpu name
                            cpuId = Convert.ToInt32(data.getsinglecolumndbViaLikeParam("cpulist", "id", new List<core.db>() {
                                      new core.db()
                                      {
                                        column = "name",
                                        value  = cpuName
                                      }
                                    })
                            );

                            // get cpu full name via name
                            cpuName = Convert.ToString(data.getDoubleColumndbViaLikeParam("cpulist", "brand", "name", new List<core.db>() { new core.db() { column = "name", value = cpuName } }));


                            if (cpuId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sorry, doesn't match CPU ID and Name! :(" }));
                                context.Response.End();
                            }
                            // get gpu id via name
                            gpuId = Convert.ToInt32(data.getsinglecolumndbViaLikeParam("gpulist", "id", new List<core.db>() { new core.db() { column = "name", value = gpuName } }));
                            if (gpuId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sorry, doesn't match GPU ID and Name! :(" }));
                                context.Response.End();
                            }

                            // get os id via name
                            osId = Convert.ToInt32(data.getsinglecolumndbViaLikeParam("oslist", "id", new List<core.db>() { new core.db() { column = "name", value = osName } }));
                            if (osId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sorry, doesn't match Operation System ID and Name! :(" }));
                                context.Response.End();
                            }


                            //check user system is already added..
                            int cnt = data.checkdb("usersystems", new List<core.db>() { new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id } });

                            if (cnt > 0)
                            {//update

                                if (data.updatedb("usersystems",
                                    new List<core.db>() {
                                               new core.db() { column = "cpu_id", value = cpuId},
                                               new core.db() { column = "cpu_name", value = cpuName},
                                               new core.db() { column = "gpu_id", value = gpuId},
                                               new core.db() { column = "gpu_name", value = gpuName},
                                               new core.db() { column = "os_id", value = osId},
                                               new core.db() { column = "os_name", value = osName},
                                               new core.db() { column = "ram", value = ram },
                                               new core.db() { column = "free_hdd", value = freeHdd },
                                               new core.db() { column = "last_scan_time", value = DateTime.Now }
                                },
                                    new List<core.db>() {
                                               new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id }
                                    }))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Sistemin güncellendi." }));
                                    context.Response.End();

                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistem Güncelleme -> Bağlantı hatası." }));
                                    context.Response.End();
                                }
                            }
                            else
                            {
                                //insert
                                if (data.insertdb("usersystems", new List<core.db>() {
                                       new core.db() { column = "userid", value =((core.user)context.Session["user"]).id },
                                       new core.db() { column = "cpu_id", value = cpuId},
                                       new core.db() { column = "cpu_name", value = cpuName},
                                       new core.db() { column = "gpu_id", value = gpuId},
                                       new core.db() { column = "gpu_name", value = gpuName},
                                       new core.db() { column = "os_id", value = osId},
                                       new core.db() { column = "os_name", value = osName},
                                       new core.db() { column = "ram", value = ram },
                                       new core.db() { column = "free_hdd", value = freeHdd },
                                       new core.db() { column = "last_scan_time", value = DateTime.Now }

                                    }))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Sistemin tanımlandı" }));
                                    context.Response.End();

                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistem Tanımlama -> Bağlantı hatası." }));
                                    context.Response.End();
                                }
                            }

                            #endregion
                        }

                        else if (method == "game-add-color")
                        {
                            #region game-add-color  in game.aspx
                            string a = context.Request.Form["backcolor"],
                              b = context.Request.Form["backcoloropacity"],
                              c = context.Request.Form["maincolor"],
                              d = context.Request.Form["gameid"],
                              backcolor = "",
                              backcoloropacity = "",
                              maincolor = "";
                            int id = 0;
                            if (!string.IsNullOrEmpty(a)) backcolor = a.Trim();
                            if (!string.IsNullOrEmpty(b)) backcoloropacity = b.Trim();
                            if (!string.IsNullOrEmpty(c)) maincolor = c.Trim();
                            if (!string.IsNullOrEmpty(d)) int.TryParse(d, out id);


                            if (data.updatedb("kalgame", new List<core.db>() {
                                   new core.db() { column = "bc", value = backcolor},
                                   new core.db() { column = "bca", value = backcoloropacity},
                                   new core.db() { column = "mc", value = maincolor }

                               }, new List<core.db>() {
                                      new core.db() { column = "id", value = id},

                               }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Renk işlendi" }));
                                context.Response.End();

                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }


                            #endregion
                        }

                        else if (method == "post-increase-viewcount")
                        {
                            #region post - when user access the news - viewcount increase +1

                            string _id = context.Request.Form["id"];
                            string _viewcount = context.Request.Form["viewcount"];

                            int id = 0, viewcount = 0;

                            if (!string.IsNullOrEmpty(_id)) int.TryParse(_id, out id);
                            if (!string.IsNullOrEmpty(_viewcount)) int.TryParse(_viewcount, out viewcount);

                            viewcount = viewcount + 1;

                            if (data.updatedb("kalnews", new List<core.db>() {
                  new core.db() { column = "viewcount", value = viewcount }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = id}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Viewcount arttırıldı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }

                        else if (method == "post-uservote-review")
                        {
                            #region post - user vote in review.aspx

                            string _userid = context.Request.Form["userid"];
                            string _gameid = context.Request.Form["gameid"];
                            string _vote = context.Request.Form["vote"];


                            int userid = 0, gameid = 0;
                            double vote = 0;

                            if (!string.IsNullOrEmpty(_userid)) int.TryParse(_userid, out userid);
                            if (!string.IsNullOrEmpty(_gameid)) int.TryParse(_gameid, out gameid);
                            if (!string.IsNullOrEmpty(_vote)) double.TryParse(_vote, out vote);

                            vote = vote * 20;

                            if (vote > 100)
                            {
                                vote = vote / 10;
                            }

                            int result = data.checkdb("gameuservote", new List<core.db>() {
                   new core.db() {column = "userid" ,value = userid },
                   new core.db() {column = "gameid" ,value = gameid}
                });

                            if (result > 0)
                            {//update
                                if (data.updatedb("gameuservote", new List<core.db>() {
                  new core.db() { column = "vote", value = vote }
                }, new List<core.db>() {
                  new core.db() { column = "userid", value = userid},
                  new core.db() { column = "gameid", value = gameid}
                }))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Oylama update başarılı" }));
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Oylama update Bağlantı hatası." }));
                                    context.Response.End();
                                }
                            }
                            else
                            {
                                if (data.insertdb("gameuservote", new List<core.db>() {
                   new core.db() { column = "userid", value = userid},
                   new core.db() { column = "gameid", value = gameid},
                   new core.db() { column = "vote", value = vote}
                }))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Oylama insert başarılı" }));
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Oylama insert Bağlantı hatası." }));
                                    context.Response.End();
                                }
                            }


                            #endregion
                        }
                        else if (method == "post-kleine-container-news")
                        {
                            #region post - when user access the news - viewcount increase +1

                            string _id = context.Request.Form["container"];
                            string _viewcount = context.Request.Form["kleine"];

                            int id = 0, viewcount = 0;

                            if (!string.IsNullOrEmpty(_id)) int.TryParse(_id, out id);
                            if (!string.IsNullOrEmpty(_viewcount)) int.TryParse(_viewcount, out viewcount);

                            viewcount = viewcount + 1;

                            if (data.updatedb("kalnews", new List<core.db>() {
                  new core.db() { column = "viewcount", value = viewcount }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = id}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Viewcount arttırıldı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "post-kleine-container-reviews")
                        {
                            #region post - when user access the reviews - viewcount increase +1

                            string _id = context.Request.Form["container"];
                            string _viewcount = context.Request.Form["kleine"];

                            int id = 0, viewcount = 0;

                            if (!string.IsNullOrEmpty(_id)) int.TryParse(_id, out id);
                            if (!string.IsNullOrEmpty(_viewcount)) int.TryParse(_viewcount, out viewcount);

                            viewcount = viewcount + 1;

                            if (data.updatedb("kalreviews", new List<core.db>() {
                  new core.db() { column = "viewcount", value = viewcount }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = id}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Viewcount arttırıldı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        #region FRIENDSHIP
                        else if (method == "send-friend-request")
                        {
                            #region sending friend request
                            int senderId = 0, receiverId = 0;
                            senderId = ((core.user)context.Session["user"]).id;

                            string a = context.Request.Form["receiverId"];
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out receiverId);

                            if (senderId == 0 || receiverId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İşlemi gerçekleştirmek için Giriş yapman gerekli." }));
                                context.Response.End();
                            }

                            //all this time ((one_id < two_id)) !!
                            int user_one_id, user_two_id;
                            if (senderId < receiverId)
                            {
                                user_one_id = senderId;
                                user_two_id = receiverId;
                            }
                            else
                            {
                                user_one_id = receiverId;
                                user_two_id = senderId;
                            }
                            /* Code Meaning
                             0   Pending
                             1   Accepted
                             2   Declined
                             3   Blocked */

                            int isExist = data.checkdb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id },
                                    new core.db() { column = "status", value = 0 }
                                });

                            if (isExist == 1)
                            {
                                if (data.removedb("kalrelationship", new List<core.db>() {
                                        new core.db() { column = "user_one_id", value = user_one_id },
                                        new core.db() { column = "user_two_id", value = user_two_id }
                                    }))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Arkadaşlık İsteğini geri çektin!" }));
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Arkadaşlık geri çekimi. Bağlantı Hatası!" }));
                                    context.Response.End();
                                }
                            }

                            if (data.insertdb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id },
                                    new core.db() { column = "status", value = 0 }, //0 --> pending
                                    new core.db() { column = "action_user_id", value = senderId}
                                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Arkadaşlık İsteğin gönderildi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Arkadaşlık gönderimi. Bağlantı Hatası!" }));
                                context.Response.End();
                            }
                            #endregion
                        }
                        else if (method == "cancel-active-friendship")
                        {
                            #region cancel active friendshipp
                            int senderId = 0, receiverId = 0;
                            senderId = ((core.user)context.Session["user"]).id;

                            string a = context.Request.Form["receiverId"];
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out receiverId);

                            if (senderId == 0 || receiverId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İşlemi gerçekleştirmek için Giriş yapman gerekli." }));
                                context.Response.End();
                            }
                            //all this time ((one_id < two_id)) !!
                            int user_one_id, user_two_id;
                            if (senderId < receiverId)
                            {
                                user_one_id = senderId;
                                user_two_id = receiverId;
                            }
                            else
                            {
                                user_one_id = receiverId;
                                user_two_id = senderId;
                            }
                            if (data.removedb("kalrelationship", new List<core.db>() {
                                        new core.db() { column = "user_one_id", value = user_one_id },
                                        new core.db() { column = "user_two_id", value = user_two_id }
                                    }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Arkadaşlıktan çıktınız" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Arkadaşlık iptali. Bağlantı Hatası!" }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "accept-friend-request")
                        {
                            #region accepting request of friendship
                            //session  kişi daki receiver.. isteklere bakan
                            int senderId = 0, receiverId = 0;
                            receiverId = ((core.user)context.Session["user"]).id;
                            string a = context.Request.Form["senderId"];
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out senderId);
                            if (senderId == 0 || receiverId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İşlemi gerçekleştirmek için Giriş yapman gerekli." }));
                                context.Response.End();
                            }

                            //all this time ((one_id < two_id)) !!
                            int user_one_id, user_two_id;
                            if (senderId < receiverId)
                            {
                                user_one_id = senderId;
                                user_two_id = receiverId;
                            }
                            else
                            {
                                user_one_id = receiverId;
                                user_two_id = senderId;
                            }
                            int isAlreadyAccepted = data.checkdb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id },
                                    new core.db() { column = "status", value = 1 }
                                });
                            if (isAlreadyAccepted > 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Zaten arkadaşsınız." }));
                                context.Response.End();
                            }

                            if (data.updatedb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "status", value = 1 },
                                    new core.db() { column = "action_user_id", value = receiverId }
                                  }, new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id }
                                  }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Arkadaşlık isteniği onayladın." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı Hatası!" }));
                                context.Response.End();
                            }
                            #endregion
                        }
                        else if (method == "decline-friend-request")
                        {
                            #region decline request of friendship
                            //bu sefer session  kişi daki receiver.. isteklere bakan
                            int senderId = 0, receiverId = 0;
                            receiverId = ((core.user)context.Session["user"]).id;
                            string a = context.Request.Form["senderId"];
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out senderId);
                            if (senderId == 0 || receiverId == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İşlemi gerçekleştirmek için Giriş yapman gerekli." }));
                                context.Response.End();
                            }

                            //all this time ((one_id < two_id)) !!
                            int user_one_id, user_two_id;
                            if (senderId < receiverId)
                            {
                                user_one_id = senderId;
                                user_two_id = receiverId;
                            }
                            else
                            {
                                user_one_id = receiverId;
                                user_two_id = senderId;
                            }
                            int isAlreadyDeclined = data.checkdb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id },
                                    new core.db() { column = "status", value = 2 }
                                });
                            if (isAlreadyDeclined > 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İsteği zaten onaylamadın! Lütfen Bekle." }));
                                context.Response.End();
                            }

                            if (data.updatedb("kalrelationship", new List<core.db>() {
                                    new core.db() { column = "status", value = 2 }, // 2 --> declined
                                    new core.db() { column = "action_user_id", value = receiverId }
                                  }, new List<core.db>() {
                                    new core.db() { column = "user_one_id", value = user_one_id },
                                    new core.db() { column = "user_two_id", value = user_two_id }
                                  }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Arkadaşlık isteniği onaylamadın!" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı Hatası!" }));
                                context.Response.End();
                            }
                            #endregion
                        }
                        #endregion

                        #region GAME <-> USER INTERACTIONS
                        else if (method == "send-request-like-game")
                        {
                            #region send request -> like or unlike to game from user
                            int userid = 0, gameid = 0, isliked = 1;
                            userid = ((core.user)context.Session["user"]).id;
                            string a = context.Request.Form["gameid"];
                            string b = context.Request.Form["isliked"];

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out gameid);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out isliked);

                            int like_interaction = 0;
                            //0 -> Unlike it!
                            //1 -> Like it!
                            string messageToUser = "";
                            //beğenilmiş ise kaldır, beğenilmemiş ise beğen!
                            if (isliked == 0)
                            {
                                like_interaction = 1;
                                messageToUser = "Oyunu Beğendin!";
                            }
                            else if (isliked == 1)
                            {

                                messageToUser = "Oyunu Beğenmekten Vazgeçtin!";
                                like_interaction = 0;
                            }

                            if (userid == 0 || gameid == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "İşlemi gerçekleştirmek için Giriş yapman gerekli." }));
                                context.Response.End();
                            }

                            //kayıt var mı diye kontrol et! varsa update, yoksa insert!
                            var tableName = "usergameinteraction";
                            int isAlreadyIntearct = data.checkdb(tableName, new List<core.db>() {
                                    new core.db() { column = "userid", value = userid },
                                    new core.db() { column = "gameid", value = gameid }
                                });

                            if (isAlreadyIntearct > 0)
                            {
                                //update!
                                if (data.updatedb(tableName, new List<core.db>() {
                                        new core.db() {column = "isliked", value = like_interaction }
                                    }, new List<core.db>() { new core.db() {  column = "userid", value = userid},
                                    new core.db() {  column = "gameid", value = gameid },
                                    new core.db() {  column = "isliked", value = isliked } }))
                                {

                                    context.Response.Write(jss.Serialize(new response() { success = true, message = messageToUser + "" }));
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı Hatası! Update Error. 500!" }));
                                    context.Response.End();
                                }
                            }
                            else
                            {
                                //insert!
                                if (data.insertdb(tableName, new List<core.db>() {
                                      new core.db() { column = "userid", value = userid },
                                      new core.db() { column = "gameid", value = gameid },
                                      new core.db() { column = "isliked", value = 1} }
                                ))
                                {

                                    context.Response.Write(jss.Serialize(new response() { success = true, message = "Oyunu Beğendin!" }));
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı Hatası! Insrt Error. 500!" }));
                                    context.Response.End();
                                }
                            }
                            #endregion
                        }

                        #endregion

                        else if (method == "add-first-user-data")
                        {
                            #region add first user data (expense,income,credit cards )
                            int userid = 0;
                            userid = ((core.user)context.Session["user"]).id;

                            string jobtype = "", bankNameOne = "",
                                bankNameTwo = "",
                                bankNameThree = "",
                                bankNameFour = "",
                                bankNameFive = "";
                            double studentmonthlyincome = 0,
                                studentmonthlyadditionalincome = 0,
                               workermonthlyincome = 0,
                               workermonthlyadditionalincome = 0,
                               houseRent = 0,
                               electricbill = 0, waterbill = 0, gasbill = 0, internetbill = 0, gsmbill = 0, otherbills = 0,
                               individualexpense = 0, marketexpense = 0;
                            double cardLimit1 = 0, cardDebt1 = 0,
                                cardLimit2 = 0, cardDebt2 = 0,
                                cardLimit3 = 0, cardDebt3 = 0,
                                cardLimit4 = 0, cardDebt4 = 0,
                                cardLimit5 = 0, cardDebt5 = 0;
                            string a = context.Request.Form["jobtype"],
                                b1 = context.Request.Form["studentmonthlyincome"],
                                b2 = context.Request.Form["studentmonthlyadditionalincome"],
                                c1 = context.Request.Form["workermonthlyincome"],
                                c2 = context.Request.Form["workermonthlyadditionalincome"],

                                e = context.Request.Form["houseRent"],
                                f = context.Request.Form["electricbill"],
                                g = context.Request.Form["waterbill"],
                                h = context.Request.Form["gasbill"],
                                i = context.Request.Form["internetbill"],
                                j = context.Request.Form["gsmbills"],
                                k = context.Request.Form["otherbills"],
                                l = context.Request.Form["individualexpense"],
                                m = context.Request.Form["marketexpense"],

                                c1a = context.Request.Form["bankNameOne"],
                                c1b = context.Request.Form["cardLimitOne"],
                                c1c = context.Request.Form["cardDebtOne"],

                                c2a = context.Request.Form["bankNameTwo"],
                                c2b = context.Request.Form["cardLimitTwo"],
                                c2c = context.Request.Form["cardDebtTwo"],

                                c3a = context.Request.Form["bankNameThree"],
                                c3b = context.Request.Form["cardLimitThree"],
                                c3c = context.Request.Form["cardDebtThree"],

                                c4a = context.Request.Form["bankNameFour"],
                                c4b = context.Request.Form["cardLimitFour"],
                                c4c = context.Request.Form["cardDebtFour"],

                                c5a = context.Request.Form["bankNameFive"],
                                c5b = context.Request.Form["cardLimitFive"],
                                c5c = context.Request.Form["cardDebtFive"];



                            if (!string.IsNullOrEmpty(a)) jobtype = a.Trim();
                            if (!string.IsNullOrEmpty(b1)) double.TryParse(b1, out studentmonthlyincome);
                            if (!string.IsNullOrEmpty(b2)) double.TryParse(b2, out studentmonthlyadditionalincome);
                            if (!string.IsNullOrEmpty(c1)) double.TryParse(c1, out workermonthlyincome);
                            if (!string.IsNullOrEmpty(c2)) double.TryParse(c2, out studentmonthlyadditionalincome);

                            if (!string.IsNullOrEmpty(e)) double.TryParse(e, out houseRent);
                            if (!string.IsNullOrEmpty(f)) double.TryParse(f, out electricbill);
                            if (!string.IsNullOrEmpty(g)) double.TryParse(g, out waterbill);
                            if (!string.IsNullOrEmpty(h)) double.TryParse(h, out gasbill);
                            if (!string.IsNullOrEmpty(i)) double.TryParse(i, out internetbill);
                            if (!string.IsNullOrEmpty(j)) double.TryParse(j, out gsmbill);
                            if (!string.IsNullOrEmpty(k)) double.TryParse(k, out otherbills);
                            if (!string.IsNullOrEmpty(l)) double.TryParse(l, out individualexpense);
                            if (!string.IsNullOrEmpty(m)) double.TryParse(m, out marketexpense);

                            if (!string.IsNullOrEmpty(c1a)) bankNameOne = c1a.Trim();
                            if (!string.IsNullOrEmpty(c1b)) double.TryParse(c1b, out cardLimit1);
                            if (!string.IsNullOrEmpty(c1c)) double.TryParse(c1c, out cardDebt1);

                            if (!string.IsNullOrEmpty(c2a)) bankNameTwo = c2a.Trim();
                            if (!string.IsNullOrEmpty(c2b)) double.TryParse(c2b, out cardLimit2);
                            if (!string.IsNullOrEmpty(c2c)) double.TryParse(c2c, out cardDebt2);

                            if (!string.IsNullOrEmpty(c3a)) bankNameThree = c3a.Trim();
                            if (!string.IsNullOrEmpty(c3b)) double.TryParse(c3b, out cardLimit3);
                            if (!string.IsNullOrEmpty(c3c)) double.TryParse(c3c, out cardDebt3);

                            if (!string.IsNullOrEmpty(c4a)) bankNameFour = c4a.Trim();
                            if (!string.IsNullOrEmpty(c4b)) double.TryParse(c4b, out cardLimit4);
                            if (!string.IsNullOrEmpty(c4c)) double.TryParse(c4c, out cardDebt4);

                            if (!string.IsNullOrEmpty(c5a)) bankNameFive = c5a.Trim();
                            if (!string.IsNullOrEmpty(c5b)) double.TryParse(c5b, out cardLimit5);
                            if (!string.IsNullOrEmpty(c5c)) double.TryParse(c5c, out cardDebt5);


                            double income = studentmonthlyincome;
                            double additionalincome = studentmonthlyadditionalincome; 
                            if(jobtype == "worker")
                            {
                                income = workermonthlyincome;
                                additionalincome = workermonthlyadditionalincome;
                            }

                            //adding income
                            data.insertdb("userincome", new List<core.db>() {
                                new core.db() { column = "jobtype", value = jobtype },
                                new core.db() { column = "monthlyincome", value = income / 100},
                                new core.db() { column = "monthlyadditionalincome", value = additionalincome / 100 },
                                new core.db() { column = "uploaddate", value = DateTime.Now },
                                new core.db() { column = "userid", value = userid }
                            });

                            //adding outcome
                            data.insertdb("useroutcome", new List<core.db>() {
                                new core.db() { column = "houserent", value = houseRent / 100 },
                                new core.db() { column = "electricbill", value = electricbill / 100 },
                                new core.db() { column = "waterbill", value = waterbill / 100 },
                                new core.db() { column = "gasbill", value = gasbill / 100}, 
                                new core.db() { column = "internetbill", value = internetbill / 100},
                                new core.db() { column = "gsmbill", value = gsmbill / 100},
                                new core.db() { column = "otherbills", value = otherbills / 100},
                                new core.db() { column = "individualexpense", value = individualexpense / 100},
                                new core.db() { column = "marketexpense", value = marketexpense / 100},
                                new core.db() { column = "updatedate", value = DateTime.Now },
                                new core.db() { column = "userid", value = userid }
                            });

                            //card1
                            if(bankNameOne != "" && cardLimit1 != 0 && cardDebt1 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameOne},
                                    new core.db() { column = "cardlimit", value = cardLimit1 / 100 },
                                    new core.db() { column = "carddebt", value = cardDebt1 / 100},
                                    new core.db() { column = "updatedate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card2
                            if (bankNameTwo != "" && cardLimit2 != 0 && cardDebt2 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameTwo},
                                    new core.db() { column = "cardlimit", value = cardLimit2 / 100},
                                    new core.db() { column = "carddebt", value = cardDebt2 / 100},
                                    new core.db() { column = "updatedate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card3
                            if (bankNameThree != "" && cardLimit3 != 0 && cardDebt3 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameThree},
                                    new core.db() { column = "cardlimit", value = cardLimit3 / 100},
                                    new core.db() { column = "carddebt", value = cardDebt3 / 100},
                                    new core.db() { column = "updatedate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card4
                            if (bankNameFour != "" && cardLimit4 != 0 && cardDebt4 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameFour},
                                    new core.db() { column = "cardlimit", value = cardLimit4 / 100},
                                    new core.db() { column = "carddebt", value = cardDebt4 / 100},
                                    new core.db() { column = "updatedate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card5
                            if (bankNameFive != "" && cardLimit5 != 0 && cardDebt5 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameFive},
                                    new core.db() { column = "cardlimit", value = cardLimit5 / 100},
                                    new core.db() { column = "carddebt", value = cardDebt5 / 100},
                                    new core.db() { column = "updatedate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }

                            context.Response.Write(jss.Serialize(new response() { success = true, message = "Girmiş Olduğunuz Bilgileriniz Sisteme Yüklendi!" }));
                            context.Response.End();
                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        #region site post methods

                        if (method == "login")
                        {
                            #region do login

                            string username = context.Request.Form["username"],
                              password = context.Request.Form["password"];

                            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen gerekli yerleri doldur." }));
                                context.Response.End();
                            }

                            var user = data.getuserinfo(username);
                            if (user == null || user.id <= 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kullanıcı adını veya şifreni yanlış girdin." }));
                                context.Response.End();
                            }

                            if (user.status == -1)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Hesabın askıya alınmış." }));
                                context.Response.End();
                            }

                            if (!data.verifypassword(password, "SHA512", user.password))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kullanıcı adını veya şifreni yanlış girdin." }));
                                context.Response.End();
                            }

                            context.Session["user"] = user;

                            data.updatedb("users", new List<core.db>() {
                                    new core.db() { column = "lastlogindate", value = DateTime.Now }
                                  }, new List<core.db>() {
                                    new core.db() { column = "id", value = user.id }
                                  });

                            context.Response.Write(jss.Serialize(new response() { success = true, message = "Giriş başarılı. Yönlendiriliyorsun. Lütfen bekle." }));
                            context.Response.End();

                            #endregion
                        }
                        else if (method == "post-kleine-container-news")
                        {
                            #region post - when user access the news - viewcount increase +1

                            string _id = context.Request.Form["container"];
                            string _viewcount = context.Request.Form["kleine"];

                            int id = 0, viewcount = 0;

                            if (!string.IsNullOrEmpty(_id)) int.TryParse(_id, out id);
                            if (!string.IsNullOrEmpty(_viewcount)) int.TryParse(_viewcount, out viewcount);

                            viewcount = viewcount + 1;

                            if (data.updatedb("kalnews", new List<core.db>() {
                  new core.db() { column = "viewcount", value = viewcount }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = id}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Viewcount arttırıldı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "post-kleine-container-reviews")
                        {
                            #region post - when user access the reviews - viewcount increase +1

                            string _id = context.Request.Form["container"];
                            string _viewcount = context.Request.Form["kleine"];

                            int id = 0, viewcount = 0;

                            if (!string.IsNullOrEmpty(_id)) int.TryParse(_id, out id);
                            if (!string.IsNullOrEmpty(_viewcount)) int.TryParse(_viewcount, out viewcount);

                            viewcount = viewcount + 1;

                            if (data.updatedb("kalreviews", new List<core.db>() {
                  new core.db() { column = "viewcount", value = viewcount }
                }, new List<core.db>() {
                  new core.db() { column = "id", value = id}
                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Viewcount arttırıldı" }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "game-add-color")
                        {
                            #region game-add-color  in game.aspx
                            string a = context.Request.Form["backcolor"],
                              b = context.Request.Form["backcoloropacity"],
                              c = context.Request.Form["maincolor"],
                              d = context.Request.Form["gameid"],
                              backcolor = "",
                              backcoloropacity = "",
                              maincolor = "";
                            int id = 0;
                            if (!string.IsNullOrEmpty(a)) backcolor = a.Trim();
                            if (!string.IsNullOrEmpty(b)) backcoloropacity = b.Trim();
                            if (!string.IsNullOrEmpty(c)) maincolor = c.Trim();
                            if (!string.IsNullOrEmpty(d)) int.TryParse(d, out id);


                            if (data.updatedb("kalgame", new List<core.db>() {
                                   new core.db() { column = "bc", value = backcolor},
                                   new core.db() { column = "bca", value = backcoloropacity},
                                   new core.db() { column = "mc", value = maincolor }

                               }, new List<core.db>() {
                                      new core.db() { column = "id", value = id},

                               }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Renk işlendi" }));
                                context.Response.End();

                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }


                            #endregion
                        }
                        #endregion
                    }
                }
            }
            else if (context.Request.HttpMethod == "GET")
            {
                core.data data = new core.data();
                string method = !string.IsNullOrEmpty(context.Request.QueryString["fm"]) ? context.Request.QueryString["fm"] : "";
                if (!string.IsNullOrEmpty(method))
                {
                    if (context.Session["user"] != null)
                    {
                        if (method == "user-auths")
                        {
                            #region get user auths

                            string googleemail = Convert.ToString(data.getsinglecolumndb("auths", "email", new List<core.db>() {
                                new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                new core.db() { column = "authtype", value = 1 }
                              }) ?? ""),
                              twitchemail = Convert.ToString(data.getsinglecolumndb("auths", "email", new List<core.db>() {
                                new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                new core.db() { column = "authtype", value = 2 }
                              }) ?? ""),
                              facebookemail = Convert.ToString(data.getsinglecolumndb("auths", "email", new List<core.db>() {
                                new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                new core.db() { column = "authtype", value = 3 }
                              }) ?? ""),
                              steamemail = Convert.ToString(data.getsinglecolumndb("auths", "email", new List<core.db>()
                              {
                                      new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
                                      new core.db() { column = "authtype", value = 4 }
                              }) ?? "");

                            var auths = new userauth()
                            {
                                google = new auth() { linked = !string.IsNullOrEmpty(googleemail), email = googleemail },
                                twitch = new auth() { linked = !string.IsNullOrEmpty(twitchemail), email = twitchemail },
                                facebook = new auth() { linked = !string.IsNullOrEmpty(facebookemail), email = facebookemail },
                                steam = new auth() { linked = !string.IsNullOrEmpty(steamemail), email = steamemail }
                            };

                            context.Response.Write(jss.Serialize(auths));
                            context.Response.End();

                            #endregion
                        }
                        else if (method == "user-info")
                        {
                            #region get user info

                            core.user profile = data.getuserinfo(((core.user)context.Session["user"]).id);
                            if (profile != null)
                            {
                                context.Response.Write(jss.Serialize(profile));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new core.profile() { namesurname = "", about = "", birthday = 0, birthmonth = 0, birthyear = 0 }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if( method == "check-user-income")
                        {
                            #region check user income 
                            int userid = ((core.user)context.Session["user"]).id;

                            int count = Convert.ToInt32(data.getsinglecolumndbcount("userincome", new List<core.db>() {
                                new core.db() { column = "userid", value = userid }
                            }));

                            if(count > 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Income is already added!" }));
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "No Income is added!" }));
                            }

                            context.Response.End();
                            #endregion
                        }
                        else if( method == "get-user-income")
                        {
                            #region get user income
                                int userid = ((core.user)context.Session["user"]).id;
                                context.Response.Write(jss.Serialize(data.getuserincome(userid)));
                                context.Response.End();
                            #endregion
                        }
                        else if (method == "get-user-outcome")
                        {
                            #region get user outcome
                                int userid = ((core.user)context.Session["user"]).id;
                                context.Response.Write(jss.Serialize(data.getuseroutcome(userid)));
                                context.Response.End();
                            #endregion
                        }
                        else if (method == "get-user-creditcards")
                        {
                            #region get user creditcards
                                int userid = ((core.user)context.Session["user"]).id;
                                context.Response.Write(jss.Serialize(data.getusercreditcards(userid)));
                                context.Response.End();
                            #endregion
                        }
                    }
                    else
                    {
                        #region site get methods

                        if (method == "search-game")
                        {
                            #region search game

                            int page = 1, itemperpage = 5;
                            string query = context.Request.QueryString["query"];

                            if (!string.IsNullOrEmpty(query)) query = query.Trim();

                            //context.Response.Write(jss.Serialize(data.getgames(query, page, itemperpage)));
                            context.Response.End();

                            #endregion
                        }
                        #endregion
                    }
                }
            }

            context.Response.Write(jss.Serialize(res));
            context.Response.End();
        }
        #region objects

        class response
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
        class userauth
        {
            public auth google { get; set; }
            public auth twitch { get; set; }
            public auth facebook { get; set; }
            public auth steam { get; set; }
        }
        class auth
        {
            public bool linked { get; set; }
            public string email { get; set; }
        }
        #endregion
        #region methods

        public static bool isvalidavatar(HttpPostedFile postedFile)
        {
            if (postedFile.ContentType.ToLower() != "image/jpg" && postedFile.ContentType.ToLower() != "image/jpeg" && postedFile.ContentType.ToLower() != "image/png") return false;
            if (System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".jpg" && System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".png" && System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg") return false;
            try
            {
                if (!postedFile.InputStream.CanRead) return false;
                if (postedFile.ContentLength < 100 || postedFile.ContentLength > 1000000) return false;
                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline)) return false;
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                    if (bitmap.Height > 200 || bitmap.Width > 200) return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static bool isvalidcover(HttpPostedFile postedFile)
        {
            if (postedFile.ContentType.ToLower() != "image/jpg" && postedFile.ContentType.ToLower() != "image/jpeg" && postedFile.ContentType.ToLower() != "image/png") return false;
            if (System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".jpg" && System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".png" && System.IO.Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg") return false;
            try
            {
                if (!postedFile.InputStream.CanRead) return false;
                if (postedFile.ContentLength < 100 || postedFile.ContentLength > 4000000) return false;
                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline)) return false;
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                    if (bitmap.Height > 1080 || bitmap.Width > 1920) return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}