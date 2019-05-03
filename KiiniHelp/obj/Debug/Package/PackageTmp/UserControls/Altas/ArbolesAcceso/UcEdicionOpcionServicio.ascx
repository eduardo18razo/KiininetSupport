<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcEdicionOpcionServicio.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcEdicionOpcionServicio" %>

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
                                            <asp:HyperLink data-toggle="collapse" ID="pnlPrincipal" runat="server" CssClass="no-underline" NavigateUrl="#faq_Configuracion">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">
                                                            <asp:Label runat="server" ID="lblTitle" Text="Servicios y Problemas" /></h3>
                                                    </div>
                                                </div>
                                            </asp:HyperLink>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq_Configuracion" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                                            Quien ve el contenido<br />
                                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                                        </div>
                                                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                                            Escribe el título de la opción<br />
                                                            <asp:TextBox runat="server" ID="txtTitulo" class="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" onkeyup="edValueKeyPress()" />
                                                        </div>
                                                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                                            Escribe una descripción de la opción<br />
                                                            <asp:TextBox runat="server" ID="txtDescripcionOpcion" class="form-control" MaxLength="100" onkeydown="return (event.keyCode!=13);" />
                                                        </div>

                                                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                                            Tipificación<br />
                                                            <asp:DropDownList runat="server" ID="ddlTipificacion" CssClass="form-control" />
                                                        </div>
                                                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                                            Formulario<br />
                                                            <asp:DropDownList runat="server" ID="ddlFormularios" CssClass="form-control" />
                                                        </div>

                                                        <div class="form-group col-lg-4 col-md-4 col-sm-3 col-xs-6">
                                                            Público<br />
                                                            <asp:CheckBox runat="server" ID="chkPublico" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" />
                                                        </div>
                                                        <div class="form-group col-lg-4 col-md-4 col-sm-3 col-xs-6">
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

                            <!--Categoria y secciones -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq_NivelMenu">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">Categoria y secciones</h3>
                                                    </div>
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <asp:Panel runat="server" CssClass="panel-collapse  collapse" ID="faq_NivelMenu" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left" runat="server">
                                                            <div class="row">
                                                                <div class="form-group form-inline col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    Mostrar en categoría<br />
                                                                    <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-control" OnSelectedIndexChanged="ddlCategoria_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionCategoria" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnGuardarCategoria" OnClick="btnGuardarCategoria_OnClick" class="fa fa-plus-circle" />

                                                                </div>
                                                                <hr />
                                                                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    Mostrar en sección
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12">
                                                                    Nivel 1<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel1" CssClass="form-control" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN1" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel1" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" />
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel2" visible="False">
                                                                    Nivel 2<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel2" CssClass="form-control" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>

                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN2" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel2" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />

                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel3" visible="False">
                                                                    Nivel 3<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel3" CssClass="form-control" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN3" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel3" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />

                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel4" visible="False">
                                                                    Nivel 4<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel4" CssClass="form-control" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN4" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel4" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />

                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel5" visible="False">
                                                                    Nivel 5<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel5" CssClass="form-control" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" />
                                                                    <label class="margin-top-5">Crear nueva sección</label>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcionN5" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                                                    <asp:LinkButton runat="server" ID="btnAgregarNivel5" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                                                                </div>
                                                                <div class="form-group col-lg-2 col-md-2 col-sm-4 col-xs-12" runat="server" id="divNivel6" visible="False">
                                                                    Nivel 6<br />
                                                                    <asp:DropDownList runat="server" ID="ddlNivel6" CssClass="form-control" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" />
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

                            <!--Grupos -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="no-underline" href="#faq_Grupos">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-sm-12">
                                                        <h3 class="TitulosAzul">Grupos de Acceso y Atención</h3>
                                                    </div>
                                                </a>
                                            </div>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq_Grupos" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <%--<div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                        Cuáles grupos ven la opción
                                                        Quien tiene Acceso a Centro de Soporte
                                                    </div>--%>
                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                        Cuáles grupos ven la opción<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoAccesoCentroSoporte" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                        Responsable de Atención<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableAtencion" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2" runat="server" id="divGpoResponsableContenido" visible="False">
                                                        Responsable de Contenido<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableContenido" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2" runat="server" id="divGpoResponsableOperacion" visible="False">
                                                        Responsable de Operación<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableOperacion" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2" runat="server" id="divGpoResponsableDesarrollo" visible="False">
                                                        Responsable de Desarrollo<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableDesarrollo" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2" runat="server" id="divGpoResponsableCategoria" visible="False">
                                                        Responsable de Categoría<br />
                                                        <asp:DropDownList runat="server" ID="ddlGrupoResponsableCategoria" CssClass="form-control" />
                                                    </div>

                                                    <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-2" runat="server" id="divGpoAgenteUniversal" visible="False">
                                                        Agente Universal<br />
                                                        <asp:ListBox ID="lstGruposAgenteUniversal" SelectionMode="Multiple" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                            <!--Sla-->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="no-underline" href="#faq_Sla">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">SLA</h3>
                                                    </div>
                                                </a>
                                            </div>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq_Sla" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="row">
                                                        <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            SLA
                                                                <asp:CheckBox runat="server" ID="chkSla" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" OnCheckedChanged="chkSla_OnCheckedChanged" AutoPostBack="True" />
                                                            <br />
                                                            Establece un tiempo de respuesta para este evento. 
                                                        </div>
                                                        <hr />
                                                        <div class="form-group">
                                                            <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12" runat="server" id="divTiempoSla">
                                                                Tiempo total<br />
                                                                <asp:TextBox runat="server" ID="txtTiempoTotal" CssClass="form-control" MaxLength="3" onkeypress="return ValidaCampo(this, 2)" />
                                                            </div>
                                                            <div class="col-lg-2 col-md-2 col-sm-3 col-xs-12" runat="server" id="divUnidadTiempoSla">
                                                                Unidad de tiempo<br />
                                                                <asp:DropDownList runat="server" ID="ddlTiempoTotal" CssClass="form-control" />
                                                            </div>
                                                            <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-12">
                                                                Gravedad<br />
                                                                <asp:DropDownList runat="server" ID="ddlPrioridad" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlImpacto_OnSelectedIndexChanged" />
                                                            </div>
                                                            <div class="form-group col-lg-2 col-md-2 col-sm-2 col-xs-12 ">
                                                                Atención<br />
                                                                <asp:DropDownList runat="server" ID="ddlUrgencia" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlUrgencia_OnSelectedIndexChanged" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group center-content-div col-lg-4 col-md-4 col-sm-3 col-xs-12" runat="server" id="divImpacto" visible="False">
                                                            Prioridad<br />
                                                            <br />
                                                            <strong>
                                                                <asp:Label runat="server" ID="lblImpacto" /></strong>
                                                        </div>
                                                        <div class="form-group" runat="server" visible="False">
                                                            <label class="col-lg-10 col-md-6 col-sm-12 col-xs-12">Detallado</label>
                                                            <asp:CheckBox runat="server" Text="Detallado" ID="chkEstimado" AutoPostBack="True" CssClass="chkIphone" Width="30px" OnCheckedChanged="chkEstimado_OnCheckedChanged" />
                                                        </div>
                                                        <div runat="server" id="divDetalle" class="form-group" visible="False">
                                                            <asp:Repeater runat="server" ID="rptSubRoles" OnItemDataBound="rptSubRoles_OnItemDataBound">
                                                                <HeaderTemplate>
                                                                    <table class="table table-responsive" id="tblHeader">
                                                                        <thead>
                                                                            <tr align="center">
                                                                                <td>
                                                                                    <asp:Label runat="server">Sub Rol</asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label runat="server">Tiempo</asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label runat="server">Unidad</asp:Label></td>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr align="center" id='<%# Eval("IdSubRol")%>' class="text-center">
                                                                        <td class="no-padding ocultar">
                                                                            <asp:Label runat="server" Text='<%# Eval("IdSubRol") %>' ID="lblIdSubRol" /></td>
                                                                        <td>
                                                                            <asp:Label runat="server" Text='<%# Eval("SubRol.Descripcion") %>' /></td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" ID="txtDias" Enabled="False" /></td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlDuracionRepeater" CssClass="form-control" Enabled="False" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </tbody>
                               <tfoot>
                               </tfoot>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />

                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding margin-top-20" />
                                                        <div runat="server" id="divNotificaciones" class="col-lg-12">
                                                            <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                Notificaciones
                                                            <br />
                                                                Establece el tiempo en el que se recibirán notificaciones sobre este evento.
                                                            </div>
                                                            <hr />
                                                            <div class="form-group">
                                                                <asp:CheckBox runat="server" ID="chkNotificacion" AutoPostBack="True" OnCheckedChanged="chkNotificacion_OnCheckedChanged" CssClass="col-lg-1 col-md-2 col-sm-2 col-xs-6" Text="Notificar" />
                                                                <asp:CheckBox runat="server" ID="chkVencimientoNotificacion" Checked="True" Text="Despues de vencimiento" Enabled="False" CssClass="col-lg-11 col-md-10 col-sm-10 col-xs-6" />
                                                                <div runat="server" id="divNotificacion" visible="False">
                                                                    <div class="form-group col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                        Grupo a Notificar<br />
                                                                        <asp:DropDownList runat="server" ID="ddlGrupoNotificaciones" CssClass="form-control" />
                                                                    </div>

                                                                    <div class="col-sm-4 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                        Tiempo total
                                                                    <br />
                                                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtTiempoNotificacion" />
                                                                    </div>
                                                                    <div class="col-sm-4 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                        Unidad de tiempo
                                                <br />
                                                                        <asp:DropDownList runat="server" ID="ddlNotificacionUnidadTiempo" CssClass="form-control" Enabled="False" />
                                                                    </div>
                                                                    <div class="col-sm-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                                                        Medio<br />
                                                                        <asp:DataList runat="server" ID="rptCanal" RepeatDirection="Horizontal">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblIdCanal" Text='<%# Eval("Id") %>' Visible="False"></asp:Label>
                                                                                <asp:Label runat="server" ID="lblDescripcionCanal" Text='<%# Eval("Descripcion") %>' CssClass="margin-left-5"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:DataList>
                                                                        <%--<asp:DropDownList runat="server" ID="ddlCanalNotificacion" CssClass="form-control" Enabled="False" />--%>
                                                                    </div>

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

                            <!--Encuestas -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="no-underline" href="#faq_Encuestas">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <h3 class="TitulosAzul">Encuesta</h3>
                                                    </div>
                                                </a>
                                            </div>
                                        </h4>
                                    </div>


                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq_Encuestas" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        Activar encuesta
                                                        <asp:CheckBox runat="server" ID="chkEncuestaActiva" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" OnCheckedChanged="chkEncuestaActiva_OnCheckedChanged" AutoPostBack="True" />

                                                    </div>
                                                    <div class="form-group col-lg-1 col-md-2 col-sm-3 col-xs-12">
                                                        <asp:DropDownList runat="server" ID="ddlEncuesta" CssClass="form-control widht100" Enabled="False" />
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
                            <asp:Button runat="server" Text="Cancelar" ID="btnCancelar" CssClass="btn btn-default" OnClick="btnCancelar_OnClick" />
                            <asp:Button runat="server" ID="btnPreview" Text="Previsualizar" CssClass="btn btn-default" OnClick="btnPreview_OnClick" />
                            <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_OnClick" />
                        </div>
                    </div>
                    <br />
                </section>
                <script type="text/javascript">
                    $(function () {
                        //$('[id*=lstGrupoNotificaciones]').multiselect({
                        //    includeSelectAllOption: true
                        //});
                        $('[id*=lstGruposAccesoAnaliticos]').multiselect({
                            includeSelectAllOption: true
                        });
                        $('[id*=lstGruposAgenteUniversal]').multiselect({
                            includeSelectAllOption: true
                        });
                    });
                    var prm = Sys.WebForms.PageRequestManager.getInstance();

                    prm.add_endRequest(function () {
                        //$('[id*=lstGrupoNotificaciones]').multiselect({
                        //    includeSelectAllOption: true
                        //});
                        $('[id*=lstGruposAccesoAnaliticos]').multiselect({
                            includeSelectAllOption: true
                        });
                        $('[id*=lstGruposAgenteUniversal]').multiselect({
                            includeSelectAllOption: true
                        });
                    });

                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
