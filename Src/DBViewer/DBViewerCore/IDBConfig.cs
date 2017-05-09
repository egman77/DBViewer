using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DBViewer.Model.Core
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    public interface IDBConfig
    {
        /// <summary>
        /// 配置控件
        /// </summary>
        /// <returns></returns>
        Control GetConfigPanel(DBViewerConfig config);

        DBViewerConfig GetConfig();
    }
}
