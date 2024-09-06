using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;

namespace vladi.revolution.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayersService _service;

        public PlayersController(IPlayersService service)
        {
            _service = service;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl,FullName,BirtDate,Position,Biography")]Player player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }
            await _service.AddAsync(player);
            return RedirectToAction(nameof(Index));
        }

        // GET: Players/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            return View(playerDetails);
        }

        // GET: Players/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);
            if (playerDetails == null) return View("NotFound");
            return View(playerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureUrl,FullName,BirtDate,Position,Biography")] Player player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }
            await _service.UpdateAsync(id, player);
            return RedirectToAction(nameof(Index));
        }

        // GET: Players/Delete/{id}
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
    }
}
