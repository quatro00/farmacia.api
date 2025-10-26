using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class GetAltasPendiente
{
    public Guid Id { get; set; }

    public int? Anio { get; set; }

    public string? NumeroAltaContable { get; set; }

    public string? Gpo { get; set; }

    public string? Gen { get; set; }

    public string? Esp { get; set; }

    public string? Dif { get; set; }

    public string? Var { get; set; }

    public string? DescripcionArticulo { get; set; }

    public string? FechaHoraAlta { get; set; }

    public string? NumeroProveedor { get; set; }

    public string? RfcProveedor { get; set; }

    public string? RazonSocial { get; set; }

    public decimal? Recepcion { get; set; }

    public decimal? Cantidad { get; set; }

    public string? CantidadAutorizada { get; set; }
}
