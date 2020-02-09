using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreTpl.Enums;

namespace CoreTpl.Domain
{
    public class UserActDomain
    {		/// <summary>使用者Id</summary>
		[Display(Name = "使用者Id")]
        public int UserId { get; set; }

        /// <summary>姓名</summary>
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Display(Name = "使用者類型")]
        public UserType UserType { get; set; }

        /// <summary>帳號</summary>
        [Display(Name = "帳號")]
        public string Account { get; set; }


        /// <summary>角色權限</summary>
        [Display(Name = "角色權限")]
        public List<string> RoleActList { get; set; }

        /// <summary>允許權限</summary>
        [Display(Name = "允許權限")]
        public List<string> AllowActList { get; set; }

        /// <summary>拒絕權限</summary>
        [Display(Name = "拒絕權限")]
        public List<string> DenyActList { get; set; }

        /// <summary>建立人</summary>
        [Display(Name = "建立者")]
        public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
        [Display(Name = "建立時間")]
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
        [Display(Name = "修改者")]
        public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
        [Display(Name = "修改時間")]
        public DateTimeOffset ModifyDate { get; set; }

    }
}
