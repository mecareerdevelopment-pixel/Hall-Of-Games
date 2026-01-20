namespace HallOfGames.ViewModels
{
    public class EditGameViewModel
    {
        [ValidateNever]
        public int gameId { get; set; }

        [Display(Name = "Game Name")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Game Name Is Required.")]
        [MaxLength(200, ErrorMessage = "Max Allowed = 200 chars")]
        [MinLength(3, ErrorMessage = "Min Allowed = 3 chars")]
        [Remote("ValidateGameName", "Games", ErrorMessage = "This game name is already exists in database!. Can not have duplicate games with the same name.", AdditionalFields = "gameId")]
        [UniqueName]
        public string Name { get; set; } = default!;


        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; } = default!;      // To show categories inside the view



        [Display(Name = "Game Category")]
        [Required(ErrorMessage = "Game must have a category")]
        public int? CategoryId { get; set; }     // To catch the selected category from the view


        [ValidateNever]
        public IEnumerable<SelectListItem> Devices { get; set; } = default!;     // To show devices inside the view



        [Display(Name = "Compatabile/Supported Devices")]
        [Required(ErrorMessage = "Game must have some compatabile/supported devices")]
        public List<int> CompatabileDevicesIds { get; set; } = null!;



        [Display(Name = "Game Description")]
        [MaxLength(2000, ErrorMessage = "Maximum number of allowed characters is 2000")]
        public string? Description { get; set; }


        [ValidateNever]
        public string UploadedCoverImageFullPath { get; set; } = null!;     // To carry the uploaded file to the view


        [Display(Name = "Game Cover Image (Upload if and only if it is required to be changed)")]
        [DataType(DataType.Upload)]
        [GameCoverImage]
        // HAS to be not required
        public IFormFile? Cover { get; set; } = default!;        // To catch the uploaded file to and from the view
    }
}
