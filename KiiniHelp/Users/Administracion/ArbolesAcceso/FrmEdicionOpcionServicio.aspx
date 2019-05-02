<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEdicionOpcionServicio.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmEdicionOpcionServicio" %>

<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcEdicionOpcionServicio.ascx" TagPrefix="uc1" TagName="UcEdicionOpcionServicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcEdicionOpcionServicio runat="server" id="UcEdicionOpcionServicio" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
