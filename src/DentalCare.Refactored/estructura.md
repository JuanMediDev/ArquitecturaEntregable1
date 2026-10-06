# Estructura de la refactorización

## Propósito

Este documento describe cómo se reorganizó el módulo legado de DentaCare para eliminar la clase monolítica `GestorCitasOdontologicas` y aplicar los principios SOLID mediante interfaces, reglas independientes e inyección manual de dependencias.

La refactorización se limita al código C# del proyecto `DentalCare.Refactored`. No se incorporan base de datos, SMTP, Twilio, contenedores IoC ni proyectos adicionales.

## Situación del módulo legacy

`GestorCitasOdontologicas` concentraba varias responsabilidades:

- Validación de disponibilidad.
- Cálculo del copago por especialidad.
- Descuentos por convenio.
- Recargos por primera consulta y radiografía.
- Creación de citas e identificadores.
- Persistencia en SQL Server.
- Envío de notificaciones.
- Cancelación y cálculo de penalizaciones.
- Acumulación de totales.

Esto generaba una clase con alto acoplamiento, múltiples razones de cambio y dependencias directas de infraestructura.

## Mapa de cambios

| Responsabilidad legacy | Implementación refactorizada | Principio aplicado |
|---|---|---|
| Agendar una cita | `Servicios/ServicioAgendamiento.cs` | SRP y DIP |
| Cancelar una cita | `Servicios/ServicioCancelacion.cs` | SRP y DIP |
| Validar disponibilidad | `Servicios/ValidadorDisponibilidadPorBandera.cs` mediante `IValidadorDisponibilidad` | SRP |
| Calcular el copago | `Servicios/CalculadorCopago.cs` mediante `ICalculadorCopago` | SRP y OCP |
| Tarifas por especialidad | `Reglas/Tarifas/TarifaOrtodoncia.cs`, `TarifaEndodoncia.cs`, `TarifaCirugia.cs` y `TarifaOdontopediatria.cs` | OCP |
| Descuentos por convenio | `Reglas/Tarifas/DescuentoEps.cs` y `DescuentoPrepagada.cs` | OCP |
| Recargo de primera consulta | `Reglas/Tarifas/RecargoPrimeraVez.cs` | OCP |
| Recargo por radiografía | `Reglas/Tarifas/RecargoRadiografia.cs` | OCP |
| Penalización por cancelación tardía | `Reglas/Penalizaciones/PenalizacionCancelacionTardia.cs` | SRP y OCP |
| Recargo tardío de cirugía | `Reglas/Penalizaciones/RecargoCancelacionTardiaCirugia.cs` | OCP |
| Orquestar penalizaciones | `Servicios/CalculadorPenalizacion.cs` mediante `ICalculadorPenalizacion` | SRP y OCP |
| Crear identificadores | `Servicios/GuidGeneradorIdentificador.cs` mediante `IGeneradorIdentificador` | DIP |
| Construir mensajes | `Servicios/ConstructorMensajesCita.cs` mediante `IConstructorMensajes` | SRP |
| Registrar totales | `Servicios/ReporteCitasEnMemoria.cs` mediante `IRegistroReportes` | SRP e ISP |
| Consultar totales | `Servicios/ReporteCitasEnMemoria.cs` mediante `IConsultaReportes` | ISP |
| Guardar citas | `Servicios/RepositorioEnMemoria.cs` mediante `ICitaRepositorio` | DIP e ISP |
| Registrar cancelaciones | `Servicios/RepositorioEnMemoria.cs` mediante `ICancelacionRepositorio` | DIP e ISP |
| Notificar al paciente | `Servicios/NotificadorEnMemoria.cs` mediante `INotificador` | DIP e ISP |
| Texto de confirmación y cancelación | `Modelos/MensajeNotificacion.cs` y `ConstructorMensajesCita.cs` | SRP |

## Organización de carpetas

```text
src/DentalCare.Refactored/
├── DentalCare.Refactored.csproj
├── Program.cs
├── estructura.md
├── Entidades/
│   ├── Cita.cs
│   ├── Paciente.cs
│   └── Odontologo.cs
├── Enumeraciones/
│   ├── Convenio.cs
│   ├── Especialidad.cs
│   └── EstadoCita.cs
├── Excepciones/
│   └── OdontologoNoDisponibleException.cs
├── Interfaces/
│   ├── ICalculadorCopago.cs
│   ├── ICalculadorPenalizacion.cs
│   ├── ICanalNotificacion.cs
│   ├── ICancelacionRepositorio.cs
│   ├── ICitaRepositorio.cs
│   ├── IConstructorMensajes.cs
│   ├── IConsultaReportes.cs
│   ├── IGeneradorIdentificador.cs
│   ├── INotificador.cs
│   ├── IRegistroReportes.cs
│   ├── IReglaPenalizacion.cs
│   ├── IReglaTarifa.cs
│   ├── IServicioAgendamiento.cs
│   ├── IServicioCancelacion.cs
│   └── IValidadorDisponibilidad.cs
├── Modelos/
│   ├── ContextoCancelacion.cs
│   ├── MensajeNotificacion.cs
│   └── SolicitudAgendamiento.cs
├── Reglas/
│   ├── PoliticaCancelacion.cs
│   ├── Penalizaciones/
│   │   ├── PenalizacionCancelacionTardia.cs
│   │   └── RecargoCancelacionTardiaCirugia.cs
│   └── Tarifas/
│       ├── DescuentoEps.cs
│       ├── DescuentoPrepagada.cs
│       ├── RecargoPrimeraVez.cs
│       ├── RecargoRadiografia.cs
│       ├── TarifaCirugia.cs
│       ├── TarifaEndodoncia.cs
│       ├── TarifaOdontopediatria.cs
│       └── TarifaOrtodoncia.cs
└── Servicios/
	├── CalculadorCopago.cs
	├── CalculadorPenalizacion.cs
	├── ConstructorMensajesCita.cs
	├── GuidGeneradorIdentificador.cs
	├── NotificadorEnMemoria.cs
	├── ReporteCitasEnMemoria.cs
	├── RepositorioEnMemoria.cs
	├── ServicioAgendamiento.cs
	├── ServicioCancelacion.cs
	└── ValidadorDisponibilidadPorBandera.cs
```

