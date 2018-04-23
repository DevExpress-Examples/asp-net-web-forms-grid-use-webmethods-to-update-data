<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function RequestNewData() {
            var startIndex = gridView.GetTopVisibleIndex();
            var rowsCount = gridView.GetVisibleRowsOnPage();
            var keys = new Array(rowsCount);
            for (var i = startIndex; i < startIndex + rowsCount; i++) {
                keys[i - startIndex] = gridView.GetRowKey(i);
            }


            PageMethods.GetUpdatedDataFromServer(keys, onSucess, onError);

        }
        function onSucess(result) {
            var items = JSON.parse(result);
            for (var i = 0; i < items.length; i++) {
                var label = ASPxClientControl.GetControlCollection().GetByName("labelC1_" + items[i].ID);
                label.SetText(items[i].C2);
                var checkBox = ASPxClientControl.GetControlCollection().GetByName("cbC4_" + items[i].ID);
                checkBox.SetChecked(items[i].C4);
            }
        }

        function onError(result) {
            alert('Something wrong!');
        }
    </script>
</head>
<body>

    <form id="frmMain" runat="server">

        <asp:ScriptManager runat="server" ID="scriptManager" EnablePageMethods="true">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btUpdateData" runat="server" Text="Refresh Data" AutoPostBack="false" ClientInstanceName="btUpdateData">
                        <ClientSideEvents Click="function(s, e){
                            RequestNewData();
                            }" />
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxCheckBox ID="cbRefreshAutomatically" runat="server" Text="Refresh Automatically">
                        <ClientSideEvents CheckedChanged="function(s, e){
                                timer.SetEnabled(s.GetChecked());
                                btUpdateData.SetEnabled(!s.GetChecked());
                        }" />
                    </dx:ASPxCheckBox>
                </td>
            </tr>
        </table>
        <dx:ASPxGridView ID="gridView" runat="server" KeyFieldName="ID" ClientInstanceName="gridView"
            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting">
            <Columns>
                <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" />
                <dx:GridViewDataColumn FieldName="ID">
                </dx:GridViewDataColumn>
                <dx:GridViewDataDateColumn FieldName="C5" Caption="Date" />
                <dx:GridViewDataSpinEditColumn FieldName="C2" Caption="Numeric (Live)">
                    <DataItemTemplate>
                        <dx:ASPxLabel ID="labelC1" runat="server" Text='<%# Eval("C2") %>' OnInit="labelC1_Init"></dx:ASPxLabel>
                    </DataItemTemplate>
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataCheckColumn FieldName="C4" Caption="Boolean (Live)">
                    <DataItemTemplate>
                        <dx:ASPxCheckBox ID="cbC4" runat="server" Value='<%# Convert.ToBoolean(Eval("C4")) %>'
                            ReadOnly="true" OnInit="cbC4_Init">
                        </dx:ASPxCheckBox>
                    </DataItemTemplate>
                </dx:GridViewDataCheckColumn>
            </Columns>
        </dx:ASPxGridView>
        <dx:ASPxTimer ID="timer" runat="server" Enabled="false" Interval="2000" ClientInstanceName="timer">
            <ClientSideEvents Tick="function(s, e){ RequestNewData(); }" />
        </dx:ASPxTimer>
    </form>
</body>
</html>
