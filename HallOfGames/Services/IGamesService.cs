using HallOfGames.ViewModels;

namespace HallOfGames.Services
{
    public interface IGamesService
    {
        // CRUD Operations

        Task Create(CreateGameViewModel model);     // Create

        IEnumerable<Game> GetAll();     // Read

        public Game? GetById(int gameId);        // Read

        public Task<Game?> Edit(EditGameViewModel model);      // Update

        public bool Delete(int gameId);      // Delete

        public bool CheckIfGameNameIsUnique(string gameName, int gameId);



    }
}
