<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmPoliticasEstatusTicketDefault.aspx.cs" Inherits="KiiniMaster.Configuracion.Politicas.FrmPoliticasEstatusTicketDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%" ID="upGeneral">
            <ContentTemplate>
                <br>
                <h3 class="h6">
                        <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                    / Politica de Estatus Ticket Default </h3>
                <hr />
                <section class="module">
                    <div class="row">
                        <div class="col-lg-12 col-md-6">
                            <div class="module-inner">
                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="RolSolicita"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlRolSolicita" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlTipoRol_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="SubRolSolicita"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlSubRolSolicita" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlSubRol_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>

                                  <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="RolPertenece"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlRolPertenece" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlRolPertenece_SelectedIndexChanged" />
                                        </div>
                                    </div>

                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="SubRolPertenece"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlSubRolPertenece" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlSubRolPertenece_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Estatus Actual"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlEstatusActual" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlEstatusActual_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Estatus Accion"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlEstatusAccion" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlEstatusAccion_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>                               

                            </div>
                        </div>
                    </div>
                </section>

                <section class="module module-headings">
                    <div class="module-inner">
                        <div class="module-content collapse in" id="content-1">
                            <div class="module-content-inner no-padding-bottom">
                                <div class="table-responsive">
                                    <asp:Repeater ID="rptResultados" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-striped display" id="tblResults">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            <asp:Label runat="server">Id</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdRolSolicita</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdSubRolSolicita</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdRolPertenece</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdSubRolPertenece</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdEstatusTicketActual</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">IdEstatusTicketAccion</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">TieneSupervisor</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">Propietario</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">LevantaTicket</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">Habilitado</asp:Label></th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>

                                                <td><%# Eval("Id")%></td>
                                                <td><%# Eval("RolSolicita.Descripcion")%></td>
                                                <td><%# Eval("SubSolicita.Descripcion")%></td>
                                                <td><%# Eval("RolPertenece.Descripcion")%></td>
                                                <td><%# Eval("SubRolPertenece.Descripcion")%></td>
                                                <td><%# Eval("EstatusTicketActual.Descripcion")%></td>
                                                <td><%# Eval("EstatusTicketAccion.Descripcion")%></td>
                                                <td><%# Eval("TieneSupervisor")%></td>
                                                <td><%# Eval("Propietario")%></td>
                                                <td><%# Eval("LevantaTicket")%></td>

                                                <td id="colHabilitado">
                                                    <ul class="list list-unstyled" id="hiddenEnabled">
                                                        <li>
                                                            <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                        </li>
                                                    </ul>
                                                </td>
                                                <td id="colEditar">
                                                    <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                        <li>
                                                            <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Editar" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" Visible="false" />
                                                        </li>
                                                    </ul>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
            $(function () {
                hidden();
            });
        </script>
    </div>
</asp:Content>
