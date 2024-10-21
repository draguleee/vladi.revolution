using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Poză Profil")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Nume Complet")]        
        public string FullName { get; set; }

        [Display(Name = "Funcție")]
        public string Function { get; set; }
    }
}
