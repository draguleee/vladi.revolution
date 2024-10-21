using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Models
{
    public class Transfers
    {
        [Key]
        [Display(Name ="Nr.")]
        public int Id { get; set; }

        [Display(Name = "Nume Jucător")]
        [Required(ErrorMessage = "Numele jucătorului este obligatoriu!")]
        public required string FullName { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "Data este obligatorie!")]
        public required DateOnly TransferDate { get; set; }

        [Display(Name = "S-a transferat de la: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public required string TransferFrom { get; set; }

        [Display(Name = "S-a transferat la: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public required string TransferTo { get; set; }

    }
}
