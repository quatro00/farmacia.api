using Farmacia.UI.Models;
using Farmacia.UI.Models.DTO.Requerimiento;

namespace Farmacia.UI.Repositories.Interface
{
    public interface IRequerimientoCataogoIIRepository
    {
        Task<ResponseModel> GetRequerimientos(string? Nss, string? nombrePaciente, string? observaciones, string? diagnostico, string? clave);
        Task<ResponseModel> InsRequerimiento(InsRequerimiento_Request model, string userId);
        Task<ResponseModel> UpdEstatusRequerimiento(int estatusId, Guid id);
        Task<ResponseModel> GetEstatus();
        Task<ResponseModel> GetArchivo(Guid id);
        Task<ResponseModel> GetArchivoById(Guid id);
    }
}
