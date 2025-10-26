using Farmacia.UI.Data;
using Farmacia.UI.Models;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Repositories.Interface;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml;

namespace Farmacia.UI.Repositories.Implementation
{
    public class LoteRepository : ILoteRepository
    {
        private readonly FarmaciaContext context;
        public LoteRepository(FarmaciaContext context) => this.context = context;
        public async Task<ResponseModel> GetAltasPendientes()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var importAltas = await context.GetAltasPendientes.FromSqlRaw("EXEC dbo.SPQ_GetAltasPendientes").ToListAsync();
                rm.result = importAltas;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado." + ex.InnerException.Message + "," + ex.Message);
            }
            return rm;
        }

        public async Task<ResponseModel> GetCaducidadDashboard()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                GetCaducidadDashboard_Response response = new GetCaducidadDashboard_Response();

                DateTime fechaActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // = DateTime.Today;

                // Agregar los próximos 14 días
                for (int i = 1; i <= 12; i++)
                {
                    response.dias.Add(fechaActual.ToString("MMMM/yy", new System.Globalization.CultureInfo("es-ES")));
                    //buscamos los que caducan en esa fecha
                    response.disponible.Add(context.Lotes.Where(x => x.Caducidad.Month == fechaActual.Month).Sum(x => x.Disponible));


                    fechaActual = fechaActual.AddMonths(1);
                }



                rm.result = response;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetDiasDesdeUltimoInventario()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var fechaUltimoInventario = await context.Inventarios.Where(x => x.EstatusInventario == 5).MaxAsync(x=>x.FechaTermino);
                int diasUltimoInventario = 0;
                if(fechaUltimoInventario != null)
                {
                    TimeSpan diferencia = DateTime.Now.Subtract((DateTime)fechaUltimoInventario);

                    // Obtener los días de diferencia
                    diasUltimoInventario = diferencia.Days;
                }

                rm.result = diasUltimoInventario;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado." + ex.InnerException.Message + "," + ex.Message);
            }
            return rm;
        }

        public async Task<ResponseModel> GetInventario()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetMedicamentosCaducos_Request> result = new List<GetMedicamentosCaducos_Request>();

                var medicamentosCaducos = await context.Lotes.Where(x => x.Disponible > 0).Select(x => new GetMedicamentosCaducos_Request()
                {
                    loteId = x.Id,
                    gpo = x.Gpo,
                    gen = x.Gen,
                    esp = x.Esp,
                    dif = x.Dif,
                    var = x.Var,
                    lote = x.Lote1,
                    descripcion = "",
                    fechaCaducidad = x.Caducidad,
                    cantidad = x.Disponible,
                    tieneCartaCanje = x.TieneCartaCanje
                }).ToListAsync();

                foreach (var item in medicamentosCaducos)
                {
                    var itm = await context.ImportAltas.Where(x =>
                    x.Gpo == item.gpo &&
                    x.Gen == item.gen &&
                    x.Esp == item.esp &&
                    x.Dif == item.dif &&
                    x.Var == item.var).FirstOrDefaultAsync();

                    item.descripcion = itm.DescripcionArticulo ?? "";
                }
                rm.result = medicamentosCaducos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado." + ex.InnerException.Message + "," + ex.Message);
            }
            return rm;
        }

        public async Task<ResponseModel> GetMedicamentosCaducos()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var medicamentosCaducos = await context.Lotes.Where(x=>x.Caducidad <= DateTime.Now && x.Disponible > 0).SumAsync(x=>x.Disponible);
                rm.result = medicamentosCaducos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado." + ex.InnerException.Message + "," + ex.Message);
            }
            return rm;
        }

        public async Task<ResponseModel> GetMedicamentosCaducosDetalle()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetMedicamentosCaducos_Request> result = new List<GetMedicamentosCaducos_Request>();

                var medicamentosCaducos = await context.Lotes.Where(x => x.Caducidad <= DateTime.Now && x.Disponible > 0).Select(x=> new GetMedicamentosCaducos_Request()
                {
                    loteId = x.Id,
                    gpo = x.Gpo,
                    gen = x.Gen,
                    esp = x.Esp,
                    dif = x.Dif,
                    var = x.Var,
                    lote = x.Lote1,
                    descripcion = "",
                    fechaCaducidad = x.Caducidad,
                    cantidad = x.Disponible,
                    tieneCartaCanje = x.TieneCartaCanje,
                    alta = x.NumeroAltaContable
                }).ToListAsync();

                foreach(var item in medicamentosCaducos)
                {
                    var itm = await context.ImportAltas.Where(x =>
                    x.Gpo == item.gpo &&
                    x.Gen == item.gen &&
                    x.Esp == item.esp &&
                    x.Dif == item.dif &&
                    x.Var == item.var).FirstOrDefaultAsync();

                    item.descripcion = itm.DescripcionArticulo ?? "";
                }
                rm.result = medicamentosCaducos;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado." + ex.InnerException.Message + "," + ex.Message);
            }
            return rm;
        }

        public async Task<ResponseModel> InsLotes(List<InsLotes_Request> model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validaciones
                string alta = model.FirstOrDefault().numeroAltaContable;
                int anio = model.FirstOrDefault().anio;

                //validamos que las cantidades correspondan al total de las altas
                List<string> claves = new List<string>();
                
                foreach(var item in model)
                {
                    if (!claves.Contains(item.clave)) 
                    {
                        claves.Add(item.clave);
                    }

                    if(item.tieneCartaCanje == null)
                    {
                        rm.SetResponse(false, "Favor de indicar si tiene carta canje.");
                        return rm;
                    }
                }

                foreach(var clave in claves)
                {
                    decimal total = 0;
                    decimal entregado = 0;
                    string[] claveArray = clave.Split('.');

                    var lotes = model.Where(x => x.clave == clave).ToList();
                    var registrado = lotes.Sum(x => x.cantidad);


                    var entregas = await context.ImportAltas.Where(x =>
                    x.Anio == anio &&
                    x.NumeroAltaContable == alta &&
                    x.Gpo == claveArray[0] &&
                    x.Gen == claveArray[1] &&
                    x.Esp == claveArray[2] &&
                    x.Dif == claveArray[3] &&
                    x.Var == claveArray[4] 
                    ).ToListAsync();

                    foreach(var entrega in entregas)
                    {
                        entregado = decimal.Parse(entrega.CantidadConteo ?? "0") + entregado;
                    }

                    if(entregado != registrado)
                    {
                        rm.SetResponse(false, "La cantidad registrada no corresponde con la cantidad entregada para la clave " + clave + ".");
                        return rm;
                    }

                    
                    
                }

                List<Lote> lotesLst = new List<Lote>();
                foreach (var item in model)
                {
                    string[] cveArray = item.clave.Split('.');

                    bool tieneCartaCanje = false;
                    if(item.tieneCartaCanje == 1)
                    {
                        tieneCartaCanje = true;
                    }
                    lotesLst.Add(new Lote()
                    {
                        Id = Guid.NewGuid(),
                        Anio = item.anio,
                        NumeroAltaContable = item.numeroAltaContable,
                        Gpo = cveArray[0],
                        Gen = cveArray[1],
                        Esp = cveArray[2],
                        Dif = cveArray[3],
                        Var = cveArray[4],
                        Lote1 = item.lote,
                        Caducidad = item.caducidad,
                        Cantidad = item.cantidad,
                        Disponible = item.cantidad,
                        TieneCartaCanje = tieneCartaCanje
                    });
                }

                await context.Lotes.AddRangeAsync(lotesLst);
                await context.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
    }
}
