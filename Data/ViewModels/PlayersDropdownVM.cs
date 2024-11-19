using vladi.revolution.Models;

namespace vladi.revolution.Data.ViewModels
{
    public class PlayersDropdownVM
    {
        public PlayersDropdownVM()
        {
            Players = new List<Player>();
        }

        public List<Player> Players { get; set; }
    }
}
