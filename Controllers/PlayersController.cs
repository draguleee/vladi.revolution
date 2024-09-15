using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;
using System;
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

        /** ---------------------------- GET ALL PLAYERS ---------------------------- **/

        // Get all the players in the Index.cshtml view
        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // Get all the players in JSON format
        [HttpGet]
        [Route("api/players")]
        public async Task<IActionResult> IndexJson()
        {
            var data = await _service.GetAllAsync();
            return Json(data);
        }


        /** ---------------------------- FILTER PLAYERS BY NAME ---------------------------- **/

        // Filter the players by name and display them in the Index.cshtml view
        [HttpGet]
        [Route("players/filter")]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allPlayers = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredResult = allPlayers.Where(n => NormalizeString(n.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                return View("Index", filteredResult);
            }
            return View("Index", allPlayers);
        }

        // Filter the players by name and display them in JSON format
        [HttpGet]
        [Route("api/players/filter")]
        public async Task<IActionResult> FilterJson(string searchString)
        {
            var allPlayers = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredResult = allPlayers.Where(n => NormalizeString(n.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                return Ok(filteredResult);
            }
            return Ok(allPlayers);
        }

        // Helper function to replace Romanian diacritics with standard Latin characters
        private string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return input
                .Replace("ă", "a")
                .Replace("â", "a")
                .Replace("î", "i")
                .Replace("ș", "s")
                .Replace("ț", "t")
                .Replace("Ă", "A")
                .Replace("Â", "A")
                .Replace("Î", "I")
                .Replace("Ș", "S")
                .Replace("Ț", "T");
        }


        /** ---------------------------- CREATE A NEW PLAYER ---------------------------- **/

        // Display the Create.cshtml form
        [HttpGet]
        [Route("players/create")]
        public IActionResult Create()
        {
            ViewBag.Positions = Enum.GetValues(typeof(Positions))
                .Cast<Positions>()
                .Select(p => new SelectListItem
                {
                    Text = $"{p.ToString()} - {(p.GetType().GetField(p.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? p.ToString())}",
                    Value = p.ToString()
                }).ToList();

            return View();
        }

        // Display the Create.cshtml form (precisely, the Positions) in JSON format
        [HttpGet]
        [Route("api/players/create")]
        public IActionResult CreateJson()
        {
            var positions = Enum.GetValues(typeof(Positions))
                .Cast<Positions>()
                .Select(p => new
                {
                    Text = $"{p.ToString()} - {(p.GetType().GetField(p.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? p.ToString())}",
                    Value = p.ToString()
                })
                .ToList();
            return Json(positions);
        }

        // Create a new player 
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl,FullName,BirthDate,Position,ShirtNumber")] Player player, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = Enum.GetValues(typeof(Positions))
                    .Cast<Positions>()
                    .Select(p => new SelectListItem
                    {
                        Text = p.ToString(),
                        Value = p.ToString()
                    }).ToList();
                return View(player);
            }
            player.Position = Position.Select(p => Enum.Parse<Positions>(p)).ToList();
            await _service.AddAsync(player);
            return RedirectToAction(nameof(Index));
        }

        // Create a new player in JSON format
        [HttpPost]
        [Route("api/players/create")]
        public async Task<IActionResult> CreateJson([FromBody] Player player, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            await _service.AddAsync(player);
            return Ok(player); 
        }


        /** ---------------------------- GET PLAYER'S DETAILS ---------------------------- **/

        // Get player's details in Details.cshtml view
        [HttpGet]
        [Route("players/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            return View(playerDetails);
        }

        // Get player's details in JSON format
        [HttpGet]
        [Route("api/players/details/{id}")]
        public async Task<IActionResult> DetailsJson(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            return Json(playerDetails);
        }


        /** ---------------------------- EDIT PLAYER'S DETAILS ---------------------------- **/

        // Edit player's details in Edit.cshtml view
        [HttpGet]
        [Route("players/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            ViewBag.Positions = Enum.GetValues(typeof(Positions))
                .Cast<Positions>()
                .Select(p => new SelectListItem
                {
                    Text = p.ToString(),
                    Value = p.ToString(),
                    Selected = playerDetails.Position.Contains(p)
                }).ToList();
            return View(playerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureUrl,FullName,BirthDate,Position,ShirtNumber")] Player player, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = Enum.GetValues(typeof(Positions))
                    .Cast<Positions>()
                    .Select(p => new SelectListItem
                    {
                        Text = p.ToString(),
                        Value = p.ToString(),
                        Selected = Position.Contains(p.ToString())
                    }).ToList();
                return View(player);
            }
            player.Position = Position.Select(p => Enum.Parse<Positions>(p)).ToList();
            await _service.UpdateAsync(id, player);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("api/players/edit/{id}")]
        public async Task<IActionResult> EditJson(int id, [FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingPlayer = await _service.GetByIdAsync(id);
            if (existingPlayer == null)
            {
                return NotFound(); 
            }
            existingPlayer.ProfilePictureUrl = player.ProfilePictureUrl;
            existingPlayer.FullName = player.FullName;
            existingPlayer.BirthDate = player.BirthDate;
            existingPlayer.Position = player.Position; 
            existingPlayer.ShirtNumber = player.ShirtNumber;

            await _service.UpdateAsync(id, existingPlayer);
            return Ok(existingPlayer); 
        }



        /** ---------------------------- DELETE PLAYER ---------------------------- **/

        [HttpGet]
        [Route("players/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            return View(playerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Route("api/players/{id}")]
        public async Task<IActionResult> DeleteConfirmedJson(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return NotFound(new { message = "Player not found" });
            await _service.DeleteAsync(id);
            return Ok(new { message = "Player deleted successfully" });
        }
    }
}
