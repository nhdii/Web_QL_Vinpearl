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
    public class SoCasController : Controller
    {
        private QL_VinpearlEntities db = new QL_VinpearlEntities();

		// Kiểm tra quyền của nhân viên
		public bool CheckPermission(string maChucNang)
		{
			if (Session["maLNV"] == null) Response.Redirect("~/Admin/Login/Index");
			var userSession = Session["maLNV"].ToString();
			var count = db.PHANQUYEN.Count(m => m.maLoaiNV == userSession && m.maChucNang == maChucNang);
			if (count == 0)
			{
				return false;
			}
			return true;
		}
		// GET: Admin/SoCas
		public ActionResult Index()
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			var sOCA = db.SOCA.Include(s => s.NHANVIEN);
            return View(sOCA.ToList());
        }

        // GET: Admin/SoCas/Details/5
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
            SOCA sOCA = db.SOCA.Find(id);
            if (sOCA == null)
            {
                return HttpNotFound();
            }
            return View(sOCA);
        }

        // GET: Admin/SoCas/Create
        public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV");
            return View();
        }

        // POST: Admin/SoCas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maCa,maNV,soCa1")] SOCA sOCA)
        {
            if (ModelState.IsValid)
            {
                db.SOCA.Add(sOCA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", sOCA.maNV);
            return View(sOCA);
        }

        // GET: Admin/SoCas/Edit/5
        public ActionResult Edit(string maCa, string maNV)
        {
			if (CheckPermission("CN03") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (maCa == null || maNV == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			SOCA sOCA = db.SOCA.SingleOrDefault(x => x.maCa == maCa && x.maNV == maNV);
			if (sOCA == null)
            {
                return HttpNotFound();
            }
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", sOCA.maNV);
            return View(sOCA);
        }

        // POST: Admin/SoCas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maCa,maNV,soCa1")] SOCA sOCA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sOCA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", sOCA.maNV);
            return View(sOCA);
        }

        // GET: Admin/SoCas/Delete/5
        public ActionResult Delete(string maCa, string maNV)
        {
			if (CheckPermission("CN04") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (maCa == null || maNV == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			SOCA sOCA = db.SOCA.SingleOrDefault(x => x.maCa == maCa && x.maNV == maNV);
			if (sOCA == null)
            {
                return HttpNotFound();
            }
            return View(sOCA);
        }

        // POST: Admin/SoCas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string maCa, string maNV)
        {
			SOCA sOCA = db.SOCA.SingleOrDefault(x => x.maCa == maCa && x.maNV == maNV);
			if (sOCA == null)
			{
				return HttpNotFound();
			}
			db.SOCA.Remove(sOCA);
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
