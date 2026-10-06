# PROMPT PARA COPILOT — Refactorización DentaCare (SOLID + DIP)

> Modo recomendado: **Copilot Chat en modo Agent** con la solución abierta en Visual Studio 2022.
> Pega desde «ROL» hasta el final. Si el chat se queda corto, pégalo por fases (cada fase es independiente y termina con un `dotnet build`).

---

## ROL

Eres un arquitecto de software senior en C#. Vas a refactorizar un sistema legado aplicando SOLID y separación por capas. Es un ejercicio académico de **arquitectura**: el objetivo es la **estructura** (responsabilidades, contratos y dependencias), no la funcionalidad real. La infraestructura (SQL Server, SMTP, Twilio) puede seguir simulada como en el código legado.

## CONTEXTO

- La solución tiene el proyecto `src/DentaCare.Legacy/` con estas clases: `GestorCitasOdontologicas` (God Class), `SqlServerEjecutor`, `NotificacionServicio`, `Cita`, `Paciente`, `Odontologo` y `Program`.
- Debes crear el proyecto **`src/DentalCare.Refactored/`** (nombre de carpeta y `.csproj`: `DentalCare.Refactored`; **RootNamespace: `DentaCare.Refactored`**, para mantener la convención del namespace legado).
- Usa el **mismo `TargetFramework`, `Nullable` e `ImplicitUsings`** que `DentaCare.Legacy.csproj`. Solo añade las mismas `PackageReference` que ya use el proyecto legado (por ejemplo `System.Data.SqlClient`). **No agregues otros paquetes NuGet.**

## RESTRICCIONES (obligatorias)

1. **No modifiques nada** dentro de `DentaCare.Legacy`.
2. **Sin contenedor de IoC:** usa inyección por constructor y una *composición manual* (Pure DI) en `Program.cs`.
3. Todo tipo de `Domain` y `Application` **no puede tener** `using System.Data.SqlClient`, `System.Net.Mail` ni referencias a Twilio. Solo `Infrastructure` los conoce.
4. Sin `static` para colaboradores (nada de `Singleton`, `static class` con lógica, ni `new` de dependencias dentro de servicios). Las únicas excepciones son `record` y `enum`.
5. Clases `sealed` salvo que se justifique lo contrario. Sin métodos vacíos ni `NotImplementedException`/`NotSupportedException` (protege LSP).
6. Sin secretos en el código: lee la configuración desde variables de entorno con un valor por defecto `"CONFIGURAR"`.
7. Comentarios y XML docs breves **en español**. En cada clase nueva, un `<summary>` de una línea que indique el principio que aplica (ej. `/// SRP: calcula únicamente el copago.`).
8. Código limpio, sin lógica adicional a la del legado. **Comportamiento idéntico** (ver «Criterios de aceptación»).
9. No crees proyecto de pruebas ni cambies la estructura fuera de `src/DentalCare.Refactored/`.

## ARQUITECTURA OBJETIVO

Un solo proyecto con tres capas por carpetas. Dependencias permitidas: `Infrastructure → Application → Domain`. Las **interfaces pertenecen a `Application`** (inversión de propiedad, DIP).

