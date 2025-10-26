using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class RequerimientoCatalogoIiarchivo
{
    public Guid Id { get; set; }

    public Guid RequerimientoId { get; set; }

    public string Archivo { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual RequerimientoCatalogoIi Requerimiento { get; set; } = null!;
}
