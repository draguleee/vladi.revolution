using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Nume complet")]
        public string FullName { get; set; }
    }
}
