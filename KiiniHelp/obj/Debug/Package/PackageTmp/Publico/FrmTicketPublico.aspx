<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmTicketPublico.aspx.cs" Inherits="KiiniHelp.Publico.FrmTicketPublico" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcFormulario.ascx" TagPrefix="uc1" TagName="UcFormulario" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdMascara" />
            <asp:HiddenField runat="server" ID="hfIdConsulta" />
            <asp:HiddenField runat="server" ID="hfIdEncuesta" />
            <asp:HiddenField runat="server" ID="hfIdSla" />
            <asp:HiddenField runat="server" ID="hfIdCanal" />
            <asp:HiddenField runat="server" ID="hfIdUsuarioSolicita" />
            <asp:Label runat="server" ID="lblTicketDescripcion"></asp:Label>

            <div class="panel-body">
                <uc1:UcFormulario runat="server" ID="ucFormulario" />
            </div>
            <%--<asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" Text="Guardar" CssClass="btn btn-lg btn-success" Visible="False" />
    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-lg btn-danger" OnClick="btnCancelar_OnClick" Visible="False" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
