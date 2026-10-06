using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: conserva notificaciones en memoria sin depender de canales externos.
/// </summary>
public sealed class NotificadorEnMemoria : INotificador
{
    private readonly List<MensajeNotificacion> _mensajes = new();

    public void Notificar(MensajeNotificacion mensaje)
    {
        _mensajes.Add(mensaje);
    }

    public IReadOnlyList<MensajeNotificacion> Mensajes => _mensajes;
}
