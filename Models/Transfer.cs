using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vladi.revolution.Data.Base;

namespace vladi.revolution.Models
{
    public class Transfer : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nr.")]
        public int TransferNumber { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly TransferDate { get; set; }

        [Display(Name = "S-a transferat de la: ")]
        public string TransferFrom { get; set; }

        [Display(Name = "S-a transferat la: ")]
        public string TransferTo { get; set; }

        // Players
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

    }
}
