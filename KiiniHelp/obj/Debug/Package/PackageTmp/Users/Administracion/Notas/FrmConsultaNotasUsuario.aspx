<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaNotasUsuario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Notas.FrmConsultaNotasUsuario" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaNotasUsuario.ascx" TagPrefix="uc1" TagName="UcConsultaNotasUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaNotasUsuario runat="server" id="UcConsultaNotasUsuario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
