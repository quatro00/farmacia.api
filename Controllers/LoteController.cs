using Farmacia.UI.Models.DTO.Configuracion;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteController : ControllerBase
    {
        private readonly ILoteRepository loteRepository;

        public LoteController(ILoteRepository loteRepository)
        {
            this.loteRepository = loteRepository;
        }

        [HttpGet]
        [Route("GetAltasPendientes")]
        //[Authorize]
        public async Task<IActionResult> Get()
        {
            
            var response = await loteRepository.GetAltasPendientes();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetMedicamentosCaducos")]
        //[Authorize]
        public async Task<IActionResult> GetMedicamentosCaducos()
        {

            var response = await loteRepository.GetMedicamentosCaducos();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetDiasDesdeUltimoInventario")]
        //[Authorize]
        public async Task<IActionResult> GetDiasDesdeUltimoInventario()
        {

            var response = await loteRepository.GetDiasDesdeUltimoInventario();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetCaducidadDashboard")]
        //[Authorize]
        public async Task<IActionResult> GetCaducidadDashboard()
        {

            var response = await loteRepository.GetCaducidadDashboard();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetMedicamentosCaducosDetalle")]
        //[Authorize]
        public async Task<IActionResult> GetMedicamentosCaducosDetalle()
        {

            var response = await loteRepository.GetMedicamentosCaducosDetalle();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetInventario")]
        //[Authorize]
        public async Task<IActionResult> GetInventario()
        {

            var response = await loteRepository.GetInventario();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpPost]
        [Route("GuardarLotes")]
        //[Authorize]
        public async Task<IActionResult> Post([FromBody] List<InsLotes_Request> request)
        {

            var response = await loteRepository.InsLotes(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }
    }
}
