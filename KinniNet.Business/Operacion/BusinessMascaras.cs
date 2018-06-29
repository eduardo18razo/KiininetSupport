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
using KinniNet.Core.Sistema;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessMascaras : IDisposable
    {
        private bool _proxy;

        public bool ValidaEstructuraMascara(Mascara mascara)
        {
            bool result;
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

        private bool CrearEstructuraMascaraBaseDatos(Mascara mascara, string tablaLlave, string campoLlave)
        {
            try
            {
                if (CreaTabla(mascara, tablaLlave, campoLlave))
                    if (CrearInsert(mascara, tablaLlave, campoLlave))
                        CreaUpdate(mascara);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        private bool CreaTabla(Mascara mascara, string tablaLlave, string campoLlave)
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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            queryCamposTabla += String.Format("[{0}] {1}({2}) {3},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida, campoMascara.Requerido ? "NOT NULL" : "NULL");
                            break;
                    }
                }
                string qryCrearTablas = String.Format("CREATE TABLE {0} ( \n" +
                                                      "Id int IDENTITY(1,1) NOT NULL, \n" +
                                                      "{3}{2} int NOT NULL, \n" +
                                                      "{1}" +
                                                      "Habilitado BIT \n" +
                                                      (mascara.Random ? ", " + BusinessVariables.ParametrosMascaraCaptura.CampoRandom + " \n" : string.Empty) +
                                                      "CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED \n" +
                                                      "( \n" +
                                                      "\t[Id] ASC \n" +
                                                      ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] \n" +
                                                      ") ON [PRIMARY] \n" +
                                                      "ALTER TABLE [dbo].[{0}]  WITH CHECK ADD  CONSTRAINT [FK_{0}_Ticket] FOREIGN KEY([{3}{2}]) \n" +
                                                      "REFERENCES [dbo].[{2}] ([{3}])\n" +
                                                      "ALTER TABLE [dbo].[{0}] CHECK CONSTRAINT [FK_{0}_Ticket]\n" +
                                                      "ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_habilitado]  DEFAULT ((1)) FOR [Habilitado]", mascara.NombreTabla, queryCamposTabla, tablaLlave, campoLlave);
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

        private bool CrearInsert(Mascara mascara, string tablaLlave, string campoLlave)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                string queryParametros = string.Format("@{0}{1} int, ", campoLlave.Trim().ToUpper(), tablaLlave.Trim().ToUpper());
                string queryCampos = string.Format("{0}{1}, ", campoLlave.Trim().ToUpper(), tablaLlave.Trim().ToUpper());
                string queryValues = string.Format("@{0}{1}, ", campoLlave.Trim().ToUpper(), tablaLlave.Trim().ToUpper());
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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            queryParametros += String.Format("@{0} {1}({2}),\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql, tmpTipoCampoMascara.LongitudMaximaPermitida);
                            queryCampos += String.Format("[{0}],\n", campoMascara.NombreCampo);
                            queryValues += String.Format("@{0},\n", campoMascara.NombreCampo);
                            break;
                    }
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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            if (campoMascara.IdCatalogo != null)
                                queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);

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
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            queryParametros += String.Format("@{0} {1},\n", campoMascara.NombreCampo, tmpTipoCampoMascara.TipoDatoSql);
                            queryCamposValues += String.Format("[{0}] = @{0},\n", campoMascara.NombreCampo);
                            break;
                    }

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

                TipoMascara tipoMascara = db.TipoMascara.SingleOrDefault(s => s.Id == mascara.IdTipoMascara);
                if (tipoMascara != null)
                {
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
                    CrearEstructuraMascaraBaseDatos(mascara, tipoMascara.TablaLlave, tipoMascara.CampoLlave);
                    mascara.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    mascara.IdUsuarioModifico = null;
                    mascara.FechaModificacion = null;
                    db.Mascara.AddObject(mascara);
                    db.SaveChanges();
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
        }

        private bool ActualizaCampoToNull(string tabla, string campo, int idTipoCampo, string tipo, int? longitud)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {


                string queryStore = string.Empty;
                switch (idTipoCampo)
                {
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}  NULL;", tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                        queryStore = string.Format("ALTER TABLE [{0}] ALTER COLUMN [{1}] {2} null ;\n", tabla, campo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, tipo);
                        queryStore += string.Format("ALTER TABLE [{0}] ALTER COLUMN [{1}] {2} null ;\n", tabla, campo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, tipo);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                    case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                        queryStore = string.Format("ALTER TABLE [{0}] \n", tabla);
                        queryStore += string.Format("ALTER COLUMN [{0}] \n", campo);
                        queryStore += string.Format(" {0}({1})  NULL;", tipo, longitud);
                        break;
                }
                db.ExecuteStoreCommand(queryStore);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Dispose();
            }
            return true;
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
                        if (campoMascara.Requerido && !mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).Requerido)
                        {
                            ActualizaCampoToNull(dbMascara.NombreTabla, campoMascara.NombreCampo, campoMascara.IdTipoCampoMascara, campoMascara.TipoCampoMascara.TipoDatoSql, campoMascara.LongitudMaxima);
                            campoMascara.Requerido = mascara.CampoMascara.Single(s => s.Id == campoMascara.Id).Requerido;
                        }
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

        public List<Mascara> ObtenerMascarasAcceso(int idTipoMascara, bool sistema, bool insertarSeleccion)
        {
            List<Mascara> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Mascara.Where(w => w.Habilitado && w.IdTipoMascara == idTipoMascara && w.Sistema == sistema).OrderBy(o => o.Descripcion).ToList();
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
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
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

        public DataTable ObtenerReporteMascara(int idMascara, Dictionary<string, DateTime> fechas)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            try
            {
                DataSet retVal = new DataSet();
                EntityConnection entityConn = (EntityConnection)db.Connection;
                SqlConnection sqlConn = (SqlConnection)entityConn.StoreConnection;
                string qryCampos = "SELECT Frm.Id [Id Formulario], t.Id [Id Ticket], iaa.Descripcion [Opcion], u.Nombre [N], u.ApellidoPaterno [P], u.ApellidoMaterno [M], t.FechaHoraAlta, et.Descripcion, \n";
                Mascara mascara = db.Mascara.SingleOrDefault(s => s.Id == idMascara);
                if (mascara != null)
                {
                    db.LoadProperty(mascara, "CampoMascara");
                    foreach (CampoMascara campoMascara in mascara.CampoMascara)
                    {
                        switch (campoMascara.IdTipoCampoMascara)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                                if (campoMascara.IdCatalogo != null)
                                    qryCampos += string.Format("(select Descripcion from {0} where Id = {1}) [{2}],", new BusinessCatalogos().ObtenerCatalogo((int)campoMascara.IdCatalogo).Tabla, campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                                if (campoMascara.IdCatalogo != null)
                                    qryCampos += string.Format("(select Descripcion from {0} where Id = {1}) [{2}],", new BusinessCatalogos().ObtenerCatalogo((int)campoMascara.IdCatalogo).Tabla, campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                                if (campoMascara.IdCatalogo != null)
                                {
                                    db.LoadProperty(campoMascara, "Catalogos");
                                    string qryCamposCasilla = string.Empty;
                                    DataTable registrosCatalogo = new BusinessCatalogos().ObtenerRegistrosArchivosCatalogo((int)campoMascara.IdCatalogo);
                                    foreach (DataRow registro in registrosCatalogo.Rows)
                                    {
                                        int idRegistroCatalogo = (int)registro["Id"];
                                        string descripcionRegistroCatalogo = (string)registro["Descripcion"];
                                        if (idRegistroCatalogo > BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                                        {
                                            qryCamposCasilla += string.Format("CASE WHEN (SELECT COUNT(*) FROM MascaraSeleccionCatalogo msc WHERE IdTicket = Frm.IdTicket and IdRegistroCatalogo = {0}) > 0 then 'Si' else 'No' end [{1}] ,", idRegistroCatalogo, descripcionRegistroCatalogo);
                                        }

                                    }
                                    qryCampos += string.Format("{0}", qryCamposCasilla);
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                                qryCampos += string.Format("CASE WHEN {0} = 0 THEN 'No' ELSE 'Si' End [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                                qryCampos += string.Format("CASE WHEN {0} = 0 THEN 'No' ELSE 'Si' End [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, campoMascara.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, campoMascara.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                                qryCampos += string.Format("CASE WHEN {0} = '' THEN 'No' ELSE 'Si' END [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                        }
                    }
                    qryCampos = qryCampos.Trim().TrimEnd(',');
                    qryCampos += String.Format(" FROM Ticket t \n" +
                                               "INNER JOIN ArbolAcceso aa ON T.IdArbolAcceso = aa.Id\n" +
                                               "INNER JOIN InventarioArbolAcceso iaa ON aa.Id = iaa.IdArbolAcceso\n" +
                                               "INNER JOIN Usuario u ON t.IdUsuarioSolicito = u.Id\n" +
                                               "INNER JOIN EstatusTicket et ON t.IdEstatusTicket = ET.Id \n" +
                                               "INNER JOIN {0} Frm on t.Id = Frm.IdTicket\n", mascara.NombreTabla);
                    if (fechas != null)
                    {
                        string fechaInicio = string.Format("{0}-{1}-{2} 00:00:00",
                            fechas.Single(s => s.Key == "inicio").Value.Year,
                                fechas.Single(s => s.Key == "inicio").Value.Month,
                                    fechas.Single(s => s.Key == "inicio").Value.Day);
                        string fechaFin = string.Format("{0}-{1}-{2} 00:00:00",
                            fechas.Single(s => s.Key == "fin").Value.Year,
                                fechas.Single(s => s.Key == "fin").Value.Month,
                                    fechas.Single(s => s.Key == "fin").Value.AddDays(1).Day);
                        qryCampos += string.Format("WHERE t.FechaHoraAlta >= CONVERT(DATETIME, '{0}') AND t.FechaHoraAlta < CONVERT(DATETIME, '{1}')", fechaInicio, fechaFin);
                    }

                    SqlCommand cmdReport = new SqlCommand((qryCampos), sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.Text;
                        daReport.Fill(retVal);
                    }

                    if (retVal.Tables.Count > 0)
                    {
                        result = retVal.Tables[0];
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

        public DataTable Test(int idMascara)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            try
            {
                DataSet retVal = new DataSet();
                EntityConnection entityConn = (EntityConnection)db.Connection;
                SqlConnection sqlConn = (SqlConnection)entityConn.StoreConnection;
                string qryCampos = "SELECT t.Id [Id Ticket], iaa.Descripcion [Opcion], u.Nombre [N], u.ApellidoPaterno [P], u.ApellidoMaterno [M], t.FechaHoraAlta, et.Descripcion, \n";
                Mascara mascara = db.Mascara.SingleOrDefault(s => s.Id == idMascara);
                if (mascara != null)
                {
                    db.LoadProperty(mascara, "CampoMascara");
                    foreach (CampoMascara campoMascara in mascara.CampoMascara)
                    {
                        switch (campoMascara.IdTipoCampoMascara)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                                if (campoMascara.IdCatalogo != null)
                                    qryCampos += string.Format("(select Descripcion from {0} where Id = {1}) [{2}],", new BusinessCatalogos().ObtenerCatalogo((int)campoMascara.IdCatalogo).Tabla, campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                                if (campoMascara.IdCatalogo != null)
                                    qryCampos += string.Format("(select Descripcion from {0} where Id = {1}) [{2}],", new BusinessCatalogos().ObtenerCatalogo((int)campoMascara.IdCatalogo).Tabla, campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                                if (campoMascara.IdCatalogo != null)
                                {
                                    db.LoadProperty(campoMascara, "Catalogos");
                                    string qryCamposCasilla = string.Empty;
                                    DataTable registrosCatalogo = new BusinessCatalogos().ObtenerRegistrosArchivosCatalogo((int)campoMascara.IdCatalogo);
                                    foreach (DataRow registro in registrosCatalogo.Rows)
                                    {
                                        int idRegistroCatalogo = (int)registro["Id"];
                                        string descripcionRegistroCatalogo = (string)registro["Descripcion"];
                                        if (idRegistroCatalogo > BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                                        {
                                            qryCamposCasilla += string.Format("CASE WHEN (SELECT COUNT(*) FROM MascaraSeleccionCatalogo msc WHERE IdTicket = Frm.IdTicket and IdRegistroCatalogo = {0}) > 0 then 'Si' else 'No' end [{1}] ,", idRegistroCatalogo, descripcionRegistroCatalogo);
                                        }

                                    }
                                    qryCampos += string.Format("{0}", qryCamposCasilla);
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                                qryCampos += string.Format("{0} [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                                qryCampos += string.Format("CASE WHEN {0} = 0 THEN 'No' ELSE 'Si' End [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                                qryCampos += string.Format("CASE WHEN {0} = 0 THEN 'No' ELSE 'Si' End [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio, campoMascara.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                                qryCampos += string.Format("CONVERT(nvarchar(10), {0}, 103) [{1}],", campoMascara.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin, campoMascara.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                                qryCampos += string.Format("CASE WHEN {0} = '' THEN 'No' ELSE 'Si' END [{1}],", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                                qryCampos += string.Format("{0} {1},", campoMascara.NombreCampo, campoMascara.Descripcion);
                                break;
                        }
                    }
                    qryCampos = qryCampos.Trim().TrimEnd(',');
                    qryCampos += String.Format(" FROM Ticket t \n" +
                                               "INNER JOIN ArbolAcceso aa ON T.IdArbolAcceso = aa.Id\n" +
                                               "INNER JOIN InventarioArbolAcceso iaa ON aa.Id = iaa.IdArbolAcceso\n" +
                                               "INNER JOIN Usuario u ON t.IdUsuarioSolicito = u.Id\n" +
                                               "INNER JOIN EstatusTicket et ON t.IdEstatusTicket = ET.Id \n" +
                                               "INNER JOIN {0} Frm on t.Id = Frm.IdTicket", mascara.NombreTabla);
                    SqlCommand cmdReport = new SqlCommand((qryCampos), sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.Text;
                        daReport.Fill(retVal);
                    }

                    if (retVal.Tables.Count > 0)
                    {
                        result = retVal.Tables[0];
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
