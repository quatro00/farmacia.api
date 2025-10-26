using Farmacia.UI.Data;
using Farmacia.UI.Models;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Models.DTO.Altas;
using Farmacia.UI.Models.DTO.Lotes;
using Farmacia.UI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;

namespace Farmacia.UI.Repositories.Implementation
{
    public class AltasRepository : IAltasRepository
    {
        private readonly FarmaciaContext context;
        public AltasRepository(FarmaciaContext context) => this.context = context;

        public async Task<ResponseModel> EliminarArchivo(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                await context.AltaArchivos.Where(x => x.Id == id).ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetAltas(DateTime desde, DateTime hasta)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var importAltas = await context.ImportAltas
                    .Where(e =>
                e.FechaAlta >= desde &&
                e.FechaAlta <= hasta
                )
                    .Select(s => new GetAltasPendientes_Result()
                    {
                        id = s.Id,
                        anio = s.Anio ?? 0,
                        numeroAltaContable = s.NumeroAltaContable ?? "",
                        gpo = s.Gpo ?? "",
                        gen = s.Gen ?? "",
                        esp = s.Esp ?? "",
                        dif = s.Dif ?? "",
                        var = s.Var ?? "",
                        descripcionArticulo = s.DescripcionArticulo ?? "",
                        fechaHoraAlta = s.FechaHoraAlta ?? "",
                        numeroProveedor = s.NumeroProveedor ?? "",
                        rfcProveedor = s.RfcProveedor ?? "",
                        razonSocial = s.RazonSocial ?? "",
                        recepcion = Decimal.Parse(s.CantidadConteo ?? "0"),
                        cantidad = Decimal.Parse(s.CantidadAutorizada ?? "0"),
                    })
                    .ToListAsync();

                
                foreach(var item in importAltas)
                {
                    var alta = await context.AltaArchivos.Where(x=>x.Anio == item.anio && x.AltaContable == item.numeroAltaContable).FirstOrDefaultAsync();
                    if(alta != null)
                    {
                        item.idArchivo = alta.Id;
                        item.nombreArchivo = alta.Archivo;
                    }
                }
                rm.result = importAltas;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public Task<ResponseModel> GetCaptura(string alta)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> UploadFile(InsArchivo_Request model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                int anio = model.anio;
                //buscamos si existe el folder del año
                string ruta = "c:\\Altas\\"+anio.ToString();
                string archivo = "";
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                ruta = ruta + "\\" + model.altaContable;
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                archivo = ruta + "\\" + model.archivo.FileName;
                using (FileStream fs = new FileStream(archivo, FileMode.Create))
                {
                    await model.archivo.CopyToAsync(fs);
                }

                AltaArchivo altaArchivo = new AltaArchivo()
                {
                    Id= Guid.NewGuid(),
                    Anio = model.anio,
                    AltaContable = model.altaContable,
                    Archivo = model.archivo.FileName.ToString(),
                    RutaArchivo = archivo
                };
                
                await context.AltaArchivos.AddAsync(altaArchivo);
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
