using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Servicios;

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
        _calculadorCopago = calculadorCopago ?? throw new ArgumentNullException(nameof(calculadorCopago));
        _generador = generador ?? throw new ArgumentNullException(nameof(generador));
        _validador = validador ?? throw new ArgumentNullException(nameof(validador));
        _registro = registro ?? throw new ArgumentNullException(nameof(registro));
        _constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
        _notificador = notificador ?? throw new ArgumentNullException(nameof(notificador));
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
    }

    public Cita Agendar(SolicitudAgendamiento solicitud)
    {
        _validador.Validar(solicitud.Odontologo, solicitud.FechaHora);

        var copago = _calculadorCopago.Calcular(solicitud);
        var id = _generador.Generar();
        var cita = new Cita(id, solicitud.Paciente, solicitud.Odontologo, solicitud.FechaHora, copago);

        _registro.RegistrarRecaudo(copago);

        _repositorio.Guardar(cita);

        var mensaje = _constructor.Confirmacion(cita);
        _notificador.Notificar(mensaje);

        return cita;
    }
}
