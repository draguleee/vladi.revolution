using System.ComponentModel.DataAnnotations;
using vladi.revolution.Data.Base;

namespace vladi.revolution.Models
{
    public class Match : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data meciului")]
        public DateOnly MatchDate { get; set; }

        [Display(Name = "Echipa gazdă")]
        public string HomeTeam { get; set; }

        [Display(Name = "Echipa oaspete")]
        public string AwayTeam { get; set; }

        [Display(Name = "Scor echipă gazdă")]
        public int? HomeTeamScore { get; set; }

        [Display(Name = "Scor echipă oaspete")]
        public int? AwayTeamScore { get; set; }

        // Relationships
        public List<PlayerMatch> PlayersMatches { get; set; }
        public List<Goal> Goals { get; set; } = new List<Goal>();
    }
}
