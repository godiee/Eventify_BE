﻿using System;
using System.Collections.Generic;

namespace Productos_Entities.Entities;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
