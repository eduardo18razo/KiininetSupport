<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcImpactoUrgencia.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcImpactoUrgencia" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upGrupos" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfTipoUsuario" />
                <asp:HiddenField runat="server" ID="hfAsignacionAutomatica" />
                <header class="modal-header" id="panelAlertaImpacto" runat="server" visible="false">
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
                        <asp:Repeater runat="server" ID="rptErrorImpacto">
                            <ItemTemplate>
                                <ul>
                                    <li><%# Container.DataItem %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <div class="well">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Prioridad Urgencia
                        </div>
                        <div class="panel-body">
                            <div class="panel">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" class="col-sm-3 control-label">Prioridad</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPrioridad" Width="450px" CssClass="DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlImpacto_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" class="col-sm-3 control-label">Urgencia</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlUrgencia" Width="450px" CssClass="DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlUrgencia_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group bg-success" runat="server" ID="divImpacto" Visible="False">
                                        <div class="col-sm-offset-1 text-center">
                                            <h3>
                                                Impacto: <strong><asp:Label runat="server" ID="lblImpacto"/></strong>
                                            </h3>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <br />

                        </div>
                        <div class="panel-footer text-center" >
                            <asp:Button ID="btnAsignar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAsignar_OnClick" />
                            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
