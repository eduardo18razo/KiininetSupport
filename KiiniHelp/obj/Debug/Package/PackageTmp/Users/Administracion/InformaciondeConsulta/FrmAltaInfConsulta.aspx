<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master"  AutoEventWireup="true" CodeBehind="FrmAltaInfConsulta.aspx.cs" Inherits="KiiniHelp.Users.Administracion.InformaciondeConsulta.FrmAltaInfConsulta" %>

<%@ Register Src="~/UserControls/Altas/UcAltaInformacionConsulta.ascx" TagPrefix="uc1" TagName="UcAltaInformacionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcAltaInformacionConsulta runat="server" id="ucAltaInformacionConsulta" />
</asp:Content>
