using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessMascaras : IDisposable
    {
        private bool _proxy;

        public bool ValidaEstructuraMascara(Mascara mascara)
        {
            bool result = false;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var pTableName = new SqlParameter { ParameterName = "@STORENAME", Value = mascara.NombreTabla };
                var pResult = new SqlParameter { ParameterName = "@OUTER", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };
                //Tabla
                db.ExecuteStoreCommand("exec ExisteTablaMascara @STORENAME, @OUTER output", pTableName, pResult);
                result = (int)pResult.Value == 1;
                if (result)
                {
                    //Comando Insertar
                    pTableName = new SqlParameter { ParameterName = "@STORENAME", Value = mascara.ComandoInsertar };
                    pResult = new SqlParameter
                    {
                        ParameterName = "@OUTER",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.Int
                    };
                    db.ExecuteStoreCommand("exec ExisteStore @STORENAME, @OUTER output", pTableName, pResult);
                    result = (int)pResult.Value == 1;
                    if (result)
                    {
                        //Comando actualizar
                        pTableName = new SqlParameter { ParameterName = "@STORENAME", Value = mascara.ComandoActualizar };
                        pResult = new SqlParameter
                        {
                            ParameterName = "@OUTER",
                            Direction = ParameterDirection.Output,
                            SqlDbType = SqlDbType.Int
                        };
                        db.ExecuteStoreCommand("exec ExisteStore @STORENAME, @OUTER output", pTableName, pResult);
                        result = (int)pResult.Value == 1;
                    }
                }

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
        public BusinessMascaras(bool proxy = false)
        {
            _proxy = proxy;
        }

        private bool CrearEstructuraMascaraBaseDatos(Mascara mascara)
        {
            try
            {
                if (CreaTabla(mascara))
                    if (CrearInsert(mascara))
                        CreaUpdate(mascara);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        private bool CreaTabla(Mascara mascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string queryCamposTabla = string.Empty;
                foreach (CampoMascara campoMascara in mascara.CampoMascara)
                {

                    TipoCampoMascara tmpTipoCampoMascara = db.TipoCampoMascara.SingleOrDefault(f => f.Id == campoMascara.IdTipoCampoMascara);
                    if (tmpTipoCampoMascara == null) continue;
                    switch (tmpTipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDepledable:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            if (campoMascara.IdCatalogo != null)
                                queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            queryCamposTabla += String.Format("[{0}] {1} {2},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                    }

                    //switch (tmpTipoCampoMascara.TipoDatoSql)
                    //{
                    //    case "NVARCHAR":
                    //        queryCamposTabla += String.Format("{0} {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima, campoMascara.Requerido ? "NOT NULL" : "NULL");
                    //        break;
                    //    default:
                    //        queryCamposTabla += String.Format("{0} {1} {2},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.Requerido ? "NOT NULL" : "NULL");
                    //        break;
                    //}
                }
                string qryCrearTablas = String.Format("CREATE TABLE {0} ( \n" +
                                                      "Id int IDENTITY(1,1) NOT NULL, \n" +
                                                      "IdTicket int NOT NULL, \n" +
                                                      "{1}" +
                                                      "Habilitado BIT \n" +
                                                      (mascara.Random ? ", " + BusinessVariables.ParametrosMascaraCaptura.CampoRandom + " \n" : string.Empty) +
                                                      "CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED \n" +
                                                      "( \n" +
                                                      "\t[Id] ASC \n" +
                                                      ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] \n" +
                                                      ") ON [PRIMARY] \n" +
                                                      "ALTER TABLE [dbo].[{0}]  WITH CHECK ADD  CONSTRAINT [FK_{0}_Ticket] FOREIGN KEY([IdTicket]) \n" +
                                                      "REFERENCES [dbo].[Ticket] ([Id])\n" +
                                                      "ALTER TABLE [dbo].[{0}] CHECK CONSTRAINT [FK_{0}_Ticket]\n" +
                                                      "ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_habilitado]  DEFAULT ((1)) FOR [Habilitado]", mascara.NombreTabla, queryCamposTabla);
                db.ExecuteStoreCommand(qryCrearTablas);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return true;
        }

        private bool CrearInsert(Mascara mascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string queryParametros = "@IDTICKET int, ";
                string queryCampos = "IDTICKET, ";
                string queryValues = "@IDTICKET, ";
                foreach (CampoMascara campoMascara in mascara.CampoMascara)
                {
                    TipoCampoMascara tmpTipoCampoMascara = db.TipoCampoMascara.SingleOrDefault(f => f.Id == campoMascara.IdTipoCampoMascara);
                    if (tmpTipoCampoMascara == null) continue;

                    switch (tmpTipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDepledable:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            if (campoMascara.IdCatalogo != null)
                                //foreach (CatalogoGenerico generico in new BusinessCatalogos().ObtenerRegistrosSistemaCatalogo((int)campoMascara.IdCatalogo, false))
                                //{
                                queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            //}

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, tmpTipoCampoMascara.TipoDatoSql);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                    }



                    //switch (tmpTipoCampoMascara.TipoDatoSql)
                    //{
                    //    case "NVARCHAR":
                    //        queryParametros += String.Format("@{0} {1}({2})", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima);
                    //        break;
                    //    default:
                    //        queryParametros += String.Format("@{0} {1}", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                    //        break;
                    //}
                    //queryCampos += String.Format("{0}", campoMascara.NombreCampo);
                    //queryValues += String.Format("@{0}", campoMascara.NombreCampo);
                    //if (contadorParametros < paramsCount)
                    //{
                    //    queryParametros += ", \n";
                    //    queryCampos += ", \n";
                    //    queryValues += ", \n";
                    //}
                }

                if (mascara.Random)
                {
                    queryParametros += String.Format("@{0} {1}", BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom, BusinessVariables.ParametrosMascaraCaptura.TipoCampoRandom);
                    queryCampos += BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom;
                    queryValues += String.Format("@{0}", BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom);
                }

                string queryStore = string.Format("Create  PROCEDURE {0}( \n" +
                                                  "{1}" +
                                                  ") \n" +
                                                  "AS \n" +
                                                  "BEGIN \n" +
                                                  "INSERT INTO {2}({3}) \n" +
                                                  "VALUES({4}) \n" +
                                                  "END", mascara.ComandoInsertar, queryParametros, mascara.NombreTabla, queryCampos, queryValues);
                db.ExecuteStoreCommand(queryStore);
            }
            catch (Exception ex)
            {
                EliminarObjetoBaseDeDatos(mascara.NombreTabla, BusinessVariables.EnumTipoObjeto.Tabla);
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return true;
        }

        private bool CreaUpdate(Mascara mascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string queryParametros = string.Empty;
                string queryCamposValues = string.Empty;
                string queryWhereValues = "Id = @ID";
                int contadorParametros = 0;
                foreach (CampoMascara campoMascara in mascara.CampoMascara)
                {
                    contadorParametros++;
                    TipoCampoMascara tmpTipoCampoMascara = db.TipoCampoMascara.SingleOrDefault(f => f.Id == campoMascara.IdTipoCampoMascara);
                    if (tmpTipoCampoMascara == null) continue;
                    switch (tmpTipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDepledable:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            if (campoMascara.IdCatalogo != null)
                                //foreach (CatalogoGenerico generico in new BusinessCatalogos().ObtenerRegistrosSistemaCatalogo((int)campoMascara.IdCatalogo, false))
                                //{
                                queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            //}

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);

                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                    }

                    //queryParametros += String.Format("@{0} {1}", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                    //queryCamposValues += String.Format("{0} = @{0}", campoMascara.NombreCampo);

                }

                if (mascara.Random)
                {
                    queryParametros += String.Format("@{0} {1}", BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom, BusinessVariables.ParametrosMascaraCaptura.TipoCampoRandom);
                    queryCamposValues += String.Format("{0} = @{0}", BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom);
                }

                string queryStore = string.Format("Create  PROCEDURE {0}( \n" +
                                                  "@ID INT, \n" +
                                                  "{1}" +
                                                  ") \n" +
                                                  "AS \n" +
                                                  "BEGIN \n" +
                                                  "UPDATE {2} \n" +
                                                  "SET {3} \n" +
                                                  "WHERE {4} \n" +
                                                  "END", mascara.ComandoActualizar, queryParametros, mascara.NombreTabla, queryCamposValues, queryWhereValues);
                db.ExecuteStoreCommand(queryStore);
            }
            catch (Exception ex)
            {
                EliminarObjetoBaseDeDatos(mascara.NombreTabla, BusinessVariables.EnumTipoObjeto.Tabla);
                EliminarObjetoBaseDeDatos(mascara.ComandoInsertar, BusinessVariables.EnumTipoObjeto.Store);
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return true;
        }

        private void EliminarObjetoBaseDeDatos(string nombreObjeto, BusinessVariables.EnumTipoObjeto objeto)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string query = "DROP ";
                switch (objeto)
                {
                    case BusinessVariables.EnumTipoObjeto.Tabla:
                        query += "TABLE " + nombreObjeto;
                        break;
                    case BusinessVariables.EnumTipoObjeto.Store:
                        query += "PROCEDURE " + nombreObjeto;
                        break;
                }
                db.ExecuteStoreCommand(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void CrearMascara(Mascara mascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                mascara.Descripcion = mascara.Descripcion.Trim();
                if (db.Mascara.Any(a => a.Descripcion == mascara.Descripcion && a.Id != mascara.Id))
                    throw new Exception("Ya existe un Formulario con este nombre");
                mascara.NoCampos = mascara.CampoMascara.Count;
                foreach (CampoMascara campoMascara in mascara.CampoMascara)
                {
                    campoMascara.Descripcion = campoMascara.Descripcion.Trim();
                    campoMascara.NombreCampo = BusinessCadenas.Cadenas.FormatoBaseDatos(campoMascara.Descripcion.Trim()).Replace(" ", "").ToUpper();
                    campoMascara.SimboloMoneda = campoMascara.SimboloMoneda == null ? null : campoMascara.SimboloMoneda.Trim();
                    campoMascara.TipoCampoMascara = null;
                    campoMascara.Catalogos = null;
                }
                Guid id = Guid.NewGuid();
                string[] words = mascara.Descripcion.Split(' ');
                string name = words.Aggregate<string, string>(null, (current, word) => current + word.Substring(0, 1));
                name += id.ToString();
                mascara.NombreTabla = (BusinessVariables.ParametrosMascaraCaptura.PrefijoTabla + BusinessCadenas.Cadenas.FormatoBaseDatos(name)).ToUpper();
                mascara.ComandoInsertar = (BusinessVariables.ParametrosMascaraCaptura.PrefijoComandoInsertar + BusinessCadenas.Cadenas.FormatoBaseDatos(name).Replace(" ", string.Empty)).ToUpper();
                mascara.ComandoActualizar = (BusinessVariables.ParametrosMascaraCaptura.PrefijoComandoActualizar + BusinessCadenas.Cadenas.FormatoBaseDatos(name).Replace(" ", string.Empty)).ToUpper();
                mascara.Habilitado = true;

                ExisteMascara(mascara);
                CrearEstructuraMascaraBaseDatos(mascara);
                mascara.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                mascara.IdUsuarioModifico = null;
                mascara.FechaModificacion = null;
                db.Mascara.AddObject(mascara);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void ActualizarMascara(Mascara mascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                mascara.Descripcion = mascara.Descripcion.Trim();
                if (db.Mascara.Any(a => a.Descripcion == mascara.Descripcion && a.Id != mascara.Id))
                    throw new Exception("Ya existe un Formulario con este nombre");
                Mascara dbMascara = db.Mascara.SingleOrDefault(s => s.Id == mascara.Id);
                if (dbMascara != null)
                {
                    dbMascara.Descripcion = mascara.Descripcion;
                    foreach (CampoMascara campoMascara in dbMascara.CampoMascara)
                    {
                        campoMascara.Descripcion = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).Descripcion;
                        campoMascara.LongitudMinima = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).LongitudMinima;
                        campoMascara.LongitudMaxima = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).LongitudMaxima;
                        campoMascara.ValorMinimo = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).ValorMinimo;
                        campoMascara.ValorMaximo = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).ValorMaximo;
                        campoMascara.SimboloMoneda = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).SimboloMoneda;
                        campoMascara.MascaraDetalle = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).MascaraDetalle;
                    }
                    dbMascara.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    dbMascara.IdUsuarioModifico = mascara.IdUsuarioAlta;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public bool ExisteMascara(Mascara mascara)
        {
            bool result = false;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var pTableName = new SqlParameter { ParameterName = "@TABLENAME", Value = mascara.NombreTabla };
                var pResult = new SqlParameter { ParameterName = "@OUTER", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };
                db.ExecuteStoreCommand("exec ExisteTablaMascara @TABLENAME, @OUTER output", pTableName, pResult);
                result = (int)pResult.Value == 1;
                if (result)
                    throw new Exception("Ya existe un Formulario con este nombre");

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

        public void Dispose()
        {
        }

        public List<Mascara> ObtenerMascarasAcceso(bool insertarSeleccion)
        {
            List<Mascara> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Mascara.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Mascara { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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

        public Mascara ObtenerMascaraCaptura(int idMascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            Mascara result;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Mascara.SingleOrDefault(s => s.Id == idMascara);
                if (result != null)
                {
                    db.LoadProperty(result, "CampoMascara");
                    foreach (CampoMascara campoMascara in result.CampoMascara)
                    {
                        db.LoadProperty(campoMascara, "TipoCampoMascara");
                        db.LoadProperty(campoMascara, "Catalogos");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public Mascara ObtenerMascaraCapturaByIdTicket(int idTicket)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            Mascara result = null;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket ticket = db.Ticket.Single(s => s.Id == idTicket);
                if (ticket != null)
                {
                    db.LoadProperty(ticket, "Mascara");
                    result = ticket.Mascara;
                    if (result != null)
                    {
                        db.LoadProperty(result, "CampoMascara");
                        foreach (CampoMascara campoMascara in result.CampoMascara)
                        {
                            db.LoadProperty(campoMascara, "TipoCampoMascara");
                            db.LoadProperty(campoMascara, "Catalogos");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public List<CatalogoGenerico> ObtenerCatalogoCampoMascara(int idCatalogo, bool insertarSeleccion, bool filtraHabilitados)
        {
            List<CatalogoGenerico> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Catalogos catalogo = db.Catalogos.SingleOrDefault(s => s.Id == idCatalogo);
                if (catalogo != null)
                {
                    var x = "ObtenerCatalogoSistema '" + catalogo.Tabla + "', " + Convert.ToInt32(insertarSeleccion) + ", " + Convert.ToInt32(filtraHabilitados);
                    result = db.ExecuteStoreQuery<CatalogoGenerico>("ObtenerCatalogoSistema '" + catalogo.Tabla + "', " + Convert.ToInt32(insertarSeleccion) + ", " + Convert.ToInt32(filtraHabilitados)).ToList();
                }
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

        public List<Mascara> Consulta(string descripcion)
        {
            List<Mascara> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Mascara> qry = db.Mascara.Where(w => !w.Sistema);
                descripcion = descripcion.ToLower().Trim();
                if (descripcion.Trim() != string.Empty)
                    qry = qry.Where(w => w.Descripcion.ToLower().Contains(descripcion));
                result = qry.ToList();
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

        public void HabilitarMascara(int idMascara, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Mascara mascara = db.Mascara.SingleOrDefault(w => w.Id == idMascara);
                if (mascara != null) mascara.Habilitado = habilitado;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public List<HelperMascaraData> ObtenerDatosMascara(int idMascara, int idTicket)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperMascaraData> result = null;
            try
            {
                Mascara mascara = db.Mascara.SingleOrDefault(s => s.Id == idMascara);
                if (mascara != null)
                {
                    db.LoadProperty(mascara, "CampoMascara");
                    string campos = string.Empty;
                    foreach (CampoMascara campoMascara in mascara.CampoMascara)
                    {
                        switch (campoMascara.IdTipoCampoMascara)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDepledable:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                                campos += campoMascara.NombreCampo + ", ";
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                                campos += campoMascara.NombreCampo + ", ";
                                //if (campoMascara.IdCatalogo != null)
                                //{
                                //    Catalogos cat = new BusinessCatalogos().ObtenerCatalogo((int)campoMascara.IdCatalogo);
                                //    if (!cat.Archivo)
                                //        campos = new BusinessCatalogos().ObtenerRegistrosSistemaCatalogo(cat.Id, false).Aggregate(campos, (current, catalogoGenerico) => current + (campoMascara.NombreCampo + BusinessCadenas.Cadenas.FormatoBaseDatos(catalogoGenerico.Descripcion) + ", "));
                                //}
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                                campos += campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio + ", ";
                                campos += campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin + ", ";
                                break;
                        }

                    }
                    //= mascara.CampoMascara.Aggregate(string.Empty, (current, campoMascara) => current + (campoMascara.NombreCampo + ", "));
                    if (mascara.Random)
                        campos += BusinessVariables.ParametrosMascaraCaptura.NombreCampoRandom;
                    else
                        campos = campos.Trim().TrimEnd(',');

                    DataSet retVal = new DataSet();
                    EntityConnection entityConn = (EntityConnection)db.Connection;
                    SqlConnection sqlConn = (SqlConnection)entityConn.StoreConnection;
                    SqlCommand cmdReport = new SqlCommand(string.Format("select {0} from {1} where IdTicket = {2}", campos, mascara.NombreTabla, idTicket), sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.Text;
                        daReport.Fill(retVal);
                    }

                    if (retVal.Tables.Count > 0)
                    {
                        if (retVal.Tables[0].Rows.Count > 0)
                        {
                            result = new List<HelperMascaraData>();
                            foreach (DataRow row in retVal.Tables[0].Rows)
                            {
                                foreach (DataColumn column in retVal.Tables[0].Columns)
                                {
                                    HelperMascaraData data = new HelperMascaraData();
                                    data.Campo = column.ColumnName;
                                    data.Value = row[column.ColumnName].ToString();
                                    result.Add(data);
                                }
                                break;
                            }
                        }

                    }

                }
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
