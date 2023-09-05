using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_Vinpearl.Models;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
    public class DichVusController : Controller
    {
        private QL_VinpearlEntities db = new QL_VinpearlEntities();

		public bool CheckPermission(string maChucNang)
		{
			if (Session["maLNV"] == null) 
                Response.Redirect("~/Admin/Login/Index");
			var userSession = Session["maLNV"].ToString();
			var count = db.PHANQUYEN.Count(m => m.maLoaiNV == userSession && m.maChucNang == maChucNang);
			if (count == 0)
			{
				return false;
			}
			return true;
		}
		// GET: Admin/DichVus
		public ActionResult Index()
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			var dICHVU = db.DICHVU.Include(d => d.LOAIDV);
            return View(dICHVU.ToList());
        }

        // GET: Admin/DichVus/Details/5
        public ActionResult Details(string id)
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU dICHVU = db.DICHVU.Find(id);
            if (dICHVU == null)
            {
                return HttpNotFound();
            }
            return View(dICHVU);
        }

		string LayMaDV()
		{
			// Lấy mã dịch vụ lớn nhất từ cơ sở dữ liệu
			var maMax = db.DICHVU.ToList().Select(n => n.maDV).Max();

			// Tách số từ mã dịch vụ lớn nhất và tăng giá trị lên 1
			int maDV = int.Parse(maMax.Substring(2)) + 1;

			// Định dạng lại số để tạo mã dịch vụ mới
			string DV = maDV.ToString().PadLeft(3, '0');

			// Kết hợp mã dịch vụ mới với tiền tố "DV" để tạo mã dịch vụ hoàn chỉnh
			return "DV" + DV;
		}
		// GET: Admin/DichVus/Create
		public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.MaDV = LayMaDV();
			ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai");
            return View();
        }

        // POST: Admin/DichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maDV,tenDV,moTa,anh,maLoaiDV,xepLoai,sdtDV,diaChiDV")] DICHVU dICHVU)
        {
			var imgService = Request.Files["ServicePicture"];
			string postedFileName = System.IO.Path.GetFileName(imgService.FileName);
			var path = Server.MapPath("/Content/img/ServicePicture" + postedFileName);
			imgService.SaveAs(path);
			if (ModelState.IsValid)
            {
				dICHVU.maDV = LayMaDV();
				dICHVU.anh = postedFileName;
				db.DICHVU.Add(dICHVU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai", dICHVU.maLoaiDV);
            return View(dICHVU);
        }

        // GET: Admin/DichVus/Edit/5
        public ActionResult Edit(string id)
        {
			if (CheckPermission("CN03") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU dICHVU = db.DICHVU.Find(id);
            if (dICHVU == null)
            {
                return HttpNotFound();
            }
            ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai", dICHVU.maLoaiDV);
            return View(dICHVU);
        }

        // POST: Admin/DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maDV,tenDV,moTa,anh,maLoaiDV,xepLoai,sdtDV,diaChiDV")] DICHVU dICHVU)
        {
			var imgService = Request.Files["ServicePicture"];
			try
			{
				string postedFileName = System.IO.Path.GetFileName(imgService.FileName);
				var path = Server.MapPath("/Content/img/ServicePicture" + postedFileName);
				imgService.SaveAs(path);
			}
			catch { }
			if (ModelState.IsValid)
            {
                db.Entry(dICHVU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai", dICHVU.maLoaiDV);
            return View(dICHVU);
        }

        // GET: Admin/DichVus/Delete/5
        public ActionResult Delete(string id)
        {
			if (CheckPermission("CN04") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU dICHVU = db.DICHVU.Find(id);
            if (dICHVU == null)
            {
                return HttpNotFound();
            }
            return View(dICHVU);
        }

        // POST: Admin/DichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DICHVU dICHVU = db.DICHVU.Find(id);
            db.DICHVU.Remove(dICHVU);
            db.SaveChanges();
            return RedirectToAction("Index");
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
