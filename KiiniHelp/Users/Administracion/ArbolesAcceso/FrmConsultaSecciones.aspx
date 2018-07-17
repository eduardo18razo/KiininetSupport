<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaSecciones.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmConsultaSecciones" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaSecciones.ascx" TagPrefix="uc1" TagName="UcConsultaSecciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaSecciones runat="server" ID="UcConsultaSecciones" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
