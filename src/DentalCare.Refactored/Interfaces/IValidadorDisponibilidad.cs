using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Interfaces;

public interface IValidadorDisponibilidad { void Validar(Odontologo odontologo, DateTime fechaHora); }
