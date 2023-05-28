﻿using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Repository;

public class UbicacionRepository : IUbicacionRepository
{
    private readonly AsistenteComprasContext _context;

    public UbicacionRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<LocalidadDTO> ObtenerTodasLasLocalidades()
    {
        return _context.Localidads.Select(l => new LocalidadDTO
        {
            Id = l.Id,
            Nombre = l.Nombre
        }).ToList();
    }
}
