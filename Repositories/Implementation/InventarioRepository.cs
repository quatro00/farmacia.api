using Farmacia.UI.Data;
using Farmacia.UI.Models;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Models.DTO.Altas;
using Farmacia.UI.Models.DTO.Inventario;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.UI.Repositories.Implementation
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly FarmaciaContext context;
        public InventarioRepository(FarmaciaContext context) => this.context = context;
        public async Task<ResponseModel> CrearInventario(CrearInventario_Request model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validaciones
                //validamos que no haya inventarios en estatus generado o terminado
                
                string cveAdsc = "20EA25";
                int contInventarios = await context.Inventarios.Where(x => x.CveAdsc == cveAdsc && (x.EstatusInventario == 1 || x.EstatusInventario == 3 || x.EstatusInventario == 4)).CountAsync();
                
                if(contInventarios > 0)
                {
                    rm.SetResponse(false, "Existen inventarios disponibles o que no han sido aplicados.");
                    return rm;
                }
                Guid inventarioId = Guid.NewGuid();

                int cont = await context.Inventarios.Where(x=>x.CveAdsc == cveAdsc).CountAsync();
                cont = cont + 1;

                var configuracion = await context.Configuracions.Where(x => x.Id == 1).FirstOrDefaultAsync();

                DateTime fechainicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
                DateTime fechatermino = fechainicio.AddMonths(configuracion.ValorEntero ?? 0);

                //buscamos las claves que caducan en el rango de fechas
                var lotes = await context.Lotes.Where(x => x.Caducidad >= fechainicio && x.Caducidad <= fechatermino).ToListAsync();
                var claves = lotes.GroupBy(i => new { i.Gpo, i.Gen, i.Esp, i.Dif, i.Var })
                .Select(g => g.First())
                .ToList();

                List<string> clavesConteo = new List<string>();
                //List<Lote> lotesConteo = new List<Lote>();
                List<InventarioDetalle> inventarioDetalles = new List<InventarioDetalle>();

                foreach(var item in claves)
                {
                    if (clavesConteo.IndexOf($"{item.Gpo}.{item.Gen}.{item.Esp}.{item.Dif}.{item.Var}") == -1)
                    {
                        //si no se ha ingresado obtenemos los lotes de esa clave
                        clavesConteo.Add($"{item.Gpo}.{item.Gen}.{item.Esp}.{item.Dif}.{item.Var}");

                        var inventarioDetalle =
                            await context.Lotes
                            .Where(x =>
                            x.Disponible > 0 &&
                            x.Gpo == item.Gpo &&
                            x.Gen == item.Gen &&
                            x.Esp == item.Esp &&
                            x.Dif == item.Dif &&
                            x.Var == item.Var).Select(y => new InventarioDetalle() 
                            { 
                                InventarioId = inventarioId,
                                LoteId = y.Id,
                                DetalleId = 1,
                                Teorico = y.Disponible,
                                Consumo = 0,
                                Conteo = 0,
                                Aplicado = false
                            }).ToListAsync();

                        foreach(var detalle in inventarioDetalle)
                        {
                            var alta = await context.ImportAltas.Where(x => x.Gpo == item.Gpo && x.Gen == item.Gen && x.Esp == item.Esp && x.Dif == item.Dif && x.Var == item.Var).FirstOrDefaultAsync();

                            detalle.ClaveProveedor = alta.NumeroProveedor ?? "";
                            detalle.NombreProveedor = alta.RazonSocial ?? "";
                            detalle.Unidad = alta.UnidadPresentacion ?? "";
                            detalle.Descripcion = alta.DescripcionArticulo ?? "";

                            int notificacion = 0;
                            try
                            {
                                notificacion = await context.InventarioDetalles.Where(x => x.LoteId == detalle.LoteId).MaxAsync(x => x.NoNotificacion);
                            }
                            catch(Exception ioe)
                            {
                                notificacion = 0;
                            }
                            
                            detalle.NoNotificacion = notificacion + 1;
                        }
                        inventarioDetalles.AddRange(inventarioDetalle);
                    }
                }


                
                Inventario inventario = new Inventario()
                {
                    InventarioId = inventarioId,
                    Fecha = DateTime.Now,
                    Folio = cont,
                    CveAdsc = cveAdsc,
                    ResponsableConteo = model.responsableConteo,
                    ResponsableControPuesto = model.responsableConteoPuesto,
                    Generado = model.generado,
                    GeneradoPuesto = model.generadoPuesto,
                    Responsable = model.responsable,
                    ResponsablePuesto = model.responsablePuesto,
                    EstatusInventario = 1,
                    FechaCreacion = DateTime.Now,
                    InventarioDetalles = inventarioDetalles
                };

                await context.Inventarios.AddAsync(inventario);
                await context.SaveChangesAsync();

                rm.result = true;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetControlCaducidades(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                GetControlInventario_Response response = new GetControlInventario_Response();

                response = 
                    await context.Inventarios.Include("InventarioDetalles").Include("InventarioDetalles.Lote").Where(x => x.InventarioId == id)
                    .Select(x=> new GetControlInventario_Response()
                    {
                        inventarioId = x.InventarioId,
                        vigenteAPartir = x.FechaCreacion,
                        proximaRevision = x.FechaCreacion,
                        fecha = x.FechaCreacion,
                        sustituye = "",
                        paginas = "",
                        folio = x.Fecha.Year.ToString() + "-" + x.Folio.ToString(),   
                        detalle = x.InventarioDetalles.Select(y=> new GetControlInventarioDetalle_Response()
                        {
                            inventarioId = y.InventarioId,
                            loteId = y.LoteId,
                            caducidad = y.Lote.Caducidad,
                            cantidadRecibida = y.Lote.Cantidad,
                            claveArticulo = y.Lote.Gpo + "." + y.Lote.Gen + "." + y.Lote.Esp + "." + y.Lote.Dif + "." + y.Lote.Var,
                            lote = y.Lote.Lote1,
                            unidad = y.Unidad,
                            existenciaFisica = y.Lote.Disponible,
                            consumo = y.Consumo,
                            descripcion = y.Descripcion,
                            numProveedor = y.ClaveProveedor,
                            razonSocial = y.NombreProveedor,
                            noNotificacion = y.NoNotificacion
                        }).OrderBy(x=>x.claveArticulo).ToList()
                    })
                    .FirstOrDefaultAsync();
                var inventario = await context.Inventarios.Include("InventarioDetalles").Include("InventarioDetalles.Lote").Where(x=>x.InventarioId == id).ToListAsync();
                rm.result = response;
                rm.SetResponse(true, "");
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetInventarios()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetInventarios_Response> getInventarios = new List<GetInventarios_Response>();
                getInventarios = await context.Inventarios.Include(x=>x.EstatusInventarioNavigation).Select(x=> new GetInventarios_Response()
                {
                    inventarioId = x.InventarioId,
                    fecha = x.Fecha,
                    folio = x.Folio,
                    cveAdsc = x.CveAdsc,
                    responsableConteo = x.ResponsableConteo,
                    responsableConteoPuesto = x.ResponsableControPuesto,
                    generado = x.Generado,
                    generadoPuesto = x.GeneradoPuesto,
                    responsable = x.Responsable,
                    responsablePuesto = x.ResponsablePuesto,
                    estatusInventarioId = x.EstatusInventario,
                    estatusInventario = x.EstatusInventarioNavigation.Descripcion,
                    fechaCreacion = x.FechaCreacion,
                    fechaTermino = x.FechaTermino,
                }).ToListAsync();
                rm.result = getInventarios;
                rm.SetResponse(true, "");
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> RegistraConteo(List<RegistraConteo_Request> model)
        {
            ResponseModel rm = new ResponseModel();
            Guid inventarioId = new Guid(); ;
            try
            {
                foreach (var item in model)
                {
                    inventarioId = item.inventarioId;
                    var detalle = await context.InventarioDetalles.Where(x => x.InventarioId == item.inventarioId && x.LoteId == item.loteId).FirstOrDefaultAsync();
                    if(item.conteo > detalle.Teorico)
                    {
                        rm.SetResponse(false, "Existen conteos mayores al inventario teorico, favor de validar Lote: " + item.loteId +", teorico: " + detalle.Teorico +", conteo: "+item.conteo+".");
                        return rm;
                    }
                    rm.SetResponse(true);
                }


                foreach (var item in model)
                {
                    inventarioId = item.inventarioId;
                    await context.InventarioDetalles.Where(x => x.InventarioId == item.inventarioId && x.LoteId == item.loteId).ExecuteUpdateAsync(
                    s => s
                    .SetProperty(t => t.Conteo, item.conteo)
                    .SetProperty(t => t.Consumo, t=>t.Teorico - item.conteo)
                    );
                    rm.SetResponse(true);
                }

                await context.Inventarios.Where(x=>x.InventarioId == inventarioId).ExecuteUpdateAsync(
                    s => s
                    .SetProperty(t => t.EstatusInventario, 3) //Cambiamos a estatus de conteo TERMINADO
                    );

                await context.SaveChangesAsync();
                rm.SetResponse(true, "");
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> UploadControlInventarios(UploadControlInventarios_Request model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var inventario = await context.Inventarios.Where(x => x.InventarioId == model.inventarioId).FirstOrDefaultAsync();
                var configuracion = await context.Configuracions.Where(x => x.Id == 4).FirstOrDefaultAsync();
                int anio = inventario.Fecha.Year;
                //buscamos si existe el folder del año
                string ruta = configuracion.ValorString + "\\" + anio.ToString();
                string archivo = "";
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                ruta = ruta + "\\" + model.inventarioId.ToString().ToUpper();
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                archivo = ruta + "\\" + model.archivo.FileName;
                using (FileStream fs = new FileStream(archivo, FileMode.Create))
                {
                    await model.archivo.CopyToAsync(fs);
                }
                //ejecutamos la actualizacion del inventario disponible
                List<InventarioDetalle> inventarioDetalles = await context.InventarioDetalles.Where(x => x.InventarioId == model.inventarioId).ToListAsync();
                foreach(var item in inventarioDetalles)
                {
                   await context.Lotes.Where(x => x.Id == item.LoteId).ExecuteUpdateAsync(
                   s => s
                   .SetProperty(t => t.Disponible, item.Conteo) //Cambiamos a estatus de conteo TERMINADO
                   );
                }

                //actualizamos la ubicacion del lote
                await context.Inventarios.Where(x => x.InventarioId == model.inventarioId).ExecuteUpdateAsync(
                   s => s
                   .SetProperty(t => t.ControlDeCaducidades, archivo) //Cambiamos a estatus de conteo TERMINADO
                   .SetProperty(t => t.EstatusInventario, 5)
                   .SetProperty(t => t.FechaTermino, DateTime.Now)
                   );

              
                await context.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
        public async Task<ResponseModel> GetReporte(Guid inventarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var inventario = await context.Inventarios.FindAsync(inventarioId);

                var fileContent = System.IO.File.ReadAllBytes(inventario.ControlDeCaducidades ?? "");

                rm.result = fileContent;

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
