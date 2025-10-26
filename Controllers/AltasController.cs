using Farmacia.UI.Models.DTO.Altas;
using Farmacia.UI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AltasController : ControllerBase
    {
        private readonly IAltasRepository altasRepository;

        public AltasController(IAltasRepository altasRepository)
        {
            this.altasRepository = altasRepository;
        }

        [HttpGet]
        [Route("GetAltas")]
        //[Authorize]
        public async Task<IActionResult> GetAltas(DateTime desde, DateTime hasta)
        {
            var response = await altasRepository.GetAltas(desde, hasta);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("GetCaptura")]
        //[Authorize]
        public async Task<IActionResult> GetCaptura(DateTime desde, DateTime hasta)
        {
            var response = await altasRepository.GetAltas(desde, hasta);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [Route("UploadFile")]
        //[Authorize]
        public async Task<IActionResult> UploadFile([FromForm] InsArchivo_Request request)
        {
            var response = await altasRepository.UploadFile(request);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("EliminarArchivo")]
        //[Authorize("Administrador")]
        public async Task<IActionResult> EliminarArchivo([FromBody] BorrarArchivo_Request model)
        {
            var response = await altasRepository.EliminarArchivo(model.id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

    }
}
