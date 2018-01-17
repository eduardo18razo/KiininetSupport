<%@ Page Title="Puestos" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaPuestos.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Puestos.FrmAltaPuestos" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaPuestos.ascx" TagPrefix="uc1" TagName="UcConsultaPuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaPuestos runat="server" id="UcConsultaPuestos" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>