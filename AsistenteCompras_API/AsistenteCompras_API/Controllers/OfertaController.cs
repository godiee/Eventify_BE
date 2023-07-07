﻿using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace AsistenteCompras_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfertaController : ControllerBase
{
    private readonly IOfertaService _ofertaService;
    private readonly IComercioService _comercioService;

    public OfertaController(IOfertaService ofertaService, IComercioService comercioService)
    {
        _ofertaService = ofertaService;
        _comercioService = comercioService;
    }

    [HttpPost("listaPersonalizada")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertaCantidadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerOfertasMasEconomicasSegunFiltros([FromBody]Filtro filtro)
    {
        try
        {
            List<OfertaCantidadDTO> ofertas = _ofertaService.GenerarListaPersonalizada(filtro);
            if (ofertas.Count != 0)
            {
                return Ok(ofertas);
            }
            else
            {
                return NoContent();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }


    [HttpPost("listadoOfertas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasPorProductoDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult ObtenerListadoOfertasMasEconomicas([FromBody] Filtro filtro)
    {
        try
        {
            List<OfertasPorProductoDTO> ofertas = _ofertaService.GenerarListaDeOfertas(filtro);
            if (ofertas.Count != 0)
            {
                return Ok(ofertas);
            }
            else
            {
                return NoContent();
            }

        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("recorrerMenos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfertasPorComercioDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult ObtenerOfertasEnMenorRecorrido([Required][FromBody] Filtro filtro)
    {
        try
        {
            List<int> idComercios = _comercioService.ObtenerComerciosPorRadio(filtro.LatitudUbicacion, filtro.LongitudUbicacion, filtro.Distancia);
            if (idComercios.IsNullOrEmpty())
                return NotFound("No se encontraron comercios en el radio seleccionado");

            List<OfertasPorComercioDTO> listaRecorrerMenos = _ofertaService.ListaRecorrerMenos(filtro, idComercios);
            if (listaRecorrerMenos.IsNullOrEmpty())
                return NotFound("No se encontraron ofertas dentro de los comercios disponibles");
            
            return Ok(listaRecorrerMenos);
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        
    }
}
