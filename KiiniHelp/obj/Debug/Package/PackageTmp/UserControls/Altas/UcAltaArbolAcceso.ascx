﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaArbolAcceso.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaArbolAcceso" %>




<%@ Register Src="~/UserControls/Seleccion/AsociarGrupoUsuario.ascx" TagPrefix="uc" TagName="AsociarGrupoUsuario" %>
<%@ Register Src="~/UserControls/Altas/AltaArea.ascx" TagPrefix="uc" TagName="AltaArea" %>
<%@ Register Src="~/UserControls/Altas/AltaTiempoEstimado.ascx" TagPrefix="uc" TagName="AltaTiempoEstimado" %>
<%@ Register Src="~/UserControls/Seleccion/UcImpactoUrgencia.ascx" TagPrefix="uc" TagName="UcImpactoUrgencia" %>
<%@ Register Src="~/UserControls/Altas/UcAltaSla.ascx" TagPrefix="uc" TagName="UcAltaSla" %>
<%@ Register Src="~/UserControls/Altas/UcAltaInformacionConsulta.ascx" TagPrefix="uc" TagName="UcAltaInformacionConsulta" %>
<%@ Register Src="~/UserControls/Altas/Formularios/UcAltaFormulario.ascx" TagPrefix="uc" TagName="UcAltaFormulario" %>
<%@ Register Src="~/UserControls/Altas/Encuestas/UcAltaEncuesta.ascx" TagPrefix="uc" TagName="UcAltaEncuesta" %>





<asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbol"/>
        <asp:HiddenField runat="server" ID="hfIdTipoArbol"/>
        <asp:UpdatePanel ID="upNivel" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header" id="panelAlertaNivel" runat="server" visible="false">
                            <div class="alert alert-danger">
                                <div>
                                    <div style="float: left">
                                        <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                    </div>
                                    <div style="float: left">
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
                                <div>
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList runat="server" ID="ddlTipoUsuarioNivel" CssClass="DropSelect" Width="100%" Enabled="False" />

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Descripcion</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtDescripcionNivel" placeholder="DESCRIPCION" class="form-control" onkeydown="return (event.keyCode!=13);" MaxLength="50"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNivelHabilitado" Text="Habilitado" Checked="True" Visible="False" />
                                            <%-- --%>
                                            <asp:CheckBox runat="server" ID="chkNivelTerminal" CssClass="col-sm-3" Text="Es Nodo terminal" Checked="False" Visible="False" AutoPostBack="True" OnCheckedChanged="chkNivelTerminal_OnCheckedChanged"/>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div>
                                    <div class="panel panel-primary center-content-div" runat="server" id="divDatos" visible="False">
                                        <div class="panel-heading">
                                            Información
                                        </div>
                                        <div class="panel-body">
                                            <asp:Button class="btn btn-primary" runat="server" Text="Consulta" ID="btnModalConsultas" OnClick="btnModalConsultas_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="Ticket" ID="btnModalTicket" OnClick="btnModalTicket_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="Grupos" ID="btnModalGrupos" OnClick="btnModalGrupos_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="SLA" ID="btnModalSla" OnClick="btnModalSla_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="Impacto/Urgencia" ID="btnModalImpactoUrgencia" OnClick="btnModalImpactoUrgencia_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="Tiempo Informe" ID="btnModalInforme" OnClick="btnModalInforme_OnClick" />
                                            <asp:Button class="btn btn-primary" runat="server" Text="Encuesta" ID="btnModalEncuesta" OnClick="btnModalEncuesta_OnClick" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnGuardarNivel" runat="server" CssClass="btn btn-lg btn-success" Text="Guardar" OnClick="btnGuardarNivel_OnClick" />
                            <asp:Button ID="btnLimpiarNivel" runat="server" CssClass="btn btn-lg btn-danger" Text="Limpiar" OnClick="btnLimpiarNivel_OnClick" />
                            <asp:Button ID="btnCancelarNivel" runat="server" CssClass="btn btn-lg btn-danger" Text="Cancelar" OnClick="btnCancelarNivel_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--INFORMACION DE CONSULTA--%>
        <div class="modal fade" id="modalConsultas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
            <asp:UpdatePanel ID="upConsultas" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            
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
                                                <div class="col-sm-3 control-label" style="width: 180px">
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
            <asp:UpdatePanel ID="upTocket" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header" id="panelAlertaTicket" runat="server" visible="false">
                                <div class="alert alert-danger">
                                    <div>
                                        <div style="float: left">
                                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                        </div>
                                        <div style="float: left">
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
                            <%--TICKET--%>
                            <div class="panel panel-primary" runat="server" id="div1" visible="True">
                                <div class="panel-heading">
                                    Mascar de Captura
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <%--Formulario de Cliente--%>
                                        <div class="form-group">
                                            <div class="form-inline" style="width: 100%">
                                                <label class="col-sm-3 control-label" style="width: 180px">Formulario de Cliente</label>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlMascaraAcceso" class="form-control" Style="width: 100%" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaMascara" data-keyboard="false" />
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
                            <uc:AsociarGrupoUsuario runat="server" ID="AsociarGrupoUsuario" />
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
                                <uc:UcAltaSla runat="server" id="UcAltaSla" />
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
                                <uc:UcImpactoUrgencia runat="server" ID="UcImpactoUrgencia" />
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
                                <uc:AltaTiempoEstimado runat="server" ID="ucAltaTiempoEstimado" />
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
                                        <div style="float: left">
                                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                        </div>
                                        <div style="float: left">
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
                            <div class="panel panel-primary" runat="server" id="div3" visible="True">
                                <div class="panel-heading">
                                    Encuesta
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <%--ENCUESTA--%>
                                        <div class="form-group">
                                            <div class="form-inline" style="width: 100%">
                                                <label class="col-sm-3 control-label" style="width: 180px">Encuesta</label>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlEncuesta" class="form-control" Style="width: 100%" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" data-toggle="modal" data-target="#modalAltaEncuesta" data-keyboard="false" />
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
                        <div class="modal-content" style="height: 100%">
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
                                <uc:UcAltaEncuesta runat="server" id="UcAltaEncuesta" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%--AREA--%>
<div class="modal fade" id="modalAreas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upModalAltaAreas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <uc:AltaArea runat="server" ID="AltaAreas" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