```text
src/DentalCare.Refactored/
├── DentalCare.Refactored.csproj
├── Program.cs                          <- composition root
├── Domain/
│   ├── Entidades/        Cita.cs, Paciente.cs, Odontologo.cs
│   ├── Enumeraciones/    Convenio.cs, Especialidad.cs, EstadoCita.cs
│   └── Excepciones/      OdontologoNoDisponibleException.cs
├── Application/
│   ├── Abstracciones/
│   │   ├── Persistencia/     ICitaRepositorio.cs, ICancelacionRepositorio.cs
│   │   ├── Notificaciones/   INotificador.cs, ICanalNotificacion.cs, IConstructorMensajes.cs
│   │   ├── Reportes/         IRegistroReportes.cs, IConsultaReportes.cs
│   │   ├── Tarifas/          IReglaTarifa.cs, ICalculadorCopago.cs
│   │   ├── Cancelaciones/    IReglaPenalizacion.cs, ICalculadorPenalizacion.cs
│   │   ├── IValidadorDisponibilidad.cs
│   │   ├── IGeneradorIdentificador.cs
│   │   ├── IServicioAgendamiento.cs
│   │   └── IServicioCancelacion.cs
│   ├── Modelos/          SolicitudAgendamiento.cs, ContextoCancelacion.cs, MensajeNotificacion.cs
│   ├── Servicios/        ServicioAgendamiento.cs, ServicioCancelacion.cs,
│   │                     CalculadorCopago.cs, CalculadorPenalizacion.cs,
│   │                     ConstructorMensajesCita.cs, ValidadorDisponibilidadPorBandera.cs
│   ├── Tarifas/          TarifaOrtodoncia.cs, TarifaEndodoncia.cs, TarifaCirugia.cs,
│   │                     TarifaOdontopediatria.cs, DescuentoEps.cs, DescuentoPrepagada.cs,
│   │                     RecargoPrimeraVez.cs, RecargoRadiografia.cs
│   └── Cancelaciones/    PoliticaCancelacion.cs, PenalizacionCancelacionTardia.cs,
│                         RecargoCancelacionTardiaCirugia.cs
└── Infrastructure/
    ├── Configuracion/    SqlServerOpciones.cs, SmtpOpciones.cs, TwilioOpciones.cs
    ├── Persistencia/     SqlServerCitaRepositorio.cs
    ├── Notificaciones/   SmtpCanalEmail.cs, TwilioCanalSms.cs, NotificadorMultiCanal.cs
    ├── Reportes/         ReporteCitasEnMemoria.cs
    └── Identificadores/  GuidGeneradorIdentificador.cs
```

### Mapa Legacy → Refactorizado (guía de diseño)

| Responsabilidad en `GestorCitasOdontologicas` | Nuevo componente | Principio |
|---|---|---|
| Validar disponibilidad (L.14–17) | `IValidadorDisponibilidad` / `ValidadorDisponibilidadPorBandera` | SRP |
| Tarifa por especialidad (L.22–37) | `IReglaTarifa`: `TarifaOrtodoncia`, `TarifaEndodoncia`, `TarifaCirugia`, `TarifaOdontopediatria` | OCP |
| Descuento por convenio (L.40–47) | `DescuentoEps`, `DescuentoPrepagada` | OCP |
| Recargos (L.49–57) | `RecargoPrimeraVez`, `RecargoRadiografia` | OCP |
| Orquestar el cálculo | `CalculadorCopago` (`ICalculadorCopago`) | SRP |
| Penalización (L.77–83) | `IReglaPenalizacion`: `PenalizacionCancelacionTardia`, `RecargoCancelacionTardiaCirugia` + `CalculadorPenalizacion` | OCP/SRP |
| Persistencia (L.7, 68, 88) | `ICitaRepositorio` + `ICancelacionRepositorio` → `SqlServerCitaRepositorio` | DIP/ISP |
| Notificaciones (L.8, 69, 89) | `INotificador` → `NotificadorMultiCanal` sobre `ICanalNotificacion` (`SmtpCanalEmail`, `TwilioCanalSms`) | DIP/ISP/OCP |
| Texto del mensaje (`Cita.GenerarTextoConfirmacion`) | `IConstructorMensajes` / `ConstructorMensajesCita` | SRP |
| Totales en memoria (L.9–10, 71, 91) | `IRegistroReportes` + `IConsultaReportes` → `ReporteCitasEnMemoria` | SRP/ISP |
| `Guid.NewGuid().ToString().Substring(0, 8)` (L.60) | `IGeneradorIdentificador` → `GuidGeneradorIdentificador` | DIP |
| Orquestación de agendar / cancelar | `ServicioAgendamiento`, `ServicioCancelacion` | SRP |

## CONTRATOS (implementa exactamente estas firmas; puedes añadir constructores y campos privados)

### Domain

