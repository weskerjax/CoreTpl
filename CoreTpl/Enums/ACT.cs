using System.ComponentModel;

namespace CoreTpl.Enums
{
	/// <summary>Access Control Tag</summary>
	public enum ACT 
	{
		/// <summary>儲區設定</summary>
		[Description("儲區設定")]
		STK_ZoneSetting,

		/// <summary>料件設定</summary>
		[Description("料件設定")]
		STK_MaterialSetting,

		/// <summary>庫位設定</summary>
		[Description("庫位設定")]
		STK_StoreLocationEdit,

		/// <summary>庫存調帳</summary>
		[Description("庫存調帳")]
		STK_StoreAdjustment,

        /// <summary>入庫</summary>
        [Description("入庫")]
        STK_StorageIn,

        /// <summary>出庫</summary>
        [Description("出庫")]
        STK_StorageOut,

        /// <summary>批次出庫</summary>
        [Description("批次出庫")]
        STK_StorageBatchOut,



        /*############################################################*/

        /// <summary>命令異常處理</summary>
        [Description("命令異常處理")]
		STK_CtrlCommandEdit,

        /// <summary>命令優先設定</summary>
        [Description("命令優先設定")]
        STK_CtrlCommandPriority,

        /// <summary>手動控制面板</summary>
        [Description("手動控制面板")]
        STK_ManualControl,

        /// <summary>PLC 工具</summary>
        [Description("PLC 工具")]
		STK_TransactionTool,
		


		/*############################################################*/

		/// <summary>角色管理</summary>
		[Description("角色管理")]
		STK_RoleSetting,

		/// <summary>使用者管理</summary>
		[Description("使用者管理")]
		STK_UserSetting,

		/// <summary>使用者個人權限</summary>
		[Description("使用者個人權限")]
		STK_UserActSetting,

	}
}