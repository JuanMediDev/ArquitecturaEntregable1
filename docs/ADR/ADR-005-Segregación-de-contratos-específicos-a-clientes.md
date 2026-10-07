# ADR-005: Segregación de contratos específicos a clientes (ISP)
* Estado: Accepted
* Fecha: 2026-10-07
* Autores: Juan José Medina Sepúlveda, Manuel Alejandro Domínguez Guerrero

## Contexto
A medida que se separaron las responsabilidades en abstracciones, existía el riesgo de generar interfaces monolíticas. Por ejemplo, una gran interfaz `IRepositorio` que incluyera métodos para agendar, cancelar, y emitir reportes. 
Si esto sucediera, un servicio que solo necesite cancelar citas (`ServicioCancelacion`) estaría forzado a conocer los métodos de agendamiento o reportes estadísticos, acoplándolo a conceptos que no requiere para funcionar.

---

## Decisión
Se aplicará el Principio de Segregación de Interfaces (ISP), dividiendo los contratos en roles granulares y altamente específicos según el caso de uso del cliente que las consume.

* En la persistencia, se crearon dos puertos separados: `ICitaRepositorio` (para creación/agendamiento) e `ICancelacionRepositorio` (para afectar el estado durante la cancelación).
* Para el acceso a estadísticas, se separó `IRegistroReportes` (utilizado por los servicios orquestadores para incrementar contadores) de `IConsultaReportes` (utilizado potencialmente por controladores de vista para ver la estadística actual).
* El componente en memoria `RepositorioEnMemoria` y `ReporteCitasEnMemoria` implementan múltiples interfaces simultáneamente, pero los servicios orquestadores solo solicitan y conocen la interfaz que les compete.

Se descartó la creación de interfaces genéricas de repositorios (tipo `IRepository<T>`) porque los requerimientos de cada entidad varían (Citas guarda todas las variables, Cancelaciones solo actualiza un estado).

---

## Consecuencias

### Positivas
* El acoplamiento aferente/eferente es limpio; los servicios solo dependen de los fragmentos exactos que utilizan.
* Si el contrato de consulta estadística (`IConsultaReportes`) cambia, no afecta al `ServicioCancelacion` ni lo obliga a recompilar, ya que este solo depende de `IRegistroReportes`.
* Hace que los *mocks* en las pruebas unitarias sean extremadamente fáciles de configurar (menos métodos que simular).

### Negativas
* Multiplica la cantidad de archivos de interfaz.
* Puede parecer redundante que una sola clase concreta (`RepositorioEnMemoria`) deba heredar de múltiples interfaces, pero es el costo asumido por mantener el aislamiento de los consumidores.

---

## Cumplimiento (Compliance)
Durante las auditorías de diseño, se verificará que ninguna clase cliente consuma una interfaz de la que solo utiliza una fracción de sus métodos. Si se detecta un patrón de métodos "huérfanos" (no usados por el cliente), la interfaz implicada deberá fraccionarse (segregarse).

Trazabilidad: Informe de Métricas, sección 8.5 y cumplimiento del Principio de Segregación de Interfaces (ISP). Norma ISO/IEC/IEEE 42010:2022.
