<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
<!-- default file list end -->
# ASPxGridView - How to show the live data without refreshing the grid (using WebMethods)
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/t530119/)**
<!-- run online end -->


<p>This example demonstrates how to update data only for several (or all, if it is required) grid columns without refreshing the entire grid. The main idea is to create a custom DataItem template for the required columns and then update its data on the client side. The server-side values can be obtained without the grid callback by using <a href="https://msdn.microsoft.com/en-us/library/byxd99hx%28v=vs.90%29.aspx">WebMethods</a>. <br><br><em>See </em><strong><em>Implementation Details</em></strong><em> for more information. <br></em><br><strong>See also:</strong><br><a href="https://www.devexpress.com/Support/Center/p/E4326">How to display dynamic data within the ASPxGridView (Live Data) without full grid updating using the ASPxCallback control</a></p>


<h3>Description</h3>

<p>To implement this task,&nbsp;add a custom DataItem template with ASPxLabel (or another control corresponding to the column type) for the column that should be updated:</p>
<code lang="aspx">&lt;dx:GridViewDataSpinEditColumn FieldName="C2" Caption="Numeric (Live)"&gt;
    &lt;DataItemTemplate&gt;
        &lt;dx:ASPxLabel ID="labelC1" runat="server" Text='&lt;%# Eval("C2") %&gt;' OnInit="labelC1_Init"&gt;&lt;/dx:ASPxLabel&gt;
    &lt;/DataItemTemplate&gt;
&lt;/dx:GridViewDataSpinEditColumn&gt;
</code>
<p>&nbsp;Then, specify the label's ClientInstanceName property based on the row key:</p>
<code lang="cs">protected void labelC1_Init(object sender, EventArgs e) {
    ASPxLabel label = sender as ASPxLabel;
    GridViewDataItemTemplateContainer container = label.NamingContainer as GridViewDataItemTemplateContainer;
    label.ClientInstanceName = "labelC1_" + container.KeyValue;
}
</code>
<p>&nbsp;<em>Please refer to the </em><a data-ticket="K18282"><em>The general technique of using the Init/Load event handler</em></a><em>&nbsp;KB article for more information about this technique.&nbsp;</em></p>
<p><br>After that, call the server-side request to get updated values and pass them to the client side, for example, in the json format:</p>
<code lang="js">PageMethods.GetUpdatedDataFromServer(keys, onSucess, onError);</code>
<p>&nbsp;</p>
<code lang="cs">[WebMethod]
public static string GetUpdatedDataFromServer(int[] keys) {
	...
	List&lt;GridDataItem&gt; GridData = GetDataSource();
	List&lt;GridDataItem&gt; newDataRequiredForClient = new List&lt;GridDataItem&gt;();

	foreach (int keyValue in keys) {
		GridDataItem item = GridData.Find(x =&gt; x.ID == keyValue);
		newDataRequiredForClient.Add(item);
	}
	var jsonSerialiser = new JavaScriptSerializer();
	var json = jsonSerialiser.Serialize(newDataRequiredForClient);
	return json;
}</code>
<p>&nbsp;Finally, update that data on the client side or generate the error (in case of any exception):</p>
<code lang="js">function onSucess(result) {
    var items = JSON.parse(result);
    for (var i = 0; i &lt; items.length; i++) {
        var label = ASPxClientControl.GetControlCollection().GetByName("labelC1_" + items[i].ID);
        label.SetText(items[i].C2);
        ...
    }
}

function onError(result) {
    alert('Something wrong!');
}</code>

<br/>


