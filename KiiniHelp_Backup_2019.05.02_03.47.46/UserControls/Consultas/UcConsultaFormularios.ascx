<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaFormularios.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaFormularios" %>


<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Centro de Soporte</li>
                <li class="active">Formularios</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Formularios" /></h3>
                            </div>
                            <p>
                                Los formularios se conforman por campos específicos y estructurados que deberán ser capturados por los usuarios para crear un evento.
                            </p>
                        </div>


                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">


                            <div class="col-lg-6 col-md-6 col-sm-5 col-xs-12 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Formularios:</label>

                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-sm-5 col-xs-12 text-right">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div>
                                            <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick"><i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>
                                            <asp:LinkButton CssClass="btn btn-success" ID="btnNew" OnClick="btnNew_OnClick" runat="server"><i class="fa fa-plus"></i>Nuevo</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-2 col-sm-2 col-xs-12 text-right">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Ver solo Activos:</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-right">
                                        <div>

                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 margin-top-3 text-right text-hide">
                                                <asp:CheckBox runat="server" ID="chkActivos" Text="Si" OnCheckedChanged="chkActivos_OnCheckedChanged" AutoPostBack="True" Checked="True" CssClass="chkIphone" Width="30px" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </section>


            <section class="module">
                <div class="module-inner">

                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%" AllowSorting="True"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25" OnSorting="tblResults_OnSorting"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Titulo" HeaderStyle-Width="30%" ItemStyle-CssClass="altoFijo" SortExpression="Descripcion">
                                            <ItemTemplate>
                                                <div>
                                                    <label runat="server" class="ocultaTexto" title='<%# Eval("Descripcion")%>'><%# Eval("Descripcion")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Formulario" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("NombreTabla")%>'><%# Eval("NombreTabla")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Creación" HeaderStyle-Width="14%" SortExpression="FechaAlta">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("FechaAlta")%>'><%# Eval("FechaAlta")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Últ. Edición" HeaderStyle-Width="14%" SortExpression="FechaModificacion">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("FechaModificacion")%>'><%# Eval("FechaModificacion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Preview" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' ID="btnPreview" Text="Ver" OnClick="btnPreview_OnClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Clonar" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenClonar">
                                                    <li>
                                                        <asp:LinkButton runat="server" Text="Clonar" CommandArgument='<%# Eval("Id")%>' Visible='<%# !(bool) Eval("Sistema") %>' OnClick="OnClick"></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                    <li>
                                                        <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' Visible='<%# !(bool) Eval("Sistema") && (bool) Eval("Habilitado")%>' OnClick="btnEditar_OnClick"><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# !(bool)Eval("Sistema") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </section>



            <script type="text/javascript">
                $(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
</div>

