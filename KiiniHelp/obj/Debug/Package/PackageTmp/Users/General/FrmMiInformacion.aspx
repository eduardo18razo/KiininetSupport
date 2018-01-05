<%@ Page Title="Mi Actividad" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmMiInformacion.aspx.cs" Inherits="KiiniHelp.Users.General.FrmMiInformacion" %>

<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
