using System;
using System.Collections.Generic;
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
    public class NhanViensController : Controller
    {
        private QL_VinpearlEntities db = new QL_VinpearlEntities();

        // GET: Admin/NhanViens
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
		public ActionResult Index()
        {
            if (CheckPermission("CN01") == false)
            {
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
            var nHANVIEN = db.NHANVIEN.Include(n => n.LOAINV);
			return View(nHANVIEN.ToList());
        }

        // GET: Admin/NhanViens/Details/5
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
            NHANVIEN nHANVIEN = db.NHANVIEN.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHANVIEN);
        }
		string LayMaNV()
		{
			var maMax = db.NHANVIEN.ToList().Select(n => n.maNV).Max();
			int maNV = int.Parse(maMax.Substring(2)) + 1;
			string NV = maNV.ToString().PadLeft(3, '0');
			return "NV" + NV;
		}
		// GET: Admin/NhanViens/Create
		public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.MaNV = LayMaNV();
			ViewBag.maLoaiNV = new SelectList(db.LOAINV, "maLoaiNV", "tenLoai");
            return View();
        }

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maNV,maLoaiNV,hoTenNV,diaChi,ngaySinh,sdt,gioiTinh,anh,email,matKhau,soCa")] NHANVIEN nHANVIEN)
        {
			var imgUser = Request.Files["Avatar"];
			string postedFileName = System.IO.Path.GetFileName(imgUser.FileName);
			var path = Server.MapPath("/Content/img/Avatar" + postedFileName);
			imgUser.SaveAs(path);
			if (ModelState.IsValid)
            {
				nHANVIEN.maNV = LayMaNV();
				nHANVIEN.anh = postedFileName;
				db.NHANVIEN.Add(nHANVIEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maLoaiNV = new SelectList(db.LOAINV, "maLoaiNV", "tenLoai", nHANVIEN.maLoaiNV);
            return View(nHANVIEN);
        }

        // GET: Admin/NhanViens/Edit/5
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
            NHANVIEN nHANVIEN = db.NHANVIEN.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            ViewBag.maLoaiNV = new SelectList(db.LOAINV, "maLoaiNV", "tenLoai", nHANVIEN.maLoaiNV);
            return View(nHANVIEN);
        }

        // POST: Admin/NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maNV,maLoaiNV,hoTenNV,diaChi,ngaySinh,sdt,gioiTinh,anh,email,matKhau,soCa")] NHANVIEN nHANVIEN)
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
                db.Entry(nHANVIEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maLoaiNV = new SelectList(db.LOAINV, "maLoaiNV", "tenLoai", nHANVIEN.maLoaiNV);
            return View(nHANVIEN);
        }

        // GET: Admin/NhanViens/Delete/5
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
            NHANVIEN nHANVIEN = db.NHANVIEN.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHANVIEN);
        }

        // POST: Admin/NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NHANVIEN nHANVIEN = db.NHANVIEN.Find(id);
            db.NHANVIEN.Remove(nHANVIEN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
		public ActionResult ExportToExcel()
		{
			var listNhanVien = db.NHANVIEN.ToList();
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("NhanViens");

				worksheet.Cells[1, 1].Value = "Mã NV";
				worksheet.Cells[1, 2].Value = "Mã Loại NV";
				worksheet.Cells[1, 3].Value = "Tên NV";
				worksheet.Cells[1, 4].Value = "Địa Chỉ";
				worksheet.Cells[1, 5].Value = "Ngày Sinh";
				worksheet.Cells[1, 6].Value = "Số Điện Thoại";
				worksheet.Cells[1, 7].Value = "Giới Tính";
				worksheet.Cells[1, 8].Value = "Email";
				worksheet.Cells[1, 9].Value = "Ảnh";

				int row = 2;
				foreach (var nv in listNhanVien)
				{
					worksheet.Cells[row, 1].Value = nv.maNV;
					worksheet.Cells[row, 2].Value = nv.maLoaiNV;
					worksheet.Cells[row, 3].Value = nv.hoTenNV;
					worksheet.Cells[row, 4].Value = nv.diaChi;
					worksheet.Cells[row, 5].Value = nv.ngaySinh;
					worksheet.Cells[row, 6].Value = nv.sdt;
					worksheet.Cells[row, 7].Value = nv.gioiTinh;
					worksheet.Cells[row, 8].Value = nv.email;
					worksheet.Cells[row, 9].Value = nv.anh;
					row++;
				}

				// Lưu package thành file Excel
				var stream = new MemoryStream(package.GetAsByteArray());
				var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				var fileName = "NhanViens.xlsx";

				// Trả về file Excel để tải xuống
				return File(stream, contentType, fileName);
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
