using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class BanCommentViewModel
    {
        public long CommentId { get; set; }

        [UIHint("Enum")]
        public BanDuration Ban { get; set; }

        public enum BanDuration
        {
            [Display(ResourceType = typeof (GlobalRes), Name = "OneHour")]
            OneHour,
            [Display(ResourceType = typeof (GlobalRes), Name = "OneDay")]
            OneDay,
            [Display(ResourceType = typeof (GlobalRes), Name = "OneWeek")]
            OneWeek,
            [Display(ResourceType = typeof (GlobalRes), Name = "OneMonth")]
            OneMonth
        }
    }
}