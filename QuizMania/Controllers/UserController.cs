using QuizMania.DBContext;
using QuizMania.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuizMania.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            UserMemberShip memberShip = new UserMemberShip();
            var result = memberShip.verifyLogin(user.Username, user.Password);
            if (result)
            {
                var authTicket = new FormsAuthenticationTicket(
              1,
              user.Username,
              DateTime.Now,
              DateTime.Now.AddDays(5),
              true,
              "Admin"
               );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User data)
        {
            if (ModelState.IsValid)
            {
                UserMemberShip memberShip = new UserMemberShip();
                int status  = memberShip.RegisterUser(data);
                return RedirectToAction("Login");
            }
            return View(data);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult UserProfile()
        {
            UserMemberShip memberShip = new UserMemberShip();
            User user = memberShip.getUser(User.Identity.Name);
            return View(user);
        }
        [HttpPost]
        public ActionResult UserProfile(User u, HttpPostedFileBase ProfilePic)
        {
            UserMemberShip memberShip = new UserMemberShip();
            User user = memberShip.getUser(User.Identity.Name);
            u.ProfilePic = ProfilePic.FileName;
            //string ImageName = System.IO.Path.GetFileName(ProfilePic.FileName);
            var ext = System.IO.Path.GetExtension(ProfilePic.FileName);
            string physicalPath = Server.MapPath("~/Theme/assets/images/profile/" + User.Identity.Name  + ext);
            ProfilePic.SaveAs(physicalPath);
            string newImageName = User.Identity.Name + ext;
            user.Name = u.Name;
            user.NickName = u.NickName;
            user.ProfilePic = newImageName;
            
            int status = memberShip.UpdateUser(user);
            return View();
        }

       
    }
}