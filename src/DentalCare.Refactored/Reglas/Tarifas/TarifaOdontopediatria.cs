using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public sealed class TarifaOdontopediatria : IReglaTarifa
{
    private const decimal Factor = 1.1m;
    public int Orden => 10;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Especialidad == Especialidad.Odontopediatria;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * Factor;
}
