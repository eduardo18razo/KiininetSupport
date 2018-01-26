using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using KiiniNet.Entities.Operacion.Dashboard;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;
using System.Data.SqlClient;

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
                dtTickets.Columns.Add(new DataColumn("Id"));
                dtTickets.Columns.Add(new DataColumn("Descripcion"));
                dtTickets.Columns.Add(new DataColumn("Total"));
                foreach (GraficoConteo datos in db.Ticket.Join(db.Canal, ticket => ticket.IdCanal, canal => canal.Id, (ticket, canals) => new { ticket, canals }).GroupBy(g => g.canals).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }).ToList())
                {
                    dtTickets.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                results.GraficoTicketsCreadosCanal = dtTickets;


                DataTable dtUsuarios = new DataTable("dt");
                dtUsuarios.Columns.Add(new DataColumn("Id"));
                dtUsuarios.Columns.Add(new DataColumn("Descripcion"));
                dtUsuarios.Columns.Add(new DataColumn("Total"));
                foreach (GraficoConteo datos in db.Usuario.Join(db.TipoUsuario, users => users.IdTipoUsuario, tipoUsuario => tipoUsuario.Id, (users, tipoUsuario) => new { users, tipoUsuario }).Where(w => userFilters.Contains(w.users.IdTipoUsuario)).GroupBy(g => g.tipoUsuario).Select(s => new GraficoConteo { Id = s.Key.Id, Descripcion = s.Key.Descripcion, Total = s.Count() }).ToList())
                {
                    dtUsuarios.Rows.Add(datos.Id, datos.Descripcion, datos.Total);
                }
                results.GraficoUsuariosRegistrados = dtUsuarios;

                DataTable dtAlmacenamiento = new DataTable("dt");
                dtAlmacenamiento.Columns.Add(new DataColumn("Ocupado"));
                dtAlmacenamiento.Columns.Add(new DataColumn("Libre"));


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
                if (totalSize > 1024)
                    totalSize = totalSize / 1024;
                dtAlmacenamiento.Rows.Add(totalSize, maxSize - totalSize);
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
    }
}
