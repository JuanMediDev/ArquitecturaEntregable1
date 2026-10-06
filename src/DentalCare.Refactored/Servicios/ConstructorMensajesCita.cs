using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: genera textos y objetos de notificación para una cita.
/// </summary>
public sealed class ConstructorMensajesCita : IConstructorMensajes
{
    public MensajeNotificacion Confirmacion(Cita cita)
    {
        var asunto = $"Confirmación cita {cita.Id}";
        var cuerpo = $"Cita programada para {cita.FechaHora:yyyy-MM-dd HH:mm} con el Dr. {cita.Odontologo.Nombre}. Copago: {cita.CopagoCalculado:C2}";
        return new MensajeNotificacion(cita.Paciente.Correo, cita.Paciente.Celular, asunto, cuerpo);
    }

    public MensajeNotificacion Cancelacion(Cita cita)
    {
        var asunto = $"Cancelación cita {cita.Id}";
        var cuerpo = $"Su cita ha sido cancelada. Penalización: {cita.PenalizacionCancelacion:C2}";
        return new MensajeNotificacion(cita.Paciente.Correo, cita.Paciente.Celular, asunto, cuerpo);
    }
}
