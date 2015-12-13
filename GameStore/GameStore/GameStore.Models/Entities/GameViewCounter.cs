namespace GameStore.Models.Entities
{
    public class GameViewCounter
    {
        public long Id { get; set; }

        public long GlobalGameId { get; set; }

        public long ViewCount { get; set; }
    }
}
