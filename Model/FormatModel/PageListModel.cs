using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.FormatModel
{
    public class PageListModel<T>
    {
        /// <summary>
        /// 页码索引，第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页容量，每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页码数
        /// </summary>
        public int PageCount { get; set; }
        
        /// <summary>
        /// 根据查询条件查得的数据总数
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> Items { get; set; }
    }
}
