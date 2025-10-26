using Farmacia.UI.Data;
using Farmacia.UI.Models;
using Farmacia.UI.Models.Domain;
using Farmacia.UI.Repositories.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Farmacia.UI.Utils;
using Microsoft.Identity.Client;

namespace Farmacia.UI.Repositories.Implementation
{
    public class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly FarmaciaContext context;
        private readonly IConfiguration configuration;
        private readonly ILoteRepository loteRepository;

        public ConfiguracionRepository(FarmaciaContext context, IConfiguration configuration, ILoteRepository loteRepository)
        {
            this.context = context;
            this.configuration = configuration;
            this.loteRepository = loteRepository;
        } 

        public async Task<ResponseModel> EnviaNotificacionCargaReporte()
        {
            ResponseModel rm = new ResponseModel();
            
            try
            {

                string clientId = configuration["AzureAd:ClientId"];
                string tenantId = configuration["AzureAd:TenantId"];
                string clientSecret = configuration["AzureAd:ClientSecret"];
                string smtpHost = configuration["Smtp:Host"];
                int smtpPort = int.Parse(configuration["Smtp:Port"]!);


                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
        .WithClientSecret(clientSecret)
        .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
        .Build();

                string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

                // Obtener el token de acceso
                AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                string token = result.AccessToken;

                // Configura el cliente SMTP
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("umae25.adquisiciones@outlook.com", token)
                };

                // Crea el mensaje
                string correo = new Html(configuration["Smtp:RestorePasswordTemplate"]!).Parse();
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress("umae25.adquisiciones@outlook.com"),
                    Subject = "Estatus sistema farmacia",
                    Body = correo,
                    IsBodyHtml = true
                };

                mailMessage.To.Add("josecarlosgarciadiaz@gmail.com");

