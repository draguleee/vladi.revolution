using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Controllers
{
    public class AccidentsController : Controller
    {
        private readonly IAccidentsService _service;

        public AccidentsController(IAccidentsService service)
        {
            _service = service;
        }

        #region GET ALL ACCIDENTS

        [HttpGet]
        [Route("accidents")]
        public async Task<IActionResult> Index(string format = "html")
        {
            var accidents = await _service.GetAllAccidentsWithPlayersAsync();
            return format == "json" ? Json(accidents) : View(accidents);
        }

        #endregion


        #region FILTER ALL ACCIDENTS

        [HttpGet]
        [Route("accidents/filter")]
        public async Task<IActionResult> Filter(string searchString, string format = "html")
        {
            var allAccidents = await _service.GetAllAccidentsWithPlayersAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredAccidents = allAccidents
                    .Where(t => NormalizeString(t.Player.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return format == "json" ? Json(filteredAccidents) : View("Index", filteredAccidents);
            }
            return format == "json" ? Json(allAccidents) : View("Index", allAccidents);
        }

        #endregion


        #region CREATE TRANSFER

        [HttpGet]
        [Route("accidents/create")]
        public async Task<IActionResult> Create()
        {
            var transferDropdownData = await _service.GetNewAccidentDropdownsValues();
            ViewBag.Players = new SelectList(transferDropdownData.Players, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [Route("accidents/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewAccidentVM accident)
        {
            if (!ModelState.IsValid)
            {
                var accidentDropdownData = await _service.GetNewAccidentDropdownsValues();
                ViewBag.Player = new SelectList(accidentDropdownData.Players, "Id", "FullName");
                return View(accident);
            }
            await _service.AddNewAccidentAsync(accident);
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region HELPER METHODS

        // Method to update the existing player's details
        private void UpdatePlayerDetails(Player existingPlayer, Player newPlayer)
        {
            existingPlayer.ProfilePictureUrl = newPlayer.ProfilePictureUrl;
            existingPlayer.FullName = newPlayer.FullName;
            existingPlayer.BirthDate = newPlayer.BirthDate;
            existingPlayer.Position = newPlayer.Position;
            existingPlayer.ShirtNumber = newPlayer.ShirtNumber;
            existingPlayer.FacebookAccount = newPlayer.FacebookAccount;
            existingPlayer.InstagramAccount = newPlayer.InstagramAccount;
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
