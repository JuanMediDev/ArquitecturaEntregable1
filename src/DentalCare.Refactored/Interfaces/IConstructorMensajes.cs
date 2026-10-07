using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

public interface IConstructorMensajes
{
    MensajeNotificacion Confirmacion(Cita cita);
    MensajeNotificacion Cancelacion(Cita cita);
}
