using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TumpahRasa_MVC.Models;

namespace TumpahRasa_MVC.Controllers
{
    public class TumpahRasaController : Controller
    {
        private db_tumpahrasaEntities db = new db_tumpahrasaEntities();

        // GET: TumpahRasa
        public ActionResult Index()
        {
            return View(db.tb_recipe.ToList());
        }

        // GET: TumpahRasa/Loved
        public ActionResult Loved()
        {
            int userId = 1;

            var lovedRecipes = from loved in db.tb_loved
                               join recipe in db.tb_recipe on loved.id_recipe equals recipe.id_recipe
                               where loved.id_member == userId
                               select recipe;

            return View(lovedRecipes.ToList());
        }

        // POST: TumpahRasa/Loving
        [HttpPost]
        public ActionResult Loving(int id, tb_loved model)
        {
            try 
            {
                model.id_member = 1;
                model.id_recipe = id;
                model.loved_at = DateTime.Now;

                db.tb_loved.Add(model);
                db.SaveChanges();
                return RedirectToAction("Loved");
            }

            catch (Exception ex) 
            {
                ViewBag.ErrorMessage = ex;
                return RedirectToAction("Index");
            }
        }

    }
}