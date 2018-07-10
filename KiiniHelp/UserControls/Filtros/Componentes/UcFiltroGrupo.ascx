<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroGrupo.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroGrupo" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Grupo<br />
                <asp:ListBox ID="lstFiltroGrupos" SelectionMode="Multiple" runat="server" OnSelectedIndexChanged="lstFiltroGrupos_OnSelectedIndexChanged"/>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroGrupos]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroGrupos]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
