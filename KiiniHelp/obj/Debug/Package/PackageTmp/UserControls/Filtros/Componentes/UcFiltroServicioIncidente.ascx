<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroServicioIncidente.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroServicioIncidente" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfConsulta" />
        <asp:HiddenField runat="server" ID="hfEncuesta" />
        <asp:HiddenField runat="server" ID="hfticket" />
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Servicio/Incidente<br />
                <asp:ListBox ID="lstFiltroServicioIncidente" SelectionMode="Multiple" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroServicioIncidente]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroServicioIncidente]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
