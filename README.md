# Práctica 1: Diseño Sostenible y Principios SOLID 🦷

Este repositorio contiene la entrega de la **Práctica 1 de Arquitectura de Software I**, enfocada en el diagnóstico, refactorización y documentación de una solución de software orientada a objetos en C# (DentaCare).

## 🎯 Objetivo del Proyecto

El módulo original (`DentaCare.Legacy`) contaba con una *God Class* altamente acoplada que violaba severamente los principios de diseño. Este entregable demuestra:

1. **Diagnóstico y Métricas:** Análisis matemático de cohesión (LCOM96b), acoplamiento (Ca, Ce), inestabilidad (I), y distancia a la secuencia principal (D).
2. **Refactorización:** Una nueva versión (`DentalCare.Refactored`) desacoplada utilizando Inyección de Dependencias (Pure DI), interfaces puras y patrones de diseño (Strategy, Composite).
3. **Decisiones Arquitectónicas:** Documentación estandarizada (ISO/IEC/IEEE 42010:2022) a través de 5 Architectural Decision Records (ADRs).

## 📂 Estructura del Repositorio

```text
/
├── src/
│   ├── DentaCare.Legacy/        # Código base inicial monolítico suministrado.
│   └── DentalCare.Refactored/   # Solución refactorizada aplicando los 5 principios SOLID.
├── docs/
│   ├── adr/                     # Registros de Decisiones Arquitectónicas (ADR-001 al ADR-005).
│   └── Informe_Metricas_Final.pdf # Informe detallado con cálculos matemáticos y trazabilidad.
└── Practica_1_Diseno_sostenible_y_principios_SOLID.md # Rúbrica de evaluación.
```

## 🚀 Cómo ejecutar la refactorización

El proyecto ha sido desvinculado de dependencias externas (SQL Server, SMTP, Twilio) utilizando adaptadores en memoria para facilitar su ejecución y validación técnica:

1. Abre una terminal.
2. Navega al directorio refactorizado:
   ```bash
   cd src/DentalCare.Refactored
   ```
3. Compila y ejecuta el proyecto:
   ```bash
   dotnet run
   ```

El programa ejecutará por consola dos escenarios de aceptación (EPS y Prepagada), demostrando el cálculo correcto de copagos, penalizaciones y simulación de notificaciones.

## 👥 Autor

- Juan José Medina Sepúlveda
