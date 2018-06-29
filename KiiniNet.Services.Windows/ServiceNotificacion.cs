using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using KiiniNet.Entities.Parametros;
using KinniNet.Core.Demonio;
using KinniNet.Core.Parametros;
using Timer = System.Timers.Timer;

namespace KiiniNet.Services.Windows
{
    public partial class ServiceNotificacion : ServiceBase
    {
        private Timer _intervaloEjecucion = null;
        public ServiceNotificacion()
        {
            InitializeComponent();
            
        }

        public void OnDebugg()
        {
            OnStart(null);
        }

        void intervaloEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {

                Thread.Sleep(1000);
                LogCorrect("KiiniNet", "Service Send Notication", "Iniciando Tickets Resuletos Sin Cerrar");
                new BusinessDemonio().CierraTicketsResueltos();
                LogCorrect("KiiniNet", "Service Send Notication", "Tickets Resuletos Sin Cerrar");

                LogCorrect("KiiniNet", "Service Send Notication", "Iniciando notificaciones");
                new BusinessDemonio().EnvioNotificacion();
                LogCorrect("KiiniNet", "Service Send Notication", "Terminado notificaciones");
                Thread.Sleep(1000);
                LogCorrect("KiiniNet", "Service Send Notication", "Iniciando Correos");
                new BusinessTicketMailService().RecibeCorreos();
                LogCorrect("KiiniNet", "Service Send Notication", "Terminado Correos");

            }
            catch (Exception ex)
            {
                LogError("KiiniNet", "Service Send Notication", ex.Message);
                
            }
        }
        void LogCorrect(string source, string application, string mensaje)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, application);

                EventLog.WriteEntry(source, mensaje);
                EventLog.WriteEntry(source, mensaje,
                    EventLogEntryType.SuccessAudit, 234);
            }
            catch (Exception ex)
            {
                LogError("KiiniNet", "Service Send Notication", ex.Message);
            }
        }

        void LogError(string source, string application, string mensaje)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, application);

                EventLog.WriteEntry(source, mensaje);
                EventLog.WriteEntry(source, mensaje,
                    EventLogEntryType.Error, 234);
            }
            catch (Exception ex)
            {
                LogError("KiiniNet", "Service Send Notication", ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
                BusinessParametros bparams = new BusinessParametros();
                ParametrosGenerales parametros =  bparams.ObtenerParametrosGenerales();
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
                LogCorrect("KiiniNet", "Service Send Notication", "Iniciando Servicio");
                _intervaloEjecucion.Start();
                LogCorrect("KiiniNet", "Service Send Notication", "Servicio Iniciado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogError("KiiniNet", "Service Send Notication", ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                _intervaloEjecucion.Stop();
                File.Create(Environment.CurrentDirectory + "OnStop.txt");
            }
            catch (Exception ex)
            {
                LogError("KiiniNet", "Service Send Notication", ex.Message);
            }
        }
    }
}
