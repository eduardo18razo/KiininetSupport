<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroGrupo.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroGrupo" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
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
            <h2 class="modal-title">Grupos</h2>
            <hr class="bordercolor">
        </div>
       
        <div class="panel-body">
            <%--ORIGEN--%>
            <div class="panel panel-primary">
                <div class="strong"> 
                Seleccione           
                </div>                               

                <div style="overflow:scroll; width: 100%; height: 140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptGpos">
                            <HeaderTemplate>
                                <div class="container-fluid" >
                                    <asp:Label CssClass="col-sm-1 text-left" runat="server" ID="lblTipoEmpleado" Text="TU" />
                                    <asp:Label CssClass="col-sm-3 text-left" runat="server" ID="lblTipoGrupo" Text="Tipo Grupo" />
                                    <asp:Label CssClass="col-sm-6 text-left" runat="server" ID="lblDescripcion" Text="Grupo"/>  
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />  
                                           <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3)%>' Visible="false" /> 
                                    <asp:Label runat="server" ID="lblAbrevTipoUsuario" CssClass="btn btn-default-alt col-sm-1 btn-square-usuario"  Text='<%# Eval("TipoUsuario.Abreviacion") %>' style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important; width: 25px !important" %>'/>                                                                                                                                                                                 
                                    <asp:HiddenField runat="server" ID="hfColor" Value='<%# Eval("TipoUsuario.Color") %>' /> 
                                    <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoGrupoDes" Text='<%# Eval("TipoGrupo.Descripcion") %>' />
                                    <asp:Label CssClass="col-sm-6" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    </div>
                <%--</asp:Panel>--%>
            </div>            
            <%--SELECCION--%>
            <div class="panel panel-primary">
                <div class="strong">       
                Seleccionados           
                </div>
                <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                    <div class="panel-body">
                        <asp:Repeater runat="server" ID="rptGpoSeleccionado">
                            <HeaderTemplate>
                                <div class="container-fluid">
                                    <asp:Label CssClass="col-sm-1 text-left" runat="server" Text="TU" />
                                    <asp:Label CssClass="col-sm-3 text-left" runat="server" ID="lblTipoGrupo" Text="Tipo Grupo" />
                                    <asp:Label CssClass="col-sm-6 text-left" runat="server" Text="Grupo"/>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />
                                    <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3)%>' Visible="false" />
                                    <asp:Label runat="server" CssClass="btn btn-default-alt col-sm-1 btn-square-usuario" ID="lblAbrevTipoUsuario"  Text='<%# Eval("TipoUsuario.Abreviacion") %>' style='<%# "Border: none !important;  Background: " + Eval("TipoUsuario.Color") + " !important; width: 25px !important" %>' />        <%-- --%>                                  
                                    <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoGrupoDes" Text='<%# Eval("TipoGrupo.Descripcion") %>' />
                                    <asp:Label CssClass="col-sm-6" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                    <asp:Button runat="server" Text="Quitar" CssClass="btn btn-danger btn-sm" ID="btnQuitar" OnClick="btnQuitar_OnClick" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>
        </div>
        
        <div class="text-center"> 
            <asp:Button runat="server" Text="Seleccionar Todo" CssClass="btn btn-primary" ID="btnSeleccionarTodo" OnClick="btnSeleccionarTodo_Click" />           
           
            <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
        </div>        
        <br />         
    </ContentTemplate>
</asp:UpdatePanel>
