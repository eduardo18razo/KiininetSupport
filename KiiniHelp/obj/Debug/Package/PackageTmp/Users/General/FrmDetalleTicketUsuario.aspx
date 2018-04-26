<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmDetalleTicketUsuario.aspx.cs" Inherits="KiiniHelp.Users.General.FrmDetalleTicketUsuario" %>

<%@ Register Src="~/UserControls/Detalles/UcTicketDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcTicketDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="heigth100">
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Users/General/FrmMisTickets.aspx">Mis Tickets</asp:HyperLink></li>
        </ol>
        <uc1:UcTicketDetalleUsuario runat="server" ID="UcTicketDetalleUsuario" />
    </div>
</asp:Content>
