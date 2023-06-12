﻿using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Contexts;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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


    public List<String> ObtenerMarcasComidasDisponibles(List<int> idProductos)
    {
        List<string> marcasEncontradas = new List<string>();

        var marcas = (from Publicacion pub in _context.Publicacions
                       join Producto p in _context.Productos on pub.IdProducto equals p.Id
                       join TipoProducto tp in _context.TipoProductos on p.IdTipoProducto equals tp.Id
                       join ComidaTipoProducto ctp in _context.ComidaTipoProductos on tp.Id equals ctp.IdTipoProducto
                       where idProductos.Contains(ctp.IdComida)
                       select p.Marca).Distinct();
                               
        foreach(var item in marcas)
        {
            marcasEncontradas.Add(item);
        }
        return marcasEncontradas; 
    }


    public List<String> ObtenerMarcasBebidasDisponibles(List<int> idProductos)
    {
        List<string> marcasEncontradas = new List<string>();

        var marcas = (from Publicacion pub in _context.Publicacions
                       join Producto p in _context.Productos on pub.IdProducto equals p.Id
                       join TipoProducto tp in _context.TipoProductos on p.IdTipoProducto equals tp.Id
                       join BebidaTipoProducto btp in _context.BebidaTipoProductos on tp.Id equals btp.IdTipoProducto
                       where idProductos.Contains(btp.IdBebida)
                       select p.Marca).Distinct();

        foreach (var item in marcas)
        {
            marcasEncontradas.Add(item);
        }
        return marcasEncontradas;
    }

    public List<OfertaDTO> OfertasPorComercio(int idComercio)
    {
        return _context.Publicacions.Where(p => p.IdComercio == idComercio)
                                    .Select(o => new OfertaDTO
                                    {
                                        IdPublicacion = o.Id,
                                        IdTipoProducto = o.IdProductoNavigation.IdTipoProducto,
                                        TipoProducto = o.IdProductoNavigation.IdTipoProductoNavigation.Nombre,
                                        NombreProducto = o.IdProductoNavigation.Nombre,
                                        Marca = o.IdProductoNavigation.Marca,
                                        Imagen = o.IdProductoNavigation.Imagen,
                                        Precio = ((double)o.Precio),
                                        Peso = o.IdProductoNavigation.Peso,
                                        Unidades = o.IdProductoNavigation.Unidades,
                                        NombreComercio = o.IdComercioNavigation.RazonSocial,
                                        Localidad = o.IdComercioNavigation.IdLocalidadNavigation.Nombre,
                                        IdLocalidad = o.IdComercioNavigation.IdLocalidad,
                                        Latitud = (double) o.IdComercioNavigation.Latitud,
                                        Longitud = (double) o.IdComercioNavigation.Longitud
                                    }).ToList();
    }
}