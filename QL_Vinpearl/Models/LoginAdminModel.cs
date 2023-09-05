using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QL_Vinpearl.Models
{
	public class LoginAdminModel
	{
		[DisplayName("Tài khoản Email")]
		public string Email { get; set; }
		[DisplayName("Mật khẩu")]
		public string MKhau { get; set; }
	}
}