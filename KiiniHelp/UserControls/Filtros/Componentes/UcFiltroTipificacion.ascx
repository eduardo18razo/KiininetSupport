<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroTipificacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroTipificacion" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTipoArbol"/>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Tipificación<br />
                <asp:ListBox ID="lstFiltroTipificacion" SelectionMode="Multiple" runat="server" OnSelectedIndexChanged="lstFiltroTipificacion_OnSelectedIndexChanged" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroTipificacion]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroTipificacion]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
