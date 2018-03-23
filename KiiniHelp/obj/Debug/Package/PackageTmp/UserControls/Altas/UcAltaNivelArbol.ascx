<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaNivelArbol.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaNivelArbol" %>


<%@ Register Src="~/UserControls/Altas/UcAltaSla.ascx" TagPrefix="uc" TagName="UcSla" %>
<%@ Register Src="~/UserControls/Seleccion/AsociarGrupoUsuario.ascx" TagPrefix="uc" TagName="AsociarGrupoUsuario" %>
<%@ Register Src="~/UserControls/Altas/AltaTiempoEstimado.ascx" TagPrefix="uc" TagName="AltaTiempoEstimado" %>
<%@ Register Src="~/UserControls/Seleccion/UcImpactoUrgencia.ascx" TagPrefix="uc" TagName="UcImpactoUrgencia" %>
<%@ Register Src="~/UserControls/Altas/UcAltaInformacionConsulta.ascx" TagPrefix="uc" TagName="UcAltaInformacionConsulta" %>
<%@ Register Src="~/UserControls/Altas/Formularios/UcAltaFormulario.ascx" TagPrefix="uc" TagName="UcAltaFormulario" %>
<%@ Register Src="~/UserControls/Altas/Encuestas/UcAltaEncuesta.ascx" TagPrefix="uc" TagName="UcAltaEncuesta" %>





<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-header" id="panelAlertaNivel" runat="server" visible="false">
                    <div class="alert alert-danger">
                        <div>
                            <div class="float-left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div class="float-left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix" />
                        </div>
                        <asp:Repeater runat="server" ID="rptErrorNivel">
                            <ItemTemplate>
                                <div class="row">
                                    <ul>
                                        <li><%# Container.DataItem %></li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="modal-title">
                            <asp:Label runat="server" ID="lblTitleCatalogo"></asp:Label>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:HiddenField runat="server" ID="hfCatalogo" />
                        <asp:HiddenField runat="server" ID="hfIdTipoArbol" />
                        <asp:HiddenField runat="server" ID="hfIdArea" />
                        <asp:HiddenField runat="server" ID="hfNivel1" />
                        <asp:HiddenField runat="server" ID="hfNivel2" />
                        <asp:HiddenField runat="server" ID="hfNivel3" />
                        <asp:HiddenField runat="server" ID="hfNivel4" />
                        <asp:HiddenField runat="server" ID="hfNivel5" />
                        <asp:HiddenField runat="server" ID="hfNivel6" />
                        <asp:HiddenField runat="server" ID="hfNivel7" />
                        <div class="form-horizontal">
                            <div class="form-group" runat="server" visible="False">
                                <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="ddlTipoUsuarioNivel" CssClass="DropSelect" Width="100%" Enabled="False" />
                                </div>
                            </div>
                            <div class="form-group" runat="server">
                                <label class="col-sm-3 control-label">Tipo de Nivel</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="ddlTipoNivel" CssClass="DropSelect" Width="100%" OnSelectedIndexChanged="ddlTipoNivel_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Descripción</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtDescripcionNivel" placeholder="DESCRIPCION" class="form-control" onkeydown="return (event.keyCode!=13);" MaxLength="50"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:CheckBox runat="server" ID="chkNivelHabilitado" Text="Habilitado" Checked="True" Visible="False" />
                                <asp:CheckBox runat="server" ID="chkNivelTerminal" CssClass="col-sm-3" Text="Es Nodo terminal" Checked="False" Visible="False" AutoPostBack="True" OnCheckedChanged="chkNivelTerminal_OnCheckedChanged" />
                            </div>
                        </div>
                        <br />
                        <div>
                            <div class="panel panel-primary center-content-div" runat="server" id="divDatos" visible="False">
                                <div class="panel-body">
                                    <asp:Button class="btn btn-primary " runat="server" Text="Consulta" ID="btnModalConsultas" OnClick="btnModalConsultas_OnClick" />
                                    <asp:Button class="btn btn-primary " runat="server" Text="Ticket" ID="btnModalTicket" OnClick="btnModalTicket_OnClick" />
                                    <asp:Button class="btn btn-primary disabled" runat="server" Text="Grupos" ID="btnModalGrupos" OnClick="btnModalGrupos_OnClick" />
                                    <asp:Button class="btn btn-primary disabled" runat="server" Text="SLA" ID="btnModalSla" OnClick="btnModalSla_OnClick" />
                                    <asp:Button class="btn btn-primary disabled" runat="server" Text="Impacto/Urgencia" ID="btnModalImpactoUrgencia" OnClick="btnModalImpactoUrgencia_OnClick" />
                                    <asp:Button class="btn btn-primary disabled" runat="server" Text="Notificaciones" ID="btnModalInforme" OnClick="btnModalInforme_OnClick" />
                                    <asp:Button class="btn btn-primary disabled" runat="server" Text="Encuesta" ID="btnModalEncuesta" OnClick="btnModalEncuesta_OnClick" />

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer text-center">
                        <asp:Button ID="btnGuardarNivel" runat="server" CssClass="btn btn-lg btn-success" Text="Guardar" OnClick="btnGuardarNivel_OnClick" />
                        <asp:Button ID="btnLimpiarNivel" runat="server" CssClass="btn btn-lg btn-danger" Text="Limpiar" OnClick="btnLimpiarNivel_OnClick" />
                        <asp:Button ID="btnCancelarNivel" runat="server" CssClass="btn btn-lg btn-danger" Text="Cancelar" OnClick="btnCancelarNivel_OnClick" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>


