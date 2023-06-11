﻿using AsistenteCompras_Entities.DTOs;
using AsistenteCompras_Entities.Entities;

namespace AsistenteCompras_Services;

public interface IEventoService
{
    List<Evento> ObtenerEventos();

    List<Comidum> ObtenerComidas(int idEvento);

    List<Bebidum> ObtenerBebidas(int idEvento);

    List<TipoProductoDTO> ObtenerListadoParaEvento(int idEvento, int idComida, int idBebida);

    List<TipoProductoDTO> ObtenerListadoParaEvento(ProductosABuscarDTO productosABuscar, int invitados);
}
