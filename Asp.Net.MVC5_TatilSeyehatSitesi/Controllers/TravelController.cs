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
    public class TravelController : Controller
    {
        TatilSeyehatMVC5Entities db = new TatilSeyehatMVC5Entities();
        TableList data = new TableList();
        // GET: Travel
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            data.TravelList = db.TBLTRAVELS.Where(x => x.STATUS == true).OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            data.TravelComment = db.TBLTRAVELCOMMENTS.Where(x => x.STATUS == true).ToList();
            return View(data);
        }

        public ActionResult TravelDetail(int id)
        {
            data.Travel = db.TBLTRAVELS.Where(x => x.ID == id).ToList();
            return View(data);
        }

        public PartialViewResult TravelRightSideBar()
        {
            data.Travel = db.TBLTRAVELS.Where(x => x.STATUS == true).ToList();
            data.TravelComment = db.TBLTRAVELCOMMENTS.Where(x => x.STATUS == true).ToList();
            return PartialView(data);
        }

        public PartialViewResult Comments(int id)
        {
            data.TravelComment = db.TBLTRAVELCOMMENTS.Where(x => x.TRAVELSID == id && x.STATUS == true).ToList();
            return PartialView(data);
        }
    }
}