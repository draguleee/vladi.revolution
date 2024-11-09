using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using vladi.revolution.Models;

namespace vladi.revolution.Data.ViewModels
{
    public class NewAccidentVM
    {
        public int Id { get; set; }

        [Display(Name = "Selectează jucător")]
        [Required(ErrorMessage = "Jucătorul este obligatoriu!")]
        public int PlayerId { get; set; }

        [Display(Name = "Indisponibil de la data de: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public DateOnly AccidentFrom { get; set; }

        [Display(Name = "Indisponibil până la data de: ")]
        [Required(ErrorMessage = "Acest câmp este obligatoriu!")]
        public DateOnly AccidentTo { get; set; }

        [Display(Name = "Tipul accidentării")]
        [Required(ErrorMessage = "Tipul accidentării este obligatoriu!")]
        public string AccidentType { get; set; }

    }
}
