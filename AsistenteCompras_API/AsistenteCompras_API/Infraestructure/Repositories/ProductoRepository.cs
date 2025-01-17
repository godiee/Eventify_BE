﻿using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using AsistenteCompras_API.Infraestructure.Contexts;
using Microsoft.IdentityModel.Tokens;

namespace AsistenteCompras_API.Infraestructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private AsistenteComprasContext _context;

    public ProductoRepository(AsistenteComprasContext context)
    {
        _context = context;
    }

    public List<string> ObtenerMarcasParaElTipoDeProducto(int tipoProducto)
    {
        return _context.Productos.Where(p => p.IdTipoProducto.Equals(tipoProducto))
                                  .Select(p => p.Marca)
                                  .Distinct()
                                  .ToList();
    }

    public ProductoDTO ObtenerProductoPorCodigoDeBarras(string codigoBarras)
    {
        return _context.Productos.Where(p => p.CodigoBarras == codigoBarras)
                                 .Select(p => new ProductoDTO
                                 {
                                     Id = p.Id,
                                     Nombre = p.Nombre,
                                     Marca = p.Marca,
                                     IdTipoProducto = p.IdTipoProducto,
                                     Imagen = p.Imagen
                                 }).FirstOrDefault();
    }

    public List<ProductoDTO> ObtenerProductosPorMarcaYTipoProducto(int tipoProducto, string marca)
    {
        return _context.Productos.Where(p => p.IdTipoProducto.Equals(tipoProducto) && p.Marca.Equals(marca))
                                 .Select(p => new ProductoDTO
                                 {
                                     Id = p.Id,
                                     Nombre = p.Nombre,
                                     Marca = p.Marca,
                                     IdTipoProducto = p.IdTipoProducto,
                                     Imagen = p.Imagen
                                 }).ToList();
    }

    public List<int> ObtenerProductoIds()
    {
        return _context.Productos.Select(p => p.Id).ToList();
    }
}
