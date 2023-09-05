using QL_Vinpearl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QL_Vinpearl.Controllers
{
	public class CartController : Controller
    {
		private QL_VinpearlEntities db = new QL_VinpearlEntities();
		// GET: Cart
		public ActionResult Index()
        {
            return View();
        }
		public ActionResult PlusItem(string maDV) 
		{
			var cart = Session["cart"] as List<Dictionary<string, object>>;
			var item = cart.FirstOrDefault(p => p["maDV"].ToString() == maDV);
			if (item != null)
			{
				// Sửa thông tin của sản phẩm
				item["soLuong"] = Convert.ToInt32(item["soLuong"]) + 1;
				int soLuongItem = Convert.ToInt32(item["soLuong"]);
				int gia = Convert.ToInt32(item["gia"]);
				item["thanhTien"] = soLuongItem * gia;
				// Cập nhật lại session
				Session["cart"] = cart;
			}
			return RedirectToAction("Index");
		}
		public ActionResult MinusItem(string maDV)
		{
			var cart = Session["cart"] as List<Dictionary<string, object>>;
			var item = cart.FirstOrDefault(p => p["maDV"].ToString() == maDV);
			if (item != null)
			{
				// Sửa thông tin của sản phẩm
				var soLuong = Convert.ToInt32(item["soLuong"]);
				if (soLuong == 1)
				{
					item["soLuong"] = 1;
					Session["cart"] = cart;
					return RedirectToAction("Index");
				}
				item["soLuong"] = Convert.ToInt32(item["soLuong"]) - 1;
				int soLuongItem = Convert.ToInt32(item["soLuong"]);
				int gia = Convert.ToInt32(item["gia"]);
				item["thanhTien"] = soLuongItem * gia;
				// Cập nhật lại session
				Session["cart"] = cart;
			}
			return RedirectToAction("Index");
		}

		public ActionResult RemoveItem(string maDV) 
		{
			var cart = Session["cart"] as List<Dictionary<string, object>>;

			if (cart != null)
			{
				cart.RemoveAll(item => item["maDV"].ToString() == maDV);
				Session["cart"] = cart;
			}
			return RedirectToAction("Index");
		}
		public ActionResult Cart(string id, int loaiVe)
		{
            if (Session["EmailKH"] == null)
            {
                return RedirectToAction("Login", "LoginKhachHang");
            }

            if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			DICHVU dICHVU = db.DICHVU.Find(id);
			if (dICHVU == null)
			{
				return HttpNotFound();
			}

			string emailKH = Session["EmailKH"].ToString();

			var cart = Session["cart"] as List<Dictionary<string, object>>;

			if (cart != null)
			{
				// Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
				var existingItem = cart.FirstOrDefault(item => item.ContainsKey("maDV") && item["maDV"].ToString() == dICHVU.maDV);
				if (existingItem != null)
				{
					// Nếu sản phẩm đã tồn tại, chỉ cần cập nhật thông tin mới
					existingItem["loaiVe"] = loaiVe;
					existingItem["soLuong"] = Convert.ToInt32(existingItem["soLuong"]) + 1;
				}
				else
				{
					// Nếu sản phẩm chưa tồn tại, thêm mới vào giỏ hàng
					bool boolValue = Convert.ToBoolean(loaiVe);
					var query = (from ve in db.VE
								 join dv in db.DICHVU on ve.maDV equals dv.maDV
								 where dv.maDV == id && ve.loaiVe == boolValue
								 select new
								 {
									 Ve = ve,
									 DichVu = dv
								 }).SingleOrDefault();
					object thanhTien = null;
					var cartItem = new Dictionary<string, object>
					{
						{ "maDV", dICHVU.maDV },
						{ "loaiVe", loaiVe },
						{ "soLuong", 1 },
						{ "anh", query.DichVu.anh },
						{"tenDV", query.DichVu.tenDV },
						{"gia", query.Ve.giaTien },
						{ "thanhTien", thanhTien }
					};
					int soLuongItem = Convert.ToInt32(cartItem["soLuong"]);
					int gia = Convert.ToInt32(cartItem["gia"]);
					cartItem["thanhTien"] = soLuongItem * gia;
					cart.Add(cartItem);
				}
			}
			else
			{
				// Nếu giỏ hàng chưa tồn tại, tạo mới và thêm sản phẩm vào
				bool boolValue = Convert.ToBoolean(loaiVe);
				var query = (from ve in db.VE
							 join dv in db.DICHVU on ve.maDV equals dv.maDV
							 where dv.maDV == id && ve.loaiVe == boolValue
							 select new
							 {
								 Ve = ve,
								 DichVu = dv
							 }).SingleOrDefault();
				object thanhTien = null;
				var cartItem = new Dictionary<string, object>
				{
					{ "maDV", dICHVU.maDV },
					{ "loaiVe", loaiVe },
					{ "soLuong", 1 },
					{ "anh", query.DichVu.anh },
					{"tenDV", query.DichVu.tenDV },
					{"gia", query.Ve.giaTien },
					{ "thanhTien", thanhTien }
		}		;
				int soLuongItem = Convert.ToInt32(cartItem["soLuong"]);
				int gia = Convert.ToInt32(cartItem["gia"]);
				cartItem["thanhTien"] = soLuongItem * gia;
				cart = new List<Dictionary<string, object>> { cartItem };
			}
			Session["cart"] = cart;
			return RedirectToAction("Index");
		}
		string LayMaHD()
		{
			var maMax = db.HOADON.ToList().Select(n => n.maHD).Max();
			int maHD = int.Parse(maMax.Substring(2)) + 1;
			string HD = maHD.ToString().PadLeft(6, '0');
			return "HD" + HD;
		}
		public ActionResult Buy()
		{
			string maHD = LayMaHD();
			string email = Session["EmailKH"].ToString();
			var user = db.KHACHHANG.FirstOrDefault(kh => kh.email == email);
			var newHoaDon = new HOADON
			{
				maHD = maHD,
				maKH = user.maKH,
				maNV = "NV001",
				ngayThanhToan = DateTime.Now,
				SDT = user.SDT,
				email = user.email
			};
			db.HOADON.Add(newHoaDon);
			db.SaveChanges();

			var cart = Session["cart"] as List<Dictionary<string, object>>;
			foreach (var cartItem in cart)
			{
				string maDV = cartItem["maDV"].ToString();
				bool boolValue = Convert.ToBoolean(cartItem["loaiVe"]);
				var query = (from ve in db.VE
							 join dv in db.DICHVU on ve.maDV equals dv.maDV
							 where dv.maDV == maDV && ve.loaiVe == boolValue
							 select new
							 {
								 Ve = ve,
								 DichVu = dv
							 }).SingleOrDefault();

				var newCTHD = new CTHD
				{
					maHD = maHD,
					maVe = query.Ve.maVe,
					soLuong = Convert.ToInt32(cartItem["soLuong"]),
					giaTien = Convert.ToInt32(cartItem["thanhTien"])

				};
				db.CTHD.Add(newCTHD);
				db.SaveChanges();
			}

			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
			mail.From = new System.Net.Mail.MailAddress("truong.vn.62cntt@ntu.edu.vn");
			mail.To.Add(Session["EmailKH"].ToString());
			mail.Subject = "Hoá Đơn Thanh Toán Của Bạn!";
			string body = "Đây là thông tin chi tiết về dịch vụ mà bạn đã mua: <br/>";
			body += "<strong>Tên dịch vụ:</strong> <br/>";
			foreach (var cartItem in cart)
			{
				string loaiVe = "";
				if (cartItem["loaiVe"] as int? == 0)
				{
					loaiVe = "Trẻ Em";
				}
				else
				{
					loaiVe = "Người Lớn";
				}
				body += "	- " + cartItem["tenDV"] + " | <strong>Loại Vé:</strong> " + loaiVe + " | <strong>Số Lượng:</strong> " + cartItem["soLuong"] + "<br/>";
			}
			body += "<strong>Ngày Mua:</strong> " + DateTime.Now + "<br/>";
			body += "<strong>Tổng Tiền:</strong> " + Session["tongTienMua"] + " vnđ" + "<br/>";
			mail.Body = body;
			mail.IsBodyHtml = true;
			System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
			smtp.Credentials = new System.Net.NetworkCredential("truong.vn.62cntt@ntu.edu.vn", "264582503a");
			smtp.EnableSsl = true;
			smtp.Send(mail);

			Session["cart"] = null;
			Session["tongTienMua"] = null;
			return RedirectToAction("Index");
		}
	}
}