using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class SalaryModule_ReimbursementMaking : System.Web.UI.Page
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

    public void Clear()
    {
        try
        {
            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            btnMakeReimbursement.Enabled = true;
            pnlButtons.Visible = false;
            btnSaveReimbursement.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlDetail.Visible = false;
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

    public enum Status
    {
        Active = 1,
        Deactive = 0
    }

    public void Bindgrid()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@ProfileID", 0);
            cmd.Parameters.AddWithValue("@SalaryStatus", 1);
            cmd.Parameters.AddWithValue("@Reimbursement", 1);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            grdrecord.DataSource = dt;
            grdrecord.DataBind();
            if (dt.Rows.Count > 0)
            {
                pnlButtons.Visible = true;
                CalculateDaysInMonth();
                GetImportPaidDaysData();
                GetLWP();
                HideLWPEffectedColumnOnPageLoad();
            }
            else
            {
                pnlDetail.Visible = false;
                pnlButtons.Visible = false;
            }
            pnlDetail.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    public void CalculateDaysInMonth()
    {
        try
        {
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlMonth.SelectedValue) > 0)
            {
                int MonthDays = 0;
                int SelectYear = Convert.ToInt32(ddlYear.SelectedItem.Text);
                MonthDays = System.DateTime.DaysInMonth(SelectYear, Convert.ToInt32(ddlMonth.SelectedValue));

                foreach (GridViewRow row in grdrecord.Rows)
                {
                    Label lblMonthDays = (Label)row.FindControl("lblMonthDays");
                    lblMonthDays.Text = MonthDays.ToString();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select both Month and Year');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetImportPaidDaysData()
    {
        try
        {
            foreach (GridViewRow row in grdrecord.Rows)
            {
                Label lblPaidDays = (Label)row.FindControl("lblPaidDays");
                string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                string AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                string MonthID = ddlMonth.SelectedValue;
                string YearID = ddlYear.SelectedValue;

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetImportDataDetails", con);
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@AssignEmpCode", AssignEmpCode);
                cmd.Parameters.AddWithValue("@MonthID", MonthID);
                cmd.Parameters.AddWithValue("@YearID", YearID);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                if (ds.Tables[3].Rows.Count > 0)
                {
                    lblPaidDays.Text = ds.Tables[3].Rows[0]["PaidDays"].ToString();
                }
                else
                {
                    lblPaidDays.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void HideLWPEffectedColumnOnPageLoad()
    {
        try
        {
            foreach (DataControlField col in grdrecord.Columns)
            {
                if (col.HeaderText == "LWP Effected Value" || col.HeaderText == "Total Reimbursement")
                {
                    col.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ShowLWPEffectedColumnAfterReimbursementCalCulation()
    {
        try
        {
            foreach (DataControlField col in grdrecord.Columns)
            {
                if (col.HeaderText == "LWP Effected Value" || col.HeaderText == "Total Reimbursement")
                {
                    col.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetLWP()
    {
        try
        {
            foreach (GridViewRow row in grdrecord.Rows)
            {
                decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                decimal PaidDays = Convert.ToDecimal(((Label)row.FindControl("lblPaidDays")).Text);
                Label lblLWP = (Label)row.FindControl("lblLWP");
                decimal LWP = MonthDays - PaidDays;
                lblLWP.Text = LWP.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateReimbursement()
    {
        try
        {
            int i = 0;
            foreach (GridViewRow row in grdrecord.Rows)
            {
                Label lblLWP = (Label)row.FindControl("lblLWP");
                decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                decimal ReimbursementPaidDays = Convert.ToDecimal(((Label)row.FindControl("lblPaidDays")).Text);
                Label lblLWPEffectedValue1 = (Label)row.FindControl("lblLWPEffectedValue1");
                Label lblLWPEffectedValue2 = (Label)row.FindControl("lblLWPEffectedValue2");
                Label lblLWPEffectedValue3 = (Label)row.FindControl("lblLWPEffectedValue3");
                Label lblLWPEffectedValue4 = (Label)row.FindControl("lblLWPEffectedValue4");
                Label lblLWPEffectedValue5 = (Label)row.FindControl("lblLWPEffectedValue5");
                Label lblTotalReimbursement = (Label)row.FindControl("lblTotalReimbursement");

                if (((CheckBox)row.FindControl("SelectChk")).Checked == true)
                {
                    SqlConnection con = new SqlConnection(constr);
                    cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                    cmd.Parameters.AddWithValue("@ProfileID", 0);
                    cmd.Parameters.AddWithValue("@SalaryStatus", 1);
                    cmd.Parameters.AddWithValue("@Reimbursement", 1);
                    cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        decimal ReimbursementValue1 = Convert.ToDecimal(dt.Rows[i]["ReimbursementValue1"].ToString());
                        decimal ReimbursementValue2 = Convert.ToDecimal(dt.Rows[i]["ReimbursementValue2"].ToString());
                        decimal ReimbursementValue3 = Convert.ToDecimal(dt.Rows[i]["ReimbursementValue3"].ToString());
                        decimal ReimbursementValue4 = Convert.ToDecimal(dt.Rows[i]["ReimbursementValue4"].ToString());
                        decimal ReimbursementValue5 = Convert.ToDecimal(dt.Rows[i]["ReimbursementValue5"].ToString());
                        decimal ReimbursementValue1PerDay = 0;
                        decimal TotalPaidReimbursementValue1 = 0;
                        decimal ReimbursementValue2PerDay = 0;
                        decimal TotalPaidReimbursementValue2 = 0;
                        decimal ReimbursementValue3PerDay = 0;
                        decimal TotalPaidReimbursementValue3 = 0;
                        decimal ReimbursementValue4PerDay = 0;
                        decimal TotalPaidReimbursementValue4 = 0;
                        decimal ReimbursementValue5PerDay = 0;
                        decimal TotalPaidReimbursementValue5 = 0;
                        decimal TotalReimbursement = 0;

                        if (MonthDays > 0)
                        {
                            ReimbursementValue1PerDay = (ReimbursementValue1 / MonthDays);
                            ReimbursementValue2PerDay = (ReimbursementValue2 / MonthDays);
                            ReimbursementValue3PerDay = (ReimbursementValue3 / MonthDays);
                            ReimbursementValue4PerDay = (ReimbursementValue4 / MonthDays);
                            ReimbursementValue5PerDay = (ReimbursementValue5 / MonthDays);

                            TotalPaidReimbursementValue1 = Converter((ReimbursementValue1PerDay * ReimbursementPaidDays));
                            TotalPaidReimbursementValue2 = Converter((ReimbursementValue2PerDay * ReimbursementPaidDays));
                            TotalPaidReimbursementValue3 = Converter((ReimbursementValue3PerDay * ReimbursementPaidDays));
                            TotalPaidReimbursementValue4 = Converter((ReimbursementValue4PerDay * ReimbursementPaidDays));
                            TotalPaidReimbursementValue5 = Converter((ReimbursementValue5PerDay * ReimbursementPaidDays));                            

                            TotalReimbursement = TotalPaidReimbursementValue1 + TotalPaidReimbursementValue2 + TotalPaidReimbursementValue3 + TotalPaidReimbursementValue4 + TotalPaidReimbursementValue5;                            
                        }

                        lblLWPEffectedValue1.Text = TotalPaidReimbursementValue1.ToString("0.00");
                        lblLWPEffectedValue2.Text = TotalPaidReimbursementValue2.ToString("0.00");
                        lblLWPEffectedValue3.Text = TotalPaidReimbursementValue3.ToString("0.00");
                        lblLWPEffectedValue4.Text = TotalPaidReimbursementValue4.ToString("0.00");
                        lblLWPEffectedValue5.Text = TotalPaidReimbursementValue5.ToString("0.00");
                        lblTotalReimbursement.Text = TotalReimbursement.ToString("0.00");
                    }
                }

                i++;
            }

            ShowLWPEffectedColumnAfterReimbursementCalCulation();
            ddlMonth.Enabled = false;
            ddlYear.Enabled = false;
            btnMakeReimbursement.Enabled = false;
            btnSaveReimbursement.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void SaveCalculatedReimbursement()
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            foreach (GridViewRow row in grdrecord.Rows)
            {
                int Count = 0;
                if (((CheckBox)row.FindControl("SelectChk")).Checked == true)
                {
                    int MonthID = Convert.ToInt32(ddlMonth.SelectedValue);
                    int YearID = Convert.ToInt32(ddlYear.SelectedValue);
                    string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                    int EmployeeID = Convert.ToInt32(((Label)row.FindControl("lblEmployeeID")).Text);
                    string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                    string SystemNumber = ((Label)row.FindControl("lblSystemNumber")).Text;
                    string AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                    string Name = ((Label)row.FindControl("lblName")).Text;
                    string Designation = ((Label)row.FindControl("lblDesignation")).Text;
                    int MonthDays = Convert.ToInt32(((Label)row.FindControl("lblMonthDays")).Text);
                    string LWP = ((Label)row.FindControl("lblLWP")).Text;
                    string PaidDays = ((Label)row.FindControl("lblPaidDays")).Text;
                    string ReimbursementFor1 = ((Label)row.FindControl("lblReimbursementFor1")).Text;
                    string LWPEffectedValue1 = ((Label)row.FindControl("lblLWPEffectedValue1")).Text;
                    string ReimbursementFor2 = ((Label)row.FindControl("lblReimbursementFor2")).Text;
                    string LWPEffectedValue2 = ((Label)row.FindControl("lblLWPEffectedValue2")).Text;
                    string ReimbursementFor3 = ((Label)row.FindControl("lblReimbursementFor3")).Text;
                    string LWPEffectedValue3 = ((Label)row.FindControl("lblLWPEffectedValue3")).Text;
                    string ReimbursementFor4 = ((Label)row.FindControl("lblReimbursementFor4")).Text;
                    string LWPEffectedValue4 = ((Label)row.FindControl("lblLWPEffectedValue4")).Text;
                    string ReimbursementFor5 = ((Label)row.FindControl("lblReimbursementFor5")).Text;
                    string LWPEffectedValue5 = ((Label)row.FindControl("lblLWPEffectedValue5")).Text;
                    string TotalReimbursement = ((Label)row.FindControl("lblTotalReimbursement")).Text;
                    string User = Convert.ToString(hash["Name"].ToString());

                    if (TotalReimbursement == null || TotalReimbursement == "")
                    {
                    }
                    else
                    {
                        SqlConnection con = new SqlConnection(constr);
                        cmd = new SqlCommand("SaveReimbursementMaking", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MonthID", MonthID);
                        cmd.Parameters.AddWithValue("@YearID", YearID);
                        cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                        cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        cmd.Parameters.AddWithValue("@Emp_Code", Emp_Code);
                        cmd.Parameters.AddWithValue("@SystemNumber", SystemNumber);
                        cmd.Parameters.AddWithValue("@AssignEmpCode", AssignEmpCode);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Designation", Designation);
                        cmd.Parameters.AddWithValue("@MonthDays", MonthDays);
                        cmd.Parameters.AddWithValue("@LWP", LWP);
                        cmd.Parameters.AddWithValue("@PaidDays", PaidDays);
                        cmd.Parameters.AddWithValue("@ReimbursementFor1", ReimbursementFor1);
                        cmd.Parameters.AddWithValue("@ReimbursementValue1", LWPEffectedValue1);
                        cmd.Parameters.AddWithValue("@ReimbursementFor2", ReimbursementFor2);
                        cmd.Parameters.AddWithValue("@ReimbursementValue2", LWPEffectedValue2);
                        cmd.Parameters.AddWithValue("@ReimbursementFor3", ReimbursementFor3);
                        cmd.Parameters.AddWithValue("@ReimbursementValue3", LWPEffectedValue3);
                        cmd.Parameters.AddWithValue("@ReimbursementFor4", ReimbursementFor4);
                        cmd.Parameters.AddWithValue("@ReimbursementValue4", LWPEffectedValue4);
                        cmd.Parameters.AddWithValue("@ReimbursementFor5", ReimbursementFor5);
                        cmd.Parameters.AddWithValue("@ReimbursementValue5", LWPEffectedValue5);
                        cmd.Parameters.AddWithValue("@TotalReimbursement", TotalReimbursement);
                        cmd.Parameters.AddWithValue("@User", User);
                        cmd.Parameters.AddWithValue("@Type", "Save");
                        con.Open();
                        Count = cmd.ExecuteNonQuery();
                        con.Close();

                        if (Count > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                        }

                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                        }
                    }
                }
            }

            Clear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkDeactivate")
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("SaveReimbursementMaking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@ProfileID", e.CommandArgument);
                cmd.Parameters.AddWithValue("@EmployeeID", 0);
                cmd.Parameters.AddWithValue("@Emp_Code", null);
                cmd.Parameters.AddWithValue("@SystemNumber", null);
                cmd.Parameters.AddWithValue("@AssignEmpCode", null);
                cmd.Parameters.AddWithValue("@Name", null);
                cmd.Parameters.AddWithValue("@Designation", null);
                cmd.Parameters.AddWithValue("@MonthDays", 0);
                cmd.Parameters.AddWithValue("@LWP", 0);
                cmd.Parameters.AddWithValue("@PaidDays", 0);
                cmd.Parameters.AddWithValue("@ReimbursementFor1", null);
                cmd.Parameters.AddWithValue("@ReimbursementValue1", 0);
                cmd.Parameters.AddWithValue("@ReimbursementFor2", null);
                cmd.Parameters.AddWithValue("@ReimbursementValue2", 0);
                cmd.Parameters.AddWithValue("@ReimbursementFor3", null);
                cmd.Parameters.AddWithValue("@ReimbursementValue3", 0);
                cmd.Parameters.AddWithValue("@ReimbursementFor4", null);
                cmd.Parameters.AddWithValue("@ReimbursementValue4", 0);
                cmd.Parameters.AddWithValue("@ReimbursementFor5", null);
                cmd.Parameters.AddWithValue("@ReimbursementValue5", 0);
                cmd.Parameters.AddWithValue("@TotalReimbursement", 0);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@Type", "Deactivate");
                con.Open();
                int Count = cmd.ExecuteNonQuery();
                con.Close();

                if (Count > 0)
                {
                    Bindgrid();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found to Deactivate.');", true);
                }
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
            grdrecord.DataSource = null;
            grdrecord.DataBind();

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
            grdrecord.DataSource = null;
            grdrecord.DataBind();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ((Label)e.Row.FindControl("lblProfileID")).Text;
                LinkButton lnkDeactivate = (LinkButton)e.Row.FindControl("lnkDeactivate");
                CheckBox SelectChk = (CheckBox)e.Row.FindControl("SelectChk");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowReimbursementMaking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    lnkDeactivate.Visible = true;
                    SelectChk.Enabled = false;

                    Label lblLWP = (Label)e.Row.FindControl("lblLWP");
                    Label lblPaidDays = (Label)e.Row.FindControl("lblPaidDays");
                    Label lblLWPEffectedValue1 = (Label)e.Row.FindControl("lblLWPEffectedValue1");
                    Label lblLWPEffectedValue2 = (Label)e.Row.FindControl("lblLWPEffectedValue2");
                    Label lblLWPEffectedValue3 = (Label)e.Row.FindControl("lblLWPEffectedValue3");
                    Label lblLWPEffectedValue4 = (Label)e.Row.FindControl("lblLWPEffectedValue4");
                    Label lblLWPEffectedValue5 = (Label)e.Row.FindControl("lblLWPEffectedValue5");
                    Label lblTotalReimbursement = (Label)e.Row.FindControl("lblTotalReimbursement");

                    lblLWP.Text = dt.Rows[0]["LWP"].ToString();
                    lblPaidDays.Text = dt.Rows[0]["PaidDays"].ToString();
                    lblLWPEffectedValue1.Text = dt.Rows[0]["ReimbursementValue1"].ToString();
                    lblLWPEffectedValue2.Text = dt.Rows[0]["ReimbursementValue2"].ToString();
                    lblLWPEffectedValue3.Text = dt.Rows[0]["ReimbursementValue3"].ToString();
                    lblLWPEffectedValue4.Text = dt.Rows[0]["ReimbursementValue4"].ToString();
                    lblLWPEffectedValue5.Text = dt.Rows[0]["ReimbursementValue5"].ToString();
                    lblTotalReimbursement.Text = dt.Rows[0]["TotalReimbursement"].ToString();
                    ShowLWPEffectedColumnAfterReimbursementCalCulation();
                }
                else
                {
                    lnkDeactivate.Visible = false;
                    SelectChk.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnMakeReimbursement_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlMonth.SelectedValue) > 0)
            {
                Bindgrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select both Month and Year');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCalculateReimbursement_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                CalculateReimbursement();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select Month and Year Both.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveReimbursement_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                SaveCalculatedReimbursement();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select Month and Year Both.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

}