<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroTipoUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroTipoUsuario" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Tipo Usuario<br />
                <asp:ListBox ID="lstFiltroTipoUsuario" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroTipoUsuario]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroTipoUsuario]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
