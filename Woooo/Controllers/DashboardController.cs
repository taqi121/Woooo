using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Woooo.Models;

namespace Woooo.Controllers
{
    public class DashboardController : Controller
    {
        WooooEntities db = new WooooEntities();
        public ActionResult Index()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        // GET: Dashboard

        public ActionResult Details()
        {
            return View();
        }
        public ActionResult KYC()
        {
            try
            {
                if(Session["UserName"] !=null)
                {
                    var id = Convert.ToInt32(Session["UserName"]);
                    var entity = db.Accounts.Where(e => e.AccountId == id).FirstOrDefault();
                    ViewBag.Record = entity;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult KYC(HttpPostedFileBase IDFrnt, HttpPostedFileBase IDBack, HttpPostedFileBase otherDoc, HttpPostedFileBase ImgUser)
        {
            var message = "";
            try
            {
                if(Session["UserName"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        var idAccount = Convert.ToInt32(Request["idAccounts"]);

                        var firstName = Request["FirstName"].ToString();
                        var middleName = Request["MiddleName"].ToString();
                        var LastName = Request["LastName"].ToString();
                        var Email = Request["email"].ToString();
                        var Address1 = Request["address1"].ToString();
                        var address2 = Request["address2"].ToString();
                        var DOBMonth = Convert.ToInt32(Request["DOBMonth"]);
                        var DOBDay = Convert.ToInt32(Request["DOBDay"]);
                        var DOBYear = Convert.ToInt32(Request["DOBYear"]);
                        var Country = Request["country"].ToString();
                        var zipcode = Request["zipCode"].ToString();
                        var idNumber = Request["IdNumber"].ToString();

                        var idFrontLink = imgLink(IDFrnt).ToString();
                        var idBackLink = imgLink(IDBack).ToString();
                        var otherDocumnet = imgLink(otherDoc).ToString();
                        var picuserLink = imgLink(ImgUser).ToString();



                        var entity = db.Accounts.Where(e => e.AccountId == idAccount).FirstOrDefault();

                        if (entity != null)
                        {

                            entity.MiddleName = middleName;
                            entity.Address1 = Address1;
                            entity.Address2 = address2;
                            entity.DOBDay = DOBDay;
                            entity.DOBYear = DOBYear;
                            entity.DOBMonth = DOBMonth;
                            entity.FirstName = firstName;
                            entity.LastName = LastName;
                            entity.Country = Country;
                            entity.ZipCode = zipcode;
                            entity.IdentityNumber = idNumber;
                            entity.IdentityProofURLFront = idFrontLink;
                            entity.IdentityProofURLBack = idBackLink;
                            entity.OtherDocuments = otherDocumnet;
                            entity.ImageURL = picuserLink;
                            entity.AccountEnabled = true;
                            entity.EnableOn = DateTime.Now;
                            entity.TwoFactorEnabled = false;
                            entity.TotalRefferedAccounts = 0;
                            entity.ReferralLink = Convert.ToBase64String(Encoding.ASCII.GetBytes(Email));
                            entity.CreationDate = DateTime.Now;



                            db.Entry(entity).State = EntityState.Modified;
                            db.SaveChanges();
                            message = "Successfully Uploaded Details";

                            var id = Convert.ToInt32(Session["UserName"]);
                            var entity1 = db.Accounts.Where(e => e.AccountId == id).FirstOrDefault();
                            ViewBag.Record = entity1;
                            return View();
                        }
                        else
                        {
                            message = "Some Technical Issues Here";
                            return View();

                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Inedx", "Home");
                }
                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult BuyAssets()
        {
            return View();
        }

        public string imgLink(HttpPostedFileBase file)
        {
            if (Session["UserName"] != null)
            {
                var DateeTime = DateTime.Now.ToString("yyyyMMdd_hhssms");
                var fname = file.FileName;
                var fullnamee = DateeTime + "_" + fname;
                var ext = Path.GetExtension(fname);
                var extension = ext.ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    var path = Server.MapPath("~/KYCData");
                    var fullpath = Path.Combine(path, fullnamee);
                    file.SaveAs(fullpath);
                    return fullpath;
                }
                else
                {
                    var path = Server.MapPath("~/KYCData");
                    var fullpath = Path.Combine(path, fullnamee);
                    file.SaveAs(fullpath);
                    return fullpath;
                }
            }
            else
            {
                return null;
            }
        }


    }
}