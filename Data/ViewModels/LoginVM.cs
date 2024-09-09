using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Adresa de e-mail")]
        [Required(ErrorMessage = "Adresa de e-mail este obligatorie!")]
        public string EmailAddress { get; set; }

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Parola este obligatorie!")]
        public string Password { get; set; }
    }
}
