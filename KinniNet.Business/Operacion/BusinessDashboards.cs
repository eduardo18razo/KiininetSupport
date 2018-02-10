using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using KiiniNet.Entities.Operacion.Dashboard;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Parametros;
using KinniNet.Data.Help;
using System.Data.SqlClient;
using KiiniNet.Entities.Cat.Sistema;

namespace KinniNet.Core.Operacion
{
    public class BusinessDashboards : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessDashboards(bool proxy = false)
        {
            _proxy = proxy;
        }

        public DashboardAdministrador GetDashboardAdministrador()
        {
            DashboardAdministrador results;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string connection = (((System.Data.EntityClient.EntityConnection)(db.Connection)).StoreConnection).ConnectionString;

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> userFilters = new List<int>
                {
                    (int) BusinessVariables.EnumTiposUsuario.Empleado,
                    (int) BusinessVariables.EnumTiposUsuario.Cliente,
                    (int) BusinessVariables.EnumTiposUsuario.Proveedor
                };

                results = new DashboardAdministrador();

                results.UsuariosRegistrados = db.Usuario.Count(c => userFilters.Contains(c.IdTipoUsuario));
                results.UsuariosActivos = db.Usuario.Count(c => userFilters.Contains(c.IdTipoUsuario) && c.Habilitado && c.Activo);
                results.TicketsCreados = db.Ticket.Count();
                results.Operadores = db.Usuario.Count(c => c.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Operador && c.Habilitado && c.Activo);

                DataTable dtTickets = new DataTable("dt");
                dtTickets.Columns.Add(new DataColumn("Id", typeof(int)));
                dtTickets.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                dtTickets.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (GraficoConteo datos in db.Ticket.Join(db.Canal, ticket => ticket.IdCanal, canal => canal.Id, (ticket, canals) => new { ticket, canals }).GroupBy(g => g.canals).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }).ToList())
                {
                    dtTickets.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                results.GraficoTicketsCreadosCanal = dtTickets;


                DataTable dtUsuarios = new DataTable("dt");
                dtUsuarios.Columns.Add(new DataColumn("Id", typeof(int)));
                dtUsuarios.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                dtUsuarios.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (GraficoConteo datos in db.Usuario.Join(db.TipoUsuario, users => users.IdTipoUsuario, tipoUsuario => tipoUsuario.Id, (users, tipoUsuario) => new { users, tipoUsuario }).Where(w => userFilters.Contains(w.users.IdTipoUsuario)).GroupBy(g => g.tipoUsuario).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }).ToList())
                {
                    dtUsuarios.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                results.GraficoUsuariosRegistrados = dtUsuarios;

                DataTable dtAlmacenamiento = new DataTable("dt");
                dtAlmacenamiento.Columns.Add(new DataColumn("Ocupado", typeof(double)));
                dtAlmacenamiento.Columns.Add(new DataColumn("Libre", typeof(double)));


                long maxSize = 1024;
                string queryString = "SELECT DataBaseName = DB_NAME(database_id), \n" +
                                     "LogSize = CAST(SUM(CASE WHEN type_desc = 'LOG' THEN size END) * 8 / 1024 AS DECIMAL(8,2)), \n" +
                                     "RowSize_mb = CAST(SUM(CASE WHEN type_desc = 'ROWS' THEN size END) * 8 / 1024 AS DECIMAL(8,2)), \n" +
                                     "TotalSize_mb = CAST(SUM(size) * 8 / 1024 AS DECIMAL(8,2)) \n" +
                                     "FROM sys.master_files \n" +
                                     "WHERE database_id = DB_ID() GROUP BY database_id";
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Almacenamiento");

                DirectoryInfo dInfo = new DirectoryInfo(BusinessVariables.Directorios.RepositorioRepositorio);
                long sizeOfDirBytes = BusinessFile.DirectorySize(dInfo, true);
                double sizeOfDirKb = ((double)sizeOfDirBytes) / 1024;
                double sizeOfDirMb = sizeOfDirKb / 1024;
                double sizeOfDirGb = sizeOfDirMb / 1024;
                double totalSize = Math.Round(double.Parse((ds.Tables[0].Rows[0][3].ToString())) + sizeOfDirMb, 2);
                //if (totalSize > 1024)
                //    totalSize = totalSize / 1024;
                dtAlmacenamiento.Rows.Add(Math.Round(totalSize, 0, MidpointRounding.ToEven), Math.Round(maxSize - totalSize, 0, MidpointRounding.ToEven));
                results.TotalArchivos = Directory.GetFiles(dInfo.FullName, "*.*", SearchOption.AllDirectories).Length;
                results.GraficoAlmacenamiento = dtAlmacenamiento;



                results.Categorias = db.Area.Count();
                results.Articulos = db.InformacionConsulta.Count();
                results.Formularios = db.Mascara.Count();
                results.Catalogos = db.Catalogos.Count(c => !c.Sistema);
                results.OperadorRol = db.Rol.Where(w => w.Id != (int)BusinessVariables.EnumRoles.Usuario)
                    .Join(db.RolTipoUsuario, rol => rol.Id, roltipousuario => roltipousuario.IdRol, (rol, roltipousuario) => new { r = rol, rtu = roltipousuario })
                    .Join(db.UsuarioRol, roltu => roltu.rtu.Id, usuariorol => usuariorol.IdRolTipoUsuario, (roltipousuario, usuariorol) => new { rtu = roltipousuario, ur = usuariorol })
                    .GroupJoin(db.Usuario.Where(w => w.IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Operador), ur => ur.ur.IdUsuario, users => users.Id, (usuariorol, usuarios) => new { ur = usuariorol, u = usuarios })
                    .Select(s => new { s.ur.rtu.r })
                    .GroupBy(g => g.r)
                    .Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }).ToList();

                results.Organizacion = db.Organizacion.Count();
                results.Ubicacion = db.Ubicacion.Count();
                results.Puestos = db.Puesto.Count();
                results.Grupos = db.GrupoUsuario.Count();
                results.Horarios = db.Horario.Count();
                results.Feriados = db.DiasFeriados.Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return results;
        }


        //TODO: Cambiar por parametro
        private int ObtenerDiasPeriodo()
        {
            int result = 7;
            ParametrosGenerales parameters = new BusinessParametros().ObtenerParametrosGenerales();
            if (parameters != null)
                result = parameters.PeriodoDashboard;
            return result;
        }

        public DashboardAgente GetDashboardAgente(int? idGrupo, int? idUsuario)
        {
            DashboardAgente result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                result = new DashboardAgente();

                List<int> lstEstatusCreado = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera
                };
                List<int> lstEstatusAbierto = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera
                };
                List<int> lstEstatusResuelto = new List<int>
                {
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto,
                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                };
                //Paramtreoo dias periodo
                int daysPeriod = ObtenerDiasPeriodo();

                //Fechas periodoActual 
                DateTime startCurrentDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString() + " 23:59:59");
                DateTime endCurrentDate = Convert.ToDateTime(startCurrentDate.AddDays(-daysPeriod).ToShortDateString());

                //Fechas Periodo anterior
                DateTime startPreviuosDate = Convert.ToDateTime(endCurrentDate.AddDays(-1).ToShortDateString() + " 23:59:59");
                DateTime endPreviousDate = Convert.ToDateTime(startPreviuosDate.AddDays(-daysPeriod).ToShortDateString());
                var qryCurrentTickets = from t in db.Ticket
                                        join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                                        where t.FechaHoraAlta <= startCurrentDate && t.FechaHoraAlta >= endCurrentDate
                                        select new { t, tgu };

                var qryPreviousTickets = from t in db.Ticket
                                         join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                                         where t.FechaHoraAlta <= startPreviuosDate && t.FechaHoraAlta >= endPreviousDate
                                         select new { t, tgu };


                if (idGrupo != null)
                {
                    qryCurrentTickets = from q in qryCurrentTickets
                                        where q.tgu.IdGrupoUsuario == idGrupo
                                        select q;
                    qryPreviousTickets = from q in qryPreviousTickets
                                         where q.tgu.IdGrupoUsuario == idGrupo
                                         select q;
                }
                if (idUsuario != null)
                {
                    List<int> idTicketsToco = db.TicketAsignacion.Where(w => w.IdUsuarioAsignado == idUsuario).Select(s => s.IdTicket).Distinct().ToList();
                    qryCurrentTickets = from q in qryCurrentTickets
                                        where idTicketsToco.Contains(q.t.Id)
                                        select q;
                    qryPreviousTickets = from q in qryPreviousTickets
                                         where idTicketsToco.Contains(q.t.Id)
                                         select q;
                }

                List<int> lstCreatedTicketsCurrent = qryCurrentTickets.Select(s => s.t.Id).Distinct().ToList();

                List<int> lstCreatedTicketsPrevious = qryPreviousTickets.Select(s => s.t.Id).Distinct().ToList();


                List<int> ticketResuletos = db.Ticket.Where(c => lstCreatedTicketsCurrent.Contains(c.Id) && lstEstatusResuelto.Contains(c.IdEstatusTicket)).Select(s => s.Id).Distinct().ToList();

                result.TotalTickets = db.Ticket.Count(c => c.FechaHoraAlta <= startCurrentDate && lstEstatusAbierto.Contains(c.IdEstatusTicket));

                result.TicketsAbiertos = db.Ticket.Count(c => lstCreatedTicketsCurrent.Contains(c.Id) && lstEstatusAbierto.Contains(c.IdEstatusTicket));
                result.TicketsCreados = lstCreatedTicketsCurrent.Count;
                result.TicketsResuletos = ticketResuletos.Count();
                result.TicketsResuletosVsCreados = result.TicketsCreados > 0 ? Math.Truncate(double.Parse((decimal.Parse(result.TicketsResuletos.ToString()) / decimal.Parse(result.TicketsCreados.ToString())).ToString()) * 100) : 0;

                result.TotalTicketsReabiertos = db.TicketAsignacion.Where(w => ticketResuletos.Contains(w.IdTicket) && w.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto).Distinct().Count();
                result.TicketsResuletosVsReabiertos = result.TotalTicketsReabiertos > 0 ? Math.Truncate(double.Parse((decimal.Parse(result.TicketsResuletos.ToString()) / decimal.Parse(result.TotalTicketsReabiertos.ToString())).ToString()) * 100) : 0;

                #region Primera Respuesta

                //Primera Respuesta Periodo Actual
                List<double> currentDiffResponse = new List<double>();
                List<int> currentTicketsResponse = db.TicketConversacion.Where(w => lstCreatedTicketsCurrent.Contains(w.IdTicket) && w.IdUsuario != w.Ticket.IdUsuarioLevanto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in currentTicketsResponse)
                {
                    TicketConversacion firstCommentResponse = db.TicketConversacion.Where(w => w.IdTicket == idticket && w.IdUsuario != w.Ticket.IdUsuarioLevanto).OrderBy(o => o.FechaGeneracion).FirstOrDefault();
                    if (firstCommentResponse != null)
                    {
                        Ticket currentTicketResponse = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (currentTicketResponse != null)
                            currentDiffResponse.Add((firstCommentResponse.FechaGeneracion - currentTicketResponse.FechaHoraAlta).TotalSeconds);
                    }
                }
                double currentAvgSecondsResponse = currentDiffResponse.Count > 0 ? currentDiffResponse.Average(a => a) : 0;
                TimeSpan currentTimeResponse = currentDiffResponse.Count > 0 ? TimeSpan.FromSeconds(currentAvgSecondsResponse) : TimeSpan.FromSeconds(0);

                result.PromedioPrimeraRespuestaSegundosActual = currentAvgSecondsResponse;
                result.PromedioPrimeraRespuestaActual = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(currentTimeResponse.TotalHours.ToString("0.##"))).ToString()), Math.Abs(currentTimeResponse.Minutes).ToString().PadLeft(2, '0'));

                //Primera Respuesta PeriodoAnterior
                List<double> previousDiffResponse = new List<double>();
                List<int> previousTicketsResponse = db.TicketConversacion.Where(w => lstCreatedTicketsPrevious.Contains(w.IdTicket) && w.IdUsuario != w.Ticket.IdUsuarioLevanto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in previousTicketsResponse)
                {
                    TicketConversacion firstCommentResponse = db.TicketConversacion.Where(w => w.IdTicket == idticket && w.IdUsuario != w.Ticket.IdUsuarioLevanto).OrderBy(o => o.FechaGeneracion).FirstOrDefault();
                    if (firstCommentResponse != null)
                    {
                        Ticket previousTicketResponse = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (previousTicketResponse != null)
                            previousDiffResponse.Add((firstCommentResponse.FechaGeneracion - previousTicketResponse.FechaHoraAlta).TotalSeconds);
                    }
                }
                double previousAvgSecondsResponse = previousDiffResponse.Count > 0 ? previousDiffResponse.Average(a => a) : 0;
                TimeSpan previousTimeResponse = previousDiffResponse.Count > 0 ? TimeSpan.FromSeconds(previousDiffResponse.Average(a => a)) : TimeSpan.FromSeconds(0);
                decimal previousAvgHourMinResponse = decimal.Parse(string.Format("{0}.{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeResponse.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeResponse.Minutes)));

                result.PromedioPrimeraRespuestaSegundosAnterior = previousAvgSecondsResponse;
                result.PromedioPrimeraRespuestaAnterior = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeResponse.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeResponse.Minutes).ToString().PadLeft(2, '0'));

                double diffPeriodsSecondsResponse = result.PromedioPrimeraRespuestaSegundosActual - result.PromedioPrimeraRespuestaSegundosAnterior;

                result.DiferenciaPromedioRespuestaPorcentaje = previousAvgHourMinResponse == 0 ? 0 : (diffPeriodsSecondsResponse / previousAvgSecondsResponse) * 100;

                #endregion Primera Respuesta

                #region Tiempo Resolucion

                //Tiempo Resolucion Periodo Actual
                List<double> currentDiffSolvedSeconds = new List<double>();
                List<int> currentTicketsSolved = db.TicketEstatus.Where(w => lstCreatedTicketsCurrent.Contains(w.IdTicket) && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in currentTicketsSolved)
                {
                    TicketEstatus firstSolved = db.TicketEstatus.Where(w => w.IdTicket == idticket && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).OrderBy(o => o.FechaMovimiento).FirstOrDefault();
                    if (firstSolved != null)
                    {
                        Ticket ticket = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (ticket != null)
                            currentDiffSolvedSeconds.Add((firstSolved.FechaMovimiento - ticket.FechaHoraAlta).TotalSeconds);
                    }
                }
                double currentAvgSecondsSolved = currentDiffSolvedSeconds.Count > 0 ? currentDiffSolvedSeconds.Average(a => a) : 0;
                TimeSpan currentTimeSolved = TimeSpan.FromSeconds(currentAvgSecondsSolved);

                result.PromedioTiempoResolucionSegundosActual = currentAvgSecondsSolved;
                result.PromedioTiempoResolucionActual = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(currentTimeSolved.TotalHours.ToString("0.##"))).ToString()), Math.Abs(currentTimeSolved.Minutes).ToString().PadLeft(2, '0'));


                //Tiempo Resolucion Periodo Anterior
                List<double> previousDiffSolvedSeconds = new List<double>();
                List<int> previousTicketsSolved = db.TicketEstatus.Where(w => lstCreatedTicketsPrevious.Contains(w.IdTicket) && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in previousTicketsSolved)
                {
                    TicketEstatus firstPreviousSolved = db.TicketEstatus.Where(w => w.IdTicket == idticket).OrderBy(o => o.FechaMovimiento).FirstOrDefault();
                    if (firstPreviousSolved != null)
                    {
                        Ticket ticketPreviousSolved = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (ticketPreviousSolved != null)
                            previousDiffSolvedSeconds.Add((firstPreviousSolved.FechaMovimiento - ticketPreviousSolved.FechaHoraAlta).TotalSeconds);
                    }
                }
                double previousAvgSecondsSolved = previousDiffSolvedSeconds.Count > 0 ? previousDiffSolvedSeconds.Average(a => a) : 0;
                TimeSpan previousTimeSolved = TimeSpan.FromSeconds(previousAvgSecondsSolved);
                decimal previousAvgHourMinSolved = decimal.Parse(string.Format("{0}.{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeSolved.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeSolved.Minutes)));

                result.PromedioTiempoResolucionSegundosAnterior = previousAvgSecondsSolved;
                result.PromedioTiempoResolucionAnterior = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeSolved.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeSolved.Minutes).ToString().PadLeft(2, '0'));

                double diffPeriodsSecondsSolved = result.PromedioTiempoResolucionSegundosActual - result.PromedioTiempoResolucionSegundosAnterior;

                result.PromedioTiempoResolucionPorcentaje = previousAvgHourMinSolved == 0 ? 0 : (diffPeriodsSecondsSolved / previousAvgSecondsSolved) * 100;

                #endregion Tiempo Resolucion

                #region Tiempo Resolucion Primer Contacto

                //Tiempo Resolucion Primer Contacto Actual
                List<double> currentDiffFirstContactSeconds = new List<double>();
                List<int> currentTicketsFirstContact = db.TicketEstatus.Where(w => lstCreatedTicketsCurrent.Contains(w.IdTicket) && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in currentTicketsFirstContact)
                {
                    TicketEstatus firstFirstContact = db.TicketEstatus.Where(w => w.IdTicket == idticket && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto)
                            .OrderBy(o => o.FechaMovimiento)
                            .FirstOrDefault();
                    if (firstFirstContact != null &&
                        !db.TicketEstatus.Any(a => a.IdTicket == idticket && a.Id > firstFirstContact.Id))
                    {
                        Ticket ticketFirstContact = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (ticketFirstContact != null)
                            currentDiffFirstContactSeconds.Add((firstFirstContact.FechaMovimiento - ticketFirstContact.FechaHoraAlta).TotalSeconds);
                    }
                }
                double currentAvgSecondsFirstContact = currentDiffFirstContactSeconds.Count > 0 ? currentDiffFirstContactSeconds.Average(a => a) : 0;
                TimeSpan currentTimeFirstContact = TimeSpan.FromSeconds(currentAvgSecondsFirstContact);

                result.PromedioResolucionPrimercontactoSegundosActual = currentAvgSecondsFirstContact;
                result.PromedioResolucionPrimercontactoActual = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(currentTimeFirstContact.TotalHours.ToString("0.##"))).ToString()), Math.Abs(currentTimeFirstContact.Minutes).ToString().PadLeft(2, '0'));

                //Tiempo Resolucion Primer Contacto Anterior
                List<double> previousDiffFirstContactSeconds = new List<double>();
                List<int> previousTicketsFirstContact = db.TicketEstatus.Where(w => lstCreatedTicketsPrevious.Contains(w.IdTicket) && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in previousTicketsFirstContact)
                {
                    TicketEstatus firstPreviousFirstContact = db.TicketEstatus.Where(w => w.IdTicket == idticket && w.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto).OrderBy(o => o.FechaMovimiento).FirstOrDefault();
                    if (firstPreviousFirstContact != null && !db.TicketEstatus.Any(a => a.IdTicket == idticket && a.Id > firstPreviousFirstContact.Id))
                    {
                        Ticket ticketPreviousFirstContact = db.Ticket.SingleOrDefault(s => s.Id == idticket);
                        if (ticketPreviousFirstContact != null)
                            previousDiffFirstContactSeconds.Add((firstPreviousFirstContact.FechaMovimiento - ticketPreviousFirstContact.FechaHoraAlta).TotalSeconds);
                    }
                }
                double previousAvgSecondsFirstContact = previousDiffFirstContactSeconds.Count > 0 ? previousDiffFirstContactSeconds.Average(a => a) : 0;
                TimeSpan previousTimeFirstContact = TimeSpan.FromSeconds(previousAvgSecondsFirstContact);
                decimal previousAvgHourMinFirstContact = decimal.Parse(string.Format("{0}.{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeFirstContact.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeFirstContact.Minutes)));

                result.PromedioResolucionPrimercontactoSegundosAnterior = previousAvgSecondsFirstContact;
                result.PromedioResolucionPrimercontactoAnterior = string.Format("{0}:{1}", int.Parse(Math.Truncate(decimal.Parse(previousTimeFirstContact.TotalHours.ToString("0.##"))).ToString()), Math.Abs(previousTimeFirstContact.Minutes).ToString().PadLeft(2, '0'));

                double diffPeriodsSecondsFirstContact = result.PromedioResolucionPrimercontactoSegundosActual - result.PromedioResolucionPrimercontactoSegundosAnterior;

                result.PromedioResolucionPrimercontactoPorcentaje = previousAvgHourMinFirstContact == 0 ? 0 : (diffPeriodsSecondsFirstContact / previousAvgSecondsFirstContact) * 100;

                #endregion Tiempo Resolucion Primer Contacto

                #region Intervenciones Agente

                //Intervenciones Agente Primer Contacto Actual
                List<int> currentInterventionsAgents = new List<int>();
                List<int> currentTicketsInterventionsAgents = db.TicketAsignacion.Where(w => lstCreatedTicketsCurrent.Contains(w.IdTicket) && lstEstatusResuelto.Contains(w.Ticket.IdEstatusTicket) && w.IdUsuarioAsignado != w.Ticket.IdUsuarioLevanto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in currentTicketsInterventionsAgents)
                {
                    List<int?> firstFirstContact = db.TicketAsignacion.Where(w => w.IdTicket == idticket && w.IdUsuarioAsignado != w.Ticket.IdUsuarioLevanto && w.IdUsuarioAsignado != null).Select(s => s.IdUsuarioAsignado).Distinct().ToList();
                    if (firstFirstContact.Count > 0)
                    {
                        currentInterventionsAgents.Add(firstFirstContact.Count);
                    }
                }

                result.PromedioIntervencionesAgenteActual = currentInterventionsAgents.Count > 0 ? currentInterventionsAgents.Average(a => a) : 0;

                //Intervenciones Agente Primer Contacto Anterior
                List<int> previousInterventionsAgents = new List<int>();
                List<int> previousTicketsInterventionsAgents = db.TicketAsignacion.Where(w => lstCreatedTicketsPrevious.Contains(w.IdTicket) && lstEstatusResuelto.Contains(w.Ticket.IdEstatusTicket) && w.IdUsuarioAsignado != w.Ticket.IdUsuarioLevanto).Select(s => s.IdTicket).Distinct().ToList();
                foreach (int idticket in previousTicketsInterventionsAgents)
                {
                    List<int?> firstFirstContactprevious = db.TicketAsignacion.Where(w => w.IdTicket == idticket && w.IdUsuarioAsignado != w.Ticket.IdUsuarioLevanto && w.IdUsuarioAsignado != null).Select(s => s.IdUsuarioAsignado).Distinct().ToList();
                    if (firstFirstContactprevious.Count > 0)
                    {
                        previousInterventionsAgents.Add(firstFirstContactprevious.Count);
                    }
                }
                result.PromedioIntervencionesAgenteAnterior = previousInterventionsAgents.Count > 0 ? previousInterventionsAgents.Average(a => a) : 0;
                result.PromedioIntervencionesAgentePorcentaje = Math.Abs(result.PromedioIntervencionesAgenteAnterior) == 0 ? 0 : ((result.PromedioIntervencionesAgenteActual - result.PromedioIntervencionesAgenteAnterior) / result.PromedioIntervencionesAgenteAnterior) * 100;

                #endregion Intervenciones Agente Primer Contacto

                #region Clientes Unicos

                result.ClientesAtendidosActual = db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id)).Select(s => s.IdUsuarioLevanto).Distinct().ToList().Count;

                result.ClientesAtendidosAnterior = db.Ticket.Where(w => lstCreatedTicketsPrevious.Contains(w.Id)).Select(s => s.IdUsuarioLevanto).Distinct().ToList().Count;

                result.ClientesAtendidosPorcentaje = Math.Abs(result.ClientesAtendidosAnterior) == 0 ? 0 : ((result.ClientesAtendidosActual - result.ClientesAtendidosAnterior) / result.ClientesAtendidosAnterior) * 100;
                #endregion Clientes Unicos

                #region graficos

                DataTable dtTicketsAbiertos = new DataTable("dt");
                dtTicketsAbiertos.Columns.Add(new DataColumn("Id", typeof(int)));
                dtTicketsAbiertos.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                dtTicketsAbiertos.Columns.Add(new DataColumn("Total", typeof(decimal)));
                var ticketsAbiertos = db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id)).Join(db.TipoArbolAcceso, t => t.IdTipoArbolAcceso, taa => taa.Id, (t, taa) => new { taa.Id, taa.Descripcion }).GroupBy(g =>
                    new
                    {
                        g.Id,
                        g.Descripcion
                    }
                    ).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() });

                foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id)).Join(db.TipoArbolAcceso, t => t.IdTipoArbolAcceso, taa => taa.Id, (t, taa) => new { taa.Id, taa.Descripcion }).GroupBy(g => new { g.Id, g.Descripcion }).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }))
                {
                    dtTicketsAbiertos.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                result.GraficoTicketsAbiertos = dtTicketsAbiertos;

                DataTable dtTicketsPrioridad = new DataTable("dt");
                dtTicketsPrioridad.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                dtTicketsPrioridad.Columns.Add(new DataColumn("Problemas", typeof(decimal)));
                dtTicketsPrioridad.Columns.Add(new DataColumn("Servicios", typeof(decimal)));
                foreach (string impacto in db.Impacto.Select(s => s.Descripcion).Distinct())
                {
                    dtTicketsPrioridad.Rows.Add(impacto);
                }
                foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ReportarProblemas)
                    .Join(db.Impacto, t => t.IdImpacto, i => i.Id, (t, i) => new { i.Id, i.Descripcion }).GroupBy(g => new { g.Id, g.Descripcion })
                    .Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }))
                {
                    foreach (DataRow row in dtTicketsPrioridad.Rows)
                    {
                        if (row[0].ToString() == datos.Descripcion)
                        {
                            for (int c = 0; c < dtTicketsPrioridad.Columns.Count; c++)
                            {
                                if (dtTicketsPrioridad.Columns[c].ColumnName == "Problemas")
                                {
                                    row[c] = datos.Total;
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.SolicitarServicio)
                    .Join(db.Impacto, t => t.IdImpacto, i => i.Id, (t, i) => new { i.Id, i.Descripcion }).GroupBy(g => new { g.Id, g.Descripcion })
                    .Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }))
                {
                    foreach (DataRow row in dtTicketsPrioridad.Rows)
                    {
                        if (row[0].ToString() == datos.Descripcion)
                        {
                            for (int c = 0; c < dtTicketsPrioridad.Columns.Count; c++)
                            {
                                if (dtTicketsPrioridad.Columns[c].ColumnName == "Servicios")
                                {
                                    row[c] = datos.Total;
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach (DataRow row in dtTicketsPrioridad.Rows)
                {
                    for (int c = 0; c < dtTicketsPrioridad.Columns.Count; c++)
                    {
                        if (row[c].ToString() == string.Empty)
                            row[c] = 0;
                    }
                }

                result.GraficoTicketsPrioridad = dtTicketsPrioridad;

                DataTable dtTicketsCanal = new DataTable("dt");
                dtTicketsCanal.Columns.Add(new DataColumn("Id", typeof(int)));
                dtTicketsCanal.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                dtTicketsCanal.Columns.Add(new DataColumn("Total", typeof(decimal)));

                foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id)).Join(db.Canal, t => t.IdCanal, c => c.Id, (t, c) => new { c.Id, c.Descripcion }).GroupBy(g => new { g.Id, g.Descripcion }).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }))
                {
                    dtTicketsCanal.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                result.GraficoTicketsCanal = dtTicketsCanal;

                DataTable dtTicketsCreadosAbiertos = new DataTable("dt");
                dtTicketsCreadosAbiertos.Columns.Add(new DataColumn("Estatus", typeof(string)));
                for (int i = 0; i <= ObtenerDiasPeriodo(); i++)
                {
                    DataColumn col = new DataColumn
                    {
                        //DefaultValue = 0,
                        DataType = typeof(decimal),
                        ColumnName = endCurrentDate.AddDays(i).ToShortDateString()
                    };
                    dtTicketsCreadosAbiertos.Columns.Add(col);

                }
                for (int i = 0; i <= ObtenerDiasPeriodo(); i++)
                {
                    DateTime fInicio = Convert.ToDateTime(endCurrentDate.AddDays(i).ToShortDateString() + " 00:00:00");
                    DateTime fFin = Convert.ToDateTime(fInicio.AddDays(1).ToShortDateString());
                    //Tickets Creados
                    var test = db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.FechaHoraAlta >= fInicio && w.FechaHoraAlta < fFin);
                    foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.FechaHoraAlta >= fInicio && w.FechaHoraAlta < fFin).GroupBy(g => new { Descripcion = "Creados" }).Select(s => new GraficoConteo { Descripcion = "Creados", Total = s.Count() }))
                    {
                        int row = 0;

                        if (dtTicketsCreadosAbiertos.Rows.Count > 0)
                        {
                            bool encontroColumna = false;
                            for (int j = 0; j < dtTicketsCreadosAbiertos.Rows.Count; j++)
                            {
                                if (dtTicketsCreadosAbiertos.Rows[j][0].ToString() != datos.Descripcion) continue;
                                encontroColumna = true;
                                break;
                            }
                            if (!encontroColumna)
                            {
                                dtTicketsCreadosAbiertos.Rows.Add(datos.Descripcion);
                            }
                            for (int j = 0; j < dtTicketsCreadosAbiertos.Rows.Count; j++)
                            {
                                if (dtTicketsCreadosAbiertos.Rows[j][0].ToString() != datos.Descripcion) continue;
                                for (int c = 0; c < dtTicketsCreadosAbiertos.Columns.Count; c++)
                                {
                                    if (dtTicketsCreadosAbiertos.Columns[c].ColumnName == fInicio.ToShortDateString())
                                    {
                                        dtTicketsCreadosAbiertos.Rows[j][c] = datos.Total;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dtTicketsCreadosAbiertos.Rows.Add(datos.Descripcion);
                            for (int c = 0; c < dtTicketsCreadosAbiertos.Columns.Count; c++)
                            {
                                if (dtTicketsCreadosAbiertos.Columns[c].ColumnName == fInicio.ToShortDateString())
                                {
                                    dtTicketsCreadosAbiertos.Rows[0][c] = datos.Total;
                                    break;
                                }

                            }
                        }
                    }
                    //Tickets Resuletos
                    foreach (GraficoConteo datos in db.Ticket.Where(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.FechaHoraAlta >= fInicio && w.FechaHoraAlta <= fFin && lstEstatusResuelto.Contains(w.IdEstatusTicket)).Join(db.EstatusTicket, t => t.IdEstatusTicket, e => e.Id, (t, e) => new { e.Id, e.Descripcion }).GroupBy(g => new { g.Id, g.Descripcion }).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }))
                    {
                        int row = 0;

                        if (dtTicketsCreadosAbiertos.Rows.Count > 0)
                        {
                            bool encontroColumna = false;
                            for (int j = 0; j < dtTicketsCreadosAbiertos.Rows.Count; j++)
                            {
                                if (dtTicketsCreadosAbiertos.Rows[j][0].ToString() != datos.Descripcion) continue;
                                encontroColumna = true;
                                break;
                            }
                            if (!encontroColumna)
                            {
                                dtTicketsCreadosAbiertos.Rows.Add(datos.Descripcion);
                            }
                            for (int j = 0; j < dtTicketsCreadosAbiertos.Rows.Count; j++)
                            {
                                if (dtTicketsCreadosAbiertos.Rows[j][0].ToString() != datos.Descripcion) continue;
                                for (int c = 0; c < dtTicketsCreadosAbiertos.Columns.Count; c++)
                                {
                                    if (dtTicketsCreadosAbiertos.Columns[c].ColumnName == fInicio.ToShortDateString())
                                    {
                                        dtTicketsCreadosAbiertos.Rows[j][c] = datos.Total;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dtTicketsCreadosAbiertos.Rows.Add(datos.Descripcion);
                            for (int c = 0; c < dtTicketsCreadosAbiertos.Columns.Count; c++)
                            {
                                if (dtTicketsCreadosAbiertos.Columns[c].ColumnName == fInicio.ToShortDateString())
                                {
                                    dtTicketsCreadosAbiertos.Rows[0][c] = datos.Total;

                                    break;
                                }
                            }
                        }
                    }
                }

                foreach (DataRow row in dtTicketsCreadosAbiertos.Rows)
                {
                    for (int j = 0; j < dtTicketsCreadosAbiertos.Columns.Count; j++)
                    {
                        if (row[j].ToString() == string.Empty)
                            row[j] = "0";
                    }
                }

                result.GraficoTicketsCreadosResueltos = dtTicketsCreadosAbiertos;
                #endregion graficos

                #region MetricasGrupos

                List<int> lstEnumImpacto = db.Impacto.Where(w => w.Descripcion == BusinessVariables.EnumImpacto.Alto).Select(s => s.Id).Distinct().ToList();
                List<HelperMetricas> metrics = db.GrupoUsuario.Join(db.TicketGrupoUsuario, gu => gu.Id, tgu => tgu.IdGrupoUsuario, (gu, tgu) => new { gu, tgu })
                    .Join(db.Ticket, @t1 => @t1.tgu.IdTicket, t => t.Id, (@t1, t) => new { @t1, t })
                    .Where(@t1 => @t1.@t1.gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && lstCreatedTicketsCurrent.Contains(@t1.t.Id))
                    .GroupBy(g => new { idticket = g.t.Id, idgrupo = g.t1.gu.Id, g.t1.gu.Descripcion })
                    .Select(s => new { IdTicket = s.Key.idticket, IdGrupo = s.Key.idgrupo, DescripcionGrupo = s.Key.Descripcion })
                    .GroupBy(g => new { g.IdGrupo, g.DescripcionGrupo })
                    .Select(s => new HelperMetricas { IdGrupo = s.Key.IdGrupo, DescripcionGrupo = s.Key.DescripcionGrupo, TotalActual = s.Count() })
                    .OrderByDescending(o => o.TotalActual)
                    .Take(5)
                    .ToList();
                foreach (HelperMetricas metricas in metrics)
                {
                    metricas.TotalAnterior = lstCreatedTicketsPrevious.Count <= 0 ? 0 : db.GrupoUsuario.Join(db.TicketGrupoUsuario, gu => gu.Id, tgu => tgu.IdGrupoUsuario, (gu, tgu) => new { gu, tgu })
                    .Join(db.Ticket, @t1 => @t1.tgu.IdTicket, t => t.Id, (@t1, t) => new { @t1, t })
                    .Where(@t1 => @t1.@t1.gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && lstCreatedTicketsPrevious.Contains(@t1.t.Id) && @t1.t1.gu.Id == metricas.IdGrupo)
                    .GroupBy(g => new { idticket = g.t.Id, idgrupo = g.t1.gu.Id, g.t1.gu.Descripcion })
                    .Select(s => new { IdTicket = s.Key.idticket, IdGrupo = s.Key.idgrupo, DescripcionGrupo = s.Key.Descripcion })
                    .GroupBy(g => new { g.IdGrupo, g.DescripcionGrupo })
                    .Select(s => new HelperMetricas { IdGrupo = s.Key.IdGrupo, DescripcionGrupo = s.Key.DescripcionGrupo, TotalActual = s.Count() })
                    .OrderByDescending(o => o.TotalActual)
                    .ToList().First().TotalActual;
                    metricas.TotalPorcentaje = metricas.TotalAnterior == 0 ? 0 : ((metricas.TotalActual - metricas.TotalAnterior) + 100);

                    metricas.TotalAbiertosActual = db.Ticket.Count(w => lstCreatedTicketsCurrent.Contains(w.Id) && w.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto);
                    metricas.TotalAbiertosAnterior = db.Ticket.Count(w => lstCreatedTicketsPrevious.Contains(w.Id) && w.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto);
                    metricas.TotalAbiertosPorcentaje = metricas.TotalAbiertosAnterior == 0 ? 0 : ((metricas.TotalAbiertosActual - metricas.TotalAbiertosAnterior) / metricas.TotalAbiertosAnterior) * 100;

                    metricas.TotalAbiertosActual = db.Ticket.Count(w => lstCreatedTicketsCurrent.Contains(w.Id) && lstEnumImpacto.Contains(w.IdImpacto));
                    metricas.TotalAbiertosAnterior = db.Ticket.Count(w => lstCreatedTicketsPrevious.Contains(w.Id) && lstEnumImpacto.Contains(w.IdImpacto));
                    metricas.TotalAbiertosPorcentaje = metricas.TotalAbiertosAnterior == 0 ? 0 : ((metricas.TotalAbiertosActual - metricas.TotalAbiertosAnterior) / metricas.TotalAbiertosAnterior) * 100;

                }

                #endregion MetricasGrupos

                result.GruposMetricas = metrics;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
    }
}
