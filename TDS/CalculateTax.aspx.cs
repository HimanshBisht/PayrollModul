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
using System.Globalization;

public partial class SalaryModule_CalculateTax : System.Web.UI.Page
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

    public enum Status
    {
        All = -1,
        Active = 1,
        Deactive = 0
    }

    public enum City
    {
        All = 0,
        MetroCity = 1,
        NonMetroCity = 0
    }

    public enum MonthDivisableRules
    {
        April = 12,
        May = 11,
        June = 10,
        July = 9,
        August = 8,
        September = 7,
        October = 6,
        November = 5,
        December = 4,
        January = 3,
        February = 1,
        March = 1
    }

    public enum IsApprove
    {
        All = 0,
        Approve = 1,
        NotApprove = 2
    }

    public enum NoticeType
    {
        All = 0,
        Payment = 1,
        Recovery = 2
    }

    public void CheckUserRights()
    {
        try
        {
            int HasMatch = 0;
            string RequestURL = Request.Url.AbsolutePath;
            FileInfo oInfo = new FileInfo(RequestURL);
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

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    public void Reset()
    {
        try
        {
            ddlFromFinancialYear.ClearSelection();
            ddlFromFinancialYear.Enabled = true;
            ddlMonthForSaveTax.ClearSelection();
            ddlMonthForSaveTax.Enabled = true;
            ddlYearForSaveTax.ClearSelection();
            ddlYearForSaveTax.Enabled = true;
            pnlDetail.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlButtons.Visible = false;
            btnSaveTax.Visible = false;
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
            ddlMonthForSaveTax.DataSource = dt;
            ddlMonthForSaveTax.DataTextField = "MonthName";
            ddlMonthForSaveTax.DataValueField = "MonthID";
            ddlMonthForSaveTax.DataBind();
            ddlMonthForSaveTax.Items.Insert(0, new ListItem("Select Month For Save Tax", "0"));
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
            ddlFromFinancialYear.DataSource = dt;
            ddlFromFinancialYear.DataTextField = "Year";
            ddlFromFinancialYear.DataValueField = "YearID";
            ddlFromFinancialYear.DataBind();
            ddlFromFinancialYear.Items.Insert(0, new ListItem("Select From Financial Year", "0"));

            ddlYearForSaveTax.DataSource = dt;
            ddlYearForSaveTax.DataTextField = "Year";
            ddlYearForSaveTax.DataValueField = "YearID";
            ddlYearForSaveTax.DataBind();
            ddlYearForSaveTax.Items.Insert(0, new ListItem("Select Year For Save Tax", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    static string CalculateYourAge(DateTime Dob, int ToYear)
    {
        DateTime ToYearDate = Convert.ToDateTime("31 March" + ToYear);
        int Years = new DateTime(ToYearDate.Subtract(Dob).Ticks).Year - 1;
        DateTime PastYearDate = Dob.AddYears(Years);
        int Months = 0;
        for (int i = 1; i <= 12; i++)
        {
            if (PastYearDate.AddMonths(i) == ToYearDate)
            {
                Months = i;
                break;
            }
            else if (PastYearDate.AddMonths(i) >= ToYearDate)
            {
                Months = i - 1;
                break;
            }
        }
        int Days = ToYearDate.Subtract(PastYearDate.AddMonths(Months)).Days;
        return String.Format("{0} Y, {1} M, {2} D",
        Years, Months, Days);
    }

    public void CalculateTaxableIncome()
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
            grdrecord.DataSource = dt;
            grdrecord.DataBind();

            if (dt.Rows.Count > 0)
            {
                pnlDetail.Visible = true;
                pnlButtons.Visible = true;

                Label lblAgeText = ((Label)this.grdrecord.HeaderRow.FindControl("lblAgeText"));
                lblAgeText.Text = "Age Till March-" + (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1);

                decimal StandardDeduction = 0;

                SqlCommand cmdManageStandardDeduction = new SqlCommand("ManageStandardDeduction", con);
                cmdManageStandardDeduction.CommandType = CommandType.StoredProcedure;
                cmdManageStandardDeduction.Parameters.AddWithValue("@Type", "GetRecords");
                cmdManageStandardDeduction.Parameters.AddWithValue("@User", null);
                DataTable dtManageStandardDeduction = new DataTable();
                SqlDataAdapter daManageStandardDeduction = new SqlDataAdapter(cmdManageStandardDeduction);
                daManageStandardDeduction.Fill(dtManageStandardDeduction);

                if (dtManageStandardDeduction.Rows.Count > 0)
                {
                    StandardDeduction = Convert.ToDecimal(dtManageStandardDeduction.Rows[0]["DedValue"].ToString());
                }

                int j = 0;

                DateTimeFormatInfo DTFI = new DateTimeFormatInfo();
                DataTable Finaldt = new DataTable();
                Finaldt.Columns.AddRange(new DataColumn[20] { new DataColumn("Type", typeof(string)),
                new DataColumn("Month", typeof(string)),
                new DataColumn("Year", typeof(string)),
                new DataColumn("Basic", typeof(decimal)),
                new DataColumn("DA", typeof(decimal)),
                new DataColumn("HRA", typeof(decimal)),
                new DataColumn("Transport", typeof(decimal)),
                new DataColumn("Medical", typeof(decimal)),
                new DataColumn("Washing", typeof(decimal)),
                new DataColumn("ExGratia", typeof(decimal)),
                new DataColumn("ArearAdjust", typeof(decimal)),
                new DataColumn("GrossTotal", typeof(decimal)),
                new DataColumn("PF", typeof(decimal)),
                new DataColumn("Deduction", typeof(decimal)),
                new DataColumn("Advance", typeof(decimal)),
                new DataColumn("TDS", typeof(decimal)),
                new DataColumn("TPTREC", typeof(decimal)),
                new DataColumn("ESI", typeof(decimal)),
                new DataColumn("GIS", typeof(decimal)),
                new DataColumn("GrossTotalSalary", typeof(decimal)) });

                foreach (GridViewRow row in grdrecord.Rows)
                {
                    DateTime DOB = Convert.ToDateTime(dt.Rows[j]["DOB"].ToString());
                    int ToYear = Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1;
                    string Age = CalculateYourAge(DOB, ToYear);

                    Label lblProfileID = (Label)row.FindControl("lblProfileID");
                    Label lblGrossPerAnnum = (Label)row.FindControl("lblGrossPerAnnum");
                    Label lblHRAExemption = (Label)row.FindControl("lblHRAExemption");
                    Label lblAge = (Label)row.FindControl("lblAge");
                    Label lblStandardDeduction = (Label)row.FindControl("lblStandardDeduction");
                    Label lblOtherIDFAdjustment = (Label)row.FindControl("lblOtherIDFAdjustment");
                    Label lblTaxableIncome = (Label)row.FindControl("lblTaxableIncome");
                    Label lblAlreadyPaidTax = (Label)row.FindControl("lblAlreadyPaidTax");

                    lblAge.Text = Age;
                    j++;

                    Finaldt.Clear();
                    decimal TotalBasic = 0;
                    decimal TotalDa = 0;
                    decimal TotalHra = 0;
                    decimal TotalTransport = 0;
                    decimal TotalMedical = 0;
                    decimal TotalWashing = 0;
                    decimal TotalExGratia = 0;
                    decimal TotalArearAdjust = 0;
                    decimal TotalGrossTotal = 0;
                    decimal TotalPF = 0;
                    decimal TotalDeduction = 0;
                    decimal TotalTdsOnBasic = 0;
                    decimal TotalAdvance = 0;
                    decimal TotalTPTREC = 0;
                    decimal TotalGIS = 0;
                    decimal TotalESI = 0;
                    decimal TotalGrossTotalSalary = 0;

                    int YearID = Convert.ToInt32(ddlFromFinancialYear.SelectedValue);
                    int Year = Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text);
                    int MonthID = 4;
                    int i = 0;
                    string DOR = string.Empty;
                    string LWD = string.Empty;
                    string Type = string.Empty;
                    string Month = string.Empty;
                    decimal Basic = 0;
                    decimal DA = 0;
                    decimal HRA = 0;
                    decimal Transport = 0;
                    decimal Medical = 0;
                    decimal Washing = 0;
                    decimal ExGratia = 0;
                    decimal ArrearAdjust = 0;
                    decimal GrossTotal = 0;
                    decimal PF = 0;
                    decimal Deduction = 0;
                    decimal Advance = 0;
                    decimal TDS = 0;
                    decimal TPTREC = 0;
                    decimal ESI = 0;
                    decimal GIS = 0;
                    decimal GrossTotalSalary = 0;
                    int HasFound = 0;

                    for (i = 0; i <= 11; i++)
                    {
                        cmd = new SqlCommand("ShowForm16Report", con);
                        cmd.Parameters.AddWithValue("@ProfileID", lblProfileID.Text);
                        cmd.Parameters.AddWithValue("@MonthID", MonthID);
                        cmd.Parameters.AddWithValue("@YearID", YearID);
                        cmd.Parameters.AddWithValue("@Type", "SalaryMaking");
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataSet dsSalaryMaking = new DataSet();
                        SqlDataAdapter sdaSalaryMaking = new SqlDataAdapter(cmd);
                        sdaSalaryMaking.Fill(dsSalaryMaking);

                        if (dsSalaryMaking.Tables[0].Rows.Count > 0)
                        {
                            HasFound++;
                            Type = "Actual";
                            Month = dsSalaryMaking.Tables[0].Rows[0]["MonthName"].ToString();
                            Year = Convert.ToInt32(dsSalaryMaking.Tables[0].Rows[0]["Year"].ToString());
                            Basic = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["PayDrawnBasic"].ToString());
                            DA = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["DA"].ToString());
                            HRA = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["HRA"].ToString());
                            Transport = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["Transport"].ToString());
                            Medical = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["Medical"].ToString());
                            Washing = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["Washing"].ToString());
                            ExGratia = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["ExGratia"].ToString());
                            ArrearAdjust = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["ArearAdjust"].ToString());
                            GrossTotal = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["GrossTotal"].ToString());
                            PF = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["PF"].ToString());
                            Deduction = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["Deduction"].ToString());
                            Advance = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["Advance"].ToString());
                            TDS = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["TDS"].ToString());
                            TPTREC = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["TPTREC"].ToString());
                            ESI = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["ESI"].ToString());
                            GIS = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["GIS"].ToString());
                            GrossTotalSalary = Convert.ToDecimal(dsSalaryMaking.Tables[0].Rows[0]["GrossTotalSalary"].ToString());
                        }

                        else
                        {
                            Month = string.Empty;
                            Basic = 0;
                            DA = 0;
                            HRA = 0;
                            Transport = 0;
                            Medical = 0;
                            Washing = 0;
                            ExGratia = 0;
                            ArrearAdjust = 0;
                            GrossTotal = 0;
                            PF = 0;
                            Deduction = 0;
                            Advance = 0;
                            TDS = 0;
                            TPTREC = 0;
                            ESI = 0;
                            GIS = 0;
                            GrossTotalSalary = 0;

                            if (HasFound > 0)
                            {
                                SqlCommand cmdProposedSalary = new SqlCommand("ShowForm16Report", con);
                                cmdProposedSalary.Parameters.AddWithValue("@ProfileID", lblProfileID.Text);
                                cmdProposedSalary.Parameters.AddWithValue("@Type", "SalaryProfile");
                                cmdProposedSalary.CommandType = CommandType.StoredProcedure;
                                DataSet dsSalaryProfile = new DataSet();
                                SqlDataAdapter sdaSalaryProfile = new SqlDataAdapter(cmdProposedSalary);
                                sdaSalaryProfile.Fill(dsSalaryProfile);

                                if (dsSalaryProfile.Tables[0].Rows.Count > 0)
                                {
                                    DOR = dsSalaryProfile.Tables[0].Rows[0]["ResignationDate"].ToString();
                                    LWD = dsSalaryProfile.Tables[0].Rows[0]["LWD"].ToString();
                                    HasFound++;
                                    Type = "Proposed";
                                    Month = DTFI.GetMonthName(MonthID);
                                    Year = Convert.ToInt32(Year.ToString());

                                    if (DOR.Length <= 0 && LWD.Length <= 0)
                                    {
                                        Basic = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["BasicScale"].ToString());
                                        DA = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["DAValue"].ToString());
                                        HRA = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["HRAValue"].ToString());
                                        Transport = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["TransportValue"].ToString());
                                        Medical = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["MedicalValue"].ToString());
                                        Washing = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["WashingValue"].ToString());
                                        ExGratia = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["ExGratiaValue"].ToString());
                                        ArrearAdjust = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["ArearAdjust"].ToString());
                                        GrossTotal = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["GrossTotal"].ToString());
                                        PF = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["PFValue"].ToString());
                                        Deduction = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["Deduction"].ToString());
                                        Advance = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["Advance"].ToString());
                                        TDS = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["TDS"].ToString());
                                        TPTREC = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["TransportRecovery"].ToString());
                                        ESI = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["EsiValue"].ToString());
                                        GIS = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["GisValue"].ToString());
                                        GrossTotalSalary = Convert.ToDecimal(dsSalaryProfile.Tables[0].Rows[0]["NetSalary"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                Type = "Undefined";
                            }
                        }

                        Finaldt.Rows.Add(Type, DTFI.GetMonthName(MonthID), Year, Basic.ToString("0.00"), DA.ToString("0.00"), HRA.ToString("0.00"), Transport.ToString("0.00"), Medical.ToString("0.00"), Washing.ToString("0.00"), ExGratia.ToString("0.00"), ArrearAdjust.ToString("0.00"), GrossTotal.ToString("0.00"), PF.ToString("0.00"), Deduction.ToString("0.00"), Advance.ToString("0.00"), TDS.ToString("0.00"), TPTREC.ToString("0.00"), ESI.ToString("0.00"), GIS.ToString("0.00"), GrossTotalSalary.ToString("0.00"));

                        MonthID = MonthID + 1;

                        if (MonthID > 12)
                        {
                            MonthID = 1;
                            Year = Year + 1;

                            SqlCommand cmdMonthNYear = new SqlCommand("ManageMonthNYears", con);
                            cmdMonthNYear.Parameters.AddWithValue("@MonthName", DTFI.GetMonthName(MonthID));
                            cmdMonthNYear.Parameters.AddWithValue("@YEAR", Year);
                            cmdMonthNYear.CommandType = CommandType.StoredProcedure;
                            DataSet dsMonthNYear = new DataSet();
                            SqlDataAdapter daMonthNYear = new SqlDataAdapter(cmdMonthNYear);
                            daMonthNYear.Fill(dsMonthNYear);

                            if (dsMonthNYear.Tables[1].Rows.Count > 0)
                            {
                                YearID = Convert.ToInt32(dsMonthNYear.Tables[1].Rows[0]["YearID"].ToString());
                            }
                            else
                            {
                                con.Close();
                                throw new Exception("Failed : Firstly Create Years after " + dsMonthNYear.Tables[2].Rows[0]["Year"].ToString() + ".");
                            }
                        }
                    }

                    // Now Checking For Notice Period Payment Adjustment.

                    SqlCommand cmdNoticePeriodPayment = new SqlCommand("ManageNoticePeriod", con);
                    cmdNoticePeriodPayment.Parameters.AddWithValue("@ProfileID", lblProfileID.Text);
                    cmdNoticePeriodPayment.Parameters.AddWithValue("@NoticeType", NoticeType.Payment);
                    cmdNoticePeriodPayment.Parameters.AddWithValue("@Type", "GetData");
                    cmdNoticePeriodPayment.CommandType = CommandType.StoredProcedure;
                    DataTable dtNoticePeriodPayment = new DataTable();
                    SqlDataAdapter daNoticePeriodPayment = new SqlDataAdapter(cmdNoticePeriodPayment);
                    daNoticePeriodPayment.Fill(dtNoticePeriodPayment);

                    string NPCaption = string.Empty;
                    decimal NPPayment = 0;

                    if (dtNoticePeriodPayment.Rows.Count > 0)
                    {
                        Type = "Notice Period Payment";
                        NPCaption = "Notice Period Payment";
                        NPPayment = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["FinalNoticeAmount"].ToString());

                        Basic = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Basic"].ToString());
                        DA = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["DA"].ToString());
                        HRA = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["HRA"].ToString());
                        Transport = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Transport"].ToString());
                        Medical = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Medical"].ToString());
                        Washing = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Washing"].ToString());
                        ExGratia = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["ExGratia"].ToString());
                        ArrearAdjust = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["ArearAdjust"].ToString());
                        GrossTotal = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["GrossTotal"].ToString());
                        PF = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["PF"].ToString());
                        Deduction = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Deduction"].ToString());
                        Advance = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["Advance"].ToString());
                        TDS = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["TDS"].ToString());
                        TPTREC = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["TPTREC"].ToString());
                        ESI = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["ESI"].ToString());
                        GIS = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["GIS"].ToString());
                        GrossTotalSalary = Convert.ToDecimal(dtNoticePeriodPayment.Rows[0]["GrossTotalSalary"].ToString());

                        Finaldt.Rows.Add(Type, NPCaption, "", Basic.ToString("0.00"), DA.ToString("0.00"), HRA.ToString("0.00"), Transport.ToString("0.00"), Medical.ToString("0.00"), Washing.ToString("0.00"), ExGratia.ToString("0.00"), ArrearAdjust.ToString("0.00"), GrossTotal.ToString("0.00"), PF.ToString("0.00"), Deduction.ToString("0.00"), Advance.ToString("0.00"), TDS.ToString("0.00"), TPTREC.ToString("0.00"), ESI.ToString("0.00"), GIS.ToString("0.00"), GrossTotalSalary.ToString("0.00"));
                    }

                    // Now Checking For any Addition Amount.

                    SqlCommand cmdAdditionalAmount = new SqlCommand("ShowForm16Report", con);
                    cmdAdditionalAmount.Parameters.AddWithValue("@ProfileID", lblProfileID.Text);
                    cmdAdditionalAmount.Parameters.AddWithValue("@YearID", ddlFromFinancialYear.SelectedValue);
                    cmdAdditionalAmount.Parameters.AddWithValue("@Type", "AdditionalAmount");
                    cmdAdditionalAmount.CommandType = CommandType.StoredProcedure;
                    DataSet dsAdditionalAmount = new DataSet();
                    SqlDataAdapter sdaAdditionalAmount = new SqlDataAdapter(cmdAdditionalAmount);
                    sdaAdditionalAmount.Fill(dsAdditionalAmount);

                    string Caption = string.Empty;
                    decimal AdditionalAmount = 0;
                    string FinancialYear = string.Empty;
                    int RowIndex = 0;

                    if (dsAdditionalAmount.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dsAdditionalAmount.Tables[0].Rows)
                        {
                            Type = "Additional Amount";
                            Caption = dsAdditionalAmount.Tables[0].Rows[RowIndex]["Caption"].ToString();
                            FinancialYear = dsAdditionalAmount.Tables[0].Rows[RowIndex]["FinancialYear"].ToString();
                            AdditionalAmount = Convert.ToDecimal(dsAdditionalAmount.Tables[0].Rows[RowIndex]["AdditionalAmount"].ToString());

                            Finaldt.Rows.Add(Type, Caption, FinancialYear, 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), AdditionalAmount.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"), 0.ToString("0.00"));
                            RowIndex++;
                        }
                    }

                    if (Finaldt.Rows.Count > 0)
                    {
                        TotalBasic = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Basic"));

                        TotalDa = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("DA"));

                        TotalHra = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("HRA"));

                        TotalTransport = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Transport"));

                        TotalMedical = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Medical"));

                        TotalWashing = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Washing"));

                        TotalExGratia = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("ExGratia"));

                        TotalArearAdjust = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("ArearAdjust"));

                        TotalGrossTotal = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("GrossTotal"));

                        TotalPF = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("PF"));

                        TotalDeduction = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Deduction"));

                        TotalTdsOnBasic = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("TDS"));

                        TotalAdvance = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("Advance"));

                        TotalTPTREC = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("TPTREC"));

                        TotalGIS = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("GIS"));

                        TotalESI = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("ESI"));

                        TotalGrossTotalSalary = Finaldt.AsEnumerable().Sum(item => item.Field<decimal>("GrossTotalSalary"));
                    }

                    string ProfileID = lblProfileID.Text;
                    string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                    decimal HRAExemption = 0;

                    decimal MedicalPolicyPremiumMaxValue = 0;
                    decimal MedicalPolicyPremiumActualValue = 0;
                    decimal IntersetOnHousingLoanMaxValue = 0;
                    decimal IntersetOnHousingLoanActualValue = 0;
                    decimal EducationLoanMaxValue = 0;
                    decimal EducationLoanActualValue = 0;
                    decimal BasicDeductionsMaxValue = 0;
                    decimal BasicDeductionsActualValue = 0;
                    decimal NationalPentionSchemeMaxValue = 0;
                    decimal NationalPentionSchemeActualValue = 0;
                    decimal FinalTaxableAmount = 0;
                    decimal FinalTotalDeductions = 0;
                    decimal AccomodationCriteria = 0;
                    decimal RentAmountPerAnnum = 0;
                    decimal BasicPlusDAGrandTotal = 0;
                    decimal ActualHRAReceived = 0;
                    decimal AccommodationCriteriaSalary = 0;
                    decimal ExcessRentPaidSalary = 0;

                    SqlCommand cmdManageTDSIDF = new SqlCommand("ManageTDSIDF", con);
                    cmdManageTDSIDF.CommandType = CommandType.StoredProcedure;
                    cmdManageTDSIDF.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmdManageTDSIDF.Parameters.AddWithValue("@FromYear", FromYear);
                    cmdManageTDSIDF.Parameters.AddWithValue("@ToYear", ToYear);
                    cmdManageTDSIDF.Parameters.AddWithValue("@SeniorCitizen", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@AgeCriteria", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@OthersDetails", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@Place", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@Signature", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@Date", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@User", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@NameAndAddress", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@CityName", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmdManageTDSIDF.Parameters.AddWithValue("@Type", "GetRecords");
                    DataTable dtManageTDSIDF = new DataTable();
                    SqlDataAdapter daManageTDSIDF = new SqlDataAdapter(cmdManageTDSIDF);
                    daManageTDSIDF.Fill(dtManageTDSIDF);
                    BasicDeductionsActualValue = TotalPF;

                    if (dtManageTDSIDF.Rows.Count > 0)
                    {
                        RentAmountPerAnnum = Convert.ToDecimal(dtManageTDSIDF.Rows[0]["RentAmountPerAnnum"].ToString());

                        DataView dv = new DataView(dtManageTDSIDF);
                        dv.RowFilter = "HeaderID=1 and RuleID=2";
                        MedicalPolicyPremiumMaxValue = Convert.ToDecimal(dv[0]["MaxAmount"].ToString());
                        MedicalPolicyPremiumActualValue = Convert.ToDecimal(dv[0]["ActualAmount"].ToString());

                        if (MedicalPolicyPremiumMaxValue > 0)
                        {
                            if (MedicalPolicyPremiumActualValue > MedicalPolicyPremiumMaxValue)
                            {
                                MedicalPolicyPremiumActualValue = MedicalPolicyPremiumMaxValue;
                            }
                        }

                        dv = new DataView(dtManageTDSIDF);
                        dv.RowFilter = "HeaderID=4";
                        IntersetOnHousingLoanMaxValue = Convert.ToDecimal(dv[0]["MaxAmount"].ToString());
                        IntersetOnHousingLoanActualValue = Convert.ToDecimal(dv[0]["ActualAmount"].ToString());

                        if (IntersetOnHousingLoanMaxValue > 0)
                        {
                            if (IntersetOnHousingLoanActualValue > IntersetOnHousingLoanMaxValue)
                            {
                                IntersetOnHousingLoanActualValue = IntersetOnHousingLoanMaxValue;
                            }
                        }

                        dv = new DataView(dtManageTDSIDF);
                        dv.RowFilter = "HeaderID=1 and RuleID=1";
                        EducationLoanMaxValue = Convert.ToDecimal(dv[0]["MaxAmount"].ToString());
                        EducationLoanActualValue = Convert.ToDecimal(dv[0]["ActualAmount"].ToString());

                        if (EducationLoanMaxValue > 0)
                        {
                            if (EducationLoanActualValue > EducationLoanMaxValue)
                            {
                                EducationLoanActualValue = EducationLoanMaxValue;
                            }
                        }

                        dv = new DataView(dtManageTDSIDF);
                        dv.RowFilter = "HeaderID=2";
                        BasicDeductionsActualValue = BasicDeductionsActualValue + dv.ToTable().AsEnumerable().Sum(data => data.Field<decimal>("ActualAmount"));

                        dv = new DataView(dtManageTDSIDF);
                        dv.RowFilter = "HeaderID=3";
                        NationalPentionSchemeMaxValue = Convert.ToDecimal(dv[0]["MaxAmount"].ToString());
                        NationalPentionSchemeActualValue = Convert.ToDecimal(dv[0]["ActualAmount"].ToString());

                        if (NationalPentionSchemeMaxValue > 0)
                        {
                            if (NationalPentionSchemeActualValue > NationalPentionSchemeMaxValue)
                            {
                                NationalPentionSchemeActualValue = NationalPentionSchemeMaxValue;
                            }
                        }
                    }

                    SqlCommand cmdManageHeaderwiseFooter = new SqlCommand("ManageHeaderwiseFooter", con);
                    cmdManageHeaderwiseFooter.CommandType = CommandType.StoredProcedure;
                    cmdManageHeaderwiseFooter.Parameters.AddWithValue("@FooterText", null);
                    cmdManageHeaderwiseFooter.Parameters.AddWithValue("@HeaderID", 2);
                    cmdManageHeaderwiseFooter.Parameters.AddWithValue("@User", null);
                    cmdManageHeaderwiseFooter.Parameters.AddWithValue("@Type", "GetRecords");
                    DataTable dtManageHeaderwiseFooter = new DataTable();
                    SqlDataAdapter daManageHeaderwiseFooter = new SqlDataAdapter(cmdManageHeaderwiseFooter);
                    daManageHeaderwiseFooter.Fill(dtManageHeaderwiseFooter);

                    if (dtManageHeaderwiseFooter.Rows.Count > 0)
                    {
                        BasicDeductionsMaxValue = Convert.ToDecimal(dtManageHeaderwiseFooter.Rows[0]["MaxAmount"].ToString());
                    }

                    if (BasicDeductionsActualValue > BasicDeductionsMaxValue)
                    {
                        BasicDeductionsActualValue = BasicDeductionsMaxValue;
                    }

                    SqlCommand cmdManageAccomodationCriteria = new SqlCommand("ManageAccomodationCriteria", con);
                    cmdManageAccomodationCriteria.CommandType = CommandType.StoredProcedure;
                    cmdManageAccomodationCriteria.Parameters.AddWithValue("@CityID", City.MetroCity);
                    cmdManageAccomodationCriteria.Parameters.AddWithValue("@User", null);
                    cmdManageAccomodationCriteria.Parameters.AddWithValue("@Type", "GetRecords");
                    DataTable dtManageAccomodationCriteria = new DataTable();
                    SqlDataAdapter daManageAccomodationCriteria = new SqlDataAdapter(cmdManageAccomodationCriteria);
                    daManageAccomodationCriteria.Fill(dtManageAccomodationCriteria);

                    BasicPlusDAGrandTotal = TotalBasic + TotalDa;

                    if (dtManageAccomodationCriteria.Rows.Count > 0)
                    {
                        AccomodationCriteria = Convert.ToDecimal(dtManageAccomodationCriteria.Rows[0]["Value"].ToString());
                    }

                    ActualHRAReceived = TotalHra;
                    AccommodationCriteriaSalary = ((BasicPlusDAGrandTotal * AccomodationCriteria) / 100);
                    ExcessRentPaidSalary = (RentAmountPerAnnum - ((BasicPlusDAGrandTotal * 10) / 100));

                    if (AccommodationCriteriaSalary >= 0 && ExcessRentPaidSalary >= 0)
                    {
                        if (AccommodationCriteriaSalary > ExcessRentPaidSalary)
                        {
                            HRAExemption = ExcessRentPaidSalary;
                        }
                        else
                        {
                            HRAExemption = AccommodationCriteriaSalary;
                        }
                    }

                    if (HRAExemption >= 0)
                    {
                        if (HRAExemption > ActualHRAReceived)
                        {
                            HRAExemption = ActualHRAReceived;
                        }
                    }

                    FinalTotalDeductions = HRAExemption + StandardDeduction + MedicalPolicyPremiumActualValue + IntersetOnHousingLoanActualValue + EducationLoanActualValue + BasicDeductionsActualValue + NationalPentionSchemeActualValue;
                    FinalTaxableAmount = TotalGrossTotal - FinalTotalDeductions;

                    lblGrossPerAnnum.Text = TotalGrossTotal.ToString();
                    lblAlreadyPaidTax.Text = TotalTdsOnBasic.ToString();
                    lblHRAExemption.Text = Converter(HRAExemption).ToString("0.00");
                    lblStandardDeduction.Text = Converter(StandardDeduction).ToString("0.00");
                    lblOtherIDFAdjustment.Text = Converter((FinalTotalDeductions - HRAExemption)).ToString("0.00");
                    lblTaxableIncome.Text = Converter(FinalTaxableAmount).ToString("0.00");
                }
            }
            else
            {
                pnlDetail.Visible = false;
                pnlButtons.Visible = false;
            }

            con.Close();
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
            Reset();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCalculateTaxableIncome_Click(object sender, EventArgs e)
    {
        try
        {
            ddlFromFinancialYear.Enabled = false;
            ddlMonthForSaveTax.Enabled = false;
            ddlYearForSaveTax.Enabled = false;
            CalculateTaxableIncome();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateFinalTaxAmount()
    {
        try
        {
            int TDSMonthID = Convert.ToInt32(ddlMonthForSaveTax.SelectedValue);

            foreach (GridViewRow grdrow in grdrecord.Rows)
            {
                if (((CheckBox)grdrow.FindControl("SelectChk")).Checked == true)
                {
                    Label lblAge = (Label)grdrow.FindControl("lblAge");
                    Label lblTotalTax = (Label)grdrow.FindControl("lblTotalTax");
                    Label lblBalanceTax = (Label)grdrow.FindControl("lblBalanceTax");
                    Label lblActualTaxForThisMonth = (Label)grdrow.FindControl("lblActualTaxForThisMonth");
                    Label lblFinalRoundedTax = (Label)grdrow.FindControl("lblFinalRoundedTax");

                    decimal FinalTaxableIncome = Convert.ToDecimal(((Label)grdrow.FindControl("lblTaxableIncome")).Text);
                    decimal NetIncomeFrom = 0;
                    decimal NetIncomeTo = 0;
                    decimal DifferenceAmount = 0;
                    decimal IncomeTaxRates = 0;
                    decimal EducationCess = 0;
                    decimal HigherEducationCess = 0;
                    int SpecialCase = 0;
                    decimal PreviousAmount = 0;
                    decimal PreviousNetIncomeFrom = 0;
                    decimal PreviousNetIncomeTo = 0;
                    decimal PreviousIncomeTaxRates = 0;
                    decimal TotalEducationCess = 0;
                    decimal SurchargeNetIncomeFrom = 0;
                    decimal SurchargeNetIncomeTo = 0;
                    decimal SurchargeIncomeTaxRates = 0;
                    int i = 0;
                    int j = 0;
                    decimal TotalTax = 0;
                    decimal AlreadyPaidTax = Convert.ToDecimal(((Label)grdrow.FindControl("lblAlreadyPaidTax")).Text);
                    decimal BalanceTax = 0;
                    decimal MonthlyTax = 0;

                    string[] CompleteAge = lblAge.Text.Split(' ');
                    int AgeInYears = Convert.ToInt32(CompleteAge[0]);

                    SqlConnection con = new SqlConnection(constr);
                    cmd = new SqlCommand("GetIncomeTaxRulesAccToAge", con);
                    cmd.Parameters.AddWithValue("@FromYearID", ddlFromFinancialYear.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaxNetIncome", FinalTaxableIncome);

                    if (AgeInYears < 60)
                    {
                        cmd.Parameters.AddWithValue("@AgeFrom", 0);
                        cmd.Parameters.AddWithValue("@AgeTo", AgeInYears);
                        cmd.Parameters.AddWithValue("@Type", "1");
                    }
                    else if (AgeInYears >= 60 && AgeInYears <= 80)
                    {
                        cmd.Parameters.AddWithValue("@AgeFrom", 60);
                        cmd.Parameters.AddWithValue("@AgeTo", AgeInYears);
                        cmd.Parameters.AddWithValue("@Type", "2");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@AgeFrom", 81);
                        cmd.Parameters.AddWithValue("@AgeTo", 0);
                        cmd.Parameters.AddWithValue("@Type", "3");
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    DataSet ds = new DataSet();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            NetIncomeFrom = Convert.ToDecimal(ds.Tables[0].Rows[i]["NetIncomeFrom"].ToString());
                            NetIncomeTo = Convert.ToDecimal(ds.Tables[0].Rows[i]["NetIncomeTo"].ToString());
                            IncomeTaxRates = Convert.ToDecimal(ds.Tables[0].Rows[i]["IncomeTaxRates"].ToString());
                            EducationCess = Convert.ToDecimal(ds.Tables[0].Rows[i]["EducationCess"].ToString());
                            HigherEducationCess = Convert.ToDecimal(ds.Tables[0].Rows[i]["HigherEducationCess"].ToString());
                            SpecialCase = Convert.ToInt32(ds.Tables[0].Rows[i]["SpecialCase"].ToString());

                            if (SpecialCase == 1)
                            {
                                if (FinalTaxableIncome >= NetIncomeFrom && FinalTaxableIncome <= NetIncomeTo)
                                {
                                    TotalTax = Converter((FinalTaxableIncome * IncomeTaxRates) / 100);
                                    break;
                                }
                            }
                            else
                            {
                                if (NetIncomeFrom > 0)
                                {
                                    if (ds.Tables[0].Rows[i - 1]["SpecialCase"].ToString() == "1")
                                    {
                                        PreviousNetIncomeFrom = Convert.ToDecimal(ds.Tables[0].Rows[i - 2]["NetIncomeFrom"].ToString());
                                        PreviousNetIncomeTo = Convert.ToDecimal(ds.Tables[0].Rows[i - 2]["NetIncomeTo"].ToString());
                                        PreviousIncomeTaxRates = Convert.ToDecimal(ds.Tables[0].Rows[i - 2]["IncomeTaxRates"].ToString());
                                    }
                                    else
                                    {
                                        PreviousNetIncomeFrom = Convert.ToDecimal(ds.Tables[0].Rows[i - 1]["NetIncomeFrom"].ToString());
                                        PreviousNetIncomeTo = Convert.ToDecimal(ds.Tables[0].Rows[i - 1]["NetIncomeTo"].ToString());
                                        PreviousIncomeTaxRates = Convert.ToDecimal(ds.Tables[0].Rows[i - 1]["IncomeTaxRates"].ToString());
                                    }

                                    PreviousAmount = Converter(PreviousAmount + (((PreviousNetIncomeTo - PreviousNetIncomeFrom) * PreviousIncomeTaxRates) / 100));

                                    if (NetIncomeTo > 0)
                                    {
                                        if (ds.Tables[0].Rows[i - 1]["SpecialCase"].ToString() == "1")
                                        {
                                            DifferenceAmount = Convert.ToDecimal(ds.Tables[0].Rows[i - 2]["NetIncomeTo"].ToString());
                                        }
                                        else
                                        {
                                            DifferenceAmount = Convert.ToDecimal(ds.Tables[0].Rows[i - 1]["NetIncomeTo"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        DifferenceAmount = Converter(NetIncomeFrom);
                                    }

                                    TotalTax = Converter(PreviousAmount + (((FinalTaxableIncome - DifferenceAmount) * IncomeTaxRates) / 100));
                                    TotalEducationCess = EducationCess + HigherEducationCess;
                                }
                                else
                                {
                                    TotalTax = Converter(((FinalTaxableIncome * IncomeTaxRates) / 100));
                                }
                            }

                            i++;
                        }
                    }

                    if (FinalTaxableIncome >= 250001 && FinalTaxableIncome <= 350000)
                    {
                        TotalTax = TotalTax - 2500;

                        if (TotalTax < 0)
                        {
                            TotalTax = 0;
                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            SurchargeNetIncomeFrom = Convert.ToDecimal(ds.Tables[1].Rows[j]["NetIncomeFrom"].ToString());
                            SurchargeNetIncomeTo = Convert.ToDecimal(ds.Tables[1].Rows[j]["NetIncomeTo"].ToString());
                            SurchargeIncomeTaxRates = Convert.ToDecimal(ds.Tables[1].Rows[j]["SurchargeRates"].ToString());
                            j++;
                        }

                        TotalTax = Converter(TotalTax + ((TotalTax * SurchargeIncomeTaxRates) / 100));
                    }

                    if (TotalEducationCess > 0)
                    {
                        TotalTax = Converter(TotalTax + ((TotalTax * TotalEducationCess) / 100));
                    }

                    BalanceTax = Converter(TotalTax - AlreadyPaidTax);

                    if (TDSMonthID == 4)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.April);
                    }
                    else if (TDSMonthID == 5)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.May);
                    }
                    else if (TDSMonthID == 6)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.June);
                    }
                    else if (TDSMonthID == 7)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.July);
                    }
                    else if (TDSMonthID == 8)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.August);
                    }
                    else if (TDSMonthID == 9)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.September);
                    }
                    else if (TDSMonthID == 10)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.October);
                    }
                    else if (TDSMonthID == 11)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.November);
                    }
                    else if (TDSMonthID == 12)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.December);
                    }
                    else if (TDSMonthID == 1)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.January);
                    }
                    else if (TDSMonthID == 2)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.February);
                    }
                    else if (TDSMonthID == 3)
                    {
                        MonthlyTax = BalanceTax / Convert.ToInt32(MonthDivisableRules.March);
                    }

                    lblTotalTax.Text = Converter(TotalTax).ToString("0.00");
                    lblBalanceTax.Text = BalanceTax.ToString("0.00");
                    lblActualTaxForThisMonth.Text = Converter(MonthlyTax).ToString("0.00");

                    if (TDSMonthID == 3)
                    {
                        lblFinalRoundedTax.Text = RoundUpValue(MonthlyTax).ToString("0.00");
                    }
                    else
                    {
                        lblFinalRoundedTax.Text = (Math.Round(MonthlyTax / 1000, 0) * 1000).ToString("0.00");
                    }

                    btnSaveTax.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public decimal RoundUpValue(decimal value)
    {
        decimal result = (Math.Round(value / 10, 0) * 10);

        if (result < value)
        {
            result = result + 10;
            result = (Math.Round(result / 10, 0) * 10);
        }

        return result;
    }

    protected void btnCalculateTax_Click(object sender, EventArgs e)
    {
        try
        {
            CalculateFinalTaxAmount();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveTax_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string MonthID = ddlMonthForSaveTax.SelectedValue;
            string Month = ddlMonthForSaveTax.SelectedItem.Text;
            string YearID = ddlYearForSaveTax.SelectedValue;
            string Year = ddlYearForSaveTax.SelectedItem.Text;

            foreach (GridViewRow grdrow in grdrecord.Rows)
            {
                if (((CheckBox)grdrow.FindControl("SelectChk")).Checked == true)
                {
                    string ProfileID = ((Label)grdrow.FindControl("lblProfileID")).Text;
                    string EmployeeID = ((Label)grdrow.FindControl("lblEmployeeID")).Text;
                    string Emp_Code = ((Label)grdrow.FindControl("lblEmp_Code")).Text;
                    string SystemNumber = ((Label)grdrow.FindControl("lblSystemNumber")).Text;
                    string AssignEmpCode = ((Label)grdrow.FindControl("lblAssignEmpCode")).Text;
                    string Name = ((Label)grdrow.FindControl("lblName")).Text;
                    string Designation = ((Label)grdrow.FindControl("lblDesignation")).Text;
                    string TotalTax = ((Label)grdrow.FindControl("lblTotalTax")).Text;
                    string ActualTDS = ((Label)grdrow.FindControl("lblActualTaxForThisMonth")).Text;
                    string TDSValue = ((Label)grdrow.FindControl("lblFinalRoundedTax")).Text;

                    SqlCommand cmdTDS = new SqlCommand("SaveImportTDS", con);
                    cmdTDS.CommandType = CommandType.StoredProcedure;
                    cmdTDS.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmdTDS.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    cmdTDS.Parameters.AddWithValue("@Emp_Code", Emp_Code);
                    cmdTDS.Parameters.AddWithValue("@SystemNumber", SystemNumber);
                    cmdTDS.Parameters.AddWithValue("@AssignEmpCode", AssignEmpCode);
                    cmdTDS.Parameters.AddWithValue("@Name", Name);
                    cmdTDS.Parameters.AddWithValue("@Designation", Designation);
                    cmdTDS.Parameters.AddWithValue("@MonthID", MonthID);
                    cmdTDS.Parameters.AddWithValue("@Month", Month);
                    cmdTDS.Parameters.AddWithValue("@YearID", YearID);
                    cmdTDS.Parameters.AddWithValue("@Year", Year);
                    cmdTDS.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                    cmdTDS.Parameters.AddWithValue("@TotalTax", TotalTax);

                    if (ActualTDS == "")
                    {
                        cmdTDS.Parameters.AddWithValue("@ActualTDS", 0);
                    }
                    else
                    {
                        cmdTDS.Parameters.AddWithValue("@ActualTDS", ActualTDS);
                    }
                    if (TDSValue == "")
                    {
                        cmdTDS.Parameters.AddWithValue("@TDSValue", 0);
                    }
                    else
                    {
                        cmdTDS.Parameters.AddWithValue("@TDSValue", TDSValue);
                    }

                    int a = cmdTDS.ExecuteNonQuery();
                }
            }

            con.Close();
            Reset();
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}