using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vladi.revolution.Data.Base;

namespace vladi.revolution.Models
{
    public class Accident : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Începutul perioadei: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly AccidentFrom { get; set; }

        [Display(Name = "Sfârșitul perioadei: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly AccidentTo { get; set; }

        [Display(Name = "Tipul accidentării")]
        public string AccidentType { get; set; }

        // Players
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

    }
}
