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
        
        <div class="modal-header">
                    <h2 class="modal-title">Ubicaciones</h2>
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
                            <asp:Repeater runat="server" ID="rptOrganizaciones">
                                <HeaderTemplate>
                                    <div class="container-fluid">
                                         <asp:Label runat="server" Text="Seleccionar"/>
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
                                        <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                        <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                        <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Pais.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Campus.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Torre.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("Piso.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Zona.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubZona.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("SiteRack.Descripcion")%>' />
                                       
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
                            <asp:Repeater runat="server" ID="rptOrganizacionSeleccionada">
                                <HeaderTemplate>
                                    <div class="container-fluid">
                                        <asp:Label runat="server" Text="Quitar"/>
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
                                        <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                                        <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                        <asp:Label CssClass="col-pe-1" runat="server" ID="lblHolding" Text='<%# Eval("Pais.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblCompania" Text='<%# Eval("Campus.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblDireccion" Text='<%# Eval("Torre.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubDireccion" Text='<%# Eval("Piso.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblGerencia" Text='<%# Eval("Zona.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblSubGerencia" Text='<%# Eval("SubZona.Descripcion")%>' />
                                        <asp:Label CssClass="col-pe-2" runat="server" ID="lblJefatura" Text='<%# Eval("SiteRack.Descripcion")%>' />

                                        
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
