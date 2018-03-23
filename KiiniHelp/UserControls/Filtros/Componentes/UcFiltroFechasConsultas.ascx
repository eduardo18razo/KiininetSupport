<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroFechasConsultas.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Componentes.UcFiltroFechasConsultas" %>
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
        
        <div class="modal-header">           
            <h2 class="modal-title"> Rango de fechas</h2>
            <hr class="bordercolor">
        </div>                      
        
        <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" Text="Fecha Inicio" />
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaInicio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" Text="Fecha Fin" />
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaFin" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"/>
                        </div>
                    </div>
                </div>
            </div>
        
        <div class="text-center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-primary" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick"/>
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick"/>
            </div>
        
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
