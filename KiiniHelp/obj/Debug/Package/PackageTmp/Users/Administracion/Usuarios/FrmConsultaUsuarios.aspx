<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaUsuarios.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Usuarios.FrmConsultaUsuarios" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaUsuarios.ascx" TagPrefix="uc1" TagName="UcConsultaUsuarios" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaUsuarios runat="server" ID="UcConsultaUsuarios" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