```csharp
public enum Convenio { Particular = 1, Eps = 2, Prepagada = 3 }
public enum Especialidad { Ortodoncia = 1, Endodoncia = 2, Cirugia = 3, Odontopediatria = 4 }
public enum EstadoCita { Programada, Cancelada }

public sealed class Paciente
{
    public string Id { get; init; }
    public string NombreCompleto { get; init; }
    public string Correo { get; init; }
    public string Celular { get; init; }
    public Convenio Convenio { get; init; }
    public bool EsPrimeraVez { get; init; }
}

public sealed class Odontologo
{
    public string Id { get; init; }
    public string Nombre { get; init; }
    public Especialidad Especialidad { get; init; }
    public bool EstaDisponible { get; init; }
}

public sealed class Cita
{
    public Cita(string id, Paciente paciente, Odontologo odontologo, DateTime fechaHora, decimal copagoCalculado);
    public string Id { get; }
    public Paciente Paciente { get; }
    public Odontologo Odontologo { get; }
    public DateTime FechaHora { get; }
    public decimal CopagoCalculado { get; }
    public EstadoCita Estado { get; private set; }        // inicia en Programada
    public decimal PenalizacionCancelacion { get; private set; }
    public void Cancelar(decimal penalizacion);            // Estado = Cancelada; guarda la penalización
}

public sealed class OdontologoNoDisponibleException : Exception
{
    public OdontologoNoDisponibleException()
        : base("El odontólogo no tiene disponibilidad en el horario seleccionado.") { }
}
```

`Cita` **no** debe tener `GenerarTextoConfirmacion`.

### Application — modelos

```csharp
public sealed record SolicitudAgendamiento(
    Paciente Paciente, Odontologo Odontologo, DateTime FechaHora,
    Especialidad Especialidad, bool RequiereRadiografia);

public sealed record ContextoCancelacion(Cita Cita, DateTime FechaHoraCancelacion)
{
    public TimeSpan Anticipacion => Cita.FechaHora - FechaHoraCancelacion;
}

public sealed record MensajeNotificacion(string Correo, string Celular, string Asunto, string Cuerpo);
```

### Application — abstracciones (una interfaz por rol, pequeñas: ISP)

```csharp
public interface ICitaRepositorio        { void Guardar(Cita cita); }
public interface ICancelacionRepositorio { void RegistrarCancelacion(string citaId, decimal penalizacion); }

public interface INotificador            { void Notificar(MensajeNotificacion mensaje); }
public interface ICanalNotificacion      { void Enviar(MensajeNotificacion mensaje); }
public interface IConstructorMensajes
{
    MensajeNotificacion Confirmacion(Cita cita);
    MensajeNotificacion Cancelacion(Cita cita);
}

public interface IRegistroReportes       { void RegistrarRecaudo(decimal monto); void RegistrarCancelacion(); }
public interface IConsultaReportes       { decimal ObtenerTotalRecaudado(); int ObtenerTotalCanceladas(); }

public interface IValidadorDisponibilidad { void Validar(Odontologo odontologo, DateTime fechaHora); } // lanza OdontologoNoDisponibleException
public interface IGeneradorIdentificador  { string Generar(); }

public interface IReglaTarifa
{
    int Orden { get; }                                       // define el orden de aplicación
    bool Aplica(SolicitudAgendamiento solicitud);
    decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud);
}
public interface ICalculadorCopago       { decimal Calcular(SolicitudAgendamiento solicitud); }

public interface IReglaPenalizacion
{
    bool Aplica(ContextoCancelacion contexto);
    decimal Calcular(ContextoCancelacion contexto);
}
public interface ICalculadorPenalizacion { decimal Calcular(ContextoCancelacion contexto); }

public interface IServicioAgendamiento   { Cita Agendar(SolicitudAgendamiento solicitud); }
public interface IServicioCancelacion    { decimal Cancelar(Cita cita, DateTime fechaHoraCancelacion); }
```

### Application — reglas de tarifa (todas `sealed`, `IReglaTarifa`)

| Clase | Orden | Aplica cuando | `Aplicar(valor, s)` |
|---|---:|---|---|
| `TarifaOrtodoncia` | 10 | `s.Especialidad == Ortodoncia` | `valor * 1.2m` |
| `TarifaEndodoncia` | 10 | `Endodoncia` | `valor * 1.8m` |
| `TarifaCirugia` | 10 | `Cirugia` | `valor * 2.5m` |
| `TarifaOdontopediatria` | 10 | `Odontopediatria` | `valor * 1.1m` |
| `DescuentoEps` | 20 | `s.Paciente.Convenio == Convenio.Eps` | `valor * 0.30m` |
| `DescuentoPrepagada` | 20 | `Convenio.Prepagada` | `valor * 0.10m` |
| `RecargoPrimeraVez` | 30 | `s.Paciente.EsPrimeraVez` | `valor + 20.0m` |
| `RecargoRadiografia` | 40 | `s.RequiereRadiografia` | `valor + 35.0m` |

