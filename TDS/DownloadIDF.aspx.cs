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

public partial class SalaryModule_DownloadIDF : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal TotalFooterAmount = 0;
    string OthersDetails = string.Empty;
    string Place = string.Empty;
    string Signature = string.Empty;
    string Date = string.Empty;

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
                    Year();
                    BindEmpNature();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Year();
                BindEmpNature();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdrecord.PageIndex = e.NewPageIndex;
            Bindgrid();
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
            con.Close();
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
            ddlNatureOfEmp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Employee Nature", "0"));
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
            ddlFromFinancialYear.ClearSelection();
            ddlFromFinancialYear.Enabled = true;
            ddlEmployeeStatus.ClearSelection();
            ddlEmployeeStatus.Enabled = true;
            ddlNatureOfEmp.ClearSelection();
            ddlNatureOfEmp.Enabled = true;
            btnGetEmployee.Enabled = true;
            lblSTMT.Text = string.Empty;
            pnlStmt.Visible = false;
            lblTotalEmployees.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlDetail.Visible = false;
            pnlButtons.Visible = false;
            rptIDFData.DataSource = null;
            rptIDFData.DataBind();
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
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", ddlEmployeeStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            DataView dv = new DataView(dt);
            dv.Sort = "Name ASC";

            if (dt.Rows.Count > 0)
            {
                ddlFromFinancialYear.Enabled = false;
                ddlEmployeeStatus.Enabled = false;
                ddlNatureOfEmp.Enabled = false;
                btnGetEmployee.Enabled = false;
                pnlDetail.Visible = true;
                lblSTMT.Text = Session["SchoolName"].ToString() + "<br /> List of Employees";
                pnlStmt.Visible = true;
                pnlButtons.Visible = true;
            }
            else
            {
                ddlFromFinancialYear.Enabled = true;
                ddlEmployeeStatus.Enabled = true;
                ddlNatureOfEmp.Enabled = true;
                btnGetEmployee.Enabled = true;
                pnlDetail.Visible = false;
                pnlStmt.Visible = false;
                pnlButtons.Visible = false;
            }
            pnlTotalRecords.Visible = true;
            lblTotalEmployees.Text = dt.Rows.Count.ToString();
            grdrecord.Caption = string.Empty;
            grdrecord.DataSource = dv;
            grdrecord.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnGetEmployee_Click(object sender, EventArgs e)
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

    public void BindEmpDetails()
    {
        try
        {
            if (Convert.ToInt32(ddlFromFinancialYear.SelectedValue) > 0)
            {
                DataTable BasicInfodt = new DataTable();
                BasicInfodt.Columns.AddRange(new DataColumn[14] { new DataColumn("SchoolName", typeof(string)),
                new DataColumn("IDFText", typeof(string)),
                new DataColumn("ProfileID", typeof(string)),
                 new DataColumn("EmpCode", typeof(string)),
                 new DataColumn("Name", typeof(string)),
                 new DataColumn("Gender", typeof(string)),
                 new DataColumn("Designation", typeof(string)),
                 new DataColumn("ContactNo", typeof(string)),
                 new DataColumn("EmailID", typeof(string)),
                 new DataColumn("PanCardNo", typeof(string)),
                 new DataColumn("DOB", typeof(string)),
                 new DataColumn("AgeAsOnDate", typeof(string)),
                new DataColumn("SeniorCitizen", typeof(string)),
                new DataColumn("AgeCriteria", typeof(string))});

                foreach (GridViewRow row in grdrecord.Rows)
                {
                    if (((CheckBox)row.FindControl("SelectChk")).Checked == true)
                    {
                        string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                        int SelectedYear = 0;
                        int GetLastYear = 0;
                        string[] AgeDetails = null;
                        SelectedYear = Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text);
                        GetLastYear = SelectedYear + 1;
                        SqlConnection con = new SqlConnection(constr);
                        cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                        cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                        cmd.Parameters.AddWithValue("@IsActive", Status.All);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        con.Close();

                        if (dt.Rows.Count > 0)
                        {
                            string IDFText = "Investment Declaration Form - F.Y." + ddlFromFinancialYear.SelectedItem.Text + "-" + GetLastYear;
                            string Age = string.Empty;

                            if (dt.Rows[0]["DOB"].ToString().Length > 0)
                            {
                                DateTime dob = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                                Age = CalculateYourAge(dob, GetLastYear);
                                AgeDetails = GetAgeCriteria(Age, "", "").Split('_');
                            }

                            BasicInfodt.Rows.Add(Session["SchoolName"].ToString(), IDFText, dt.Rows[0]["ProfileID"].ToString(), dt.Rows[0]["Emp_Code"].ToString(),
                                dt.Rows[0]["Name"].ToString(), dt.Rows[0]["GenderText"].ToString(), dt.Rows[0]["DesignationText"].ToString(), dt.Rows[0]["MobileNo"].ToString(),
                                dt.Rows[0]["EmailID"].ToString(), dt.Rows[0]["PanCardNo"].ToString(), dt.Rows[0]["DOB"].ToString(), Age, AgeDetails[0].ToString(), AgeDetails[1].ToString());
                        }
                    }
                }

                rptIDFData.DataSource = BasicInfodt;
                rptIDFData.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public string GetAgeCriteria(string Age, string SeniorCitizen, string AgeCriteria)
    {
        try
        {
            int Years = 0;
            int Days = 0;

            if (Age.Length > 0)
            {
                string[] EmpAgeText = Age.Split(' ');
                Years = Convert.ToInt32(EmpAgeText[0]);
                Days = Convert.ToInt32(EmpAgeText[4]);

                if (Years > 60 || (Years == 60 && Days > 0))
                {
                    SeniorCitizen = "Yes";
                    AgeCriteria = "60+";
                }
                if (Years > 80 || (Years == 80 && Days > 0))
                {
                    SeniorCitizen = "Yes";
                    AgeCriteria = "80+";
                }
                if (Years < 60)
                {
                    SeniorCitizen = "No";
                    AgeCriteria = "Below 60";
                }
            }

            return SeniorCitizen + "_" + AgeCriteria;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
            return "";
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
        return String.Format("{0} Year's {1} Month's {2} Day's",
        Years, Months, Days);
    }

    public void GetIDFForm()
    {
        try
        {
            int SelectedYear = 0;
            int GetLastYear = 0;

            if (Convert.ToInt32(ddlFromFinancialYear.SelectedValue) > 0)
            {
                SelectedYear = Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text);
                GetLastYear = SelectedYear + 1;
                BindEmpDetails();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select From Financial Year.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {
        try
        {
            GetIDFForm();
            Session["ctrl"] = pnlPrint;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindHouseRent(GridView grdHeader5, string ProfileID)
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
            dt.Columns.AddRange(new DataColumn[8] {
                new DataColumn("ProfileID", typeof(string)),
                new DataColumn(dtGrdHeader.Rows[0]["RuleText"].ToString(), typeof(string)),
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
                dt.Rows.Add(ProfileID, NameAndAddressoftheLandlord, AddressofAccommodation, CityName, PANNOoftheowner, RentAmountPM, EffectedFrom, TotalRentAmountPerAnnum);
            }

            dt.Rows.Add(ProfileID, NameAndAddressoftheLandlord, AddressofAccommodation, CityName, PANNOoftheowner, RentAmountPM, EffectedFrom, TotalRentAmountPerAnnum);

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

            Label lblHead1 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead1"));
            lblHead1.Text = dtGrdHeader.Rows[0]["RuleText"].ToString();

            Label lblHead2 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead2"));
            lblHead2.Text = dtGrdHeader.Rows[1]["RuleText"].ToString();

            Label lblHead3 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead3"));
            lblHead3.Text = dtGrdHeader.Rows[2]["RuleText"].ToString();

            Label lblHead4 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead4"));
            lblHead4.Text = dtGrdHeader.Rows[3]["RuleText"].ToString();

            Label lblHead5 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead5"));
            lblHead5.Text = dtGrdHeader.Rows[4]["RuleText"].ToString();

            Label lblHead6 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead6"));
            lblHead6.Text = dtGrdHeader.Rows[5]["RuleText"].ToString();

            Label lblHead7 = ((Label)grdHeader5.HeaderRow.FindControl("lblHead7"));
            lblHead7.Text = dtGrdHeader.Rows[6]["RuleText"].ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindDeclaration(Label lblDeclaration)
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

    protected void rptIDFData_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblProfileID = (Label)e.Item.FindControl("lblProfileID");
                GridView grdHeader1 = (GridView)e.Item.FindControl("grdHeader1");
                GridView grdHeader2 = (GridView)e.Item.FindControl("grdHeader2");
                Panel pnlOthers = (Panel)e.Item.FindControl("pnlOthers");
                Label txtOthers = (Label)e.Item.FindControl("txtOthers");
                GridView grdHeader3 = (GridView)e.Item.FindControl("grdHeader3");
                GridView grdHeader4 = (GridView)e.Item.FindControl("grdHeader4");
                GridView grdHeader5 = (GridView)e.Item.FindControl("grdHeader5");
                Label lblDeclaration = (Label)e.Item.FindControl("lblDeclaration");
                Label txtPlace = (Label)e.Item.FindControl("txtPlace");
                Label txtSignature = (Label)e.Item.FindControl("txtSignature");
                Label txtDate = (Label)e.Item.FindControl("txtDate");

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
                        DataColumn newColumn = new System.Data.DataColumn("ProfileID", typeof(System.String));
                        newColumn.DefaultValue = lblProfileID.Text;
                        dt.Columns.Add(newColumn);

                        if (i == 1)
                        {
                            grdHeader1.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                            grdHeader1.DataSource = dt;
                            grdHeader1.DataBind();

                            txtPlace.Text = Place;
                            txtSignature.Text = Signature;
                            txtDate.Text = Date;
                        }
                        else if (i == 2)
                        {
                            grdHeader2.Caption = "<b><span style='font-size:18px;'>" + dt.Rows[0]["HeaderText"].ToString() + "</span></b>";
                            grdHeader2.DataSource = dt;
                            grdHeader2.DataBind();

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


                BindHouseRent(grdHeader5, lblProfileID.Text);
                BindDeclaration(lblDeclaration);

                con.Close();
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
                string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
                Label txtHeaderAmount1 = (Label)e.Row.FindControl("txtHeaderAmount1");
                Label ProfileID = (Label)e.Row.FindControl("ProfileID");
                Label RuleID1 = (Label)e.Row.FindControl("RuleID1");
                Label HeaderID1 = (Label)e.Row.FindControl("HeaderID1");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID.Text);
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
                    Place = dt.Rows[0]["Place"].ToString();
                    Signature = dt.Rows[0]["Signature"].ToString();
                    Date = dt.Rows[0]["Date"].ToString();
                }
                else
                {
                    Place = string.Empty;
                    Signature = string.Empty;
                    Date = string.Empty;
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
                Label ProfileID = (Label)e.Row.FindControl("ProfileID");
                string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
                Label txtHeaderAmount2 = (Label)e.Row.FindControl("txtHeaderAmount2");
                Label RuleID2 = (Label)e.Row.FindControl("RuleID2");
                Label HeaderID2 = (Label)e.Row.FindControl("HeaderID2");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID.Text);
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
                    TotalFooterAmount = TotalFooterAmount + Convert.ToDecimal(txtHeaderAmount2.Text);
                    OthersDetails = dt.Rows[0]["OthersDetails"].ToString();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblRuleTextFooter2 = (Label)e.Row.FindControl("lblRuleTextFooter2");
                Label txtFooterAmount2 = (Label)e.Row.FindControl("txtFooterAmount2");

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

                if (dt.Rows.Count > 0)
                {
                    lblRuleTextFooter2.Text = dt.Rows[0]["FooterText"].ToString();

                }

                txtFooterAmount2.Text = Converter(TotalFooterAmount).ToString("0.00");
                TotalFooterAmount = 0;
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
                Label ProfileID = (Label)e.Row.FindControl("ProfileID");
                string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
                Label txtHeaderAmount3 = (Label)e.Row.FindControl("txtHeaderAmount3");
                Label RuleID3 = (Label)e.Row.FindControl("RuleID3");
                Label HeaderID3 = (Label)e.Row.FindControl("HeaderID3");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID.Text);
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
                Label ProfileID = (Label)e.Row.FindControl("ProfileID");
                string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
                Label txtHeaderAmount4 = (Label)e.Row.FindControl("txtHeaderAmount4");
                Label RuleID4 = (Label)e.Row.FindControl("RuleID4");
                Label HeaderID4 = (Label)e.Row.FindControl("HeaderID4");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID.Text);
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
                Label ProfileID = (Label)e.Row.FindControl("ProfileID");
                string FromYear = ddlFromFinancialYear.SelectedItem.Text;
                string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
                Label txtLandlordNameAndAddress = (Label)e.Row.FindControl("txtLandlordNameAndAddress");
                Label txtAddressofAccommodation = (Label)e.Row.FindControl("txtAddressofAccommodation");
                Label txtCityName = (Label)e.Row.FindControl("txtCityName");
                Label txtPANNOoftheowner = (Label)e.Row.FindControl("txtPANNOoftheowner");
                Label txtRentAmountPM = (Label)e.Row.FindControl("txtRentAmountPM");
                Label ddlEffectedFrom = (Label)e.Row.FindControl("ddlEffectedFrom");
                Label txtTotalRentAmountPerAnnum = (Label)e.Row.FindControl("txtTotalRentAmountPerAnnum");

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageTDSIDF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID.Text);
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
                    ddlEffectedFrom.Text = dt.Rows[0]["MonthName"].ToString();
                    txtTotalRentAmountPerAnnum.Text = dt.Rows[0]["RentAmountPerAnnum"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}