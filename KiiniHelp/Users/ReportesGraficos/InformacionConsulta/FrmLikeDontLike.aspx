<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmLikeDontLike.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.InformacionConsulta.FrmLikeDontLike" %>

<%@ Register Src="~/UserControls/ReportesGraficos/InformacionConsulta/UcReporteInformacionConsulta.ascx" TagPrefix="uc1" TagName="UcReporteInformacionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteInformacionConsulta runat="server" id="UcReporteInformacionConsulta" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
