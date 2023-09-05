using QL_Vinpearl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
		private QL_VinpearlEntities db = new QL_VinpearlEntities();
		// GET: Admin/Dashboard
		public ActionResult Index()
		{
			// Đếm số lượng khách hàng trong bảng "KHACHHANG"
			int countKH = db.KHACHHANG.Count();

			// Đếm số lượng dịch vụ trong bảng "DICHVU"
			int countDV = db.DICHVU.Count();

			// Đếm số lượng hóa đơn trong bảng "HOADON"
			int countHD = db.HOADON.Count();

			// Đếm số lượng vé trong bảng "VE"
			int countVe = db.VE.Count();

			// Lưu trữ các giá trị đếm vào các thuộc tính của đối tượng "ViewBag"
			ViewBag.CountKH = countKH;
			ViewBag.CountDV = countDV;
			ViewBag.CountHD = countHD;
			ViewBag.CountVe = countVe;

			return View();
		}
	}
}