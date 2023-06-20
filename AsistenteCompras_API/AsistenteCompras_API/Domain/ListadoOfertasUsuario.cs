﻿using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Contexts.Configurations;

namespace AsistenteCompras_API.Domain
{
    public class ListadoOfertasUsuario
    {
        public int IdListado { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; } = null!;
        public string Evento { get; set; } = null!;
        public List<string> ComidasElegidas { get; set; } = null!;
        public List<string> BebidasElegidas { get; set; } = null!;
        public int CantidadOfertas { get; set; }
        public List<OfertaCantidadDTO>? Ofertas { get; set; }
        public double TotalListado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}