using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class UserCreateViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Password")]
        [MinLength(6, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "MinPasswordLength")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ComparePasswordValidationError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "RepeatePassword")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "CommentName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Roles")]
        public List<long> RoleIds { get; set; }
        
        public List<RoleViewModel> Roles { get; set; }
    }
}