- Los valores van como `private const` con nombre descriptivo dentro de cada clase.
- `CalculadorCopago` recibe `IEnumerable<IReglaTarifa>` por constructor, parte de `CostoConsultaBase = 100.0m` y aplica `reglas.OrderBy(r => r.Orden)` filtrando con `Aplica`. `Particular` no tiene regla y no cambia el valor (igual que el legado).

### Application — reglas de penalización

- `PoliticaCancelacion`: clase `static` **solo con constantes** (`HorasLimite = 24`), única excepción a la restricción 4.
- `PenalizacionCancelacionTardia` (`IReglaPenalizacion`): aplica si `contexto.Anticipacion.TotalHours < PoliticaCancelacion.HorasLimite`; calcula `50.0m`.
- `RecargoCancelacionTardiaCirugia`: aplica si además `contexto.Cita.Odontologo.Especialidad == Especialidad.Cirugia`; calcula `40.0m`.
- `CalculadorPenalizacion` recibe `IEnumerable<IReglaPenalizacion>` y **suma** `Calcular` de todas las que `Aplica`.

### Application — servicios (orden idéntico al legado)

`ServicioAgendamiento(IValidadorDisponibilidad, ICalculadorCopago, IGeneradorIdentificador, ICitaRepositorio, INotificador, IConstructorMensajes, IRegistroReportes)`

1. `validador.Validar(...)`
2. `copago = calculador.Calcular(solicitud)`
3. `new Cita(generador.Generar(), ..., copago)`
4. `repositorio.Guardar(cita)`
5. `notificador.Notificar(constructor.Confirmacion(cita))`
6. `reportes.RegistrarRecaudo(copago)`
7. devuelve la cita

`ServicioCancelacion(ICalculadorPenalizacion, ICancelacionRepositorio, INotificador, IConstructorMensajes, IRegistroReportes)`

1. `penalizacion = calculador.Calcular(new ContextoCancelacion(cita, fechaHoraCancelacion))`
2. `cita.Cancelar(penalizacion)`
3. `repositorio.RegistrarCancelacion(cita.Id, penalizacion)`
4. `notificador.Notificar(constructor.Cancelacion(cita))`
5. `reportes.RegistrarCancelacion()`
6. devuelve la penalización

`ConstructorMensajesCita`:
- `Confirmacion`: asunto `"Confirmación de Cita Odontológica"`; cuerpo con el texto que hoy genera `Cita.GenerarTextoConfirmacion()`.
- `Cancelacion`: asunto `"Cancelación de Cita Odontológica"`; cuerpo `$"Su cita #{cita.Id} ha sido CANCELADA."`. (Corrige el defecto del legado, que ignoraba el mensaje de cancelación.)

`ValidadorDisponibilidadPorBandera`: lanza `OdontologoNoDisponibleException` si `!odontologo.EstaDisponible`.

### Infrastructure

- `SqlServerOpciones`, `SmtpOpciones`, `TwilioOpciones`: clases simples de configuración. Se llenan en `Program.cs` con `Environment.GetEnvironmentVariable(...) ?? "CONFIGURAR"`.
- `SqlServerCitaRepositorio : ICitaRepositorio, ICancelacionRepositorio`: conserva el `SqlCommand` de `SqlServerEjecutor` (mismos INSERT/UPDATE parametrizados), pero la cadena de conexión viene de `SqlServerOpciones`. Debe **mapear** `Cita`, no exponer `SqlCommand` fuera de la clase.
- `SmtpCanalEmail : ICanalNotificacion`: usa `SmtpClient`/`MailMessage` con `mensaje.Correo`, `Asunto`, `Cuerpo`; servidor desde `SmtpOpciones`. Usa `using` para liberar los recursos.
- `TwilioCanalSms : ICanalNotificacion`: simulación con `Console.WriteLine` (sin secretos en el texto), usando `mensaje.Celular` y `Cuerpo`.
- `NotificadorMultiCanal : INotificador`: recibe `IEnumerable<ICanalNotificacion>` y envía el mensaje por todos los canales.
- `ReporteCitasEnMemoria : IRegistroReportes, IConsultaReportes`: acumuladores en memoria.
- `GuidGeneradorIdentificador : IGeneradorIdentificador`: `Guid.NewGuid().ToString("N").Substring(0, 8)`.

