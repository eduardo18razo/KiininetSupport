<%@ Page Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="FrmTicket.aspx.cs" Inherits="KiiniHelp.Agente.FrmTicket" %>

<%@ Register Src="~/UserControls/Detalles/UcTicketDetalle.ascx" TagPrefix="uc1" TagName="UcTicketDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <uc1:UcTicketDetalle runat="server" ID="UcTicketDetalle" />
    </div>
</asp:Content>
