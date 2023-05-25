using QuizMania.DBContext;
using QuizMania.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizMania.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserMemberShip memberShip = new UserMemberShip();
            User user = memberShip.getUser(User.Identity.Name);
            TempData["ProfilePic"] = user.ProfilePic;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}