<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroFechasGrafico.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroFechasGrafico" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="col-lg-1">
                <div class="form-group">
                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Visualizar:</label>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">
                        <asp:DropDownList runat="server" ID="ddlTipoFiltro" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoFiltro_OnSelectedIndexChanged">
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
                        <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaInicio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2">
                <div class="form-group">
                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Fecha Fin:</label>
                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                        <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaFin" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
