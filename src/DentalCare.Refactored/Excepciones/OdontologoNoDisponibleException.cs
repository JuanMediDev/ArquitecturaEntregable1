using System;

namespace DentaCare.Refactored.Excepciones;

/// <summary>
/// Excepción lanzada cuando un odontólogo no está disponible.
/// </summary>
public sealed class OdontologoNoDisponibleException : Exception
{
    public OdontologoNoDisponibleException()
        : base("El odontólogo no tiene disponibilidad en el horario seleccionado.")
    { }
}
