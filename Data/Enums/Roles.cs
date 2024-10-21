using System.ComponentModel.DataAnnotations;

namespace vladi.revolution.Data.Enums
{
    public enum Roles
    {
        [Display(Name = "Președinte")]
        Presedinte,

        [Display(Name = "Vicepreședinte")]
        Vicepresedinte,

        [Display(Name = "Antrenor")]
        Antrenor,

        [Display(Name = "Antrenor Secund")]
        Antrenor_Secund,

        [Display(Name = "Medic")]
        Medic,

        [Display(Name = "Asistent")]
        Asistent,

        [Display(Name = "Manager Tehnic")]
        Manager_Tehnic
    }
}
