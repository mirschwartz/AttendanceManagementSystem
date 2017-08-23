using Attendance_Management_System.Helpers;
using Attendance_Management_System.Services;
using Attendance_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Attendance_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService(ConnectionString.DB);
        }

        // GET: User
        public ActionResult Signin()
        {
            SigninVM vm = new SigninVM();
            if (TempData["Message"] != null)
            {
                vm.Message = (string)TempData["Message"];
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Signin(string userName, string password, string verify)
        {
            var user = _userService.Login(userName, password);

            if (user == null || verify != "whatever")
            {
                TempData["Message"] = "Username or password is incorrect!";
                return Redirect("/user/signin");
            }
            else if(user.SubDomain != null && user.SubDomain != "")
            {
                FormsAuthentication.SetAuthCookie(userName, true);
                return Redirect($"http://{user.SubDomain}.{Request.Url.Host}{(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)}");
            }
            FormsAuthentication.SetAuthCookie(userName, true);
            return Redirect("/");
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/user/signin");
        }
    }
}