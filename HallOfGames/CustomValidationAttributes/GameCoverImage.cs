using System.ComponentModel.DataAnnotations;

namespace HallOfGames.CustomValidationAttributes
{
    public class GameCoverImage : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var coverImage = value as IFormFile;

            var staticValues = validationContext.GetService<IConfiguration>();

            if (coverImage != null)
            {
                string[] allowedExtensions = staticValues!.GetSection("AllowedCoverImageExtenstions").Get<string[]>()!;
                
                if (!allowedExtensions.Contains(Path.GetExtension(coverImage.FileName), StringComparer.OrdinalIgnoreCase))
                {
                    return new ValidationResult($"Only {string.Join(", ", allowedExtensions)} are the allowed cover image extenstions");
                }


                int maxAllowedImageSize = Convert.ToInt32(staticValues["MaxGameCoverImageSizeMegaBytes"]);

                if (coverImage.Length > maxAllowedImageSize * 1024 * 1024)
                {
                    return new ValidationResult($"Maximum size allowed for Game Cover Image is {maxAllowedImageSize} MegaBytes, uploaded image is {Math.Round(coverImage.Length / 1024f / 1024, 2)} MegaBytes");
                }
            }

            return ValidationResult.Success;    // if null, another validation should deal with it
        }
    }
}
