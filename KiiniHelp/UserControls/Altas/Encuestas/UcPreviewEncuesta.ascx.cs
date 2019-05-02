using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Encuestas
{
    public partial class UcPreviewEncuesta : UserControl
    {
        private List<Control> _lstControles;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Control myControl = GetPostBackControl(Page);

            if ((myControl != null))
            {
                if ((myControl.ClientID == "btnAddTextBox"))
                {

                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _lstControles = new List<Control>();
            Encuesta encuesta = (Encuesta)Session["PreviewEncuesta"];

            if (encuesta != null)
            {
                lbltitulo.Text = string.Format("{0} {1}", encuesta.TituloCliente, encuesta.Tipificacion);
                lblDescripcionCliente.Text = encuesta.Descripcion;
                PintaControles(encuesta.EncuestaPregunta, encuesta.IdTipoEncuesta);
                Session["EncuestaActiva"] = encuesta;
            }
        }

        public static Control GetPostBackControl(Page thePage)
        {
            Control myControl = null;
            string ctrlName = thePage.Request.Params.Get("__EVENTTARGET");
            if (((ctrlName != null) & (ctrlName != string.Empty)))
            {
                myControl = thePage.FindControl(ctrlName);
            }
            else
            {
                foreach (string item in thePage.Request.Form)
                {
                    Control c = thePage.FindControl(item);
                    if (((c) is Button))
                    {
                        myControl = c;
                    }
                }

            }
            return myControl;
        }

        public void PintaControles(List<EncuestaPregunta> lstControles, int tipoEncuesta)
        {
            try
            {
                int contador = 0;
                switch (tipoEncuesta)
                {
                    case (int)BusinessVariables.EnumTipoEncuesta.PromotorScore:
                        foreach (EncuestaPregunta pregunta in lstControles)
                        {
                            HtmlGenericControl createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";
                            Label lbl = new Label { Text = string.Format("{0}", pregunta.Pregunta.Trim()), CssClass = "control-label" };
                            createDiv.Controls.Add(lbl);
                            divControles.Controls.Add(createDiv);
                            createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";
                            for (int i = 0; i <= 10; i++)
                            {

                                RadioButton rb = new RadioButton();
                                rb.Text = i.ToString();
                                rb.Style.Add("padding", "10px");
                                createDiv.Controls.Add(rb);
                            }
                            divControles.Controls.Add(createDiv);
                        }
                        break;

                    case (int)BusinessVariables.EnumTipoEncuesta.SiNo:
                        foreach (EncuestaPregunta pregunta in lstControles)
                        {
                            HtmlGenericControl createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";
                            Label lbl = new Label { Text = string.Format("{0}", pregunta.Pregunta.Trim()), CssClass = "control-label" };
                            createDiv.Controls.Add(lbl);
                            divControles.Controls.Add(createDiv);
                            createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";

                            RadioButton rbNo = new RadioButton();
                            rbNo.Text = "No";
                            rbNo.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbNo);
                            RadioButton rbSi = new RadioButton();
                            rbSi.Text = "SI";
                            rbSi.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbSi);

                            divControles.Controls.Add(createDiv);
                            contador++;
                        }
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.Calificacion:
                        foreach (EncuestaPregunta pregunta in lstControles)
                        {
                            HtmlGenericControl createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";
                            Label lbl = new Label { Text = string.Format("{0}", pregunta.Pregunta.Trim()), CssClass = "control-label" };
                            createDiv.Controls.Add(lbl);
                            divControles.Controls.Add(createDiv);
                            createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";
                            for (int i = 0; i <= 10; i++)
                            {

                                RadioButton rb = new RadioButton();
                                rb.Text = i.ToString();
                                rb.Style.Add("padding", "10px");
                                createDiv.Controls.Add(rb);
                            }
                            divControles.Controls.Add(createDiv);
                        }
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente:
                        foreach (EncuestaPregunta pregunta in lstControles)
                        {
                            HtmlGenericControl createDiv = new HtmlGenericControl("DIV") ;
                            createDiv.Attributes["class"] = "form-group";
                            Label lbl = new Label { Text = string.Format("{0}", pregunta.Pregunta.Trim()), CssClass = "control-label" };
                            createDiv.Controls.Add(lbl);
                            divControles.Controls.Add(createDiv);
                            createDiv = new HtmlGenericControl("DIV");
                            createDiv.Attributes["class"] = "form-group";

                            RadioButton rbtnMala = new RadioButton();
                            rbtnMala.Text = "PESIMO";
                            rbtnMala.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbtnMala);

                            RadioButton rbtnRegular = new RadioButton();
                            rbtnRegular.Text = "MALO";
                            rbtnRegular.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbtnRegular);

                            RadioButton rbtnBuena = new RadioButton();
                            rbtnBuena.Text = "REGULAR";
                            rbtnBuena.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbtnBuena);

                            RadioButton rbtnMuyBuena = new RadioButton();
                            rbtnMuyBuena.Text = "BUENO";
                            rbtnMuyBuena.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbtnMuyBuena);

                            RadioButton rbtnExelente = new RadioButton();
                            rbtnExelente.Text = "EXCELENTE";
                            rbtnExelente.Style.Add("padding", "10px");
                            createDiv.Controls.Add(rbtnExelente);
                            divControles.Controls.Add(createDiv);
                        }
                        break;
                }

                upEncuestas.Update();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


            }
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}