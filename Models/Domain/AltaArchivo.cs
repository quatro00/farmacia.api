using System;
using System.Collections.Generic;

namespace Farmacia.UI.Models.Domain;

public partial class AltaArchivo
{
    public Guid Id { get; set; }

    public int Anio { get; set; }

    public string AltaContable { get; set; } = null!;

    public string Archivo { get; set; } = null!;

    public string RutaArchivo { get; set; } = null!;
}
