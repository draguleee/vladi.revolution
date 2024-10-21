using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Data.Enums
{
    public enum Positions
    {
        [Display(Name = "Portar")]
        GK,

        [Display(Name = "Fundaș")]
        CB,

        [Display(Name = "Mijlocaș")]
        CM,

        [Display(Name = "Atacant")]
        ST,

        [Display(Name = "Antrenor Secund")]
        AS,

        [Display(Name = "Antrenor")]
        A
    }
}