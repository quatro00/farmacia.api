using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class ImportAlta
{
    public Guid Id { get; set; }

    public int? Anio { get; set; }

    public DateTime? FechaAlta { get; set; }

    public string? NumeroAltaContable { get; set; }

    public string? ClasPtalOrigen { get; set; }

    public string? NombreOoad { get; set; }

    public string? FechaRegistro { get; set; }

    public string? ClasPtalUnidadEntrega { get; set; }

    public string? NombreUnidadEntrega { get; set; }

    public string? TipoReporte { get; set; }

    public string? NumeroDeDocumento { get; set; }

    public string? NumeroDeReposicion { get; set; }

    public string? Cargoa { get; set; }

    public string? Creditoa { get; set; }

    public string? DescripcionMovimiento { get; set; }

    public string? Gpo { get; set; }

    public string? Gen { get; set; }

    public string? Esp { get; set; }

    public string? Dif { get; set; }

    public string? Var { get; set; }

    public string? DescripcionArticulo { get; set; }

    public string? UnidadPresentacion { get; set; }

    public string? CantidadPresentacion { get; set; }

    public string? TipoPresentacion { get; set; }

    public string? PrecioCatalogoArticulos { get; set; }

    public string? PrecioCompra { get; set; }

    public string? Iva { get; set; }

    public string? CantidadAutorizada { get; set; }

    public string? CantidadConteo { get; set; }

    public string? ImporteArticuloSinIva { get; set; }

    public string? ImporteAltaConIva { get; set; }

    public string? LineaArticulo { get; set; }

    public string? RfcProveedor { get; set; }

    public string? NumeroProveedor { get; set; }

    public string? RazonSocial { get; set; }

    public string? NumeroLicitacion { get; set; }

    public string? FechaHoraRecepcion { get; set; }

    public string? FechaHoraEntrega { get; set; }

    public string? FechaHoraAlta { get; set; }

    public string? PartidaPresupuestal { get; set; }

    public string? TipoError { get; set; }

    public string? Enviado { get; set; }

    public string? FechaEnvioPrei { get; set; }
}
