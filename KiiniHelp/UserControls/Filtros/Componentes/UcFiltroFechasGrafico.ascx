<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroFechasGrafico.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroFechasGrafico" %>
<script>

    function getWeekNumber(d) {
        // Copy date so don't modify original
        d = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate()));
        // Set to nearest Thursday: current date + 4 - current day number
        // Make Sunday's day number 7
        d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
        // Get first day of year
        var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
        // Calculate full weeks to nearest Thursday
        var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
        // Return array of year and week number
        return [d.getUTCFullYear(), weekNo];
    }

    Date.prototype.getWeek = function () {
        var date = new Date(this.getTime());
        date.setHours(0, 0, 0, 0);
        // Thursday in current week decides the year.
        date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
        // January 4 is always in week 1.
        var week1 = new Date(date.getFullYear(), 0, 4);
        // Adjust to Thursday in week 1 and count number of weeks from date to week1.
        return 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000 - 3 + (week1.getDay() + 6) % 7) / 7);
    }

    function getDateRangeOfWeek(weekNo, y) {
        var d1, numOfdaysPastSinceLastMonday, rangeIsFrom, rangeIsTo;
        d1 = new Date('' + y + '');
        numOfdaysPastSinceLastMonday = d1.getDay() - 1;
        d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
        d1.setDate(d1.getDate() + (7 * (weekNo - d1.getWeek())));
        rangeIsFrom = d1.getDate() + "/" + (d1.getMonth() + 1) + "/" + d1.getFullYear();
        d1.setDate(d1.getDate() + 6);
        rangeIsTo = +d1.getDate() + "/" + (d1.getMonth() + 1) + "/" + d1.getFullYear();
        return [rangeIsFrom, rangeIsTo];
        //return rangeIsFrom + " to " + rangeIsTo;
    };

