using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Woooo.Models;

namespace Woooo.Controllers
{
    public class MainCredentialsController : Controller
    {
        WooooEntities db = new WooooEntities();
        // GET: MainCredentials
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string userName, string Password)
        {
            try
            {
                var entity = db.UserRoles.Where(e => e.Username == userName && e.Password == Password).FirstOrDefault();
                if (entity!=null)
                {
                    Session["userName"] = entity.Account_Id;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("contact", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string Pass,string CPassword)
        {
            try
            {
                var message = "";
                if (ModelState.IsValid)
                {
                    string fname = Request["FirstName"].ToString();
                    string Lname = Request["LastName"].ToString();
                    string Email = Request["Email"].ToString();
                    Account user = new Account()
                    {
                        FirstName=fname,
                        LastName=Lname,
                        Email=Email
                    };
                    if (user!=null)
                    {
                        var check = db.Accounts.Where(e => e.Email == Email).FirstOrDefault().ToString();
                        if(check !=null)
                        {
                            db.Accounts.Add(user);
                            db.SaveChanges();


                            var max = 0;
                            max = Convert.ToInt32(db.Accounts.OrderByDescending(p => p.AccountId).FirstOrDefault().AccountId);

                            SendVerificationEmail(max);

                            UserRole role = new UserRole()
                            {
                                Account_Id = max,
                                Password = Pass,
                                Username = user.Email
                            };

                            db.UserRoles.Add(role);
                            db.SaveChanges();
                            message = "Successfully Created";

                            Session["userName"] = max;
                        }
                        else
                        {
                            message = "Email is Already Exist";
                        }
                        
                        
                    }
                    else
                    {
                        message = "Enter All Fields";
                    }
                }
                ViewBag.Message = message;
                return RedirectToAction("Index", "Dashboard");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string imgLink(HttpPostedFileBase ImageofUser)
        {
            if (Session["UserName"] != null)
            {
                var DateeTime = DateTime.Now.ToString("yyyyMMdd_hhssms");
                var fname = ImageofUser.FileName;
                var fullnamee = DateeTime + "_" + fname;
                var ext = Path.GetExtension(fname);
                var extension = ext.ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    var path = Server.MapPath("~/KYCData");
                    var fullpath = Path.Combine(path, fullnamee);
                    ImageofUser.SaveAs(fullpath);
                    return fullnamee;
                }
                else
                {
                    var path = Server.MapPath("~/KYCData");
                    var fullpath = Path.Combine(path, fullnamee);
                    ImageofUser.SaveAs(fullpath);
                    return fullnamee;
                }
            }
            else
            {
                return null;
            }
        }

        public ActionResult Logout()
        {
            if (Session["UserName"] != null)
            {
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public bool SendVerificationEmail(long acId)
        {
            try
            {
                var ac = db.Accounts.Find(acId);
                var getAllCodes = db.EmailVerifications.Where(x => x.Account_Id == ac.AccountId).ToList();
                if (getAllCodes.Count > 0)
                {
                    foreach (var item in getAllCodes)
                    {
                        item.IsExpired = true;
                        db.SaveChanges();
                    }
                }
                var verficationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks + ":" + ac.Email));
                db.EmailVerifications.Add(new EmailVerification()
                {
                    Account_Id = acId,
                    ExpirationDate = DateTime.Now.AddMinutes(20),
                    IsExpired = false,
                    VerificationCode = verficationCode
                });
                db.SaveChanges();

                #region body
                string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
                            <div style=""width:95%;margin:auto;height:70px;background-color:black"">
                                <div style=""float:left;background-color:black;height:100%"">
                                    //<img src=""https://images.walahala.org/Woooo_logo.png""style=""width:62px; padding-left:10px; padding-right:10px ; padding-top:4px""/>
                                </div>
                                <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                                    <p style=""padding:20px;text-align:center"">  Welcome To Woooo</p>
                                </div>
                            </div>
                            <br />
                            <div style=""width:95%;margin:auto"">
                                <br /><br /><br />
                                <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear " + ac.Email + @",


               
                                </div>
                                <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                                    Thank you for signing up with Woooo. To provide you the best service possible. 

                                </div>

                                <br />
                                <hr />
                                <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #BF9000"">
                                   Best regards,<br />
                                   Woooo Team
                                </div>
                                <br />
                                <br />
                                <br />

            

            
                        </div>


                        <div style=""width:95%;margin:auto;height:50px;background-color:#BF9000"">

          

                        <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS ARE RESERVED BY <br /> Woooo
                </div>
                </div>
                </div>";
                #endregion

                new Thread(() => SendMail.SendEmail(body, "Verify your email address", new string[] { ac.Email })).Start();

            }
            catch (Exception)
            {
            }
            return true;
        }

    }
}