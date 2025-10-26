using Farmacia.UI.Models;
using Farmacia.UI.Models.DTO.Altas;

namespace Farmacia.UI.Repositories.Interface
{
    public interface IAltasRepository
    {
        Task<ResponseModel> GetAltas(DateTime desde, DateTime hasta);
        Task<ResponseModel> UploadFile(InsArchivo_Request model);
        Task<ResponseModel> EliminarArchivo(Guid id);
        Task<ResponseModel> GetCaptura(string alta);
    }
}
