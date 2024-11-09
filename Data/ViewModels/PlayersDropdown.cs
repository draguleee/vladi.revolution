using vladi.revolution.Models;

namespace vladi.revolution.Data.ViewModels
{
    public class PlayersDropdown
    {
        public PlayersDropdown()
        {
            Players = new List<Player>();
        }

        public List<Player> Players { get; set; }
    }
}
