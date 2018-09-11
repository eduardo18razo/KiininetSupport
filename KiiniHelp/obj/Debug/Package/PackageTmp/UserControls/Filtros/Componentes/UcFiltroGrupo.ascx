<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroGrupo.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroGrupo" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Grupo<br />
                <tc:RadComboBox ID="rcbFiltroGrupos" runat="server" RenderMode="Lightweight" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" /> 
                <%--OnClientDropDownClosing="doPostback"--%>
            </div>
        </div>
       <%-- <script type="text/javascript">
            function doPostback() {
                __doPostBack('Seleccion', 'OnKeyPress');
            };
        </script>--%>
    </ContentTemplate>
</asp:UpdatePanel>
