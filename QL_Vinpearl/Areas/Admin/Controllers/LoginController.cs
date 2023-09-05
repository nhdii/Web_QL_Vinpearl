using QL_Vinpearl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
		private QL_VinpearlEntities db = new QL_VinpearlEntities();
		// GET: Admin/Login
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Login(LoginAdminModel model)
		{
			// query vào bảng NV và check các điều kiện.
			var user = (from nv in db.NHANVIEN
						join lnv in db.LOAINV on nv.maLoaiNV equals lnv.maLoaiNV
						where nv.email == model.Email
						select new
						{
							NhanVien = nv,
							LoaiNhanVien = lnv
						}).SingleOrDefault();

			if (user == null)
			{
				ModelState.AddModelError("", "Tài khoản không tồn tại!");
			}
			else if (user.NhanVien.matKhau == model.MKhau)
			{
				// tạo session để lưu dữ liệu trong phiên làm việc
				Session.Add("Email", model.Email);
				Session.Add("TenTaiKhoan", user.NhanVien.hoTenNV);
				Session.Add("maLNV", user.LoaiNhanVien.maLoaiNV);
				return RedirectToAction("Index", "Dashboard");
			}
			else if (user.NhanVien.matKhau != model.MKhau)
			{
				ModelState.AddModelError("", "Mật khẩu không hợp lệ!");
			}
			else
			{
				ModelState.AddModelError("", "Thông tin tài khoản không hợp lệ!");
			}
			return View("Index");
		}
		public ActionResult Logout()
		{
			// destroy session
			Session["Email"] = null;
			Session["TenTaiKhoan"] = null;
			Session["maLNV"] = null;
			return RedirectToAction("Index", "Login");
		}
	}
}