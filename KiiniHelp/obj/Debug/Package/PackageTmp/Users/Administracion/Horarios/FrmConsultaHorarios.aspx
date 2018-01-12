<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaHorarios.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Horarios.FrmConsultaHorarios" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaHorario.ascx" TagPrefix="uc1" TagName="UcConsultaHorario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaHorario runat="server" id="UcConsultaHorario" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>