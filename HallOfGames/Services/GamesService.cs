using HallOfGames.ViewModels;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HallOfGames.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _coverImagesFolderPath;
       

        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration staticValues)
        {
            // webHostEnvironment is to get the root of the web application (wwwroot)


            _context = context;
            _coverImagesFolderPath = $"{webHostEnvironment.WebRootPath}{staticValues["GamesCoverImagesSavingPath"]}";
        }

        public async Task Create(CreateGameViewModel model)
        {
            /*Two Steps to save
             * First is saving the file inside the server Hard
             * Second is saving the data of the game and the cover address inside Database
             */

            Game newGame = new()
            {
                Name = model.Name,
                CategoryId = (int)model.CategoryId!,
                Description = model.Description ?? "",
                Cover = await SaveCoverImage(model.Cover),
                CompatibleDevices = model.CompatabileDevicesIds.Select(d => new GameDeviceCompatibility
                {
                    DeviceId = d
                }).ToList()
            };

            _context.Games.Add(newGame);
            _context.SaveChanges();
        }


        public IEnumerable<Game> GetAll()
        {
            var games = _context.Games
                .Include(g => g.Category)
                .Include(g => g.CompatibleDevices).ThenInclude(d => d.Device)
                .AsNoTracking()     // more faster and optimized
                .ToList();

            return games;
        }

        public Game? GetById(int gameId)
        {
            var game = _context.Games.Where(g => g.Id == gameId)
                .Include(g => g.Category)
                .Include(g => g.CompatibleDevices).ThenInclude(c => c.Device)
                .FirstOrDefault();

            return game;
        }

        public async Task<Game?> Edit(EditGameViewModel model)
        {
            //the row/entity/object of the game is tracked by ef core, so any change will reflect on the DB
            Game? gameInDB = _context.Games
                .Include(g => g.CompatibleDevices)
                .SingleOrDefault(g => g.Id == model.gameId);

            if (gameInDB is null)
            {
                // the id may be changed from HTML of the edit page and sent
                return null;
            }


            if (model.Cover is not null)
            {
                // This means that the user entered a new covr image so the image must be edited and old
                // cover image must be deleted
                DeleteCoverImage(gameInDB.Cover);

                gameInDB.Cover = await SaveCoverImage(model.Cover);
            }

            gameInDB.Name = model.Name;
            gameInDB.Description = model.Description ?? "";
            gameInDB.CategoryId = (int)model.CategoryId!;



            // all old compatabilities are deleted and compatabilities that in model are added as new records
            // This needs to be enhanced by inserting only the new and deleting only the non-existant records, not delete all and insert all
            // the correct approach here is DELETE ONLY REMOVED, INSERT ONLY NEW, instead of what I am doing
            gameInDB.CompatibleDevices = model.CompatabileDevicesIds.Select(d => 
            new GameDeviceCompatibility
            {
                DeviceId = d,
                GameId = gameInDB.Id
            }).ToList();

            await _context.SaveChangesAsync();

            return gameInDB;
        }


        public bool Delete(int gameId)
        {
            try
            {
                var gameInDB = GetById(gameId);

                if (gameInDB is null)
                {
                    return false;
                }

                _context.Games.Remove(gameInDB);


                int affectedRows = _context.SaveChanges();

                if (affectedRows != 0)
                {
                    DeleteCoverImage(gameInDB.Cover);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }







        public bool CheckIfGameNameIsUnique(string gameName, int gameId)
        {
            return _context.Games.SingleOrDefault(g => g.Name == gameName) == null || _context.Games.SingleOrDefault(g => g.Name == gameName).Id == gameId;
        }


        /// <summary>
        /// Saves cover image on the hard disk of the server
        /// </summary>
        /// <param name="coverImage"></param>
        /// <returns>Name of the cover image with the extension to be saved directly on Database</returns>
        private async Task<string> SaveCoverImage(IFormFile coverImage)
        {
            string coverImageNameAndExtension = $"{Guid.NewGuid()}{Path.GetExtension(coverImage.FileName)}";

            string pathOfImageOnServerHardDisk = Path.Combine(_coverImagesFolderPath, coverImageNameAndExtension);

            using var creationStream = File.Create(pathOfImageOnServerHardDisk);
            await coverImage.CopyToAsync(creationStream); // Here the image has been saved inside the server's hard disk

            return coverImageNameAndExtension;

        }


        private void DeleteCoverImage(string coverImageNameWithExtension)
        {
            var oldCoverImageAbsolutePath = Path.Combine(_coverImagesFolderPath, coverImageNameWithExtension);
            File.Delete(oldCoverImageAbsolutePath);
        }
    }
}
