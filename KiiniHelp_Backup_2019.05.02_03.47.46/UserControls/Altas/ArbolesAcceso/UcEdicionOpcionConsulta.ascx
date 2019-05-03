<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcEdicionOpcionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcEdicionOpcionConsulta" %>
<script>
    function edValueKeyPress() {
        var edValue = document.getElementById('<%= txtTitulo.ClientID %>');
        var s = edValue.value;

        var lblValue = document.getElementById('<%= txtDescripcionOpcion.ClientID %>');
        lblValue.value = s;
    }
</script>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upGeneral" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfIdUsuario" />
                <asp:HiddenField runat="server" ID="hfEsMoral" />
                <asp:HiddenField runat="server" ID="hdEsDetalle" />
                <asp:HiddenField runat="server" ID="hfHabilitado" />
                <asp:HiddenField runat="server" ID="hfEditaDetalle" />
                <asp:HiddenField runat="server" ID="hfGeneraUsuario" Value="true" />
                <asp:HiddenField runat="server" ID="hfConsultas" Value="false" />
                <!--MIGAS DE PAN-->
                <div class="row">
                    <div>
                        <ol class="breadcrumb">
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                            <li>Centro de Soporte</li>
                            <li class=" active">
                                <asp:Label runat="server" ID="lblMovimiento"></asp:Label></li>
                        </ol>
                    </div>
                </div>
                <!--/MIGAS DE PAN-->

                <!--MÓDULO FORMULARIO-->
                <section class="module">
                    <div class="module-inner">
                        <div class="row">
                            <!--Configuración-->
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <asp:HyperLink data-toggle="collapse" ID="pnlPrincipal" runat="server" CssClass="no-underline" NavigateUrl="#faq5_1">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">
                                                            <asp:Label runat="server" ID="lblTitle" Text="Datos de la consulta" /></h3>
                                                    </div>
                                                </div>
                                            </asp:HyperLink>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq5_1" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="form-group col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                                            Quién ve el contenido<br />
                                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                                        </div>

                                                        <div class="form-group col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                                            Escribe el título de la opción<br />
                                                            <asp:TextBox runat="server" ID="txtTitulo" MaxLength="50" onkeydown="return (event.keyCode!=13);" class="form-control" onkeyup="edValueKeyPress()" />
                                                        </div>
                                                        <div class="form-group col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                                            Escribe una descripción de la opción<br />
                                                            <asp:TextBox runat="server" ID="txtDescripcionOpcion" MaxLength="100" onkeydown="return (event.keyCode!=13);" class="form-control" />
                                                        </div>

                                                        <div class="form-group col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                                            Artículo asociado<br />
                                                            <asp:DropDownList runat="server" ID="ddlArticulo" CssClass="form-control" />
                                                        </div>
                                                        <div class="form-group col-lg-2 col-md-3 col-sm-4 col-xs-6">
                                                            Público<br />
                                                            <asp:CheckBox runat="server" ID="chkPublico" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" />
                                                        </div>
                                                        <div class="form-group col-lg-2 col-md-3 col-sm-4 col-xs-6">
                                                            Habilitar evaluación<br />
                                                            <asp:CheckBox runat="server" ID="chkEvaluacion" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" />
                                                        </div>
                                                        <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12">
                                                            Activo<br />
                                                            <asp:CheckBox runat="server" ID="chkNivelHabilitado" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                            <!--Nivel del menu -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_2">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-sm-12">
                                                        <h3 class="TitulosAzul">Categoria y secciones</h3>
                                                    </div>
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <asp:Panel runat="server" CssClass="panel-collapse  collapse" ID="faq5_2" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left" runat="server">
                                                            <div class="row">
                                                                <div class="form-group form-inline col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    Mostrar en categoría<br />
                                                                    <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-control" OnSelectedIndexChanged="ddlCategoria_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva categoría</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionCategoria" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnGuardarCategoria" OnClick="btnGuardarCategoria_OnClick" class="fa fa-plus-circle" />

                                                                </div>
                                                                <hr />
                                                                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    Mostrar en sección
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12">
                                                                    Nivel 1<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel1" CssClass="form-control" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN1" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel1" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" />
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel2" visible="False">
                                                                    Nivel 2<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel2" CssClass="form-control" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN2" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel2" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />

                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel3" visible="False">
                                                                    Nivel 3<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel3" CssClass="form-control" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN3" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel3" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />

                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel4" visible="False">
                                                                    Nivel 4<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel4" CssClass="form-control" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN4" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel4" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel5" visible="False">
                                                                    Nivel 5<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel5" CssClass="form-control" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN5" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel5" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel6" visible="False">
                                                                    Nivel 6<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel6" CssClass="form-control" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN6" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel6" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="6" Enabled="False" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>

                            <!--GRUPO -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="no-underline" href="#faq5_3">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">Grupos de accesos</h3>
                                                    </div>
                                                </a>
                                            </div>
                                        </h4>
                                    </div>


                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq5_3" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                        Que grupo ve la opción<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoAccesoCentroSoporte" CssClass="form-control" />
                                                    </div>
                                                    <%--<div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                        Acceso a Centro de Soporte<br />
                                                        <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control" />
                                                    </div>--%>
                                                    <div class="form-group" runat="server" id="divGpoResponsableCategoria" visible="False">
                                                        Responsable de Categoría<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableCategoria" CssClass="form-control" />
                                                    </div>
                                                    <div class="form-group" runat="server" id="divGpoResponsableContenido" visible="False">
                                                        Responsable de Contenido<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableContenido" CssClass="form-control" />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="module" id="divBtnGuardar" runat="server" visible="true">
                    <div class="module-inner">
                        <div class="row widht100 text-center">
                            <br />
                            <asp:Button runat="server" Text="Cancelar" ID="btnCancelarEdicion" CssClass="btn btn-default" OnClick="btnCancelarEdicion_OnClick" />
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CssClass="btn btn-default" OnClick="btnPreview_OnClick" />
                            <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_OnClick" />
                        </div>
                    </div>
                    <br />
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
