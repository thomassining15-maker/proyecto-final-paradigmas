# Explicación completa del proyecto — Proveedor de Streaming

> Guía práctica para entender toda la lógica del sistema antes de la sustentación.

---

## ¿Qué hace este programa?

Simula el backend de una plataforma de streaming (tipo Netflix + Steam). Permite:
- Registrar usuarios, películas, series y juegos
- Acumular puntos cuando un usuario ve contenido o juega
- Cambiar la categoría del usuario automáticamente según sus puntos
- Notificar eventos (nuevo título, nuevo récord, cambio de categoría, contenido visto)
- Validar datos y autenticar usuarios mediante **aspectos AOP** (código que se ejecuta "alrededor" de otros métodos sin modificarlos)

---

## Estructura de carpetas

```
p_ProveedorStreaming_propuesta1/
│
├── Clases/
│   ├── strContenido/        → Película y Serie (el catálogo de streaming)
│   ├── strCuenta/           → Cuenta (vincula un usuario con lo que ha visto)
│   ├── strJuego/            → Juego y sus servicios
│   ├── strProveedor/        → Proveedor (el "Netflix" que contiene todo)
│   ├── strUsuario/          → Usuario (quien tiene puntos y categoría)
│   ├── AccesoSistema.cs     → Implementa la autenticación
│   └── RegistroBD.cs        → Simula guardar en base de datos
│
├── Interfaces/              → Contratos que deben cumplir las clases
├── Aspectos/                → Los interceptores AOP (el "código invisible")
├── Eventos/                 → Los publicadores de eventos (patrón Observer)
├── Factories/               → La fábrica que crea objetos con aspectos
├── Archivos/                → Datos de prueba en .txt
└── Program.cs               → Demo que muestra todo funcionando
```

---

## Las clases del dominio

### Usuario
Es la persona que usa la plataforma. Tiene:
- `id_interno` — número aleatorio entre 1 y 9999
- `nombre`, `fecha_afiliacion`
- `puntos` — se acumulan al ver contenido o jugar
- `categoria` — cambia según los puntos: **General** (< 700), **Pro** (700–1999), **Master** (≥ 2000)

```
Puntos:   0 ──────── 700 ──────── 2000 ──────────→
Categoría: General     Pro          Master
```

**Método clave: `CambiarCategoria(usuario)`**
Lee los puntos del usuario y actualiza su categoría. Si la categoría cambió, dispara el evento `PublisherCambioCategoria`.

---

### Contenido (clase abstracta)
Es la base de Película y Serie. Una clase abstracta significa que **no se puede instanciar directamente** — solo sirve como molde.

Implementa la interfaz `IActualizacionPuntos`, que exige dos métodos:
- `Reproducir()` → simula que el usuario empieza a ver algo
- `ActualizarPuntaje()` → suma puntos al usuario activo

Tiene una propiedad especial: `UsuarioActivo`. **Antes de llamar `Reproducir()`, siempre hay que asignar qué usuario está viendo ese contenido:**
```csharp
contenido.UsuarioActivo = usuario;
contenido.Reproducir();  // el interceptor llama ActualizarPuntaje() automáticamente
```

---

### Película (hereda de Contenido)
Añade `Duracion` (TimeSpan) y `Calificacion` (1 o 2).
- Si `duracion < 1:30` → **50 puntos** (película corta)
- Si `duracion ≥ 1:30` → **50 puntos** (película larga)
- Calificación fuera del rango 1–2 **lanza una excepción** en el constructor.

### Serie (hereda de Contenido)
Añade `Temporadas` y `Cap_x_temp` (capítulos por temporada).
- Cada vez que se llama `Reproducir()` → representa **un episodio** → **28 puntos**
- Mínimo 1 temporada y 5 capítulos por temporada (validado en el constructor).

---

### Juego
Implementa `INuevoRecord`. Tiene `nombre`, `genero`, `nro_record` y `usuario_record` (quién tiene el récord).

