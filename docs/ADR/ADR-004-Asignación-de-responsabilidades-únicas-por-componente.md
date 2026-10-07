# ADR-004: Asignación de responsabilidades únicas por componente (SRP)
* Estado: Accepted
* Fecha: 2026-10-07
* Autores: Juan José Medina Sepúlveda, Manuel Alejandro Domínguez Guerrero

## Contexto
En el diseño legado, la clase `GestorCitasOdontologicas` era una "God Class" con un nivel de cohesión intermedio-bajo. Esta única clase gestionaba la lógica de negocio (agendar y cancelar), los cálculos financieros (copagos y penalizaciones), la orquestación de correos y SMS, y la ejecución de comandos SQL a la base de datos.
Esta mezcla de responsabilidades provocaba que cualquier cambio (ya fuera modificar un texto de correo, alterar un porcentaje o cambiar una tabla en BD) obligara a modificar la misma unidad de código, aumentando dramáticamente el riesgo de introducir defectos y dificultando su comprensión y mantenimiento.
Los requisitos arquitectónicamente significativos dictan que cada componente debe tener una única razón para cambiar, aislando los motivos de modificación.

---

## Decisión
Se aplicará el Principio de Responsabilidad Única (SRP), garantizando que cada clase o módulo tenga una y solo una razón para cambiar. 

La arquitectura se dividirá en componentes altamente cohesivos:
* **Orquestadores de Casos de Uso:** `ServicioAgendamiento` y `ServicioCancelacion` se encargan únicamente de coordinar el flujo (validar, calcular, guardar y notificar), sin ejecutar los detalles internos.
* **Calculadores:** `CalculadorCopago` y `CalculadorPenalizacion` se dedican exclusivamente a procesar las reglas financieras.
* **Reglas Específicas:** Cada regla (ej. `TarifaCirugia`, `RecargoPrimeraVez`) tiene la única labor de calcular su propio valor de afectación.
* **Utilidades Especializadas:** Se extrajo `ConstructorMensajesCita` para armar los textos, y `GuidGeneradorIdentificador` para crear IDs.
* **Adaptadores de Persistencia:** Las clases `RepositorioEnMemoria` e `ICitaRepositorio` se limitan exclusivamente al acceso a datos.

Se descartó dividir la lógica en microservicios debido a que el alcance del proyecto sigue siendo un monolito interno, prefiriendo la separación lógica a nivel de espacio de nombres y clases.

---

## Consecuencias

### Positivas
* LCOM = 0.00 en los servicios orquestadores, logrando una cohesión técnica perfecta (aunque este valor se deba a que tienen un solo método principal).
* Los adaptadores de memoria presentan un LCOM = 0,50 (cohesión intermedia) dado que exponen métodos separados de lectura y escritura.
* Los archivos son mucho más pequeños y fáciles de leer.
* La probabilidad de dañar el flujo de base de datos al modificar un texto de notificación es nula.

### Negativas
* Aumento significativo en la cantidad de clases y archivos (de 6 a 48 tipos).
* El flujo de ejecución es más indirecto (hay que navegar por varias clases pequeñas para entender el proceso completo de extremo a extremo).

---

## Cumplimiento (Compliance)
Se verificará durante las revisiones de código que ninguna clase asuma responsabilidades cruzadas. Por ejemplo, los repositorios no deben enviar correos, y los calculadores no deben interactuar con la base de datos. Cada nueva funcionalidad deberá empaquetarse en un servicio o clase especializada que será inyectada donde se necesite.

Trazabilidad: Informe de Métricas, secciones 7.3 a 7.5, 8.3, 8.4 y validación del Principio de Responsabilidad Única. Norma ISO/IEC/IEEE 42010:2022.
