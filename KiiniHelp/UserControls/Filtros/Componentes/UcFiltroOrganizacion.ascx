<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroOrganizacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Organización<br />
                <asp:ListBox ID="lstFiltroOrganizacion" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroOrganizacion]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroOrganizacion]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
