using System.Runtime.CompilerServices;

// Permite a Castle.DynamicProxy generar proxies de clases internal
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

// Permite al proyecto de tests acceder a clases internal
[assembly: InternalsVisibleTo("p_ProveedorStreaming.Tests")]

// Permite al proyecto MVC web acceder a clases internal
[assembly: InternalsVisibleTo("AppStreaming.web")]

// Permite al proyecto demo consola acceder a clases internal
[assembly: InternalsVisibleTo("AppStreaming.Demo")]
