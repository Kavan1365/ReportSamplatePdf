using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaUI.Utilities
{
    public static class ExtensionMethod
    {

        /// <summary>
        /// انتخاب کردن یک عنصر به صورت تصادفی
        /// </summary>
        /// <typeparam name="T">عنصر خروجی</typeparam>
        /// <param name="list">لیست ورودی</param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default(T);

            Random rnd = new Random();
            return list.ElementAt(rnd.Next(list.Count()));
        }

    }
}
