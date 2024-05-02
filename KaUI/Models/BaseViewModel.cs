using Microsoft.AspNetCore.Mvc;
using System;

namespace KaUI.Models
{
    public class BaseViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Guid Guid { get; set; }
    }
}
