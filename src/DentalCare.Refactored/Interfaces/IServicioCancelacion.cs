using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Servicio de orquestación para cancelar citas.
/// </summary>
public interface IServicioCancelacion { decimal Cancelar(Cita cita, DateTime fechaHoraCancelacion); }
