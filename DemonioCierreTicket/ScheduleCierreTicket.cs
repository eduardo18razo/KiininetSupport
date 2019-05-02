using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KinniNet.Core.Demonio;

namespace DemonioCierreTicket
{
    public class ScheduleCierreTicket
    {
        private readonly string _serviceLog;
        public ScheduleCierreTicket(string serviceName)
        {
            _serviceLog = serviceName;
        }
        public void ExecutaProceso()
        {
            try
            {
                LogCorrect("----------------------------------Iniciando Cerrando Ticket Resuletos----------------------------------");
                LogCorrect(new BusinessDemonio().CierraTicketsResueltos());
                LogCorrect("----------------------------------Terminado Cerrando Ticket Resuletos----------------------------------");
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
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
