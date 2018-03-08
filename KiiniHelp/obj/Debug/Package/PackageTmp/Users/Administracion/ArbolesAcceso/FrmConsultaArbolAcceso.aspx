<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaArbolAcceso.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmConsultaArbolAcceso" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaArboles.ascx" TagPrefix="uc1" TagName="UcConsultaArboles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaArboles runat="server" ID="UcConsultaArboles" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
