using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

public interface IReglaTarifa
{
    int Orden { get; }
    bool Aplica(SolicitudAgendamiento solicitud);
    decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud);
}
