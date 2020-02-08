using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTpl.WebApp.Models
{
	public class UserActViewModel
	{

		/// <summary>使用者Id</summary>
		[Required]
		[Display(Name = "使用者編號")]
		public int UserId { get; set; }

		/// <summary>姓名</summary>
		[Display(Name = "姓名")]
		public string UserName { get; set; }

		/// <summary>帳號</summary>
		[Display(Name = "帳號")]
		public string Account { get; set; }

		/// <summary>角色權限</summary>
		[Display(Name = "角色權限")]
		public List<string> RoleActList { get; set; }

		/// <summary>個人權限</summary>
		[Display(Name = "個人權限")]
		public List<string> ActList { get; set; }


		/// <summary>建立人</summary>
		[Display(Name = "建立人")]
		public int CreateBy { get; set; }

		/// <summary>建立日期</summary>
		[Display(Name = "建立日期")]
		public DateTimeOffset CreateDate { get; set; }

		/// <summary>修改人</summary>
		[Display(Name = "修改人")]
		public int ModifyBy { get; set; }

		/// <summary>修改日期</summary>
		[Display(Name = "修改日期")]
		public DateTimeOffset ModifyDate { get; set; }


	}
}