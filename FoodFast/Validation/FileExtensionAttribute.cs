using System.ComponentModel.DataAnnotations;

namespace FastFood.DAL.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
       

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    var extentsion = Path.GetExtension(file.FileName);
                    string[] extensions = { "jpg", "png", "jpeg" };
                    bool result = extensions.Any(x => extentsion.EndsWith(x));
                    if (!result)
                    {
                        return new ValidationResult("Allowed extensions are jpg or png or jpeg");
                    }
                }
                return ValidationResult.Success;
            }
        
    }
}
