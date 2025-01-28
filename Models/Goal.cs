using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vladi.revolution.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Scorer))]
        public int? ScorerId { get; set; } 
        public Player Scorer { get; set; }

        [ForeignKey(nameof(Assister))]
        public int? AssisterId { get; set; } 
        public Player? Assister { get; set; }

        [Required]
        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; } 
        public Match Match { get; set; }
    }
}
