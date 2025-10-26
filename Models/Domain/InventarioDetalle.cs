using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class InventarioDetalle
{
    public Guid InventarioId { get; set; }

    public Guid LoteId { get; set; }

    public int DetalleId { get; set; }

    public decimal Teorico { get; set; }

    public decimal Conteo { get; set; }

    public decimal Consumo { get; set; }

    public bool Aplicado { get; set; }

    public string ClaveProveedor { get; set; } = null!;

    public string NombreProveedor { get; set; } = null!;

    public int NoNotificacion { get; set; }

    public string Unidad { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual Inventario Inventario { get; set; } = null!;

    public virtual Lote Lote { get; set; } = null!;
}
