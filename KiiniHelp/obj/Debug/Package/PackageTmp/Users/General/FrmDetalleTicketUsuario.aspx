<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmDetalleTicketUsuario.aspx.cs" Inherits="KiiniHelp.Users.General.FrmDetalleTicketUsuario" %>

<%@ Register Src="~/UserControls/Detalles/UcTicketDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcTicketDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="heigth100">
        <uc1:UcTicketDetalleUsuario runat="server" id="UcTicketDetalleUsuario" />
    </div>
</asp:Content>
