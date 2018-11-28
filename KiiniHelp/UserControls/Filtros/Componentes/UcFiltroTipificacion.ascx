<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroTipificacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroTipificacion" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTipoArbol"/>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Opcion<br />
                <tc:RadComboBox  ID="rcbFiltroTipificacion" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"/>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
