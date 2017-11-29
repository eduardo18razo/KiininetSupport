<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEdicionUsuario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Usuarios.FrmEdicionUsuario" %>

<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuario.ascx" TagPrefix="uc1" TagName="UcAltaUsuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcAltaUsuario runat="server" id="UcAltaUsuario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
