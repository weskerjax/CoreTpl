using System.ComponentModel.DataAnnotations;

namespace CoreTpl.WebApp.Models
{
	public class UserLoginViewModel
	{
		/// <summary>帳號</summary>
		[Required]
		[Display(Name = "帳號")]
		public string Account { get; set; }

		/// <summary>密碼</summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "密碼")]
		public string Password { get; set; }


		[Display(Name = "驗證碼")]
		public string Captcha { get; set; }

	}
}