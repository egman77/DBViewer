using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DBViewer.Model.Core
{
    /// <summary>
    /// DBViewer模型
    /// </summary>
    public interface IDBViewerModel
    {
        DBViewerConfig Coinfig { get; set; }
        /// <summary>
        /// 关闭触发器
        /// </summary>
        void DeleteTrigger(string tableName);
        /// <summary>
        /// 重新建立触发器
        /// </summary>
        void ReBuildTrigger(string tableName);

        /// <summary>
        /// 移除触发器
        /// </summary>
        void RemoveTrigger();

        /// <summary>
        /// 返回表列表数据
        /// </summary>
        /// <returns></returns>
        DataTable GetTableList();



        /// <summary>
        /// 获取跟踪数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        DataTable GetTraceData(string userName, DateTime startTime);

        /// <summary>
        /// 清楚跟踪数据
        /// </summary>
        /// <param name="userName"></param>
        void ClearTraceData(string userName);
    }
}
