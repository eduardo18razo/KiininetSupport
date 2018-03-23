<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroStack.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroStack" %>
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
        <div class="panel panel-primary">
            <div class="panel-heading">
                Stack por
            </div>
            <div class="panel-body">
                <asp:Button runat="server" ID="btnUbicacion"/>
                <asp:Button runat="server" ID="btnOrganizacion"/>
                <asp:Button runat="server" ID="btnTipificacion"/>
                <asp:Button runat="server" ID="btnTipificacionOpcion"/>
                <asp:Button runat="server" ID="btnEstatus"/>
                <asp:Button runat="server" ID="btnSla"/>
            </div>
            <div class="panel-footer text-center">
                 <%--OnClick="btnAceptar_OnClick" 
                OnClick="btnLimpiar_OnClick"
                OnClick="btnCancelar_OnClick"--%>
                <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar"/>
                <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar"  />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar"  />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>