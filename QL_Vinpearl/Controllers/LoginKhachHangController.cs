using QL_Vinpearl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Vinpearl.Controllers
{
	public class LoginKhachHangController : Controller
	{
		private QL_VinpearlEntities db = new QL_VinpearlEntities();
		// GET: LoginKhachHangs
		public ActionResult Index()
		{
			return View();
		}
		private bool IsEmailExists(string email)
		{
			return db.KHACHHANG.Any(kh => kh.email == email);
		}
		string LayMaKH()
		{
			// query lấy mã cuối cùng trong bảng và parse sang số
			var maMax = db.KHACHHANG.ToList().Select(n => n.maKH).Max();
			int maKH = int.Parse(maMax.Substring(2)) + 1;
			// mã kh: KH000000 => 6 số
			string KH = maKH.ToString().PadLeft(6, '0');
			return "KH" + KH;
		}
		// GET: Customer/Register
		public ActionResult Register()
		{
			ViewBag.MaKH = LayMaKH();
			return View();
		}

		// POST: Customer/Register
		[HttpPost]
		public ActionResult Register(KHACHHANG model)
		{

			if (IsEmailExists(model.email))
			{
				ModelState.AddModelError("Email", "Email đã được sử dụng.");
				return View(model);
			}

			if (ModelState.IsValid)
			{
				Session["EmailKH"] = model.email;
				model.maKH = LayMaKH();
				db.KHACHHANG.Add(model);
				db.SaveChanges();
				return RedirectToAction("Index", "DICHVU");
			}
			return View(model);
		}

		// GET: LoginKhachHang
		public ActionResult Login()
		{
			return View();
		}
		// POST: LoginKhachHang
		[HttpPost]
		public ActionResult Login(LoginKhachHangModel model)
		{
			var user = db.KHACHHANG.SingleOrDefault(kh => kh.email == model.Email);
			if (user == null)
			{
				ModelState.AddModelError("Email", "Tài khoản không tồn tại!");
			}
			else if (user.matKhau == model.MKhau)
			{
				Session["EmailKH"] = model.Email;
				return RedirectToAction("Index", "DICHVU");
			}
			else if (user.matKhau != model.MKhau)
			{
				ModelState.AddModelError("MKhau", "Mật khẩu không chính xác!");
			}
			else
			{
				ModelState.AddModelError("", "Thông tin tài khoản không hợp lệ!");
			}
			return View("Login");
		}

		public ActionResult Logout()
		{
			Session["EmailKH"] = null;
			return RedirectToAction("Index", "DICHVU");
		}
	}
}