**Métodos clave:**
- `RegistrarPuntaje(usuario, puesto)` — suma puntos al usuario según su posición:
  - 1° puesto → 30 pts
  - 2° puesto → 20 pts
  - 3° puesto → 10 pts
  - Resto → 5 pts
- `ObtenerNuevoRecord(nro, usuario, juego, validador)` — registra el nuevo récord. Antes de ejecutarse, el interceptor verifica que el nuevo número supere al récord actual.

---

### Cuenta
Une un `Usuario` con la lista de contenido que ha visto. Es un historial.

### Proveedor
Es el "Netflix" del sistema. Tiene tres listas: `l_cuentas`, `l_contenidos`, `l_juegos`. Agrupa todo.

---

## Las interfaces — ¿Para qué sirven?

Una interfaz es un **contrato**: si una clase dice "implemento esta interfaz", está obligada a tener esos métodos.

| Interfaz | Quién la implementa | Qué garantiza |
|----------|--------------------|-|
| `IActualizacionPuntos` | `Contenido` (Película, Serie) | Tiene `Reproducir()` y `ActualizarPuntaje()` |
| `ICambioCategoria` | `Usuario` | Tiene `CambiarCategoria(usuario)` |
| `INuevoRecord` | `Juego` | Tiene `ObtenerNuevoRecord(...)` |
| `INuevoTitulo` | — | Tiene `ObtenerNuevoTitulo(...)` |
| `IRegistroEntidad` | `RegistroBD` | Tiene `Registrar(objeto)` |
| `IAcceso` | `AccesoSistema` | Tiene `IngresarSistema(usuario, clave)` |

Las interfaces también son necesarias para que **Castle.DynamicProxy** pueda crear proxies de interfaz (`CreateInterfaceProxyWithTarget`).

---

## Los Aspectos (AOP) — El corazón del proyecto

### ¿Qué es AOP?
AOP significa *Aspect-Oriented Programming* (Programación Orientada a Aspectos). La idea es separar responsabilidades que "cruzan" todo el sistema — como logging, validación o seguridad — sin mezclarlas dentro de la lógica de negocio.

**Analogía:** imagina que cada vez que alguien abre la puerta de tu casa, una cámara graba automáticamente, sin que la puerta "sepa" que está siendo grabada. El interceptor es la cámara.

### ¿Cómo funciona en este proyecto?

Se usa la librería **Castle.DynamicProxy**. Ella crea un **proxy**: un objeto falso que envuelve al objeto real. Cuando llamas a un método del proxy, primero pasa por el interceptor.

```
Tu código llama:   proxy.Reproducir()
                        ↓
               [Interceptor_GestionPuntos]
                   invocation.Proceed()  ← ejecuta el método real
                        ↓
               objeto_real.Reproducir()
                        ↓
               [Interceptor llama ActualizarPuntaje()]
```

### Los 4 interceptores

#### `Interceptor_GestionPuntos`
- **Cuándo actúa:** después de que se llama `Reproducir()` en un contenido
- **Qué hace:**
  1. Sincroniza `UsuarioActivo` del proxy al objeto real (necesario porque son objetos distintos en Castle)
  2. Llama `ActualizarPuntaje()` en el objeto real → que suma puntos al usuario

```csharp
// FLUJO COMPLETO de reproducir una película:
pelicula.UsuarioActivo = maria;      // 1. asignar usuario al proxy
pelicula.Reproducir();               // 2. interceptor actúa
// ↑ automáticamente ocurre:
//   - pelicula_real.UsuarioActivo = maria  (sincronización)
//   - pelicula_real.ActualizarPuntaje()    (suma 50 pts a maria)
//   - maria.CambiarCategoria(maria)        (actualiza categoría si aplica)
//   - evento ContenidoVisto se dispara
```

