using Jeremy.IdentityCenter.Database.Models;
using Jeremy.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeremy.IdentityCenter.Database.Helper
{
    public class EnumHelper
    {
        /// <summary>
        /// 将 Enum 类型转为 List&lt;<see cref="SelectItem"/>&gt; 类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<SelectItem> EnumToList<T>() where T : struct, IComparable
        {
            return Utils.EnumToList<T>().Select(x => new SelectItem(x.Key, x.Value)).ToList();
        }
    }
}
