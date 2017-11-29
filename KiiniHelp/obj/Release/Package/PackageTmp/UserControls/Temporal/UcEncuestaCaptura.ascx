<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcEncuestaCaptura.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcEncuestaCaptura" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upEncuestas">
    <ContentTemplate>


        <asp:HiddenField runat="server" ID="hfIdEncuesta" />
        <asp:HiddenField runat="server" ID="hfIdTicket" />
        <asp:HiddenField runat="server" ID="hfIdTipoServicio" />
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label runat="server" ID="lblDescripcionMascara"></asp:Label>
            </div>
            <div class="panel-body">
                <div runat="server" id="divControles">
                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button runat="server" Text="Enviar" ID="btnAceptar" CssClass="btn btn-success" OnClick="btnAceptar_OnClick" />
                <%--<asp:Button runat="server" Text="Enviar" ID="btnCancelar" CssClass="btn btn-danger" OnClick="btnCancelar_OnClick"/>--%>
            </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="click" />
    </Triggers>
</asp:UpdatePanel>
