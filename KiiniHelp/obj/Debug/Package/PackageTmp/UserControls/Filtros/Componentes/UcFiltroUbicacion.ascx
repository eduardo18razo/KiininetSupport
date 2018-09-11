<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroUbicacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroUbicacion" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Ubicación<br />
                <tc:RadComboBox ID="rcbFiltroUbicacion" runat="server" RenderMode="Lightweight"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
