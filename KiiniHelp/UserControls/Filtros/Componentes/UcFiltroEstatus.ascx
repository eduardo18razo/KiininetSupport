<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroEstatus.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroEstatus" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Estatus<br />
                <asp:ListBox ID="lstFiltroEstatus" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroEstatus]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroEstatus]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
