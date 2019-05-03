<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMasterDetail.aspx.cs" Inherits="KiiniHelp.Test.FrmMasterDetail" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        
        <iframe src="http://facebook.com" runat="server" ID="iframes" height="450" width="450"></iframe>
        <tc:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" ShowStatusBar="true" AutoGenerateColumns="False"
            PageSize="7" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True"
            OnDetailTableDataBind="RadGrid1_DetailTableDataBind" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnPreRender="RadGrid1_PreRender">
            <PagerStyle Mode="NumericPages"></PagerStyle>
            <MasterTableView DataKeyNames="Id, IdArea, IdNivel1, IdNivel2, IdNivel3, IdNivel4, IdNivel5, IdNivel6, IdNivel7" AllowMultiColumnSorting="True">
                <DetailTables>
                    <tc:GridTableView DataKeyNames="Id" Name="Opcion" Width="100%">
                        <Columns>
                            <tc:GridBoundColumn SortExpression="Tipificacion" HeaderText="Tipificacion" HeaderButtonType="TextButton"
                                DataField="Tipificacion">
                            </tc:GridBoundColumn>
                        </Columns>
                    </tc:GridTableView>
                </DetailTables>
                <Columns>
                    <tc:GridBoundColumn SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton"
                        DataField="Id">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="TipoUsuario.Descripcion" HeaderText="Tipo Usuario" HeaderButtonType="TextButton"
                        DataField="TipoUsuario.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Area.Descripcion" HeaderText="Categoria" HeaderButtonType="TextButton"
                        DataField="Area.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel1.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel1.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel2.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel2.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel3.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel3.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel4.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel4.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel5.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel5.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel6.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel6.Descripcion">
                    </tc:GridBoundColumn>
                    <tc:GridBoundColumn SortExpression="Nivel7.Descripcion" HeaderText="Seccion 1" HeaderButtonType="TextButton"
                        DataField="Nivel7.Descripcion">
                    </tc:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </tc:RadGrid>
    </div>
    </form>
</body>
</html>
