using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        // GET: TumpahRasa/Recipe/5
        public ActionResult Recipe(int id)
        {
            if (db.tb_recipe.Find(id) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // detailedRecipe
            var detailedRecipe = db.tb_recipe.Find(id);
            if (detailedRecipe == null)
            {
                return HttpNotFound();
            }

            detailedRecipe.description = HttpUtility.HtmlDecode(detailedRecipe.description);

            // otherRecipes
            var otherRecipes = db.tb_recipe.Take(4).ToList();

            // comments with member names
            var comments = from comment in db.tb_comment
                           join member in db.tb_member on comment.id_member equals member.id_member
                           where comment.id_recipe == id
                           select new CommentView
                           {
                               IdComment = comment.id_comment,
                               IdMember = comment.id_member,
                               IdRecipe = comment.id_recipe,
                               Comment = comment.comment,
                               MemberName = member.name,
                               Rating = comment.rating
                           };

            var viewModel = new Recipe
            {
                DetailedRecipe = detailedRecipe,
                OtherRecipes = otherRecipes,
                Comments = comments.ToList()
            };

            return View(viewModel);
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
                // Loop to find the innermost exception message
                var exceptionMessage = ex.Message;
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    exceptionMessage = innerEx.Message;
                    innerEx = innerEx.InnerException;
                }

                TempData["ErrorMessage"] = exceptionMessage;
                return RedirectToAction("Index");
            }
        }

        // POST: TumpahRasa/Unloving/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unloving(int recipeId)
        {
            int memberId = 1;
            // Find the recipe by id_recipe and id_member
            tb_loved lovedRecipe = db.tb_loved.FirstOrDefault(r => r.id_recipe == recipeId && r.id_member == memberId);

            if (lovedRecipe == null)
            {
                // Handle the case where the recipe doesn't exist or doesn't belong to the member
                return HttpNotFound();
            }

            db.tb_loved.Remove(lovedRecipe);
            db.SaveChanges();

            return RedirectToAction("Loved");
        }


    }
}