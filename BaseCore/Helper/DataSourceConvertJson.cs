using BaseCore.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Helper
{
    public static class DataSourceConvertJson
    {
        public static DataSourceRequest Convert(string str)
        {
            var queryString = (System.Uri.UnescapeDataString(str)).Replace("\\", "").Replace("?", "");
            var qurestringCount = queryString.Length;
            var indexFirst = queryString.IndexOf("{");
            if (indexFirst > 0)
                queryString = queryString.Substring(indexFirst, qurestringCount - indexFirst);
            var index = queryString.IndexOf("&_=");
            if (index > 0)
                queryString = queryString.Substring(0, index);
           return JsonConvert.DeserializeObject<DataSourceRequest>(queryString);

        }
    }
}
