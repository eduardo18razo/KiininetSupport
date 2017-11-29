<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroTipoUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroTipoUsuario" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
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
        <div class="panel panel-primary">
            <div class="panel-heading">
                Tipo de Usuario
            </div>
            <div class="panel-body">
                <%--ORIGEN--%>
                <div class="panel panel-primary">
                    <div class="panel-heading text-center text-primary">
                        Seleccione
                    </div>
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptTipoUsuario">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-4 text-center" runat="server" Text="Tipo de Usuario" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <br />
                <%--SELECCION--%>
                <div class="panel panel-primary">
                    <div class="panel-heading text-center text-primary">
                        Seleccionados
                    </div>
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptTipoUsuarioSeleccionado">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-4 text-center" runat="server" Text="Tipo De Usuario" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label CssClass="col-sm-4" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="panel-footer text-center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
