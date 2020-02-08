using CoreTpl.Enums;
using System.Collections.Generic;

namespace CoreTpl.Domain
{

    public class TplConfig
    {
        /// <summary>網站標題</summary>
        public string Title { get; set; }

        /// <summary>控制器 Log 的目錄</summary>
        public string CtrlLogDirectory { get; set; }

        /// <summary>控制器狀態的過期天數</summary>
        public int CtrlStatusExpiryDays { get; set; }

        /// <summary>控制器警報的過期天數</summary>
        public int CtrlAlarmExpiryDays { get; set; }

        /// <summary>命令的過期天數</summary>
        public int CtrlCommandExpiryDays { get; set; }

        /// <summary>庫位紀錄的過期天數</summary>
        public int LocationRecordExpiryDays { get; set; }

        /// <summary>部屬前準備</summary>
        public bool IsDeployReady { get; set; }
    }

}