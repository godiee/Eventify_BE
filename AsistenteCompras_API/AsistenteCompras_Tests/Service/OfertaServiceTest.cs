using AsistenteCompras_API.Domain;
using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.DTOs;
using Moq;

namespace AsistenteCompras_Tests.Service;

public class OfertaServiceTest
{
    private static Mock<IOfertaRepository> ofertaRepo = new Mock<IOfertaRepository>();
    
    private static Mock<IComercioService> comercioServicio = new Mock<IComercioService>();

    private static Mock<ITipoProductoService> tipoProductoServicio = new Mock<ITipoProductoService>();
    
    private static Mock<IUbicacionService> ubicacionServicio = new Mock<IUbicacionService>();

    private DateTime fechaArgentina = DateTime.UtcNow.AddHours(-3).Date;

    private OfertaService ofertaServicio = new OfertaService(ofertaRepo.Object, comercioServicio.Object, tipoProductoServicio.Object, ubicacionServicio.Object);

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaVaciaCuandoNoHayOfertasParaLosProductosQueQuieroComprar()
    {
        //Dado
        List<int> idComercios = new List<int>() { 1 };
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 } }
        };

        List<Oferta> ofertasDelComercio = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 15,
                IdTipoProducto = 8,
                TipoProducto = "Snacks",
                NombreProducto = "Papa Fritas Lays",
                Marca = "Lays",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 350,
                Unidades = 1,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = 1,
                Longitud = 1,
                FechaVencimiento = fechaArgentina.ToString("dd-MM-yy")
            }
        };

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(1,fechaArgentina)).Returns(ofertasDelComercio);
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro, idComercios);
        
        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(1, fechaArgentina));
        Assert.Empty(resultado);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaVaciaCuandoNoHayOfertasEnLosComercios()
    {
        //Dado
        List<int> idComercios = new List<int>() { 1 };
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 } }
        };
        List<Oferta> ofertasDelComercio = new List<Oferta>();

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(1, fechaArgentina)).Returns(ofertasDelComercio);
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro, idComercios);

        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(1, fechaArgentina));
        Assert.Empty(resultado);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaDeComerciosCuandoSeEncuentraTodosLosProductosAComprar()
    {
        //Dado
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, {"gaseosa", 1 } }
        };
        List<int> comerciosEncontados = new List<int>() { 1,2 };
        List<Oferta> ofertasDelComercioChino = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 15,
                IdTipoProducto = 8,
                TipoProducto = "Snacks",
                NombreProducto = "Papa Fritas Lays",
                Marca = "Lays",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 350,
                Unidades = 1,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68479,
                Longitud = -58.50380
            }
        };
        List<Oferta> ofertasDelComercioAlmacen = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 4,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Coca-cola 1.5lt",
                Marca = "Coca-cola",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            }


        };

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0],fechaArgentina)).Returns(ofertasDelComercioChino);
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1],fechaArgentina)).Returns(ofertasDelComercioAlmacen);
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[1])).Returns("ImagenAlmacen");
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro,comerciosEncontados);

        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina));
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1], fechaArgentina));
        comercioServicio.Verify(c => c.ObtenerImagenDelComercio(comerciosEncontados[1]));
        Assert.Equal("Almacen",resultado.First().NombreComercio);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaVaciaCuandoNoSeEncuentraTodosLosProductosAComprar()
    {
        //Dado
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, { "gaseosa", 1 } }
        };
        List<int> comerciosEncontados = new List<int>() {1};
        List<Oferta> ofertasDelComercioAlmacen = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            }
        };

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina)).Returns(ofertasDelComercioAlmacen);
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[0])).Returns("ImagenAlmacen");
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro, comerciosEncontados);

        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina));
        comercioServicio.Verify(c => c.ObtenerImagenDelComercio(comerciosEncontados[0]));
        Assert.Empty(resultado);
    }

    [Fact]
    public void QueListaRecorrerMenosAlEncontraseUnProductoAComprarConVariasMarcasSeleccioneElMasBataro()
    {
        //Dado
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, { "gaseosa", 1 } }
        };
        List<int> comerciosEncontados = new List<int>() {1};
        List<Oferta> ofertasDelComercioAlmacen = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 4,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Coca-cola 1.5lt",
                Marca = "Coca-cola",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 11,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Manaos 1.5lt",
                Marca = "Manaos",
                Imagen = "Imagen",
                Precio = 50,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            }
        };
        Oferta marcaBarata = ofertasDelComercioAlmacen[3];
        Oferta marcaCara = ofertasDelComercioAlmacen[2];

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina)).Returns(ofertasDelComercioAlmacen);
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[0])).Returns("ImagenAlmacen");
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro, comerciosEncontados);
        List<Oferta> ofertasDelComercio = (List<Oferta>)resultado.First().Ofertas.Select(o => o.Oferta).ToList();

        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina));
        comercioServicio.Verify(c => c.ObtenerImagenDelComercio(comerciosEncontados[0]));
        Assert.Contains(marcaBarata, ofertasDelComercio);
        Assert.DoesNotContain(marcaCara, ofertasDelComercio);
    }

    [Fact]
    public void QueListaRecorrerMenosMeDevuelvaUnaListaDeComerciosOrdenadosPorDistancia()
    {
        //Dado
        Filtro filtro = new Filtro()
        {
            LatitudUbicacion = -34.68544,
            LongitudUbicacion = -58.50168,
            Distancia = 5,
            Comidas = new List<int>() { 2 },
            Bebidas = new List<int>() { 1 },
            MarcasComida = new List<string>() { "Vienisima", "Paladini" },
            MarcasBebida = new List<string>() { "Coca-cola" },
            CantidadProductos = new Dictionary<string, double>() { { "salchichas", 1 }, { "pan de pancho", 1 }, { "gaseosa", 1 } }
        };
        List<int> comerciosEncontados = new List<int>() { 1, 2 };
        List<Oferta> ofertasDelComercioChino = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68479,
                Longitud = -58.50380
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68479,
                Longitud = -58.50380
            },
            new Oferta
            {
                IdPublicacion = 4,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Coca-cola 1.5lt",
                Marca = "Coca-cola",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Chino",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68479,
                Longitud = -58.50380
            }
        };
        List<Oferta> ofertasDelComercioAlmacen = new List<Oferta>() {
            new Oferta
            {
                IdPublicacion = 2,
                IdTipoProducto = 1,
                TipoProducto = "salchichas",
                NombreProducto = "salchichas vienissima x12",
                Marca = "vienissima",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 12,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 3,
                IdTipoProducto = 2,
                TipoProducto = "pan de pancho",
                NombreProducto = "pan de pancho la perla x6",
                Marca = "la perla",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 0,
                Unidades = 6,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            },
            new Oferta
            {
                IdPublicacion = 4,
                IdTipoProducto = 3,
                TipoProducto = "gaseosa",
                NombreProducto = "Coca-cola 1.5lt",
                Marca = "Coca-cola",
                Imagen = "Imagen",
                Precio = 100,
                Peso = 1500,
                Unidades = 0,
                NombreComercio = "Almacen",
                Localidad = "Ciudad Madero",
                IdLocalidad = 1,
                Latitud = -34.68485,
                Longitud = -58.50218
            }
        };

        //Cuando
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina)).Returns(ofertasDelComercioChino);
        ofertaRepo.Setup(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1], fechaArgentina)).Returns(ofertasDelComercioAlmacen);
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[0])).Returns("ImagenChino");
        comercioServicio.Setup(c => c.ObtenerImagenDelComercio(comerciosEncontados[1])).Returns("ImagenAlmacen");
        ubicacionServicio.Setup(u => u.CalcularDistanciaPorHaversine(filtro.LatitudUbicacion, filtro.LongitudUbicacion, ofertasDelComercioChino.First().Latitud, ofertasDelComercioChino.First().Longitud)).Returns(205);
        ubicacionServicio.Setup(u => u.CalcularDistanciaPorHaversine(filtro.LatitudUbicacion, filtro.LongitudUbicacion, ofertasDelComercioAlmacen.First().Latitud, ofertasDelComercioAlmacen.First().Longitud)).Returns(76);
        List<OfertasPorComercioDTO> resultado = ofertaServicio.ListaRecorrerMenos(filtro, comerciosEncontados);

        //Entonces
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[0], fechaArgentina));
        ofertaRepo.Verify(o => o.OfertasPorComercioFiltradasPorFecha(comerciosEncontados[1], fechaArgentina));
        comercioServicio.Verify(c => c.ObtenerImagenDelComercio(comerciosEncontados[0]));
        comercioServicio.Verify(c => c.ObtenerImagenDelComercio(comerciosEncontados[1]));
        ubicacionServicio.Verify(u => u.CalcularDistanciaPorHaversine(filtro.LatitudUbicacion, filtro.LongitudUbicacion, ofertasDelComercioChino.First().Latitud, ofertasDelComercioChino.First().Longitud));
        ubicacionServicio.Verify(u => u.CalcularDistanciaPorHaversine(filtro.LatitudUbicacion, filtro.LongitudUbicacion, ofertasDelComercioAlmacen.First().Latitud, ofertasDelComercioAlmacen.First().Longitud));
        Assert.Equal("Almacen", resultado.First().NombreComercio);
        Assert.Equal("Chino", resultado[1].NombreComercio);
    }
}