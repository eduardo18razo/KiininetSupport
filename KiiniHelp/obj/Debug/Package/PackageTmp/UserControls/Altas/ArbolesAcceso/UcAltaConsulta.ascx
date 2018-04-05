<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcAltaConsulta" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row" runat="server" id="divData">
            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                <!--Step 1-->
                <div class="margin-top" runat="server" id="divStep1">
                    <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel1" visible="False"></i>
                    <br />
                    <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnPaso1" CommandArgument="1" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step1.png" Width="20" Height="20" alt="" runat="server" />&nbsp;CONFIGURACIÓN
                    </asp:LinkButton>
                </div>

                <!--Step 2-->
                <div class="margin-top" runat="server" id="divStep2" visible="False">
                    <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel2" visible="False"></i>
                    <br />
                    <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnStatusNivel2" CommandArgument="2" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step2.png" Width="20" Height="20" alt="" runat="server" />&nbsp;NIVEL DEL MENÚ
                    </asp:LinkButton>
                </div>

                <!--Step 3-->
                <div class="margin-top" runat="server" id="divStep3" visible="False">
                    <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel3" visible="False"></i>
                    <br />
                    <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnStatusNivel3" CommandArgument="3">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;GRUPOS
                    </asp:LinkButton>
                </div>
            </div>
            <!--/CONTAINER IZQUIERDA-->

            <!--CONTAINER DERECHA-->
            <!--Filtro 1 ORGANIZACIÓN-->
            <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                <div class="module no-border-bottom">
                    <div class="module-inner padding-10-left no-padding-right no-padding-top">
                        <div runat="server" id="divStep1Data">
                            <div class="form-group">
                                Escribe el título de la opción<br />
                                <asp:TextBox runat="server" ID="txtDescripcionNivel" MaxLength="50" onkeydown="return (event.keyCode!=13);" class="form-control" />
                            </div>
                            <div class="form-group">
                                Escribe una descripción de la opción<br />
                                <asp:TextBox runat="server" ID="txtDescripcionOpcion" MaxLength="100" onkeydown="return (event.keyCode!=13);" class="form-control" />
                            </div>
                            <div class="form-group">
                                Quién ve el contenido<br />
                                <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" OnSelectedIndexChanged="ddlTipoUsuarioNivel_OnSelectedIndexChanged" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 no-padding-top">Público</label>
                                <asp:CheckBox runat="server" ID="chkPublico" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" />
                            </div>
                            <hr />
                            <div class="form-group">
                                Artículo<br />
                                <asp:DropDownList runat="server" ID="ddlConsultas" CssClass="form-control" />
                            </div>

                            <div class="form-group">
                                <label class="col-lg-4 no-padding-top">Habilitar evaluación</label>
                                <asp:CheckBox runat="server" ID="chkEvaluacion" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 no-padding-top">Activo</label>
                                <asp:CheckBox runat="server" ID="chkNivelHabilitado" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" />
                            </div>
                        </div>
                        <div runat="server" id="divStep2Data" visible="False">
                            <div class="form-group">
                                Mostrar en categoría<br />
                                <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva categoría</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionArea" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnGuardarArea" OnClick="btnGuardarArea_OnClick" class="fa fa-plus-circle" />

                            </div>
                            <hr />
                            <div class="form-group">
                                Mostrar en sección
                            </div>

                            <div class="form-group">
                                Nivel 1<br />
                                <asp:DropDownList runat="server" ID="ddlNivel1" CssClass="form-control" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN1" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel1" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" />
                            </div>
                            <div class="form-group" runat="server" id="divNivel2" visible="False">
                                Nivel 2<br />
                                <asp:DropDownList runat="server" ID="ddlNivel2" CssClass="form-control" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN2" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel2" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />

                            </div>
                            <div class="form-group" runat="server" id="divNivel3" visible="False">
                                Nivel 3<br />
                                <asp:DropDownList runat="server" ID="ddlNivel3" CssClass="form-control" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN3" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel3" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />

                            </div>
                            <div class="form-group" runat="server" id="divNivel4" visible="False">
                                Nivel 4<br />
                                <asp:DropDownList runat="server" ID="ddlNivel4" CssClass="form-control" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN4" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel4" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />
                            </div>
                            <div class="form-group" runat="server" id="divNivel5" visible="False">
                                Nivel 5<br />
                                <asp:DropDownList runat="server" ID="ddlNivel5" CssClass="form-control" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN5" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel5" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                            </div>
                            <div class="form-group" runat="server" id="divNivel6" visible="False">
                                Nivel 6<br />
                                <asp:DropDownList runat="server" ID="ddlNivel6" CssClass="form-control" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <label class="margin-top-5">Crear nueva sección</label> 
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN6" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel6" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="6" Enabled="False" />
                            </div>
                            <%--<div class="form-group" runat="server" id="divNivel7" visible="False">
                                Nivel 7<br />
                                <asp:DropDownList runat="server" ID="ddlNivel7" CssClass="form-control" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:TextBox CssClass="form-control" ID="txtDescripcionN7" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                <asp:LinkButton runat="server" ID="btnAgregarNivel7" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="7" Enabled="False" />
                            </div>--%>
                        </div>
                        <div runat="server" id="divStep3Data" visible="False">
                            <div class="form-group">
                                Cuáles grupos ven la opción
                            </div>
                            <div class="form-group">
                                Usuarios<br />
                                <asp:DropDownList runat="server" ID="ddlGrupoAcceso" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                Responsable de Categoría<br />
                                <asp:DropDownList runat="server" ID="ddlDuenoServicio" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                Responsable de Contenido<br />
                                <asp:DropDownList runat="server" ID="ddlGrupoResponsableMantenimiento" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                Consultas Especiales<br />
                                <asp:ListBox ID="lstGrupoEspecialConsulta" runat="server" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                        </div>

                    </div>
                    <!--BTN-TERMINAR-->
                    <p class="text-right margin-top-10">
                        <asp:Button runat="server" ID="btnPreview" Text="Preview" CssClass="btn btn-default" Visible="False" OnClick="btnPreview_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Siguiente" ID="btnSiguiente" OnClick="btnSiguiente_OnClick" CommandArgument="1" />
                        <asp:Button runat="server" ID="btnSaveAll" Text="Guardar" CssClass="btn btn-primary" Visible="False" OnClick="btnSaveAll_OnClick" />
                    </p>

                    <!--/BTN-TERMINAR-->
                </div>
                <!--DIV que cierra el bg-grey -->
            </div>
        </div>
        <!--/Filtro 1 ORGANIZACIÓN-->
        <!--/CONTAINER DERECHA-->
        <script type="text/javascript">
            $(function () {
                $('[id*=lstGrupoEspecialConsulta]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstGrupoEspecialConsulta]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
