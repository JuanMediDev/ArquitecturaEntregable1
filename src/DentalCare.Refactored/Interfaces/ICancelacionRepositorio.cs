namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Registra cancelaciones en persistencia.
/// </summary>
public interface ICancelacionRepositorio { void RegistrarCancelacion(string citaId, decimal penalizacion); }
