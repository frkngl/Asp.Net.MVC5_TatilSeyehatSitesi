using Asp.Net.MVC5_TatilSeyehatSitesi.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Asp.Net.MVC5_TatilSeyehatSitesi.Controllers
{
    public class AdminController : Controller
    {
        TatilSeyehatMVC5Entities db = new TatilSeyehatMVC5Entities();
        TableList data = new TableList();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var degerler = db.TBLABOUT.FirstOrDefault();
            return View("About", degerler);
        }
         
        [HttpPost]
        public ActionResult AboutSave(TBLABOUT b, HttpPostedFileBase BlogImage)
        {
            var guncellenecekkayit = db.TBLABOUT.Find(b.ID);

            try
            {
                if (BlogImage != null && BlogImage.ContentLength > 0)
                {
                    if (BlogImage.ContentType == "image/jpeg" || BlogImage.ContentType == "image/png" || BlogImage.ContentType == "image/jpg" || BlogImage.ContentType == "image/jfif")
                    {
                        var yol = Request.MapPath("~/image/" + guncellenecekkayit.IMAGE);
                        if (System.IO.File.Exists(yol))
                        {
                            System.IO.File.Delete(yol);
                        }


                        var fi = new FileInfo(BlogImage.FileName);
                        var fileName = Path.GetFileName(BlogImage.FileName);
                        fileName = Guid.NewGuid().ToString() + fi.Extension;
                        var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                        WebImage rr = new WebImage(BlogImage.InputStream);

                        if (rr.Width > 1000)

                            rr.Resize(800, 800); 
                        rr.Save(path);
                        guncellenecekkayit.IMAGE = fileName;
                    }
                }
                guncellenecekkayit.DESCRIPTION = b.DESCRIPTION;
                db.SaveChanges();
                TempData["success"] = "Beceri güncelleme işlemi başarıyla gerçekleşti.";
                return RedirectToAction("About");
            }
            catch (Exception)
            {
                TempData["error"] = "Beceri güncelleme sıraasında hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("About");
            }
        }

        public ActionResult Blog(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            data.BlogsList = db.TBLBLOG.OrderByDescending(x=>x.DATE).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        [HttpGet]
        public ActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBlog(TBLBLOG add, HttpPostedFileBase BlogImage)
        {
            try
            {
                if (BlogImage.ContentType == "image/jpeg" || BlogImage.ContentType == "image/png" || BlogImage.ContentType == "image/jpg" || BlogImage.ContentType == "image/jfif")
                {
                    var fi = new FileInfo(BlogImage.FileName);
                    var fileName = Path.GetFileName(BlogImage.FileName);
                    fileName = Guid.NewGuid().ToString() + fi.Extension;
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                    WebImage rr = new WebImage(BlogImage.InputStream);

                    if (rr.Width > 1000)

                        rr.Resize(800, 800);
                    rr.Save(path);

                    TBLBLOG blog = new TBLBLOG();
                    blog.TITLE = add.TITLE;
                    blog.DATE = DateTime.Now;
                    blog.DESCRIPTOIN = add.DESCRIPTOIN;
                    blog.IMAGE = fileName;
                    blog.STATUS = true;
                    db.TBLBLOG.Add(blog);
                    db.SaveChanges();
                    TempData["success"] = "Blog başarıyla eklenmiştir.";
                    return RedirectToAction("Blog");
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Blog eklenirken bir hata oluştu. Lütfen tekrar deneyiniz.";
            }
            return RedirectToAction("Blog");
        }
    }
}