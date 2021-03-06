﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroCategoria.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroCategoria" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tc" %>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Categoria<br />
                <tc:RadComboBox ID="rcbFiltroCategoria" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
