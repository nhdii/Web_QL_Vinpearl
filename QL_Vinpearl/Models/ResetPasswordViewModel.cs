using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QL_Vinpearl.Models
{
	public class ResetPasswordViewModel
	{
		[Required]
		public string Code { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}