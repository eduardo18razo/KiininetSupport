<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarEstatusTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarEstatusTicket" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTicketCerrado" />
        <asp:HiddenField runat="server" ID="hfEstatusActual" />
        <asp:HiddenField runat="server" ID="hfIdSubRolActual" />
        <div class="modal-header">
            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCancelar_OnClick" Text='&times' />
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBrandingModal" Text="Cambio de Estatus" /></h6>           
        </div>

        <div class="modal-body">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="module-inner">
                        <div class="row form-group">
                            <asp:Label runat="server" Text="Ticket" CssClass="col-lg-3" />
                            <div class="col-lg-9 no-padding-right">
                                <asp:Label runat="server" ID="lblIdticket" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <asp:Label runat="server" Text="Estatus Ticket" CssClass="col-lg-3 margin-top-9" />
                            <div class="col-lg-9 no-padding-right">
                                <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <asp:Label runat="server" Text="Comentarios" CssClass="col-lg-3 margin-top-9" />
                            <div class="col-lg-9 no-padding-right">
                                <asp:TextBox runat="server" ID="txtComentarios" CssClass="form-control" Height="120px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="text-right margin-top-20">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick"/>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
