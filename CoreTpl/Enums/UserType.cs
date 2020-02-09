using System.ComponentModel.DataAnnotations;

namespace CoreTpl.Enums
{
    /// <summary>使用者類型</summary>
    public enum UserType
	{
        /// <summary>無</summary>
        [Display(Name = "無")]
        None,

        /// <summary>系統</summary>
        [Display(Name = "系統")]
        System,

        /// <summary>管理者</summary>
        [Display(Name = "管理者")]
        Manager,

    }
}
