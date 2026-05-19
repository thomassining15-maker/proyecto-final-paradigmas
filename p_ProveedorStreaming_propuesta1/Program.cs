using p_ProveedorStreaming.Aspectos;
using p_ProveedorStreaming.Clases;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strProveedor;
using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Interfaces;

namespace p_ProveedorStreaming
{
    public static class ProgramaDemo
    {
        public static void EjecutarDemo()
        {
            var interceptorPuntos  = new Interceptor_GestionPuntos();
            var interceptorRecord  = new Interceptor_GestionRecord();
            var interceptorVal     = new Interceptor_Validacion();
            var interceptorAuth    = new Interceptor_Autenticacion();

            var factory = new MensajeFactory(
                interceptorPuntos,
                interceptorRecord,
                interceptorVal,
                interceptorAuth);

            var usuarioService   = new UsuarioService();
            var contenidoService = new ContenidoService(factory);
            var juegoService     = new JuegoService(factory);
            var proveedor        = new Proveedor("StreamingMax");
            var proveedorService = new ProveedorService(proveedor);

            Titulo("SISTEMA DE STREAMING — DEMO COMPLETA");

            // 1. ASPECTO AUTENTICACIÓN
            Seccion("1. ASPECTO AUTENTICACIÓN (AOP)");

            IAcceso accesoConAspecto = factory.CrearMensajeAutenticacion(new AccesoSistema());

            Console.WriteLine("Intento con credenciales incorrectas:");
            try   { accesoConAspecto.IngresarSistema("hacker", "0000"); }
            catch (Exception ex) { Console.WriteLine($"  => {ex.Message}"); }

            Console.WriteLine("\nIntento con credenciales correctas:");
            bool sesionActiva = accesoConAspecto.IngresarSistema("admin", "1234");
            Console.WriteLine($"  => Sesión activa: {sesionActiva}");

            // 2. CARGA DESDE ARCHIVOS
            Seccion("2. CARGA DESDE ARCHIVOS .TXT");

            usuarioService.Cargar("Usuarios.txt");
            Console.WriteLine($"  Usuarios cargados: {usuarioService.ObtenerTodos().Count}");
            foreach (var u in usuarioService.ObtenerTodos())
                Console.WriteLine($"    - {u.Nombre} (ID {u.Id_interno}) | {u.Categoria} | {u.Puntos} pts");

            Console.WriteLine();
            contenidoService.Cargar("Contenidos.txt");
            Console.WriteLine($"  Contenidos cargados: {contenidoService.ObtenerTodos().Count}");
            foreach (var c in contenidoService.ObtenerTodos())
                Console.WriteLine($"    - {c.Nombre}");

            Console.WriteLine();
            juegoService.Cargar("Juegos.txt");
            Console.WriteLine($"  Juegos cargados: {juegoService.ObtenerTodos().Count}");
            foreach (var j in juegoService.ObtenerTodos())
                Console.WriteLine($"    - {j.Nombre} ({j.Genero})");

            // 3. ASPECTO VALIDACIÓN
            Seccion("3. ASPECTO VALIDACIÓN — SIMULACIÓN GUARDADO EN BD (AOP)");

            IRegistroEntidad bdConAspecto = factory.CrearMensajeValidacion(new RegistroBD());

            Console.WriteLine("Registrando usuario manual en BD simulada:");
            var usuarioManual = new Usuario("Diego Ramirez");
            string resultadoBD = bdConAspecto.Registrar(usuarioManual);
            Console.WriteLine($"  => {resultadoBD}");
            usuarioService.Agregar(usuarioManual);

            Console.WriteLine("\nIntento de registrar objeto nulo:");
            try   { bdConAspecto.Registrar(null!); }
            catch (Exception ex) { Console.WriteLine($"  => {ex.Message}"); }

            // 4. EVENTO NUEVO TÍTULO
            Seccion("4. EVENTO: NUEVO TÍTULO LLEGA AL SISTEMA");

            var peliculaNueva = contenidoService.ObtenerTodos()[0];
            peliculaNueva.pub_nuevo_tit.EventoNuevoTitulo += titulo =>
                Console.WriteLine($"  [SUSCRIPTOR] Plataforma notificada: '{titulo}' ya disponible!");

            peliculaNueva.ObtenerNuevoTitulo(peliculaNueva.Nombre);

            // 5. ASPECTO GESTIÓN PUNTOS + EVENTO CONTENIDO VISTO
            Seccion("5. REPRODUCCIÓN CON ASPECTO DE PUNTOS + EVENTO CONTENIDO VISTO (AOP)");

            var usuario1 = usuarioService.ObtenerTodos()[0];
            Console.WriteLine($"Usuario: {usuario1.Nombre} | Puntos iniciales: {usuario1.Puntos} | Categoría: {usuario1.Categoria}");
            Console.WriteLine();

            foreach (var contenido in contenidoService.ObtenerTodos().Take(3))
            {
                contenido.UsuarioActivo = usuario1;
                contenido.Reproducir();
                Console.WriteLine();
            }

            Console.WriteLine($"Puntos acumulados: {usuario1.Puntos} | Categoría: {usuario1.Categoria}");

            // 6. EVENTO CAMBIO DE CATEGORÍA
            Seccion("6. EVENTO: CAMBIO DE CATEGORÍA (acumulando 700+ puntos)");

            Console.WriteLine($"Puntos actuales: {usuario1.Puntos}. Reproduciendo contenidos hasta categoría Pro...");
            Console.WriteLine();

            int ronda = 0;
            while (usuario1.Puntos < 700 && ronda < 20)
            {
                var c = contenidoService.ObtenerTodos()[ronda % contenidoService.ObtenerTodos().Count];
                c.UsuarioActivo = usuario1;
                c.Reproducir();
                ronda++;
            }

            Console.WriteLine($"\nPuntos finales: {usuario1.Puntos} | Categoría: {usuario1.Categoria}");

            // 7. ASPECTO RÉCORD + EVENTO NUEVO RÉCORD
            Seccion("7. JUEGO: ASPECTO RÉCORD + EVENTO NUEVO RÉCORD (AOP)");

            var juego = juegoService.ObtenerTodos()[0];
            Console.WriteLine($"Juego: {juego.Nombre} | Récord actual: {juego.Nro_record}");
            Console.WriteLine();

            juego.pub_nuevo_rec.EventoNuevoRecord += record =>
                Console.WriteLine($"  [SUSCRIPTOR] ¡Nuevo récord mundial en {juego.Nombre}: {record}!");

            string msgPuntaje = juego.RegistrarPuntaje(usuario1, 1);
            Console.WriteLine(msgPuntaje);
            Console.WriteLine($"Puntos de {usuario1.Nombre} después de jugar: {usuario1.Puntos}");
            Console.WriteLine();

            juego.ObtenerNuevoRecord(98500, usuario1, juego, juego);
            Console.WriteLine($"Récord registrado: {juego.Nro_record} por {juego.Usuario_record?.Nombre}");

            // RESUMEN FINAL
            Seccion("RESUMEN DEL SISTEMA");
            Console.WriteLine($"  Proveedor: {proveedor.Nombre}");
            Console.WriteLine($"  Usuarios  : {usuarioService.ObtenerTodos().Count}");
            Console.WriteLine($"  Contenidos: {contenidoService.ObtenerTodos().Count}");
            Console.WriteLine($"  Juegos    : {juegoService.ObtenerTodos().Count}");
            Console.WriteLine();
            Console.WriteLine("  Estado de usuarios:");
            foreach (var u in usuarioService.ObtenerTodos())
                Console.WriteLine($"    {u.Nombre,-20} | {u.Puntos,5} pts | {u.Categoria}");

            Console.WriteLine();
            Console.WriteLine("  Demo completada sin errores.");
        }

        private static void Titulo(string texto)
        {
            Console.WriteLine();
            Console.WriteLine(new string('═', 60));
            Console.WriteLine($"  {texto}");
            Console.WriteLine(new string('═', 60));
            Console.WriteLine();
        }

        private static void Seccion(string texto)
        {
            Console.WriteLine();
            Console.WriteLine($"── {texto} ──");
            Console.WriteLine(new string('─', 50));
        }
    }
}
