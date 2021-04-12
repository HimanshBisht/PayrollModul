using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Drawing;

public partial class SalaryModule_MidMonthIncrementSalaryMaking : System.Web.UI.Page
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

    public enum Status
    {
        Active = 1,
        Deactive = 0
    }

    public enum ModeOFSalary
    {
        Cheque = 1,
        BankAccount = 2,
        Cash = 3
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
                Employee();
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
            ddlMonth.ClearSelection();
            ddlYear.Enabled = true;
            ddlYear.ClearSelection();
            ddlemployee.Enabled = true;
            ddlemployee.ClearSelection();
            txtCurrentBasic.Text = string.Empty;
            txtEffectedCurrentBasicFrom.Text = string.Empty;
            CalEffectedCurrentBasicFrom.StartDate = null;
            CalEffectedCurrentBasicFrom.EndDate = null;
            txtCurrentBasicMonthDays.Text = string.Empty;
            txtCurrentBasicPaidDays.Text = string.Empty;
            pnlCurrentBasic.Visible = false;
            MakeEmpty();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void MakeEmpty()
    {
        try
        {
            grdCalculateCurrentBasic.DataSource = null;
            grdCalculateCurrentBasic.DataBind();
            pnlCalculateCurrentBasic.Visible = false;
            txtPreviousBasic.Text = string.Empty;
            txtPreviousBasicPaidDays.Text = string.Empty;
            pnlPreviousBasic.Visible = false;
            MakeEmptyFromPrevious();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void MakeEmptyFromPrevious()
    {
        try
        {
            grdCalculatePreviousBasic.DataSource = null;
            grdCalculatePreviousBasic.DataBind();
            pnlCalculatePreviousBasic.Visible = false;
            pnlFinalSalary.Visible = false;
            grdCalculateFinalSalary.DataSource = null;
            grdCalculateFinalSalary.DataBind();
            pnlCalculateFinalSalary.Visible = false;
            pnlButtons.Visible = false;
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

    public void Employee()
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

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    public void BindEmpDetails()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                ddlMonth.Enabled = false;
                ddlYear.Enabled = false;
                ddlemployee.Enabled = false;
                pnlCurrentBasic.Visible = true;
                txtCurrentBasic.Text = dt.Rows[0]["BasicScale"].ToString();
                CalEffectedCurrentBasicFrom.StartDate = Convert.ToDateTime("01" + ddlMonth.SelectedItem.Text + ddlYear.SelectedItem.Text);
                int MonthLastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedItem.Text), Convert.ToInt32(ddlMonth.SelectedValue));
                CalEffectedCurrentBasicFrom.EndDate = Convert.ToDateTime(MonthLastDay + ddlMonth.SelectedItem.Text + ddlYear.SelectedItem.Text);
                txtCurrentBasicMonthDays.Text = MonthLastDay.ToString();
            }
            else
            {
                ddlMonth.Enabled = true;
                ddlYear.Enabled = true;
                ddlemployee.Enabled = true;
                pnlCurrentBasic.Visible = false;
                pnlPreviousBasic.Visible = false;
                txtCurrentBasic.Text = string.Empty;
                txtEffectedCurrentBasicFrom.Text = string.Empty;
                txtPreviousBasic.Text = string.Empty;
                CalEffectedCurrentBasicFrom.StartDate = null;
                CalEffectedCurrentBasicFrom.EndDate = null;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Salary is Already Calculated by Salary Making Page of this Employee For this Month and Year.');", true);
            }
            else
            {
                BindEmpDetails();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateDataForCurrentBasic()
    {
        try
        {
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlMonth.SelectedValue) > 0)
            {
                int MonthDays = 0;
                MonthDays = Convert.ToInt32(txtCurrentBasicMonthDays.Text);
                pnlCalculateCurrentBasic.Visible = true;

                foreach (GridViewRow row in grdCalculateCurrentBasic.Rows)
                {
                    Label lblMonthDays = (Label)row.FindControl("lblMonthDays");
                    Label lblPaidDays = (Label)row.FindControl("lblPaidDays");
                    Label lblLWP = (Label)row.FindControl("lblLWP");
                    Label lblAdvance = (Label)row.FindControl("lblAdvance");
                    Label lblArearAdjust = (Label)row.FindControl("lblArearAdjust");
                    Label lblDeduction = (Label)row.FindControl("lblDeduction");
                    Label lblTDS = (Label)row.FindControl("lblTDS");
                    Label lblGISOnBasic = (Label)row.FindControl("lblGISOnBasic");
                    Label lblTPTRECOnBasic = (Label)row.FindControl("lblTPTRECOnBasic");

                    lblMonthDays.Text = MonthDays.ToString();
                    lblPaidDays.Text = Convert.ToDecimal(txtCurrentBasicPaidDays.Text).ToString("0.00");
                    decimal LWP = MonthDays - Convert.ToDecimal(txtCurrentBasicPaidDays.Text);
                    lblLWP.Text = Convert.ToDecimal(LWP.ToString()).ToString("0.00");
                    lblAdvance.Text = "0.00";
                    lblArearAdjust.Text = "0.00";
                    lblDeduction.Text = "0.00";
                    lblTDS.Text = "0.00";
                    lblGISOnBasic.Text = "0.00";
                    lblTPTRECOnBasic.Text = "0.00";
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

    public void BindCurrentBasicGrid()
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
                grdCalculateCurrentBasic.DataSource = dt;
                grdCalculateCurrentBasic.DataBind();
                if (dt.Rows.Count > 0)
                {
                    CalculateDataForCurrentBasic();
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

    public void CalculateCurrentBasicSalary()
    {
        try
        {
            int i = 0;
            foreach (GridViewRow row in grdCalculateCurrentBasic.Rows)
            {
                string Name = ((Label)row.FindControl("lblName")).Text;
                string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                Label lblLWP = (Label)row.FindControl("lblLWP");
                Label lblPaidDays = (Label)row.FindControl("lblPaidDays");
                Label lblCurrentBasicScale = (Label)row.FindControl("lblBasicScale");
                Label lblPayDrawnBasic = (Label)row.FindControl("lblPayDrawnBasic");
                Label lblDaOnBasic = (Label)row.FindControl("lblDaOnBasic");
                Label lblDaForReportOnBasic = (Label)row.FindControl("lblDaForReportOnBasic");
                Label lblHraOnBasic = (Label)row.FindControl("lblHraOnBasic");
                Label lblTransportOnBasic = (Label)row.FindControl("lblTransportOnBasic");
                Label lblMedicalOnBasic = (Label)row.FindControl("lblMedicalOnBasic");
                Label lblWashingOnBasic = (Label)row.FindControl("lblWashingOnBasic");
                Label lblGrossRevisedsalary = (Label)row.FindControl("lblGrossRevisedsalary");
                Label lblExGratiaOnBasic = (Label)row.FindControl("lblExGratiaOnBasic");
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
                    decimal BasicSalary = Convert.ToDecimal(txtCurrentBasic.Text.ToString());
                    decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                    decimal SalaryPaidDays = Convert.ToDecimal(((Label)row.FindControl("lblPaidDays")).Text);
                    decimal DA = Convert.ToDecimal(dt.Rows[i]["DA"].ToString());
                    if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                    {
                        string[] HRAText = dt.Rows[i]["SelectHRA"].ToString().Split('%');
                        HRA = Convert.ToDecimal(HRAText[0]);
                    }
                    else
                    {
                        HRA = Convert.ToDecimal(dt.Rows[i]["SelectHRA"].ToString());
                    }
                    decimal Transport = Convert.ToDecimal(dt.Rows[i]["TransportValue"].ToString());
                    decimal Medical = Convert.ToDecimal(dt.Rows[i]["MedicalValue"].ToString());
                    decimal Washing = Convert.ToDecimal(dt.Rows[i]["WashingValue"].ToString());
                    decimal ExGratia = Convert.ToDecimal(dt.Rows[i]["ExGratiaValue"].ToString());
                    decimal ArearAdjust = Convert.ToDecimal(((Label)row.FindControl("lblArearAdjust")).Text);
                    string PFDeduct = dt.Rows[i]["PFDeduct"].ToString();
                    decimal PF = Convert.ToDecimal(dt.Rows[i]["PFValue"].ToString());
                    decimal Deduction = Convert.ToDecimal(((Label)row.FindControl("lblDeduction")).Text);
                    decimal TDS = Convert.ToDecimal(((Label)row.FindControl("lblTDS")).Text);
                    decimal Advance = Convert.ToDecimal(((Label)row.FindControl("lblAdvance")).Text);
                    decimal TptRec = Convert.ToDecimal(((Label)row.FindControl("lblTPTRECOnBasic")).Text);
                    decimal GIS = Convert.ToDecimal(((Label)row.FindControl("lblGISOnBasic")).Text);
                    string ESIDeduct = dt.Rows[i]["ESIDeduct"].ToString();
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
                                ///SalaryPaidDays = SalaryPaidDays - FractionValue; close by him
                                lblLWP.Text = (MonthDays - SalaryPaidDays).ToString("0.00");
                                lblPaidDays.Text = SalaryPaidDays.ToString("0.00");
                            }
                        }

                        if (SalaryPaidDays > MonthDays)
                        {
                            decimal ExtraPaidDays = 0;
                            ExtraPaidDays = SalaryPaidDays - MonthDays;
                            SalaryPaidDays = SalaryPaidDays - ExtraPaidDays;
                            lblLWP.Text = (SalaryPaidDays - MonthDays).ToString("0.00");
                            lblPaidDays.Text = SalaryPaidDays.ToString("0.00");
                        }

                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * SalaryPaidDays));

                        TotalDA = Converter((TotalPaidBasicSalary * DA) / 100);

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

                        //TotalExGratia = ExtraPaidDaysSalary + Converter((ExGratia / MonthDays) * SalaryPaidDays); //close by him
                        TotalExGratia =  Converter((ExGratia / MonthDays) * SalaryPaidDays);

                        GrossTotal = Converter((GrossRevisedSalary + TotalExGratia + ArearAdjust));

                        TotalPF = 0;

                        TotalTransportRecovery = Converter((TptRec / MonthDays) * SalaryPaidDays);

                        TotalESI = 0;

                        TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));

                        Total = (GrossRevisedSalary + TotalExGratia) - (TotalPF + TotalESI);
                        FinalSalaryPerDay = Converter((Total / SalaryPaidDays));
                    }

                    if (SalaryPaidDays == 0)
                    {
                        TotalDeduction = Converter((Deduction + TDS + Advance + TotalTransportRecovery + GIS));
                    }

                    if (GrossTotal >= TotalDeduction)
                    {
                        lblCurrentBasicScale.Text = BasicSalary.ToString("0.00");
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
                        lblDeduction.Text = Deduction.ToString("0.00");
                        GISOnBasic.Text = GIS.ToString("0.00");
                        lblTDS.Text = TDS.ToString("0.00");
                        lblAdvance.Text = Advance.ToString("0.00");
                        lblTPTRECOnBasic.Text = TotalTransportRecovery.ToString("0.00");
                        ESIOnBasic.Text = TotalESI.ToString("0.00");
                        lblTotalDeduction.Text = TotalDeduction.ToString("0.00");
                        lblGrossTotalSalary.Text = GrossTotalSalaryAfterDeduction.ToString("0.00");
                        ExtraPaidDaysSalary = 0;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Emp Name = " + Name + "\\n" + "Emp_Code = " + Emp_Code +
                            "\\n" + "Gross Total = " + GrossTotal + "\\n" + "Gross Deduction = " + TotalDeduction + "\\n" +
                            "Failed : Gross Total Should be (Equal or Greater) than Total Deduction.');", true);
                        break;
                    }
                }

                i++;
            }

            FillPreviousData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void FillPreviousData()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", Convert.ToInt32(ddlMonth.SelectedValue) - 1);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                txtPreviousBasic.Text = dt.Rows[0]["Basic"].ToString();
            }
            else
            {
                txtPreviousBasic.Text = string.Empty;
            }

            decimal MonthDays = Convert.ToDecimal(txtCurrentBasicMonthDays.Text);
            decimal CurrentBasicPaidDays = Convert.ToDecimal(txtCurrentBasicPaidDays.Text);
            txtPreviousBasicPaidDays.Text = (MonthDays - CurrentBasicPaidDays).ToString("0.00");
            pnlPreviousBasic.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtEffectedCurrentBasicFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int MonthDays = Convert.ToInt32(txtCurrentBasicMonthDays.Text);
            string[] CurrentBasicEffectedDate = txtEffectedCurrentBasicFrom.Text.Split('-');
            int GetCurrentBasicDate = Convert.ToInt32(CurrentBasicEffectedDate[0]);
            txtCurrentBasicPaidDays.Text = Convert.ToDecimal((MonthDays - GetCurrentBasicDate + 1).ToString()).ToString("0.00");

            grdCalculateCurrentBasic.DataSource = null;
            grdCalculateCurrentBasic.DataBind();
            pnlCalculateCurrentBasic.Visible = false;
            txtPreviousBasic.Text = string.Empty;
            txtPreviousBasicPaidDays.Text = string.Empty;
            pnlPreviousBasic.Visible = false;
            grdCalculatePreviousBasic.DataSource = null;
            grdCalculatePreviousBasic.DataBind();
            pnlCalculatePreviousBasic.Visible = false;
            pnlFinalSalary.Visible = false;
            grdCalculateFinalSalary.DataSource = null;
            grdCalculateFinalSalary.DataBind();
            pnlCalculateFinalSalary.Visible = false;
            pnlButtons.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCalculateCurrentSalary_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                int MonthDays = Convert.ToInt32(txtCurrentBasicMonthDays.Text);
                string[] CurrentBasicEffectedDate = txtEffectedCurrentBasicFrom.Text.Split('-');
                int GetCurrentBasicDate = Convert.ToInt32(CurrentBasicEffectedDate[0]);
                decimal MaxPaidDays = 0;
                decimal AssignCurrentBasicPaidDays = 0;
                MaxPaidDays = MonthDays - GetCurrentBasicDate + 1;
                AssignCurrentBasicPaidDays = Convert.ToDecimal(txtCurrentBasicPaidDays.Text);

                if (AssignCurrentBasicPaidDays > MaxPaidDays)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('In this Case Maximum Paid Days is up to " + MaxPaidDays + ".');", true);
                }
                else
                {
                    grdCalculatePreviousBasic.DataSource = null;
                    grdCalculatePreviousBasic.DataBind();
                    pnlCalculatePreviousBasic.Visible = false;
                    BindCurrentBasicGrid();
                    CalculateCurrentBasicSalary();
                }
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

    public void CalculateDataForPreviousBasic()
    {
        try
        {
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlMonth.SelectedValue) > 0)
            {
                int MonthDays = 0;
                MonthDays = Convert.ToInt32(txtCurrentBasicMonthDays.Text);
                string[] CurrentBasicEffectedDate = txtEffectedCurrentBasicFrom.Text.Split('-');
                int GetCurrentBasicDate = Convert.ToInt32(CurrentBasicEffectedDate[0]);
                pnlCalculatePreviousBasic.Visible = true;

                foreach (GridViewRow row in grdCalculatePreviousBasic.Rows)
                {
                    Label lblMonthDays = (Label)row.FindControl("lblMonthDays");
                    Label lblPaidDaysPreviousBasic = (Label)row.FindControl("lblPaidDays");
                    Label lblLWP = (Label)row.FindControl("lblLWP");
                    Label lblAdvance = (Label)row.FindControl("lblAdvance");
                    Label lblArearAdjust = (Label)row.FindControl("lblArearAdjust");
                    Label lblDeduction = (Label)row.FindControl("lblDeduction");
                    Label lblTDS = (Label)row.FindControl("lblTDS");
                    Label lblGISOnBasic = (Label)row.FindControl("lblGISOnBasic");
                    Label lblTPTRECOnBasic = (Label)row.FindControl("lblTPTRECOnBasic");


                    lblMonthDays.Text = MonthDays.ToString();
                    lblPaidDaysPreviousBasic.Text = Convert.ToDecimal((txtPreviousBasicPaidDays.Text).ToString()).ToString("0.00");
                    decimal LWP = MonthDays - Convert.ToDecimal(txtPreviousBasicPaidDays.Text);
                    lblLWP.Text = Convert.ToDecimal(LWP.ToString()).ToString("0.00");
                    lblAdvance.Text = "0.00";
                    lblArearAdjust.Text = "0.00";
                    lblDeduction.Text = "0.00";
                    lblTDS.Text = "0.00";
                    lblGISOnBasic.Text = "0.00";
                    lblTPTRECOnBasic.Text = "0.00";
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

    public void BindPreviousBasicgrid()
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
                grdCalculatePreviousBasic.DataSource = dt;
                grdCalculatePreviousBasic.DataBind();
                if (dt.Rows.Count > 0)
                {
                    CalculateDataForPreviousBasic();
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

    public void CalculatePreviousBasicSalary()
    {
        try
        {
            int i = 0;
            foreach (GridViewRow row in grdCalculatePreviousBasic.Rows)
            {
                string Name = ((Label)row.FindControl("lblName")).Text;
                string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                Label lblLWP = (Label)row.FindControl("lblLWP");
                Label lblPaidDays = (Label)row.FindControl("lblPaidDays");
                Label lblPreviousBasicScale = (Label)row.FindControl("lblBasicScale");
                Label lblPayDrawnBasic = (Label)row.FindControl("lblPayDrawnBasic");
                Label lblDaOnBasic = (Label)row.FindControl("lblDaOnBasic");
                Label lblDaForReportOnBasic = (Label)row.FindControl("lblDaForReportOnBasic");
                Label lblHraOnBasic = (Label)row.FindControl("lblHraOnBasic");
                Label lblTransportOnBasic = (Label)row.FindControl("lblTransportOnBasic");
                Label lblMedicalOnBasic = (Label)row.FindControl("lblMedicalOnBasic");
                Label lblWashingOnBasic = (Label)row.FindControl("lblWashingOnBasic");
                Label lblGrossRevisedsalary = (Label)row.FindControl("lblGrossRevisedsalary");
                Label lblExGratiaOnBasic = (Label)row.FindControl("lblExGratiaOnBasic");
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
                    decimal BasicSalary = Convert.ToDecimal(txtPreviousBasic.Text.ToString());
                    decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                    decimal SalaryPaidDays = Convert.ToDecimal(((Label)row.FindControl("lblPaidDays")).Text);
                    decimal DA = Convert.ToDecimal(dt.Rows[i]["DA"].ToString());
                    if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                    {
                        string[] HRAText = dt.Rows[i]["SelectHRA"].ToString().Split('%');
                        HRA = Convert.ToDecimal(HRAText[0]);
                    }
                    else
                    {
                        HRA = Convert.ToDecimal(dt.Rows[i]["SelectHRA"].ToString());
                    }
                    decimal Transport = Convert.ToDecimal(dt.Rows[i]["TransportValue"].ToString());
                    decimal Medical = Convert.ToDecimal(dt.Rows[i]["MedicalValue"].ToString());
                    decimal Washing = Convert.ToDecimal(dt.Rows[i]["WashingValue"].ToString());
                    decimal ExGratia = Convert.ToDecimal(dt.Rows[i]["ExGratiaValue"].ToString());
                    decimal ArearAdjust = Convert.ToDecimal(((Label)row.FindControl("lblArearAdjust")).Text);
                    string PFDeduct = dt.Rows[i]["PFDeduct"].ToString();
                    decimal PF = Convert.ToDecimal(dt.Rows[i]["PFValue"].ToString());
                    decimal Deduction = Convert.ToDecimal(((Label)row.FindControl("lblDeduction")).Text);
                    decimal TDS = Convert.ToDecimal(((Label)row.FindControl("lblTDS")).Text);
                    decimal Advance = Convert.ToDecimal(((Label)row.FindControl("lblAdvance")).Text);
                    decimal TptRec = Convert.ToDecimal(((Label)row.FindControl("lblTPTRECOnBasic")).Text);
                    decimal GIS = Convert.ToDecimal(((Label)row.FindControl("lblGISOnBasic")).Text);
                    string ESIDeduct = dt.Rows[i]["ESIDeduct"].ToString();
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
                            }
                        }

                        if (SalaryPaidDays > MonthDays)
                        {
                            decimal ExtraPaidDays = 0;
                            ExtraPaidDays = SalaryPaidDays - MonthDays;
                            SalaryPaidDays = SalaryPaidDays - ExtraPaidDays;
                            lblLWP.Text = (SalaryPaidDays - MonthDays).ToString("0.00");
                            lblPaidDays.Text = SalaryPaidDays.ToString("0.00");
                        }

                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * SalaryPaidDays));

                        TotalDA = Converter((TotalPaidBasicSalary * DA) / 100);

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

                        TotalPF = 0;

                        TotalTransportRecovery = Converter((TptRec / MonthDays) * SalaryPaidDays);

                        TotalESI = 0;

                        TotalDeduction = Converter((TotalPF + Deduction + TDS + Advance + TotalTransportRecovery + GIS + TotalESI));

                        GrossTotalSalaryAfterDeduction = Converter((GrossTotal - TotalDeduction));

                        Total = (GrossRevisedSalary + TotalExGratia) - (TotalPF + TotalESI);
                        FinalSalaryPerDay = Converter((Total / SalaryPaidDays));
                    }

                    if (SalaryPaidDays == 0)
                    {
                        TotalDeduction = Converter((Deduction + TDS + Advance + TotalTransportRecovery + GIS));
                    }

                    if (GrossTotal >= TotalDeduction)
                    {
                        lblPreviousBasicScale.Text = Convert.ToDecimal(txtPreviousBasic.Text).ToString("0.00");
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
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Emp Name = " + Name + "\\n" + "Emp_Code = " + Emp_Code +
                            "\\n" + "Gross Total = " + GrossTotal + "\\n" + "Gross Deduction = " + TotalDeduction + "\\n" +
                            "Failed : Gross Total Should be (Equal or Greater) than Total Deduction.');", true);
                        break;
                    }
                }

                i++;
                pnlFinalSalary.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCalculatePreviousSalary_Click(object sender, EventArgs e)
    {
        try
        {
            int TotalDaysInMonth = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedItem.Text), Convert.ToInt32(ddlMonth.SelectedValue));
            BindPreviousBasicgrid();
            CalculatePreviousBasicSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindFinalSalarygrid()
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
                grdCalculateFinalSalary.DataSource = dt;
                grdCalculateFinalSalary.DataBind();
                pnlCalculateFinalSalary.Visible = true;
                if (dt.Rows.Count > 0)
                {
                    GetImportAllDataForFinalSalary();
                    GetFinalLWP();
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

    public void GetImportAllDataForFinalSalary()
    {
        try
        {
            foreach (GridViewRow row in grdCalculateFinalSalary.Rows)
            {
                Label lblMonthDays = (Label)row.FindControl("lblMonthDays");
                Label lblAdvance = (Label)row.FindControl("lblAdvance");
                Label lblArearAdjust = (Label)row.FindControl("lblArearAdjust");
                Label lblDeduction = (Label)row.FindControl("lblDeduction");
                Label lblPaidDays = (Label)row.FindControl("lblPaidDays");
                Label lblTDS = (Label)row.FindControl("lblTDS");
                string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                string AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                string MonthID = ddlMonth.SelectedValue;
                string YearID = ddlYear.SelectedValue;
                decimal CurrentBasicPaidDays = Convert.ToDecimal(txtCurrentBasicPaidDays.Text);
                decimal PreviousBasicPaidDays = Convert.ToDecimal(txtPreviousBasicPaidDays.Text);
                lblPaidDays.Text = (CurrentBasicPaidDays + PreviousBasicPaidDays).ToString("0.00");
                lblMonthDays.Text = txtCurrentBasicMonthDays.Text;

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

    public void GetFinalLWP()
    {
        try
        {
            foreach (GridViewRow row in grdCalculateFinalSalary.Rows)
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

    public void CalculateFinalSalary()
    {
        try
        {
            decimal CurrentPayDrawnBasic = 0;
            decimal CurrentDaOnBasic = 0;
            decimal CurrentHraOnBasic = 0;
            decimal CurrentTransport = 0;
            decimal CurrentMedical = 0;
            decimal CurrentWashing = 0;
            decimal CurrentGrossRevisedSalary = 0;
            decimal CurrentExGratia = 0;
            decimal PreviousPayDrawnBasic = 0;
            decimal PreviousDaOnBasic = 0;
            decimal PreviousHraOnBasic = 0;
            decimal PreviousTransport = 0;
            decimal PreviousMedical = 0;
            decimal PreviousWashing = 0;
            decimal PreviousGrossRevisedSalary = 0;
            decimal PreviousExGratia = 0;
            decimal FinalPayDrawnBasic = 0;
            decimal FinalDaOnBasic = 0;
            decimal FinalHraOnBasic = 0;
            decimal FinalTransport = 0;
            decimal FinalMedical = 0;
            decimal FinalWashing = 0;
            decimal FinalGrossRevisedSalary = 0;
            decimal FinalExGratia = 0;
            decimal FinalArearAdjust = 0;
            decimal FinalGrossTotal = 0;
            decimal FinalDeduction = 0;
            decimal FinalTDS = 0;
            decimal FinalAdvance = 0;
            decimal FinalGIS = 0;
            decimal FinalTotalDeduction = 0;
            decimal FinalGrossTotalSalary = 0;
            string PFDeduct = "";
            string ESIDeduct = "";
            string Name = "";
            string Emp_Code = "";
            decimal DA = 0;
            decimal TotalDaForReport = 0;
            decimal FinalPFValue = 0;
            decimal FinalESIValue = 0;
            decimal TptRec = 0;
            decimal FinalTransportRecovery = 0;

            foreach (GridViewRow item in grdCalculateCurrentBasic.Rows)
            {
                CurrentPayDrawnBasic = Convert.ToDecimal(((Label)item.FindControl("lblPayDrawnBasic")).Text);
                CurrentDaOnBasic = Convert.ToDecimal(((Label)item.FindControl("lblDaOnBasic")).Text);
                CurrentHraOnBasic = Convert.ToDecimal(((Label)item.FindControl("lblHraOnBasic")).Text);
                CurrentTransport = Convert.ToDecimal(((Label)item.FindControl("lblTransportOnBasic")).Text);
                CurrentMedical = Convert.ToDecimal(((Label)item.FindControl("lblMedicalOnBasic")).Text);
                CurrentWashing = Convert.ToDecimal(((Label)item.FindControl("lblWashingOnBasic")).Text);
                CurrentGrossRevisedSalary = Convert.ToDecimal(((Label)item.FindControl("lblGrossRevisedsalary")).Text);
                CurrentExGratia = Convert.ToDecimal(((Label)item.FindControl("lblExGratiaOnBasic")).Text);
            }

            foreach (GridViewRow data in grdCalculatePreviousBasic.Rows)
            {
                PreviousPayDrawnBasic = Convert.ToDecimal(((Label)data.FindControl("lblPayDrawnBasic")).Text);
                PreviousDaOnBasic = Convert.ToDecimal(((Label)data.FindControl("lblDaOnBasic")).Text);
                PreviousHraOnBasic = Convert.ToDecimal(((Label)data.FindControl("lblHraOnBasic")).Text);
                PreviousTransport = Convert.ToDecimal(((Label)data.FindControl("lblTransportOnBasic")).Text);
                PreviousMedical = Convert.ToDecimal(((Label)data.FindControl("lblMedicalOnBasic")).Text);
                PreviousWashing = Convert.ToDecimal(((Label)data.FindControl("lblWashingOnBasic")).Text);
                PreviousGrossRevisedSalary = Convert.ToDecimal(((Label)data.FindControl("lblGrossRevisedsalary")).Text);
                PreviousExGratia = Convert.ToDecimal(((Label)data.FindControl("lblExGratiaOnBasic")).Text);
            }

            FinalPayDrawnBasic = CurrentPayDrawnBasic + PreviousPayDrawnBasic;
            FinalDaOnBasic = CurrentDaOnBasic + PreviousDaOnBasic;
            FinalHraOnBasic = CurrentHraOnBasic + PreviousHraOnBasic;
            FinalTransport = CurrentTransport + PreviousTransport;
            FinalMedical = CurrentMedical + PreviousMedical;
            FinalWashing = CurrentWashing + PreviousWashing;
            FinalGrossRevisedSalary = CurrentGrossRevisedSalary + PreviousGrossRevisedSalary;
            FinalExGratia = CurrentExGratia + PreviousExGratia;

            foreach (GridViewRow row in grdCalculateFinalSalary.Rows)
            {
                int MonthDays = Convert.ToInt32(((Label)row.FindControl("lblMonthDays")).Text);
                decimal SalaryPaidDays = Convert.ToDecimal(((Label)row.FindControl("lblPaidDays")).Text);
                Label lblPayDrawnBasic = (Label)row.FindControl("lblPayDrawnBasic");
                Label lblDaOnBasic = (Label)row.FindControl("lblDaOnBasic");
                Label lblDaForReportOnBasic = (Label)row.FindControl("lblDaForReportOnBasic");
                Label lblHraOnBasic = (Label)row.FindControl("lblHraOnBasic");
                Label lblTransportOnBasic = (Label)row.FindControl("lblTransportOnBasic");
                Label lblMedicalOnBasic = (Label)row.FindControl("lblMedicalOnBasic");
                Label lblWashingOnBasic = (Label)row.FindControl("lblWashingOnBasic");
                Label lblGrossRevisedsalary = (Label)row.FindControl("lblGrossRevisedsalary");
                Label lblExGratiaOnBasic = (Label)row.FindControl("lblExGratiaOnBasic");
                Label lblGrossTotal = (Label)row.FindControl("lblGrossTotal");
                Label lblPFOnBasic = (Label)row.FindControl("lblPFOnBasic");
                FinalDeduction = Convert.ToDecimal(((Label)row.FindControl("lblDeduction")).Text);
                FinalTDS = Convert.ToDecimal(((Label)row.FindControl("lblTDS")).Text);
                FinalAdvance = Convert.ToDecimal(((Label)row.FindControl("lblAdvance")).Text);
                Label lblTPTRECOnBasic = (Label)row.FindControl("lblTPTRECOnBasic");
                Label lblGISOnBasic = (Label)row.FindControl("lblGisOnBasic");
                Label lblESIOnBasic = (Label)row.FindControl("lblESIOnBasic");
                Label lblTotalDeduction = (Label)row.FindControl("lblTotalDeduction");
                Label lblGrossTotalSalary = (Label)row.FindControl("lblGrossTotalSalary");

                FinalArearAdjust = Convert.ToDecimal(((Label)row.FindControl("lblArearAdjust")).Text);
                FinalDeduction = Convert.ToDecimal(((Label)row.FindControl("lblDeduction")).Text);
                FinalTDS = Convert.ToDecimal(((Label)row.FindControl("lblTDS")).Text);
                FinalAdvance = Convert.ToDecimal(((Label)row.FindControl("lblAdvance")).Text);
                FinalGrossTotal = FinalGrossRevisedSalary + FinalExGratia + FinalArearAdjust;

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

                if (dt.Rows.Count > 0)
                {
                    PFDeduct = dt.Rows[0]["PFDeduct"].ToString();
                    ESIDeduct = dt.Rows[0]["ESIDeduct"].ToString();
                    DA = Convert.ToDecimal(dt.Rows[0]["DA"].ToString());
                    TptRec = Convert.ToDecimal(dt.Rows[0]["TransportRecovery"].ToString());
                    FinalGIS = Convert.ToDecimal(dt.Rows[0]["GISValue"].ToString());
                    Name = dt.Rows[0]["Name"].ToString();
                    Emp_Code = dt.Rows[0]["Emp_Code"].ToString();
                }

                SqlCommand cmdSetUpDetails = new SqlCommand("GetSetUpDetails", con);
                cmdSetUpDetails.Parameters.AddWithValue("@GradePay", 0);
                cmdSetUpDetails.CommandType = CommandType.StoredProcedure;
                DataSet dsSetUpDetails = new DataSet();
                SqlDataAdapter daSetUpDetails = new SqlDataAdapter(cmdSetUpDetails);
                daSetUpDetails.Fill(dsSetUpDetails);

                if (PFDeduct == "1" || PFDeduct == "3")
                {
                    decimal GetBasicPlusDA = Converter((FinalPayDrawnBasic) + (FinalPayDrawnBasic * DA) / 100);
                    decimal GetPFMaxRange = Convert.ToDecimal(dsSetUpDetails.Tables[5].Rows[0]["MaxRange"].ToString());
                    decimal GetPFValue = Convert.ToDecimal(dsSetUpDetails.Tables[5].Rows[0]["PF"].ToString());

                    if (PFDeduct == "1")
                    {

                        if (GetBasicPlusDA >= GetPFMaxRange)
                        {
                            FinalPFValue = Converter((GetPFMaxRange * GetPFValue) / 100);
                        }
                        else if (GetBasicPlusDA < GetPFMaxRange)
                        {
                            FinalPFValue = Converter((GetBasicPlusDA * GetPFValue) / 100);
                        }

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
                        FinalPFValue = Converter((GetBasicPlusDA * GetPFValue) / 100);
                    }
                }

                FinalTransportRecovery = Converter((TptRec / MonthDays) * SalaryPaidDays);

                if (ESIDeduct == "1")
                {
                    decimal GetESIMaxRange = Convert.ToDecimal(dsSetUpDetails.Tables[4].Rows[0]["MaxRange"].ToString());
                    decimal GetESIValue = Convert.ToDecimal(dsSetUpDetails.Tables[4].Rows[0]["ESI"].ToString());
                    FinalESIValue = Math.Ceiling((FinalGrossTotal * GetESIValue) / 100);
                }

                FinalTotalDeduction = Converter((FinalPFValue + FinalDeduction + FinalTDS + FinalAdvance + FinalTransportRecovery + FinalGIS + FinalESIValue));
                FinalGrossTotalSalary = Converter((FinalGrossTotal - FinalTotalDeduction));

                if (FinalGrossTotal >= FinalTotalDeduction)
                {
                    lblPayDrawnBasic.Text = FinalPayDrawnBasic.ToString("0.00");
                    lblDaOnBasic.Text = FinalDaOnBasic.ToString("0.00");
                    lblHraOnBasic.Text = FinalHraOnBasic.ToString("0.00");
                    lblTransportOnBasic.Text = FinalTransport.ToString("0.00");
                    lblMedicalOnBasic.Text = FinalMedical.ToString("0.00");
                    lblWashingOnBasic.Text = FinalWashing.ToString("0.00");
                    lblGrossRevisedsalary.Text = FinalGrossRevisedSalary.ToString("0.00");
                    lblExGratiaOnBasic.Text = FinalExGratia.ToString("0.00");
                    lblGrossTotal.Text = FinalGrossTotal.ToString("0.00");
                    lblDaForReportOnBasic.Text = TotalDaForReport.ToString("0.00");
                    lblPFOnBasic.Text = FinalPFValue.ToString("0.00");
                    lblTPTRECOnBasic.Text = FinalTransportRecovery.ToString("0.00");
                    lblGISOnBasic.Text = FinalGIS.ToString("0.00");
                    lblESIOnBasic.Text = FinalESIValue.ToString("0.00");
                    lblTotalDeduction.Text = FinalTotalDeduction.ToString("0.00");
                    lblGrossTotalSalary.Text = FinalGrossTotalSalary.ToString("0.00");
                    pnlCalculateFinalSalary.Visible = true;
                    pnlButtons.Visible = true;
                }
                else
                {
                    pnlCalculateFinalSalary.Visible = false;
                    pnlButtons.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Emp Name = " + Name + "\\n" + "Emp_Code = " + Emp_Code +
                                "\\n" + "Gross Total = " + FinalGrossTotal + "\\n" + "Gross Deduction = " + FinalTotalDeduction + "\\n" +
                                "Failed : Gross Total Should be (Equal or Greater) than Total Deduction.');", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void bthCalculateFinalSalary_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                BindFinalSalarygrid();
                CalculateFinalSalary();
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

    protected void txtCurrentBasicMonthDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int MaxMonthDays = 0;
            int SelectYear = Convert.ToInt32(ddlYear.SelectedItem.Text);
            MaxMonthDays = System.DateTime.DaysInMonth(SelectYear, Convert.ToInt32(ddlMonth.SelectedValue));
            int MonthDays = Convert.ToInt32(txtCurrentBasicMonthDays.Text);
            if (MonthDays > MaxMonthDays)
            {
                txtCurrentBasicMonthDays.Text = string.Empty;
                txtCurrentBasicPaidDays.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Maximum Month Days is upto " + MaxMonthDays + ".');", true);
            }
            else
            {
                string[] CurrentBasicEffectedDate = txtEffectedCurrentBasicFrom.Text.Split('-');
                int GetCurrentBasicDate = Convert.ToInt32(CurrentBasicEffectedDate[0]);
                txtCurrentBasicPaidDays.Text = Convert.ToDecimal((MonthDays - GetCurrentBasicDate + 1).ToString()).ToString("0.00");
            }

            MakeEmpty();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtCurrentBasicPaidDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            MakeEmpty();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtPreviousBasic_TextChanged(object sender, EventArgs e)
    {
        try
        {
            MakeEmptyFromPrevious();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtPreviousBasicPaidDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            MakeEmptyFromPrevious();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveFinalSalary_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            foreach (GridViewRow row in grdCalculateFinalSalary.Rows)
            {
                int Count = 0;
                int MonthID = Convert.ToInt32(ddlMonth.SelectedValue);
                int YearID = Convert.ToInt32(ddlYear.SelectedValue);
                string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                string NatureID = ((Label)row.FindControl("lblNatureID")).Text;
                string StaffTypeID = ((Label)row.FindControl("lblStaffTypeID")).Text;
                int EmployeeID = Convert.ToInt32(((Label)row.FindControl("lblEmployeeID")).Text);
                string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                string SystemNumber = ((Label)row.FindControl("lblSystemNumber")).Text;
                string AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                string Name = ((Label)row.FindControl("lblName")).Text;
                string Designation = ((Label)row.FindControl("lblDesignation")).Text;
                string BasicScale = ((Label)row.FindControl("lblBasicScale")).Text;
                int MonthDays = Convert.ToInt32(((Label)row.FindControl("lblMonthDays")).Text);
                string LWP = ((Label)row.FindControl("lblLWP")).Text;
                string PaidDays = ((Label)row.FindControl("lblPaidDays")).Text;
                string PayDrawnBasic = ((Label)row.FindControl("lblPayDrawnBasic")).Text;
                string DAApply = ((Label)row.FindControl("lblDAApply")).Text;
                string DaOnBasic = ((Label)row.FindControl("lblDaOnBasic")).Text;
                string HRAApply = ((Label)row.FindControl("lblHRAApply")).Text;
                string HraOnBasic = ((Label)row.FindControl("lblHraOnBasic")).Text;
                string TransportOnBasic = ((Label)row.FindControl("lblTransportOnBasic")).Text;
                string MedicalOnBasic = ((Label)row.FindControl("lblMedicalOnBasic")).Text;
                string WashingOnBasic = ((Label)row.FindControl("lblWashingOnBasic")).Text;
                string GrossRevisedsalaryOnBasic = ((Label)row.FindControl("lblGrossRevisedsalary")).Text;
                string ExGratiaOnBasic = ((Label)row.FindControl("lblExGratiaOnBasic")).Text;
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

                if (GrossTotalSalaryOnBasic == null || GrossTotalSalaryOnBasic == "")
                {
                }
                else
                {
                    SqlConnection con = new SqlConnection(constr);
                    cmd = new SqlCommand("SaveSalaryMaking", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MonthID", MonthID);
                    cmd.Parameters.AddWithValue("@YearID", YearID);
                    cmd.Parameters.AddWithValue("@NatureID", NatureID);
                    cmd.Parameters.AddWithValue("@StaffTypeID", StaffTypeID);
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    cmd.Parameters.AddWithValue("@Emp_Code", Emp_Code);
                    cmd.Parameters.AddWithValue("@SystemNumber", SystemNumber);
                    cmd.Parameters.AddWithValue("@AssignEmpCode", AssignEmpCode);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Designation", Designation);
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
                    con.Open();
                    Count = cmd.ExecuteNonQuery();
                    con.Close();

                    if (Count > 0)
                    {
                        Clear();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Salary is Already Calculated by Salary Making Page of this Employee For this Month and Year.');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
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
}