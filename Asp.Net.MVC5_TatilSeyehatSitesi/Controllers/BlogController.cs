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
            data.Blogs = db.TBLBLOG.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            data.BlogComment = db.TBLBLOGCOMMENTS.ToList();
            return View(data);
        }
    }
}