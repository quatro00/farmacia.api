using Farmacia.UI.Models;
using Farmacia.UI.Models.DTO.Inventario;

namespace Farmacia.UI.Repositories.Interface
{
    public interface IInventarioRepository
    {
        Task<ResponseModel> GetInventarios();
        Task<ResponseModel> CrearInventario(CrearInventario_Request model);
        Task<ResponseModel> GetControlCaducidades(Guid id);
        Task<ResponseModel> RegistraConteo(List<RegistraConteo_Request> model);
        Task<ResponseModel> UploadControlInventarios(UploadControlInventarios_Request model);
        Task<ResponseModel> GetReporte(Guid inventarioId);
    }
}
