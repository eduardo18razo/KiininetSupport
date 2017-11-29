<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarEstatusTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarEstatusTicket" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTicketCerrado"/>
        <asp:HiddenField runat="server" ID="hfEstatusActual"/>
        <header id="panelAlertaGeneral" runat="server" visible="false">
            Cambiar Estatus...
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
                <asp:Repeater runat="server" ID="rptErrorGeneral">
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
                <h3>Cambio de estatus</h3>
            </div>
            <div class="panel panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Ticket" CssClass="col-xs-3" />
                        <div class="col-xs-6">
                            <asp:Label runat="server" ID="lblIdticket" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Estatus Ticket" CssClass="col-xs-3" />
                        <div class="col-xs-6">
                            <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="DropSelect" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Comentarios" CssClass="col-xs-3" />
                        <div class="col-xs-6">
                            <asp:TextBox runat="server" ID="txtComentarios" CssClass="form-control" Height="120px" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
