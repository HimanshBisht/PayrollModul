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

public partial class SalaryModule_MonthlySalaryReport : System.Web.UI.Page
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
                    BindDesignation();
                    BindStaffType();
                    BindEmpNature();
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
                BindDesignation();
                BindStaffType();
                BindEmpNature();
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

    protected void BindDesignation()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageDesignation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Designation", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlDesignation.DataSource = dt;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "DesignationID";
            ddlDesignation.DataBind();
            ddlDesignation.Items.Insert(0, new ListItem("All Designations Types", "0"));

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindStaffType()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageStaffType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StaffType", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlStaffType.DataSource = dt;
            ddlStaffType.DataTextField = "StaffType";
            ddlStaffType.DataValueField = "StaffTypeID";
            ddlStaffType.DataBind();
            ddlStaffType.Items.Insert(0, new ListItem("All Staff Types", "0"));

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindEmpNature()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageNatureofEmp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpNature", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlNatureOfEmp.DataSource = dt;
            ddlNatureOfEmp.DataTextField = "EmpNature";
            ddlNatureOfEmp.DataValueField = "NatureID";
            ddlNatureOfEmp.DataBind();
            ddlNatureOfEmp.Items.Insert(0, new ListItem("All Emp Nature Types", "0"));

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
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@SalaryMakeStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);

            if (ddlPFApply.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "Name ASC";
            ddlemployee.DataSource = dv;
            ddlemployee.DataTextField = "DropText";
            ddlemployee.DataValueField = "ProfileID";
            ddlemployee.DataBind();
            ddlemployee.Items.Insert(0, new ListItem("All Employees", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public enum IsApprove
    {
        All = 0,
        Approve = 1,
        NotApprove = 2
    }

    public void Bindgrid()
    {
        try
        {
            grdrecord.Caption = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);

            if (ddlPFApply.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }

            cmd.Parameters.AddWithValue("@SalaryMakeStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

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
                lblSTMT.Text = "K.R.Mangalam World School, Faridabad " + "<br />" + "Salary Statement For the Month of " + ddlMonth.SelectedItem.Text + " - " + ddlYear.SelectedItem.Text;
                pnlDetail.Visible = true;
            }
            else
            {
                pnlDetail.Visible = false;
                pnlButtons.Visible = false;
                lblSTMT.Text = string.Empty;
                pnlStmt.Visible = false;
            }
            grdrecord.DataSource = dt;
            grdrecord.DataBind();
            pnlFooter.Visible = false;
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
            ddlNatureOfEmp.ClearSelection();
            ddlStaffType.ClearSelection();
            ddlDesignation.ClearSelection();
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            pnlEmployees.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblSTMT.Text = string.Empty;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
            pnlTotalRecords.Visible = false;
            lblTotalRecords.Text = string.Empty;
            lblPreparedByName.Text = string.Empty;
            lblCheckedByName.Text = string.Empty;
            lblScrutinizedByName.Text = string.Empty;
            lblPrincipalName.Text = string.Empty;
            lblChairmanName.Text = string.Empty;
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
            Bindgrid();
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
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);

            if (ddlPFApply.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }

            cmd.Parameters.AddWithValue("@SalaryMakeStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblPreparedByName.Text = dt.Rows[0]["CreatedBy"].ToString();
                lblCheckedByName.Text = dt.Rows[0]["ApprovedBy"].ToString();
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Salary_Statement_" + ddlMonth.SelectedItem.Text + "_" + ddlYear.SelectedItem.Text + "_FDB.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
                pnlFooter.Visible = true;
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

    protected void lnkExportToExcel_Click(object sender, EventArgs e)
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

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                pnlEmployees.Visible = true;
                Employee();
            }
            else
            {
                pnlEmployees.Visible = false;
                ddlemployee.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                pnlEmployees.Visible = true;
                Employee();
            }
            else
            {
                pnlEmployees.Visible = false;
                ddlemployee.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlNatureOfEmp_SelectedIndexChanged(object sender, EventArgs e)
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

    protected void ddlStaffType_SelectedIndexChanged(object sender, EventArgs e)
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

    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
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

    protected void ddlPFApply_SelectedIndexChanged(object sender, EventArgs e)
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

    protected void btnPrintSelected_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);

            if (ddlPFApply.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }
            else if (ddlPFApply.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
            }

            cmd.Parameters.AddWithValue("@SalaryMakeStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblPreparedByName.Text = dt.Rows[0]["CreatedBy"].ToString();
                lblCheckedByName.Text = dt.Rows[0]["ApprovedBy"].ToString();
            }
            grdrecord.UseAccessibleHeader = true;
            grdrecord.HeaderRow.TableSection = TableRowSection.TableHeader;
            grdrecord.Attributes["style"] = "border-collapse:separate";
            grdrecord.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
            pnlFooter.Visible = true;
            Session["ctrl"] = pnlPrint;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
            lblSTMT.Text = string.Empty;
            pnlStmt.Visible = false;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlSalaryStatus_SelectedIndexChanged(object sender, EventArgs e)
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
}