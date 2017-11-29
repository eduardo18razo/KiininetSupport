<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroTipificacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroTipificacion" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTipoArbol"/>
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
                        <asp:Repeater runat="server" ID="rptArbol">
                            <HeaderTemplate>
                                <div class="container-fluid"> 
                                    <asp:Label runat="server" Text="" Width="90px"/>                       
                                    <asp:Label runat="server" Text="Id" Visible="false"/>             
                                    <asp:Label runat="server" Text="TU" Width="40px"/>
                                    <asp:Label runat="server" Text="Producto" Width="100px"/>
                                    <asp:Label runat="server" Text="Servicio/incidente" Width="140px"/>
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 1" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 2" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 3" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 4" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 5" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 6" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 7" />                                                                    
                                </div>                       
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />                                                                      
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label runat="server" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0, 3)%>' Width="40px" />
                                    <asp:Label runat="server" Text='<%# Eval("Area.Descripcion")%>' Width="100px" />
                                    <asp:Label runat="server" Text='<%# Eval("TipoArbolAcceso.Descripcion")%>' Width="140px" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel1.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel2.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel3.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel4.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel5.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel6.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel7.Descripcion")%>' />                                                                    
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    </asp:Panel>
                </div>                
                <%--SELECCION--%>
                <div class="panel panel-primary">
                    <div style="font-weight: bold"">
                        Seleccionados
                    </div>
                    <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptArbolSeleccionado">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label runat="server" Text="" Width="90px" />
                                    <asp:Label runat="server" Text="Id" Visible="false"/>
                                    <asp:Label runat="server" Text="TU" Width="40px"   />
                                    <asp:Label runat="server" Text="Producto" Width="100px"/>
                                    <asp:Label runat="server" Text="Servicio/incidente" Width="140px"/>
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 1" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 2" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 3" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 4" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 5" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 6" />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text="Nivel 7" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid" style="margin-top: 2px">
                                    <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" Width="84px" />
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id")%>' />
                                    <asp:Label runat="server" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0, 3)%>' Width="40px" />
                                    <asp:Label runat="server" Text='<%# Eval("Area.Descripcion")%>' Width="100px"/>
                                    <asp:Label runat="server" Text='<%# Eval("TipoArbolAcceso.Descripcion")%>' Width="140px"/>
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel1.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel2.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel3.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel4.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel5.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel6.Descripcion")%>' />
                                    <asp:Label CssClass="col-pe-1" runat="server" Text='<%# Eval("Nivel7.Descripcion")%>' />
                                   
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
