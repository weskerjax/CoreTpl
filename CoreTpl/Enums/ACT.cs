using System.ComponentModel;

namespace CoreTpl.Enums
{
	/// <summary>Access Control Tag</summary>
	public enum ACT 
	{

		/// <summary>角色管理</summary>
		[Description("角色管理")]
		RoleSetting,

		/// <summary>使用者管理</summary>
		[Description("使用者管理")]
		UserSetting,

	}
}