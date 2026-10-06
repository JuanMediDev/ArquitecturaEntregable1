using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Reglas.Tarifas;

public class RecargoRadiografia : IReglaTarifa
{
    private const decimal Recargo = 35.0m;
    public int Orden => 40;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.RequiereRadiografia;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual + Recargo;
}
