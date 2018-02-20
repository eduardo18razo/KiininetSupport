<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="FrmConsultaGrupos.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Grupos.FrmConsultaGrupos" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaGrupos.ascx" TagPrefix="uc1" TagName="UcConsultaGrupos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaGrupos runat="server" ID="UcConsultaGrupos" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
