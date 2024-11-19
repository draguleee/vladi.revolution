using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Services.Classes;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchesService _service;

        public MatchesController(IMatchesService service)
        {
            _service = service;
        }


        #region GET ALL MATCHES

        [HttpGet]
        [Route("matches")]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllMatchesWithPlayersAsync(); 
            return View(data);
        }

        #endregion


        #region CREATE MATCH

        [HttpGet]
        [Route("matches/create")]
        public async Task<IActionResult> Create()
        {
            var matchDropdownData = await _service.GetPlayersDropdownsValues();
            ViewBag.Players = new SelectList(matchDropdownData.Players, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [Route("matches/create")]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            if (!ModelState.IsValid)
            {
                var matchDropdownData = await _service.GetPlayersDropdownsValues();
                ViewBag.Players = new SelectList(matchDropdownData.Players, "Id", "FullName");
                return View(match);
            }
            await _service.AddNewMatchAsync(match);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region EDIT MATCH

        [HttpGet]
        [Route("matches/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null) return View("NotFound");
            return View(match);
        }

        #endregion


        #region MATCH DETAILS

        [HttpGet]
        [Route("match/details/{id}")]
        public async Task<IActionResult> Details(int id, string format = "html")
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null)
                return format == "json" ? NotFound(new { message = "Player not found" }) : View("NotFound");

            return format == "json" ? Json(match) : View(match);
        }

        #endregion

        #region DELETE MATCH

        [HttpGet]
        [Route("matches/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null) return View("NotFound");
            return View(match);
        }

        [HttpPost, ActionName("Delete")]
        [Route("matches/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id, string format = "html")
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null) return format == "json" ? NotFound(new { message = "Match not found" }) : View("NotFound");
            await _service.DeleteAsync(id);
            return format == "json" ? Ok(new { message = "Match deleted successfully" }) : RedirectToAction(nameof(Index));
        }

        #endregion

        #region HELPER METHODS

        // Method to update the existing player's details
        private void UpdateMatchDetails(Match existingMatch, Match newMatch)
        {
            existingMatch.MatchDate = newMatch.MatchDate;
            existingMatch.HomeTeam = newMatch.HomeTeam;
            existingMatch.AwayTeam = newMatch.AwayTeam;
            existingMatch.HomeTeamScore = newMatch.HomeTeamScore;
            existingMatch.AwayTeamScore = newMatch.AwayTeamScore;
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
