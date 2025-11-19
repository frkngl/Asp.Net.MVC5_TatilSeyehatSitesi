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
        public ActionResult AboutSave(TBLABOUT update, HttpPostedFileBase BlogImage)
        {
            var UpdateAbout = db.TBLABOUT.Find(update.ID);

            try
            {
                if (BlogImage != null && BlogImage.ContentLength > 0)
                {
                    if (BlogImage.ContentType == "image/jpeg" || BlogImage.ContentType == "image/png" || BlogImage.ContentType == "image/jpg" || BlogImage.ContentType == "image/jfif")
                    {
                        var yol = Request.MapPath("~/image/" + UpdateAbout.IMAGE);
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
                        UpdateAbout.IMAGE = fileName;
                    }
                }
                UpdateAbout.DESCRIPTION = update.DESCRIPTION;
                db.SaveChanges();
                TempData["success"] = "Hakkımızda güncelleme işlemi başarıyla gerçekleşti.";
                return RedirectToAction("About");
            }
            catch (Exception)
            {
                TempData["error"] = "Hakkımızda güncelleme sıraasında hata oluştu. Lütfen tekrar deneyiniz.";
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
                    blog.STATUS = false;
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
        //-------------------------BLOGCOMMENT PAGE-------------------------//









        //-------------------------TRAVEL PAGE-------------------------//
        public ActionResult Travel(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            data.TravelList = db.TBLTRAVELS.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        [HttpGet]
        public ActionResult AddTravel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTravel(TBLTRAVELS add, HttpPostedFileBase TravelImage)
        {
            try
            {
                if (TravelImage.ContentType == "image/jpeg" || TravelImage.ContentType == "image/png" || TravelImage.ContentType == "image/jpg" || TravelImage.ContentType == "image/jfif")
                {
                    var fi = new FileInfo(TravelImage.FileName);
                    var fileName = Path.GetFileName(TravelImage.FileName);
                    fileName = Guid.NewGuid().ToString() + fi.Extension;
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                    WebImage rr = new WebImage(TravelImage.InputStream);

                    if (rr.Width > 1000)

                        rr.Resize(800, 800);
                    rr.Save(path);

                    TBLTRAVELS travel = new TBLTRAVELS();
                    travel.TITLE = add.TITLE;
                    travel.DATE = DateTime.Now;
                    travel.DESCRIPTION = add.DESCRIPTION;
                    travel.IMAGE = fileName;
                    travel.STATUS = false;
                    db.TBLTRAVELS.Add(travel);
                    db.SaveChanges();
                    TempData["success"] = "Seyahat başarıyla eklenmiştir.";
                    return RedirectToAction("Travel");
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Seyahat eklenirken bir hata oluştu. Lütfen tekrar deneyiniz.";
            }
            return RedirectToAction("Travel");
        }


        public ActionResult UpdateTravel(int id)
        {
            var degerler = db.TBLTRAVELS.Find(id);
            return View("UpdateTravel", degerler);
        }

        [HttpPost]
        public ActionResult UpdateTravelSave(TBLTRAVELS UpdateTravel, HttpPostedFileBase TravelImage)
        {
            var update = db.TBLTRAVELS.Find(UpdateTravel.ID);
            try
            {
                if (TravelImage != null && TravelImage.ContentLength > 0)
                {
                    if (TravelImage.ContentType == "image/jpeg" || TravelImage.ContentType == "image/png" || TravelImage.ContentType == "image/jpg" || TravelImage.ContentType == "image/jfif")
                    {
                        var lane = Request.MapPath("~/image/" + update.IMAGE);
                        if (System.IO.File.Exists(lane))
                        {
                            System.IO.File.Delete(lane);
                        }

                        var fi = new FileInfo(TravelImage.FileName);
                        var fileName = Path.GetFileName(TravelImage.FileName);
                        fileName = Guid.NewGuid().ToString() + fi.Extension;
                        var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                        WebImage rr = new WebImage(TravelImage.InputStream);

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
                update.TITLE = UpdateTravel.TITLE;
                update.DESCRIPTION = UpdateTravel.DESCRIPTION;
                db.SaveChanges();
                TempData["success2"] = "Seyahat başarıyla güncellenmiştir.";
                return RedirectToAction("Travel");
            }
            catch (Exception)
            {
                TempData["error2"] = "Seyahat güncellenirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Travel");
            }
        }

        public ActionResult DeleteTravel(int id)
        {
            var DeleteTravel = db.TBLTRAVELS.Find(id);
            try
            {
                var lane = Request.MapPath("~/image/" + DeleteTravel.IMAGE);
                if (System.IO.File.Exists(lane))
                {
                    System.IO.File.Delete(lane);
                }
                db.TBLTRAVELS.Remove(DeleteTravel);
                db.SaveChanges();
                TempData["success3"] = "Seyahat başarıyla silinmiştir.";
                return RedirectToAction("Travel");
            }
            catch (Exception)
            {
                TempData["error4"] = "Seyahat silinirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Travel");
            }
        }

        public ActionResult TravelStatus(int id)
        {
            try
            {
                var travel = db.TBLTRAVELS.Find(id);
                bool current = travel.STATUS.GetValueOrDefault(false);
                travel.STATUS = !current;
                db.SaveChanges();
                TempData["success4"] = travel.STATUS == true ? "Seyahat aktif hale getirildi." : "Seyahat pasif (yayından kaldırıldı).";
                return RedirectToAction("Travel");
            }
            catch (Exception)
            {
                TempData["error5"] = "Durum güncellenirken bir hata oluştu.";
                return RedirectToAction("Travel");
            }
        }
        //-------------------------TRAVEL PAGE-------------------------//








        //-------------------------TRAVELCOMMENT PAGE-------------------------//
        public ActionResult TravelComment(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            data.TravelCommentList = db.TBLTRAVELCOMMENTS.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        public ActionResult DeleteTravelComment(int id)
        {
            var DeleteTravelComment = db.TBLTRAVELCOMMENTS.Find(id);
            try
            {
                db.TBLTRAVELCOMMENTS.Remove(DeleteTravelComment);
                db.SaveChanges();
                TempData["success"] = "Seyahat Yorumu başarıyla silinmiştir.";
                return RedirectToAction("TravelComment");
            }
            catch (Exception)
            {
                TempData["error"] = "Seyahat Yorumu silinirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("TravelComment");
            }
        }

        public ActionResult TravelCommentStatus(int id)
        {
            try
            {
                var TravelComment = db.TBLTRAVELCOMMENTS.Find(id);
                bool current = TravelComment.STATUS.GetValueOrDefault(false);
                TravelComment.STATUS = !current;
                db.SaveChanges();
                TempData["success2"] = TravelComment.STATUS == true ? "Seyahat Yorumu aktif hale getirildi." : "Seyahat Yorumu pasif (yayından kaldırıldı).";
                return RedirectToAction("TravelComment");
            }
            catch (Exception)
            {
                TempData["error2"] = "Seyahat Yorum Durumu güncellenirken bir hata oluştu.";
                return RedirectToAction("TravelComment");
            }
        }
        //-------------------------TRAVELCOMMENT PAGE-------------------------//






        //-------------------------CONTACT PAGE-------------------------//
        public ActionResult Contact()
        {
            var degerler = db.TBLCONTACT.FirstOrDefault();
            return View("Contact", degerler);
        }

        [HttpPost]
        public ActionResult ContactSave(TBLCONTACT update)
        {
            var UpdateContact = db.TBLCONTACT.Find(update.ID);

            try
            {
                UpdateContact.DESCRIPTION = update.DESCRIPTION;
                UpdateContact.LOCATION = update.LOCATION;
                UpdateContact.PHONE = update.PHONE;
                UpdateContact.PHONE2 = update.PHONE2;
                UpdateContact.FAX = update.FAX;
                UpdateContact.FAX2 = update.FAX2;
                UpdateContact.EMAIL = update.EMAIL;
                UpdateContact.EMAIL2 = update.EMAIL2;
                UpdateContact.ADDRESS = update.ADDRESS;
                db.SaveChanges();
                TempData["success"] = "İletişim güncelleme işlemi başarıyla gerçekleşti.";
                return RedirectToAction("Contact");
            }
            catch (Exception)
            {
                TempData["error"] = "İletişim güncelleme sıraasında hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Contact");
            }
        }
        //-------------------------CONTACT PAGE-------------------------//
    }
}