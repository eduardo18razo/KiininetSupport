<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroSla.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroSla" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                SLA<br />
                <asp:ListBox ID="lstFiltroSla" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroSla]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroSla]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
