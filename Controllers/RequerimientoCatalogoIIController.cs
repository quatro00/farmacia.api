using Farmacia.UI.Helpers;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Models.DTO.Configuracion;
using Farmacia.UI.Models.DTO.Requerimiento;
using Farmacia.UI.Repositories.Implementation;
using Farmacia.UI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequerimientoCatalogoIIController : ControllerBase
    {
        private readonly IRequerimientoCataogoIIRepository requerimientoCataogoIIRepository;

        public RequerimientoCatalogoIIController(IRequerimientoCataogoIIRepository requerimientoCataogoIIRepository)
        {
            this.requerimientoCataogoIIRepository = requerimientoCataogoIIRepository;
        }

        [HttpGet]
        [Route("GetRequerimientos")]
        //[Authorize]
        public async Task<IActionResult> GetRequerimientos(string? Nss, string? nombrePaciente, string? observaciones, string? diagnostico, string? clave)
        {

            var response = await requerimientoCataogoIIRepository.GetRequerimientos(Nss,nombrePaciente,observaciones,diagnostico,clave);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpGet]
        [Route("GetEstatusRequerimientoCatalogoII")]
        //[Authorize]
        public async Task<IActionResult> GetEstatusRequerimientoCatalogoII()
        {

            var response = await requerimientoCataogoIIRepository.GetEstatus();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [Route("InsRequerimiento")]
        //[Authorize]
        public async Task<IActionResult> InsRequerimiento([FromForm] InsRequerimiento_Request model)
        {
            var response = await requerimientoCataogoIIRepository.InsRequerimiento(model,Guid.NewGuid().ToString());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);

        }

        [HttpPost]
        [Route("UpdEstatusRequerimiento")]
       // [Authorize]
        public async Task<IActionResult> UpdEstatusRequerimiento(int estatusId, Guid id)
        {
            var response = await requerimientoCataogoIIRepository.UpdEstatusRequerimiento(estatusId, id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
        [HttpGet]
        [Route("GetArchivo")]
        public async Task<IActionResult> GetArchivo(Guid id)
        {
            var archivoResponse = await requerimientoCataogoIIRepository.GetArchivoById(id);
            RequerimientoCatalogoIiarchivo archivo = archivoResponse.result;

            var response = await requerimientoCataogoIIRepository.GetArchivo(id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            string mimeType;
            string[] allowedExtensions = { ".xls", ".jpg", ".jpeg", ".pdf" };

            string extension = Path.GetExtension(archivo.Archivo);

            if (allowedExtensions.Contains(extension))
            {
                switch (extension.ToLower())
                {
                    case ".xls":
                        mimeType = "application/vnd.ms-excel";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        mimeType = "image/jpeg";
                        break;
                    case ".pdf":
                        mimeType = "application/pdf";
                        break;
                    default:
                        // Establecer un tipo MIME predeterminado si la extensión no es reconocida
                        mimeType = "application/octet-stream";
                        break;
                }
            }
            else
            {
                // Establecer un tipo MIME predeterminado si la extensión no está permitida
                mimeType = "application/octet-stream";
            }

            return File(response.result, mimeType, archivo.Archivo);
        }
    }
}
