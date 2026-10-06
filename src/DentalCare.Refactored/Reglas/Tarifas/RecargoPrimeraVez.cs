using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Reglas.Tarifas;

public sealed class RecargoPrimeraVez : IReglaTarifa
{
    private const decimal Recargo = 20.0m;
    public int Orden => 30;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Paciente.EsPrimeraVez;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual + Recargo;
}
