﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarEstatusTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarEstatusTicket" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTicketCerrado" />
        <asp:HiddenField runat="server" ID="hfEstatusActual" />
        <div class="modal-header">
            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCancelar_OnClick" Text='&times' />
            <h2 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBrandingModal" /></h2>
            <p class="text-center">
                <asp:Label runat="server" Text="Cambio de Estatus" />
            </p>
        </div>
        <div class="modal-body">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
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
                                <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" Text="Comentarios" CssClass="col-xs-3" />
                            <div class="col-xs-6">
                                <asp:TextBox runat="server" ID="txtComentarios" CssClass="form-control" Height="120px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <p class="text-right margin-top-40">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick"/>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                    </p>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>