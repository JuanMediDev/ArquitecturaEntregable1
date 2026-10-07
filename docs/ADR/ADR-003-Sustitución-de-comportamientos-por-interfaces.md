# ADR-003: Sustitución de comportamientos por interfaces (LSP)
* Estado: Accepted
* Fecha: 2026-10-07
* Autores: Juan José Medina Sepúlveda, Manuel Alejandro Domínguez Guerrero

## Contexto
Durante la refactorización del código original (`GestorCitasOdontologicas`), nos enfrentamos al problema de que cualquier comportamiento variante obligaba a alterar el flujo principal de ejecución o requerir comprobaciones de tipos.
Para garantizar que el sistema funcione predeciblemente al inyectar distintas reglas de negocio, canales de comunicación o motores de base de datos, los componentes deben ser totalmente intercambiables. Requerimos asegurar que si un servicio demanda un contrato (`ICanalNotificacion`, `IReglaTarifa`), cualquier implementación proporcionada funcionará correctamente sin que el consumidor deba conocer sus detalles técnicos ni alterar su lógica. Los requisitos arquitectónicamente significativos incluyen la confianza plena en el polimorfismo y la prevención de excepciones inesperadas por parte de dependencias inyectadas.

---

## Decisión
Se aplicará el Principio de Sustitución de Liskov (LSP) asegurando que toda clase concreta que implemente una interfaz pueda ser sustituida por otra sin alterar la corrección del programa.

* **Reglas de Tarifa y Penalización:** Cualquier nueva clase que implemente `IReglaTarifa` o `IReglaPenalizacion` debe respetar el contrato, respetando el ciclo de vida diseñado (el orquestador valida `Aplica(contexto)` antes de invocar `Aplicar()` o `Calcular()`), sin arrojar excepciones no controladas ni obligar al `Calculador` a comprobar su tipo exacto.
* **Notificaciones:** Las implementaciones de `ICanalNotificacion` (`CanalCorreoEnMemoria`, `CanalSmsEnMemoria`) procesarán los mismos mensajes. El `NotificadorCompuesto` invoca la lista entera asumiendo que cada canal sabe cómo resolver su tarea.
* **Persistencia:** Cualquier implementación de `ICitaRepositorio` actuará bajo las mismas precondiciones y poscondiciones lógicas que se esperan (por ejemplo, devolviendo los objetos o guardándolos exitosamente), sin lanzar excepciones de "No implementado".

Se descartaron el uso de clases base abstractas con métodos virtuales, prefiriendo interfaces puras para evitar la herencia frágil. Además, se descartó el uso de introspección de tipos (`is`, `as`, `typeof`) en los orquestadores.

---

## Consecuencias

### Positivas
* El orquestador principal (`ServicioAgendamiento` o `ServicioCancelacion`) no posee condicionales para discriminar el tipo de objeto que está utilizando.
* El comportamiento del software es determinista y confiable, basándose enteramente en los contratos definidos por las interfaces.
* Fomenta el uso de polimorfismo puro.

### Negativas
* Requiere un diseño meticuloso de las interfaces para evitar obligar a las clases derivadas a manejar escenarios para los que no están diseñadas.
* Si una implementación requiere una configuración particular (como credenciales), esta no puede pedirse por el contrato común; debe resolverse en el constructor del adaptador específico, delegando esta responsabilidad a `Program.cs`.

---

## Cumplimiento (Compliance)
Se verificará que en ninguna parte del código existan sentencias como `if (dependencia is ClaseConcreta)` ni conversiones de tipo forzadas (casteos). Las interfaces deben bastar por sí solas.
Además, ninguna implementación deberá arrojar `NotImplementedException` por un método del contrato. Si una clase no puede implementar algo, es señal de que la interfaz debe segregarse.

Trazabilidad: Informe de Métricas, sección 8.2 y cumplimiento estricto del Principio de Sustitución de Liskov (LSP). Norma ISO/IEC/IEEE 42010:2022.
