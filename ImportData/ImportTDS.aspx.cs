using System;
using System.Collections;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class SalaryModule_ImportTDS : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;

    protected void page_Init()
    {
        constr = ConfigurationManager.ConnectionStrings["myconnectionstring"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        da = new SqlDataAdapter();
        cmd = new SqlCommand();
        hash = new Hashtable();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User"] != null)
            {
                hash = (Hashtable)Session["User"];
                if (!IsPostBack)
                {
                    CheckUserRights();
                }
            }
            else
            {
                Response.Redirect("../Default.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public enum MenuType
    {
        All = 0,
        SetUp = 1,
        ImportData = 2,
        Actions = 3,
        Reports = 4
    }

    public void CheckUserRights()
    {
        try
        {
            int HasMatch = 0;
            string RequestURL = Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(RequestURL);
            string PageName = oInfo.Name;
            string CheckPageName = "";

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetLoginDetails", con);
            cmd.Parameters.AddWithValue("@UserName", null);
            cmd.Parameters.AddWithValue("@Password", null);
            cmd.Parameters.AddWithValue("@LoginID", Session["LoginID"]);
            cmd.Parameters.AddWithValue("@MenuID", MenuType.All);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            if (ds.Tables[1].Rows.Count > 0)
            {
                int i = 0;

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    CheckPageName = ds.Tables[1].Rows[i]["PageName"].ToString();
                    if (PageName == CheckPageName)
                    {
                        HasMatch++;
                        break;
                    }

                    i++;
                }

                if (HasMatch > 0)
                {
                    Month();
                    Year();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Month();
                Year();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Month()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowMonth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlMonth.DataSource = dt;
            ddlMonth.DataTextField = "MonthName";
            ddlMonth.DataValueField = "MonthID";
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new ListItem("Select Month", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Year()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageYears", con);
            cmd.Parameters.AddWithValue("@Year", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlYear.DataSource = dt;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "YearID";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Clear()
    {
        try
        {
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    private void Import_To_Grid(string FilePath, string Extension)
    {
        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls":
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, 1);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            int i = 0;
            int HasRow = 0;
            foreach (DataRow row in dt.Rows)
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                string TDSValue = dt.Rows[i]["TDS Value"].ToString();
                string ActualTDS = dt.Rows[i]["Actual TDS"].ToString();
                string TotalTax = dt.Rows[i]["Total Tax"].ToString();
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("SaveImportTDS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", dt.Rows[i]["ProfileID"].ToString());
                cmd.Parameters.AddWithValue("@EmployeeID", dt.Rows[i]["EmployeeID"].ToString());
                cmd.Parameters.AddWithValue("@Emp_Code", dt.Rows[i]["Emp_Code"].ToString());
                cmd.Parameters.AddWithValue("@SystemNumber", dt.Rows[i]["SystemNumber"].ToString());
                cmd.Parameters.AddWithValue("@AssignEmpCode", dt.Rows[i]["AssignEmpCode"].ToString());
                cmd.Parameters.AddWithValue("@Name", dt.Rows[i]["Name"].ToString());
                cmd.Parameters.AddWithValue("@Designation", dt.Rows[i]["Designation"].ToString());
                cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@Month", dt.Rows[i]["Month"].ToString());
                cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@Year", dt.Rows[i]["Year"].ToString());

                if (TotalTax == "")
                {
                    cmd.Parameters.AddWithValue("@TotalTax", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TotalTax", TotalTax);
                }
                if (ActualTDS == "")
                {
                    cmd.Parameters.AddWithValue("@ActualTDS", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ActualTDS", ActualTDS);
                }
                if (TDSValue == "")
                {
                    cmd.Parameters.AddWithValue("@TDSValue", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TDSValue", TDSValue);
                }

                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                con.Open();
                HasRow = HasRow + cmd.ExecuteNonQuery();
                con.Close();
                i++;
            }

            Clear();

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + HasRow + " Record Saved Sucessfully.');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public enum Status
    {
        Active = 1,
        Deactive = 0
    }

    private void ExportGridToExcel()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Fill_TDS_" + ddlMonth.SelectedItem.Text.Substring(0, 3) + "_" + ddlYear.SelectedItem.Text + "_FDB.xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;
                int Count = 0;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    if (dtcol.ColumnName == "ProfileID" || dtcol.ColumnName == "EmployeeID" || dtcol.ColumnName == "Emp_Code" || dtcol.ColumnName == "SystemNumber" || dtcol.ColumnName == "AssignEmpCode" || dtcol.ColumnName == "Name" || dtcol.ColumnName == "Designation" || dtcol.ColumnName == "DOB")
                    {
                        Response.Write(str + dtcol.ColumnName);
                        str = "\t";
                        Count++;
                    }
                    else
                    {
                        if (Count == 8)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Response.Write(str + "Month");
                Response.Write(str + "Year");
                Response.Write(str + "Total Tax");
                Response.Write(str + "Actual TDS");
                Response.Write(str + "TDS Value");
                str = "\t";
                Response.Write("\n");
                int Row = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 0 || j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 8 || j == 6)
                        {
                            Response.Write(str + Convert.ToString(dr[j]));
                            str = "\t";
                        }
                    }
                    Response.Write(str + ddlMonth.SelectedItem.Text);
                    Response.Write(str + ddlYear.SelectedItem.Text);
                    con = new SqlConnection(constr);
                    cmd = new SqlCommand("GetImportDataDetails", con);
                    cmd.Parameters.AddWithValue("@ProfileID", dt.Rows[Row]["ProfileID"].ToString());
                    cmd.Parameters.AddWithValue("@AssignEmpCode", dt.Rows[Row]["AssignEmpCode"].ToString());
                    cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    DataSet ds = new DataSet();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        Response.Write(str + ds.Tables[4].Rows[0]["TotalTax"].ToString());
                        Response.Write(str + ds.Tables[4].Rows[0]["ActualTDS"].ToString());
                        Response.Write(str + ds.Tables[4].Rows[0]["TDSValue"].ToString());
                    }
                    str = "\t";
                    Response.Write("\n");
                    Row++;
                }

                Response.End();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found in Profile.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            ExportGridToExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            if (flTDSImport.HasFile)
            {
                string FileName = ddlMonth.SelectedValue + ddlYear.SelectedItem.Text + "_" + Path.GetFileName(flTDSImport.PostedFile.FileName);
                string Extension = Path.GetExtension(flTDSImport.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = Server.MapPath(FolderPath + FileName);
                flTDSImport.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select File.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}