using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Regla de tarifa aplicada al cálculo del copago.
/// </summary>
public interface IReglaTarifa
{
    int Orden { get; }
    bool Aplica(SolicitudAgendamiento solicitud);
    decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud);
}
