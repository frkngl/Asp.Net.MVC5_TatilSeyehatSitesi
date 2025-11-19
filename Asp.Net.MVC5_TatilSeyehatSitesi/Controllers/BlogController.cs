using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Net.MVC5_TatilSeyehatSitesi.Models;
using PagedList; 
using PagedList.Mvc;

namespace Asp.Net.MVC5_TatilSeyehatSitesi.Controllers
{
    public class BlogController : Controller 
    {
        TatilSeyehatMVC5Entities db = new TatilSeyehatMVC5Entities();
        TableList data = new TableList();
        // GET: Blog
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            data.BlogsList = db.TBLBLOG.Where(x=>x.STATUS == true).OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            data.BlogComment = db.TBLBLOGCOMMENTS.Where(x => x.STATUS == true).ToList();
            return View(data);
        }

        public ActionResult BlogDetail(int id)
        { 
            data.Blog = db.TBLBLOG.Where(x=>x.ID == id).ToList();
            return View(data);
        }

        public PartialViewResult BlogRightSideBar()
        {
            data.Blog = db.TBLBLOG.Where(x => x.STATUS == true).ToList();
            data.BlogComment = db.TBLBLOGCOMMENTS.Where(x => x.STATUS == true).ToList();
            return PartialView(data);
        }

        public PartialViewResult Comments(int id)
        {
            data.BlogComment = db.TBLBLOGCOMMENTS.Where(x => x.BLOGID == id && x.STATUS == true).ToList();
            return PartialView(data);
        }


        [HttpGet]
        public PartialViewResult Comment(int id)
        {
            ViewBag.BlogID = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult Comment(TBLBLOGCOMMENTS AddCdomment)
        {
            try
            {
                AddCdomment.DATE = DateTime.Now;
                AddCdomment.STATUS = false;
                db.TBLBLOGCOMMENTS.Add(AddCdomment);
                db.SaveChanges();
                TempData["success"] = "Yorumunuz Admin tarafından kontrol edilip ona göre yayına alınacaktır.";
                return PartialView();
            }
            catch (Exception)
            {
                TempData["error"] = "Yorumunuz eklenirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return PartialView();
            }
            
        }
    }
}