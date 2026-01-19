using System.Diagnostics;

namespace HallOfGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesService _gamesService;
        private readonly IConfiguration _config;

        public HomeController(IGamesService gamesService, IConfiguration config)
        {
            _gamesService = gamesService;
            _config = config;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();

            ViewData["GameCoverImagesPath"] = _config["GamesCoverImagesSavingPath"];

            return View(games);
        }
    }
}
