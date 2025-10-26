using Farmacia.UI.Data;
using Farmacia.UI.Models;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Models.DTO.Requerimiento;
using Farmacia.UI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.UI.Repositories.Implementation
{
    public class RequerimientoCataogoIIRepository : IRequerimientoCataogoIIRepository
    {
        private readonly FarmaciaContext context;
        public RequerimientoCataogoIIRepository(FarmaciaContext context) => this.context = context;
        public Task<ResponseModel> GetEstatus()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetRequerimientos(string? Nss, string? nombrePaciente, string? observaciones, string? diagnostico, string? clave)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                
                var result = await context.RequerimientoCatalogoIis.Include(x=>x.RequerimientoCatalogoIiarchivos).Include(x=>x.Estatus)
                   
                    .Select(s => new GetRequerimiento_Response()
                    {
                        id = s.Id,
                        nss = s.Nss,
                        folio = s.Folio,
                        nombre= s.NombrePaciente ?? "",
                        diagnostico = s.Diagnostico ?? "",
                        observaciones = s.Observaciones ?? "",
                        requerimientoMensual = s.RequerimientoMensual,
                        clave = s.ClaveMedicamento,
                        meses = s.Meses,
                        piezas = s.Piezas,
                        fechaVencimiento = s.FechaVencimiento,
                        fechaEvaluacion = s.FechaEvaluacion,
                        archivos = s.RequerimientoCatalogoIiarchivos.Select(x=> new GetRequerimientoArchivo_Response()
                        {
                            archivo = x.Archivo,
                            archivoId = x.Id
                        }).ToList()
                    })
                    .ToListAsync();


               
                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");
                
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> InsRequerimiento(InsRequerimiento_Request model, string userId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

                if (await this.context.RequerimientoCatalogoIis.Where(x => x.Folio == model.folio).CountAsync() > 0)
                {
                    rm.SetResponse(false, "El folio "+model.folio+" ya se encuentra registrado.");
                    return rm;
                }
                //validamos que el nss no tenga un folio vigente
                var solicitud = await this.context.RequerimientoCatalogoIis.Where(x => x.Nss == model.nss && x.FechaVencimiento > DateTime.Now && x.EstatusId == 1).FirstOrDefaultAsync();
                if (solicitud != null)
                {
                    rm.SetResponse(false, "El paciente ya tiene registrada una solicitud con folio: " + solicitud.Folio);
                    return rm;
                }

                RequerimientoCatalogoIi requerimientoCatalogoIi = new RequerimientoCatalogoIi()
                {
                    Id = Guid.NewGuid(),
                    Folio = model.folio,
                    Nss = model.nss,
                    NombrePaciente = model.nombrePaciente,
                    Diagnostico = model.diagnostico,    
                    Ooad ="U.M.A.E. Especialidades Nuevo León",
                    ClaveMedicamento = model.claveMedicamento,
                    RequerimientoMensual = model.requerimientoMensual,
                    Meses = model.meses,
                    Piezas = model.piezas,
                    FechaVencimiento = model.fechaVencimiento,
                    Observaciones = model.observaciones,
                    FechaEvaluacion = model.fechaEvaluacion,
                    EstatusId = 1,
                    Agregado = model.agregado,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacionId = Guid.Parse(userId),
                };

                await this.context.RequerimientoCatalogoIis.AddAsync(requerimientoCatalogoIi);

                if (model.archivo != null)
                {
                    var configuracion = await context.Configuracions.Where(x => x.Id == 8).FirstOrDefaultAsync();
                    int anio = model.fechaVencimiento.Year;
                    string ruta = configuracion.ValorString + "\\" + anio.ToString();
                    string archivo = "";

                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    Guid archivoId = Guid.NewGuid();

                    ruta = ruta + "\\" + archivoId.ToString().ToUpper();
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    archivo = ruta + "\\" + model.archivo.FileName;
                    using (FileStream fs = new FileStream(archivo, FileMode.Create))
                    {
                        await model.archivo.CopyToAsync(fs);
                    }

                    RequerimientoCatalogoIiarchivo requerimientoCatalogoIiarchivo = new RequerimientoCatalogoIiarchivo() {
                        Id = archivoId,
                        RequerimientoId = requerimientoCatalogoIi.Id,
                        Ruta = archivo,
                        Tipo = model.archivo.FileName,
                        Archivo = model.archivo.FileName,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = Guid.Parse(userId)
                    };

                    this.context.RequerimientoCatalogoIiarchivos.AddAsync(requerimientoCatalogoIiarchivo);
                }
                await context.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public Task<ResponseModel> UpdEstatusRequerimiento(int estatusId, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetArchivo(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var archivo = await this.context.RequerimientoCatalogoIiarchivos.FindAsync(id);

                string archivoRuta = archivo.Ruta;

                var fileContent = System.IO.File.ReadAllBytes(archivoRuta);

                rm.result = fileContent;

                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetArchivoById(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var archivo = await this.context.RequerimientoCatalogoIiarchivos.FindAsync(id);

                rm.result = archivo;

                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }
    }
}
