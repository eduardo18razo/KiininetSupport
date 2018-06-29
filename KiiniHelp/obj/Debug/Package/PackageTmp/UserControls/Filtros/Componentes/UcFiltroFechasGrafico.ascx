<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroFechasGrafico.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroFechasGrafico" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <header class="modal-header" id="panelAlerta" runat="server" visible="false">
            <div class="alert alert-danger">
                <div>
                    <div class="float-left">
                        <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                    </div>
                    <div class="float-left">
                        <h3>Error</h3>
                    </div>
                    <div class="clearfix clear-fix" />
                </div>
                <hr />
                <asp:Repeater runat="server" ID="rptError">
                    <ItemTemplate>
                        <ul>
                            <li><%# Container.DataItem %></li>
                        </ul>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>
       <%-- <div class="panel panel-primary">--%>
            <%--<div class="panel-heading">
                Rango de fechas
            </div>--%>
            <div class="modal-header">
                <h3 class="modal-title">Rango de fechas</h3>
                <hr class="bordercolor">
            </div>
       
            <div class="panel-body">
                <div class="form-horizontal col-sm-12">
                    <div class="form-group col-sm-10">
                        <asp:Label CssClass="col-sm-3" runat="server" Text="Visualizar: " style="width: 110px" />
                        <div class="col-sm-8 widht180">
                            <asp:DropDownList runat="server" ID="ddlTipoFiltro" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoFiltro_OnSelectedIndexChanged">
                                <asp:ListItem Text="Diario" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Semanal" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Mensual" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Anual" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group col-sm-12">
                        <asp:Label CssClass="col-sm-3" runat="server" Text="Fecha Inicio" style="width: 110px"/>
                        <div class="col-sm-9" style="width: 205px">
                            <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaInicio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"/>
                        </div>
                    </div>
                    <div class="form-group col-sm-12">
                        <asp:Label CssClass="col-sm-3" runat="server" Text="Fecha Fin" style="width: 110px" />
                        <div class="col-sm-9" style="width: 205px">
                            <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaFin" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"/>
                        </div>
                    </div>
                </div>
            </div>
         
            <div class="text-center"> <%--panel-footer --%>
                <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick"/>
            </div>
        <br />
       <%-- </div>--%>
    </ContentTemplate>
</asp:UpdatePanel>
