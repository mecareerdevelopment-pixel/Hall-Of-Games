namespace HallOfGames.Services
{
    public interface IDevicesService
    {
        public IEnumerable<SelectListItem> GetAllDevicesAsSelectList();
    }
}
