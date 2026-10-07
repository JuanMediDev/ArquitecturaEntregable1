using System;
using System.Collections.Generic;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;
using DentaCare.Refactored.Reglas.Tarifas;
using DentaCare.Refactored.Reglas.Penalizaciones;
using DentaCare.Refactored.Servicios;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

var reglasTarifa = new IReglaTarifa[] {
    new TarifaOrtodoncia(), new TarifaEndodoncia(), new TarifaCirugia(), new TarifaOdontopediatria(),
    new DescuentoEps(), new DescuentoPrepagada(), new RecargoPrimeraVez(), new RecargoRadiografia()
};

var reglasPenalizacion = new IReglaPenalizacion[] {
    new PenalizacionCancelacionTardia(), new RecargoCancelacionTardiaCirugia()
};

var calculadorCopago = new CalculadorCopago(reglasTarifa);
var calculadorPenalizacion = new CalculadorPenalizacion(reglasPenalizacion);
var validador = new ValidadorDisponibilidadPorBandera();
var generador = new GuidGeneradorIdentificador();
var reporte = new ReporteCitasEnMemoria();
var constructor = new ConstructorMensajesCita();
var canalesNotificacion = new ICanalNotificacion[] {
    new CanalCorreoEnMemoria(),
    new CanalSmsEnMemoria()
};
var notificador = new NotificadorCompuesto(canalesNotificacion);
var repositorio = new RepositorioEnMemoria();

var servicioAgendamiento = new ServicioAgendamiento(calculadorCopago, generador, validador, reporte, constructor, notificador, repositorio);
var servicioCancelacion = new ServicioCancelacion(calculadorPenalizacion, reporte, constructor, notificador, repositorio);

var paciente1 = new Paciente { Id = "p1", NombreCompleto = "Ana Pérez", Correo = "ana@correo.com", Celular = "3000000000", Convenio = Convenio.Eps, EsPrimeraVez = true };
var odontologo1 = new Odontologo { Id = "d1", Nombre = "Dr. Gómez", Especialidad = Especialidad.Cirugia, EstaDisponible = true };
var solicitud1 = new SolicitudAgendamiento(paciente1, odontologo1, DateTime.Now.AddHours(12), Especialidad.Cirugia, RequiereRadiografia: true);

var cita1 = servicioAgendamiento.Agendar(solicitud1);
Console.WriteLine($"Cita agendada (EPS): {cita1.Id}, Copago: {cita1.CopagoCalculado:C2}");

var penalizacion = servicioCancelacion.Cancelar(cita1, DateTime.Now);
Console.WriteLine($"Cita cancelada: {cita1.Id}, Penalización aplicada: {penalizacion:C2}");

var paciente2 = new Paciente { Id = "p2", NombreCompleto = "Juan Lopez", Correo = "juan@correo.com", Celular = "3000000001", Convenio = Convenio.Prepagada, EsPrimeraVez = false };
var odontologo2 = new Odontologo { Id = "d2", Nombre = "Dra. Suarez", Especialidad = Especialidad.Cirugia, EstaDisponible = true };
var solicitud2 = new SolicitudAgendamiento(paciente2, odontologo2, DateTime.Now.AddHours(48), Especialidad.Cirugia, RequiereRadiografia: false);

var cita2 = servicioAgendamiento.Agendar(solicitud2);
Console.WriteLine($"Cita agendada (Prepagada): {cita2.Id}, Copago: {cita2.CopagoCalculado:C2}");

Console.WriteLine($"Total recaudado (memoria): {reporte.ObtenerTotalRecaudado():C2}");
Console.WriteLine($"Total canceladas (memoria): {reporte.ObtenerTotalCanceladas()}");
