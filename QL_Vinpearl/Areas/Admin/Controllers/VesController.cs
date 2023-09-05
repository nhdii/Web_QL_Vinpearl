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
    public class VesController : Controller
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
		// GET: Admin/Ves
		public ActionResult Index()
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			var vE = db.VE.Include(v => v.DICHVU);
            return View(vE.ToList());
        }

        // GET: Admin/Ves/Details/5
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
            VE vE = db.VE.Find(id);
            if (vE == null)
            {
                return HttpNotFound();
            }
            return View(vE);
        }

		// GET: Admin/Ves/Create
		string LayMaVe()
		{
			var maMax = db.VE.ToList().Select(n => n.maVe).Max();
			int maVe = int.Parse(maMax.Substring(2)) + 1;
			string Ve = maVe.ToString().PadLeft(6, '0');
			return "VE" + Ve;
		}
		public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.MaVe = LayMaVe();
			ViewBag.maDV = new SelectList(db.DICHVU, "maDV", "tenDV");
            return View();
        }

        // POST: Admin/Ves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maVe,maDV,loaiVe,giaTien")] VE vE)
        {

            if (ModelState.IsValid)
            {
                vE.maVe = LayMaVe();
                db.VE.Add(vE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maDV = new SelectList(db.DICHVU, "maDV", "tenDV", vE.maDV);
            return View(vE);
        }

        // GET: Admin/Ves/Edit/5
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
            VE vE = db.VE.Find(id);
            if (vE == null)
            {
                return HttpNotFound();
            }
            ViewBag.maDV = new SelectList(db.DICHVU, "maDV", "tenDV", vE.maDV);
            return View(vE);
        }

        // POST: Admin/Ves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maVe,maDV,loaiVe,giaTien")] VE vE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maDV = new SelectList(db.DICHVU, "maDV", "tenDV", vE.maDV);
            return View(vE);
        }

        // GET: Admin/Ves/Delete/5
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
            VE vE = db.VE.Find(id);
            if (vE == null)
            {
                return HttpNotFound();
            }
            return View(vE);
        }

        // POST: Admin/Ves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            VE vE = db.VE.Find(id);
            db.VE.Remove(vE);
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
