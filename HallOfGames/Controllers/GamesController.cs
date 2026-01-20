using HallOfGames.ViewModels;
using System.IO;

namespace HallOfGames.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;
        private readonly IConfiguration _staticValues;

        public GamesController(ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gamesService, IConfiguration staticValues)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
            _staticValues = staticValues;
        }

        public IActionResult Index([FromServices] IConfiguration _config)
        {
            var games = _gamesService.GetAll();

            ViewData["GameCoverImagesPath"] = _config["GamesCoverImagesSavingPath"];

            return View(games);
        }


        public IActionResult Details(int gameId)
        {
            var game = _gamesService.GetById(gameId);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoriesService.GetAllCategoriesAsSelectList();
            var devices = _devicesService.GetAllDevicesAsSelectList();
           
            CreateGameViewModel viewModel = new()
            {
                Categories = categories,
                Devices = devices
            };

            ViewData["allowedExtensions"] = _staticValues.GetSection("AllowedCoverImageExtenstions").Get<string[]>();
            ViewData["MaxSize"] = _staticValues["MaxGameCoverImageSizeMegaBytes"];
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = _categoriesService.GetAllCategoriesAsSelectList();
                var devices = _devicesService.GetAllDevicesAsSelectList();

                viewModel.Categories = categories;
                viewModel.Devices = devices;

                ViewData["allowedExtensions"] = _staticValues.GetSection("AllowedCoverImageExtenstions").Get<string[]>();
                ViewData["MaxSize"] = _staticValues["MaxGameCoverImageSizeMegaBytes"];
                return View(viewModel);
            }

            await _gamesService.Create(viewModel);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int gameId)
        {
            var game = _gamesService.GetById(gameId);

            if (game == null)
            {
                return NotFound();
            }

            var categories = _categoriesService.GetAllCategoriesAsSelectList();
            var devices = _devicesService.GetAllDevicesAsSelectList(); 

            EditGameViewModel viewModel = new()
            {
                gameId = game.Id,
                Name = game.Name,
                CategoryId = game.CategoryId,
                CompatabileDevicesIds = game.CompatibleDevices.Select(d => d.DeviceId).ToList(),
                Description = game.Description,
                Categories = categories,
                Devices = devices,
                UploadedCoverImageFullPath = $"~{_staticValues["GamesCoverImagesSavingPath"]}/{game.Cover}"
            };
            
            ViewData["allowedExtensions"] = _staticValues.GetSection("AllowedCoverImageExtenstions").Get<string[]>();
            ViewData["MaxSize"] = _staticValues["MaxGameCoverImageSizeMegaBytes"];
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = _categoriesService.GetAllCategoriesAsSelectList();
                var devices = _devicesService.GetAllDevicesAsSelectList();

                viewModel.Categories = categories;
                viewModel.Devices = devices;

                ViewData["allowedExtensions"] = _staticValues.GetSection("AllowedCoverImageExtenstions").Get<string[]>();
                ViewData["MaxSize"] = _staticValues["MaxGameCoverImageSizeMegaBytes"];
                return View(viewModel);
            }

            var updatedGame = await _gamesService.Edit(viewModel);

            if (updatedGame == null)
            {
                // unsuccessful update due to wrong id
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpDelete]
        public IActionResult Delete(int gameId)
        {
            var isDeleted = _gamesService.Delete(gameId);

            return isDeleted ? Ok("ok") : BadRequest();
            
        }



        public IActionResult ValidateGameName(string Name, int gameId)
        {
            return Json(((GamesService)_gamesService).CheckIfGameNameIsUnique(Name, gameId));
        }
    }
}
