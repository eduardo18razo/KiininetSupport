<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcRolGrupo.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcRolGrupo" %>
<asp:UpdatePanel ID="upCatlogos" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario" />
        <asp:HiddenField runat="server" ID="hfAsignacionAutomatica" />
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <asp:LinkButton CssClass="close" runat="server" ID="btnClose" OnClick="btnClose_OnClick" Text='&times' />
                    <h6 class="modal-title" id="modal-new-ticket-label">
                        <asp:Label runat="server" ID="lblTitleCatalogo" Text="Agregar rol y grupos" />
                    </h6>
                </div>

                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="ddlRol" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlRol_OnSelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="padding-2-left">
                            <div><%--class="table-responsive"--%>
                                <asp:Repeater runat="server" ID="rptGruposSub"  OnItemDataBound="rptGruposSub_OnItemDataBound">
                                    <HeaderTemplate>
                                        <h3 class="no-margin-top">Selecciona grupos</h3>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="col-lg-12 col-md-12 lblChkBold">
                                            <asp:Label runat="server" ID="lblIdGrupo" Text='<%# Eval("Id") %>' Visible="False" />
                                            <asp:Label runat="server" ID="lblTipoGrupo" Text='<%# Eval("IdTipoGrupo") %>' Visible="False" />
                                            <asp:CheckBox runat="server" ID="chkGrupo" Text='<%# Eval("Descripcion") %>' OnCheckedChanged="chkGrupo_OnCheckedChanged" AutoPostBack="True" Width="98%" />
                                            <div runat="server" id="divSubGrupos" visible="False">
                                                <asp:Repeater runat="server" ID="rptSubGrupos">
                                                    <HeaderTemplate>
                                                        <br />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblIdSubGpo" Text='<%# Eval("Id") %>' Visible="False"></asp:Label>
                                                        <asp:CheckBox runat="server" ID="chkSubGrupo" Text='<%# Eval("Descripcion") %>' CssClass="col-lg-3 col-md-3" Enabled="False" Width="30%" />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <hr class="col-lg-12 col-md-12 no-margin-bottom" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <p class="text-right">
                            <asp:Button CssClass="btn btn-primary margin-right-4" ID="btnTerminar" runat="server" Text="Seleccionar" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
