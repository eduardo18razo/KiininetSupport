﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroServicioIncidenteEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroServicioIncidenteEncuesta" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>


        <div class="modal-header">
            <h2 class="modal-title">Tipo de Servicio</h2>
            <hr class="bordercolor">

            <asp:HiddenField runat="server" ID="hfticket" />
            <asp:HiddenField runat="server" ID="hfConsulta" />
            <asp:HiddenField runat="server" ID="hfEncuesta" />
        </div>

        <div class="panel-body">
            <%--ORIGEN--%>
            <div class="panel panel-primary">
                <div class="strong">
                    Seleccione                   
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptTipoArbol">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-4 text-center" runat="server" Text="Servicio/Incidente" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>

            <%--SELECCION--%>
            <div class="panel panel-primary">
                <div class="strong">
                    Seleccionados                   
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptTipoArbolSeleccionado">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-4 text-center" runat="server" Text="Servicio/Incidente" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div class="text-center">
            <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
        </div>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