### Program.cs (composition root)

- Es el **único** lugar donde se usa `new` sobre clases concretas.
- Construye las reglas de tarifa (las 8) y de penalización (las 2), calculadores, canales (`SmtpCanalEmail`, `TwilioCanalSms`), notificador, repositorio, reportes y ambos servicios.
- Reproduce los **tres flujos** del `Program.cs` legado con los mismos datos y textos de consola: paciente EPS, primera vez, `Especialidad.Cirugia`, radiografía, cita a `DateTime.Now.AddHours(12)`, cancelación inmediata, y reporte de totales leyendo `IConsultaReportes`.
- Debe conservar el `try/catch` con el mensaje «ERROR CRÍTICO EN EL SISTEMA».
- Como SQL Server, SMTP y Twilio no existen en este entorno, el flujo real puede fallar al conectarse: añade la constante de compilación `SIMULACION` (bloque `#if`) o, mejor, un repositorio y canal *simulados* solo si es imprescindible para poder ejecutar. Si lo haces, **no** los cuentes como parte de la arquitectura: ponlos en `Program.cs` como clases privadas anidadas y avísame.

## CRITERIOS DE ACEPTACIÓN

1. `dotnet build` de la solución completa **sin errores y sin advertencias nuevas**.
2. Con el escenario del `Program.cs`: **copago = 130.00** (100 × 2,5 → × 0,30 → + 20 → + 35) y **penalización = 90.00** (50 + 40 por Cirugía, cita a menos de 24 h).
3. No existe ninguna clase con más de una responsabilidad reconocible ni ningún servicio de `Application` que haga `new` de una dependencia concreta.
4. Búsqueda en `Domain/` y `Application/` de `SqlClient`, `System.Net.Mail`, `Twilio`, `Console.` → **cero coincidencias**.
5. Añadir una especialidad, un convenio, un recargo, una regla de penalización o un canal nuevo debe requerir **solo una clase nueva y una línea en `Program.cs`**, sin tocar servicios ni calculadores.

## PLAN DE EJECUCIÓN (haz un `dotnet build` al final de cada fase)

1. **Andamiaje:** crea el proyecto, agrégalo a la solución (`.sln`) y copia la configuración del proyecto legado.
2. **Domain:** enumeraciones, entidades y excepción.
3. **Application/Abstracciones y Modelos:** todas las interfaces y `record`.
4. **Application/Tarifas y Cancelaciones:** las 8 reglas de tarifa, las 2 de penalización y `PoliticaCancelacion`.
5. **Application/Servicios:** calculadores, validador, constructor de mensajes y los dos servicios.
6. **Infrastructure:** configuración, repositorio, canales, notificador, reportes y generador.
7. **Program.cs:** composición y los tres flujos.
8. **Verificación:** ejecuta los criterios de aceptación 1 a 5.

## ENTREGA FINAL (responde con esto al terminar)

1. Árbol final de archivos creados.
2. **Tabla de trazabilidad** `Legacy (archivo, línea) → nuevo tipo`, para cada bloque del `GestorCitasOdontologicas`.
3. **Inventario por tipo** en una tabla: nombre, capa, si es interfaz/clase abstracta/concreta, número de métodos públicos, y los tipos del proyecto de los que depende (constructor, campos, parámetros y retornos). Márcalo como *tentativo*: lo verificaré manualmente para recalcular Ca, Ce, I, A, D y LCOM96b.
4. Lista de **desviaciones** respecto a este prompt y su justificación (si hubo alguna).
5. Resultado de la ejecución (copago y penalización obtenidos).
