using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using KiiniNet.Entities.Parametros;
using KinniNet.Core.Demonio;
using KinniNet.Core.Parametros;
using Timer = System.Timers.Timer;

namespace DemonioCierreTicket
{
    public partial class ServiceCierreTicket : ServiceBase
    {
        private readonly string _serviceLog;
        private Timer _intervaloEjecucion;
        StreamWriter _log;
        public ServiceCierreTicket(string serviceName)
        {
            InitializeComponent();
            _serviceLog = serviceName;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                BusinessParametros bparams = new BusinessParametros();
                ParametrosGenerales parametros = bparams.ObtenerParametrosGenerales();
                double intervalo = 60000;
                if (parametros != null)
                {
                    intervalo = parametros.FrecuenciaDemonioSegundos * 1000;
                    _intervaloEjecucion = new Timer(intervalo);
                }
                else
                {
                    _intervaloEjecucion = new Timer(intervalo);
                }

                _intervaloEjecucion.Elapsed += intervaloEjecucion_Elapsed;
                _intervaloEjecucion.Start();
                LogCorrect("Servicio en ejecución");
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }
        void intervaloEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                LogCorrect("Iniciando Tickets Resuletos Sin Cerrar");
                _intervaloEjecucion.Stop();
                new BusinessDemonio().CierraTicketsResueltos();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

            }
            finally
            {
                _intervaloEjecucion.Start();
            }
        }
        protected override void OnStop()
        {
            try
            {
                _intervaloEjecucion.Stop();
                LogCorrect("Termino Servicio Cierre de tickets");
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        void LogCorrect(string mensaje)
        {
            try
            {
                _log = !File.Exists(string.Format("Log_{0}.txt", _serviceLog)) ? new StreamWriter(string.Format("log{0}.txt", _serviceLog)) : File.AppendText(string.Format("log{0}.txt", _serviceLog));
                _log.WriteLine("Ejecucion (correcta): {0} - {1} ", DateTime.Now, mensaje);
                _log.Close();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        void LogError(string mensaje)
        {
            try
            {
                _log = !File.Exists(string.Format("Log_{0}.txt", _serviceLog)) ? new StreamWriter(string.Format("log{0}.txt", _serviceLog)) : File.AppendText(string.Format("log{0}.txt", _serviceLog));
                _log.WriteLine("Ejecucion (error): {0} - {1} ", DateTime.Now, mensaje);
                _log.Close();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }
    }
}
