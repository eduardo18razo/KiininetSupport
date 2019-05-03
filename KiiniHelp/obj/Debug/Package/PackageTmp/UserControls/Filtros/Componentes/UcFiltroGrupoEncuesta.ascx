<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroGrupoEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroGrupoEncuesta" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblOperacion" Font-Bold="true" Text="Grupos Sobre Los Que Tengo Privilegios" />
            </h6>
        </div>

        <div class="panel-body">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="strong">
                        Seleccione           
                    </div>
                    <asp:Panel runat="server" ScrollBars="Vertical" Width="100%" Height="140px">
                        <div class="panel-body">
                            <asp:Repeater runat="server" ID="rptGpos">
                                <HeaderTemplate>
                                    <div class="container-fluid">
                                        <asp:Label CssClass="col-sm-1 text-center" runat="server" ID="lblTipoEmpleado" Text="TipoUsuario" />
                                        <asp:Label CssClass="col-sm-5 text-center" runat="server" ID="lblDescripcion" Text="Grupo" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="container-fluid margin-top-2">
                                        <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                        <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />
                                        <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3) %>' />
                                        <asp:Label CssClass="col-sm-6" runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' />
                                        <asp:CheckBox runat="server" Checked="False" CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text="Hola" ID="chkSeleccion"/>
                                        <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary btn-sm" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>
                </div>
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
                                    <asp:Label CssClass="col-sm-1 text-center" runat="server" Text="TipoUsuario" />
                                    <asp:Label CssClass="col-sm-5 text-center" runat="server" Text="Grupo" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="container-fluid margin-top-2">
                                    <asp:Label runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                    <asp:Label runat="server" Visible="False" ID="lblIdTipoUsuario" Text='<%# Eval("IdTipoUsuario") %>' />
                                    <asp:Label CssClass="col-sm-3" runat="server" ID="lblTipoUsuario" Text='<%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,3) %>' />
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
            <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
            <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
        </div>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
