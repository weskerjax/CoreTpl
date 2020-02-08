using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Orion.API.Models;


namespace CoreTpl.Domain
{
    public class UserDomain 
    {

        /// <summary>使用者Id</summary>
        [Display(Name = "使用者Id")]
        public int UserId { get; set; }

        /// <summary>帳號</summary>
        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        /// <summary>姓名</summary>
        [Required]
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        
        /// <summary>E-Mail</summary>
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        /// <summary>狀態</summary>
        [Display(Name = "狀態")]
        public UseStatus UseStatus { get; set; }

        /// <summary>部門</summary>
        [Display(Name = "部門")]
        public string Department { get; set; }

        /// <summary>分機號碼</summary>
        [Display(Name = "分機號碼")]
        public string ExtensionNum { get; set; }

        /// <summary>職稱</summary>
        [Display(Name = "職稱")]
        public string UserTitle { get; set; }

        /// <summary>備註</summary>
        [DataType(DataType.MultilineText)]
        [Display(Name = "備註")]
        public string RemarkText { get; set; }


        /// <summary>角色Ids</summary>
        [Display(Name = "角色")]
        public List<int> RoleIds { get; set; }


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
