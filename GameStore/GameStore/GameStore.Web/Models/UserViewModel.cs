using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "IsPersistent")]
        public bool IsPersistent { get; set; }

        public string RedirectUrl { get; set; }
    }
}