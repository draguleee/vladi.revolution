using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;
using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.ViewModels; 

namespace vladi.revolution.Controllers
{
    public class TransfersController : Controller
    {
        private readonly ITransfersService _service;

        public TransfersController(ITransfersService service)
        {
            _service = service;
        }


        #region GET ALL TRANSFERS

        [HttpGet]
        [Route("transfers")]
        public async Task<IActionResult> Index(string format = "html")
        {
            var transfers = await _service.GetAllTransfersWithPlayersAsync();
            return format == "json" ? Json(transfers) : View(transfers);
        }

        #endregion


        #region FILTER TRANSFERS

        [HttpGet]
        [Route("transfers/filter")]
        public async Task<IActionResult> Filter(string searchString, string format = "html")
        {
            var allTransfers = await _service.GetAllTransfersWithPlayersAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredTransfers = allTransfers
                    .Where(t => NormalizeString(t.Player.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return format == "json" ? Json(filteredTransfers) : View("Index", filteredTransfers);
            }
            return format == "json" ? Json(allTransfers) : View("Index", allTransfers);
        }

        #endregion


        #region CREATE TRANSFER

        [HttpGet]
        [Route("transfers/create")]
        public async Task<IActionResult> Create()
        {
            var transferDropdownData = await _service.GetNewTransferDropdownsValues();
            ViewBag.Players = new SelectList(transferDropdownData.Players, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [Route("transfers/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewTransferVM transfer)
        {
            if (!ModelState.IsValid)
            {
                var transferDropdownData = await _service.GetNewTransferDropdownsValues();
                ViewBag.Player = new SelectList(transferDropdownData.Players, "Id", "FullName");
                return View(transfer);
            }
            await _service.AddNewTransferAsync(transfer);
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region EDIT TRANSFER

        [HttpGet]
        [Route("transfers/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var transfer = await _service.GetTransferByIdAsync(id);
            if (transfer == null) return View("NotFound");
            var transferDropdownData = await _service.GetNewTransferDropdownsValues();
            ViewBag.Players = new SelectList(transferDropdownData.Players, "Id", "FullName");
            var transferVM = new NewTransferVM
            {
                PlayerId = transfer.Player.Id,
                TransferDate = transfer.TransferDate,
                TransferFrom = transfer.TransferFrom,
                TransferTo = transfer.TransferTo
            };
            return View(transferVM);
        }

        [HttpPost]
        [Route("transfers/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewTransferVM transferVM)
        {
            if (!ModelState.IsValid)
            {
                var transferDropdownData = await _service.GetNewTransferDropdownsValues();
                ViewBag.Players = new SelectList(transferDropdownData.Players, "Id", "FullName");
                return View(transferVM);
            }
            await _service.UpdateTransferAsync(id, transferVM);
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region DELETE PLAYER

        [HttpGet]
        [Route("transfers/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transfer = await _service.GetTransferByIdAsync(id);
            if (transfer == null) return View("NotFound");
            return View(transfer);
        }

        [HttpPost, ActionName("Delete")]
        [Route("transfers/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id, string format = "html")
        {
            var transfer = await _service.GetTransferByIdAsync(id);
            if (transfer == null) return format == "json" ? NotFound(new { message = "Transfer not found" }) : View("NotFound");
            await _service.DeleteAsync(id);
            return format == "json" ? Ok(new { message = "Transfer deleted successfully" }) : RedirectToAction(nameof(Index));
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
