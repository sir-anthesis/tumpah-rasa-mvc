using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumpahRasa_MVC.Models
{
    public class Recipe
    {
        public tb_recipe DetailedRecipe { get; set; }
        public IEnumerable<tb_recipe> OtherRecipes { get; set; }
        public List<CommentView> Comments { get; set; }
    }

    public class CommentView
    {
        public int IdComment { get; set; }
        public int IdMember { get; set; }
        public int IdRecipe { get; set; }
        public string Comment { get; set; }
        public string MemberName { get; set; }
        public float Rating { get; set; }
        public string CreatedAt { get; set; }
    }
}