<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaMisTickets.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaMisTickets" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>

<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>

<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta" />

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="active">Mis Tickets</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Mis Tickets" /></h3>
                            </div>
                        </div>


                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="col-xs-12 col-sm-12 col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta de Tickets:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-3 col-lg-3 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o consulta por estatus</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="form-control no-padding-left no-margin-left" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlEstatus_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4 text-center margin-top-btn-consulta">
                                <a data-toggle="modal" role="menuitem" data-keyboard="false" data-target="#modal-new-ticket">
                                    <label class="btn btn-success"><i class="fa fa-plus"></i>Nuevo</label>
                                </a>
                            </div>
                        </div>
                    </div>
            </section>

            <section class="module">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%" ShowHeaderWhenEmpty="True"
                                    CssClass="table table-striped display alineaTablaIzquierda" OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                    <EmptyDataTemplate>
                                        <h3>Sin informacion Disponible</h3>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Ticket" HeaderStyle-Width="10%" ItemStyle-CssClass="altoFijo">
                                            <ItemTemplate>
                                                <div>
                                                    <label><%# Eval("IdTicket")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asunto" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <label><%# Eval("Tipificacion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Solicitado" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <label><%# Eval("FechaHora" , "{0:dd/MMM/yyyy}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <div class="btn btnBandejaCliente" style='<%# "background-color: " + Eval("Estatusticket.Color") %>'>
                                                    <asp:Label runat="server" Text='<%# Eval("Estatusticket.Descripcion")%>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Button runat="server" data-tieneEncuesta='<%# Eval("TieneEncuesta")%>' Text="Estatus" ID="btnCambiaEstatus" CssClass="btn btn-primary" OnClick="btnCambiaEstatus_OnClick" CommandArgument='<%# Eval("IdTicket")%>' CommandName='<%# Eval("EstatusTicket.Id") %>'
                                                    Visible='<%# int.Parse(Eval("Estatusticket.Id").ToString()) == (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </section>



        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalCambiaEstatus" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCambiaestatus" runat="server">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" ID="ucCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



</div>











