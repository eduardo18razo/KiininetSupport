<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroAtendedores.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroAtendedores" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.1.215.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Agente<br />
                <tc:RadComboBox ID="rcbFiltroAgentes" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
