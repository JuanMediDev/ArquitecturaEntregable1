using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Calcula el copago aplicando reglas de tarifa.
/// </summary>
public interface ICalculadorCopago { decimal Calcular(SolicitudAgendamiento solicitud); }
