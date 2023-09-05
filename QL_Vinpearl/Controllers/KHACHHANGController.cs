using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using QL_Vinpearl.Models;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
	public class KhachHangController : Controller
	{
		private QL_VinpearlEntities db = new QL_VinpearlEntities();

		public ActionResult Index()
		{
			return View(db.KHACHHANG.ToList());
		}

		// GET: Admin/KhachHangs/Details/5
		public ActionResult Details()
		{
			string emailKH = Session["EmailKH"] as string;

			if (emailKH == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			KHACHHANG khachHang = db.KHACHHANG.FirstOrDefault(kh => kh.email == emailKH);
			ViewBag.Test111 = khachHang.ResetPasswordCode;
			if (khachHang == null)
			{
				return HttpNotFound();
			}

			return View(khachHang);
		}

		// GET: Admin/KhachHangs/Edit/5
		public ActionResult Edit()
		{
			string emailKH = Session["EmailKH"] as string;
			if (emailKH == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			KHACHHANG khachHang = db.KHACHHANG.FirstOrDefault(kh => kh.email == emailKH);
			if (khachHang == null)
			{
				return HttpNotFound();
			}
			return View(khachHang);
		}

		// POST: Admin/KhachHangs/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "maKH,hoTenKH,SDT,diaChi,ngaySinh,gioiTinh,email,matKhau,anh")] KHACHHANG kHACHHANG)
		{
			var imgUser = Request.Files["Avatar"];
			try
			{
				string postedFileName = System.IO.Path.GetFileName(imgUser.FileName);
				var path = Server.MapPath("/Content/img/Avatar" + postedFileName);
				imgUser.SaveAs(path);
			}
			catch { }
			if (ModelState.IsValid)
			{
				db.Entry(kHACHHANG).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Details","KHACHHANG");
			}
			return View(kHACHHANG);
		}

		public ActionResult DichVuDaThanhToan()
		{

			return View();
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