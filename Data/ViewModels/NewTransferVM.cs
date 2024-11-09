using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using vladi.revolution.Models;

namespace vladi.revolution.Data.ViewModels
{
    public class NewTransferVM
    {
        public int Id { get; set; }

        [Display(Name = "Selectează jucător")]
        [Required(ErrorMessage = "Jucătorul este obligatoriu!")]
        public int PlayerId { get; set; }

        [Display(Name = "Nr.")]
        [Required(ErrorMessage = "Numărul este obligatoriu!")]
        public int TransferNumber { get; set; }

        [Display(Name = "Data transferului")]
        [Required(ErrorMessage = "Selectează data transferului!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly TransferDate { get; set; }

        [Display(Name = "S-a transferat de la: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public string TransferFrom { get; set; }

        [Display(Name = "S-a transferat la: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public string TransferTo { get; set; }
        
    }
}
