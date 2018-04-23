<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script type="text/javascript">
        function OnClientEntryAdding(sender, args) {
            var path = args.get_entry().get_fullPath();
            var s = path.split("/");
            if (s.length == 1) {
                args.set_cancel(true);
            }

            else {
                sender.closeDropDown();
            }
        }
    </script>--%>
</head>
<body>
    <div id="full">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true" />
            <tc:RadEditor RenderMode="Lightweight" runat="server" ID="RadEditor1" Width="800px">
                <tools>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="PageProperties" Text="Page Properties"></tc:EditorTool>
                </tc:EditorToolGroup>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="Bold"></tc:EditorTool>
                    <tc:EditorTool Name="Italic"></tc:EditorTool>
                    <tc:EditorTool Name="Underline"></tc:EditorTool>
                    <tc:EditorTool Name="Cut"></tc:EditorTool>
                    <tc:EditorTool Name="Copy"></tc:EditorTool>
                    <tc:EditorTool Name="Paste"></tc:EditorTool>
                    <tc:EditorTool Name="FontName"></tc:EditorTool>
                    <tc:EditorTool Name="RealFontSize"></tc:EditorTool>
                </tc:EditorToolGroup>
                <tc:EditorToolGroup>
                    <tc:EditorTool Name="InsertTable"></tc:EditorTool>
                    <tc:EditorTool Name="InsertImage"></tc:EditorTool>
                    <tc:EditorTool Name="LinkManager"></tc:EditorTool>
                    <tc:EditorTool Name="Unlink"></tc:EditorTool>
                    <tc:EditorTool Name="InsertOrderedList"></tc:EditorTool>
                    <tc:EditorTool Name=""/>
                    <tc:EditorTool Name="InsertUnorderedList"></tc:EditorTool>
                </tc:EditorToolGroup>
            </tools>
                <imagemanager viewpaths="~/Editor/images/UserDir/Marketing,~/Editor/images/UserDir/PublicRelations"></imagemanager>
                <content>
            </content>
            </tc:RadEditor>
            <%--<asp:RadioButton runat="server" Text="Quiero recibir un correo con un enlace" />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:TextBox
                        ID="txtComments"
                        TextMode="MultiLine"
                        Columns="50"
                        Rows="8"
                        runat="server" />--%>
            <%--OnImageUploadComplete=" txtDescricao_HtmlEditorExtender_ImageUploadComplete"--%>
            <%--<ajax:HtmlEditorExtender ID="txtDescricao_HtmlEditorExtender" runat="server" DisplaySourceTab="True"
                        Enabled="True" TargetControlID="txtComments" >
                        <Toolbar>
                            <ajax:Bold />
                            <ajax:Italic />
                            <ajax:InsertImage />
                        </Toolbar>
                    </ajax:HtmlEditorExtender>

                    <ajax:Editor
                        ID="Editor1"
                        Width="450px"
                        Height="200px"
                        runat="server">
                    </ajax:Editor>
                    <br />
                    <asp:Button
                        ID="btnSubmit"
                        Text="Submit"
                        runat="server" OnClick="btnSubmit_Click" />

                    <hr />
                    <h1>You Entered:</h1>
                    <hr />

                    <asp:Label
                        ID="lblComment"
                        runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <%--<asp:ListBox runat="server">
                <Items>
                    <asp:ListItem Text="Red" Value="#FF0000" Selected="True" />
                    <asp:ListItem Text="Blue" Value="#0000FF" />
                    <asp:ListItem Text="Green" Value="#008000" />
                </Items>
            </asp:ListBox>
            <tc:RadGrid runat="server" ID="gvTest" EnableHeaderContextFilterMenu="true" FilterType="HeaderContext" EnableHeaderContextMenu="true">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <tc:GridBoundColumn DataField="Id" FilterControlAltText="Filter ContactName column" HeaderText="Id" SortExpression="Id" UniqueName="Id" AutoPostBackOnFilter="true" EnableHeaderContextMenu="False">
                        </tc:GridBoundColumn>
                        <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Descripcion" FilterControlAltText="Filter ContactName column" HeaderText="Descripcion" SortExpression="Descripcion" UniqueName="Descripcion" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                        </tc:GridBoundColumn>
                        <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Sistema" FilterControlAltText="Filter ContactName column" HeaderText="Sistema" SortExpression="Sistema" UniqueName="Sistema" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                        </tc:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </tc:RadGrid>--%>
        </form>
    </div>
</body>
</html>
