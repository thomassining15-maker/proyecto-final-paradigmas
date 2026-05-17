using p_ProveedorStreaming;
using p_ProveedorStreaming.Aspectos;
using p_ProveedorStreaming.Clases;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strContenido.Clases.strPelicula;
using p_ProveedorStreaming.Clases.strContenido.Clases.strSerie;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using p_ProveedorStreaming.Clases.strUsuario;
using static p_ProveedorStreaming.Clases.strUsuario.ReglasNegocioUsuario;

namespace p_ProveedorStreaming.Tests;

// ─────────────────────────────────────────────────────────
//  TESTS DE USUARIO
// ─────────────────────────────────────────────────────────
public class UsuarioTests
{
    [Fact]
    public void SumarPuntos_CantidadPositiva_SumaPuntos()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(100);
        Assert.Equal(100UL, u.Puntos);
    }

    [Fact]
    public void SumarPuntos_CantidadCero_NoCambiaPuntos()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(0);
        Assert.Equal(0UL, u.Puntos);
    }

    [Fact]
    public void SumarPuntos_CantidadNegativa_NoCambiaPuntos()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(-50);
        Assert.Equal(0UL, u.Puntos);
    }

    [Fact]
    public void SumarPuntos_VariasVeces_AcumulaCorrectamente()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(50);
        u.SumarPuntos(50);
        u.SumarPuntos(28);
        Assert.Equal(128UL, u.Puntos);
    }

    [Fact]
    public void CambiarCategoria_MenosDe700Puntos_PermaneceGeneral()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(699);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.General, u.Categoria);
    }

    [Fact]
    public void CambiarCategoria_700Puntos_CambiaAPro()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(700);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.Pro, u.Categoria);
    }

    [Fact]
    public void CambiarCategoria_1999Puntos_PermanecePro()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(1999);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.Pro, u.Categoria);
    }

    [Fact]
    public void CambiarCategoria_2000Puntos_CambiaAMaster()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(2000);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.Master, u.Categoria);
    }

    [Fact]
    public void CambiarCategoria_SinCruzarUmbral_MantieneCategoriaGeneral()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(500);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.General, u.Categoria);
    }

    [Fact]
    public void CambiarCategoria_AlCruzar700_CategoriaEsPro()
    {
        var u = new Usuario("Test");
        u.SumarPuntos(700);
        u.CambiarCategoria(u);
        Assert.Equal(l_categorias.Pro, u.Categoria);
    }
}

// ─────────────────────────────────────────────────────────
//  TESTS DE PELÍCULA
// ─────────────────────────────────────────────────────────
public class PeliculaTests
{
    [Fact]
    public void Constructor_CalificacionMinima_CreaCorrectamente()
    {
        var p = new Pelicula("Test", TimeSpan.FromHours(2), 1);
        Assert.Equal("Test", p.Nombre);
        Assert.Equal(1, p.Calificacion);
    }

    [Fact]
    public void Constructor_CalificacionMaxima_CreaCorrectamente()
    {
        var p = new Pelicula("Test", TimeSpan.FromHours(2), 2);
        Assert.Equal(2, p.Calificacion);
    }

    [Fact]
    public void Constructor_CalificacionCero_LanzaExcepcion()
    {
        Assert.Throws<Exception>(() =>
            new Pelicula("Test", TimeSpan.FromHours(2), 0));
    }

    [Fact]
    public void Constructor_CalificacionMayorQueDos_LanzaExcepcion()
    {
        Assert.Throws<Exception>(() =>
            new Pelicula("Test", TimeSpan.FromHours(2), 3));
    }

    [Fact]
    public void ActualizarPuntaje_PeliculaCorta_Suma50Puntos()
    {
        var factory = CrearFactory();
        var service = new ContenidoService(factory);
        var usuario = new Usuario("Test");

        // Duración < 1:30 → pts_pelicula_corta = 50
        var pelicula = new Pelicula("Corta", TimeSpan.FromMinutes(90), 1);
        service.Agregar(pelicula);

        var proxy = service.ObtenerPorTitulo("Corta");
        proxy.UsuarioActivo = usuario;
        proxy.Reproducir();

        Assert.Equal(50UL, usuario.Puntos);
    }

    [Fact]
    public void ActualizarPuntaje_PeliculaLarga_Suma50Puntos()
    {
        var factory = CrearFactory();
        var service = new ContenidoService(factory);
        var usuario = new Usuario("Test");

        // Duración > 1:30 → pts_pelicula_larga = 50
        var pelicula = new Pelicula("Larga", TimeSpan.FromHours(3), 1);
        service.Agregar(pelicula);

        var proxy = service.ObtenerPorTitulo("Larga");
        proxy.UsuarioActivo = usuario;
        proxy.Reproducir();

        Assert.Equal(50UL, usuario.Puntos);
    }

    private static MensajeFactory CrearFactory() =>
        new(new Interceptor_GestionPuntos(), new Interceptor_GestionRecord(),
            new Interceptor_Validacion(), new Interceptor_Autenticacion());
}

