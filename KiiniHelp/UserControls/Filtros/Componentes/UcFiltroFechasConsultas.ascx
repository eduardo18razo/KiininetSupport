<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroFechasConsultas.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroFechasConsultas" %>
<script>
    function padLeft(data, size, paddingChar) {
        return (new Array(size + 1).join(paddingChar || '0') + String(data)).slice(-size);
    }
    function SetStartDate(txt) {
        debugger;
        var dateText = txt.value;
        var dateSplit = dateText.split("/");
        if (dateSplit.length < 3) {
            return;
        }
        var c;
        if (dateText.length < 10) {
            dateText = "";
            for (c = 0; c < dateSplit.length; c++) {
                if (c == 2) {
                    if (dateSplit[c].length < 4) {
                        return;
                    }
                    dateText += padLeft(dateSplit[c], 4, '0');
                } else {
                    dateText += padLeft(dateSplit[c], 2, '0') + '/';
                }

            }
        }
        if (dateText.length == 10) {
            $('#<%= txtFechaInicio.ClientID %>').val(dateText);
        }
    }

    function SetEndDate(txt) {
        debugger;
        var dateText = txt.value;
        var dateSplit = dateText.split("/");
        if (dateSplit.length < 3) {
            return;
        }
        var c;
        if (dateText.length < 10) {
            dateText = "";
            for (c = 0; c < dateSplit.length; c++) {
                if (c == 2) {
                    if (dateSplit[c].length < 4) {
                        return;
                    }
                    dateText += padLeft(dateSplit[c], 4, '0');
                } else {
                    dateText += padLeft(dateSplit[c], 2, '0') + '/';
                }

            }
        }
        if (dateText.length == 10) {
            $('#<%= txtFechaFin.ClientID %>').val(dateText);
        }
    }
</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row no-padding-top no-padding-left">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 no-padding-top no-padding-left">
                <div class="form-group">
                    <label class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left no-margin-left">Fecha Inicio:</label>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-lg-12 no-padding-left no-margin-left">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFechaInicio" autocomplete="off" onkeyup="SetStartDate(this);" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 no-padding-top no-padding-left">
                <div class="form-group">
                    <label class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left no-margin-left">Fecha Fin:</label>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left no-margin-left">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFechaFin" autocomplete="off" onkeyup="SetEndDate(this);" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
        </div>
        <script>
            $(function () {

                $('#<%= txtFechaInicio.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                });

                $('#<%= txtFechaFin.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,

                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('#<%= txtFechaInicio.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                });

                $('#<%= txtFechaFin.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                });
            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
