using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DevExpress.Web.Data;
using DevExpress.Web;
using System.Web.Services;
using System.Web;
using System.Web.Script.Serialization;

public partial class _Default: System.Web.UI.Page {

    protected void labelC1_Init(object sender, EventArgs e) {
        ASPxLabel label = sender as ASPxLabel;
        GridViewDataItemTemplateContainer container = label.NamingContainer as GridViewDataItemTemplateContainer;
        label.ClientInstanceName = "labelC1_" + container.KeyValue;
    }
    protected void cbC4_Init(object sender, EventArgs e) {
        ASPxCheckBox cb = sender as ASPxCheckBox;
        GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;
        cb.ClientInstanceName = "cbC4_" + container.KeyValue;
    }

    [WebMethod]
    public static string GetUpdatedDataFromServer(int[] keys) {

        ChangeAllColumnDataRandomly();
        List<GridDataItem> GridData = GetDataSource();
        List<GridDataItem> newDataRequiredForClient = new List<GridDataItem>();

        foreach (int keyValue in keys) {
            GridDataItem item = GridData.Find(x => x.ID == keyValue);
            newDataRequiredForClient.Add(item);
        }
        var jsonSerialiser = new JavaScriptSerializer();
        var json = jsonSerialiser.Serialize(newDataRequiredForClient);
        return json;
    }
    public static void ChangeAllColumnDataRandomly() {
        List<GridDataItem> GridData = GetDataSource();
        Random random = new Random();
        int randomNumber;
        for (int i = 0; i < GridData.Count; i++) {
            randomNumber = random.Next(0, 200);
            GridData[i].C2 = randomNumber;
            GridData[i].C4 = randomNumber % 4 != 0;
        }
    }
    public static List<GridDataItem> GetDataSource() {
        var key = "34FAA431-CF79-4869-9488-93F6AAE81263";
        List<GridDataItem> GridData = (List<GridDataItem>)HttpContext.Current.Session[key];
        return GridData;
    }

    #region Standard data
    protected List<GridDataItem> GridData
    {
        get
        {
            var key = "34FAA431-CF79-4869-9488-93F6AAE81263";
            if (!IsPostBack || Session[key] == null)
                Session[key] = Enumerable.Range(0, 100).Select(i => new GridDataItem
                {
                    ID = i,
                    C1 = i % 2,
                    C2 = i * (i % 3),
                    C3 = "C3 " + i,
                    C4 = i % 4 != 0,
                    C5 = new DateTime(2013 + i, 12, 16)
                }).ToList();
            return (List<GridDataItem>)Session[key];
        }
    }
    protected void Page_Load(object sender, EventArgs e) {
        gridView.DataSource = GridData;
        gridView.DataBind();
    }
    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e) {
        InsertNewItem(e.NewValues);
        CancelEditing(e);
    }
    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e) {
        UpdateItem(e.Keys, e.NewValues);
        CancelEditing(e);
    }
    protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e) {
        DeleteItem(e.Keys, e.Values);
        CancelEditing(e);
    }
    protected GridDataItem InsertNewItem(OrderedDictionary newValues) {
        var item = new GridDataItem() { ID = GridData.Count };
        LoadNewValues(item, newValues);
        GridData.Add(item);
        return item;
    }
    protected GridDataItem UpdateItem(OrderedDictionary keys, OrderedDictionary newValues) {
        var id = Convert.ToInt32(keys["ID"]);
        var item = GridData.First(i => i.ID == id);
        LoadNewValues(item, newValues);
        return item;
    }
    protected GridDataItem DeleteItem(OrderedDictionary keys, OrderedDictionary values) {
        var id = Convert.ToInt32(keys["ID"]);
        var item = GridData.First(i => i.ID == id);
        GridData.Remove(item);
        return item;
    }
    protected void LoadNewValues(GridDataItem item, OrderedDictionary values) {
        item.C1 = Convert.ToInt32(values["C1"]);
        item.C2 = Convert.ToDouble(values["C2"]);
        item.C3 = Convert.ToString(values["C3"]);
        item.C4 = Convert.ToBoolean(values["C4"]);
        item.C5 = Convert.ToDateTime(values["C5"]);
    }
    protected void CancelEditing(CancelEventArgs e) {
        e.Cancel = true;
        gridView.CancelEdit();
    }
    public class GridDataItem {
        public int ID { get; set; }
        public int C1 { get; set; }
        public double C2 { get; set; }
        public string C3 { get; set; }
        public bool C4 { get; set; }
        public DateTime C5 { get; set; }
    }
    #endregion

}