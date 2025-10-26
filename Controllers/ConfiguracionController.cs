using Farmacia.UI.Models.DTO.Configuracion;
using Farmacia.UI.Repositories.Implementation;
using Farmacia.UI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace Farmacia.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionRepository configuracionRepository;

        public ConfiguracionController(IConfiguracionRepository configuracionRepository)
        {
            this.configuracionRepository = configuracionRepository;
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [Route("UploadReporteAltas")]
        //[Authorize]
        public async Task<IActionResult> Post([FromForm] CargaReporte_Request model)
        {
            var dataTable = (new Helpers.Helpers()).ConvertCsvToList(model.csvFile);

            var response = await configuracionRepository.ImportarAltas(dataTable);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
           
        }

        [HttpGet]
        [Route("EnviaNotificacionCargaReporte")]
        //[Authorize]
        public async Task<IActionResult> EnviaNotificacionCargaReporte()
        {

            var response = await configuracionRepository.EnviaNotificacionCargaReporte();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }
    }
}