<div class="modal fade" id="modalConsultas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upInformacionconsulta" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header" id="panelAlertaInfoConsulta" runat="server" visible="false">
                        <div class="alert alert-danger">
                            <div>
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div class="float-left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <asp:Repeater runat="server" ID="rptErrorInfoConsulta">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="col-sm-3 control-label">
                                Tipo Información
                            </div>
                            <div class="col-sm-3 control-label">
                                Información
                            </div>
                            <div class="clear-fix clearfix"></div>
                        </div>
                        <div class="panel-body">

                            <asp:Repeater runat="server" ID="rptInformacion" OnItemDataBound="rptInformacion_OnItemDataBound">
                                <ItemTemplate>
                                    <div class="row ">
                                        <asp:Label runat="server" ID="lblIndex" Text='<%# Container.ItemIndex %>' Visible="False"></asp:Label>
                                        <asp:Label runat="server" Text='<%# Eval("TipoInfConsulta.Id") %>' Visible="False" ID="lblIdTipoInformacion"></asp:Label>
                                        <div class="col-sm-3 control-label">
                                            <asp:CheckBox runat="server" Text='<%# Eval("TipoInfConsulta.Descripcion") %>' Checked="False" ID="chkInfoConsulta" OnCheckedChanged="chkInfoConsulta_OnCheckedChanged" AutoPostBack="True" />
                                        </div>
                                        <div runat="server" class="col-sm-9 control-label margen-arriba">
                                            <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsBaseDatos")) %>'>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlPropietario" CssClass="DropSelect" Width="100%" Enabled="False" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Button runat="server" Text="Agregar" ID="btnAgregarPropietario" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaInfCons" Enabled="False" />
                                                </div>
                                            </div>
                                            <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsDirectorio")) %>'>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlDocumento" CssClass="DropSelect" Width="100%" Enabled="False" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Button runat="server" Text="Agregar" ID="btnAgregarDocumento" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaInfCons" Enabled="False" />
                                                </div>
                                            </div>
                                            <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsUrl")) %>'>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlUrl" CssClass="DropSelect" Width="100%" Enabled="False" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Button runat="server" Text="Agregar" ID="btnAgregarUrl" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaInfCons" Enabled="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrarConsultas" Text="Cerrar" OnClick="btnCerrarConsultas_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


<%--TICKET--%>
<div class="modal fade" id="modalTicket" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upTicket" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" id="panelAlertaTicket" runat="server" visible="false">
                        <div class="alert alert-danger">
                            <div>
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div class="float-left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <asp:Repeater runat="server" ID="rptErrorTicket">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-primary" runat="server" id="div1" visible="True">
                            <div class="panel-heading">
                                Formulario de Cliente
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="form-inline widht100">
                                            <label class="col-sm-3 control-label widht180">Formulario de Cliente</label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList runat="server" ID="ddlMascaraAcceso" class="form-control widht100"/>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaMascara" data-keyboard="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrarTicket" Text="Cerrar" OnClick="btnCerrarTicket_OnClick" />
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--GRUPOS--%>
<div class="modal fade" id="modalGruposNodo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upGrupos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:asociargrupousuario runat="server" id="AsociarGrupoUsuario" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btnCerraGrupos" Text="Cerrar" OnClick="btnCerraGrupos_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--SLA--%>
<div class="modal fade" id="modalSla" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:ucsla runat="server" id="UcSla" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--IMPACTO URGENCIA--%>
<div class="modal fade" id="modalImpacto" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:ucimpactourgencia runat="server" id="UcImpactoUrgencia" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--TIEMPO INFORME--%>
<div class="modal fade" id="modalTiempoInforme" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:altatiempoestimado runat="server" id="ucAltaTiempoEstimado" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--ENCUESTA--%>
<div class="modal fade" id="modalEncuesta" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header" id="Div2" runat="server" visible="false">
                        <div class="alert alert-danger">
                            <div>
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div class="float-left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <asp:Repeater runat="server" ID="Repeater1">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-primary" runat="server" id="div3" visible="True">
                            <div class="panel-heading">
                                Encuesta
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="form-inline widht100">
                                            <label class="col-sm-3 control-label widht180">Encuesta</label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList runat="server" ID="ddlEncuesta" class="form-control widht100"/>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaEncuesta" data-keyboard="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrarEncuesta" Text="Cerrar" OnClick="btnCerrarEncuesta_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%-- ALTAS --%>
<div class="modal fade" id="modalAltaInfCons" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upModalAltaInfCons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:UcAltaInformacionConsulta runat="server" id="ucAltaInformacionConsulta" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalAltaMascara" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" style="width: 1300px; height: 500px">
                <div class="modal-content heigth100">
                    <div class="modal-body">
                        <uc:UcAltaFormulario runat="server" id="ucAltaFormulario" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalAltaEncuesta" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <uc:UcAltaEncuesta runat="server" id="ucAltaEncuesta" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
