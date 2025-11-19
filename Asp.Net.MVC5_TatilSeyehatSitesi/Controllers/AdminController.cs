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




        //-------------------------ABOUT PAGE-------------------------//
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
        //-------------------------ABOUT PAGE-------------------------//













        //-------------------------BLOG PAGE-------------------------//
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


        public ActionResult UpdateBlog(int id)
        {
            var degerler = db.TBLBLOG.Find(id); 
            return View("UpdateBlog", degerler);
        }

        [HttpPost]
        public ActionResult UpdateBlogSave(TBLBLOG UpdateBlog, HttpPostedFileBase BlogImage)
        {
            var update = db.TBLBLOG.Find(UpdateBlog.ID);
            try
            {
                if (BlogImage != null && BlogImage.ContentLength > 0)
                {
                    if (BlogImage.ContentType == "image/jpeg" || BlogImage.ContentType == "image/png" || BlogImage.ContentType == "image/jpg" || BlogImage.ContentType == "image/jfif")
                    {
                        var lane = Request.MapPath("~/image/" + update.IMAGE);
                        if (System.IO.File.Exists(lane))
                        {
                            System.IO.File.Delete(lane);
                        }

                        var fi = new FileInfo(BlogImage.FileName);
                        var fileName = Path.GetFileName(BlogImage.FileName);
                        fileName = Guid.NewGuid().ToString() + fi.Extension;
                        var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                        WebImage rr = new WebImage(BlogImage.InputStream);

                        if (rr.Width > 1000)

                            rr.Resize(800, 800);
                        rr.Save(path);
                        update.IMAGE = fileName;
                    }
                    else
                    {
                        TempData["error3"] = "Lütfem resim formatında bir dosya seçiniz!!!";
                        return RedirectToAction("Blog");
                    }
                }
                update.TITLE = UpdateBlog.TITLE;
                update.DESCRIPTOIN = UpdateBlog.DESCRIPTOIN;
                db.SaveChanges();
                TempData["success2"] = "Blog başarıyla güncellenmiştir.";
                return RedirectToAction("Blog");
            }
            catch (Exception)
            {
                TempData["error2"] = "Blog güncellenirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Blog");
            }
        }

        public ActionResult DeleteBlog(int id)
        {
            var DeleteBlog = db.TBLBLOG.Find(id);
            try
            {
                var lane = Request.MapPath("~/image/" + DeleteBlog.IMAGE);
                if (System.IO.File.Exists(lane))
                {
                    System.IO.File.Delete(lane);
                }
                db.TBLBLOG.Remove(DeleteBlog);
                db.SaveChanges();
                TempData["success3"] = "Blog başarıyla silinmiştir.";
                return RedirectToAction("Blog");
            }
            catch (Exception)
            {
                TempData["error4"] = "Blog silinirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Blog");
            }
        }

        public ActionResult BlogStatus(int id)
        {
            try
            {
                var blog = db.TBLBLOG.Find(id);
                bool current = blog.STATUS.GetValueOrDefault(false);
                blog.STATUS = !current;
                db.SaveChanges();
                TempData["success4"] = blog.STATUS == true ? "Blog aktif hale getirildi." : "Blog pasif (yayından kaldırıldı).";
                return RedirectToAction("Blog");
            }
            catch (Exception)
            {
                TempData["error5"] = "Durum güncellenirken bir hata oluştu.";
                return RedirectToAction("Blog");
            }
        }
        //-------------------------BLOG PAGE-------------------------//










        //-------------------------BLOGCOMMENT PAGE-------------------------//
        public ActionResult BlogComment(int? page)
        {
            int pageSize =  10;
            int pageNumber = (page ?? 1);
            data.BlogCommentList = db.TBLBLOGCOMMENTS.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        public ActionResult DeleteBlogComment(int id)
        {
            var DeleteBlogComment = db.TBLBLOGCOMMENTS.Find(id);
            try
            {
                db.TBLBLOGCOMMENTS.Remove(DeleteBlogComment);
                db.SaveChanges();
                TempData["success"] = "Blog Yorumu başarıyla silinmiştir.";
                return RedirectToAction("BlogComment");
            }
            catch (Exception)
            {
                TempData["error"] = "Blog Yorumu silinirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("BlogComment");
            }
        }

        public ActionResult BlogCommentStatus(int id)
        {
            try
            {
                var BlogComment = db.TBLBLOGCOMMENTS.Find(id);
                bool current = BlogComment.STATUS.GetValueOrDefault(false);
                BlogComment.STATUS = !current;
                db.SaveChanges();
                TempData["success2"] = BlogComment.STATUS == true ? "Blog Yorumu aktif hale getirildi." : "Blog Yorumu pasif (yayından kaldırıldı).";
                return RedirectToAction("BlogComment");
            }
            catch (Exception)
            {
                TempData["error2"] = "Blog Yorum Durumu güncellenirken bir hata oluştu.";
                return RedirectToAction("BlogComment");
            }
        }
    }
}