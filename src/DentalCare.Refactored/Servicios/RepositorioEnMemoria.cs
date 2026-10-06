using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: conserva citas y cancelaciones en memoria sin depender de infraestructura externa.
/// </summary>
public sealed class RepositorioEnMemoria : ICitaRepositorio, ICancelacionRepositorio
{
    private readonly List<Cita> _citas = new();
    private readonly List<(string CitaId, decimal Penalizacion)> _cancelaciones = new();

    public void Guardar(Cita cita)
    {
        _citas.Add(cita);
    }

    public void RegistrarCancelacion(string citaId, decimal penalizacion)
    {
        _cancelaciones.Add((citaId, penalizacion));
    }

    public IReadOnlyList<Cita> Citas => _citas;

    public IReadOnlyList<(string CitaId, decimal Penalizacion)> Cancelaciones => _cancelaciones;
}
