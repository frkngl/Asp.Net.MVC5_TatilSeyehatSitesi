using Asp.Net.MVC5_TatilSeyehatSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Asp.Net.MVC5_TatilSeyehatSitesi.Controllers
{
    public class LoginController : Controller
    {
        TatilSeyehatMVC5Entities db = new TatilSeyehatMVC5Entities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TBLADMIN ad)
        {
            var varmi = db.TBLADMIN.Where(x => x.USERNAME == ad.USERNAME & x.PASSWORD == ad.PASSWORD).FirstOrDefault();

            if (varmi != null)
            {
                FormsAuthentication.SetAuthCookie(varmi.USERNAME, false);
                Session["USERNAME"] = varmi.USERNAME;
                Session["ID"] = varmi.ID;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["error"] = "Kullanıcı adı veya şifreniz hatalı girdiniz. Lütfen tekrar deneyiniz.";
                return View();
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return Redirect("~/");
        }
    }
}