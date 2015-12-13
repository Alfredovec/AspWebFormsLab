using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class CommentViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Body")]
        public string Body { get; set; }

        public bool IsDeleted { get; set; }

        public string QuoteName { get; set; }

        public string QuoteText { get; set; }

        public ICollection<CommentViewModel> Children { get; set; }
    }
}