#### `Interceptor_GestionRecord`
- **Cuándo actúa:** cuando se llama `ObtenerNuevoRecord()` en un juego
- **Qué hace:**
  1. **ANTES:** verifica que el nuevo récord supere al actual. Si no, cancela la operación.
  2. Ejecuta el método real
  3. **DESPUÉS:** sincroniza `Nro_record` y `Usuario_record` del objeto real al proxy

```csharp
// Si el récord actual es 50000 y llamas con 30000:
juego.ObtenerNuevoRecord(30000, usuario, juego, juego);
// → Interceptor cancela: "30000 no supera 50000. Operación cancelada."

// Si llamas con 98500:
juego.ObtenerNuevoRecord(98500, usuario, juego, juego);
// → Interceptor valida, procede, sincroniza, dispara evento NuevoRecord
```

#### `Interceptor_Validacion`
- **Cuándo actúa:** cuando se llama `Registrar(objeto)` en `RegistroBD`
- **Qué hace:**
  1. **ANTES:** verifica que el objeto no sea nulo. Si es nulo, lanza excepción.
  2. Ejecuta el método real
  3. **DESPUÉS:** imprime "Guardado simulado en BD: OK"

#### `Interceptor_Autenticacion`
- **Cuándo actúa:** cuando se llama `IngresarSistema(usuario, clave)` en `AccesoSistema`
- **Qué hace:**
  1. **ANTES:** registra el intento de acceso. Si usuario o clave están vacíos, lanza excepción.
  2. Ejecuta el método real (que verifica credenciales)
  3. **DESPUÉS:** imprime CONCEDIDO o DENEGADO según el resultado

---

### La MensajeFactory — cómo se crean los proxies

La `MensajeFactory` es la única clase que sabe cómo combinar objetos reales con interceptores. Tiene 4 métodos:

```csharp
// Para contenido (Película, Serie):
factory.CrearMensajeGestionP(pelicula)
// → devuelve un proxy que intercepta Reproducir()

// Para juegos:
factory.CrearMensajeGestionR(juego)
// → devuelve un proxy que intercepta ObtenerNuevoRecord()

// Para guardar en BD:
factory.CrearMensajeValidacion(new RegistroBD())
// → devuelve un proxy que intercepta Registrar()

// Para autenticación:
factory.CrearMensajeAutenticacion(new AccesoSistema())
// → devuelve un proxy que intercepta IngresarSistema()
```

**¿Por qué necesita Castle un constructor sin parámetros?**
Cuando Castle crea una subclase proxy de `Pelicula`, necesita poder instanciarla sin argumentos. Por eso `Pelicula`, `Serie`, `Contenido` y `Juego` tienen un constructor `protected` vacío — solo para uso interno de Castle.

**¿Por qué los métodos son `virtual`?**
Castle solo puede interceptar métodos marcados como `virtual`. Si un método no es virtual, el proxy lo hereda directamente sin pasar por el interceptor.

---

## Los Eventos (patrón Observer)

### ¿Qué es Observer?
Es un patrón donde un objeto (el *publicador*) avisa a otros (los *suscriptores*) cuando algo pasa. Como una alerta de correo: el sistema publica "llegó un mensaje" y tú recibes la notificación.

### Los 4 publishers del proyecto

Todos siguen la misma estructura:

```csharp
// 1. Definir la firma del evento (delegate)
public delegate void dele_nuevo_tit(object titulo);

// 2. Declarar el evento
public event dele_nuevo_tit EventoNuevoTitulo;

// 3. Método que dispara el evento
public string InformarNuevoTitulo(object titulo)
{
    EventoNuevoTitulo?.Invoke(titulo);  // llama a todos los suscriptores
    return $"Notificación enviada para: {titulo}";
}
```

| Publisher | Dónde vive | Cuándo se dispara |
|-----------|-----------|-------------------|
| `PublisherNuevoTitulo` | `Contenido`, `Juego` | Cuando llega un nuevo título al sistema |
| `PublisherJuego` | `Juego` | Cuando se rompe el récord de un juego |
| `PublisherCambioCategoria` | `Usuario` | Cuando el usuario sube de categoría |
| `PublisherContenidoVisto` | `Contenido` | Cuando un usuario termina de ver algo |

