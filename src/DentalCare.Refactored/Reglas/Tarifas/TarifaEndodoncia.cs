using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public class TarifaEndodoncia : IReglaTarifa
{
    private const decimal Factor = 1.8m;
    public int Orden => 10;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Especialidad == Especialidad.Endodoncia;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * Factor;
}
