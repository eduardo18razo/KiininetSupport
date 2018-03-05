<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleGrupoUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleGrupoUsuario" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
   
        <asp:Repeater runat="server" ID="rptRoles" OnItemDataBound="rptRoles_OnItemDataBound">
            <ItemTemplate>
                <div class="row col-lg-12 col-md-12">
                    <div runat="server" id="divRolesGrupos">
                        <span><%# Eval("DescripcionRol") %></span>
                        <div class="row">
                            <asp:Repeater runat="server" ID="rptGrupos">
                                <ItemTemplate>
                                    <div class="form-group col-lg-2 col-md-2 col-sm-2" style="padding: 5px">
                                        <span class="tag label label-info">
                                            <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 5px">
                                                <span><%# Eval("DescripcionGrupo") %></span>
                                                <asp:Repeater runat="server" ID="rptSubGrupos">
                                                    <HeaderTemplate>
                                                        <br />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# Eval("Descripcion") %>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <hr />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>



    </ContentTemplate>
</asp:UpdatePanel>
