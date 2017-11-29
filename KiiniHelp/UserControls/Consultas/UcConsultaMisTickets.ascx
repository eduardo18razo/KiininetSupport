<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaMisTickets.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaMisTickets" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta" />

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item active">Mis Tickets</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Mis Tickets"/></h3>
                            </div>
                            <p>
                                Texto Mis Tickets
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-5 col-md-5">
                            Consulta de Tickets:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 separador-vertical-derecho">
                            ... o consulta por estatus<br />                           
                            <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="form-control" Width="190px" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_OnSelectedIndexChanged" />                          
                        </div>


                        <div class="col-lg-3 col-md-3 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Crear Nuevo Ticket" OnClick="btnNew_OnClick" />
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

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false"
                                    CssClass="table table-striped display" Width="99%"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                    BorderStyle="None" PagerSettings-Mode="Numeric"
                                    PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Ticket" ControlStyle-Width="10%">
                                            <ItemTemplate>
                                                <div style="min-height: 30px;">
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
                                                <div style='<%# " width: 50%; background-color: " + Eval("Estatusticket.Color") %>' class="text-center">
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
            <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <uc1:UcCambiarEstatusTicket runat="server" ID="ucCambiarEstatusTicket" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>

</div>
