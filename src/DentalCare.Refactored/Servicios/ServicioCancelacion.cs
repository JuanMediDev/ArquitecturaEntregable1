using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: orquesta el caso de uso de cancelar una cita.
/// </summary>
public sealed class ServicioCancelacion : IServicioCancelacion
{
    private readonly ICalculadorPenalizacion _calculadorPenalizacion;
    private readonly IRegistroReportes _registro;
    private readonly IConstructorMensajes _constructor;
    private readonly INotificador _notificador;
    private readonly ICancelacionRepositorio _repositorio;

    public ServicioCancelacion(
        ICalculadorPenalizacion calculadorPenalizacion,
        IRegistroReportes registro,
        IConstructorMensajes constructor,
        INotificador notificador,
        ICancelacionRepositorio repositorio)
    {
        _calculadorPenalizacion = calculadorPenalizacion;
        _registro = registro;
        _constructor = constructor;
        _notificador = notificador;
        _repositorio = repositorio;
    }

    public decimal Cancelar(Cita cita, DateTime fechaHoraCancelacion)
    {
        var contexto = new ContextoCancelacion(cita, fechaHoraCancelacion);
        var penalizacion = _calculadorPenalizacion.Calcular(contexto);
        cita.Cancelar(penalizacion);

        _registro?.RegistrarCancelacion();

        _repositorio?.RegistrarCancelacion(cita.Id, penalizacion);

        var mensaje = _constructor.Cancelacion(cita);
        _notificador.Notificar(mensaje);

        return penalizacion;
    }
}
