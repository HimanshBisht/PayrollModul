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

public partial class SalaryModule_TDSInvestmentDeclarationForm : System.Web.UI.Page
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
                    Employee();
                    Year();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Employee();
                Year();
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

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    static string CalculateYourAge(DateTime Dob)
    {
        DateTime Now = DateTime.Now;
        int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
        DateTime PastYearDate = Dob.AddYears(Years);
        int Months = 0;
        for (int i = 1; i <= 12; i++)
        {
            if (PastYearDate.AddMonths(i) == Now)
            {
                Months = i;
                break;
            }
            else if (PastYearDate.AddMonths(i) >= Now)
            {
                Months = i - 1;
                break;
            }
        }
        int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
        return String.Format("{0} Year's {1} Month's {2} Day's",
        Years, Months, Days);
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
            ddlYear.Items.Insert(0, new ListItem("Select From Financial Year", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
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
                lblEmp_Code.Text = dt.Rows[0]["Emp_Code"].ToString();
                lblName.Text = dt.Rows[0]["Name"].ToString();
                txtSignature.Text = dt.Rows[0]["Name"].ToString();
                txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                lblDOB.Text = dt.Rows[0]["DOB"].ToString();
                if (lblDOB.Text.Length > 0)
                {
                    DateTime dob = Convert.ToDateTime(lblDOB.Text);
                    string Age = CalculateYourAge(dob);
                    lblEmpAge.Text = Age;
                    GetAgeCriteria();
                }
                lblDesignation.Text = dt.Rows[0]["DesignationText"].ToString();
                lblEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                lblContactNo.Text = dt.Rows[0]["MobileNo"].ToString();
                lblGender.Text = dt.Rows[0]["GenderText"].ToString();
                lblPanNo.Text = dt.Rows[0]["PanCardNo"].ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetAgeCriteria()
    {
        try
        {
            int Years = 0;
            int Days = 0;

            if (lblEmpAge.Text.Length > 0)
            {
                string[] EmpAgeText = lblEmpAge.Text.Split(' ');
                Years = Convert.ToInt32(EmpAgeText[0]);
                Days = Convert.ToInt32(EmpAgeText[4]);

                if (Years > 60 || (Years == 60 && Days > 0))
                {
                    lblSeniorCitizen.Text = "Yes";
                    lblAgeCriteria.Text = "60+";
                    pnlAge.Visible = true;
                }
                if (Years > 80 || (Years == 80 && Days > 0))
                {
                    lblSeniorCitizen.Text = "Yes";
                    lblAgeCriteria.Text = "80+";
                    pnlAge.Visible = true;
                }
                if (Years < 60)
                {
                    lblSeniorCitizen.Text = "No";
                    lblAgeCriteria.Text = string.Empty;
                    pnlAge.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetIDFForm()
    {
        try
        {
            int SelectedYear = 0;
            int GetLastYear = 0;

            if (Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlemployee.SelectedValue) > 0)
            {
                SelectedYear = Convert.ToInt32(ddlYear.SelectedItem.Text);
                GetLastYear = SelectedYear + 1;
                pnlGetData.Visible = true;
                lblHead1.Text = Session["SchoolName"].ToString();
                lblHead2.Text = "Investment Declaration Form - F.Y." + ddlYear.SelectedItem.Text + "-" + GetLastYear.ToString();
                BindEmpDetails();
                BindHeaderWiseRule();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select an Employee and From Year.');", true);
            }
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
            ddlemployee.Enabled = true;
            ddlYear.Enabled = true;
            ddlemployee.ClearSelection();
            ddlYear.ClearSelection();
            lblHead1.Text = string.Empty;
            lblHead2.Text = string.Empty;
            lblEmp_Code.Text = string.Empty;
            lblName.Text = string.Empty;
            lblGender.Text = string.Empty;
            lblDesignation.Text = string.Empty;
            lblContactNo.Text = string.Empty;
            lblEmailID.Text = string.Empty;
            lblPanNo.Text = string.Empty;
            lblSeniorCitizen.Text = string.Empty;
            lblAgeCriteria.Text = string.Empty;
            pnlAge.Visible = false;
            lblDOB.Text = string.Empty;
            lblEmpAge.Text = string.Empty;
            pnlGetData.Visible = false;
            pnlButtons.Visible = false;
            grdHeader1.Caption = string.Empty;
            grdHeader1.DataSource = null;
            grdHeader1.DataBind();
            grdHeader2.Caption = string.Empty;
            grdHeader2.DataSource = null;
            grdHeader2.DataBind();
            grdHeader3.Caption = string.Empty;
            grdHeader3.DataSource = null;
            grdHeader3.DataBind();
            grdHeader4.Caption = string.Empty;
            grdHeader4.DataSource = null;
            grdHeader4.DataBind();
            grdHeader5.DataSource = null;
            grdHeader5.DataBind();
            lblDeclaration.Text = string.Empty;
            txtPlace.Text = string.Empty;
            txtSignature.Text = string.Empty;
            txtDate.Text = string.Empty;
            pnlDeclaration.Visible = false;
            pnlButtons.Visible = false;
            btnGetForm.Enabled = true;
            btnSave.Text = "Save Details";
            //btnDeactivate.Visible = false;
            btnPrintSelected.Visible = false;
            txtPlace.Style.Add("margin-top", "0px");
            txtSignature.Style.Add("margin-top", "0px");
            txtDate.Style.Add("margin-top", "0px");
            txtOthers.Text = string.Empty;
            pnlOthers.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindHeaderWiseRule()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            for (int i = 1; i <= 4; i++)
            {
                cmd = new SqlCommand("ManageHeaderwiseRules", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RuleText", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@HeaderID", i);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (i == 1)
                    {
                        grdHeader1.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                        grdHeader1.DataSource = dt;
                        grdHeader1.DataBind();
                    }
                    else if (i == 2)
                    {
                        grdHeader2.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                        grdHeader2.DataSource = dt;
                        grdHeader2.DataBind();
                    }
                    else if (i == 3)
                    {
                        grdHeader3.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                        grdHeader3.DataSource = dt;
                        grdHeader3.DataBind();
                    }
                    else if (i == 4)
                    {
                        grdHeader4.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                        grdHeader4.DataSource = dt;
                        grdHeader4.DataBind();
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        grdHeader1.Caption = string.Empty;
                        grdHeader1.DataSource = null;
                        grdHeader1.DataBind();
                    }
                    else if (i == 2)
                    {
                        grdHeader2.Caption = string.Empty;
                        grdHeader2.DataSource = null;
                        grdHeader2.DataBind();
                    }
                    else if (i == 3)
                    {
                        grdHeader3.Caption = string.Empty;
                        grdHeader3.DataSource = null;
                        grdHeader3.DataBind();
                    }
                    else if (i == 4)
                    {
                        grdHeader4.Caption = string.Empty;
                        grdHeader4.DataSource = null;
                        grdHeader4.DataBind();
                    }
                }
            }
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnGetForm_Click(object sender, EventArgs e)
    {
        try
        {
            ddlemployee.Enabled = false;
            ddlYear.Enabled = false;
            pnlDeclaration.Visible = true;
            pnlButtons.Visible = true;
            btnGetForm.Enabled = false;
            GetIDFForm();
            BindHouseRent();
            BindDeclaration();
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

    public void BindHouseRent()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmdHeader = new SqlCommand("ManageHeaderwiseRules", con);
            cmdHeader.CommandType = CommandType.StoredProcedure;
            cmdHeader.Parameters.AddWithValue("@RuleText", null);
            cmdHeader.Parameters.AddWithValue("@User", null);
            cmdHeader.Parameters.AddWithValue("@HeaderID", 5);
            cmdHeader.Parameters.AddWithValue("@Type", "GetRecords");
            DataTable dtGrdHeader = new DataTable();
            SqlDataAdapter daHeader = new SqlDataAdapter(cmdHeader);
            con.Open();
            daHeader.Fill(dtGrdHeader);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn(dtGrdHeader.Rows[0]["RuleText"].ToString(), typeof(string)),
                         new DataColumn(dtGrdHeader.Rows[1]["RuleText"].ToString(), typeof(string)),
                          new DataColumn(dtGrdHeader.Rows[2]["RuleText"].ToString(), typeof(string)),
                           new DataColumn(dtGrdHeader.Rows[3]["RuleText"].ToString(), typeof(string)),
                            new DataColumn(dtGrdHeader.Rows[4]["RuleText"].ToString(), typeof(decimal)),
                            new DataColumn(dtGrdHeader.Rows[5]["RuleText"].ToString(), typeof(decimal)),
                            new DataColumn(dtGrdHeader.Rows[6]["RuleText"].ToString(), typeof(decimal)) });

            string NameAndAddressoftheLandlord = string.Empty;
            string AddressofAccommodation = string.Empty;
            string CityName = string.Empty;
            string PANNOoftheowner = string.Empty;
            decimal RentAmountPM = 0;
            int EffectedFrom = 0;
            decimal TotalRentAmountPerAnnum = 0;

            foreach (GridViewRow row in grdHeader5.Rows)
            {
                dt.Rows.Add(NameAndAddressoftheLandlord, AddressofAccommodation, CityName, PANNOoftheowner, RentAmountPM, EffectedFrom, TotalRentAmountPerAnnum);
            }

            dt.Rows.Add(NameAndAddressoftheLandlord, AddressofAccommodation, CityName, PANNOoftheowner, RentAmountPM, EffectedFrom, TotalRentAmountPerAnnum);

            cmd = new SqlCommand("ManageTDSHeaders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HeaderText", null);
            cmd.Parameters.AddWithValue("@HeaderID", 5);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            DataTable dtHeader = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtHeader);
            con.Close();
            grdHeader5.Caption = "<b><span style='font-size:18px;'>" + dtHeader.Rows[0]["HeaderText"].ToString() + "</span></b>";
            grdHeader5.DataSource = dt;
            grdHeader5.DataBind();

            Label lblHead1 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead1"));
            lblHead1.Text = dtGrdHeader.Rows[0]["RuleText"].ToString();

            Label lblHead2 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead2"));
            lblHead2.Text = dtGrdHeader.Rows[1]["RuleText"].ToString();

            Label lblHead3 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead3"));
            lblHead3.Text = dtGrdHeader.Rows[2]["RuleText"].ToString();

            Label lblHead4 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead4"));
            lblHead4.Text = dtGrdHeader.Rows[3]["RuleText"].ToString();

            Label lblHead5 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead5"));
            lblHead5.Text = dtGrdHeader.Rows[4]["RuleText"].ToString();

            Label lblHead6 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead6"));
            lblHead6.Text = dtGrdHeader.Rows[5]["RuleText"].ToString();

            Label lblHead7 = ((Label)this.grdHeader5.HeaderRow.FindControl("lblHead7"));
            lblHead7.Text = dtGrdHeader.Rows[6]["RuleText"].ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindDeclaration()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageTDSHeaders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HeaderText", null);
            cmd.Parameters.AddWithValue("@DisplayLevel", 6);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dtHeader = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtHeader);
            con.Close();

            if (dtHeader.Rows.Count > 0)
            {
                lblDeclaration.Text = dtHeader.Rows[0]["HeaderText"].ToString();
            }
            else
            {
                lblDeclaration.Text = "No Declaration Found.";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtHeaderAmount2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal HeaderAmount2 = 0;
            decimal FooterAmount = 0;
            TextBox txtFooterAmount2 = ((TextBox)this.grdHeader2.FooterRow.FindControl("txtFooterAmount2"));

            foreach (GridViewRow row in grdHeader2.Rows)
            {
                if (((TextBox)row.FindControl("txtHeaderAmount2")).Text.Length > 0)
                {
                    HeaderAmount2 = Convert.ToDecimal((((TextBox)row.FindControl("txtHeaderAmount2")).Text));
                }
                else
                {
                    HeaderAmount2 = 0;
                    ((TextBox)row.FindControl("txtHeaderAmount2")).Text = HeaderAmount2.ToString();
                }

                if (((Label)row.FindControl("RuleText2")).Text.Contains("Others") && HeaderAmount2 > 0)
                {
                    pnlOthers.Visible = true;
                }
                else
                {
                    pnlOthers.Visible = false;
                    txtOthers.Text = string.Empty;
                }

                FooterAmount = FooterAmount + HeaderAmount2;
                txtFooterAmount2.Text = Converter(FooterAmount).ToString("0.00");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdHeader1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                TextBox txtHeaderAmount1 = (TextBox)e.Row.FindControl("txtHeaderAmount1");
                Label RuleID1 = (Label)e.Row.FindControl("RuleID1");
                Label HeaderID1 = (Label)e.Row.FindControl("HeaderID1");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@FromYear", FromYear);
                cmd.Parameters.AddWithValue("@ToYear", ToYear);
                cmd.Parameters.AddWithValue("@HeaderID", HeaderID1.Text);
                cmd.Parameters.AddWithValue("@RuleID", RuleID1.Text);
                cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                cmd.Parameters.AddWithValue("@AgeCriteria", null);
                cmd.Parameters.AddWithValue("@OthersDetails", null);
                cmd.Parameters.AddWithValue("@Place", null);
                cmd.Parameters.AddWithValue("@Signature", null);
                cmd.Parameters.AddWithValue("@Date", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@NameAndAddress", null);
                cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                cmd.Parameters.AddWithValue("@CityName", null);
                cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtHeaderAmount1.Text = dt.Rows[0]["ActualAmount"].ToString();
                    lblSeniorCitizen.Text = dt.Rows[0]["SeniorCitizen"].ToString();
                    if (lblSeniorCitizen.Text == "Yes")
                    {
                        pnlAge.Visible = true;
                    }
                    else
                    {
                        pnlAge.Visible = false;
                    }
                    lblAgeCriteria.Text = dt.Rows[0]["AgeCriteria"].ToString();
                    txtPlace.Text = dt.Rows[0]["Place"].ToString();
                    txtSignature.Text = dt.Rows[0]["Signature"].ToString();
                    txtDate.Text = dt.Rows[0]["Date"].ToString();
                    btnSave.Text = "Update Details";
                    //btnDeactivate.Visible = true;
                    btnPrintSelected.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdHeader2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                TextBox txtHeaderAmount2 = (TextBox)e.Row.FindControl("txtHeaderAmount2");
                Label RuleID2 = (Label)e.Row.FindControl("RuleID2");
                Label HeaderID2 = (Label)e.Row.FindControl("HeaderID2");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@FromYear", FromYear);
                cmd.Parameters.AddWithValue("@ToYear", ToYear);
                cmd.Parameters.AddWithValue("@HeaderID", HeaderID2.Text);
                cmd.Parameters.AddWithValue("@RuleID", RuleID2.Text);
                cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                cmd.Parameters.AddWithValue("@AgeCriteria", null);
                cmd.Parameters.AddWithValue("@OthersDetails", null);
                cmd.Parameters.AddWithValue("@Place", null);
                cmd.Parameters.AddWithValue("@Signature", null);
                cmd.Parameters.AddWithValue("@Date", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@NameAndAddress", null);
                cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                cmd.Parameters.AddWithValue("@CityName", null);
                cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtHeaderAmount2.Text = dt.Rows[0]["ActualAmount"].ToString();
                    string OthersDetails = dt.Rows[0]["OthersDetails"].ToString();

                    if (OthersDetails.Length > 0)
                    {
                        pnlOthers.Visible = true;
                        txtOthers.Text = OthersDetails;
                    }
                    else
                    {
                        pnlOthers.Visible = false;
                        txtOthers.Text = string.Empty;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblRuleTextFooter2 = (Label)e.Row.FindControl("lblRuleTextFooter2");
                TextBox txtFooterAmount2 = (TextBox)e.Row.FindControl("txtFooterAmount2");

                decimal TotalFooterAmount = 0;
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageHeaderwiseFooter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FooterText", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@HeaderID", 2);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                grdHeader2.ShowFooter = true;

                if (dt.Rows.Count > 0)
                {
                    lblRuleTextFooter2.Text = dt.Rows[0]["FooterText"].ToString();
                    decimal HeaderAmount2 = 0;

                    foreach (GridViewRow row in grdHeader2.Rows)
                    {
                        HeaderAmount2 = Convert.ToDecimal(((TextBox)row.FindControl("txtHeaderAmount2")).Text);
                        TotalFooterAmount = TotalFooterAmount + HeaderAmount2;
                    }
                }

                txtFooterAmount2.Text = Converter(TotalFooterAmount).ToString("0.00");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdHeader3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                TextBox txtHeaderAmount3 = (TextBox)e.Row.FindControl("txtHeaderAmount3");
                Label RuleID3 = (Label)e.Row.FindControl("RuleID3");
                Label HeaderID3 = (Label)e.Row.FindControl("HeaderID3");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@FromYear", FromYear);
                cmd.Parameters.AddWithValue("@ToYear", ToYear);
                cmd.Parameters.AddWithValue("@HeaderID", HeaderID3.Text);
                cmd.Parameters.AddWithValue("@RuleID", RuleID3.Text);
                cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                cmd.Parameters.AddWithValue("@AgeCriteria", null);
                cmd.Parameters.AddWithValue("@OthersDetails", null);
                cmd.Parameters.AddWithValue("@Place", null);
                cmd.Parameters.AddWithValue("@Signature", null);
                cmd.Parameters.AddWithValue("@Date", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@NameAndAddress", null);
                cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                cmd.Parameters.AddWithValue("@CityName", null);
                cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtHeaderAmount3.Text = dt.Rows[0]["ActualAmount"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdHeader4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                TextBox txtHeaderAmount4 = (TextBox)e.Row.FindControl("txtHeaderAmount4");
                Label RuleID4 = (Label)e.Row.FindControl("RuleID4");
                Label HeaderID4 = (Label)e.Row.FindControl("HeaderID4");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@FromYear", FromYear);
                cmd.Parameters.AddWithValue("@ToYear", ToYear);
                cmd.Parameters.AddWithValue("@HeaderID", HeaderID4.Text);
                cmd.Parameters.AddWithValue("@RuleID", RuleID4.Text);
                cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                cmd.Parameters.AddWithValue("@AgeCriteria", null);
                cmd.Parameters.AddWithValue("@OthersDetails", null);
                cmd.Parameters.AddWithValue("@Place", null);
                cmd.Parameters.AddWithValue("@Signature", null);
                cmd.Parameters.AddWithValue("@Date", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@NameAndAddress", null);
                cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                cmd.Parameters.AddWithValue("@CityName", null);
                cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtHeaderAmount4.Text = dt.Rows[0]["ActualAmount"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdHeader5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                TextBox txtLandlordNameAndAddress = (TextBox)e.Row.FindControl("txtLandlordNameAndAddress");
                TextBox txtAddressofAccommodation = (TextBox)e.Row.FindControl("txtAddressofAccommodation");
                TextBox txtCityName = (TextBox)e.Row.FindControl("txtCityName");
                TextBox txtPANNOoftheowner = (TextBox)e.Row.FindControl("txtPANNOoftheowner");
                TextBox txtRentAmountPM = (TextBox)e.Row.FindControl("txtRentAmountPM");
                DropDownList ddlEffectedFrom = (DropDownList)e.Row.FindControl("ddlEffectedFrom");
                TextBox txtTotalRentAmountPerAnnum = (TextBox)e.Row.FindControl("txtTotalRentAmountPerAnnum");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@FromYear", FromYear);
                cmd.Parameters.AddWithValue("@ToYear", ToYear);
                cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                cmd.Parameters.AddWithValue("@AgeCriteria", null);
                cmd.Parameters.AddWithValue("@OthersDetails", null);
                cmd.Parameters.AddWithValue("@Place", null);
                cmd.Parameters.AddWithValue("@Signature", null);
                cmd.Parameters.AddWithValue("@Date", null);
                cmd.Parameters.AddWithValue("@User", null);
                cmd.Parameters.AddWithValue("@NameAndAddress", null);
                cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                cmd.Parameters.AddWithValue("@CityName", null);
                cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtLandlordNameAndAddress.Text = dt.Rows[0]["NameAndAddress"].ToString();
                    txtAddressofAccommodation.Text = dt.Rows[0]["AddressOfAccommodation"].ToString();
                    txtCityName.Text = dt.Rows[0]["CityName"].ToString();
                    txtPANNOoftheowner.Text = dt.Rows[0]["PanNoOfOwner"].ToString();
                    txtRentAmountPM.Text = dt.Rows[0]["RentAmountPM"].ToString();
                    ddlEffectedFrom.SelectedValue = dt.Rows[0]["EffectedFrom"].ToString();
                    txtTotalRentAmountPerAnnum.Text = dt.Rows[0]["RentAmountPerAnnum"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetTotalRentAmountPerAnnum()
    {
        try
        {
            foreach (GridViewRow row in grdHeader5.Rows)
            {
                decimal RentAmountPM = 0;
                int EffectedFrom = 0;
                decimal TotalRentAmountPerAnnum = 0;
                int MaxNoOfMonths = 12;
                int Count = 0;
                TextBox lblTotalRentAmountPerAnnum = (TextBox)row.FindControl("txtTotalRentAmountPerAnnum");

                if (((TextBox)row.FindControl("txtRentAmountPM")).Text.Length > 0 && Convert.ToInt32(((DropDownList)row.FindControl("ddlEffectedFrom")).SelectedValue) > 0)
                {
                    RentAmountPM = Convert.ToDecimal(((TextBox)row.FindControl("txtRentAmountPM")).Text);
                    EffectedFrom = Convert.ToInt32(((DropDownList)row.FindControl("ddlEffectedFrom")).SelectedValue);

                    if (EffectedFrom > 3)
                    {
                        for (int i = EffectedFrom; i <= MaxNoOfMonths; i++)
                        {
                            Count++;
                        }

                        Count = Count + 3;
                    }
                    else
                    {
                        MaxNoOfMonths = 3;

                        for (int i = EffectedFrom; i <= MaxNoOfMonths; i++)
                        {
                            Count++;
                        }
                    }

                    TotalRentAmountPerAnnum = RentAmountPM * Count;
                    lblTotalRentAmountPerAnnum.Text = Converter(TotalRentAmountPerAnnum).ToString("0.00");

                    if (TotalRentAmountPerAnnum <= 0)
                    {
                        ((TextBox)row.FindControl("txtRentAmountPM")).Text = "0";
                        ((DropDownList)row.FindControl("ddlEffectedFrom")).ClearSelection();
                        lblTotalRentAmountPerAnnum.Text = "0";
                    }
                }
                else if (((TextBox)row.FindControl("txtRentAmountPM")).Text.Length <= 0)
                {
                    ((TextBox)row.FindControl("txtRentAmountPM")).Text = "0";
                    ((DropDownList)row.FindControl("ddlEffectedFrom")).ClearSelection();
                    lblTotalRentAmountPerAnnum.Text = "0";
                }
                else if (Convert.ToInt32(((DropDownList)row.FindControl("ddlEffectedFrom")).SelectedValue) <= 0)
                {
                    lblTotalRentAmountPerAnnum.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtRentAmountPM_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotalRentAmountPerAnnum();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlEffectedFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotalRentAmountPerAnnum();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    //protected void btnDeactivate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string ProfileID = ddlemployee.SelectedValue;
    //        string FromYear = ddlYear.SelectedItem.Text;
    //        string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
    //        int Count = 0;

    //        SqlConnection con = new SqlConnection(constr);
    //        cmd = new SqlCommand("ManageTDSIDF", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
    //        cmd.Parameters.AddWithValue("@FromYear", FromYear);
    //        cmd.Parameters.AddWithValue("@ToYear", ToYear);
    //        cmd.Parameters.AddWithValue("@SeniorCitizen", null);
    //        cmd.Parameters.AddWithValue("@AgeCriteria", null);
    //        cmd.Parameters.AddWithValue("@OthersDetails", null);
    //        cmd.Parameters.AddWithValue("@Place", null);
    //        cmd.Parameters.AddWithValue("@Signature", null);
    //        cmd.Parameters.AddWithValue("@Date", null);
    //        cmd.Parameters.AddWithValue("@User", null);
    //        cmd.Parameters.AddWithValue("@NameAndAddress", null);
    //        cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
    //        cmd.Parameters.AddWithValue("@CityName", null);
    //        cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
    //        cmd.Parameters.AddWithValue("@Type", "Deactivate");
    //        con.Open();
    //        Count = cmd.ExecuteNonQuery();
    //        con.Close();
    //        if (Count > 0)
    //        {
    //            Reset();
    //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Deactivating Record.');", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
    //    }
    //}

    public void SaveIDF()
    {
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlTransaction trans = con.BeginTransaction();
        int HasSuccess = 0;

        try
        {
            if ((pnlOthers.Visible == true && txtOthers.Text.Length > 0) || pnlOthers.Visible == false)
            {
                string ProfileID = ddlemployee.SelectedValue;
                string FromYear = ddlYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                string SeniorCitizen = lblSeniorCitizen.Text;
                string AgeCriteria = lblAgeCriteria.Text;
                string OthersDetails = txtOthers.Text;
                string Place = txtPlace.Text;
                string Signature = txtSignature.Text;
                DateTime Date = Convert.ToDateTime(txtDate.Text);
                string User = Convert.ToString(hash["Name"].ToString());

                foreach (GridViewRow row in grdHeader1.Rows)
                {
                    int Count1 = 0;
                    string HeaderID1 = ((Label)row.FindControl("HeaderID1")).Text;
                    string RuleID1 = ((Label)row.FindControl("RuleID1")).Text;
                    string HeaderAmount1 = ((TextBox)row.FindControl("txtHeaderAmount1")).Text;
                    string MaxAmount1 = ((Label)row.FindControl("MaxAmount1")).Text;

                    cmd = new SqlCommand("ManageTDSIDF", con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", SeniorCitizen);
                    cmd.Parameters.AddWithValue("@AgeCriteria", AgeCriteria);
                    cmd.Parameters.AddWithValue("@HeaderID", HeaderID1);
                    cmd.Parameters.AddWithValue("@RuleID", RuleID1);
                    cmd.Parameters.AddWithValue("@MaxAmount", MaxAmount1);
                    cmd.Parameters.AddWithValue("@ActualAmount", HeaderAmount1);
                    cmd.Parameters.AddWithValue("@OthersDetails", OthersDetails);
                    cmd.Parameters.AddWithValue("@Place", Place);
                    cmd.Parameters.AddWithValue("@Signature", Signature);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", null);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmd.Parameters.AddWithValue("@CityName", null);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmd.Parameters.AddWithValue("@Type", "SaveIDF");
                    Count1 = cmd.ExecuteNonQuery();
                    if (Count1 == 0)
                    {
                        HasSuccess = 0;
                        break;
                    }
                    else
                    {
                        HasSuccess = HasSuccess + 1;
                    }
                }

                foreach (GridViewRow row in grdHeader2.Rows)
                {
                    int Count2 = 0;
                    string HeaderID2 = ((Label)row.FindControl("HeaderID2")).Text;
                    string RuleID2 = ((Label)row.FindControl("RuleID2")).Text;
                    string HeaderAmount2 = ((TextBox)row.FindControl("txtHeaderAmount2")).Text;
                    string MaxAmount2 = ((Label)row.FindControl("MaxAmount2")).Text;

                    cmd = new SqlCommand("ManageTDSIDF", con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", SeniorCitizen);
                    cmd.Parameters.AddWithValue("@AgeCriteria", AgeCriteria);
                    cmd.Parameters.AddWithValue("@HeaderID", HeaderID2);
                    cmd.Parameters.AddWithValue("@RuleID", RuleID2);
                    cmd.Parameters.AddWithValue("@MaxAmount", MaxAmount2);
                    cmd.Parameters.AddWithValue("@ActualAmount", HeaderAmount2);
                    cmd.Parameters.AddWithValue("@OthersDetails", OthersDetails);
                    cmd.Parameters.AddWithValue("@Place", Place);
                    cmd.Parameters.AddWithValue("@Signature", Signature);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", null);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmd.Parameters.AddWithValue("@CityName", null);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmd.Parameters.AddWithValue("@Type", "SaveIDF");
                    Count2 = cmd.ExecuteNonQuery();
                    if (Count2 == 0)
                    {
                        HasSuccess = 0;
                        break;
                    }
                    else
                    {
                        HasSuccess = HasSuccess + 1;
                    }
                }

                foreach (GridViewRow row in grdHeader3.Rows)
                {
                    int Count3 = 0;
                    string HeaderID3 = ((Label)row.FindControl("HeaderID3")).Text;
                    string RuleID3 = ((Label)row.FindControl("RuleID3")).Text;
                    string HeaderAmount3 = ((TextBox)row.FindControl("txtHeaderAmount3")).Text;
                    string MaxAmount3 = ((Label)row.FindControl("MaxAmount3")).Text;

                    cmd = new SqlCommand("ManageTDSIDF", con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", SeniorCitizen);
                    cmd.Parameters.AddWithValue("@AgeCriteria", AgeCriteria);
                    cmd.Parameters.AddWithValue("@HeaderID", HeaderID3);
                    cmd.Parameters.AddWithValue("@RuleID", RuleID3);
                    cmd.Parameters.AddWithValue("@MaxAmount", MaxAmount3);
                    cmd.Parameters.AddWithValue("@ActualAmount", HeaderAmount3);
                    cmd.Parameters.AddWithValue("@OthersDetails", OthersDetails);
                    cmd.Parameters.AddWithValue("@Place", Place);
                    cmd.Parameters.AddWithValue("@Signature", Signature);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", null);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmd.Parameters.AddWithValue("@CityName", null);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmd.Parameters.AddWithValue("@Type", "SaveIDF");
                    Count3 = cmd.ExecuteNonQuery();
                    if (Count3 == 0)
                    {
                        HasSuccess = 0;
                        break;
                    }
                    else
                    {
                        HasSuccess = HasSuccess + 1;
                    }
                }

                foreach (GridViewRow row in grdHeader4.Rows)
                {
                    int Count4 = 0;
                    string HeaderID4 = ((Label)row.FindControl("HeaderID4")).Text;
                    string RuleID4 = ((Label)row.FindControl("RuleID4")).Text;
                    string HeaderAmount4 = ((TextBox)row.FindControl("txtHeaderAmount4")).Text;
                    string MaxAmount4 = ((Label)row.FindControl("MaxAmount4")).Text;

                    cmd = new SqlCommand("ManageTDSIDF", con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", SeniorCitizen);
                    cmd.Parameters.AddWithValue("@AgeCriteria", AgeCriteria);
                    cmd.Parameters.AddWithValue("@HeaderID", HeaderID4);
                    cmd.Parameters.AddWithValue("@RuleID", RuleID4);
                    cmd.Parameters.AddWithValue("@MaxAmount", MaxAmount4);
                    cmd.Parameters.AddWithValue("@ActualAmount", HeaderAmount4);
                    cmd.Parameters.AddWithValue("@OthersDetails", OthersDetails);
                    cmd.Parameters.AddWithValue("@Place", Place);
                    cmd.Parameters.AddWithValue("@Signature", Signature);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", null);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmd.Parameters.AddWithValue("@CityName", null);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmd.Parameters.AddWithValue("@Type", "SaveIDF");
                    Count4 = cmd.ExecuteNonQuery();
                    if (Count4 == 0)
                    {
                        HasSuccess = 0;
                        break;
                    }
                    else
                    {
                        HasSuccess = HasSuccess + 1;
                    }
                }

                foreach (GridViewRow row in grdHeader5.Rows)
                {
                    int Count5 = 0;
                    string NameAndAddress = ((TextBox)row.FindControl("txtLandlordNameAndAddress")).Text;
                    string AddressOfAccommodation = ((TextBox)row.FindControl("txtAddressofAccommodation")).Text;
                    string CityName = ((TextBox)row.FindControl("txtCityName")).Text;
                    string PanNoOfOwner = ((TextBox)row.FindControl("txtPANNOoftheowner")).Text;
                    decimal RentAmountPM = Convert.ToDecimal(((TextBox)row.FindControl("txtRentAmountPM")).Text);
                    int EffectedFrom = Convert.ToInt32(((DropDownList)row.FindControl("ddlEffectedFrom")).SelectedValue);
                    decimal TotalRentAmountPerAnnum = Convert.ToDecimal(((TextBox)row.FindControl("txtTotalRentAmountPerAnnum")).Text);

                    cmd = new SqlCommand("ManageTDSIDF", con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                    cmd.Parameters.AddWithValue("@AgeCriteria", null);
                    cmd.Parameters.AddWithValue("@OthersDetails", null);
                    cmd.Parameters.AddWithValue("@Place", null);
                    cmd.Parameters.AddWithValue("@Signature", null);
                    cmd.Parameters.AddWithValue("@Date", null);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", NameAndAddress);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", AddressOfAccommodation);
                    cmd.Parameters.AddWithValue("@CityName", CityName);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", PanNoOfOwner);
                    cmd.Parameters.AddWithValue("@RentAmountPM", RentAmountPM);
                    cmd.Parameters.AddWithValue("@EffectedFrom", EffectedFrom);
                    cmd.Parameters.AddWithValue("@RentAmountPerAnnum", TotalRentAmountPerAnnum);
                    cmd.Parameters.AddWithValue("@Type", "SaveClaimingHouseRent");
                    Count5 = cmd.ExecuteNonQuery();
                    if (Count5 == 0)
                    {
                        HasSuccess = 0;
                        break;
                    }
                    else
                    {
                        HasSuccess = HasSuccess + 1;
                    }
                }

                if (HasSuccess > 0)
                {
                    trans.Commit();
                    con.Close();
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
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Other Details Section is Mandatory if any amount is filled in Others Criteria.');", true);
            }

        }
        catch (Exception ex)
        {
            trans.Rollback();
            con.Close();
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveIDF();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCancelForm_Click(object sender, EventArgs e)
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

    protected void btnPrintSelected_Click(object sender, EventArgs e)
    {
        try
        {
            txtPlace.Style.Add("margin-top", "18px");
            txtSignature.Style.Add("margin-top", "18px");
            txtDate.Style.Add("margin-top", "18px");
            Session["ctrl"] = pnlGetData;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}