using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_Vinpearl.Models;

namespace QL_Vinpearl.Controllers
{
	public class DICHVUController : Controller
	{
		private QL_VinpearlEntities db = new QL_VinpearlEntities();

		public ActionResult TimKiem()
		{
			var DICHVU = db.DICHVU.Include(n => n.LOAIDV);
			return View(DICHVU.ToList());
		}
		[HttpPost]
		public ActionResult TimKiem(string timKiem)
		{

			var DICHVU = db.DICHVU.Include(c => c.LOAIDV).Where(a => a.tenDV.Contains(timKiem) || a.LOAIDV.tenLoai.Contains(timKiem));
			if (DICHVU.Count() == 0)
				ViewBag.TB = "Không có thông tin tìm kiếm.";
			return View(DICHVU.ToList());
		}

		// GET: DICHVU
		public ActionResult Index(int? page)
		{
			int currentPage = int.Parse(Request.QueryString["page"] ?? "1");
			Session["CurrentPage"] = currentPage;
			var dICHVU = db.DICHVU.Include(d => d.LOAIDV);
			return View(dICHVU.ToList());
		}

		// GET: DICHVU/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			DICHVU dICHVU = db.DICHVU.Find(id);
			if (dICHVU == null)
			{
				return HttpNotFound();
			}

			// Get the price information from VE table
			var ve = db.VE.Where(v => v.maDV == id).FirstOrDefault();
			ViewBag.giaTien = ve?.giaTien ?? 0; // Set default value to 0 if ve is nul

			return View(dICHVU);
		}

		// GET: DICHVU/Create
		public ActionResult Create()
		{
			ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai");
			return View();
		}

		// POST: DICHVU/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "maDV,tenDV,moTa,anh,maLoaiDV,xepLoai,sdtDV,diaChiDV")] DICHVU dICHVU)
		{
			if (ModelState.IsValid)
			{
				db.DICHVU.Add(dICHVU);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai", dICHVU.maLoaiDV);
			return View(dICHVU);
		}

		// GET: DICHVU/Edit/5
		public ActionResult Edit(string id)
		{
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

		// POST: DICHVU/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "maDV,tenDV,moTa,anh,maLoaiDV,xepLoai,sdtDV,diaChiDV")] DICHVU dICHVU)
		{
			if (ModelState.IsValid)
			{
				db.Entry(dICHVU).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.maLoaiDV = new SelectList(db.LOAIDV, "maLoaiDV", "tenLoai", dICHVU.maLoaiDV);
			return View(dICHVU);
		}

		// GET: DICHVU/Delete/5
		public ActionResult Delete(string id)
		{
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

		// POST: DICHVU/Delete/5
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
