using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Calculador de penalización compuesto por reglas.
/// </summary>
public interface ICalculadorPenalizacion { decimal Calcular(ContextoCancelacion contexto); }
