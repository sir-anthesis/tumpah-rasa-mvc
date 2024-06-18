using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TumpahRasa_MVC.Models;

namespace TumpahRasa_MVC.Controllers
{
    public class AuthController : Controller
    {
        private db_tumpahrasaEntities db = new db_tumpahrasaEntities();
        // GET: Auth
        public ActionResult Index()
        {
            if (Session["role"] != null && Session["role"].ToString() == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (Session["role"] != null && Session["role"].ToString() == "member")
            {
                return RedirectToAction("Index", "TumpahRasa");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tb_member objUser)
        {
            if (ModelState.IsValid)
            {
                // Check in tb_member table first
                var member = db.tb_member
                               .FirstOrDefault(a => a.email.Equals(objUser.email) && a.password.Equals(objUser.password));

                if (member != null)
                {
                    Session["memberName"] = member.name;
                    Session["memberId"] = member.id_member;
                    Session["role"] = "member";
                    return RedirectToAction("Index", "TumpahRasa");
                }
                else
                {
                    // If not found in tb_member, check in tb_admin table
                    var admin = db.tb_admin
                                  .FirstOrDefault(a => a.email.Equals(objUser.email) && a.password.Equals(objUser.password));

                    if (admin != null)
                    {
                        Session["adminName"] = admin.name;
                        Session["adminId"] = admin.id_admin;
                        Session["role"] = "admin";
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.Message = "Login Failed";
                    }
                }
            }

            return View(objUser);
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(tb_member model)
        {
            if (ModelState.IsValid)
            {
                db.tb_member.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //Logout
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Auth");
        }

    }
}