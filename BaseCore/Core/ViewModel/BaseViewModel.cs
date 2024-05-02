using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCore.Core.ViewModel
{
    public class BaseViewModel
    {
        [HiddenInput]
        public int ID { get; set; }
        [HiddenInput]
        public Guid Guid { get; set; }

        
    }
}