## Contratos y dependencias

Las interfaces se encuentran en `Interfaces/` y definen los contratos que consumen los servicios:

- `ServicioAgendamiento` depende de `ICalculadorCopago`, `IGeneradorIdentificador`, `IValidadorDisponibilidad`, `IRegistroReportes`, `IConstructorMensajes`, `INotificador` e `ICitaRepositorio`.
- `ServicioCancelacion` depende de `ICalculadorPenalizacion`, `IRegistroReportes`, `IConstructorMensajes`, `INotificador` e `ICancelacionRepositorio`.
- `CalculadorCopago` depende de una colección de `IReglaTarifa`.
- `CalculadorPenalizacion` depende de una colección de `IReglaPenalizacion`.
- Los servicios no conocen clases concretas de SQL Server, SMTP o Twilio.
- Las implementaciones concretas se ensamblan manualmente en `Program.cs`.

## Composición manual

`Program.cs` funciona como composition root. Allí se crean:

1. Las reglas de tarifas.
2. Las reglas de penalización.
3. Los calculadores.
4. El validador.
5. El generador de identificadores.
6. El reporte en memoria.
7. El constructor de mensajes.
8. El repositorio en memoria.
9. El notificador en memoria.
10. Los servicios de agendamiento y cancelación.

No se utiliza un contenedor de inversión de control.

## Aplicación de SOLID

### SRP — Single Responsibility Principle

Las responsabilidades se distribuyeron entre servicios, reglas, entidades y adaptadores. `ServicioAgendamiento` y `ServicioCancelacion` coordinan sus respectivos casos de uso, mientras que los cálculos, validaciones, mensajes y persistencia tienen colaboradores separados.

### OCP — Open/Closed Principle

Las tarifas, descuentos, recargos y penalizaciones se implementan mediante `IReglaTarifa` e `IReglaPenalizacion`. Una nueva regla puede agregarse como una clase independiente sin modificar el algoritmo de cálculo.

### LSP — Liskov Substitution Principle

Los servicios consumen interfaces y las implementaciones actuales respetan los contratos definidos. Las clases concretas no dependen de jerarquías de negocio extensibles y se declaran `sealed` cuando no requieren herencia.

### ISP — Interface Segregation Principle

Los contratos se dividieron por responsabilidad: persistencia de citas, cancelaciones, notificaciones, reportes, reglas y servicios de aplicación. Los clientes no necesitan depender de métodos que no utilizan.

### DIP — Dependency Inversion Principle

Los servicios dependen de abstracciones y no de persistencia, notificación o generación de identificadores concretos. Las dependencias se proporcionan por constructor y las implementaciones se seleccionan en `Program.cs`.

## Implementaciones locales

Para mantener el alcance sin infraestructura externa se utilizan dos adaptadores en memoria:

- `RepositorioEnMemoria`: conserva las citas y cancelaciones durante la ejecución.
- `NotificadorEnMemoria`: conserva los mensajes generados durante la ejecución.

Estas clases permiten demostrar los contratos y la inyección de dependencias sin conexión a SQL Server, correo electrónico o servicios SMS.

## Caso de ejecución validado

`Program.cs` ejecuta actualmente el escenario equivalente al caso solicitado:

- Paciente con convenio EPS.
- Primera consulta.
- Especialidad Cirugía.
- Radiografía requerida.
- Cancelación con 12 horas de anticipación.

Resultado esperado y observado:

```text
Copago: 130,00
Penalización: 90,00
Total recaudado: 130,00
Total canceladas: 1
```

## Límites del alcance

Esta refactorización no incluye:

- Implementación de conexión a SQL Server.
- Envío real de correo electrónico.
- Integración real con Twilio.
- Contenedor IoC.
- Nuevos proyectos.
- Pruebas automatizadas.
- Cambios en `src/DentaCare.Legacy`.

El objetivo es demostrar una estructura C# desacoplada y mantenible mediante SOLID, interfaces e inyección de dependencias.
