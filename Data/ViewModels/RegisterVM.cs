using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Numele complet")]
        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string FullName { get; set; }

        [Display(Name = "Adresa de e-mail")]
        [Required(ErrorMessage = "Adresa de e-mail este obligatorie!")]
        [EmailAddress(ErrorMessage = "Te rugăm să introduci o adresă de e-mail validă.")]
        public string EmailAddress { get; set; }

        [Display(Name = "Parola (min. 6 caractere, numere incluse)")]
        [Required(ErrorMessage = "Parola este obligatorie!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parola trebuie să conțină cel puțin 6 caractere.")]
        public string Password { get; set; }

        [Display(Name = "Confirmă parola")]
        [Required(ErrorMessage = "Confirmarea parolei este obligatorie!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolele nu coincid!")]
        public string ConfirmPassword { get; set; }
    }
}
