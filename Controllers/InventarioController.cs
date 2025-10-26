using Farmacia.UI.Helpers;
using Farmacia.UI.Models.DTO.Altas;
using Farmacia.UI.Models.DTO.Inventario;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Repositories.Implementation;
using Farmacia.UI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository inventarioRepository;

        public InventarioController(IInventarioRepository inventarioRepository)
        {
            this.inventarioRepository = inventarioRepository;
        }

        [HttpPost]
        [Route("CrearInventario")]
        //[Authorize]
        public async Task<IActionResult> CrearInventario([FromBody] CrearInventario_Request request)
        {
            
            var response = await inventarioRepository.CrearInventario(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("GetInventarios")]
        //[Authorize]
        public async Task<IActionResult> GetInventarios()
        {

            var response = await inventarioRepository.GetInventarios();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet("GetControlCaducidadesConteo/{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetControlCaducidadesConteo([FromRoute] Guid id)
        {
            PdfFormater pdfFormater = new PdfFormater();
            var response = await inventarioRepository.GetControlCaducidades(id);
            GetControlInventario_Response model = response.result;
           
            var pdfBytes = pdfFormater.Formato_RegistroControlCaducidades(model);

            return File(pdfBytes, "application/pdf", $"ControlCaducidades_{model.folio}.pdf");
        }

        [HttpGet("GetReporteInventario/{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetReporteInventario([FromRoute] Guid id)
        {
            var response = await inventarioRepository.GetReporte(id);

            

            return File(response.result, "application/pdf", "ControlCaducidades.pdf");
        }

        [HttpGet("GetControlCaducidadesLleno/{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetControlCaducidadesLleno([FromRoute] Guid id)
        {
            PdfFormater pdfFormater = new PdfFormater();
            var response = await inventarioRepository.GetControlCaducidades(id);
            GetControlInventario_Response model = response.result;

            var pdfBytes = pdfFormater.Formato_RegistroControlCaducidadesLleno(model);

            return File(pdfBytes, "application/pdf", $"ControlCaducidades_{model.folio}.pdf");
        }

        [HttpGet("GetInventario")]
        //[Authorize]
        public async Task<IActionResult> GetInventario(Guid id)
        {
            var response = await inventarioRepository.GetControlCaducidades(id);
            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
        [HttpPost]
        [Route("RegistraConteo")]
        //[Authorize]
        public async Task<IActionResult> RegistraConteo([FromBody] List<RegistraConteo_Request> request)
        {

            var response = await inventarioRepository.RegistraConteo(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [Route("UploadControlInventarios")]
        //[Authorize]
        public async Task<IActionResult> UploadControlInventarios([FromForm] UploadControlInventarios_Request request)
        {
            var response = await inventarioRepository.UploadControlInventarios(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
