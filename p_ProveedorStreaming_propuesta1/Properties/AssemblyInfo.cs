using System.Runtime.CompilerServices;

// Permite a Castle.DynamicProxy generar proxies de clases internal (Pelicula, Serie)
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

// Permite al proyecto de tests acceder a Pelicula, Serie y otras clases internal
[assembly: InternalsVisibleTo("p_ProveedorStreaming.Tests")]
