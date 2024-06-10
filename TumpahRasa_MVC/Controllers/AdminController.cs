using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TumpahRasa_MVC.Models;
using System.IO;

namespace TumpahRasa_MVC.Controllers
{
    public class AdminController : Controller
    {
        private db_tumpahrasaEntities db = new db_tumpahrasaEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.tb_recipe.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(tb_recipe model, HttpPostedFileBase thumbnail)
        {
            if (ModelState.IsValid)
            {
                if (thumbnail != null && thumbnail.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(thumbnail.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content"), fileName);
                    thumbnail.SaveAs(path);

                    // Save the file path to the model
                    model.thumbnail = fileName; // Save the relative path
                }

                db.tb_recipe.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