### ¿Cómo se suscribe alguien a un evento?

Con el operador `+=`:
```csharp
// Suscribirse:
juego.pub_nuevo_rec.EventoNuevoRecord += MiMetodo;

// El método suscrito:
void MiMetodo(ulong record) {
    Console.WriteLine($"Nuevo récord: {record}");
}

// Cuando el evento se dispara, se llama MiMetodo automáticamente.
```

En el proyecto, las clases se suscriben a sus propios eventos en el constructor:
```csharp
// En Contenido.cs (constructor):
pub_contenido_visto.EventoContenidoVisto += EventHandlerContenidoVisto;
// → cuando alguien dispara el evento, la propia clase lo maneja
```

---

## Los Servicios — el CRUD del sistema

Cada entidad tiene su servicio que gestiona la lista en memoria:

| Servicio | Responsabilidad |
|----------|----------------|
| `UsuarioService` | Agregar, buscar usuarios, cargar desde archivo |
| `ContenidoService` | Agregar contenido **con aspecto de puntos activo**, buscar, cargar |
| `JuegoService` | Agregar juegos **con aspecto de récord activo**, buscar, cargar |
| `CuentaService` | Crear cuentas, registrar visualizaciones |
| `ProveedorService` | Registrar cuentas en el proveedor, listar todo |

**Nota importante sobre `ContenidoService.Agregar()`:**
Cuando agregas contenido, el servicio lo envuelve automáticamente en un proxy antes de guardarlo en la lista:
```csharp
public void Agregar(Contenido contenido) {
    // NO guarda el objeto original, guarda el PROXY con el interceptor activo
    Contenido contenidoConAspecto = (Contenido)_factory.CrearMensajeGestionP(contenido);
    _contenidos.Add(contenidoConAspecto);
}
```
Así, cuando recuperas contenido de la lista y llamas `Reproducir()`, los puntos se suman automáticamente.

---

## Carga desde archivos

Cada servicio tiene un método `Cargar(nombreArchivo)` que lee un `.txt` de la carpeta `Archivos/`.

**Formato de los archivos:**

```
# Usuarios.txt — una línea por usuario
Maria Gomez
Carlos Perez

# Contenidos.txt — P para película, S para serie
P|Interstellar|02:49:00|1        ← tipo|nombre|duración|calificación
S|Breaking Bad|5|10              ← tipo|nombre|temporadas|caps_por_temp

# Juegos.txt — nombre y género
FIFA 25|Deportes
Minecraft|Aventura
```

Las líneas que empiezan con `#` son ignoradas (comentarios). Si el formato es incorrecto, el método lanza una excepción con el mensaje de la línea problemática.

---

## Flujo completo de una sesión de usuario

Así funciona todo junto cuando corres `dotnet run`:

