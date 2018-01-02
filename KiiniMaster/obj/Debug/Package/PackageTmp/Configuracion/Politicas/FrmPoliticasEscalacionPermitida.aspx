<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmPoliticasEscalacionPermitida.aspx.cs" Inherits="KiiniMaster.Configuracion.Politicas.FrmPoliticasEscalacionPermitida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%" ID="upGeneral">
            <ContentTemplate>
                <br>
                <h3 class="h6">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                    / Politicas de Escalación Permitida </h3>
                <hr />
                <section class="module">
                    <div class="row">
                        <div class="col-lg-12 col-md-6">
                            <div class="module-inner">
                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="SubRol"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlSubRol" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSubRol_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="SubRolPermitido"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlSubRolPermitido" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSubRolPermitido_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Estatus Asignacion"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlEstatusAsignacion" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlEstatusActual_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                      
                                    </div>
                                </div>
                                <br />                                
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
                                                            <asp:Label runat="server">SubRol</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">EstatusAsignacion</asp:Label></th>            
                                                        <th>
                                                            <asp:Label runat="server">SubRolPermitido</asp:Label></th>                                                                                                  
                                                        <th>
                                                            <asp:Label runat="server">Habilitado</asp:Label></th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>

                                                <td><%# Eval("Id")%></td>                                              
                                                <td><%# Eval("SubRol.Descripcion")%></td>
                                                <td><%# Eval("EstatusAsignacion.Descripcion")%></td>
                                                <td><%# Eval("SubRolPermitido.Descripcion")%></td>
                                                                                            
                                                <td id="colHabilitado">
                                                    <ul class="list list-unstyled" id="hiddenEnabled">
                                                        <li>
                                                            <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
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