<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmPreview.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Encuestas.FrmPreview" %>

<%@ Register Src="~/UserControls/Altas/Encuestas/UcPreviewEncuesta.ascx" TagPrefix="uc1" TagName="UcPreviewEncuesta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcPreviewEncuesta runat="server" id="UcPreviewEncuesta" />
</asp:Content>
