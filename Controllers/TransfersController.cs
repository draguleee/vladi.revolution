using Microsoft.AspNetCore.Mvc;
using vladi.revolution.Data.Services.Interfaces;

namespace vladi.revolution.Controllers
{
    public class TransfersController : Controller
    {
        private readonly ITransfersService _service;

        public TransfersController(ITransfersService service)
        {
            _service = service;
        }


        #region GET ALL PLAYERS

        [HttpGet]
        [Route("transfers")]
        public async Task<IActionResult> Index(string format = "html")
        {
            var transfers = await _service.GetAllAsync();
            return format == "json" ? Json(transfers) : View(transfers);
        }

        #endregion
    }
}
