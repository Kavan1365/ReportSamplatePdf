using System.Collections.Generic;
using System.Linq;
using BaseCore.Helper.DynamicLinq;
using Newtonsoft.Json;

namespace BaseCore.ViewModel
{
    public class DataSourceRequest
    {
        public int skip { get; set; }

        public int take { get; set; } = 50;
        //public int page { get; set; }
        //public int pageSize { get; set; }
        public List<SortDescription> sort { get; set; }
        public FilterDescription filter { get; set; }

        public bool HasFilter()
        {
            return filter?.Filters != null && filter.Filters.Any();
        }


        public IEnumerable<Aggregator> aggregate { get; set; }
        public IEnumerable<Group> group { get; set; }
    }
    public class DataSourceRequestTree
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public IEnumerable<SortTree> Sort { get; set; }
        public FilterTreeList filter { get; set; }
        public IEnumerable<Aggregator> aggregate { get; set; }
        public IEnumerable<Group> group { get; set; }
        public int? Id { get; set; }
    }
}
