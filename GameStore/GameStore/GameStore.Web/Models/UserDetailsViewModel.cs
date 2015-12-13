using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class UserDetailsViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "CommentName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Roles")]
        public string Roles { get; set; }
    }
}