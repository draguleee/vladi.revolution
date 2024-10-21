using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace vladi.revolution.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _service;

        public StaffController(IStaffService service)
        {
            _service = service;
        }


        #region GET ALL STAFF MEMBERS

        [HttpGet]
        [Route("staff")]
        public async Task<IActionResult> Index(string format = "html")
        {
            var staffMembers = await _service.GetAllAsync();
            return format == "json" ? Json(staffMembers) : View(staffMembers);
        }

        #endregion

        #region FILTER PLAYERS

        [HttpGet]
        [Route("staff/filter")]
        public async Task<IActionResult> Filter(string searchString, string format = "html")
        {
            var allStaff = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = NormalizeString(searchString);
                var filteredPlayers = allStaff.Where(p => NormalizeString(p.FullName).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                return format == "json" ? Ok(filteredPlayers) : View("Index", filteredPlayers);
            }
            return format == "json" ? Ok(allStaff) : View("Index", allStaff);
        }

        #endregion

        #region CREATE PLAYER

        [HttpGet]
        [Route("staff/create")]
        public IActionResult Create(string format = "html")
        {
            var positions = GetPositionsList();
            return format == "json" ? Json(positions) : ViewWithPositions();
        }

        [HttpPost]
        [Route("staff/create")]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl,FullName,Function")] Staff staffMember, string[] Function, string format = "html")
        {
            if (!ModelState.IsValid) return format == "json" ? BadRequest(ModelState) : ViewWithPositions(staffMember);
            await _service.AddAsync(staffMember);
            return format == "json" ? Ok(staffMember) : RedirectToAction(nameof(Index));
        }

        #endregion

        #region PLAYER DETAILS

        [HttpGet]
        [Route("staff/details/{id}")]
        public async Task<IActionResult> Details(int id, string format = "html")
        {
            var staffMember = await _service.GetByIdAsync(id);
            if (staffMember == null) return format == "json" ? NotFound(new { message = "Staff member not found" }) : View("NotFound");
            return format == "json" ? Json(staffMember) : View(staffMember);
        }

        #endregion

        #region EDIT PLAYER

        [HttpGet]
        [Route("staff/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var staffMember = await _service.GetByIdAsync(id);
            if (staffMember == null) return View("NotFound");
            return ViewWithPositions(staffMember);
        }

        [HttpPost]
        [Route("staff/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureUrl,FullName,Function")] Staff staffMember, string[] Position, string format = "html")
        {
            if (!ModelState.IsValid) return format == "json" ? BadRequest(ModelState) : ViewWithPositions(staffMember);
            var existingStaffMember = await _service.GetByIdAsync(id);
            if (existingStaffMember == null) return format == "json" ? NotFound() : View("NotFound");
            UpdateStaffMemberDetails(existingStaffMember, staffMember);
            await _service.UpdateAsync(id, existingStaffMember);
            return format == "json" ? Ok(existingStaffMember) : RedirectToAction(nameof(Index));
        }

        #endregion

        #region DELETE PLAYER

        [HttpGet]
        [Route("staff/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var staffMember = await _service.GetByIdAsync(id);
            if (staffMember == null) return View("NotFound");
            return View(staffMember);
        }

        [HttpPost, ActionName("Delete")]
        [Route("staff/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id, string format = "html")
        {
            var staffMember = await _service.GetByIdAsync(id);
            if (staffMember == null) return format == "json" ? NotFound(new { message = "Staff member not found" }) : View("NotFound");
            await _service.DeleteAsync(id);
            return format == "json" ? Ok(new { message = "Staff member deleted successfully" }) : RedirectToAction(nameof(Index));
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
        private List<Roles> ParsePositions(string[] positions)
        {
            return positions.Select(p => Enum.Parse<Roles>(p)).ToList();
        }

        // Method to render the view with Positions data pre-populated
        private IActionResult ViewWithPositions(object model = null)
        {
            ViewBag.Positions = GetPositionsList();
            return View(model);
        }

        // Method to update the existing player's details
        private void UpdateStaffMemberDetails(Staff existingStaffMember, Staff staffMember)
        {
            existingStaffMember.ProfilePictureUrl = staffMember.ProfilePictureUrl;
            existingStaffMember.FullName = staffMember.FullName;
            existingStaffMember.Function = staffMember.Function;
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
