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
    public class CTHDsController : Controller
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
		// GET: Admin/CTHDs
		public ActionResult Index()
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			var cTHD = db.CTHD.Include(c => c.VE);
            return View(cTHD.ToList());
        }

        // GET: Admin/CTHDs/Details/5
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
            CTHD cTHD = db.CTHD.Find(id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            return View(cTHD);
        }

        // GET: Admin/CTHDs/Create
        public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.maVe = new SelectList(db.VE, "maVe", "maDV");
            return View();
        }

        // POST: Admin/CTHDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maHD,maVe,soLuong,giaTien")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.CTHD.Add(cTHD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maVe = new SelectList(db.VE, "maVe", "maDV", cTHD.maVe);
            return View(cTHD);
        }

		// GET: Admin/CTHDs/Edit/5
		public ActionResult Edit(string maHD, string maVe)
		{
			if (CheckPermission("CN03") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (maHD == null || maVe == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			CTHD cTHD = db.CTHD.SingleOrDefault(x => x.maHD == maHD && x.maVe == maVe);

			if (cTHD == null)
			{
				return HttpNotFound();
			}

			ViewBag.maVe = new SelectList(db.VE, "maVe", "maDV", cTHD.maVe);
			return View(cTHD);
		}

		// POST: Admin/CTHDs/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maHD,maVe,soLuong,giaTien")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTHD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maVe = new SelectList(db.VE, "maVe", "maDV", cTHD.maVe);
            return View(cTHD);
        }

		// GET: Admin/CTHDs/Delete/5
		public ActionResult Delete(string maHD, string maVe)
		{
			if (CheckPermission("CN04") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			if (maHD == null || maVe == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			CTHD cTHD = db.CTHD.SingleOrDefault(x => x.maHD == maHD && x.maVe == maVe);

			if (cTHD == null)
			{
				return HttpNotFound();
			}

			return View(cTHD);
		}

		// POST: Admin/CTHDs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string maHD, string maVe)
		{
			CTHD cTHD = db.CTHD.SingleOrDefault(x => x.maHD == maHD && x.maVe == maVe);

			if (cTHD == null)
			{
				return HttpNotFound();
			}

			db.CTHD.Remove(cTHD);
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
