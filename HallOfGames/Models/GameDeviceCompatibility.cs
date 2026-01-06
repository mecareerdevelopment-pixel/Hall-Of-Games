namespace HallOfGames.Models
{
    public class GameDeviceCompatibility
    {
        public int GameId { get; set; }

        public Game Game { get; set; } = default!;

        public int DeviceId { get; set; }

        public Device Device { get; set; } = default!;

    }
}
