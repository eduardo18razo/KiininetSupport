<%@ Page Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuariosAgente.aspx.cs" Inherits="KiiniHelp.Agente.ConsultaUsuariosAgente" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaUsuariosAgente.ascx" TagPrefix="uc1" TagName="UcConsultaUsuariosAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaUsuariosAgente runat="server" ID="UcConsultaUsuariosAgente" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
