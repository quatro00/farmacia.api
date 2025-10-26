using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class RequerimientoCatalogoIi
{
    public Guid Id { get; set; }

    public string Folio { get; set; } = null!;

    public string Nss { get; set; } = null!;

    public string? Agregado { get; set; }

    public string NombrePaciente { get; set; } = null!;

    public string Diagnostico { get; set; } = null!;

    public string Ooad { get; set; } = null!;

    public string ClaveMedicamento { get; set; } = null!;

    public string RequerimientoMensual { get; set; } = null!;

    public int Meses { get; set; }

    public int Piezas { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public string Observaciones { get; set; } = null!;

    public DateTime FechaEvaluacion { get; set; }

    public int EstatusId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual RequerimientoCatalogoIiestatus Estatus { get; set; } = null!;

    public virtual ICollection<RequerimientoCatalogoIiarchivo> RequerimientoCatalogoIiarchivos { get; set; } = new List<RequerimientoCatalogoIiarchivo>();
}
