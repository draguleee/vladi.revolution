using System.ComponentModel.DataAnnotations;
using vladi.revolution.Models;

namespace vladi.revolution.Data.ViewModels
{
    public class NewMatchVM
    {
        public int Id { get; set; }

        [Display(Name = "Data meciului")]
        [Required(ErrorMessage = "Data meciului este obligatorie!")]
        public DateOnly MatchDate { get; set; }

        [Display(Name = "Echipa gazdă")]
        [Required(ErrorMessage = "Numele echipei gazdă este obligatoriu!")]
        public string HomeTeam { get; set; }

        [Display(Name = "Echipa oaspete")]
        [Required(ErrorMessage = "Numele echipei oaspete este obligatoriu!")]
        public string AwayTeam { get; set; }

        [Display(Name = "Scor echipă gazdă")]
        public int? HomeTeamScore { get; set; }

        [Display(Name = "Scor echipă oaspete")]
        public int? AwayTeamScore { get; set; }

        // Relationships
        [Display(Name = "Selectează jucători...")]
        [Required(ErrorMessage = "Selectează jucătorii!")]
        public List<int> PlayerIds { get; set; }
    }
}
