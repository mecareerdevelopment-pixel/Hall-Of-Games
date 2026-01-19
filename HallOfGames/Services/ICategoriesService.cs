namespace HallOfGames.Services
{
    public interface ICategoriesService
    {
        public IEnumerable<SelectListItem> GetAllCategoriesAsSelectList();
    }
}
