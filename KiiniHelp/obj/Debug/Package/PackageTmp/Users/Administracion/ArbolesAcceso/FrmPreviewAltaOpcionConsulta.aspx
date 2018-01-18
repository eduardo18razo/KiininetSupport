<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmPreviewAltaOpcionConsulta.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmPreviewAltaOpcionConsulta" %>

<%@ Register Src="~/UserControls/Preview/UcPreviewOpcionConsulta.ascx" TagPrefix="uc1" TagName="UcPreviewOpcionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcPreviewOpcionConsulta runat="server" id="UcPreviewOpcionConsulta" />
</asp:Content>
