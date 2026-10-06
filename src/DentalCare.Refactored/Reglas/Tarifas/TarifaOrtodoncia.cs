using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public class TarifaOrtodoncia : IReglaTarifa
{
    private const decimal Factor = 1.2m;
    public int Orden => 10;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Especialidad == Especialidad.Ortodoncia;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * Factor;
}
