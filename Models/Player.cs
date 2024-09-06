using System.ComponentModel.DataAnnotations;
using vladi.revolution.Data.Enums;

namespace vladi.revolution.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Poză Profil")]
        [Required(ErrorMessage = "Selectează o poză!")]
        public required string ProfilePictureUrl { get; set; }

        [Display(Name = "Nume Complet")]
        [Required(ErrorMessage = "Numele complet este obligatoriu!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Numele complet trebuie să conțină între 5 și 50 de caractere!")]
        public required string FullName { get; set; }

        [Display(Name = "Data Nașterii")]
        [Required(ErrorMessage = "Data nașterii este obligatorie!")]
        public DateOnly BirthDate { get; set; }

        [Display(Name = "Poziție")]
        [Required(ErrorMessage = "Poziția jucătorului este obligatorie!")]
        public string Position { get; set; }

        [Display(Name = "Biografie")]
        [Required(ErrorMessage = "Biografia este obligatorie!")]
        public required string Biography { get; set; }   
    }
}
