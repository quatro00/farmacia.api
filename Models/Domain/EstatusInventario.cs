using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class EstatusInventario
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