// ─────────────────────────────────────────────────────────
//  TESTS DE SERIE
// ─────────────────────────────────────────────────────────
public class SerieTests
{
    [Fact]
    public void Constructor_ValoresValidos_CreaCorrectamente()
    {
        var s = new Serie("Test", 2, 10);
        Assert.Equal("Test", s.Nombre);
        Assert.Equal(2, s.Temporadas);
        Assert.Equal(10, s.Cap_x_temp);
    }

    [Fact]
    public void Constructor_CeroTemporadas_LanzaExcepcion()
    {
        Assert.Throws<Exception>(() => new Serie("Test", 0, 10));
    }

    [Fact]
    public void Constructor_CapitulosMenorA5_LanzaExcepcion()
    {
        Assert.Throws<Exception>(() => new Serie("Test", 1, 4));
    }

    [Fact]
    public void ActualizarPuntaje_Episodio_Suma28Puntos()
    {
        var factory = new MensajeFactory(
            new Interceptor_GestionPuntos(), new Interceptor_GestionRecord(),
            new Interceptor_Validacion(), new Interceptor_Autenticacion());
        var service = new ContenidoService(factory);
        var usuario = new Usuario("Test");

        var serie = new Serie("TestSerie", 1, 5);
        service.Agregar(serie);

        var proxy = service.ObtenerPorTitulo("TestSerie");
        proxy.UsuarioActivo = usuario;
        proxy.Reproducir();

        Assert.Equal(28UL, usuario.Puntos);
    }
}

// ─────────────────────────────────────────────────────────
//  TESTS DE JUEGO
// ─────────────────────────────────────────────────────────
public class JuegoTests
{
    [Fact]
    public void RegistrarPuntaje_Puesto1_Suma30Puntos()
    {
        var juego = new Juego("Test", "Accion");
        var usuario = new Usuario("Test");
        juego.RegistrarPuntaje(usuario, 1);
        Assert.Equal(30UL, usuario.Puntos);
    }

    [Fact]
    public void RegistrarPuntaje_Puesto2_Suma20Puntos()
    {
        var juego = new Juego("Test", "Accion");
        var usuario = new Usuario("Test");
        juego.RegistrarPuntaje(usuario, 2);
        Assert.Equal(20UL, usuario.Puntos);
    }

    [Fact]
    public void RegistrarPuntaje_Puesto3_Suma10Puntos()
    {
        var juego = new Juego("Test", "Accion");
        var usuario = new Usuario("Test");
        juego.RegistrarPuntaje(usuario, 3);
        Assert.Equal(10UL, usuario.Puntos);
    }

    [Fact]
    public void RegistrarPuntaje_OtroPuesto_Suma5Puntos()
    {
        var juego = new Juego("Test", "Accion");
        var usuario = new Usuario("Test");
        juego.RegistrarPuntaje(usuario, 10);
        Assert.Equal(5UL, usuario.Puntos);
    }

    [Fact]
    public void RegistrarPuntaje_ActualizaCategoriaUsuario()
    {
        var juego = new Juego("Test", "Accion");
        var usuario = new Usuario("Test");

        // Sumar suficientes puntos para llegar a Pro con el puesto 1
        for (int i = 0; i < 23; i++)      // 23 × 30 = 690 (aún General)
            juego.RegistrarPuntaje(usuario, 1);
        Assert.Equal(l_categorias.General, usuario.Categoria);

        juego.RegistrarPuntaje(usuario, 1); // 24 × 30 = 720 → Pro
        Assert.Equal(l_categorias.Pro, usuario.Categoria);
    }
}

// ─────────────────────────────────────────────────────────
//  TESTS DE ASPECTOS AOP
// ─────────────────────────────────────────────────────────
public class AspectoTests
{
    private static MensajeFactory CrearFactory() =>
        new(new Interceptor_GestionPuntos(), new Interceptor_GestionRecord(),
            new Interceptor_Validacion(), new Interceptor_Autenticacion());

    [Fact]
    public void Autenticacion_CredencialesCorrectas_RetornaTrue()
    {
        var factory = CrearFactory();
        var acceso = factory.CrearMensajeAutenticacion(new AccesoSistema());
        Assert.True(acceso.IngresarSistema("admin", "1234"));
    }

    [Fact]
    public void Autenticacion_CredencialesIncorrectas_RetornaFalse()
    {
        var factory = CrearFactory();
        var acceso = factory.CrearMensajeAutenticacion(new AccesoSistema());
        Assert.False(acceso.IngresarSistema("hacker", "0000"));
    }

    [Fact]
    public void Autenticacion_UsuarioVacio_LanzaExcepcion()
    {
        var factory = CrearFactory();
        var acceso = factory.CrearMensajeAutenticacion(new AccesoSistema());
        Assert.Throws<Exception>(() => acceso.IngresarSistema("", "1234"));
    }

    [Fact]
    public void ValidacionBD_ObjetoValido_RetornaMensaje()
    {
        var factory = CrearFactory();
        var bd = factory.CrearMensajeValidacion(new RegistroBD());
        var resultado = bd.Registrar(new Usuario("Test"));
        Assert.Contains("persistida en BD", resultado);
    }

    [Fact]
    public void ValidacionBD_ObjetoNulo_LanzaExcepcion()
    {
        var factory = CrearFactory();
        var bd = factory.CrearMensajeValidacion(new RegistroBD());
        Assert.Throws<Exception>(() => bd.Registrar(null!));
    }
}
