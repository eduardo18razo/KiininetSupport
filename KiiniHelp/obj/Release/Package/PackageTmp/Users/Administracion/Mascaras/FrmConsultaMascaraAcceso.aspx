<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaMascaraAcceso.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Mascaras.FrmConsultaMascaraAcceso" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaMascaras.ascx" TagPrefix="uc1" TagName="UcConsultaMascaras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaMascaras runat="server" ID="UcConsultaMascaras" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>