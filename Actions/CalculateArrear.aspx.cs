using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class SalaryModule_CalculateArrear : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal ExtraPaidDaysSalary = 0;

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

    public enum IsApprove
    {
        All = 0,
        Approve = 1,
        NotApprove = 2
    }

    public enum SalaryStatus
    {
        All = 0,
        Release = 1,
        Hold = 2
    }

    public enum Status
    {
        Active = 1,
        Deactive = 0
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
            ddlArrearMonth.DataSource = dt;
            ddlArrearMonth.DataTextField = "MonthName";
            ddlArrearMonth.DataValueField = "MonthID";
            ddlArrearMonth.DataBind();
            ddlArrearMonth.Items.Insert(0, new ListItem("Select Month", "0"));
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
            ddlArrearYear.DataSource = dt;
            ddlArrearYear.DataTextField = "Year";
            ddlArrearYear.DataValueField = "YearID";
            ddlArrearYear.DataBind();
            ddlArrearYear.Items.Insert(0, new ListItem("Select Year", "0"));
            con.Close();
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
            cmd.Parameters.AddWithValue("@SalaryMakeStatus", SalaryStatus.Release);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
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
            ddlemployee.Items.Insert(0, new ListItem("Select Employee", "0"));
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
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@SalaryMakeStatus", SalaryStatus.Release);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                pnlDetails.Visible = true;
                ddlMonth.Enabled = false;
                ddlYear.Enabled = false;
                ddlemployee.Enabled = false;
                lblActualGrossTotalSalary.Text = dt.Rows[0]["GrossTotalSalary"].ToString();
                lblActualSalaryText.Text = Session["SchoolName"].ToString() + "<br />" + "Actual Salary Report of " + ddlemployee.SelectedItem.Text + " For the Month of " + ddlMonth.SelectedItem.Text + " - " + ddlYear.SelectedItem.Text;
                lblCalculatedText.Text = "Change Respective Column Value to Calculate Salary For Arrear";
            }
            else
            {
                pnlDetails.Visible = false;
                ddlMonth.Enabled = true;
                ddlYear.Enabled = true;
                ddlemployee.Enabled = true;
                lblActualGrossTotalSalary.Text = "0.00";
                lblActualSalaryText.Text = string.Empty;
                lblCalculatedText.Text = string.Empty;
            }
            grdActualSalary.DataSource = dt;
            grdActualSalary.DataBind();
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
            pnlTotalRecords.Visible = false;
            lblTotalRecords.Text = string.Empty;
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;
            ddlemployee.Enabled = true;
            pnlEmployees.Visible = false;
            grdActualSalary.DataSource = null;
            grdActualSalary.DataBind();
            pnlDetails.Visible = false;
            lblActualGrossTotalSalary.Text = "0.00";
            lblArrearGrossTotalSalary.Text = "0.00";
            lblFinalArrearAmount.Text = "0.00";
            lblTotalArrearAmount.Text = "0.00";
            ddlArrearMonth.ClearSelection();
            ddlArrearYear.ClearSelection();
            txtRemarks.Text = string.Empty;
            pnlArrear.Visible = false;
            pnlSaveSection.Visible = false;
            ViewState["PFDeduct"] = null;
            ViewState["ESIDeduct"] = null;
            ViewState["TotalPaidBasicSalary"] = null;
            ViewState["TotalDA"] = null;
            ViewState["GrossTotal"] = null;
            ViewState["AssignPFValue"] = null;
            ViewState["AssignESIValue"] = null;
            ViewState["EffectedTotalPaidBasicSalary"] = null;
            ViewState["EffectedTotalDA"] = null;
            ViewState["EffectedGrossTotal"] = null;
            ViewState["EffectedAssignPFValue"] = null;
            ViewState["EffectedAssignESIValue"] = null;
            lblCalculatedText.Text = string.Empty;
            lblActualSalaryText.Text = string.Empty;
            lblPreArrearAmount.Text = "0.00";
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Bindgrid();
            BindCalculateSalarygrid();
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

    protected void btnCalculateArrear_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                CalculateSalary();
                CalculateArrear();
                pnlArrear.Visible = true;
                pnlSaveSection.Visible = true;
            }
            else
            {
                pnlArrear.Visible = false;
                pnlSaveSection.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select Month and Year Both.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void SaveCalculatedSalary()
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            decimal ArearValue = Convert.ToDecimal(lblFinalArrearAmount.Text);
            decimal TotalArrearAmount = Convert.ToDecimal(lblTotalArrearAmount.Text);
            decimal PreArrearAmount = Convert.ToDecimal(lblPreArrearAmount.Text);
            string SalaryID = string.Empty;
            int SalaryMonthID = Convert.ToInt32(ddlMonth.SelectedValue);
            int SalaryYearID = Convert.ToInt32(ddlYear.SelectedValue);
            int ArrearMonthID = Convert.ToInt32(ddlArrearMonth.SelectedValue);
            int ArrearYearID = Convert.ToInt32(ddlArrearYear.SelectedValue);
            string ArrearMonth = ddlArrearMonth.SelectedItem.Text;
            string ArrearYear = ddlArrearYear.SelectedItem.Text;

            if (ArearValue > 0)
            {
                if ((ArrearMonthID > SalaryMonthID && ArrearYearID == SalaryYearID) || (ArrearYearID > SalaryYearID))
                {
                    foreach (GridViewRow row in grdActualSalary.Rows)
                    {
                        SalaryID = ((Label)row.FindControl("lblSalaryID")).Text;
                    }

                    foreach (GridViewRow row in grdrecord.Rows)
                    {
                        int Count = 0;

                        string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                        string NatureID = ((Label)row.FindControl("lblNatureID")).Text;
                        string StaffTypeID = ((Label)row.FindControl("lblStaffTypeID")).Text;
                        int EmployeeID = Convert.ToInt32(((Label)row.FindControl("lblEmployeeID")).Text);
                        string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                        string SystemNumber = ((Label)row.FindControl("lblSystemNumber")).Text;
                        string AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                        string Name = ((Label)row.FindControl("lblName")).Text;
                        string Designation = ((Label)row.FindControl("lblDesignation")).Text;
                        string DesignationText = ((Label)row.FindControl("lblDesignationText")).Text;
                        string BasicScale = ((TextBox)row.FindControl("lblBasicScale")).Text;
                        int MonthDays = Convert.ToInt32(((Label)row.FindControl("lblMonthDays")).Text);
                        string LWP = ((Label)row.FindControl("lblLWP")).Text;
                        string PaidDays = ((TextBox)row.FindControl("lblPaidDays")).Text;
                        string PayDrawnBasic = ((Label)row.FindControl("lblPayDrawnBasic")).Text;
                        string DAApply = ((TextBox)row.FindControl("lblDAApply")).Text;
                        string DaOnBasic = ((Label)row.FindControl("lblDaOnBasic")).Text;
                        string HRAApply = ((TextBox)row.FindControl("lblHRAApply")).Text;
                        string HraOnBasic = ((Label)row.FindControl("lblHraOnBasic")).Text;
                        string TransportOnBasic = ((TextBox)row.FindControl("lblTransportOnBasic")).Text;
                        string MedicalOnBasic = ((TextBox)row.FindControl("lblMedicalOnBasic")).Text;
                        string WashingOnBasic = ((Label)row.FindControl("lblWashingOnBasic")).Text;
                        string GrossRevisedsalaryOnBasic = ((Label)row.FindControl("lblGrossRevisedsalary")).Text;
                        string ExGratiaOnBasic = ((TextBox)row.FindControl("lblExGratiaOnBasic")).Text;
                        string ArearAdjust = ((Label)row.FindControl("lblArearAdjust")).Text;
                        string GrossTotalOnBasic = ((Label)row.FindControl("lblGrossTotal")).Text;
                        string DaForReportOnBasic = ((Label)row.FindControl("lblDaForReportOnBasic")).Text;
                        string SalaryMode = ((Label)row.FindControl("lblSalaryMode")).Text;
                        string PFDeduct = ((Label)row.FindControl("lblPFDeduct")).Text;
                        string PFOnBasic = ((Label)row.FindControl("lblPFOnBasic")).Text;
                        string Deduction = ((Label)row.FindControl("lblDeduction")).Text;
                        string TDSOnBasic = ((Label)row.FindControl("lblTDS")).Text;
                        string Advance = ((Label)row.FindControl("lblAdvance")).Text;
                        string TPTRECOnBasic = ((Label)row.FindControl("lblTPTRECOnBasic")).Text;
                        string GISOnBasic = ((Label)row.FindControl("lblGISOnBasic")).Text;
                        string ESIDeduct = ((Label)row.FindControl("lblESIDeduct")).Text;
                        string ESIOnBasic = ((Label)row.FindControl("lblESIOnBasic")).Text;
                        string TotalDeductionOnBasic = ((Label)row.FindControl("lblTotalDeduction")).Text;
                        string GrossTotalSalaryOnBasic = ((Label)row.FindControl("lblGrossTotalSalary")).Text;
                        string User = Convert.ToString(hash["Name"].ToString());

                        string Remarks = txtRemarks.Text.Trim();

                        if (GrossTotalSalaryOnBasic == null || GrossTotalSalaryOnBasic == "")
                        {
                        }
                        else
                        {
                            SqlConnection con = new SqlConnection(constr);
                            cmd = new SqlCommand("SaveArrearDetails", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MonthID", ArrearMonthID);
                            cmd.Parameters.AddWithValue("@YearID", ArrearYearID);
                            cmd.Parameters.AddWithValue("@NatureID", NatureID);
                            cmd.Parameters.AddWithValue("@StaffTypeID", StaffTypeID);
                            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                            cmd.Parameters.AddWithValue("@Emp_Code", Emp_Code);
                            cmd.Parameters.AddWithValue("@SystemNumber", SystemNumber);
                            cmd.Parameters.AddWithValue("@AssignEmpCode", AssignEmpCode);
                            cmd.Parameters.AddWithValue("@Name", Name);
                            cmd.Parameters.AddWithValue("@Designation", Designation);
                            cmd.Parameters.AddWithValue("@DesignationName", DesignationText);
                            cmd.Parameters.AddWithValue("@Basic", BasicScale);
                            cmd.Parameters.AddWithValue("@MonthDays", MonthDays);
                            cmd.Parameters.AddWithValue("@LWP", LWP);
                            cmd.Parameters.AddWithValue("@PaidDays", PaidDays);
                            cmd.Parameters.AddWithValue("@PayDrawnBasic", PayDrawnBasic);
                            cmd.Parameters.AddWithValue("@DAApply", DAApply);
                            cmd.Parameters.AddWithValue("@DA", DaOnBasic);
                            cmd.Parameters.AddWithValue("@HRAApply", HRAApply);
                            cmd.Parameters.AddWithValue("@HRA", HraOnBasic);
                            cmd.Parameters.AddWithValue("@Transport", TransportOnBasic);
                            cmd.Parameters.AddWithValue("@Medical", MedicalOnBasic);
                            cmd.Parameters.AddWithValue("@Washing", WashingOnBasic);
                            cmd.Parameters.AddWithValue("@GrossRevisedSalary", GrossRevisedsalaryOnBasic);
                            cmd.Parameters.AddWithValue("@ExGratia", ExGratiaOnBasic);
                            cmd.Parameters.AddWithValue("@ArearAdjust", ArearAdjust);
                            cmd.Parameters.AddWithValue("@GrossTotal", GrossTotalOnBasic);
                            cmd.Parameters.AddWithValue("@DAForReport", DaForReportOnBasic);
                            cmd.Parameters.AddWithValue("@ModeOfSalary", SalaryMode);
                            cmd.Parameters.AddWithValue("@PFDeduct", PFDeduct);
                            cmd.Parameters.AddWithValue("@PF", PFOnBasic);
                            cmd.Parameters.AddWithValue("@Deduction", Deduction);
                            cmd.Parameters.AddWithValue("@TDS", TDSOnBasic);
                            cmd.Parameters.AddWithValue("@Advance", Advance);
                            cmd.Parameters.AddWithValue("@TPTREC", TPTRECOnBasic);
                            cmd.Parameters.AddWithValue("@GIS", GISOnBasic);
                            cmd.Parameters.AddWithValue("@ESIDeduct", ESIDeduct);
                            cmd.Parameters.AddWithValue("@ESI", ESIOnBasic);
                            cmd.Parameters.AddWithValue("@TotalDeduction", TotalDeductionOnBasic);
                            cmd.Parameters.AddWithValue("@GrossTotalSalary", GrossTotalSalaryOnBasic);
                            cmd.Parameters.AddWithValue("@User", User);
                            cmd.Parameters.AddWithValue("@ModeChangeRemarks", null);
                            cmd.Parameters.AddWithValue("@Type", "Save");
                            cmd.Parameters.AddWithValue("@Month", ArrearMonth);
                            cmd.Parameters.AddWithValue("@Year", ArrearYear);
                            cmd.Parameters.AddWithValue("@Remarks", Remarks);
                            cmd.Parameters.AddWithValue("@SalaryID", SalaryID);
                            cmd.Parameters.AddWithValue("@ArearValue", ArearValue);
                            cmd.Parameters.AddWithValue("@TotalArrearAmount", TotalArrearAmount);
                            cmd.Parameters.AddWithValue("@PreArrearAmount", PreArrearAmount);
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

                    Clear();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Arrear Selected Month and year should be greater than Salary Month and Year.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Arrear Amount should be greater than 0');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveArrear_Click(object sender, EventArgs e)
    {
        try
        {
            CheckValidSaveMonthAndYear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CheckValidSaveMonthAndYear()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlArrearMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlArrearYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@SalaryMakeStatus", SalaryStatus.Release);
            cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Salary is already given for this Month and Year, Please select valid month and year to save Arrear Details.');", true);
            }
            else
            {
                SaveCalculatedSalary();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
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

    public void GetImportAllData()
    {
        try
        {
            foreach (GridViewRow row in grdrecord.Rows)
            {
                Label lblAdvance = (Label)row.FindControl("lblAdvance");
                Label lblArearAdjust = (Label)row.FindControl("lblArearAdjust");
                Label lblDeduction = (Label)row.FindControl("lblDeduction");
                TextBox lblPaidDays = (TextBox)row.FindControl("lblPaidDays");
                Label lblTDS = (Label)row.FindControl("lblTDS");
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

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblAdvance.Text = ds.Tables[0].Rows[0]["AdvanceValue"].ToString();
                }
                else
                {
                    lblAdvance.Text = "0.00";
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblArearAdjust.Text = ds.Tables[1].Rows[0]["ArearValue"].ToString();
                }
                else
                {
                    lblArearAdjust.Text = "0.00";
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    lblDeduction.Text = ds.Tables[2].Rows[0]["DeductionValue"].ToString();
                }
                else
                {
                    lblDeduction.Text = "0.00";
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    lblPaidDays.Text = ds.Tables[3].Rows[0]["PaidDays"].ToString();
                }
                else
                {
                    lblPaidDays.Text = "0.00";
                }

                if (ds.Tables[4].Rows.Count > 0)
                {
                    lblTDS.Text = ds.Tables[4].Rows[0]["TDSValue"].ToString();
                }
                else
                {
                    lblTDS.Text = "0.00";
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
                decimal PaidDays = Convert.ToDecimal(((TextBox)row.FindControl("lblPaidDays")).Text);
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

    public void BindCalculateSalarygrid()
    {
        try
        {
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlMonth.SelectedValue) > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@SalaryStatus", Status.Active);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
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
                    GetImportAllData();
                    GetLWP();
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

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    public void CalculatePF()
    {
        try
        {
            decimal BasicScale = Convert.ToDecimal(ViewState["TotalPaidBasicSalary"]);
            decimal DAValue = Convert.ToDecimal(ViewState["TotalDA"]);
            decimal GetBasicPlusDAForPF = BasicScale + DAValue;
            string PF = ViewState["PFDeduct"].ToString();
            decimal AssignPFValue = 0;
            decimal GetPFMaxRange = 0;
            decimal GetPFValue = 0;

            if (PF == "1" || PF == "3")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                GetPFMaxRange = Convert.ToDecimal(ds.Tables[5].Rows[0]["MaxRange"].ToString());
                GetPFValue = Convert.ToDecimal(ds.Tables[5].Rows[0]["PF"].ToString());

                if (PF == "1")
                {
                    if (GetBasicPlusDAForPF >= GetPFMaxRange)
                    {
                        AssignPFValue = Converter((GetPFMaxRange * GetPFValue) / 100);
                    }
                    else if (GetBasicPlusDAForPF < GetPFMaxRange)
                    {
                        AssignPFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                    }
                }
                else if (PF == "3")
                {
                    AssignPFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                }
            }
            ViewState["AssignPFValue"] = AssignPFValue;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateESI()
    {
        try
        {
            decimal BasicScale = Convert.ToDecimal(ViewState["TotalPaidBasicSalary"]);
            string ESI = ViewState["ESIDeduct"].ToString();
            decimal AssignESIValue = 0;

            if (ESI == "1")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                decimal GetGrossTotal = Convert.ToDecimal(ViewState["GrossTotal"]);
                decimal GetESIMaxRange = Convert.ToDecimal(ds.Tables[4].Rows[0]["MaxRange"].ToString());
                decimal GetESIValue = Convert.ToDecimal(ds.Tables[4].Rows[0]["ESI"].ToString());
                AssignESIValue = Math.Ceiling((GetGrossTotal * GetESIValue) / 100);
            }

            ViewState["AssignESIValue"] = AssignESIValue;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateEffectedPF()
    {
        try
        {
            string PF = ViewState["PFDeduct"].ToString();
            decimal EffectedAssignPFValue = 0;
            decimal EffectedBasicScale = Convert.ToDecimal(ViewState["EffectedTotalPaidBasicSalary"]);
            decimal EffectedDAValue = Convert.ToDecimal(ViewState["EffectedTotalDA"]);
            decimal GetPFMaxRange = 0;
            decimal GetPFValue = 0;
            decimal GetEffectedBasicPlusDAForPF = EffectedBasicScale + EffectedDAValue;

            if (PF == "1" || PF == "3")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                GetPFMaxRange = Convert.ToDecimal(ds.Tables[5].Rows[0]["MaxRange"].ToString());
                GetPFValue = Convert.ToDecimal(ds.Tables[5].Rows[0]["PF"].ToString());

                if (PF == "1")
                {
                    if (GetEffectedBasicPlusDAForPF >= GetPFMaxRange)
                    {
                        EffectedAssignPFValue = Converter((GetPFMaxRange * GetPFValue) / 100);
                    }
                    else if (GetEffectedBasicPlusDAForPF < GetPFMaxRange)
                    {
                        EffectedAssignPFValue = Converter((GetEffectedBasicPlusDAForPF * GetPFValue) / 100);
                    }
                }
                else if (PF == "3")
                {
                    EffectedAssignPFValue = Converter((GetEffectedBasicPlusDAForPF * GetPFValue) / 100);
                }
            }
            ViewState["EffectedAssignPFValue"] = EffectedAssignPFValue;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateEffectedESI()
    {
        try
        {
            string ESI = ViewState["ESIDeduct"].ToString();
            decimal EffectedBasicScale = Convert.ToDecimal(ViewState["EffectedTotalPaidBasicSalary"]);
            decimal EffectedAssignESIValue = 0;
            decimal EffectedDAValue = Convert.ToDecimal(ViewState["EffectedTotalDA"]);

            if (ESI == "1")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                decimal GetEffectedGrossTotal = Convert.ToDecimal(ViewState["EffectedGrossTotal"]);
                decimal GetESIMaxRange = Convert.ToDecimal(ds.Tables[4].Rows[0]["MaxRange"].ToString());
                decimal GetESIValue = Convert.ToDecimal(ds.Tables[4].Rows[0]["ESI"].ToString());
                EffectedAssignESIValue = Math.Ceiling((GetEffectedGrossTotal * GetESIValue) / 100);
            }

            ViewState["EffectedAssignESIValue"] = EffectedAssignESIValue;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateSalaryForExtraPaidDays(decimal ExtraPaidDays, int i)
    {
        try
        {
            foreach (GridViewRow row in grdrecord.Rows)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@SalaryStatus", Status.Active);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    decimal BasicSalary = Convert.ToDecimal(((TextBox)row.FindControl("lblBasicScale")).Text);
                    decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                    decimal ArearAdjust = 0;
                    decimal Deduction = 0;
                    decimal TDS = 0;
                    decimal Advance = 0;
                    decimal GIS = 0;
                    decimal SalaryPerDay = 0;
                    decimal FinalSalaryPerDay = 0;
                    decimal TotalPaidBasicSalary = 0;
                    decimal TotalDA = 0;
                    decimal TotalHRA = 0;
                    decimal TotalTransport = 0;
                    decimal TotalMedical = 0;
                    decimal TotalWashing = 0;
                    decimal TotalExGratia = 0;
                    decimal TotalPF = 0;
                    decimal TotalTransportRecovery = 0;
                    decimal TotalESI = 0;
                    decimal TotalDeduction = 0;
                    decimal GrossRevisedSalary = 0;
                    decimal GrossTotal = 0;
                    decimal GrossTotalSalaryAfterDeduction = 0;
                    decimal EffectedGrossTotalSalaryAfterDeduction = 0;
                    decimal Total = 0;
                    decimal EffectedTotal = 0;
                    string ChangeScale = dt.Rows[i]["ChangeScaleText"].ToString();

                    // Calculate Salary On Basic with Total salary Paid Days start here.

                    if (BasicSalary > 0 && MonthDays > 0)
                    {
                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * ExtraPaidDays));

                        GrossRevisedSalary = Converter((TotalPaidBasicSalary + TotalDA + TotalHRA + TotalTransport + TotalMedical + TotalWashing));

                        GrossTotal = Converter((GrossRevisedSalary + TotalExGratia + ArearAdjust));

                        TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));

                        ExtraPaidDaysSalary = GrossTotalSalaryAfterDeduction;

                        if (ExtraPaidDays > 0)
                        {
                            Total = (GrossRevisedSalary + TotalExGratia) - (TotalPF + TotalESI);
                            FinalSalaryPerDay = Converter((Total / ExtraPaidDays));
                        }
                        else
                        {
                            FinalSalaryPerDay = 0;
                        }
                    }

                    // Calculate Salary On Basic with Total salary Paid Days End here.

                    // Calculate Salary On Effected Basic with Total salary Paid Days start here.

                    if (ChangeScale == "Yes" && ExtraPaidDays > 0)
                    {
                        decimal EffectedScale = Convert.ToDecimal(dt.Rows[i]["EffectedScale"].ToString());
                        decimal EffectedSalaryPerDay = 0;
                        decimal EffectedTotalPaidBasicSalary = 0;
                        decimal EffectedTotalDA = 0;
                        decimal EffectedTotalHRA = 0;
                        decimal EffectedTotalTransport = 0;
                        decimal EffectedTotalMedical = 0;
                        decimal EffectedTotalWashing = 0;
                        decimal EffectedTotalExGratia = 0;
                        decimal EffectedTotalPF = 0;
                        decimal EffectedTotalTransportRecovery = 0;
                        decimal EffectedTotalESI = 0;
                        decimal EffectedTotalDeduction = 0;
                        decimal EffectedGrossRevisedSalary = 0;
                        decimal EffectedGrossTotal = 0;

                        EffectedSalaryPerDay = (Convert.ToDecimal(EffectedScale) / MonthDays);
                        EffectedTotalPaidBasicSalary = Converter((EffectedSalaryPerDay * ExtraPaidDays));

                        EffectedGrossRevisedSalary = Converter((EffectedTotalPaidBasicSalary + EffectedTotalDA + EffectedTotalHRA + EffectedTotalTransport + EffectedTotalMedical + EffectedTotalWashing));

                        EffectedGrossTotal = Converter((EffectedGrossRevisedSalary + EffectedTotalExGratia + ArearAdjust));

                        EffectedTotalDeduction = Converter((EffectedTotalPF + Deduction + TDS + Advance + EffectedTotalTransportRecovery + GIS + EffectedTotalESI));

                        EffectedGrossTotalSalaryAfterDeduction = Converter((EffectedGrossTotal - EffectedTotalDeduction));

                        EffectedTotal = (EffectedGrossRevisedSalary + EffectedTotalExGratia) - (EffectedTotalPF + EffectedTotalESI);

                        float NoofLWPStart = 0;
                        decimal TotalNearestAmount = 0;
                        float NoofLWPEnd = 20;
                        decimal TotalDifference = (Total - EffectedTotal);
                        string[] res = ExtraPaidDays.ToString().Split('.');
                        if (Convert.ToDecimal(res[1]) > 0)
                        {
                            for (NoofLWPStart = 0.5F; NoofLWPStart <= NoofLWPEnd;)
                            {
                                if (TotalNearestAmount <= TotalDifference)
                                {
                                    TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                    if (TotalNearestAmount <= TotalDifference)
                                    {
                                        NoofLWPStart = NoofLWPStart + 0.5F;
                                        continue;
                                    }
                                    else
                                    {
                                        NoofLWPStart = NoofLWPStart - 0.5F;
                                        TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (NoofLWPStart = 1; NoofLWPStart <= NoofLWPEnd;)
                            {
                                if (TotalNearestAmount <= TotalDifference)
                                {
                                    TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                    if (TotalNearestAmount <= TotalDifference)
                                    {
                                        NoofLWPStart++;
                                        continue;
                                    }
                                    else
                                    {
                                        NoofLWPStart--;
                                        TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        decimal FinalTotalNearestAmount = Converter(Convert.ToDecimal(TotalNearestAmount));
                        decimal FinalTotalDedInScale = Converter(Convert.ToDecimal(TotalDifference));

                        // Again Calculate Salary for Effected Scale users on the basis of original Scale with New Salary Paid Days start here.

                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * ExtraPaidDays));

                        GrossRevisedSalary = Converter((TotalPaidBasicSalary + TotalDA + TotalHRA + TotalTransport + TotalMedical + TotalWashing));

                        GrossTotal = Converter((GrossRevisedSalary + TotalExGratia + ArearAdjust));

                        decimal CalculateTotalForAssignTotalDeduction = (GrossTotal - (TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        Deduction = Deduction + CalculateTotalForAssignTotalDeduction - EffectedGrossTotalSalaryAfterDeduction;

                        TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));
                        ExtraPaidDaysSalary = GrossTotalSalaryAfterDeduction;

                        // Again Calculate Salary for Effected Scale users on the basis of original Scale with New Salary Paid Days End here.
                    }

                    if (ExtraPaidDaysSalary > 0)
                    {
                        break;
                    }

                    // Calculate Salary On Effected Basic with Total salary Paid Days End here.
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateSalary()
    {
        try
        {
            int i = 0;
            foreach (GridViewRow row in grdrecord.Rows)
            {

                string Name = ((Label)row.FindControl("lblName")).Text;
                string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                Label lblLWP = (Label)row.FindControl("lblLWP");
                TextBox lblPaidDays = (TextBox)row.FindControl("lblPaidDays");
                Label lblPayDrawnBasic = (Label)row.FindControl("lblPayDrawnBasic");
                Label lblDaOnBasic = (Label)row.FindControl("lblDaOnBasic");
                Label lblDaForReportOnBasic = (Label)row.FindControl("lblDaForReportOnBasic");
                Label lblHraOnBasic = (Label)row.FindControl("lblHraOnBasic");
                TextBox lblTransportOnBasic = (TextBox)row.FindControl("lblTransportOnBasic");
                TextBox lblMedicalOnBasic = (TextBox)row.FindControl("lblMedicalOnBasic");
                Label lblWashingOnBasic = (Label)row.FindControl("lblWashingOnBasic");
                Label lblGrossRevisedsalary = (Label)row.FindControl("lblGrossRevisedsalary");
                TextBox lblExGratiaOnBasic = (TextBox)row.FindControl("lblExGratiaOnBasic");
                Label lblArearAdjust = (Label)row.FindControl("lblArearAdjust");
                Label lblGrossTotal = (Label)row.FindControl("lblGrossTotal");
                Label lblPFOnBasic = (Label)row.FindControl("lblPFOnBasic");
                Label lblDeduction = (Label)row.FindControl("lblDeduction");
                Label lblTDS = (Label)row.FindControl("lblTDS");
                Label lblAdvance = (Label)row.FindControl("lblAdvance");
                Label lblTPTRECOnBasic = (Label)row.FindControl("lblTPTRECOnBasic");
                Label GISOnBasic = (Label)row.FindControl("lblGISOnBasic");
                Label ESIOnBasic = (Label)row.FindControl("lblESIOnBasic");
                Label lblTotalDeduction = (Label)row.FindControl("lblTotalDeduction");
                Label lblGrossTotalSalary = (Label)row.FindControl("lblGrossTotalSalary");
                Label lblEffectedGrossTotalSalary = (Label)row.FindControl("lblEffectedGrossTotalSalary");
                TextBox lblDAApply = (TextBox)row.FindControl("lblDAApply");
                TextBox lblHRAApply = (TextBox)row.FindControl("lblHRAApply");
                TextBox lblBasicScale = (TextBox)row.FindControl("lblBasicScale");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@SalaryStatus", Status.Active);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                decimal HRA = 0;
                if (dt.Rows.Count > 0)
                {
                    decimal BasicSalary = Convert.ToDecimal(((TextBox)row.FindControl("lblBasicScale")).Text);
                    decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                    decimal SalaryPaidDays = Convert.ToDecimal(Convert.ToDecimal(((TextBox)row.FindControl("lblPaidDays")).Text).ToString("0.00"));
                    decimal DA = Convert.ToDecimal(((TextBox)row.FindControl("lblDAApply")).Text);
                    if (((TextBox)row.FindControl("lblHRAApply")).Text.ToString().Contains("%"))
                    {
                        string[] HRAText = ((TextBox)row.FindControl("lblHRAApply")).Text.ToString().Split('%');
                        HRA = Convert.ToDecimal(HRAText[0]);
                    }
                    else
                    {
                        HRA = Convert.ToDecimal(((TextBox)row.FindControl("lblHRAApply")).Text);
                    }
                    decimal Transport = Convert.ToDecimal(((TextBox)row.FindControl("lblTransportOnBasic")).Text);
                    decimal Medical = Convert.ToDecimal(((TextBox)row.FindControl("lblMedicalOnBasic")).Text);
                    decimal Washing = Convert.ToDecimal(dt.Rows[i]["WashingValue"].ToString());
                    decimal ExGratia = Convert.ToDecimal(((TextBox)row.FindControl("lblExGratiaOnBasic")).Text);
                    decimal ArearAdjust = Convert.ToDecimal(((Label)row.FindControl("lblArearAdjust")).Text);
                    string PFDeduct = dt.Rows[i]["PFDeduct"].ToString();
                    ViewState["PFDeduct"] = PFDeduct;
                    decimal PF = Convert.ToDecimal(dt.Rows[i]["PFValue"].ToString());
                    decimal Deduction = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblDeduction")).Text);
                    decimal TDS = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblTDS")).Text);
                    decimal Advance = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblAdvance")).Text);
                    decimal TptRec = Convert.ToDecimal(dt.Rows[i]["TransportRecovery"].ToString());
                    decimal GIS = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblGIS")).Text);
                    string ESIDeduct = dt.Rows[i]["ESIDeduct"].ToString();
                    ViewState["ESIDeduct"] = ESIDeduct;
                    decimal ESI = Convert.ToDecimal(dt.Rows[i]["ESIValue"].ToString());
                    decimal SalaryPerDay = 0;
                    decimal FinalSalaryPerDay = 0;
                    decimal TotalPaidBasicSalary = 0;
                    decimal TotalDA = 0;
                    decimal TotalDaForReport = 0;
                    decimal TotalHRA = 0;
                    decimal TotalTransport = 0;
                    decimal TotalMedical = 0;
                    decimal TotalWashing = 0;
                    decimal TotalExGratia = 0;
                    decimal TotalPF = 0;
                    decimal TotalTransportRecovery = 0;
                    decimal TotalESI = 0;
                    decimal TotalDeduction = 0;
                    decimal GrossRevisedSalary = 0;
                    decimal GrossTotal = 0;
                    decimal GrossTotalSalaryAfterDeduction = 0;
                    decimal Total = 0;
                    decimal EffectedGrossTotalSalaryAfterDeduction = 0;
                    decimal EffectedTotal = 0;
                    string ChangeScale = dt.Rows[i]["ChangeScaleText"].ToString();
                    decimal HRAPerDay = 0;

                    // Calculate Salary On Basic with Total salary Paid Days start here.

                    if (BasicSalary > 0 && SalaryPaidDays > 0)
                    {
                        if (SalaryPaidDays < MonthDays)
                        {
                            string[] strPaidDays = SalaryPaidDays.ToString().Split('.');
                            decimal FractionValue = Convert.ToDecimal("." + strPaidDays[1]);

                            if (FractionValue > 0)
                            {
                                decimal ExtraPaidDays = 0;
                                ExtraPaidDays = FractionValue;
                                SalaryPaidDays = SalaryPaidDays - FractionValue;
                                lblLWP.Text = (MonthDays - SalaryPaidDays).ToString("0.00");
                                lblPaidDays.Text = SalaryPaidDays.ToString("0.00");
                                CalculateSalaryForExtraPaidDays(ExtraPaidDays, i);
                            }
                        }

                        if (SalaryPaidDays > MonthDays)
                        {
                            decimal ExtraPaidDays = 0;
                            ExtraPaidDays = SalaryPaidDays - MonthDays;
                            SalaryPaidDays = SalaryPaidDays - ExtraPaidDays;
                            lblLWP.Text = (SalaryPaidDays - MonthDays).ToString("0.00");
                            lblPaidDays.Text = SalaryPaidDays.ToString("0.00");
                            CalculateSalaryForExtraPaidDays(ExtraPaidDays, i);
                        }

                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * SalaryPaidDays));
                        ViewState["TotalPaidBasicSalary"] = TotalPaidBasicSalary;

                        TotalDA = Converter((TotalPaidBasicSalary * DA) / 100);
                        ViewState["TotalDA"] = TotalDA;

                        if (PFDeduct == "1" || PFDeduct == "3")
                        {
                            cmd = new SqlCommand("GetSetUpDetails", con);
                            cmd.Parameters.AddWithValue("@GradePay", 0);
                            cmd.CommandType = CommandType.StoredProcedure;
                            con.Open();
                            DataSet ds = new DataSet();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                            con.Close();

                            decimal GetBasicPlusDA = Converter((TotalPaidBasicSalary) + (TotalPaidBasicSalary * DA) / 100);
                            decimal GetPFMaxRange = Convert.ToDecimal(ds.Tables[5].Rows[0]["MaxRange"].ToString());

                            if (PFDeduct == "1")
                            {

                                if (GetBasicPlusDA >= GetPFMaxRange)
                                {
                                    TotalDaForReport = GetPFMaxRange;
                                }
                                else
                                {
                                    TotalDaForReport = GetBasicPlusDA;
                                }
                            }

                            if (PFDeduct == "3")
                            {
                                TotalDaForReport = GetBasicPlusDA;
                            }
                        }

                        if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                        {
                            TotalHRA = Converter((TotalPaidBasicSalary * HRA) / 100);
                        }
                        else
                        {
                            HRAPerDay = HRA / MonthDays;
                            TotalHRA = Converter(HRAPerDay * SalaryPaidDays);
                        }

                        TotalTransport = Converter((Transport / MonthDays) * SalaryPaidDays);

                        TotalMedical = Converter((Medical / MonthDays) * SalaryPaidDays);

                        TotalWashing = Converter((Washing / MonthDays) * SalaryPaidDays);

                        GrossRevisedSalary = Converter((TotalPaidBasicSalary + TotalDA + TotalHRA + TotalTransport + TotalMedical + TotalWashing));

                        TotalExGratia = ExtraPaidDaysSalary + Converter((ExGratia / MonthDays) * SalaryPaidDays);

                        GrossTotal = Converter((GrossRevisedSalary + TotalExGratia + ArearAdjust));
                        ViewState["GrossTotal"] = GrossTotal;

                        TotalPF = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblPF")).Text);

                        TotalTransportRecovery = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblTPTREC")).Text);

                        TotalESI = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblESI")).Text);

                        TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));

                        Total = (GrossRevisedSalary + TotalExGratia) - (TotalPF + TotalESI);
                        FinalSalaryPerDay = Converter((Total / SalaryPaidDays));

                        // Calculate Salary On Basic with Total salary Paid Days End here.

                        // Calculate Salary On Effected Basic with Total salary Paid Days start here.

                        if (ChangeScale == "Yes" && SalaryPaidDays > 0)
                        {
                            decimal EffectedScale = Convert.ToDecimal(dt.Rows[i]["EffectedScale"].ToString());
                            decimal EffectedSalaryPerDay = 0;
                            decimal EffectedTotalPaidBasicSalary = 0;
                            decimal EffectedTotalDA = 0;
                            decimal EffectedTotalHRA = 0;
                            decimal EffectedTotalTransport = 0;
                            decimal EffectedTotalMedical = 0;
                            decimal EffectedTotalWashing = 0;
                            decimal EffectedTotalExGratia = 0;
                            decimal EffectedTotalPF = 0;
                            decimal EffectedTotalTransportRecovery = 0;
                            decimal EffectedTotalESI = 0;
                            decimal EffectedTotalDeduction = 0;
                            decimal EffectedGrossRevisedSalary = 0;
                            decimal EffectedGrossTotal = 0;
                            decimal LWP = Convert.ToDecimal(lblLWP.Text);
                            decimal EffectedHRAPerDay = 0;
                            decimal EffectedTotalDaForReport = 0;

                            EffectedSalaryPerDay = (Convert.ToDecimal(EffectedScale) / MonthDays);
                            EffectedTotalPaidBasicSalary = Converter((EffectedSalaryPerDay * SalaryPaidDays));
                            ViewState["EffectedTotalPaidBasicSalary"] = EffectedTotalPaidBasicSalary;

                            EffectedTotalDA = Converter((EffectedTotalPaidBasicSalary * DA) / 100);
                            ViewState["EffectedTotalDA"] = EffectedTotalDA;

                            if (PFDeduct == "1" || PFDeduct == "3")
                            {
                                cmd = new SqlCommand("GetSetUpDetails", con);
                                cmd.Parameters.AddWithValue("@GradePay", 0);
                                cmd.CommandType = CommandType.StoredProcedure;
                                con.Open();
                                DataSet ds = new DataSet();
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                con.Close();

                                decimal GetEffectedBasicPlusDA = Converter((EffectedTotalPaidBasicSalary) + (EffectedTotalPaidBasicSalary * DA) / 100);
                                decimal GetEffectedPFMaxRange = Convert.ToDecimal(ds.Tables[5].Rows[0]["MaxRange"].ToString());

                                if (PFDeduct == "1")
                                {

                                    if (GetEffectedBasicPlusDA >= GetEffectedPFMaxRange)
                                    {
                                        EffectedTotalDaForReport = GetEffectedPFMaxRange;
                                    }
                                    else
                                    {
                                        EffectedTotalDaForReport = GetEffectedBasicPlusDA;
                                    }
                                }
                                if (PFDeduct == "3")
                                {
                                    EffectedTotalDaForReport = GetEffectedBasicPlusDA;
                                }
                            }

                            if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                            {
                                EffectedTotalHRA = Converter((EffectedTotalPaidBasicSalary * HRA) / 100);
                            }
                            else
                            {
                                EffectedHRAPerDay = HRA / MonthDays;
                                EffectedTotalHRA = Converter(EffectedHRAPerDay * SalaryPaidDays);
                            }

                            EffectedTotalTransport = Converter((Transport / MonthDays) * SalaryPaidDays);

                            EffectedTotalMedical = Converter((Medical / MonthDays) * SalaryPaidDays);

                            EffectedTotalWashing = Converter((Washing / MonthDays) * SalaryPaidDays);

                            EffectedGrossRevisedSalary = Converter((EffectedTotalPaidBasicSalary + EffectedTotalDA + EffectedTotalHRA + EffectedTotalTransport + EffectedTotalMedical + EffectedTotalWashing));

                            EffectedTotalExGratia = ExtraPaidDaysSalary + Converter((ExGratia / MonthDays) * SalaryPaidDays);

                            EffectedGrossTotal = Converter((EffectedGrossRevisedSalary + EffectedTotalExGratia + ArearAdjust));

                            ViewState["EffectedGrossTotal"] = EffectedGrossTotal;

                            EffectedTotalPF = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblPF")).Text);

                            EffectedTotalTransportRecovery = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblTPTREC")).Text);

                            GISOnBasic.Text = GIS.ToString("0.00");

                            EffectedTotalESI = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblESI")).Text);

                            EffectedTotalDeduction = Converter((EffectedTotalPF + Deduction + TDS + Advance + EffectedTotalTransportRecovery + GIS + EffectedTotalESI));

                            EffectedGrossTotalSalaryAfterDeduction = Converter((EffectedGrossTotal - EffectedTotalDeduction));

                            EffectedTotal = (EffectedGrossRevisedSalary + EffectedTotalExGratia) - (EffectedTotalPF + EffectedTotalESI);

                            float NoofLWPStart = 0;
                            decimal TotalNearestAmount = 0;
                            float NoofLWPEnd = 20;
                            decimal TotalDifference = (Total - EffectedTotal);

                            for (NoofLWPStart = 1; NoofLWPStart <= NoofLWPEnd;)
                            {
                                if (TotalNearestAmount <= TotalDifference)
                                {
                                    TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                    if (TotalNearestAmount <= TotalDifference)
                                    {
                                        NoofLWPStart++;
                                        continue;
                                    }
                                    else
                                    {
                                        NoofLWPStart--;
                                        TotalNearestAmount = (FinalSalaryPerDay * Convert.ToDecimal(NoofLWPStart));
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            decimal FinalTotalNearestAmount = Converter(Convert.ToDecimal(TotalNearestAmount));
                            decimal FinalTotalDedInScale = Converter(Convert.ToDecimal(TotalDifference));
                            SalaryPaidDays = MonthDays - (LWP + Convert.ToDecimal(NoofLWPStart));
                            lblLWP.Text = (LWP + Convert.ToDecimal(NoofLWPStart)).ToString();
                            lblPaidDays.Text = SalaryPaidDays.ToString();

                            // Again Calculate Salary for Effected Scale users on the basis of original Scale with New Salary Paid Days start here.

                            SalaryPerDay = (BasicSalary / MonthDays);

                            TotalPaidBasicSalary = Converter((SalaryPerDay * SalaryPaidDays));
                            ViewState["TotalPaidBasicSalary"] = TotalPaidBasicSalary;

                            TotalDA = Converter((TotalPaidBasicSalary * DA) / 100);
                            ViewState["TotalDA"] = TotalDA;

                            if (PFDeduct == "1" || PFDeduct == "3")
                            {
                                cmd = new SqlCommand("GetSetUpDetails", con);
                                cmd.Parameters.AddWithValue("@GradePay", 0);
                                cmd.CommandType = CommandType.StoredProcedure;
                                con.Open();
                                DataSet ds = new DataSet();
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                con.Close();

                                decimal GetFinalBasicPlusDA = Converter((SalaryPerDay * SalaryPaidDays) + (TotalPaidBasicSalary * DA) / 100);
                                decimal GetFinalPFMaxRange = Convert.ToDecimal(ds.Tables[5].Rows[0]["MaxRange"].ToString());

                                if (PFDeduct == "1")
                                {

                                    if (GetFinalBasicPlusDA >= GetFinalPFMaxRange)
                                    {
                                        TotalDaForReport = GetFinalPFMaxRange;
                                    }
                                    else
                                    {
                                        TotalDaForReport = GetFinalBasicPlusDA;
                                    }
                                }
                                if (PFDeduct == "3")
                                {
                                    TotalDaForReport = GetFinalBasicPlusDA;
                                }
                            }

                            if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                            {
                                TotalHRA = Converter((TotalPaidBasicSalary * HRA) / 100);
                            }
                            else
                            {
                                HRAPerDay = HRA / MonthDays;
                                TotalHRA = Converter(HRAPerDay * SalaryPaidDays);
                            }

                            TotalTransport = Converter((Transport / MonthDays) * SalaryPaidDays);

                            TotalMedical = Converter((Medical / MonthDays) * SalaryPaidDays);

                            TotalWashing = Converter((Washing / MonthDays) * SalaryPaidDays);

                            GrossRevisedSalary = Converter((TotalPaidBasicSalary + TotalDA + TotalHRA + TotalTransport + TotalMedical + TotalWashing));

                            TotalExGratia = ExtraPaidDaysSalary + Converter((ExGratia / MonthDays) * SalaryPaidDays);

                            GrossTotal = Converter((GrossRevisedSalary + TotalExGratia + ArearAdjust));
                            ViewState["GrossTotal"] = GrossTotal;

                            TotalPF = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblPF")).Text);

                            TotalESI = Convert.ToDecimal(((Label)grdActualSalary.Rows[0].FindControl("lblESI")).Text);

                            decimal CalculateTotalForAssignTotalDeduction = (GrossTotal - (TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                            Deduction = Deduction + CalculateTotalForAssignTotalDeduction - EffectedGrossTotalSalaryAfterDeduction;

                            decimal SystemDeduction = 0;
                            SystemDeduction = CalculateTotalForAssignTotalDeduction - EffectedGrossTotalSalaryAfterDeduction;

                            // code here                                

                            TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                            GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));

                            // Again Calculate Salary for Effected Scale users on the basis of original Scale with New Salary Paid Days End here.
                        }

                        // Calculate Salary On Effected Basic with Total salary Paid Days End here.
                    }

                    if (SalaryPaidDays == 0)
                    {
                        TotalDeduction = Converter((Deduction + TDS + Advance + TotalTransportRecovery + GIS));
                    }

                    if (GrossTotal >= TotalDeduction)
                    {
                        lblPayDrawnBasic.Text = TotalPaidBasicSalary.ToString("0.00");
                        lblDaOnBasic.Text = TotalDA.ToString("0.00");
                        lblDaForReportOnBasic.Text = TotalDaForReport.ToString("0.00");
                        lblHraOnBasic.Text = TotalHRA.ToString("0.00");
                        lblTransportOnBasic.Text = TotalTransport.ToString("0.00");
                        lblMedicalOnBasic.Text = TotalMedical.ToString("0.00");
                        lblWashingOnBasic.Text = TotalWashing.ToString("0.00");
                        lblGrossRevisedsalary.Text = GrossRevisedSalary.ToString("0.00");
                        lblExGratiaOnBasic.Text = TotalExGratia.ToString("0.00");
                        lblArearAdjust.Text = ArearAdjust.ToString("0.00");
                        lblGrossTotal.Text = GrossTotal.ToString("0.00");
                        lblPFOnBasic.Text = TotalPF.ToString("0.00");
                        lblDeduction.Text = Deduction.ToString("0.00"); GISOnBasic.Text = GIS.ToString("0.00");
                        lblTDS.Text = TDS.ToString("0.00");
                        lblAdvance.Text = Advance.ToString("0.00");
                        lblTPTRECOnBasic.Text = TotalTransportRecovery.ToString("0.00");
                        ESIOnBasic.Text = TotalESI.ToString("0.00");
                        lblTotalDeduction.Text = TotalDeduction.ToString("0.00");
                        lblGrossTotalSalary.Text = GrossTotalSalaryAfterDeduction.ToString("0.00");
                        ExtraPaidDaysSalary = 0;
                        lblArrearGrossTotalSalary.Text = GrossTotalSalaryAfterDeduction.ToString("0.00");
                        lblPaidDays.Enabled = false;
                        lblTransportOnBasic.Enabled = false;
                        lblMedicalOnBasic.Enabled = false;
                        lblExGratiaOnBasic.Enabled = false;
                        lblDAApply.Enabled = false;
                        lblHRAApply.Enabled = false;
                        lblBasicScale.Enabled = false;
                    }
                    else
                    {
                        grdrecord.DataSource = null;
                        grdrecord.DataBind();
                        ViewState["PFDeduct"] = null;
                        ViewState["ESIDeduct"] = null;
                        ViewState["TotalPaidBasicSalary"] = null;
                        ViewState["TotalDA"] = null;
                        ViewState["GrossTotal"] = null;
                        ViewState["AssignPFValue"] = null;
                        ViewState["AssignESIValue"] = null;
                        ViewState["EffectedTotalPaidBasicSalary"] = null;
                        ViewState["EffectedTotalDA"] = null;
                        ViewState["EffectedGrossTotal"] = null;
                        ViewState["EffectedAssignPFValue"] = null;
                        ViewState["EffectedAssignESIValue"] = null;
                        lblPaidDays.Enabled = true;
                        lblTransportOnBasic.Enabled = true;
                        lblMedicalOnBasic.Enabled = true;
                        lblExGratiaOnBasic.Enabled = true;
                        lblDAApply.Enabled = true;
                        lblHRAApply.Enabled = true;
                        lblBasicScale.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Emp Name = " + Name + "\\n" + "Emp_Code = " + Emp_Code +
                            "\\n" + "Gross Total = " + GrossTotal + "\\n" + "Gross Deduction = " + TotalDeduction + "\\n" +
                            "Failed : Gross Total Should be (Equal or Greater) than Total Deduction.');", true);
                        break;
                    }
                }

                i++;
                ViewState["PFDeduct"] = null;
                ViewState["ESIDeduct"] = null;
                ViewState["TotalPaidBasicSalary"] = null;
                ViewState["TotalDA"] = null;
                ViewState["GrossTotal"] = null;
                ViewState["AssignPFValue"] = null;
                ViewState["AssignESIValue"] = null;
                ViewState["EffectedTotalPaidBasicSalary"] = null;
                ViewState["EffectedTotalDA"] = null;
                ViewState["EffectedGrossTotal"] = null;
                ViewState["EffectedAssignPFValue"] = null;
                ViewState["EffectedAssignESIValue"] = null;
            }
            pnlButtons.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateArrear()
    {
        try
        {
            decimal ActualSalary = Convert.ToDecimal(lblActualGrossTotalSalary.Text);
            decimal ArrearSalary = Convert.ToDecimal(lblArrearGrossTotalSalary.Text);
            decimal TotalArrearAmount = ArrearSalary - ActualSalary;
            decimal PreArrearValue = Convert.ToDecimal(lblPreArrearAmount.Text);
            decimal ArrearValue = TotalArrearAmount + PreArrearValue;
            lblTotalArrearAmount.Text = TotalArrearAmount.ToString("0.00");
            lblFinalArrearAmount.Text = Converter(ArrearValue).ToString("0.00");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void lblPaidDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetLWP();
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
            GetPrint();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetPrint()
    {
        try
        {
            int SalaryMonthID = Convert.ToInt32(ddlMonth.SelectedValue);
            int SalaryYearID = Convert.ToInt32(ddlYear.SelectedValue);
            int ArrearMonthID = Convert.ToInt32(ddlArrearMonth.SelectedValue);
            int ArrearYearID = Convert.ToInt32(ddlArrearYear.SelectedValue);

            decimal ArearValue = Convert.ToDecimal(lblFinalArrearAmount.Text);
            if (ArearValue > 0)
            {
                if (Convert.ToInt32(ddlArrearMonth.SelectedValue) > 0 && Convert.ToInt32(ddlArrearYear.SelectedValue) > 0)
                {
                    lblCalculatedText.Text = string.Empty;
                    lblCalculatedText.Text = "Arrear Report of " + ddlemployee.SelectedItem.Text + " For the Month of " + ddlArrearMonth.SelectedItem.Text + " - " + ddlArrearYear.SelectedItem.Text;
                    Session["ctrl"] = pnlPrint;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
                }
                else
                {
                    lblCalculatedText.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Arrear Amount should be greater than 0');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CheckPreValue()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowArrearDetails", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlArrearMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlArrearYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 1);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblPreArrearAmount.Text = dt.Rows[0]["ArearValue"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            }
            else
            {
                lblPreArrearAmount.Text = "0.00";
                txtRemarks.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlArrearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlArrearMonth.SelectedValue) > 0 && Convert.ToInt32(ddlArrearYear.SelectedValue) > 0)
            {
                CheckPreValue();
                CalculateArrear();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlArrearYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlArrearMonth.SelectedValue) > 0 && Convert.ToInt32(ddlArrearYear.SelectedValue) > 0)
            {
                CheckPreValue();
                CalculateArrear();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}