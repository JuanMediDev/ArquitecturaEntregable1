using System;

namespace DentaCare.Refactored.Excepciones;

public sealed class OdontologoNoDisponibleException : Exception
{
    public OdontologoNoDisponibleException()
        : base("El odontÃ³logo no tiene disponibilidad en el horario seleccionado.")
    { }
}
