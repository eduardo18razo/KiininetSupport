<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroPrioridad.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroPrioridad" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Prioridad<br />
                <asp:ListBox ID="lstFiltroPrioridad" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroPrioridad]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroPrioridad]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
