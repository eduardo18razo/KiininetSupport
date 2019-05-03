using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using AjaxControlToolkit;

namespace KiiniHelp.Test
{
    public partial class FrmTest : Page
    {
        private List<Control> _lstControles;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _lstControles = new List<Control>();
                    PintaControles();
            
        }

        private void PintaControles()
        {

            AsyncFileUpload asyncFileUpload1 = new AsyncFileUpload();
            asyncFileUpload1.ID = "idupfile";
            asyncFileUpload1.ClientIDMode = ClientIDMode.Static;

            form1.Controls.Add(asyncFileUpload1);
            _lstControles.Add(asyncFileUpload1);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //LineSeries line = new LineSeries();
            //line.DataFieldX
            //gvTest.DataSource = _servicioArea.ObtenerAreas(false);
            //gvTest.DataBind();
            if (!IsPostBack)
            {
                PintaControles();
            //    ddlUsuarioAsignacion.DataFieldID = "IdUsuario";
            //    ddlUsuarioAsignacion.DataFieldParentID = "IdSubRol";
            //    ddlUsuarioAsignacion.DataValueField = "DescripcionSubRol";
            //    ddlUsuarioAsignacion.DataTextField = "NombreUsuario";
            //    ddlUsuarioAsignacion.DataSource = _servicioUsuario.ObtenerUsuarioAgenteByGrupoUsuario(4, 1, new List<int>());
            //    ddlUsuarioAsignacion.DataBind();
            }

            //DataTable dtLike = new DataTable("dt");
            //dtLike.Columns.Add("Descripcion", typeof(string));
            //dtLike.Columns.Add("Enero", typeof(int));
            //dtLike.Columns.Add("Febrero", typeof(int));
            //dtLike.Columns.Add("Marzo", typeof(int));
            //dtLike.Columns.Add("Abril", typeof(int));
            //dtLike.Columns.Add("Mayo", typeof(int));
            //dtLike.Columns.Add("Junio", typeof(int));
            //dtLike.Columns.Add("Julio", typeof(int));
            //dtLike.Columns.Add("Agosto", typeof(int));
            //dtLike.Columns.Add("Septiembre", typeof(int));
            //dtLike.Columns.Add("Octubre", typeof(int));
            //dtLike.Columns.Add("Noviembre", typeof(int));
            //dtLike.Columns.Add("Diciembre", typeof(int));
            //dtTicketsCanal.Columns.Add("Canal", typeof(string));
            //dtTicketsCanal.Columns.Add("Total", typeof(double));
            //dtLike.Rows.Add("Like", 7, 9, 11, 13, 12, 9, 12, 14, 18, 22, 24, 26);
            //dtLike.Rows.Add("Dont Like", 16, 7, 14, 5, 8, 8, 8, 8, 10, 7, 6, 4);
            //dtTicketsCanal.Rows.Add(3, "Chat", 8);
            //dtTicketsCanal.Rows.Add(6, "Telefono", 7);
            //GeneraGraficaPieTickets(rhcTickets, dtTicketsCanal);

            ////Stacket chart bar Espacio utilizado
            //DataTable dtAlmacenado = new DataTable("dt");
            //dtAlmacenado.Columns.Add("Ocupado", typeof(double));
            //dtAlmacenado.Columns.Add("Libre", typeof(double));
            //dtAlmacenado.Columns.Add("Titulo", typeof(string));
            //dtAlmacenado.Rows.Add(123.5, 900.5, "Almacenado");

            //GeneraGraficaStackedColumn(rhcLike, dtLike);





            //    cGraficoEspacio.Series[0].Points.Add(new DataPoint(0, 200));
            //    cGraficoEspacio.Series[1].Points.Add(new DataPoint(0, 1000));

            //// October Data
            //cGraficoEspacio.Series[1].Points.Add(new DataPoint(0, 20));
            //cGraficoEspacio.Series[1].Points.Add(new DataPoint(1, 16));

            //// April Data
            //cGraficoEspacio.Series[2].Points.Add(new DataPoint(0, 15));
            //cGraficoEspacio.Series[2].Points.Add(new DataPoint(1, 18));

            //foreach (Series cs in cGraficoEspacio.Series)
            //    cs.ChartType = SeriesChartType.StackedBar100;



            //List<Object> dataSource = new List<object>();
            //dataSource.Add(new { Ocupado = 350, Libre = 700, Titulo = "Almacenado" });

            //rcTest.DataSource = dt;
            //rcTest.DataBind();






            //BusinessGraficosDasboard.Pastel.GeneraGraficoBarraApilada(cGraficoEspacio, dt);
            //ucDetalleArbolAcceso.IdArbolAcceso = 7;
        }
        
    }
}