```
1. Se crean los 4 interceptores y la MensajeFactory

2. Autenticación:
   - AccesoSistema envuelto en proxy con Interceptor_Autenticacion
   - IngresarSistema("admin","1234") → interceptor registra → AccesoSistema verifica → true

3. Carga de datos:
   - UsuarioService.Cargar("Usuarios.txt") → crea 5 usuarios
   - ContenidoService.Cargar("Contenidos.txt") → crea 6 contenidos (cada uno ya con proxy AOP)
   - JuegoService.Cargar("Juegos.txt") → crea 5 juegos (cada uno ya con proxy AOP)

4. Validación BD:
   - RegistroBD envuelto en proxy con Interceptor_Validacion
   - Registrar(usuario) → interceptor valida → RegistroBD guarda → "OK en BD"

5. Evento nuevo título:
   - Se suscribe un manejador al evento EventoNuevoTitulo de la primera película
   - Se llama ObtenerNuevoTitulo() → publisher invoca al suscriptor → mensaje en consola

6. Reproducir contenido (AOP en acción):
   - contenido.UsuarioActivo = maria
   - contenido.Reproducir()
     ↓ interceptor actúa
   - Se sincroniza UsuarioActivo al objeto real
   - Se llama ActualizarPuntaje() en el objeto real
     ↓ en Pelicula:
   - maria.SumarPuntos(50)
   - maria.CambiarCategoria(maria)  → sin cambio aún (150 pts < 700)
   - pub_contenido_visto.InformarContenidoVisto() → evento disparado

7. Cambio de categoría:
   - Se reproduce más contenido hasta acumular 700+ pts
   - CambiarCategoria detecta el cruce del umbral
   - Categoría cambia de General → Pro
   - pub_cambio_cat.InformarCambioCategoria() → evento disparado

8. Juego (AOP en acción):
   - juego.RegistrarPuntaje(maria, puesto=1) → +30 pts a maria
   - juego.ObtenerNuevoRecord(98500, maria, juego, juego)
     ↓ interceptor actúa ANTES
   - Valida: 98500 > 0 (récord actual) → válido, procede
   - Se actualiza nro_record = 98500, usuario_record = maria
     ↓ interceptor actúa DESPUÉS
   - Sincroniza el estado del target al proxy
   - pub_nuevo_rec.InformarNuevoRecord(98500) → evento disparado
```

---

## Por qué `virtual` en tantos métodos

Cuando Castle crea un proxy de clase (`CreateClassProxyWithTarget`), crea una **subclase** del tipo original. Solo puede interceptar (y delegar al target) los métodos que son `virtual` — los demás se heredan normalmente y se ejecutan en el proxy, no en el target.

Esto causa un problema: si un método no-virtual modifica `this.campo`, lo modifica en el **proxy** (que tiene sus propios campos), pero el **target** (el objeto real) no se entera. Por eso se marcaron como `virtual`:

- `Nombre`, `Genero` → para que `proxy.Nombre` lea del target, no del proxy vacío
- `Nro_record`, `Usuario_record` → para poder sincronizar target→proxy en el interceptor
- `ObtenerNuevoRecord`, `Reproducir`, `ActualizarPuntaje`, `RegistrarPuntaje` → para que Castle pueda interceptarlos y delegarlos al target

---

## Cómo correr el proyecto

```bash
# Clonar / actualizar
git clone https://github.com/thomassining15-maker/proyecto-final-paradigmas
cd proyecto-final-paradigmas
git checkout cambios-estructura

# Compilar y correr la demo
cd p_ProveedorStreaming_propuesta1
dotnet run

# Correr los tests (30 tests)
cd ../p_ProveedorStreaming_propuesta1.Tests
dotnet test

# Ver detalle de cada test
dotnet test --logger "console;verbosity=normal"
```

---

## Resumen de patrones implementados

| Patrón | Dónde | Para qué |
|--------|-------|----------|
| **Herencia** | `Pelicula`, `Serie` heredan de `Contenido` | Reutilizar lógica base y polimorfismo |
| **Polimorfismo** | `ActualizarPuntaje()` se comporta distinto en Película vs Serie | Cada tipo da sus propios puntos |
| **Interfaces** | `IActualizacionPuntos`, `ICambioCategoria`, etc. | Contratos que permiten AOP y DI |
| **AOP** | 4 interceptores con Castle.DynamicProxy | Validar, autenticar y registrar sin tocar la lógica de negocio |
| **Factory** | `MensajeFactory` | Crear proxies con aspectos de forma centralizada |
| **Observer/Eventos** | 4 publishers con delegates | Notificar cambios desacoplando quien publica de quien escucha |
| **Service Layer** | 5 servicios | Separar la lógica de acceso a datos de las clases de dominio |
| **Reglas de negocio estáticas** | `ReglasNegocioUsuario`, `ReglasNegocioContenido`, `ReglasNegocioJuego` | Centralizar constantes y umbrales en un solo lugar |
