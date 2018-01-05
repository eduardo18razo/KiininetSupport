<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcMensajeValidacion.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcMensajeValidacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnClose_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h3><asp:Label runat="server" ID="lblMensaje"/></h3>
        </div>
        <div class="modal-body">
            <uc1:UcDetalleUsuario runat="server" id="ucDetalleUsuario" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
