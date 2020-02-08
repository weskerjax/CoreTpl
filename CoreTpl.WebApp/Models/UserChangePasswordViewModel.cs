using System.ComponentModel.DataAnnotations;

namespace CoreTpl.WebApp.Models
{
    public class UserChangePasswordViewModel
    {
        /// <summary>舊密碼</summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "舊密碼")]
        public string OldPassword { get; set; }

        /// <summary>密碼</summary>
        [Required]
        [StringLength(30, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>確認密碼</summary>
        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }




    }
}