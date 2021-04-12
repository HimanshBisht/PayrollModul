using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;

public partial class SalaryModule_EmpWiseSalaryReport : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal PayDrawnBasic = 0;
    decimal Da = 0;
    decimal Hra = 0;
    decimal Transport = 0;
    decimal Medical = 0;
    decimal Washing = 0;
    decimal GrossRevisedSalary = 0;
    decimal ExGratia = 0;
    decimal ArearAdjust = 0;
    decimal GrossTotal = 0;
    decimal PF = 0;
    decimal Deduction = 0;
    decimal TdsOnBasic = 0;
    decimal Advance = 0;
    decimal TPTREC = 0;
    decimal GIS = 0;
    decimal ESI = 0;
    decimal TotalDeduction = 0;
    decimal GrossTotalSalary = 0;

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

            ddlFromMonth.DataSource = dt;
            ddlFromMonth.DataTextField = "MonthName";
            ddlFromMonth.DataValueField = "MonthID";
            ddlFromMonth.DataBind();
            ddlFromMonth.Items.Insert(0, new ListItem("Select From Month", "0"));

            ddlToMonth.DataSource = dt;
            ddlToMonth.DataTextField = "MonthName";
            ddlToMonth.DataValueField = "MonthID";
            ddlToMonth.DataBind();
            ddlToMonth.Items.Insert(0, new ListItem("Select To Month", "0"));

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

            ddlFromYear.DataSource = dt;
            ddlFromYear.DataTextField = "Year";
            ddlFromYear.DataValueField = "YearID";
            ddlFromYear.DataBind();
            ddlFromYear.Items.Insert(0, new ListItem("Select From Year", "0"));

            ddlToYear.DataSource = dt;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "YearID";
            ddlToYear.DataBind();
            ddlToYear.Items.Insert(0, new ListItem("Select To Year", "0"));

            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Bindgrid()
    {
        try
        {
            int FromMonthID = Convert.ToInt32(ddlFromMonth.SelectedValue);
            int FromYearID = Convert.ToInt32(ddlFromYear.SelectedValue);
            int ToMonthID = Convert.ToInt32(ddlToMonth.SelectedValue);
            int ToYearID = Convert.ToInt32(ddlToYear.SelectedValue);

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("EmpWiseSalaryReport", con);
            cmd.Parameters.AddWithValue("@FromMonthID", FromMonthID);
            cmd.Parameters.AddWithValue("@FromYearID", FromYearID);
            cmd.Parameters.AddWithValue("@ToMonthID", ToMonthID);
            cmd.Parameters.AddWithValue("@ToYearID", ToYearID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            grdrecord.Caption = string.Empty;

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pnlButtons.Visible = true;
                    pnlStmt.Visible = true;
                    lblSTMT.Text = Session["SchoolName"].ToString() + "<br />" + "Employee Wise Salary Statement From " + ddlFromMonth.SelectedItem.Text + " - " + ddlFromYear.SelectedItem.Text + " to " + ddlToMonth.SelectedItem.Text + " - " + ddlToYear.SelectedItem.Text;
                    pnlDetail.Visible = true;
                }
                else
                {
                    pnlDetail.Visible = false;
                    pnlButtons.Visible = false;
                    lblSTMT.Text = string.Empty;
                    pnlStmt.Visible = false;
                }
                grdrecord.DataSource = ds.Tables[0];
                grdrecord.DataBind();
                pnlTotalRecords.Visible = true;
                lblTotalRecords.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found');", true);
                grdrecord.DataSource = null;
                grdrecord.DataBind();
                lblSTMT.Text = string.Empty;
                pnlTotalRecords.Visible = false;
                lblTotalRecords.Text = "0";
                pnlButtons.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindIndividualEmpSalaryRpt()
    {
        try
        {
            int FromMonthID = Convert.ToInt32(ddlFromMonth.SelectedValue);
            int FromYearID = Convert.ToInt32(ddlFromYear.SelectedValue);
            int ToMonthID = Convert.ToInt32(ddlToMonth.SelectedValue);
            int ToYearID = Convert.ToInt32(ddlToYear.SelectedValue);

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("IndividualEmpSalaryReport", con);
            cmd.Parameters.AddWithValue("@FromMonthID", FromMonthID);
            cmd.Parameters.AddWithValue("@FromYearID", FromYearID);
            cmd.Parameters.AddWithValue("@ToMonthID", ToMonthID);
            cmd.Parameters.AddWithValue("@ToYearID", ToYearID);
            cmd.Parameters.AddWithValue("@Emp_Code", ddlemployee.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            grdIndividualEmpSalary.Caption = string.Empty;

            if (dt.Rows.Count > 0)
            {
                PayDrawnBasic = dt.AsEnumerable().Sum(row => row.Field<decimal>("PayDrawnBasic"));

                Da = dt.AsEnumerable().Sum(row => row.Field<decimal>("DA"));

                Hra = dt.AsEnumerable().Sum(row => row.Field<decimal>("HRA"));

                Transport = dt.AsEnumerable().Sum(row => row.Field<decimal>("Transport"));

                Medical = dt.AsEnumerable().Sum(row => row.Field<decimal>("Medical"));

                Washing = dt.AsEnumerable().Sum(row => row.Field<decimal>("Washing"));

                GrossRevisedSalary = dt.AsEnumerable().Sum(row => row.Field<decimal>("GrossRevisedSalary"));

                ExGratia = dt.AsEnumerable().Sum(row => row.Field<decimal>("ExGratia"));

                ArearAdjust = dt.AsEnumerable().Sum(row => row.Field<decimal>("ArearAdjust"));

                GrossTotal = dt.AsEnumerable().Sum(row => row.Field<decimal>("GrossTotal"));

                PF = dt.AsEnumerable().Sum(row => row.Field<decimal>("PF"));

                Deduction = dt.AsEnumerable().Sum(row => row.Field<decimal>("Deduction"));

                TdsOnBasic = dt.AsEnumerable().Sum(row => row.Field<decimal>("TDS"));

                Advance = dt.AsEnumerable().Sum(row => row.Field<decimal>("Advance"));

                TPTREC = dt.AsEnumerable().Sum(row => row.Field<decimal>("TPTREC"));

                GIS = dt.AsEnumerable().Sum(row => row.Field<decimal>("GIS"));

                ESI = dt.AsEnumerable().Sum(row => row.Field<decimal>("ESI"));

                TotalDeduction = dt.AsEnumerable().Sum(row => row.Field<decimal>("TotalDeduction"));

                GrossTotalSalary = dt.AsEnumerable().Sum(row => row.Field<decimal>("GrossTotalSalary"));

                pnlButtons.Visible = true;
                pnlStmt.Visible = true;
                lblSTMT.Text = Session["SchoolName"].ToString() + "<br />" + "Salary Statement of " + ddlemployee.SelectedItem.Text + " From " + ddlFromMonth.SelectedItem.Text + " - " + ddlFromYear.SelectedItem.Text + " to " + ddlToMonth.SelectedItem.Text + " - " + ddlToYear.SelectedItem.Text;
                pnlIndividualEmpSalary.Visible = true;
            }
            else
            {
                pnlIndividualEmpSalary.Visible = false;
                pnlButtons.Visible = false;
                lblSTMT.Text = string.Empty;
                pnlStmt.Visible = false;
            }

            grdIndividualEmpSalary.DataSource = dt;
            grdIndividualEmpSalary.DataBind();
            pnlTotalRecords.Visible = true;
            lblTotalRecords.Text = dt.Rows.Count.ToString();
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
            ddlFromMonth.ClearSelection();
            ddlFromYear.ClearSelection();
            ddlToMonth.ClearSelection();
            ddlToYear.ClearSelection();
            ddlReportType.ClearSelection();
            ddlEmployeeStatus.ClearSelection();
            ddlemployee.ClearSelection();
            pnlSelectEmp.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            grdIndividualEmpSalary.DataSource = null;
            grdIndividualEmpSalary.DataBind();
            lblSTMT.Text = string.Empty;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
            pnlIndividualEmpSalary.Visible = false;
            pnlTotalRecords.Visible = false;
            lblTotalRecords.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlReportType.SelectedValue == "1")
            {
                grdIndividualEmpSalary.DataSource = null;
                grdIndividualEmpSalary.DataBind();
                pnlIndividualEmpSalary.Visible = false;
                Bindgrid();
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                grdrecord.DataSource = null;
                grdrecord.DataBind();
                pnlDetail.Visible = false;
                BindIndividualEmpSalaryRpt();
            }
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

    private void ExportGridToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EmpWise_Salary_Statement" + Session["SchoolPrefix"].ToString() + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.HeaderRow.BackColor = Color.White;
                this.Bindgrid();
                foreach (TableCell cell in grdrecord.HeaderRow.Cells)
                {
                    cell.BackColor = Color.White;
                }
                foreach (GridViewRow row in grdrecord.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.BackColor = Color.White;
                        cell.ForeColor = Color.Black;
                        cell.CssClass = "textmode";
                    }
                }
                grdrecord.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
                pnlDetail.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    private void ExportIndividualEmpGridToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EmpWise_Salary_Statement" + Session["SchoolPrefix"].ToString() + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdIndividualEmpSalary.HeaderRow.BackColor = Color.White;
                this.BindIndividualEmpSalaryRpt();
                foreach (TableCell cell in grdIndividualEmpSalary.HeaderRow.Cells)
                {
                    cell.BackColor = Color.White;
                }
                foreach (GridViewRow row in grdIndividualEmpSalary.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.BackColor = Color.White;
                        cell.ForeColor = Color.Black;
                        cell.CssClass = "textmode";
                    }
                }
                grdIndividualEmpSalary.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
                pnlIndividualEmpSalary.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlReportType.SelectedValue == "1")
            {
                ExportGridToExcel();
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                ExportIndividualEmpGridToExcel();
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

    protected void grdrecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int FromMonthID = Convert.ToInt32(ddlFromMonth.SelectedValue);
                int FromYearID = Convert.ToInt32(ddlFromYear.SelectedValue);
                int ToMonthID = Convert.ToInt32(ddlToMonth.SelectedValue);
                int ToYearID = Convert.ToInt32(ddlToYear.SelectedValue);

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("EmpWiseSalaryReport", con);
                cmd.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmd.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmd.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmd.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                grdrecord.ShowFooter = true;

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        for (int ColumnIndex = 0; ColumnIndex <= ds.Tables[1].Columns.Count; ColumnIndex++)
                        {
                            if (ColumnIndex > 2)
                            {
                                Label lblGrandTotal = new Label();
                                e.Row.Cells[ColumnIndex].Controls.Add(lblGrandTotal);
                                lblGrandTotal.Text = row[ds.Tables[1].Columns[ColumnIndex - 1].ColumnName].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnPrintSelected_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlReportType.SelectedValue == "1")
            {
                this.Bindgrid();
                grdrecord.UseAccessibleHeader = true;
                grdrecord.HeaderRow.TableSection = TableRowSection.TableHeader;
                grdrecord.Attributes["style"] = "border-collapse:separate";
                grdrecord.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
                Session["ctrl"] = grdrecord;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                this.BindIndividualEmpSalaryRpt();
                grdIndividualEmpSalary.UseAccessibleHeader = true;
                grdIndividualEmpSalary.HeaderRow.TableSection = TableRowSection.TableHeader;
                grdIndividualEmpSalary.Attributes["style"] = "border-collapse:separate";
                grdIndividualEmpSalary.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
                Session["ctrl"] = grdIndividualEmpSalary;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);

            }

            lblSTMT.Text = string.Empty;
            pnlStmt.Visible = false;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
            pnlIndividualEmpSalary.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Employee()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", ddlEmployeeStatus.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "Name ASC";
            ddlemployee.DataSource = dv;
            ddlemployee.DataTextField = "DropText";
            ddlemployee.DataValueField = "Emp_Code";
            ddlemployee.DataBind();
            ddlemployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlReportType.SelectedValue == "2")
            {
                pnlSelectEmp.Visible = true;
            }
            else
            {
                pnlSelectEmp.Visible = false;
            }

            ddlEmployeeStatus.ClearSelection();
            ddlemployee.ClearSelection();
            Employee();
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            grdIndividualEmpSalary.DataSource = null;
            grdIndividualEmpSalary.DataBind();
            lblSTMT.Text = string.Empty;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
            pnlIndividualEmpSalary.Visible = false;
            pnlTotalRecords.Visible = false;
            lblTotalRecords.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlEmployeeStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdIndividualEmpSalary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGrandTotalPayDrawnBasic = (Label)e.Row.FindControl("lblGrandTotalPayDrawnBasic");
                Label lblGrandTotalDA = (Label)e.Row.FindControl("lblGrandTotalDA");
                Label lblGrandTotalHRA = (Label)e.Row.FindControl("lblGrandTotalHRA");
                Label lblGrandTotalTransport = (Label)e.Row.FindControl("lblGrandTotalTransport");
                Label lblGrandTotalMedical = (Label)e.Row.FindControl("lblGrandTotalMedical");
                Label lblGrandTotalWashing = (Label)e.Row.FindControl("lblGrandTotalWashing");
                Label lblGrandTotalGrossRevisedSalary = (Label)e.Row.FindControl("lblGrandTotalGrossRevisedSalary");
                Label lblGrandTotalExGratia = (Label)e.Row.FindControl("lblGrandTotalExGratia");
                Label lblGrandTotalArearAdjust = (Label)e.Row.FindControl("lblGrandTotalArearAdjust");
                Label lblGrandTotalGrossTotal = (Label)e.Row.FindControl("lblGrandTotalGrossTotal");
                Label lblGrandTotalPF = (Label)e.Row.FindControl("lblGrandTotalPF");
                Label lblGrandTotalDeduction = (Label)e.Row.FindControl("lblGrandTotalDeduction");
                Label lblGrandTotalTDS = (Label)e.Row.FindControl("lblGrandTotalTDS");
                Label lblGrandTotalAdvance = (Label)e.Row.FindControl("lblGrandTotalAdvance");
                Label lblGrandTotalTPTREC = (Label)e.Row.FindControl("lblGrandTotalTPTREC");
                Label lblGrandTotalGIS = (Label)e.Row.FindControl("lblGrandTotalGIS");
                Label lblGrandTotalESI = (Label)e.Row.FindControl("lblGrandTotalESI");
                Label lblGrandTotalGrossDeduction = (Label)e.Row.FindControl("lblGrandTotalGrossDeduction");
                Label lblGrandTotalGrossTotalSalary = (Label)e.Row.FindControl("lblGrandTotalGrossTotalSalary");

                lblGrandTotalPayDrawnBasic.Text = PayDrawnBasic.ToString();
                lblGrandTotalDA.Text = Da.ToString();
                lblGrandTotalHRA.Text = Hra.ToString();
                lblGrandTotalTransport.Text = Transport.ToString();
                lblGrandTotalMedical.Text = Medical.ToString();
                lblGrandTotalWashing.Text = Washing.ToString();
                lblGrandTotalGrossRevisedSalary.Text = GrossRevisedSalary.ToString();
                lblGrandTotalExGratia.Text = ExGratia.ToString();
                lblGrandTotalArearAdjust.Text = ArearAdjust.ToString();
                lblGrandTotalGrossTotal.Text = GrossTotal.ToString();
                lblGrandTotalPF.Text = PF.ToString();
                lblGrandTotalDeduction.Text = Deduction.ToString();
                lblGrandTotalTDS.Text = TdsOnBasic.ToString();
                lblGrandTotalAdvance.Text = Advance.ToString();
                lblGrandTotalTPTREC.Text = TPTREC.ToString();
                lblGrandTotalGIS.Text = GIS.ToString();
                lblGrandTotalESI.Text = ESI.ToString();
                lblGrandTotalGrossDeduction.Text = TotalDeduction.ToString();
                lblGrandTotalGrossTotalSalary.Text = GrossTotalSalary.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}