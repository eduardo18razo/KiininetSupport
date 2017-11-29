<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroOrganizacion" %>
<asp:UpdatePanel runat="server">
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
                Organizaciones
            </div>
            <div class="panel-body">
                <%--ORIGEN--%>
                <div class="panel panel-primary">
                    <div class="panel-heading text-center text-primary">
                        Seleccione
                    </div>
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptOrganizaciones">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Holding" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Compañia" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Dirección" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Sub Dirección" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Gerencia" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Sub Gerencia" />
                                    <asp:Label CssClass="col-sm-1" runat="server" Text="Jefatura" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblHolding" Text='<%# Eval("Holding.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblCompania" Text='<%# Eval("Compania.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblDireccion" Text='<%# Eval("Direccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblSubDireccion" Text='<%# Eval("SubDireccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblGerencia" Text='<%# Eval("Gerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubGerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-sm-1" runat="server" ID="lblJefatura" Text='<%# Eval("Jefatura.Descripcion")%>' />
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
                        <asp:Repeater runat="server" ID="rptOrganizacionSeleccionada">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Holding" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Compañia" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Dirección" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Dirección" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Gerencia" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Gerencia" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Jefatura" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Holding.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Compania.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Direccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("SubDireccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Gerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubGerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("Jefatura.Descripcion")%>' />
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
