using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using KiiniNet.Entities.Parametros;
using KinniNet.Core.Demonio;
using KinniNet.Core.Parametros;
using Timer = System.Timers.Timer;

namespace DemonioTicketsCorreo
{
    public partial class ServiceTicketsCorreo : ServiceBase
    {
        private readonly string _serviceLog;
        private Timer _intervaloEjecucion;
        public ServiceTicketsCorreo(string serviceName)
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
                LogCorrect( "Timer Start");
            }
            catch (Exception ex)
            {
                LogError( ex.Message);
            }
        }
        void intervaloEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                LogCorrect("Iniciando Recepcion de correos");
                _intervaloEjecucion.Stop();
                LogCorrect(new BusinessTicketMailService().RecibeCorreos());
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
            }
            catch (Exception ex)
            {
                LogError( ex.Message);
            }
        }
        void VerificaDirectorio()
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        void LogCorrect(string mensaje)
        {
            try
            {
                VerificaDirectorio();
                string pathFile = string.Format("{0}\\Log_{1}.txt", AppDomain.CurrentDomain.BaseDirectory + "\\Logs", _serviceLog);
                if (File.Exists(pathFile))
                {
                    DateTime logCreatedDate = File.GetCreationTime(pathFile);
                    if (logCreatedDate < DateTime.Now.AddDays(-30))
                    {
                        File.Move(pathFile, string.Format("{0}\\Log_{1}_Respaldo{2}-{3}.txt", System.Reflection.Assembly.GetExecutingAssembly().Location, _serviceLog, DateTime.Now.AddDays(-30).ToShortDateString(), DateTime.Now.ToShortDateString()));
                    }
                }

                FileStream objFilestream = new FileStream(pathFile, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine("{0} - {1} ", DateTime.Now, mensaje);
                objStreamWriter.Close();
                objFilestream.Close();
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
                VerificaDirectorio();
                string pathFile = string.Format("{0}\\Log_{1}.txt", AppDomain.CurrentDomain.BaseDirectory + "\\Logs", _serviceLog);
                if (File.Exists(pathFile))
                {
                    DateTime logCreatedDate = File.GetCreationTime(pathFile);
                    if (logCreatedDate < DateTime.Now.AddDays(-30))
                    {
                        File.Move(pathFile, string.Format("{0}\\Log_{1}_Respaldo{2}-{3}.txt", System.Reflection.Assembly.GetExecutingAssembly().Location, _serviceLog, DateTime.Now.AddDays(-30).ToShortDateString(), DateTime.Now.ToShortDateString()));
                    }
                }

                FileStream objFilestream = new FileStream(pathFile, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine("Error: {0} - {1} ", DateTime.Now, mensaje);
                objStreamWriter.Close();
                objFilestream.Close();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }
    }
}
