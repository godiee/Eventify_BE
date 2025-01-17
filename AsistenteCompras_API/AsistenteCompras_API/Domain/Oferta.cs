﻿using System.Globalization;

namespace AsistenteCompras_API.Domain;

public class Oferta
{
    public int IdPublicacion { get; set; }

    public int IdTipoProducto { get; set; }

    public string TipoProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public double Precio { get; set; }

    public double Peso { get; set; }

    public int Unidades { get; set; }

    public string NombreComercio { get; set; } = null!;

    public string Localidad { get; set; } = null!;

    public int IdLocalidad { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }
    public string FechaVencimiento { get; set; } = null!;

}

