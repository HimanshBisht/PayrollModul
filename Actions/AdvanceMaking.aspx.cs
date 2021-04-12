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
using System.Globalization;

public partial class SalaryModule_AdvanceMaking : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal TotalAmount = 0;

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

    public enum Status
    {
        Active = 1,
        Deactive = 0
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
                    Employee();
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

    public void Clear()
    {
        try
        {
            ddlemployee.ClearSelection();
            ddlemployee.Enabled = true;
            lblPendingAmount.Text = string.Empty;
            btnAddAdvanceAmount.Text = string.Empty;
            pnlPending.Visible = false;
            txtAddAmount.Text = string.Empty;
            txtAddAmount.ReadOnly = false;
            lblTotalAmount.Text = string.Empty;
            pnlFillData.Visible = false;
            pnlTotal.Visible = false;
            pnlType.Visible = false;
            ddlAmtRec.ClearSelection();
            txtNoOfEMI.Text = string.Empty;
            pnlEMI.Visible = false;
            txtFixedAmt.Text = string.Empty;
            pnlMonthlyAmt.Visible = false;
            pnlStructure.Visible = false;
            ViewState["TotalPendingAmount"] = null;
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            lblHeaderText.Text = string.Empty;
            grdrecord.DataSource = null;
            grdUpdate.DataBind();
            pnlDetail.Visible = false;
            grdUpdate.DataSource = null;
            grdUpdate.DataBind();
            pnlUpdate.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Reset()
    {
        try
        {
            txtAddAmount.ReadOnly = false;
            txtAddAmount.Text = string.Empty;
            lblTotalAmount.Text = string.Empty;
            pnlTotal.Visible = false;
            pnlType.Visible = false;
            ddlAmtRec.ClearSelection();
            txtNoOfEMI.Text = string.Empty;
            pnlEMI.Visible = false;
            txtFixedAmt.Text = string.Empty;
            pnlMonthlyAmt.Visible = false;
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            pnlStructure.Visible = false;
            pnlAddAmount.Visible = true;
            lblHeaderText.Text = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlDetail.Visible = false;
            grdUpdate.DataSource = null;
            grdUpdate.DataBind();
            pnlUpdate.Visible = false;
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

    protected void ddlemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlemployee.SelectedValue) > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetPendingAdvance", con);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                decimal TotalPendingAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    TotalPendingAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("AdvanceValue"));
                    btnAddAdvanceAmount.Text = "Add More Advance Amount";
                    pnlUpdate.Visible = true;
                }
                else
                {
                    btnAddAdvanceAmount.Text = "Add Advance Amount";
                    pnlUpdate.Visible = false;
                }
                pnlPending.Visible = true;
                lblPendingAmount.Text = "Already Pending Advance Amount of " + ddlemployee.SelectedItem.Text + " : " + TotalPendingAmount;
                ViewState["TotalPendingAmount"] = TotalPendingAmount;
                grdUpdate.DataSource = dt;
                grdUpdate.DataBind();
                ddlemployee.Enabled = false;
            }
            else
            {
                Clear();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnAddAdvanceAmount_Click(object sender, EventArgs e)
    {
        try
        {
            pnlFillData.Visible = true;
            pnlAddAmount.Visible = true;
            pnlUpdate.Visible = false;
            grdUpdate.DataSource = null;
            grdUpdate.DataBind();
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
            Reset();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnAddAmount_Click(object sender, EventArgs e)
    {
        try
        {
            decimal TotalAmount = 0;
            TotalAmount = Convert.ToDecimal(ViewState["TotalPendingAmount"]) + Convert.ToDecimal(txtAddAmount.Text);
            lblTotalAmount.Text = TotalAmount.ToString();
            pnlTotal.Visible = true;
            pnlType.Visible = true;
            txtAddAmount.ReadOnly = true;

            DateTimeFormatInfo DTFI = new DateTimeFormatInfo();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmdMonthNYear = new SqlCommand("ManageMonthNYears", con);
            cmdMonthNYear.Parameters.AddWithValue("@MonthName", DTFI.GetMonthName(DateTime.Now.Month));
            cmdMonthNYear.Parameters.AddWithValue("@YEAR", DateTime.Now.Year);
            cmdMonthNYear.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet dsMonthNYear = new DataSet();
            SqlDataAdapter daMonthNYear = new SqlDataAdapter(cmdMonthNYear);
            daMonthNYear.Fill(dsMonthNYear);
            con.Close();

            ddlMonth.SelectedValue = dsMonthNYear.Tables[0].Rows[0]["MonthID"].ToString();
            ddlYear.SelectedValue = dsMonthNYear.Tables[1].Rows[0]["YearID"].ToString();

            pnlStructure.Visible = true;
            pnlAddAmount.Visible = false;
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

    protected void ddlAmtRec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAmtRec.SelectedValue == "1")
            {
                pnlEMI.Visible = true;
                txtNoOfEMI.Text = string.Empty;
                pnlMonthlyAmt.Visible = false;
                txtFixedAmt.Text = string.Empty;
            }
            else if (ddlAmtRec.SelectedValue == "2")
            {
                pnlEMI.Visible = false;
                txtNoOfEMI.Text = string.Empty;
                pnlMonthlyAmt.Visible = true;
                txtFixedAmt.Text = string.Empty;
            }
            else
            {
                pnlEMI.Visible = false;
                txtNoOfEMI.Text = string.Empty;
                pnlMonthlyAmt.Visible = false;
                txtFixedAmt.Text = string.Empty;
            }
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlDetail.Visible = false;
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

    protected void btnShowStructure_Click(object sender, EventArgs e)
    {
        try
        {
            DateTimeFormatInfo DTFI = new DateTimeFormatInfo();
            int Month = Convert.ToInt32(ddlMonth.SelectedValue);
            int Year = Convert.ToInt32(ddlYear.SelectedItem.Text);
            int CurrentMonth = DateTime.Now.Month;
            int CurrentYear = DateTime.Now.Year;

            if ((Month >= CurrentMonth - 11 && Year >= CurrentYear) || (Year > CurrentYear))
            {
                decimal NetAdvanceAmount = Convert.ToDecimal(lblTotalAmount.Text);
                decimal MonthlyAdvanceAmount = 0;
                int MonthlyFixedAmount = 0;
                int SNo = 1;
                int i = 0;
                decimal RemainingAmount = 0;

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("SNo", typeof(int)),
                        new DataColumn("Month", typeof(string)),
                            new DataColumn("Year", typeof(int)),
                            new DataColumn("MonthlyAmount",typeof(decimal)) });

                if (ddlAmtRec.SelectedValue == "1")
                {
                    decimal NoOfEMI = Convert.ToDecimal(txtNoOfEMI.Text);
                    if (NoOfEMI > 0)
                    {
                        MonthlyAdvanceAmount = Converter(NetAdvanceAmount / NoOfEMI);

                        for (i = 0; i < NoOfEMI; i++)
                        {
                            if (i < NoOfEMI - 1)
                            {
                                dt.Rows.Add(SNo, DTFI.GetMonthName(Month), Year, MonthlyAdvanceAmount.ToString("0.00"));
                                TotalAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("MonthlyAmount"));
                                Month++;
                                SNo++;

                                if (Month > 12)
                                {
                                    Month = 1;
                                    Year++;
                                }
                            }
                            else
                            {
                                RemainingAmount = NetAdvanceAmount - TotalAmount;
                                dt.Rows.Add(SNo, DTFI.GetMonthName(Month), Year, RemainingAmount.ToString("0.00"));
                                TotalAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("MonthlyAmount"));
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No. of EMI Should be greater than zero.');", true);
                    }
                }
                else
                {
                    MonthlyFixedAmount = Convert.ToInt32(txtFixedAmt.Text);
                    if (MonthlyFixedAmount > 0)
                    {
                        int TotalMonths = (Convert.ToInt32(NetAdvanceAmount) / MonthlyFixedAmount);

                        for (i = 0; i < TotalMonths; i++)
                        {
                            dt.Rows.Add(SNo, DTFI.GetMonthName(Month), Year, MonthlyFixedAmount.ToString("0.00"));
                            TotalAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("MonthlyAmount"));
                            Month++;
                            SNo++;

                            if (Month > 12)
                            {
                                Month = 1;
                                Year++;
                            }
                        }

                        RemainingAmount = NetAdvanceAmount - TotalAmount;

                        if (RemainingAmount > 0)
                        {
                            dt.Rows.Add(SNo, DTFI.GetMonthName(Month), Year, RemainingAmount.ToString("0.00"));
                            TotalAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("MonthlyAmount"));
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Monthly Payble Amount Should be greater than zero.');", true);
                    }
                }

                if (MonthlyAdvanceAmount > 0 || MonthlyFixedAmount > 0)
                {
                    grdrecord.DataSource = dt;
                    grdrecord.DataBind();
                    pnlDetail.Visible = true;
                    lblHeaderText.Text = "Monthly Advance Structure of " + ddlemployee.SelectedItem.Text;
                }
                else
                {
                    grdrecord.DataSource = null;
                    grdrecord.DataBind();
                    pnlDetail.Visible = false;
                    lblHeaderText.Text = string.Empty;
                }
            }
            else
            {
                grdrecord.DataSource = null;
                grdrecord.DataBind();
                pnlDetail.Visible = false;
                lblHeaderText.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select Valid Month and Year.');", true);
            }
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
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblNetAdvanceAmount = (Label)e.Row.FindControl("lblNetAdvanceAmount");
                lblNetAdvanceAmount.Text = TotalAmount.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveAdvance_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlTransaction trans = con.BeginTransaction();

        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];

            cmd = new SqlCommand("ShowEmpSalaryProfile", con, trans);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            int HasRow = 0;
            int Flag = 1;

            foreach (GridViewRow row in grdrecord.Rows)
            {
                Label lblMonth = (Label)row.FindControl("lblMonth");
                Label lblYear = (Label)row.FindControl("lblYear");
                Label lblMonthlyAmount = (Label)row.FindControl("lblMonthlyAmount");
                cmd = new SqlCommand("SaveImportAdvance", con, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@EmployeeID", dt.Rows[0]["EmployeeID"].ToString());
                cmd.Parameters.AddWithValue("@Emp_Code", dt.Rows[0]["Emp_Code"].ToString());
                cmd.Parameters.AddWithValue("@SystemNumber", dt.Rows[0]["SystemNumber"].ToString());
                cmd.Parameters.AddWithValue("@AssignEmpCode", dt.Rows[0]["AssignEmpCode"].ToString());
                cmd.Parameters.AddWithValue("@Name", dt.Rows[0]["Name"].ToString());
                cmd.Parameters.AddWithValue("@Designation", dt.Rows[0]["DesignationText"].ToString());
                cmd.Parameters.AddWithValue("@Month", lblMonth.Text);
                cmd.Parameters.AddWithValue("@Year", lblYear.Text);

                if (lblMonth.Text.Length > 0 && lblYear.Text.Length > 0)
                {
                    SqlCommand cmdMonthNYear = new SqlCommand("ManageMonthNYears", con, trans);
                    cmdMonthNYear.Parameters.AddWithValue("@MonthName", lblMonth.Text);
                    cmdMonthNYear.Parameters.AddWithValue("@YEAR", lblYear.Text);
                    cmdMonthNYear.CommandType = CommandType.StoredProcedure;
                    DataSet dsMonthNYear = new DataSet();
                    SqlDataAdapter daMonthNYear = new SqlDataAdapter(cmdMonthNYear);
                    daMonthNYear.Fill(dsMonthNYear);

                    if (dsMonthNYear.Tables[1].Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@MonthID", dsMonthNYear.Tables[0].Rows[0]["MonthID"].ToString());
                        cmd.Parameters.AddWithValue("@YearID", dsMonthNYear.Tables[1].Rows[0]["YearID"].ToString());
                        cmd.Parameters.AddWithValue("@AdvanceRecoveryType", ddlAmtRec.SelectedValue);
                        cmd.Parameters.AddWithValue("@AdvanceValue", lblMonthlyAmount.Text);
                        cmd.Parameters.AddWithValue("@Flag", Flag);
                        cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                        HasRow = cmd.ExecuteNonQuery();
                        if (HasRow > 0)
                        {
                            Flag = 0;
                        }
                    }
                    else
                    {
                        HasRow = 0;
                        throw new Exception("Failed : Firstly Create Years after " + dsMonthNYear.Tables[2].Rows[0]["Year"].ToString() + ".");
                    }
                }
            }

            if (HasRow > 0)
            {
                trans.Commit();
                con.Close();
                Clear();
                Reset();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
            }
            else
            {
                trans.Rollback();
                con.Close();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Saving Details.');", true);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            con.Close();
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdUpdate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblNetAdvanceAmount = (Label)e.Row.FindControl("lblNetAdvanceAmount");
                lblNetAdvanceAmount.Text = ViewState["TotalPendingAmount"].ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTimeFormatInfo DTFI = new DateTimeFormatInfo();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("SNo", typeof(int)),
                 new DataColumn("AdvanceImportID", typeof(string)),
                        new DataColumn("ProfileID", typeof(string)),
                         new DataColumn("EmployeeID", typeof(string)),
                          new DataColumn("Emp_Code", typeof(string)),
                           new DataColumn("SystemNumber", typeof(string)),
                            new DataColumn("AssignEmpCode", typeof(string)),
                            new DataColumn("Name", typeof(string)),
                             new DataColumn("Designation", typeof(string)),
                              new DataColumn("MonthID", typeof(string)),
                              new DataColumn("Month", typeof(string)),
                              new DataColumn("YearID", typeof(string)),
                              new DataColumn("AdvanceRecoveryType", typeof(string)),
                              new DataColumn("Year", typeof(string)),
            new DataColumn("AdvanceValue", typeof(decimal)) });
            int LastMonthID = 0;
            int LastYear = 0;
            string AdvanceImportID = string.Empty;
            string ProfileID = string.Empty;
            string EmployeeID = string.Empty;
            string Emp_Code = string.Empty;
            string SystemNumber = string.Empty;
            string AssignEmpCode = string.Empty;
            string Name = string.Empty;
            string Designation = string.Empty;
            string MonthID = string.Empty;
            string Month = string.Empty;
            string YearID = string.Empty;
            string AdvanceRecoveryType = string.Empty;
            string Year = string.Empty;
            decimal AdvanceValue = 0;
            int SNO = 0;

            foreach (GridViewRow row in grdUpdate.Rows)
            {
                SNO++;
                AdvanceImportID = "0";
                ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                EmployeeID = ((Label)row.FindControl("lblEmployeeID")).Text;
                Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                SystemNumber = ((Label)row.FindControl("lblSystemNumber")).Text;
                AssignEmpCode = ((Label)row.FindControl("lblAssignEmpCode")).Text;
                Name = ((Label)row.FindControl("lblName")).Text;
                Designation = ((Label)row.FindControl("lblDesignation")).Text;
                MonthID = ((Label)row.FindControl("lblMonthID")).Text;
                Month = ((Label)row.FindControl("lblMonth")).Text;
                YearID = ((Label)row.FindControl("lblYearID")).Text;
                AdvanceRecoveryType = ((Label)row.FindControl("lblAdvanceRecoveryType")).Text;
                Year = ((Label)row.FindControl("lblYear")).Text;
                AdvanceValue = Convert.ToDecimal(((TextBox)row.FindControl("txtMonthlyAmount")).Text);
                LastMonthID = Convert.ToInt32(MonthID);
                LastYear = Convert.ToInt32(Year);
                dt.Rows.Add(SNO, AdvanceImportID, ProfileID, EmployeeID, Emp_Code, SystemNumber, AssignEmpCode, Name, Designation, MonthID, Month, YearID, AdvanceRecoveryType, Year, AdvanceValue);
            }

            if (LastMonthID >= 12)
            {
                LastMonthID = 1;
                LastYear = LastYear + 1;
                dt.Rows.Add(SNO, AdvanceImportID, ProfileID, EmployeeID, Emp_Code, SystemNumber, AssignEmpCode, Name, Designation, LastMonthID, DTFI.GetMonthName(LastMonthID), YearID, AdvanceRecoveryType, LastYear, 0);
            }
            else
            {
                dt.Rows.Add(SNO, AdvanceImportID, ProfileID, EmployeeID, Emp_Code, SystemNumber, AssignEmpCode, Name, Designation, LastMonthID + 1, DTFI.GetMonthName(LastMonthID + 1), YearID, AdvanceRecoveryType, LastYear, 0);
            }

            grdUpdate.DataSource = dt;
            grdUpdate.DataBind();
            GetFooterTotal();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetFooterTotal()
    {
        try
        {
            GridViewRow row = grdUpdate.FooterRow;
            Label lblNetAdvanceAmount = (Label)row.FindControl("lblNetAdvanceAmount");
            decimal AdvanceValue = 0;

            foreach (GridViewRow grdRow in grdUpdate.Rows)
            {
                if (((TextBox)grdRow.FindControl("txtMonthlyAmount")).Text.Length > 0)
                {
                    AdvanceValue = AdvanceValue + Convert.ToDecimal(((TextBox)grdRow.FindControl("txtMonthlyAmount")).Text);
                }
                else
                {
                    ((TextBox)grdRow.FindControl("txtMonthlyAmount")).Text = "0";
                }
            }

            lblNetAdvanceAmount.Text = AdvanceValue.ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtMonthlyAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetFooterTotal();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnUpdateStructure_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Footerrow = grdUpdate.FooterRow;
            decimal NetAdvanceAmountAfterUpdate = Convert.ToDecimal(((Label)Footerrow.FindControl("lblNetAdvanceAmount")).Text);
            decimal TotalPendingAmount = Convert.ToDecimal(ViewState["TotalPendingAmount"]);

            if (TotalPendingAmount == NetAdvanceAmountAfterUpdate)
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];

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
                int Flag = 1;

                foreach (GridViewRow row in grdUpdate.Rows)
                {
                    Label lblMonth = (Label)row.FindControl("lblMonth");
                    Label lblYear = (Label)row.FindControl("lblYear");
                    TextBox txtMonthlyAmount = (TextBox)row.FindControl("txtMonthlyAmount");
                    cmd = new SqlCommand("SaveImportAdvance", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@EmployeeID", dt.Rows[0]["EmployeeID"].ToString());
                    cmd.Parameters.AddWithValue("@Emp_Code", dt.Rows[0]["Emp_Code"].ToString());
                    cmd.Parameters.AddWithValue("@SystemNumber", dt.Rows[0]["SystemNumber"].ToString());
                    cmd.Parameters.AddWithValue("@AssignEmpCode", dt.Rows[0]["AssignEmpCode"].ToString());
                    cmd.Parameters.AddWithValue("@Name", dt.Rows[0]["Name"].ToString());
                    cmd.Parameters.AddWithValue("@Designation", dt.Rows[0]["DesignationText"].ToString());
                    cmd.Parameters.AddWithValue("@Month", lblMonth.Text);
                    cmd.Parameters.AddWithValue("@Year", lblYear.Text);
                    if (lblMonth.Text.Length > 0 && lblYear.Text.Length > 0)
                    {
                        SqlCommand cmdMonthNYear = new SqlCommand("ManageMonthNYears", con);
                        cmdMonthNYear.Parameters.AddWithValue("@MonthName", lblMonth.Text);
                        cmdMonthNYear.Parameters.AddWithValue("@YEAR", lblYear.Text);
                        cmdMonthNYear.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        DataSet dsMonthNYear = new DataSet();
                        SqlDataAdapter daMonthNYear = new SqlDataAdapter(cmdMonthNYear);
                        daMonthNYear.Fill(dsMonthNYear);
                        con.Close();
                        cmd.Parameters.AddWithValue("@MonthID", dsMonthNYear.Tables[0].Rows[0]["MonthID"].ToString());
                        cmd.Parameters.AddWithValue("@YearID", dsMonthNYear.Tables[1].Rows[0]["YearID"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MonthID", 0);
                        cmd.Parameters.AddWithValue("@YearID", 0);
                    }
                    cmd.Parameters.AddWithValue("@AdvanceRecoveryType", ddlAmtRec.SelectedValue);
                    cmd.Parameters.AddWithValue("@AdvanceValue", txtMonthlyAmount.Text);
                    cmd.Parameters.AddWithValue("@Flag", Flag);
                    cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                    con.Open();
                    int HasRow = 0;
                    HasRow = cmd.ExecuteNonQuery();
                    con.Close();
                    if (HasRow > 0)
                    {
                        Flag = 0;
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Update Sucessfully.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Updating Details.');", true);
                    }
                }
                Clear();
                Reset();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Pending and Net Amount is Mis-matched.');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in grdUpdate.Rows)
            {
                string AdvanceImportID = ((Label)row.FindControl("lblAdvanceImportID")).Text;

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageAdvance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdvanceValue", 0);
                cmd.Parameters.AddWithValue("@IsActive", Status.Deactive);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@AdvanceImportID", AdvanceImportID);
                cmd.Parameters.AddWithValue("@Type", "Deactivate");
                con.Open();
                int HasRow = 0;
                HasRow = cmd.ExecuteNonQuery();
                con.Close();
                if (HasRow > 0)
                {
                    Clear();
                    Reset();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Deactivating Record.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}