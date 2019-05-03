<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEdicionOpcionConsulta.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmEdicionOpcionConsulta" %>

<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcEdicionOpcionConsulta.ascx" TagPrefix="uc1" TagName="UcEdicionOpcionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcEdicionOpcionConsulta runat="server" ID="UcEdicionOpcionConsulta" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>