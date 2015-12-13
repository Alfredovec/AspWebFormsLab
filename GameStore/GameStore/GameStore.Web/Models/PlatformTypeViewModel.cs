using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class PlatformTypeViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "GameName")]
        public string Type { get; set; }
    }
}