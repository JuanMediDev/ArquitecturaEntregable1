using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Excepciones;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: valida disponibilidad basándose en la bandera del odontólogo.
/// </summary>
public sealed class ValidadorDisponibilidadPorBandera : IValidadorDisponibilidad
{
    public void Validar(Odontologo odontologo, DateTime fechaHora)
    {
        if (!odontologo.EstaDisponible)
            throw new OdontologoNoDisponibleException();
    }
}
