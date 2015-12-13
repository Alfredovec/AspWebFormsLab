namespace GameStore.Models.Utils
{
    public class CommentAction
    {
        public CommentActionEnum ActionEnum { get; set; }

        public long CommentActionId { get; set; }

        public enum CommentActionEnum
        {
            Reply,
            Quote
        }
    }


}