﻿using AsistenteCompras_API.DTOs;

namespace AsistenteCompras_API.Domain.Services;

public interface IOfertaRepository
{
    List<OfertaDTO> OfertasPorLocalidad(int idLocalidad, List<int> idProductos);
    decimal ObtenerPrecioMinimoDelProductoPorLocalidad(List<int> localidades, int idTipoProducto);
    List<OfertaDTO> ObtenerOfertasPorPrecio(int idTipoProducto, decimal precio);
    List<OfertaDTO> OfertasDentroDelRadio(List<int> idProductos, List<int> idComercios);
    List<OfertaDTO> OfertasDentroDelRadioV2(List<int> idProductos, List<int> idComercios, List<string> marcasElegidas);
    List<string> ObtenerMarcasDisponibles(List<int> idProductos);
}