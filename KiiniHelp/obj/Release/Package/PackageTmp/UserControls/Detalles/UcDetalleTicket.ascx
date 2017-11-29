<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleTicket" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleMascaraCaptura.ascx" TagPrefix="uc1" TagName="UcDetalleMascaraCaptura" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3>Detalle de ticket</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-xs-2" Text="Ticket " />
                        <div class="col-xs-10">
                            <asp:Label runat="server" CssClass="form-control" ID="lblticket" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-xs-2" Text="Estatus Actual " />
                        <div class="col-xs-10">
                            <asp:Label runat="server" CssClass="form-control" ID="lblestatus" />
                        </div>

                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-xs-2" Text="Asignacion Actual " />
                        <div class="col-xs-10">
                            <asp:Label runat="server" CssClass="form-control" ID="lblAsignacion" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-xs-2" Text="Fecha y hora Creacion " />
                        <div class="col-xs-10">
                            <asp:Label runat="server" CssClass="form-control" ID="lblfecha" />
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary">
                        <uc1:UcDetalleMascaraCaptura runat="server" ID="ucDetalleMascaraCaptura" />
                </div>
                <br/>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h5>Movimientos Estatus</h5>
                    </div>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="gvEstatus" CssClass="table table-bordered " AutoGenerateColumns="False">
                            <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#ffffcc" />
                            <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                            <PagerTemplate>
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-lg-1" style="text-align: right;">
                                        <h5>
                                            <asp:Label ID="MessageLabel" Text="Ir a la pág." runat="server" /></h5>
                                    </div>
                                    <div class="col-lg-1" style="text-align: left;">
                                        <asp:DropDownList ID="PageDropDownList" Width="50px" AutoPostBack="true" runat="server" CssClass="form-control" /></h3>
                                    </div>
                                    <div class="col-lg-10" style="text-align: right;">
                                        <h3>
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h3>
                                    </div>
                                </div>
                            </PagerTemplate>
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Estatus Ticket" InsertVisible="False" ReadOnly="True" SortExpression="Descripcion" ControlStyle-Width="70px" />
                                <asp:BoundField DataField="UsuarioMovimiento" HeaderText="Usuario Movimiento" InsertVisible="False" ReadOnly="True" SortExpression="UsuarioMovimiento" ControlStyle-Width="70px" />
                                <asp:BoundField DataField="Comentarios" HeaderText="Comentario" InsertVisible="False" ReadOnly="True" SortExpression="UsuarioMovimiento" ControlStyle-Width="70px" />
                                <asp:BoundField DataField="FechaMovimiento" HeaderText="Fecha Movimiento" ReadOnly="True" SortExpression="FechaMovimiento" ControlStyle-Width="300px" />
                            </Columns>
                        </asp:GridView>
                        <%--Paginador...--%>
                    </div>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h5>Movimientos Asignaciones</h5>
                    </div>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="gvAsignaciones" CssClass="table table-bordered" AutoGenerateColumns="False">
                            <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#ffffcc" />
                            <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                            <PagerTemplate>
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-lg-1" style="text-align: right;">
                                        <h5>
                                            <asp:Label ID="MessageLabel" Text="Ir a la pág." runat="server" /></h5>
                                    </div>
                                    <div class="col-lg-1" style="text-align: left;">
                                        <asp:DropDownList ID="PageDropDownList" Width="50px" AutoPostBack="true" runat="server" CssClass="form-control" /></h3>
                                    </div>
                                    <div class="col-lg-10" style="text-align: right;">
                                        <h3>
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h3>
                                    </div>
                                </div>
                            </PagerTemplate>
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Estatus Asignacion" InsertVisible="False" ReadOnly="True" SortExpression="Descripcion" ControlStyle-Width="70px" />
                                <asp:BoundField DataField="UsuarioAsigno" HeaderText="Usuario Asigno" InsertVisible="False" ReadOnly="True" SortExpression="UsuarioAsigno" ControlStyle-Width="70px" />
                                <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado" ReadOnly="True" SortExpression="UsuarioAsignado" ControlStyle-Width="300px" />
                                <asp:BoundField DataField="FechaMovimiento" HeaderText="Fecha Movimiento" ReadOnly="True" SortExpression="FechaMovimiento" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <%--<asp:Button ID="btnCerrarOrganizacion" runat="server" CssClass="btn btn-success btn-lg" Text="Aceptar" OnClick="btnCerrarOrganizacion_OnClick" />--%>
                <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-danger btn-sm" Text="Cerrar" data-dismiss="modal" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

