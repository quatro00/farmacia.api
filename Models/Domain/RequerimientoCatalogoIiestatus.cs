using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class RequerimientoCatalogoIiestatus
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<RequerimientoCatalogoIi> RequerimientoCatalogoIis { get; set; } = new List<RequerimientoCatalogoIi>();
}
