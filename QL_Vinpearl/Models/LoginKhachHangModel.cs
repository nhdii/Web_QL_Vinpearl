using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QL_Vinpearl.Models
{
	public class LoginKhachHangModel
	{
		[Required(ErrorMessage = "Email không được để trống!")]
		[DisplayName("Tài khoản Email")]
		public string Email { get; set; }
		[DisplayName("Mật khẩu")]
		public string MKhau { get; set; }
	}
}