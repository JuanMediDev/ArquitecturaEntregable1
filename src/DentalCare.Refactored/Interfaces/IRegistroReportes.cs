namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Registra métricas de negocio en memoria.
/// </summary>
public interface IRegistroReportes { void RegistrarRecaudo(decimal monto); void RegistrarCancelacion(); }
