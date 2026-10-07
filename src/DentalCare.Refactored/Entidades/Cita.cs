using System;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Entidades;

public sealed class Cita
{
    public Cita(string id, Paciente paciente, Odontologo odontologo, DateTime fechaHora, decimal copagoCalculado)
    {
        Id = id;
        Paciente = paciente;
        Odontologo = odontologo;
        FechaHora = fechaHora;
        CopagoCalculado = copagoCalculado;
        Estado = EstadoCita.Programada;
        PenalizacionCancelacion = 0m;
    }

    public string Id { get; }
    public Paciente Paciente { get; }
    public Odontologo Odontologo { get; }
    public DateTime FechaHora { get; }
    public decimal CopagoCalculado { get; }
    public EstadoCita Estado { get; private set; }
    public decimal PenalizacionCancelacion { get; private set; }

    public void Cancelar(decimal penalizacion)
    {
        Estado = EstadoCita.Cancelada;
        PenalizacionCancelacion = penalizacion;
    }
}
