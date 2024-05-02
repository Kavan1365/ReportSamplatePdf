using KaUI.Configuration;
using KaUI.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KaUI.Helper
{
    public class ActionViewModelUpdateResult
    {

        [DataType(DataType.MultilineText)]
        public string AddedActions { get; set; }

        [DataType(DataType.MultilineText)]
        public string RemovedActions { get; set; }
    }
    
    public class ActionViewModel : BaseViewModel
    {
        [Display(Name = "اکشن")]
        public string Name { get; set; }
        [Display(Name = "کنترلر")]
        public string ControllerName { get; set; }
        [Display(Name = "اکشن (فارسی)")]
        public string NameFa { get; set; }
        [Display(Name = "کنترلر(فارسی)")]
        public string ControllerNameFa { get; set; }
        [Display(Name = "فضای نام")]
        public string NameSpace { get; set; }
        [Display(Name = "متد")]
        public string Method { get; set; }
        [Display(Name = "آدرس")]
        public string Url { get; set; }
 
    }
    
    public static class ActionHelper
    {
        public static IEnumerable<ActionViewModel> GetAllActions(Assembly asm)
        {

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                    }).ToList();

            return asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Select(n => new ActionViewModel()
                {
                    Name = n.Name,
                    ControllerName = n.DeclaringType?.Name.Replace("Controller", ""),
                    Method = n.IsDefined(typeof(HttpPostAttribute)) ? "Post" : "Get",
                    NameSpace = n.DeclaringType.Namespace,
                    Url = $"/{n.DeclaringType?.Name.Replace("Controller", "")}/{n.Name}".ToLower(),
                }); ;



        }

         
        }
}
