<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroEstatus.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroEstatus" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group no-padding-left">
                Estatus<br />
                <tc:RadComboBox  ID="rcbFiltroEstatus" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
