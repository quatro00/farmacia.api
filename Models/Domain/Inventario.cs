using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class Inventario
{
    public Guid InventarioId { get; set; }

    public DateTime Fecha { get; set; }

    public int Folio { get; set; }

    public string CveAdsc { get; set; } = null!;

    public string ResponsableConteo { get; set; } = null!;

    public string ResponsableControPuesto { get; set; } = null!;

    public string Generado { get; set; } = null!;

    public string GeneradoPuesto { get; set; } = null!;

    public string Responsable { get; set; } = null!;

    public string ResponsablePuesto { get; set; } = null!;

    public int EstatusInventario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaTermino { get; set; }

    public string? ControlDeCaducidades { get; set; }

    public virtual EstatusInventario EstatusInventarioNavigation { get; set; } = null!;

    public virtual ICollection<InventarioDetalle> InventarioDetalles { get; set; } = new List<InventarioDetalle>();
}
