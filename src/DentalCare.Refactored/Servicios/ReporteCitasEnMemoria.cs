using DentaCare.Refactored.Interfaces;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: almacena contadores sencillos en memoria para reportes.
/// </summary>
public sealed class ReporteCitasEnMemoria : IRegistroReportes, IConsultaReportes
{
    private decimal _totalRecaudado;
    private int _totalCanceladas;

    public void RegistrarRecaudo(decimal monto) => _totalRecaudado += monto;

    public void RegistrarCancelacion() => _totalCanceladas++;

    public decimal ObtenerTotalRecaudado() => _totalRecaudado;

    public int ObtenerTotalCanceladas() => _totalCanceladas;
}
