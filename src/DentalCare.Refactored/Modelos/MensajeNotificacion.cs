namespace DentaCare.Refactored.Modelos;

/// <summary>
/// Representa el mensaje que se envía por los canales.
/// </summary>
public record MensajeNotificacion(string Correo, string Celular, string Asunto, string Cuerpo);
