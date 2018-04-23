Option Infer On

Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.Web.Data
Imports DevExpress.Web
Imports System.Web.Services
Imports System.Web
Imports System.Web.Script.Serialization

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub labelC1_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim label As ASPxLabel = TryCast(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = TryCast(label.NamingContainer, GridViewDataItemTemplateContainer)
        label.ClientInstanceName = "labelC1_" & container.KeyValue
    End Sub
    Protected Sub cbC4_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim cb As ASPxCheckBox = TryCast(sender, ASPxCheckBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(cb.NamingContainer, GridViewDataItemTemplateContainer)
        cb.ClientInstanceName = "cbC4_" & container.KeyValue
    End Sub

    <WebMethod> _
    Public Shared Function GetUpdatedDataFromServer(ByVal keys() As Integer) As String

        ChangeAllColumnDataRandomly()
        Dim GridData As List(Of GridDataItem) = GetDataSource()
        Dim newDataRequiredForClient As New List(Of GridDataItem)()

        For Each keyValue As Integer In keys
            Dim item As GridDataItem = GridData.Find(Function(x) x.ID = keyValue)
            newDataRequiredForClient.Add(item)
        Next keyValue
        Dim jsonSerialiser = New JavaScriptSerializer()
        Dim json = jsonSerialiser.Serialize(newDataRequiredForClient)
        Return json
    End Function
    Public Shared Sub ChangeAllColumnDataRandomly()
        Dim GridData As List(Of GridDataItem) = GetDataSource()
        Dim random As New Random()
        Dim randomNumber As Integer
        For i As Integer = 0 To GridData.Count - 1
            randomNumber = random.Next(0, 200)
            GridData(i).C2 = randomNumber
            GridData(i).C4 = randomNumber Mod 4 <> 0
        Next i
    End Sub
    Public Shared Function GetDataSource() As List(Of GridDataItem)
        Dim key = "34FAA431-CF79-4869-9488-93F6AAE81263"
        Dim GridData As List(Of GridDataItem) = DirectCast(HttpContext.Current.Session(key), List(Of GridDataItem))
        Return GridData
    End Function

    #Region "Standard data"
    Protected ReadOnly Property GridData() As List(Of GridDataItem)
        Get
            Dim key = "34FAA431-CF79-4869-9488-93F6AAE81263"
            If Not IsPostBack OrElse Session(key) Is Nothing Then
                Session(key) = Enumerable.Range(0, 100).Select(Function(i) New GridDataItem With { _
                    .ID = i, _
                    .C1 = i Mod 2, _
                    .C2 = i * (i Mod 3), _
                    .C3 = "C3 " & i, _
                    .C4 = i Mod 4 <> 0, _
                    .C5 = New Date(2013 + i, 12, 16) _
                }).ToList()
            End If
            Return DirectCast(Session(key), List(Of GridDataItem))
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gridView.DataSource = GridData
        gridView.DataBind()
    End Sub
    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
        InsertNewItem(e.NewValues)
        CancelEditing(e)
    End Sub
    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
        UpdateItem(e.Keys, e.NewValues)
        CancelEditing(e)
    End Sub
    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)
        DeleteItem(e.Keys, e.Values)
        CancelEditing(e)
    End Sub
    Protected Function InsertNewItem(ByVal newValues As OrderedDictionary) As GridDataItem
        Dim item = New GridDataItem() With {.ID = GridData.Count}
        LoadNewValues(item, newValues)
        GridData.Add(item)
        Return item
    End Function
    Protected Function UpdateItem(ByVal keys As OrderedDictionary, ByVal newValues As OrderedDictionary) As GridDataItem

        Dim id_Renamed = Convert.ToInt32(keys("ID"))
        Dim item = GridData.First(Function(i) i.ID = id_Renamed)
        LoadNewValues(item, newValues)
        Return item
    End Function
    Protected Function DeleteItem(ByVal keys As OrderedDictionary, ByVal values As OrderedDictionary) As GridDataItem

        Dim id_Renamed = Convert.ToInt32(keys("ID"))
        Dim item = GridData.First(Function(i) i.ID = id_Renamed)
        GridData.Remove(item)
        Return item
    End Function
    Protected Sub LoadNewValues(ByVal item As GridDataItem, ByVal values As OrderedDictionary)
        item.C1 = Convert.ToInt32(values("C1"))
        item.C2 = Convert.ToDouble(values("C2"))
        item.C3 = Convert.ToString(values("C3"))
        item.C4 = Convert.ToBoolean(values("C4"))
        item.C5 = Convert.ToDateTime(values("C5"))
    End Sub
    Protected Sub CancelEditing(ByVal e As CancelEventArgs)
        e.Cancel = True
        gridView.CancelEdit()
    End Sub
    Public Class GridDataItem
        Public Property ID() As Integer
        Public Property C1() As Integer
        Public Property C2() As Double
        Public Property C3() As String
        Public Property C4() As Boolean
        Public Property C5() As Date
    End Class
    #End Region

End Class