using System.Collections.Generic;
using System.Linq;

namespace Jeremy.IdentityCenter.Database.Common.Models
{
    /// <summary>
    /// 页列表对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> where T : class
    {
        public PageList()
        {

        }

        public PageList(IEnumerable<T> items, int totalCount, int pageSize)
        {
            Items = items.ToList();
            TotalCount = totalCount;
            PageSize = pageSize;
        }

        /// <summary>
        /// 所有数据
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 数据总量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 每页长度
        /// </summary>
        public int PageSize { get; set; }
    }
}