</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="col-lg-1">
                <div class="form-group">
                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Visualizar:</label>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">
                        <asp:DropDownList runat="server" ID="ddlTipoFiltro" CssClass="form-control" OnSelectedIndexChanged="ddlTipoFiltro_OnSelectedIndexChanged">
                            <asp:ListItem Text="Diario" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Semanal" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Mensual" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Anual" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-lg-2">
                <div class="form-group">
                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Fecha Inicio:</label>
                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFechaInicio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2">
                <div class="form-group">
                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Fecha Fin:</label>
                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFechaFin" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
        </div>
        <script>
            $(function () {
                $('#<%= ddlTipoFiltro.ClientID %>').on('change', function () {
                    $('#<%= txtFechaInicio.ClientID %>').val('');
                    $('#<%= txtFechaFin.ClientID %>').val('');
                });

                $('#<%= txtFechaInicio.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    onSelect: function (dateText, inst) {
                        debugger;
                        switch ($('#<%= ddlTipoFiltro.ClientID %>').val()) {
                            case "1":
                                $('#<%= txtFechaInicio.ClientID %>').val(dateText);
                        break;
                    case "2":
                        var dateStringWeek = dateText; // Oct 23
                        var datePartsWeek = dateStringWeek.split("/");
                        var dateObjectWeek = new Date(datePartsWeek[2], datePartsWeek[1] - 1, datePartsWeek[0]);
                        var result = getDateRangeOfWeek(getWeekNumber(dateObjectWeek)[1], dateObjectWeek.getFullYear());
                        $('#<%= txtFechaInicio.ClientID %>').val(result[0]);
                        break;
                    case "3":
                        var dateStringMonth = dateText;
                        var datePartsMonth = dateStringMonth.split("/");
                        var firstDay = new Date(datePartsMonth[2], datePartsMonth[1] - 1, 1);
                        $('#<%= txtFechaInicio.ClientID %>').val((firstDay.getDate()) + '/' + (firstDay.getMonth() + 1) + '/' + firstDay.getFullYear());
                        break;
                    case "4":
                        var dateStringYear = dateText; // Oct 23
                        var datePartsYear = dateStringYear.split("/");
                        var dateObjectYear = new Date(datePartsYear[2], datePartsYear[1] - 1, datePartsYear[0]);
                        var yearYear = dateObjectYear.getUTCFullYear();
                        $('#<%= txtFechaInicio.ClientID %>').val('01/01' + yearYear);
                        break;
                }
                    }
                });

                $('#<%= txtFechaFin.ClientID %>').datepicker({
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    onSelect: function (dateText, inst) {
                        debugger;
                        switch ($('#<%= ddlTipoFiltro.ClientID %>').val()) {
                            case "1":
                                $('#<%= txtFechaFin.ClientID %>').val(dateText);
                        break;
                    case "2":
                        var dateStringWeek = dateText; // Oct 23
                        var datePartsWeek = dateStringWeek.split("/");
                        var dateObjectWeek = new Date(datePartsWeek[2], datePartsWeek[1] - 1, datePartsWeek[0]);
                        var result = getDateRangeOfWeek(getWeekNumber(dateObjectWeek)[1], dateObjectWeek.getFullYear());
                        $('#<%= txtFechaFin.ClientID %>').val(result[1]);
                        break;
                    case "3":
                        var dateStringMonth = dateText;
                        var datePartsMonth = dateStringMonth.split("/");

                        var lastDay = new Date(datePartsMonth[2], datePartsMonth[1], 0);

                        $('#<%= txtFechaFin.ClientID %>').val((lastDay.getDate()) + '/' + (lastDay.getMonth() + 1) + '/' + lastDay.getFullYear());
                        break;
                    case "4":
                        var dateStringYear = dateText;
                        var datePartsYear = dateStringYear.split("/");
                        var dateObjectYear = new Date(datePartsYear[2], datePartsYear[1] - 1, datePartsYear[0]);
                        var yearYear = dateObjectYear.getUTCFullYear();
                        $('#<%= txtFechaFin.ClientID %>').val('31/12/' + yearYear);
                        break;
                }
                    }
                });
            });
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        $('#<%= ddlTipoFiltro.ClientID %>').on('change', function () {
            $('#<%= txtFechaInicio.ClientID %>').val('');
            $('#<%= txtFechaFin.ClientID %>').val('');
        });
        $('#<%= txtFechaInicio.ClientID %>').datepicker({
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                onSelect: function (dateText, inst) {
                    debugger;
                    switch ($('#<%= ddlTipoFiltro.ClientID %>').val()) {
                    case "1":
                        $('#<%= txtFechaInicio.ClientID %>').val(dateText);
                        break;
                    case "2":
                        var dateStringWeek = dateText; // Oct 23
                        var datePartsWeek = dateStringWeek.split("/");
                        var dateObjectWeek = new Date(datePartsWeek[2], datePartsWeek[1] - 1, datePartsWeek[0]);
                        var result = getDateRangeOfWeek(getWeekNumber(dateObjectWeek)[1], dateObjectWeek.getFullYear());
                        $('#<%= txtFechaInicio.ClientID %>').val(result[0]);
                            break;
                        case "3":
                            var dateStringMonth = dateText;
                            var datePartsMonth = dateStringMonth.split("/");
                            var firstDay = new Date(datePartsMonth[2], datePartsMonth[1] - 1, 1);
                            $('#<%= txtFechaInicio.ClientID %>').val((firstDay.getDate()) + '/' + (firstDay.getMonth() + 1) + '/' + firstDay.getFullYear());
                            break;
                        case "4":
                            var dateStringYear = dateText; // Oct 23
                            var datePartsYear = dateStringYear.split("/");
                            var dateObjectYear = new Date(datePartsYear[2], datePartsYear[1] - 1, datePartsYear[0]);
                            var yearYear = dateObjectYear.getUTCFullYear();
                            $('#<%= txtFechaInicio.ClientID %>').val('01/01' + yearYear);
                            break;
                    }
            }
            });

            $('#<%= txtFechaFin.ClientID %>').datepicker({
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                onSelect: function (dateText, inst) {
                    debugger;
                    switch ($('#<%= ddlTipoFiltro.ClientID %>').val()) {
                    case "1":
                        $('#<%= txtFechaFin.ClientID %>').val(dateText);
                        break;
                    case "2":
                        var dateStringWeek = dateText; // Oct 23
                        var datePartsWeek = dateStringWeek.split("/");
                        var dateObjectWeek = new Date(datePartsWeek[2], datePartsWeek[1] - 1, datePartsWeek[0]);
                        var result = getDateRangeOfWeek(getWeekNumber(dateObjectWeek)[1], dateObjectWeek.getFullYear());
                        $('#<%= txtFechaFin.ClientID %>').val(result[1]);
                            break;
                        case "3":
                            var dateStringMonth = dateText;
                            var datePartsMonth = dateStringMonth.split("/");

                            var lastDay = new Date(datePartsMonth[2], datePartsMonth[1], 0);

                            $('#<%= txtFechaFin.ClientID %>').val((lastDay.getDate()) + '/' + (lastDay.getMonth() + 1) + '/' + lastDay.getFullYear());
                            break;
                        case "4":
                            var dateStringYear = dateText;
                            var datePartsYear = dateStringYear.split("/");
                            var dateObjectYear = new Date(datePartsYear[2], datePartsYear[1] - 1, datePartsYear[0]);
                            var yearYear = dateObjectYear.getUTCFullYear();
                            $('#<%= txtFechaFin.ClientID %>').val('31/12/' + yearYear);
                            break;
                    }
            }
        });
        });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>

