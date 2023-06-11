﻿using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AsistenteCompras_API.Domain.Services;

namespace AsistenteCompras_API.Infraestructure.Repositories;

public class OfertaRepository : IOfertaRepository
{

    private AsistenteComprasContext _context;

    public OfertaRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, decimal precio)
    {
        return _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && p.Precio == precio)
                                    .Select(o => new OfertaDTO
                                    {
                                        IdPublicacion = o.Id,
                                        IdTipoProducto = o.IdProductoNavigation.IdTipoProducto,
                                        TipoProducto = o.IdProductoNavigation.IdTipoProductoNavigation.Nombre,
                                        IdLocalidad = o.IdComercioNavigation.IdLocalidad,
                                        NombreProducto = o.IdProductoNavigation.Nombre,
                                        Marca = o.IdProductoNavigation.Marca,
                                        Imagen = o.IdProductoNavigation.Imagen,
                                        Precio = (double)o.Precio,
                                        NombreComercio = o.IdComercioNavigation.RazonSocial,
                                        Latitud = (double)o.IdComercioNavigation.Latitud,
                                        Longitud = (double)o.IdComercioNavigation.Longitud,
                                        Localidad = o.IdComercioNavigation.IdLocalidadNavigation.Nombre
                                    }).ToList();
    }

    public decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto)
    {
        return _context.Publicacions.Where(p => p.IdProductoNavigation.IdTipoProducto == idTipoProducto && localidades.Contains(p.IdComercioNavigation.IdLocalidad))
                                    .Min(p => p.Precio);
    }

    public List<OfertaDTO> OfertasPorLocalidad(int idLocalidad, List<int> idProductos)
    {
        return _context.Publicacions.Where(pub => pub.IdComercioNavigation.IdLocalidadNavigation.Id == idLocalidad)
                                                .Join(_context.Productos, pub => pub.IdProducto, p => p.Id,
                                                    (pub, p) => new OfertaDTO
                                                    {
                                                        IdPublicacion = pub.Id,
                                                        IdTipoProducto = p.IdTipoProducto,
                                                        TipoProducto = p.IdTipoProductoNavigation.Nombre,
                                                        IdLocalidad = pub.IdComercioNavigation.IdLocalidad,
                                                        NombreProducto = p.Nombre,
                                                        Marca = p.Marca,
                                                        Imagen = p.Imagen,
                                                        Precio = (double)pub.Precio,
                                                        NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                                        Latitud = (double)pub.IdComercioNavigation.Latitud,
                                                        Longitud = (double)pub.IdComercioNavigation.Longitud,
                                                        Localidad = pub.IdComercioNavigation.IdLocalidadNavigation.Nombre
                                                    })
                                                .Where(oferta => idProductos.Contains(oferta.IdTipoProducto)).ToList();
    }

    public List<OfertaDTO> OfertasDentroDelRadio(List<int> idProductos, List<int> idComercios)
    {
        return _context.Publicacions.Where(pub => idComercios.Contains(pub.IdComercio))
                            .Join(_context.Productos, pub => pub.IdProducto, p => p.Id,
                                  (pub, p) => new OfertaDTO
                                  {
                                      IdPublicacion = pub.Id,
                                      IdTipoProducto = p.IdTipoProducto,
                                      TipoProducto = p.IdTipoProductoNavigation.Nombre,
                                      IdLocalidad = pub.IdComercioNavigation.IdLocalidad,
                                      NombreProducto = p.Nombre,
                                      Marca = p.Marca,
                                      Imagen = p.Imagen,
                                      Precio = (double)pub.Precio,
                                      NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                      Latitud = (double)pub.IdComercioNavigation.Latitud,
                                      Longitud = (double)pub.IdComercioNavigation.Longitud,
                                      Localidad = pub.IdComercioNavigation.IdLocalidadNavigation.Nombre,
                                      Peso = p.Peso,
                                      Unidades = p.Unidades
                                  })
                            .Where(oferta => idProductos.Contains(oferta.IdTipoProducto)).ToList();

    }

    public List<OfertaDTO> OfertasDentroDelRadioV2(List<int> idProductos, List<int> idComercios, List<String> marcasElegidas)
    {


        List<OfertaDTO> ofertas = _context.Publicacions.Where(pub => idComercios.Contains(pub.IdComercio))
                            .Join(_context.Productos, pub => pub.IdProducto, p => p.Id,
                                  (pub, p) => new OfertaDTO
                                  {
                                      IdPublicacion = pub.Id,
                                      IdTipoProducto = p.IdTipoProducto,
                                      TipoProducto = p.IdTipoProductoNavigation.Nombre,
                                      IdLocalidad = pub.IdComercioNavigation.IdLocalidad,
                                      NombreProducto = p.Nombre,
                                      Marca = p.Marca,
                                      Imagen = p.Imagen,
                                      Precio = (double)pub.Precio,
                                      NombreComercio = pub.IdComercioNavigation.RazonSocial,
                                      Latitud = (double)pub.IdComercioNavigation.Latitud,
                                      Longitud = (double)pub.IdComercioNavigation.Longitud,
                                      Localidad = pub.IdComercioNavigation.IdLocalidadNavigation.Nombre,
                                      Peso = p.Peso,
                                      Unidades = p.Unidades
                                  })
                            .Where(oferta => idProductos.Contains(oferta.IdTipoProducto) && marcasElegidas.Contains(oferta.Marca))
                            .ToList();
        return ofertas;
    }


    public List<String> ObtenerMarcasDisponibles(List<int> idProductos)
    {
        return _context.Publicacions.Where(p=> idProductos.Contains(p.IdProductoNavigation.IdTipoProducto))
                                    .Select(p=>p.IdProductoNavigation.Marca) 
                                    .ToList();
    }

}