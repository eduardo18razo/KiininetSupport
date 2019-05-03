<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroOrganizacion" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Organización<br />
                <tc:RadComboBox ID="rcbFiltroOrganizacion" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
