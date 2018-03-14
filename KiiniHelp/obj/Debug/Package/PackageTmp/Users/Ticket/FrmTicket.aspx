<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmTicket.aspx.cs" Inherits="KiiniHelp.Users.Ticket.FrmTicket" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcFormulario.ascx" TagPrefix="uc1" TagName="UcFormulario" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
