using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public class TarifaCirugia : IReglaTarifa
{
    private const decimal Factor = 2.5m;
    public int Orden => 10;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Especialidad == Especialidad.Cirugia;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * Factor;
}
