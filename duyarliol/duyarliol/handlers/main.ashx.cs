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
                        else if(method == "update-user-income")
                        {
                            #region update user income

                            string a = context.Request.Form["jobtype"],
                            b = context.Request.Form["monthlyincome"],
                            c = context.Request.Form["monthlyadditionalincome"];

                            string jobtype = "";
                            double income = 0, additionalincome = 0;

                            if (!string.IsNullOrEmpty(a)) jobtype = a.Trim();
                            if (!string.IsNullOrEmpty(b)) double.TryParse(b, out income);
                            if (!string.IsNullOrEmpty(c)) double.TryParse(c, out additionalincome);

                            if (data.updatedb("userincome", new List<core.db>() {
                                           new core.db() { column = "jobtype", value = jobtype },
                                           new core.db() { column = "monthlyincome", value = (income) },
                                           new core.db() { column = "monthlyadditionalincome", value = (additionalincome) },
                                           new core.db() { column = "updatedate", value = DateTime.Now }
                                        }, new List<core.db>() {
                                      new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id }
                                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Gelir Bilgilerin Güncellendi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "update-user-outcome")
                        {
                            #region update user outcome

                            string a = context.Request.Form["houserent"],
                            b = context.Request.Form["electricbill"],
                            c = context.Request.Form["waterbill"],
                            d = context.Request.Form["gasbill"],
                            e = context.Request.Form["otherbills"],
                            f = context.Request.Form["internetbill"],
                            g = context.Request.Form["individualexpense"],
                            h = context.Request.Form["marketexpense"],
                            j = context.Request.Form["gsmbill"];

                            double house = 0, electric = 0, water = 0, gas = 0, others = 0, internet = 0, individual = 0, market = 0, gsm = 0;

                            if (!string.IsNullOrEmpty(a)) double.TryParse(a, out house);
                            if (!string.IsNullOrEmpty(b)) double.TryParse(b, out electric);
                            if (!string.IsNullOrEmpty(c)) double.TryParse(c, out water);
                            if (!string.IsNullOrEmpty(d)) double.TryParse(d, out gas);
                            if (!string.IsNullOrEmpty(e)) double.TryParse(e, out others);
                            if (!string.IsNullOrEmpty(f)) double.TryParse(f, out internet);
                            if (!string.IsNullOrEmpty(g)) double.TryParse(g, out individual);
                            if (!string.IsNullOrEmpty(h)) double.TryParse(h, out market);
                            if (!string.IsNullOrEmpty(j)) double.TryParse(j, out gsm);
                            
                            if (data.updatedb("useroutcome", new List<core.db>() {
                                           new core.db() { column = "houserent", value = (house) },
                                           new core.db() { column = "electricbill", value = (electric) },
                                           new core.db() { column = "waterbill", value = (water) },
                                           new core.db() { column = "gasbill", value = (gas) },
                                           new core.db() { column = "gsmbill", value = (gsm) },
                                           new core.db() { column = "otherbills", value = (others) },
                                           new core.db() { column = "individualexpense", value = (individual) },
                                           new core.db() { column = "marketexpense", value = (market) },
                                           new core.db() { column = "internetbill", value = (internet) },
                                           new core.db() { column = "updatedate", value = DateTime.Now }
                                        }, new List<core.db>() {
                                      new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id }
                                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Gider Bilgilerin Güncellendi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if (method == "add-credit-card")
                        {
                            #region add credit card

                            string a = context.Request.Form["bankname"],
                            b = context.Request.Form["cardlimit"],
                            c = context.Request.Form["carddebt"];

                            string bankname = "";
                            double cardlimit = 0, carddebt = 0;

                            if (!string.IsNullOrEmpty(a)) bankname = a.Trim();
                            if (!string.IsNullOrEmpty(b)) double.TryParse(b, out cardlimit);
                            if (!string.IsNullOrEmpty(c)) double.TryParse(c, out carddebt);

                            if (string.IsNullOrEmpty(bankname))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen, Geçerli Bir Kart İsmi Giriniz!" }));
                                context.Response.End();
                            }
                            if (carddebt == 0 || cardlimit == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kart Limiti/Borcu Boş Bırakılamaz!" }));
                                context.Response.End();
                            }

                            if (cardlimit < carddebt)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kart Borcu, Kart Limitinden Yüksek Olamaz!" }));
                                context.Response.End();
                            }

                            if (data.insertdb("usercreditcards", new List<core.db>() {
                                           new core.db() { column = "bankname", value = bankname },
                                           new core.db() { column = "cardlimit", value = (cardlimit) },
                                           new core.db() { column = "carddebt", value = (carddebt) },
                                           new core.db() { column = "updatedate", value = DateTime.Now },
                                           new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id }
                            }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = string.Format("{0} İsimli Kredi Kartın Başarıyla Eklendi!", bankname) }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }
                            #endregion
                        }
                        else if(method == "update-credit-card")
                        {
                            #region update credit card

                            string a = context.Request.Form["bankname"],
                                b = context.Request.Form["cardlimit"],
                                c = context.Request.Form["carddebt"],
                                d = context.Request.Form["creditcardid"];

                            string bankname = "";
                            double cardlimit = 0, carddebt = 0, creditcardid = 0;

                            if (!string.IsNullOrEmpty(a)) bankname = a.Trim();
                            if (!string.IsNullOrEmpty(b)) double.TryParse(b, out cardlimit);
                            if (!string.IsNullOrEmpty(c)) double.TryParse(c, out carddebt);
                            if (!string.IsNullOrEmpty(d)) double.TryParse(d, out creditcardid);

                            if (string.IsNullOrEmpty(bankname))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Lütfen, Geçerli Bir Kart İsmi Giriniz!" }));
                                context.Response.End();
                            }

                            if (carddebt == 0 || cardlimit == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kart Limiti/Borcu Boş Bırakılamaz!" }));
                                context.Response.End();
                            }

                            if (cardlimit < carddebt)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Kart Borcu, Kart Limitinden Yüksek Olamaz!" }));
                                context.Response.End();
                            }

                            if (data.updatedb("usercreditcards", new List<core.db>() {
                                           new core.db() { column = "bankname", value = bankname },
                                           new core.db() { column = "cardlimit", value = (cardlimit) },
                                           new core.db() { column = "carddebt", value = (carddebt) },
                                           new core.db() { column = "updatedate", value = DateTime.Now }
                                        }, new List<core.db>() {
                                      new core.db() { column = "id", value = creditcardid}
                                }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Kredi Kartı Bilgilerin Güncellendi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

                            #endregion
                        }
                        else if(method == "delete-credit-card")
                        {
                            #region update user income

                            string a = context.Request.Form["creditcardid"];
                            int creditcardid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out creditcardid);

                            if(data.removedb("usercreditcards", new List<core.db>() {
                                  new core.db() { column = "id", value = creditcardid}
                            }))
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = "Seçmiş Olduğun Kredi Kartı Silindi." }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Bağlantı hatası." }));
                                context.Response.End();
                            }

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
                                new core.db() { column = "monthlyincome", value = income },
                                new core.db() { column = "monthlyadditionalincome", value = additionalincome },
                                new core.db() { column = "createddate", value = DateTime.Now },
                                new core.db() { column = "userid", value = userid }
                            });

                            //adding outcome
                            data.insertdb("useroutcome", new List<core.db>() {
                                new core.db() { column = "houserent", value = houseRent },
                                new core.db() { column = "electricbill", value = electricbill },
                                new core.db() { column = "waterbill", value = waterbill },
                                new core.db() { column = "gasbill", value = gasbill }, 
                                new core.db() { column = "internetbill", value = internetbill},
                                new core.db() { column = "gsmbill", value = gsmbill },
                                new core.db() { column = "otherbills", value = otherbills},
                                new core.db() { column = "individualexpense", value = individualexpense},
                                new core.db() { column = "marketexpense", value = marketexpense},
                                new core.db() { column = "createddate", value = DateTime.Now },
                                new core.db() { column = "userid", value = userid }
                            });

                            //card1
                            if(bankNameOne != "" && cardLimit1 != 0 && cardDebt1 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameOne},
                                    new core.db() { column = "cardlimit", value = cardLimit1 },
                                    new core.db() { column = "carddebt", value = cardDebt1},
                                    new core.db() { column = "createddate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card2
                            if (bankNameTwo != "" && cardLimit2 != 0 && cardDebt2 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameTwo},
                                    new core.db() { column = "cardlimit", value = cardLimit2},
                                    new core.db() { column = "carddebt", value = cardDebt2 },
                                    new core.db() { column = "createddate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card3
                            if (bankNameThree != "" && cardLimit3 != 0 && cardDebt3 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameThree},
                                    new core.db() { column = "cardlimit", value = cardLimit3},
                                    new core.db() { column = "carddebt", value = cardDebt3 },
                                    new core.db() { column = "createddate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card4
                            if (bankNameFour != "" && cardLimit4 != 0 && cardDebt4 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameFour},
                                    new core.db() { column = "cardlimit", value = cardLimit4 },
                                    new core.db() { column = "carddebt", value = cardDebt4 },
                                    new core.db() { column = "createddate", value = DateTime.Now },
                                    new core.db() { column = "userid", value = userid }
                                });
                            }
                            //card5
                            if (bankNameFive != "" && cardLimit5 != 0 && cardDebt5 != 0)
                            {
                                data.insertdb("usercreditcards", new List<core.db>() {
                                    new core.db() { column = "bankname", value = bankNameFive},
                                    new core.db() { column = "cardlimit", value = cardLimit5 },
                                    new core.db() { column = "carddebt", value = cardDebt5 },
                                    new core.db() { column = "createddate", value = DateTime.Now },
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
                        #region user logged

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
                        else if(method == "get-user-wishlist")
                        {
                            #region get user wishlist
                            int userid = ((core.user)context.Session["user"]).id;
                            context.Response.Write(jss.Serialize(data.getuserwishlist(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "get-user-pending-wishlist")
                        {
                            #region get user pending wishlist
                            string a = context.Request.QueryString["a"],
                                 b = context.Request.QueryString["b"];

                            int itemperpage = 20, page = 1;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out itemperpage);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out page);

                            int userid = ((core.user)context.Session["user"]).id;
                            context.Response.Write(jss.Serialize(data.getuserwishlist(userid, 1, itemperpage, page)));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "get-user-completed-wishlist")
                        {
                            #region get user completed wishlist

                            string a = context.Request.QueryString["a"],
                                b = context.Request.QueryString["b"];

                            int itemperpage = 20, page = 1;

                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out itemperpage);
                            if (!string.IsNullOrEmpty(b)) int.TryParse(b, out page);

                            int userid = ((core.user)context.Session["user"]).id;
                            context.Response.Write(jss.Serialize(data.getuserwishlist(userid, 0, itemperpage, page)));
                            context.Response.End();

                            #endregion
                        }
                        else if(method == "get-user-answerlist")
                        {
                            #region get user wishlist
                            int userid = ((core.user)context.Session["user"]).id;
                            context.Response.Write(jss.Serialize(data.getuserwishlist(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "get-user-info")
                        {
                            #region get user info  FOR Chrome Extension
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getuserinfo(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "get-credit-cards-ce")
                        {
                            #region get credit cards FOR Chrome Extension 
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getcreditcards(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "check-answer-list-ce")
                        {
                            #region check answer list FOR Chrome Extension 
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getsinglecolumndbcount("answerlist", new List<core.db>() { new core.db() { column = "userid", value = userid } })));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "run-chrome-extension-control")
                        {
                            #region run chrome extension control

                            string userid = context.Request.QueryString["userid"],
                             wl = context.Request.QueryString["wishlist"],
                              al = context.Request.QueryString["answerlist"];

                            if (context.Session["user"] != null)
                            {
                                if (((core.user)context.Session["user"]).id != Convert.ToInt32(userid))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Duyarlı.ol Sitesine Tekrar Giriş Yapmalısın!" }));
                                    context.Response.End();
                                }
                            }


                            var wishlist = jss.Deserialize<List<wishlist>>(wl);
                            var answerlist = jss.Deserialize<List<answerlist>>(al);

                            #region first check income / outcome

                            double userincometotal = data.getuserincometotal(Convert.ToInt32(userid));

                            if (userincometotal == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Gelir Bilgilerin Bulunmamakta! \n Lütfen Gelir Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }
                            double useroutcometotal = data.getuseroutcometotal(Convert.ToInt32(userid));

                            if (useroutcometotal == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Gider Bilgilerin Bulunmamakta! \n Lütfen Gider Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }

                            double net = userincometotal - useroutcometotal;
                            if (net < 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Net Paran Bulunmamakta!" }));
                                context.Response.End();
                            }
                            #endregion

                            double wishTotal = 0;
                            double interestrate = 0;

                            double usercreditdebtstotal = 0;
                            var creditcardlist = data.getusercreditcards(Convert.ToInt32(userid));

                            if (creditcardlist.Count == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Kredi Kartı Bilgilerin Bulunmamakta! \n Lütfen Kredi Kartı Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }

                            #region wishlist 
                            /* öncelikle gelen istekler wishlist db'ye kaydedilir. */
                            foreach (var wish in wishlist)
                            {
                                data.insertdb("wishlist", new List<core.db>() {
                                    new core.db() { column = "ordername",  value = wish.name },
                                    new core.db() { column = "ordercount", value = Convert.ToInt32(wish.count) },
                                    new core.db() { column = "orderprice", value = (float)Convert.ToDouble(wish.price) / 100 },
                                    new core.db() { column = "orderdate", value = DateTime.Now },
                                    new core.db() { column = "pending", value = 1 },
                                    new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                                });
                                wishTotal += Convert.ToDouble(wish.price) / 100;
                            }
                            #endregion

                            #region answerlist

                            /* verilen cevaplara interest rate oluşturma*/
                            int turn = 0;
                            foreach (var answr in answerlist)
                            {
                                if (turn == 0 || turn == 1 || turn == 3)
                                {
                                    if (answr.answer.Contains("evet")) interestrate += 5;
                                }
                                else if (turn == 2 || turn == 4)
                                {
                                    if (answr.answer.Contains("hayır")) interestrate += 5;
                                }
                                else
                                {
                                    if (answr.answer.Contains("hayır")) interestrate += 10;
                                }
                                turn++;
                            }


                            /* cevaplar kontrol edilir */
                            int checkanswers = Convert.ToInt32(data.getsinglecolumndbcount("answerlist", new List<core.db>() { new core.db() { column = "userid", value = Convert.ToInt32(userid) } }));
                            var counter = 1;
                            if (checkanswers == 0)
                            {
                                /* cevaplar veritabanına kaydedilir */
                                foreach (var answerobject in answerlist)
                                {
                                    data.insertdb("answerlist", new List<core.db>()
                                    {
                                        new core.db() { column = "question", value = "question"+counter  },
                                        new core.db() { column = "answer", value = answerobject.answer  },
                                        new core.db() { column = "date", value = DateTime.Now },
                                        new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                                    });
                                    counter++;
                                }
                            }
                            //else
                            //{
                            //    foreach (var answerobject in answerlist)
                            //    {
                            //        data.updatedb(
                            //            "answerlist",
                            //            new List<core.db>()
                            //            {
                            //                new core.db() { column = "answer", value = answerobject.answer  },
                            //                new core.db() { column = "date", value = DateTime.Now }
                            //            },
                            //            new List<core.db>()
                            //            {
                            //                new core.db() { column = "question", value = "question"+counter  },
                            //                new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                            //            }
                            //        );
                            //        counter++;
                            //    }
                            //}
                            #endregion


                            bool wishHigherThanAllCard = true;

                            foreach (var card in creditcardlist)
                            {
                                usercreditdebtstotal += (card.carddebt * 30) / 100; //asgari tutar 

                                if (wishTotal < (card.cardlimit - card.carddebt))
                                {
                                    wishHigherThanAllCard = false; //herhangibir kk alışveriş için uygunsa 'false' gelir hata almadan bi sonraki adıma geçilir.
                                }
                            }

                            if (wishHigherThanAllCard)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Sistemdeki Kredi Kartı Bakiyelerin Alışveriş İçin Yetersiz!") }));
                                context.Response.End();
                            }

                            net -= usercreditdebtstotal;
                            if (net < 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Kredi Kartı Borçlarından Dolayı Alışveriş Yapmamalısın!") }));
                                context.Response.End();
                            }

                            double last = net - (wishTotal + ((wishTotal * interestrate) / 100));
                            if (last < 0)
                            {

                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Duyarlı.Ol'a katıldığın için teşekkürler. \n Vermiş Olduğun Bilgiler Doğrultusunda Sepetindeki Almamalısın!") }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = string.Format("Duyarlı.Ol'a katıldığın için teşekkürler. \n Sepetindeki Ürünleri Almaman Konusunda Hiçbir Sakınca Bulamadık. Duyarlı Kalın!") }));
                                context.Response.End();
                            }

                            #endregion
                        }
                      

                        #endregion
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
                        else if(method == "get-user-info")
                        {
                            #region get user info  FOR Chrome Extension
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getuserinfo(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if(method == "get-credit-cards-ce")
                        {
                            #region get credit cards FOR Chrome Extension 
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getcreditcards(userid)));
                            context.Response.End();
                            #endregion
                        }
                        else if(method == "check-answer-list-ce")
                        {
                            #region check answer list FOR Chrome Extension 
                            string a = context.Request.QueryString["id"];
                            int userid = 0;
                            if (!string.IsNullOrEmpty(a)) int.TryParse(a, out userid);

                            context.Response.Write(jss.Serialize(data.getsinglecolumndbcount("answerlist", new List<core.db>() { new core.db() {  column = "userid", value = userid} })));
                            context.Response.End();
                            #endregion
                        }
                        else if (method == "run-chrome-extension-control")
                        {
                            #region run chrome extension control

                            string userid = context.Request.QueryString["userid"],
                             wl = context.Request.QueryString["wishlist"],
                              al = context.Request.QueryString["answerlist"];

                            if (context.Session["user"] != null)
                            {
                                if (((core.user)context.Session["user"]).id != Convert.ToInt32(userid))
                                {
                                    context.Response.Write(jss.Serialize(new response() { success = false, message = "Duyarlı.ol Sitesine Tekrar Giriş Yapmalısın!" }));
                                    context.Response.End();
                                }
                            }


                            var wishlist = jss.Deserialize<List<wishlist>>(wl);
                            var answerlist = jss.Deserialize<List<answerlist>>(al);

                            #region first check income / outcome

                            double userincometotal = data.getuserincometotal(Convert.ToInt32(userid));

                            if (userincometotal == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Gelir Bilgilerin Bulunmamakta! \n Lütfen Gelir Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }
                            double useroutcometotal = data.getuseroutcometotal(Convert.ToInt32(userid));

                            if (useroutcometotal == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Gider Bilgilerin Bulunmamakta! \n Lütfen Gider Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }

                            double net = userincometotal - useroutcometotal;
                            if (net < 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Net Paran Bulunmamakta!" }));
                                context.Response.End();
                            }
                            #endregion

                            double wishTotal = 0;
                            double interestrate = 0;

                            double usercreditdebtstotal = 0;
                            var creditcardlist = data.getusercreditcards(Convert.ToInt32(userid));

                            if (creditcardlist.Count == 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = "Sistemde Kayıtlı Kredi Kartı Bilgilerin Bulunmamakta! \n Lütfen Kredi Kartı Bilgilerini Doldurup Tekrar Deneyiniz!" }));
                                context.Response.End();
                            }

                            #region wishlist 
                            /* öncelikle gelen istekler wishlist db'ye kaydedilir. */
                            foreach (var wish in wishlist)
                            {
                                data.insertdb("wishlist", new List<core.db>() {
                                    new core.db() { column = "ordername",  value = wish.name },
                                    new core.db() { column = "ordercount", value = Convert.ToInt32(wish.count) },
                                    new core.db() { column = "orderprice", value = (float)Convert.ToDouble(wish.price) / 100 },
                                    new core.db() { column = "orderdate", value = DateTime.Now },
                                    new core.db() { column = "pending", value = 1 },
                                    new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                                });
                                wishTotal += Convert.ToDouble(wish.price) / 100;
                            }
                            #endregion

                            #region answerlist

                            /* verilen cevaplara interest rate oluşturma*/
                            int turn = 0;
                            foreach (var answr in answerlist)
                            {
                                if (turn == 0 || turn == 1 || turn == 3)
                                {
                                    if (answr.answer.Contains("evet")) interestrate += 5;
                                }
                                else if (turn == 2 || turn == 4)
                                {
                                    if (answr.answer.Contains("hayır")) interestrate += 5;
                                }
                                else
                                {
                                    if (answr.answer.Contains("hayır")) interestrate += 10;
                                }
                                turn++;
                            }


                            /* cevaplar kontrol edilir */
                            int checkanswers = Convert.ToInt32(data.getsinglecolumndbcount("answerlist", new List<core.db>() { new core.db() { column = "userid", value = Convert.ToInt32(userid) } }));
                            var counter = 1;
                            if (checkanswers == 0)
                            {
                                /* cevaplar veritabanına kaydedilir */
                                foreach (var answerobject in answerlist)
                                {
                                    data.insertdb("answerlist", new List<core.db>()
                                    {
                                        new core.db() { column = "question", value = "question"+counter  },
                                        new core.db() { column = "answer", value = answerobject.answer  },
                                        new core.db() { column = "date", value = DateTime.Now },
                                        new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                                    });
                                    counter++;
                                }
                            }
                            //else
                            //{
                            //    foreach (var answerobject in answerlist)
                            //    {
                            //        data.updatedb(
                            //            "answerlist",
                            //            new List<core.db>()
                            //            {
                            //                new core.db() { column = "answer", value = answerobject.answer  },
                            //                new core.db() { column = "date", value = DateTime.Now }
                            //            },
                            //            new List<core.db>()
                            //            {
                            //                new core.db() { column = "question", value = "question"+counter  },
                            //                new core.db() { column = "userid", value = Convert.ToInt32(userid) }
                            //            }
                            //        );
                            //        counter++;
                            //    }
                            //}
                            #endregion

                            
                            bool wishHigherThanAllCard = true;

                            foreach (var card in creditcardlist)
                            {
                                usercreditdebtstotal += (card.carddebt * 30) / 100; //asgari tutar 

                                if (wishTotal < (card.cardlimit - card.carddebt))
                                {
                                    wishHigherThanAllCard = false; //herhangibir kk alışveriş için uygunsa 'false' gelir hata almadan bi sonraki adıma geçilir.
                                }
                            }

                            if (wishHigherThanAllCard)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Sistemdeki Kredi Kartı Bakiyelerin Alışveriş İçin Yetersiz!") }));
                                context.Response.End();
                            }

                            net -= usercreditdebtstotal;
                            if (net < 0)
                            {
                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Kredi Kartı Borçlarından Dolayı Alışveriş Yapmamalısın!") }));
                                context.Response.End();
                            }

                            double last = net - (wishTotal + ((wishTotal * interestrate) / 100));
                            if (last < 0)
                            {

                                context.Response.Write(jss.Serialize(new response() { success = false, message = string.Format("Duyarlı.Ol'a katıldığın için teşekkürler. \n Vermiş Olduğun Bilgiler Doğrultusunda Sepetindeki Almamalısın!") }));
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(jss.Serialize(new response() { success = true, message = string.Format("Duyarlı.Ol'a katıldığın için teşekkürler. \n Sepetindeki Ürünleri Almaman Konusunda Hiçbir Sakınca Bulamadık. Duyarlı Kalın!") }));
                                context.Response.End();
                            }

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

   
    public class wishlist
    {
        public string name { get; set; }
        public string count { get; set; }
        public string price { get; set; }

    }
    public class answerlist
    {
        public string answer { get; set; }

    }
}