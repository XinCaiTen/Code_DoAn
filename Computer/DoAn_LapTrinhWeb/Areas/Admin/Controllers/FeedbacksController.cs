using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly DbContext db = new DbContext();
        //View list đánh giá
        public ActionResult FeedbackIndex(int?size,int?page,string search,string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.countTrash = db.Feedbacks.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                var list = from fb in db.Feedbacks
                           join p in db.Products on fb.product_id equals p.product_id
                           join a in db.Accounts on fb.account_id equals a.account_id
                           join fbimg in db.Feedback_Image on fb.feedback_id equals fbimg.feedback_id
                           where fb.status == "1" || fb.status == "2"
                           orderby fb.feedback_id descending
                           select new FeedbackDTOs
                           {
                               product_name=p.product_name,

                               feedback_id = fb.feedback_id,
                               genre_id = p.genre_id,
                               discount_id = p.disscount_id,
                               description = fb.description,
                               rating_star = fb.rate_star,
                               status = fb.status,
                               create_at = fb.create_at,
                               Image = fbimg.image,
                               User_Email = a.Email,
                               account_id = a.account_id,
                           };
                      return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View list trash đánh giá
        public ActionResult FeedbackTrash(int? size, int? page, string search, string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                var list = from fb in db.Feedbacks
                           join p in db.Products on fb.product_id equals p.product_id
                           join a in db.Accounts on fb.account_id equals a.account_id
                           join fbimg in db.Feedback_Image on fb.feedback_id equals fbimg.feedback_id
                           where fb.status == "0" 
                           orderby fb.feedback_id descending
                           select new FeedbackDTOs
                           {
                               product_name = p.product_name,
                               feedback_id = fb.feedback_id,
                               genre_id = p.genre_id,
                               discount_id = p.disscount_id,
                               description = fb.description,
                               rating_star = fb.rate_star,
                               status = fb.status,
                               create_at = fb.create_at,
                               Image = fbimg.image,
                               User_Email = a.Email,
                               update_at=fb.update_at,
                               account_id = a.account_id,
                           };
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận duyệt đánh giá
        public ActionResult ChangeComplete(int? id)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var feedback = db.Feedbacks.SingleOrDefault(fb=>fb.feedback_id == id);
                if (feedback != null)
                {
                    feedback.status = "2";
                    feedback.update_at = DateTime.Now;
                    feedback.update_by = User.Identity.GetEmail();
                    db.Entry(feedback).State = EntityState.Modified;
                }
                db.SaveChanges();
                Notification.set_flash("Đã xét duyệt đánh giá: "  + feedback.feedback_id+"", "success");
                return RedirectToAction("FeedbackIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xem chi tiết đánh giá

        public ActionResult FeedbackDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }
        //Hủy đánh giá
        public ActionResult Disable(int? id, string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Disable", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var feedback = db.Feedbacks.SingleOrDefault(a => a.feedback_id == id);
                if (feedback == null)
                {
                    Notification.set_flash("Không tồn tại thể loại ID: " + feedback.feedback_id + "", "warning");
                    return RedirectToAction("GenreTrash");
                }
                return View(feedback);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận hủy đánh giá
        // POST: Areas/Banners/Delete/5
        [HttpPost]
        [ActionName("Disable")]
        [ValidateAntiForgeryToken]
        public ActionResult DelTrash(int? id, string returnUrl)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var feedback = db.Feedbacks.SingleOrDefault(a => a.feedback_id == id);
                if (feedback == null || id == null)
                {
                    Notification.set_flash("Không tồn tại ID: " + feedback.feedback_id + "", "warning");
                    return RedirectToAction("FeedbackIndex");
                }
                feedback.status = "0";
                feedback.update_at = DateTime.Now;
                feedback.update_by = User.Identity.GetEmail();
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã chuyển đánh giá ID: " + feedback.feedback_id + " vào thùng rác!", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                return RedirectToAction("FeedbackIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
