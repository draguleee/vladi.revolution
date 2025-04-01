using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(string sortOrder = "asc", string format = "html")
        {
            ViewData["CurrentSortOrder"] = sortOrder;
            var players = await _service.GetAllAsync();
            players = sortOrder.ToLower() == "desc"
                ? players.OrderByDescending(p => p.FullName).ToList()
                : players.OrderBy(p => p.FullName).ToList();
            return format == "json" ? Json(players) : View(players);
        }

        #endregion

        #region GET ALL ACTIVE PLAYERS

        [HttpGet]
        [Route("activeplayers")]
        public async Task<IActionResult> IndexActive(string sortOrder = "asc", string format = "html")
        {
            ViewData["CurrentSortOrder"] = sortOrder;
            var players = (await _service.GetAllPlayersWithTransfersAsync()).ToList();
            players = players
                .Where(p =>
                    p.Transfers == null ||
                    !p.Transfers.Any() ||
                    p.Transfers.All(t =>
                        t.TransferTo != null &&
                        t.TransferTo.Trim().Equals("Vladi Revolution", StringComparison.OrdinalIgnoreCase)))
                .ToList();
            players = sortOrder.ToLower() == "desc"
                ? players.OrderByDescending(p => p.FullName).ToList()
                : players.OrderBy(p => p.FullName).ToList();
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

        #region FILTER ACTIVE PLAYERS

        [HttpGet]
        [Route("players/filteractiveplayers")]
        public async Task<IActionResult> FilterActive(string searchString, string format = "html")
        {
            var allPlayers = (await _service.GetAllPlayersWithTransfersAsync()).ToList();
            var activePlayers = allPlayers
                .Where(p =>
                    p.Transfers == null ||
                    !p.Transfers.Any() ||
                    p.Transfers.All(t =>
                        t.TransferTo != null &&
                        t.TransferTo.Trim().Equals("Vladi Revolution", StringComparison.OrdinalIgnoreCase)))
                .ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                activePlayers = activePlayers
                    .Where(p => NormalizeString(p.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return format == "json" ? Ok(activePlayers) : View("IndexActive", activePlayers);
        }
        #endregion


        #region CREATE PLAYER

        [HttpGet]
        [Route("players/create")]
        public IActionResult Create(string format = "html")
        {
            var positions = GetPositionsList();
            ViewBag.Positions = positions;
            var viewModel = new NewPlayerVM
            {
                Position = new List<Positions>()
            };
            return format == "json" ? Json(viewModel) : View(viewModel);
        }

        [HttpPost]
        [Route("players/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewPlayerVM viewModel, string format = "html")
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = GetPositionsList();
                return format == "json" ? BadRequest(ModelState) : View(viewModel);
            }

            await _service.AddNewPlayerAsync(viewModel);
            return format == "json" ? Ok(viewModel) : RedirectToAction(nameof(Index));
        }

        #endregion

        #region PLAYER DETAILS

        [HttpGet]
        [Route("players/details/{id}")]
        public async Task<IActionResult> Details(int id, string format = "html")
        {
            var player = await _service.GetPlayerByIdAsync(id);
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
            var player = await _service.GetPlayerForEditAsync(id);
            if (player == null) return View("NotFound");
            ViewBag.Positions = GetPositionsList();
            return View(player);
        }

        [HttpPost]
        [Route("players/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewPlayerVM viewModel, string format = "html")
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = GetPositionsList();
                return format == "json" ? BadRequest(ModelState) : View(viewModel);
            }

            await _service.UpdatePlayerAsync(id, viewModel);
            return format == "json" ? Ok(viewModel) : RedirectToAction(nameof(Index));
        }

        #endregion

        #region DELETE PLAYER

        [HttpGet]
        [Route("players/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _service.GetPlayerByIdAsync(id);
            if (player == null) return View("NotFound");
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [Route("players/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string format = "html")
        {
            var player = await _service.GetPlayerByIdAsync(id);
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
