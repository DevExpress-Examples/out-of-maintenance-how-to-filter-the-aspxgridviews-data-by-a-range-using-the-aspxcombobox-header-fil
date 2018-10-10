using DevExpress.Data.Filtering;
using DevExpress.Web;
using System;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    private const int maxPower = 8;
    DataTable dataTable = CreateDataSourse();

    protected void Page_Load(object sender, EventArgs e)
    {
        grid.DataBind();
    }
    protected void grid_DataBinding(object sender, EventArgs e)
    {
        (sender as ASPxGridView).DataSource = dataTable;
    }

    protected void grid_AutoFilterCellEditorCreate(object sender, ASPxGridViewEditorCreateEventArgs e)
    {
        if (e.Column.FieldName != "Value")
            return;

        ComboBoxProperties combo = new ComboBoxProperties();
        combo.EnableCallbackMode = true;
        combo.CallbackPageSize = 10;
        combo.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
        e.EditorProperties = combo;
    }
    protected void grid_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
    {
        if (e.Column.FieldName != "Value")
            return;

        if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria)
        {
            int integerValue;

            if (Int32.TryParse(e.Value, out integerValue))
                e.Criteria = (new OperandProperty("Value") >= integerValue) & (new OperandProperty("Value") < GetUpperLimit(integerValue));
            else
                e.Criteria = null;
        }
        else
        {
           if (e.Criteria != null)
            {
                GroupOperator groupOperator = e.Criteria as GroupOperator;

                e.Value = ((groupOperator.Operands[0] as BinaryOperator).RightOperand as ConstantValue).ToString() + " - " +
                          ((groupOperator.Operands[1] as BinaryOperator).RightOperand as ConstantValue).ToString();
            }
        }
    }
    protected void grid_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        ASPxComboBox combo = e.Editor as ASPxComboBox;
        combo.Items.Add("0 - 10", "0");
        for (int i = 1; i <= maxPower; i++)
            for (int j = 1; j < 10; j++)
            {
                int value = j * (int)Math.Pow(10, i);
                combo.Items.Add(String.Format("{0} - {1}", value, GetUpperLimit(value)), value.ToString());
            }
    }
    private int GetUpperLimit(int lowerValue)
    {
        if (lowerValue == 0)
            return 10;
        int remainder = lowerValue % 10;
        int value = (int)(lowerValue / 10);
        int pow = 0;
        while (value != 0)
        {
            remainder = value % 10;
            value = (int)(value / 10);
            ++pow;
        }
        return ++remainder * (int)Math.Pow(10, pow);
    }
    private static DataTable CreateDataSourse()
    {

        DataTable dt = new DataTable("dataTable");
        dt.Columns.Add("Id", typeof(Int32));
        dt.Columns.Add("Value", typeof(Int32));

        int id = 0;
        int value = 0;

        for (int i = 1; i <= maxPower; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if ((i > 1) && (j == 0))
                    continue;
                for (int k = 0; k < 5; k++)
                {
                    value = j * (int)Math.Pow(10, i) + k;
                    dt.Rows.Add(id, value);
                    ++id;
                }

            }
        }
        return dt;
    }
}