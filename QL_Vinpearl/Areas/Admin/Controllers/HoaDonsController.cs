using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using QL_Vinpearl.Models;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
    public class HoaDonsController : Controller
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
		// GET: Admin/HoaDons
		public ActionResult Index()
        {
			if (CheckPermission("CN01") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			var hOADON = db.HOADON.Include(h => h.KHACHHANG).Include(h => h.NHANVIEN);
            return View(hOADON.ToList());
        }

        // GET: Admin/HoaDons/Details/5
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
            HOADON hOADON = db.HOADON.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            return View(hOADON);
        }

        // GET: Admin/HoaDons/Create

        public ActionResult Create()
        {
			if (CheckPermission("CN02") == false)
			{
				Response.Redirect("~/Admin/PermissionError/NotAllowPermission");
			}
			ViewBag.maKH = new SelectList(db.KHACHHANG, "maKH", "hoTenKH");
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV");
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maHD,maKH,maNV,ngayThanhToan,hoTenKH,SDT,email")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.HOADON.Add(hOADON);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maKH = new SelectList(db.KHACHHANG, "maKH", "hoTenKH", hOADON.maKH);
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", hOADON.maNV);
            return View(hOADON);
        }

        // GET: Admin/HoaDons/Edit/5
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
            HOADON hOADON = db.HOADON.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            ViewBag.maKH = new SelectList(db.KHACHHANG, "maKH", "hoTenKH", hOADON.maKH);
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", hOADON.maNV);
            return View(hOADON);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maHD,maKH,maNV,ngayThanhToan,hoTenKH,SDT,email")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOADON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maKH = new SelectList(db.KHACHHANG, "maKH", "hoTenKH", hOADON.maKH);
            ViewBag.maNV = new SelectList(db.NHANVIEN, "maNV", "maLoaiNV", hOADON.maNV);
            return View(hOADON);
        }

        // GET: Admin/HoaDons/Delete/5
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
            HOADON hOADON = db.HOADON.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            return View(hOADON);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOADON hOADON = db.HOADON.Find(id);
            db.HOADON.Remove(hOADON);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
		public ActionResult ExportToExcel()
		{
            // query join 2 bảng để lấy dữ liệu hoá đơn
			var query = from hd in db.HOADON
						join cthd in db.CTHD on hd.maHD equals cthd.maHD
						select new
						{
							HOADON = hd,
							CTHD = cthd
						};
			var listHoaDon = query.ToList();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // tạo tên của sheet excel
                var worksheet = package.Workbook.Worksheets.Add("Hoadons");

                // add tên của các cột
                worksheet.Cells[1, 1].Value = "Mã HD";
                worksheet.Cells[1, 2].Value = "Mã KH";
                worksheet.Cells[1, 3].Value = "Mã NV";
                worksheet.Cells[1, 4].Value = "Ngày Thanh Toán";
                worksheet.Cells[1, 5].Value = "SĐT";
                worksheet.Cells[1, 6].Value = "Email";
                worksheet.Cells[1, 7].Value = "Mã Vé";
				worksheet.Cells[1, 8].Value = "Số Lượng";
				worksheet.Cells[1, 9].Value = "Giá Tiền";
				int row = 2;
                foreach (var hd in listHoaDon)
                {
                    // add dữ liệu tương ứng
                    worksheet.Cells[row, 1].Value = hd.HOADON.maHD;
                    worksheet.Cells[row, 2].Value = hd.HOADON.maKH;
                    worksheet.Cells[row, 3].Value = hd.HOADON.maNV;
                    worksheet.Cells[row, 4].Value = hd.HOADON.ngayThanhToan;
                    worksheet.Cells[row, 5].Value = hd.HOADON.SDT;
                    worksheet.Cells[row, 6].Value = hd.HOADON.email;
                    worksheet.Cells[row, 7].Value = hd.CTHD.maVe;
					worksheet.Cells[row, 8].Value = hd.CTHD.soLuong;
					worksheet.Cells[row, 9].Value = hd.CTHD.giaTien;
					row++;
                }

                // Lưu package thành file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "HoaDons.xlsx";

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
