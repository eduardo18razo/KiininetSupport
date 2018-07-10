<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroCanalApertura.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroCanalApertura" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="form-group">
                Canal<br />
                <asp:ListBox ID="lstFiltroCanalApertura" SelectionMode="Multiple" runat="server" />
            </div>
        </div>

        
        <script type="text/javascript">
            $(function () {
                $('[id*=lstFiltroCanalApertura]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstFiltroCanalApertura]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
