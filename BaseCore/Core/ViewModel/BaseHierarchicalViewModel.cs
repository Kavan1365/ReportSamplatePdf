using BaseCore.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BaseCore.ViewModel
{
    public class BaseHierarchicalViewModel : BaseViewModel
    {
        [HiddenInput]
        public int? ParentId { get; set; }

        [HiddenInput]
        public bool hasChildren { get { return true; } }
    }
}
