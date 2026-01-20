using HallOfGames.ViewModels;

namespace HallOfGames.CustomValidationAttributes
{
    public class UniqueNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult($"Enter game name.");
            }

            var context = validationContext.GetService<ApplicationDbContext>();

            if (validationContext.ObjectInstance is CreateGameViewModel && context.Games.Any(g => g.Name == value.ToString()) || validationContext.ObjectInstance is EditGameViewModel && context.Games.Any(g => g.Name == value.ToString()) && context.Games.Single(g => g.Name == value.ToString()).Id != (validationContext.ObjectInstance as EditGameViewModel)?.gameId)
            {
                return new ValidationResult($"This game name is already exists in database!. Can not have duplicate games with the same name.");

            }

            return ValidationResult.Success;
        }
    }
}
