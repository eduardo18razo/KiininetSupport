﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleMascaraCaptura.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleMascaraCaptura" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfIdTicket" />

        <asp:Label runat="server" ID="lblDescripcionMascara"></asp:Label>
        <hr />

        <div runat="server" id="divControles" class="form-horizontal col-lg-12 col-md-12 col-sm-12 col-xs-12">
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
