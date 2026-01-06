namespace HallOfGames.Models
{
    public class Device : BaseEntity
    {
        public string Icon { get; set; } = string.Empty;

        public ICollection<GameDeviceCompatibility> CompetabileGames { get; set; } = new List<GameDeviceCompatibility>();
    }
}
