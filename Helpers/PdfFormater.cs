using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Barcodes;
using iText.IO.Font.Constants;
using Farmacia.UI.Models.DTO.Inventario;

namespace Farmacia.UI.Helpers
{
    public class PdfFormater
    {
        public byte[] Formato_RegistroControlCaducidades(GetControlInventario_Response model)
        {
            //String imageFile = "C:/logo_benavides.jpg";
            try
            {
                int fontSize = 6;
                int fontSizeDetalle = 6;
                PdfFont arialFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                using (MemoryStream stream = new MemoryStream())
                {
                    // Crear un nuevo documento PDF
                    //string rutaFuente = System.IO.Path.Combine("Temp", "MADETOMMY.otf");
                    //PdfFont fuentePersonalizada = PdfFontFactory.CreateFont(rutaFuente, PdfEncodings.IDENTITY_H);

                    using (PdfWriter pdfWriter = new PdfWriter(stream))
                    using (PdfDocument pdfDocument = new PdfDocument(pdfWriter))
                    {
                        pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());
                        
                       
                        
                        using (Document document = new Document(pdfDocument))
                        {
                            
                           
                            var imageFile = System.IO.Path.Combine("Temp", "logo.JPG");
                            ImageData data = ImageDataFactory.Create(imageFile);
                            Image img = new Image(data);
                            img.SetWidth(50); // Establece el ancho en 200 unidades de PDF (por ejemplo, puntos)
                            img.SetHeight(50); // Establece el alto en 100 unidades de PDF (por ejemplo, puntos)

                            Table tableFechas = new Table(3).UseAllAvailableWidth();
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Dia")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Mes")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Año")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));

                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Day.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Month.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Year.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));

                            Table tableFirmas = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Inspeccionó\n(Nombre, Puesto y Firma)")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Matrícula")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));

                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("\n")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("\n")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));


                            Table tableCabeceros = new Table(13).UseAllAvailableWidth();
                            tableCabeceros.AddHeaderCell(new Cell(1, 3).Add(new Paragraph("Fecha Caducidad o fabricación")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Cantidad recibida")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Clave del artículo")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Lote")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Unidad")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Existencia fisica")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Consumo")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Descripción")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 2).Add(new Paragraph("Proveedor")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("No. notificación")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));

                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Dia")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Mes")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Año")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Clave")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Nombre")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));



                            //
                            foreach(var item in model.detalle)
                            {
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Day.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Month.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Year.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.cantidadRecibida.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.claveArticulo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.lote.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.unidad.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.existenciaFisica.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.descripcion.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.numProveedor.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.razonSocial.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.noNotificacion.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));



                            }
                            Table tableFinal = new Table(7).UseAllAvailableWidth();
                            tableFinal.AddHeaderCell(new Cell(2, 1).Add(img).SetWidth(50).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 6).Add(new Paragraph("Registro de Control de Caducidades")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("CLAVE:\n1832-009-003")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("VERSION: 1.0")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("VIGENTE A PARTIR:\n" + model.vigenteAPartir.ToString("dd/MM/yyyy"))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PROXIMA REVISION:\n" + model.proximaRevision.ToString("dd/MM/yyyy"))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("SUSTITUYE A:\n" + model.sustituye)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PAGINAS:\n"+model.paginas)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            
                            
                            tableFinal.AddHeaderCell(new Cell(1, 7).Add(new Paragraph("Registro de Control de Caducidades")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(12).SetFont(arialFont).SetBackgroundColor(ColorConstants.BLACK).SetFontColor(ColorConstants.WHITE));

                            tableFinal.AddHeaderCell(new Cell(1, 5).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(12).SetFont(arialFont));

                            tableFinal.AddHeaderCell(new Cell(1, 2).Add(tableFechas));

                            tableFinal.AddCell(new Cell(1, 7).Add(tableCabeceros));

                            tableFinal.AddFooterCell(new Cell(1, 7).Add(tableFirmas));
                            document.Add(tableFinal);
                        }
                    }

                   

                    // Convertir el MemoryStream a un arreglo de bytes
                    byte[] pdfBytes = stream.ToArray();
                    return pdfBytes;
                }





                //pdfDocument.SetDefaultPageSize




            }
            catch (Exception ioe)
            {
                return null;
            }

        }

        public byte[] Formato_RegistroControlCaducidadesLleno(GetControlInventario_Response model)
        {
            //String imageFile = "C:/logo_benavides.jpg";
            try
            {
                int fontSize = 6;
                int fontSizeDetalle = 6;
                PdfFont arialFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                using (MemoryStream stream = new MemoryStream())
                {
                    // Crear un nuevo documento PDF
                    //string rutaFuente = System.IO.Path.Combine("Temp", "MADETOMMY.otf");
                    //PdfFont fuentePersonalizada = PdfFontFactory.CreateFont(rutaFuente, PdfEncodings.IDENTITY_H);

                    using (PdfWriter pdfWriter = new PdfWriter(stream))
                    using (PdfDocument pdfDocument = new PdfDocument(pdfWriter))
                    {
                        pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());



                        using (Document document = new Document(pdfDocument))
                        {


                            var imageFile = System.IO.Path.Combine("Temp", "logo.JPG");
                            ImageData data = ImageDataFactory.Create(imageFile);
                            Image img = new Image(data);
                            img.SetWidth(50); // Establece el ancho en 200 unidades de PDF (por ejemplo, puntos)
                            img.SetHeight(50); // Establece el alto en 100 unidades de PDF (por ejemplo, puntos)

                            Table tableFechas = new Table(3).UseAllAvailableWidth();
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Dia")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Mes")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph("Año")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));

                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Day.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Month.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFechas.AddCell(new Cell(1, 1).Add(new Paragraph(model.fecha.Year.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));

                            Table tableFirmas = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Inspeccionó\n(Nombre, Puesto y Firma)")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Matrícula")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));

                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("\n")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));
                            tableFirmas.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("\n")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont).SetWidth(UnitValue.CreatePercentValue(50)));


                            Table tableCabeceros = new Table(13).UseAllAvailableWidth();
                            tableCabeceros.AddHeaderCell(new Cell(1, 3).Add(new Paragraph("Fecha Caducidad o fabricación")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Cantidad recibida")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Clave del artículo")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Lote")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Unidad")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Existencia fisica")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Consumo")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Descripción")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 2).Add(new Paragraph("Proveedor")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("No. notificación")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));

                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Dia")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Mes")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Año")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Clave")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                            tableCabeceros.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Nombre")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));



                            //
                            foreach (var item in model.detalle)
                            {
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Day.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Month.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.caducidad.Year.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.cantidadRecibida.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.claveArticulo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.lote.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.unidad.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph((item.existenciaFisica - item.consumo).ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.consumo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.descripcion.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.numProveedor.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.razonSocial.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));
                                tableCabeceros.AddCell(new Cell(1, 1).Add(new Paragraph(item.noNotificacion.ToString())).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSizeDetalle).SetFont(arialFont));



                            }
                            Table tableFinal = new Table(7).UseAllAvailableWidth();
                            tableFinal.AddHeaderCell(new Cell(2, 1).Add(img).SetWidth(50).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 6).Add(new Paragraph("Registro de Control de Caducidades")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("CLAVE:\n1832-009-003")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("VERSION: 1.0")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("VIGENTE A PARTIR:\n" + model.vigenteAPartir.ToString("dd/MM/yyyy"))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PROXIMA REVISION:\n" + model.proximaRevision.ToString("dd/MM/yyyy"))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("SUSTITUYE A:\n" + model.sustituye)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));
                            tableFinal.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PAGINAS:\n" + model.paginas)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(arialFont));


                            tableFinal.AddHeaderCell(new Cell(1, 7).Add(new Paragraph("Registro de Control de Caducidades")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(12).SetFont(arialFont).SetBackgroundColor(ColorConstants.BLACK).SetFontColor(ColorConstants.WHITE));

                            tableFinal.AddHeaderCell(new Cell(1, 5).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(12).SetFont(arialFont));

                            tableFinal.AddHeaderCell(new Cell(1, 2).Add(tableFechas));

                            tableFinal.AddCell(new Cell(1, 7).Add(tableCabeceros));

                            tableFinal.AddFooterCell(new Cell(1, 7).Add(tableFirmas));
                            document.Add(tableFinal);
                        }
                    }



                    // Convertir el MemoryStream a un arreglo de bytes
                    byte[] pdfBytes = stream.ToArray();
                    return pdfBytes;
                }





                //pdfDocument.SetDefaultPageSize




            }
            catch (Exception ioe)
            {
                return null;
            }

        }

    }



}
