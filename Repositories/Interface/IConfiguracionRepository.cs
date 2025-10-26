using Farmacia.UI.Models;
using System.Data;

namespace Farmacia.UI.Repositories.Interface
{
    public interface IConfiguracionRepository
    {
        Task<ResponseModel> ImportarAltas(DataTable importAltas);
        Task<ResponseModel> EnviaNotificacionCargaReporte();
    }
}
