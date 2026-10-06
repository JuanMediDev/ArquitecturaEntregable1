using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Construye textos y objetos de notificación para citas.
/// </summary>
public interface IConstructorMensajes
{
    MensajeNotificacion Confirmacion(Cita cita);
    MensajeNotificacion Cancelacion(Cita cita);
}
