<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroResponsablesEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroResponsablesEncuesta" %>
<asp:UpdatePanel runat="server" ID="upResponsable" UpdateMode="Conditional">
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
                Grupos
            </div>
            <div class="panel-body">
        <%--ORIGEN--%>
        <div class="panel panel-primary">
            <div class="panel-heading text-center text-primary">
                Seleccione
            </div>
            <div class="panel-body">
                <asp:Repeater runat="server" ID="rptGpos">
                    <HeaderTemplate>
                        <div class="container-fluid">
                            <asp:Label CssClass="col-sm-1 text-center" runat="server" ID="lblTipoEmpleado" Text="TipoUsuario" />
                            <asp:Label CssClass="col-sm-5 text-center" runat="server" ID="lblDescripcion" Text="Grupo" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="container-fluid" style="margin-top: 2px">
                            <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                            <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />
                            <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3) %>' />
                            <asp:Label CssClass="col-sm-6" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                            <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <br/>
        <%--SELECCION--%>
        <div class="panel panel-primary">
            <div class="panel-heading text-center text-primary">
                Seleccionados
            </div>
            <div class="panel-body">
                <asp:Repeater runat="server" ID="rptGpoSeleccionado">
                    <HeaderTemplate>
                        <div class="container-fluid">
                            <asp:Label CssClass="col-sm-1 text-center" runat="server" Text="TipoUsuario" />
                            <asp:Label CssClass="col-sm-5 text-center" runat="server" Text="Grupo" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="container-fluid" style="margin-top: 2px">
                            <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                            <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />
                            <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3) %>' />
                            <asp:Label CssClass="col-sm-6" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                            <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        </div>
            <div class="panel-footer text-center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick"/>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
