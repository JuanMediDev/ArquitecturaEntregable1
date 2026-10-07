using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Interfaces;

public interface IServicioCancelacion { decimal Cancelar(Cita cita, DateTime fechaHoraCancelacion); }
