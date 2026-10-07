# Métricas de Arquitectura (Comparativa)

Este documento recopila la información necesaria para aplicar y analizar las métricas de abstracción, acoplamiento e inestabilidad entre la versión original (Legacy) y la refactorizada de DentaCare.

---

## 1. Conceptos y Fórmulas Base

* **A (Abstractness - Abstracción):** `A = Na / Nc`
  Donde `Na` es el número de abstracciones (interfaces y clases abstractas) y `Nc` es el número total de tipos (clases concretas + abstracciones).
  * *Un valor de 0 indica un sistema totalmente concreto.*
  * *Un valor de 1 indica un sistema totalmente abstracto.*
* **Ce (Acoplamiento Eferente):** Cantidad de clases externas de las que depende un módulo (fan-out). Mide la vulnerabilidad al cambio.
* **Ca (Acoplamiento Aferente):** Cantidad de clases externas que dependen de un módulo (fan-in). Mide la responsabilidad.
* **I (Instability - Inestabilidad):** `I = Ce / (Ca + Ce)`
  * *Un valor de 0 indica un módulo completamente estable (muchos dependen de él, él no depende de nadie).*
  * *Un valor de 1 indica un módulo completamente inestable (depende de muchos, nadie depende de él).*
* **D (Distancia a la Secuencia Principal):** `D = |A + I - 1|`
  * Mide qué tan equilibrado está un módulo. El ideal es `0`. 
  * `(A=0, I=0)`: Zona de Dolor (Pain Zone) - Difícil de cambiar, fuertemente acoplado.
  * `(A=1, I=1)`: Zona de Inutilidad (Useless Zone) - Abstracciones que nadie implementa.

---

## 2. Sistema Legado (DentaCare.Legacy)

El sistema legado se caracteriza por tener toda su lógica concentrada en una única clase monolítica.

### Abstracción (A)
* **Na (Interfaces):** 0
* **Nc (Total Tipos):** 6 (`GestorCitasOdontologicas`, `SqlServerEjecutor`, `NotificacionServicio`, `Cita`, `Paciente`, `Odontologo`)
* **A = 0 / 6 = 0,00**
* *Análisis:* Sistema 100% concreto.

### Acoplamiento e Inestabilidad (I) - Foco en la clase principal `GestorCitasOdontologicas`
* **Ce (Dependencias de salida):** Depende de 5 tipos concretos de la solución (`SqlServerEjecutor`, `NotificacionServicio`, `Cita`, `Paciente`, `Odontologo`). Ce = 5.
* **Ca (Dependencias de entrada):** `Program.cs` lo crea con `new` y lo usa. Ca = 1.
* **I = 5 / (1 + 5) = 0,833**

### Distancia (D) y Ubicación
* **D = |0,00 + 0,833 - 1| = 0,167**
* **Ubicación en la Zona de Dolor:** No. Aunque la clase es sumamente problemática debido a su complejidad ciclomática y ausencia de abstracciones, matemáticamente no cae en la Zona de Dolor porque su inestabilidad (`I`) es alta (0,833).

---

## 3. Sistema Refactorizado (DentalCare.Refactored)

El diseño actual distribuye responsabilidades en interfaces, DTOs, servicios y políticas, conectadas a través de la inyección de dependencias. Se debe mantener el mismo criterio de medición (contar todos los tipos de datos consumidos) para que la comparación sea válida.

### Abstracción (A)
* **Na (Interfaces):** 15
* **Clases Concretas:** 33 (12 servicios, 10 reglas, 1 `PoliticaCancelacion`, 3 entidades, 3 records/modelos, 3 enumeraciones y 1 excepción).
* **Nc (Total Tipos):** 48 (15 + 33)
* **A = 15 / 48 = 0,31**
* *Análisis:* Se incrementó drásticamente la capacidad de abstracción. Un 31% del código corresponde a contratos puros.

### Acoplamiento e Inestabilidad (I) - Foco en la clase principal `ServicioAgendamiento`
* **Ce (Dependencias de salida):** Depende de las 7 interfaces inyectadas, más `IServicioAgendamiento` (la interfaz que implementa), `SolicitudAgendamiento` (el parámetro) y `Cita` (el retorno y objeto manipulado). Ce = 10.
* **Ca (Dependencias de entrada):** `Program.cs` la instancia directamente. Ca = 1.
* **I de `ServicioAgendamiento` = 10 / (1 + 10) = 0,909**
* **I de las interfaces consumidas = 0,09 a 0,33** (Las interfaces sí dependen de otros tipos; por ejemplo, `IReglaTarifa` depende de `SolicitudAgendamiento` dando un I=0,09; `ICitaRepositorio` depende de `Cita` dando un I=0,33).

### Distancia (D) y Ubicación
* **D de la clase principal = |0,00 + 0,909 - 1| = 0,091** (Siendo una clase concreta, A=0).
* **D de las interfaces = 0,09 a 0,33** (Dado que A=1 y su I varía).
* **Ubicación:** Ni el gestor legado ni los servicios orquestadores refactorizados caen en la Zona de Dolor. Sin embargo, la refactorización reduce la distancia `D` de 0,167 a 0,091. Lo verdaderamente defendible es que **el negocio ya no depende de infraestructura concreta** (pasando de dependencias concretas a interfaces) y que la cohesión (medida externamente con LCOM) mejoró de 0,50 a 0,00. Las únicas estructuras que caen en la Zona de Dolor son las entidades, modelos y enumeraciones (A=0, I=0), lo cual es 100% deseable y tolerable dada su nula volatilidad.

---

## 4. Resumen Comparativo de Métricas

Si se requieren las cifras para tablas de apoyo, estos son los valores de referencia según el criterio unificado de medición:

| Métrica | Legado | Refactorizado |
| :--- | :--- | :--- |
| **A (Abstracción global de tipos)** | 0 / 6 = 0,00 | 15 / 48 = 0,31 |
| **Ca / Ce (de la clase principal)** | 1 / 5 (`GestorCitasOdontologicas`) | 1 / 10 (`ServicioAgendamiento`) |
| **I (Inestabilidad de la clase principal)** | 0,833 | 0,909 |
| **D (Distancia de la clase principal)** | 0,167 | 0,091 |
| **Zona de Dolor (Clase Principal)** | No | No (solo entidades, modelos y enumeraciones) |
