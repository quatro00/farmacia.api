using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class Lote
{
    public Guid Id { get; set; }

    public int Anio { get; set; }

    public string NumeroAltaContable { get; set; } = null!;

    public string Gpo { get; set; } = null!;

    public string Gen { get; set; } = null!;

    public string Esp { get; set; } = null!;

    public string Dif { get; set; } = null!;

    public string Var { get; set; } = null!;

    public string Lote1 { get; set; } = null!;

    public DateTime Caducidad { get; set; }

    public decimal Cantidad { get; set; }

    public decimal Disponible { get; set; }

    public bool? TieneCartaCanje { get; set; }

    public virtual ICollection<InventarioDetalle> InventarioDetalles { get; set; } = new List<InventarioDetalle>();
}
