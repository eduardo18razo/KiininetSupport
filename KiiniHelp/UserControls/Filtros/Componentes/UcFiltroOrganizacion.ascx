<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroOrganizacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <header class="modal-header" id="panelAlerta" runat="server" visible="false">
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
            <h2 class="modal-title">Organizaciones</h2>
            <hr class="bordercolor">
        </div>

        <div class="panel-body">
            <%--ORIGEN--%>
            <div class="panel panel-primary">
                <div class="strong">
                    Seleccione                   
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptOrganizaciones" >
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label runat="server" Text="Seleccionar"/>
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Holding" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Compañia"   />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Dirección" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Dirección" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Gerencia" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Sub Gerencia" />
                                    <asp:Label CssClass="col-pe-2" runat="server" Text="Jefatura" />

                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Holding.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Compania.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Direccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("SubDireccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Gerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubGerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("Jefatura.Descripcion")%>' />
                                    
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
                        <asp:Repeater runat="server" ID="rptOrganizacionSeleccionada">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label runat="server" Text="Quitar"/>
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
                                <div class="container-fluid margin-top-2">
                                    <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Holding.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Compania.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Direccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("SubDireccion.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Gerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubGerencia.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("Jefatura.Descripcion")%>' />                                    
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
