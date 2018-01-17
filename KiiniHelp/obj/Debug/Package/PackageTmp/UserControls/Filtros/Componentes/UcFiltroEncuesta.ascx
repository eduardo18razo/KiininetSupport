<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroEncuesta" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upEncuestas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTipoArbol" />
        <header class="modal-header" id="panelAlerta" runat="server" visible="false">
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
                <hr />
                <asp:Repeater runat="server" ID="rptError">
                    <ItemTemplate>
                        <ul>
                            <li><%# Container.DataItem %></li>
                        </ul>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>

        <div class="modal-header">
            <h2 class="modal-title">Tipificación</h2>
            <hr class="bordercolor">
        </div>

        <div class="panel-body">
            <%--ORIGEN--%>
            <div class="panel panel-primary">
                <div style="font-weight: bold">
                    Seleccione                   
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptEncuestas">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="" Visible="False" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="" Visible="False" />
                                    <asp:Label CssClass="col-sm-3" runat="server" Text="Tipo de Encuesta" />
                                    <asp:Label CssClass="col-sm-4" runat="server" Text="Encuesta" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="Label1" Text='<%# Eval("IdTipoEncuesta")%>' />
                                    <asp:Label CssClass="col-sm-3" runat="server" Text='<%# Eval("TipoEncuesta.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" Text='<%# Eval("Descripcion")%>' />
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>

            <%--SELECCION--%>
            <div class="panel panel-primary">
                <div style="font-weight: bold">
                    Seleccionados                   
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptEncuestasSeleccionadas">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="" Visible="False" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="" Visible="False" />
                                    <asp:Label CssClass="col-sm-3" runat="server" Text="Tipo de Encuesta" />
                                    <asp:Label CssClass="col-sm-4" runat="server" Text="Encuesta" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="Label1" Text='<%# Eval("IdTipoEncuesta")%>' />
                                    <asp:Label CssClass="col-sm-3" runat="server" Text='<%# Eval("TipoEncuesta.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" Text='<%# Eval("Descripcion")%>' />
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
