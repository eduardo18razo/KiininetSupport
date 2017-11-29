<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroUbicacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroUbicacion" %>
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
                Ubicaciones
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
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="País" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Campus" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Torre" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Piso" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Zona" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Zona" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Site Rack" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Pais.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Campus.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Torre.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("Piso.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Zona.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubZona.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("SiteRack.Descripcion")%>' />
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
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="País" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Campus" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Torre" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Piso" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Zona" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Zona" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Site Rack" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Pais.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Campus.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Torre.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("Piso.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Zona.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubZona.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("SiteRack.Descripcion")%>' />

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
