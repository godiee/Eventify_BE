﻿using AsistenteCompras_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteCompras_Repository
{
    public interface IComercioRepository
    {
        List<Comercio> ObtenerComerciosPorRadio(double latitud, double longitud, float distancia);

    }
}
