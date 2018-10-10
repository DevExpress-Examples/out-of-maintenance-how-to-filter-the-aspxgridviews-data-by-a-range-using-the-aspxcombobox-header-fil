Imports DevExpress.Data.Filtering
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxGridView
Imports System
Imports System.Data

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private Const maxPower As Integer = 8
    Private dataTable As DataTable = CreateDataSourse()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        grid.DataBind()
    End Sub
    Protected Sub grid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        TryCast(sender, ASPxGridView).DataSource = dataTable
    End Sub

    Protected Sub grid_AutoFilterCellEditorCreate(ByVal sender As Object, ByVal e As ASPxGridViewEditorCreateEventArgs)
        If e.Column.FieldName <> "Value" Then
            Return
        End If

        Dim combo As New ComboBoxProperties()
        combo.EnableCallbackMode = True
        combo.CallbackPageSize = 10
        combo.IncrementalFilteringMode = IncrementalFilteringMode.Contains
        e.EditorProperties = combo
    End Sub
    Protected Sub grid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As ASPxGridViewAutoFilterEventArgs)
        If e.Column.FieldName <> "Value" Then
            Return
        End If

        If e.Kind = GridViewAutoFilterEventKind.CreateCriteria Then
            Dim integerValue As Integer = Nothing

            If Int32.TryParse(e.Value, integerValue) Then
                e.Criteria = (New OperandProperty("Value") >= integerValue) And (New OperandProperty("Value") < GetUpperLimit(integerValue))
            Else
                e.Criteria = Nothing
            End If
        Else
           If e.Criteria IsNot Nothing Then
                Dim groupOperator As GroupOperator = TryCast(e.Criteria, GroupOperator)

                e.Value = (TryCast((TryCast(groupOperator.Operands(0), BinaryOperator)).RightOperand, ConstantValue)).ToString() & " - " & (TryCast((TryCast(groupOperator.Operands(1), BinaryOperator)).RightOperand, ConstantValue)).ToString()
           End If
        End If
    End Sub
    Protected Sub grid_AutoFilterCellEditorInitialize(ByVal sender As Object, ByVal e As ASPxGridViewEditorEventArgs)
        Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
        combo.Items.Add("0 - 10", "0")
        For i As Integer = 1 To maxPower
            For j As Integer = 1 To 9
                Dim value As Integer = j * CInt((Math.Pow(10, i)))
                combo.Items.Add(String.Format("{0} - {1}", value, GetUpperLimit(value)), value.ToString())
            Next j
        Next i
    End Sub
    Private Function GetUpperLimit(ByVal lowerValue As Integer) As Integer
        If lowerValue = 0 Then
            Return 10
        End If
        Dim remainder As Integer = lowerValue Mod 10
        Dim value As Integer = CInt(lowerValue \ 10)
        Dim pow As Integer = 0
        Do While value <> 0
            remainder = value Mod 10
            value = CInt(value \ 10)
            pow += 1
        Loop
        remainder += 1
        Return remainder * CInt((Math.Pow(10, pow)))
    End Function
    Private Shared Function CreateDataSourse() As DataTable

        Dim dt As New DataTable("dataTable")
        dt.Columns.Add("Id", GetType(Int32))
        dt.Columns.Add("Value", GetType(Int32))


        Dim id_Renamed As Integer = 0
        Dim value As Integer = 0

        For i As Integer = 1 To maxPower
            For j As Integer = 0 To 9
                If (i > 1) AndAlso (j = 0) Then
                    Continue For
                End If
                For k As Integer = 0 To 4
                    value = j * CInt((Math.Pow(10, i))) + k
                    dt.Rows.Add(id_Renamed, value)
                    id_Renamed += 1
                Next k

            Next j
        Next i
        Return dt
    End Function
End Class