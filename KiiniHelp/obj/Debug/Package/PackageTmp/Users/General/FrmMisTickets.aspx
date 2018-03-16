<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmMisTickets.aspx.cs" Inherits="KiiniHelp.Users.General.FrmMisTickets" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaMisTickets.ascx" TagPrefix="uc1" TagName="UcConsultaMisTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <link href="../../assets/css/styles_movil.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaMisTickets runat="server" id="UcConsultaMisTickets" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
