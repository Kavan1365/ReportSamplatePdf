using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KaUI.Core.ViewModel
{
    public class FileViewModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string Folder { get; set; }

    }
}
