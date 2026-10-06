using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: orquesta el caso de uso de agendar una cita.
/// </summary>
public sealed class ServicioAgendamiento : IServicioAgendamiento
{
    private readonly ICalculadorCopago _calculadorCopago;
    private readonly IGeneradorIdentificador _generador;
    private readonly IValidadorDisponibilidad _validador;
    private readonly IRegistroReportes _registro;
    private readonly IConstructorMensajes _constructor;
    private readonly INotificador _notificador;
    private readonly ICitaRepositorio _repositorio;

    public ServicioAgendamiento(
        ICalculadorCopago calculadorCopago,
        IGeneradorIdentificador generador,
        IValidadorDisponibilidad validador,
        IRegistroReportes registro,
        IConstructorMensajes constructor,
        INotificador notificador,
        ICitaRepositorio repositorio)
    {
        _calculadorCopago = calculadorCopago;
        _generador = generador;
        _validador = validador;
        _registro = registro;
        _constructor = constructor;
        _notificador = notificador;
        _repositorio = repositorio;
    }

    public Cita Agendar(SolicitudAgendamiento solicitud)
    {
        _validador.Validar(solicitud.Odontologo, solicitud.FechaHora);

        var copago = _calculadorCopago.Calcular(solicitud);
        var id = _generador.Generar();
        var cita = new Cita(id, solicitud.Paciente, solicitud.Odontologo, solicitud.FechaHora, copago);

        _registro?.RegistrarRecaudo(copago);

        _repositorio?.Guardar(cita);

        var mensaje = _constructor.Confirmacion(cita);
        _notificador.Notificar(mensaje);

        return cita;
    }
}
