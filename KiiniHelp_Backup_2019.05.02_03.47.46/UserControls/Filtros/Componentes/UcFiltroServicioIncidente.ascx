<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroServicioIncidente.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroServicioIncidente" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.1.215.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfConsulta" />
        <asp:HiddenField runat="server" ID="hfEncuesta" />
        <asp:HiddenField runat="server" ID="hfticket" />
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Tipo Ticket<br />
                <tc:RadComboBox ID="rcbFiltroServicioIncidente" runat="server" RenderMode="Lightweight" CssClass="form-control no-border no-padding-left widht200" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnClientDropDownClosing="doPostback" />
            </div>
        </div>
        <script type="text/javascript">
            function doPostback() {
                __doPostBack('Seleccion', 'OnKeyPress');
            };
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
