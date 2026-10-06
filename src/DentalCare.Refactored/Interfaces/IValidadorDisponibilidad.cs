using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Valida si un odontólogo está disponible en una fecha/hora.
/// </summary>
public interface IValidadorDisponibilidad { void Validar(Odontologo odontologo, DateTime fechaHora); }