                smtpClient.Send(mailMessage);
            }
            catch(Exception ex) {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }

           
            
            /*
            try
            {
                //Buscamos la ultima fecha de carga de reportes
                Configuracion configuracion_fechaCarga = await context.Configuracions.FindAsync(7);

                //Buscamos los correos a notificar
                Configuracion configuracion_correosNotificar = await context.Configuracions.FindAsync(6);
                string[] correos = (configuracion_correosNotificar.ValorString ?? "").Split(',');
                //Buscamos el limite de dias para el envio de notificaciones
                Configuracion configuracion_diasTolerancia = await context.Configuracions.FindAsync(5);

                DateTime fechaCarga = (DateTime)configuracion_fechaCarga.ValorDate;
                fechaCarga = new DateTime(fechaCarga.Year, fechaCarga.Month, fechaCarga.Day);

                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                
                TimeSpan diferencia = fechaActual - fechaCarga;
                int diasDiferencia = diferencia.Days;
                int diasTolerancia = (int)configuracion_diasTolerancia.ValorEntero;

                //enviamos el correo
                if(diasDiferencia > diasTolerancia)
                {
                    SmtpClient smtpClient = new SmtpClient()
                    {
                        Host = configuration["Smtp:Host"]!,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(configuration["Smtp:Email"], configuration["Smtp:Password"]),
                        Port = int.Parse(configuration["Smtp:Port"]!),
                    };

                    smtpClient.UseDefaultCredentials = false;

                    string correo = new Html(configuration["Smtp:RestorePasswordTemplate"]!).Parse();
                    correo = correo.Replace("{FechaUltimaActualizacion}", fechaCarga.ToString("dd/MM/yyyy"));

                    
                    
                    ResponseModel altasPendientes_rm = await loteRepository.GetAltasPendientes();
                    List <GetAltasPendiente> altasPendientes = altasPendientes_rm.result;
                    correo = correo.Replace("{CantidadEntradasPendientes}", altasPendientes.Count.ToString("N"));

                    ResponseModel medicamentosCaducos_rm = await loteRepository.GetMedicamentosCaducos();
                    decimal medicamentosCaducos = medicamentosCaducos_rm.result;
                    correo = correo.Replace("{CantidadMedicamentosCaducos}", medicamentosCaducos.ToString("N"));

                    MailMessage mailMessage = new MailMessage()
                    {
                        From = new MailAddress(configuration["Smtp:Email"]!),
                        Subject = "Estatus sistema farmacia",
                        Body = correo,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add("josecarlosgarciadiaz@gmail.com");

                    foreach (var item in correos)
                    {
                        mailMessage.To.Add(item);
                    }


                    smtpClient.Send(mailMessage);

                    rm.SetResponse(true);

                }

                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            */
            return rm;
        }

        public async Task<ResponseModel> ImportarAltas(DataTable importAltas)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //obtenemos los distintos años en el datatable
                var anios = importAltas.AsEnumerable()
               .Select(row => DateTime.Parse(row.Field<string>("FECHA HORA ALTA")))
               .Select(date => date.Year)
               .Distinct();

                if(anios.Count() > 1)
                {
                    rm.SetResponse(false, "No es posible trabajar con un archivo que contenta altas de años diferentes.");
                    return rm;
                }

                int anio = anios.FirstOrDefault();

                //borramos las altas del año
                await context.ImportAltas.Where(x => x.Anio == anios.FirstOrDefault()).ExecuteDeleteAsync();


                //insertamos las altas registradas
                List<ImportAlta> altas = new List<ImportAlta>();
                foreach(DataRow item in importAltas.Rows)
                {
                    altas.Add(new ImportAlta()
                    {
                        Id = Guid.NewGuid(),
                        Anio = anio,
                        FechaAlta = DateTime.ParseExact(item[35].ToString().Substring(0,10), "dd/MM/yyyy", CultureInfo.InvariantCulture),

                        ClasPtalOrigen = item[0].ToString(),
                        NombreOoad = item[1].ToString(),
                        FechaRegistro = item[2].ToString(),
                        ClasPtalUnidadEntrega = item[3].ToString(),
                        NombreUnidadEntrega = item[4].ToString(),
                        TipoReporte = item[5].ToString(),
                        NumeroAltaContable = item[6].ToString(),
                        NumeroDeDocumento = item[7].ToString(),
                        NumeroDeReposicion = item[8].ToString(),
                        Cargoa = item[9].ToString(),
                        Creditoa = item[10].ToString(),
                        DescripcionMovimiento = item[11].ToString(),
                        Gpo = item[12].ToString(),
                        Gen = item[13].ToString(),
                        Esp = item[14].ToString(),
                        Dif = item[15].ToString(),
                        Var = item[16].ToString(),
                        DescripcionArticulo = item[17].ToString(),
                        UnidadPresentacion = item[18].ToString(),
                        CantidadPresentacion = item[19].ToString(),
                        TipoPresentacion = item[20].ToString(),
                        PrecioCatalogoArticulos = item[21].ToString(),
                        PrecioCompra = item[22].ToString(),
                        Iva = item[23].ToString(),
                        CantidadAutorizada = item[24].ToString(),
                        CantidadConteo = item[25].ToString(),
                        ImporteArticuloSinIva = item[26].ToString(),
                        ImporteAltaConIva = item[27].ToString(),
                        LineaArticulo = item[28].ToString(),
                        RfcProveedor = item[29].ToString(),
                        NumeroProveedor = item[30].ToString(),
                        RazonSocial = item[31].ToString(),
                        NumeroLicitacion = item[32].ToString(),
                        FechaHoraRecepcion = item[33].ToString(),
                        FechaHoraEntrega = item[34].ToString(),
                        FechaHoraAlta = item[35].ToString(),
                        PartidaPresupuestal = item[36].ToString(),
                        TipoError = item[37].ToString(),
                        Enviado = item[38].ToString(),
                        FechaEnvioPrei = item[39].ToString(),
                    });
                }
                
                //borramos todas las altas correspondiente al año enviado
                
                await context.ImportAltas.Where(x => x.Anio == anio).ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                await context.ImportAltas.AddRangeAsync(altas);
                await context.SaveChangesAsync();

                //actualizamos las configuraciones
                Configuracion configuracion_fechaCarga = await context.Configuracions.FindAsync(7);
                configuracion_fechaCarga.ValorDate = DateTime.Now;
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
