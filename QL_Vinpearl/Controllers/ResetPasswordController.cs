using QL_Vinpearl.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QL_Vinpearl.Controllers
{
    public class ResetPasswordController : Controller
    {
		private QL_VinpearlEntities db = new QL_VinpearlEntities();
		// GET: ResetPassword
		public ActionResult Index()
        {
            return View();
        }
		public ActionResult SendMail()
		{
			return View();
		}
		[HttpPost]
		public ActionResult SendMail(MailInfo model)
		{
			var user = db.KHACHHANG.FirstOrDefault( m => m.email == model.To);
			if (user == null)
			{
				ModelState.AddModelError("", "Email chưa đăng ký tài khoản.");
				return View();
			}
			var code = GenerateRandomCode();
			user.ResetPasswordCode = code;
			user.ResetPasswordCodeExpiration = DateTime.Now.AddMinutes(5); // Đặt thời gian hết hạn là 5 phút sau
			db.SaveChanges();

			// Tạo đường link để cập nhật lại mật khẩu
			var resetLink = Url.Action("Index", "ResetPassword", new { code = code }, Request.Url.Scheme);
			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
			mail.From = new System.Net.Mail.MailAddress("truong.vn.62cntt@ntu.edu.vn");
			mail.To.Add(model.To);
			mail.Subject = "Đặt Lại Mật Khẩu Của Bạn";
			mail.Body = "Đây là đường dẫn đặt lại mật khẩu của bạn: " + resetLink;
			mail.IsBodyHtml = true;
			System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
			smtp.Credentials = new System.Net.NetworkCredential("truong.vn.62cntt@ntu.edu.vn", "264582503a");
			smtp.EnableSsl = true;
			smtp.Send(mail);
			return RedirectToAction("Index", "DICHVU");
		}
		private string GenerateRandomCode()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			var code = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
			return code;
		}

		[HttpGet]
		public ActionResult PageUpdatePassword(string code)
		{
			// Kiểm tra mã code và thời gian hết hạn
			var user = db.KHACHHANG.FirstOrDefault(u => u.ResetPasswordCode == code && u.ResetPasswordCodeExpiration > DateTime.Now);
			if (user == null)
			{
				// Mã code không hợp lệ hoặc đã hết hạn
				return RedirectToAction("InvalidCode");
			}

			// Truyền mã code vào View để sử dụng trong form cập nhật mật khẩu
			ViewBag.Code = code;

			return View();
		}

		[HttpPost]
		public ActionResult PageUpdatePassword(string code, string newPassword)
		{
			// Kiểm tra mã code và thời gian hết hạn
			var user = db.KHACHHANG.FirstOrDefault(u => u.ResetPasswordCode == code && u.ResetPasswordCodeExpiration > DateTime.Now);
			
			if (user == null)
			{
				// Mã code không hợp lệ hoặc đã hết hạn
				return RedirectToAction("InvalidCode");
			}

			// Cập nhật mật khẩu mới
			user.matKhau = newPassword;

			// Xóa mã code và thời gian hết hạn
			user.ResetPasswordCode = null;
			user.ResetPasswordCodeExpiration = null;

			db.SaveChanges();

			// Chuyển hướng đến trang thông báo cập nhật mật khẩu thành công
			return RedirectToAction("Index","DICHVU");
		}

	}
}