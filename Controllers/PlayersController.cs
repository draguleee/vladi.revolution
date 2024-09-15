using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace vladi.revolution.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayersService _service;

        public PlayersController(IPlayersService service)
        {
            _service = service;
        }


        #region GET ALL PLAYERS

        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> Index(string format = "html")
        {
            var players = await _service.GetAllAsync();
            return format == "json" ? Json(players) : View(players);
        }

        #endregion


        #region FILTER PLAYERS

        [HttpGet]
        [Route("players/filter")]
        public async Task<IActionResult> Filter(string searchString, string format = "html")
        {
            var allPlayers = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredPlayers = allPlayers.Where(p => NormalizeString(p.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                return format == "json" ? Ok(filteredPlayers) : View("Index", filteredPlayers);
            }
            return format == "json" ? Ok(allPlayers) : View("Index", allPlayers);
        }

        #endregion


        #region CREATE PLAYER

        [HttpGet]
        [Route("players/create")]
        public IActionResult Create(string format = "html")
        {
            var positions = GetPositionsList();
            return format == "json" ? Json(positions) : ViewWithPositions();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl,FullName,BirthDate,Position,ShirtNumber")] Player player, string[] Position)
        {
            if (!ModelState.IsValid) return ViewWithPositions(player);
            player.Position = ParsePositions(Position);
            await _service.AddAsync(player);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("players/createJson")]
        public async Task<IActionResult> CreateJson([FromBody] Player player, string[] Position)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddAsync(player);
            return Ok(player);
        }

        #endregion


        #region PLAYER DETAILS

        [HttpGet]
        [Route("players/details/{id}")]
        public async Task<IActionResult> Details(int id, string format = "html")
        {
            var player = await _service.GetByIdAsync(id);
            if (player == null)
                return format == "json" ? NotFound(new { message = "Player not found" }) : View("NotFound");
            return format == "json" ? Json(player) : View(player);
        }

        #endregion


        #region EDIT PLAYER

        [HttpGet]
        [Route("players/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await _service.GetByIdAsync(id);
            if (player == null) return View("NotFound");
            return ViewWithPositions(player);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureUrl,FullName,BirthDate,Position,ShirtNumber")] Player player, string[] Position)
        {
            if (!ModelState.IsValid) return ViewWithPositions(player);
            player.Position = ParsePositions(Position);
            await _service.UpdateAsync(id, player);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("players/editSuccess/{id}")]
        public async Task<IActionResult> EditJson(int id, [FromBody] Player player)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existingPlayer = await _service.GetByIdAsync(id);
            if (existingPlayer == null) return NotFound();
            UpdatePlayerDetails(existingPlayer, player);
            await _service.UpdateAsync(id, existingPlayer);
            return Ok(existingPlayer);
        }

        #endregion


        #region DELETE PLAYER

        [HttpGet]
        [Route("players/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _service.GetByIdAsync(id);
            if (player == null) return View("NotFound");
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [Route("players/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id, string format = "html")
        {
            var player = await _service.GetByIdAsync(id);
            if (player == null) return format == "json" ? NotFound(new { message = "Player not found" }) : View("NotFound");
            await _service.DeleteAsync(id);
            return format == "json" ? Ok(new { message = "Player deleted successfully" }) : RedirectToAction(nameof(Index));
        }

        #endregion


        #region HELPER METHODS

        // Method to fetch and format the Positions enum as SelectListItems
        private List<SelectListItem> GetPositionsList()
        {
            return Enum.GetValues(typeof(Positions))
                .Cast<Positions>()
                .Select(p => new SelectListItem
                {
                    Text = $"{p} - {(p.GetType().GetField(p.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? p.ToString())}",
                    Value = p.ToString()
                })
                .ToList();
        }

        // Method to parse Positions strings to enum
        private List<Positions> ParsePositions(string[] positions)
        {
            return positions.Select(p => Enum.Parse<Positions>(p)).ToList();
        }

        // Method to render the view with Positions data pre-populated
        private IActionResult ViewWithPositions(object model = null)
        {
            ViewBag.Positions = GetPositionsList();
            return View(model);
        }

        // Method to update the existing player's details
        private void UpdatePlayerDetails(Player existingPlayer, Player newPlayer)
        {
            existingPlayer.ProfilePictureUrl = newPlayer.ProfilePictureUrl;
            existingPlayer.FullName = newPlayer.FullName;
            existingPlayer.BirthDate = newPlayer.BirthDate;
            existingPlayer.Position = newPlayer.Position;
            existingPlayer.ShirtNumber = newPlayer.ShirtNumber;
        }

        // Method to normalize Romanian diacritics in a string
        private string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input
                .Replace("ă", "a").Replace("â", "a").Replace("î", "i")
                .Replace("ș", "s").Replace("ț", "t")
                .Replace("Ă", "A").Replace("Â", "A").Replace("Î", "I")
                .Replace("Ș", "S").Replace("Ț", "T");
        }

        #endregion

    }
}
