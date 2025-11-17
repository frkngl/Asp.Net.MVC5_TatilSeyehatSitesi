using Asp.Net.MVC5_TatilSeyehatSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Asp.Net.MVC5_TatilSeyehatSitesi.Controllers
{
    public class HomeController : Controller
    {
        TatilSeyehatMVC5Entities db = new TatilSeyehatMVC5Entities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Veritabanı işlemleri G/Ç Yoğun olduğu için, uygulamanızın gelecekteki performansını ve ölçeklenebilirliğini sağlamak adına async Task<ActionResult> yapısı kullanılır.
        public async Task<ActionResult> About()
        {
            var degerler = await db.TBLABOUT.FirstOrDefaultAsync();

            return View(degerler);
        }

        public ActionResult Contact()
        {
            var degerler = db.TBLCONTACT.FirstOrDefault();
            return View(degerler);
        }
    }
}