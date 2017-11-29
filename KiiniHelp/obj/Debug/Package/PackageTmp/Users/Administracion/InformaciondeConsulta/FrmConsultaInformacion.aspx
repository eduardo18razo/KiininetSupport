<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaInformacion.aspx.cs" Inherits="KiiniHelp.Users.Administracion.InformaciondeConsulta.FrmConsultaInformacion" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaInformacionConsulta.ascx" TagPrefix="uc1" TagName="UcConsultaInformacionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaInformacionConsulta runat="server" ID="UcConsultaInformacionConsulta" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>