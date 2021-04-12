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

public partial class SalaryModule_ImportIDF : System.Web.UI.Page
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
                    Year();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Year();
            }
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
            ddlFromFinancialYear.ClearSelection();
            ddlEmployeeStatus.ClearSelection();
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
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlTransaction trans = con.BeginTransaction();

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
            int HasSuccess = 0;
            string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
            string User = Convert.ToString(hash["Name"].ToString());
            string Place = "New Delhi";
            string Date = DateTime.Now.ToString();
            int Count = 0;
            int HeaderWiseRuleCount = 1;
            string msg = string.Empty;


            foreach (DataRow row in dt.Rows)
            {
                string Signature = dt.Rows[i]["Name"].ToString().Trim();
                string SeniorCitizen = dt.Rows[i]["Senior Citizen"].ToString().Trim();
                string AgeCriteria = dt.Rows[i]["Age Criteria"].ToString().Trim();
                decimal OtherAmount = dt.Rows[i]["Others (Please Provide Details) *"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(dt.Rows[i]["Others (Please Provide Details) *"].ToString().Trim());
                string OthersDetails = dt.Rows[i]["Others Details"].ToString().Trim();
                string NameAddLandlord = dt.Rows[i]["Name & Add# of the Landlord"].ToString().Trim();
                string AddOfAcc = dt.Rows[i]["Address of Accommodation"].ToString().Trim();
                string CityName = dt.Rows[i]["City Name"].ToString().Trim();
                string PanNo = dt.Rows[i]["PAN NO of the owner if rent paid exceeds Rs 100000/- Annum"].ToString().Trim();
                decimal RentAmountPM = dt.Rows[i]["Rent Amount P#M#"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(dt.Rows[i]["Rent Amount P#M#"].ToString().Trim());
                int EffectedFrom = dt.Rows[i]["Effected From"].ToString().Trim() == "" ? 0 : Convert.ToInt32(dt.Rows[i]["Effected From"].ToString().Trim());
                decimal TotalRentAmountPerAnnum = 0;
                int MaxNoOfMonths = 12;
                int CountRentAmount = 0;
                string ActualAmount = string.Empty;
                string RuleText = string.Empty;
                string HeaderID = string.Empty;
                string RuleID = string.Empty;
                string MaxAmount = string.Empty;

                if (OtherAmount > 0 && (OthersDetails == "" || OthersDetails == null))
                {
                    Count = -1;
                    HeaderWiseRuleCount = -1;
                    msg = "Failed : If the amount is greater than zero in the Others column for any employee then the amount details must be provided in the Others Details column.";
                    break;
                }

                if (RentAmountPM > 0 && EffectedFrom == 0)
                {
                    Count = -2;
                    HeaderWiseRuleCount = -2;
                    msg = "Failed : When entering any value greater than zero in the Rent Amount P.M. column, then the Effected From column must have the value ranging from 1 to 12, where 1 indicates Januray and 12 indicates December.";
                    break;
                }

                foreach (DataColumn dtcol in dt.Columns)
                {
                    if (dtcol.ColumnName == "ProfileID" || dtcol.ColumnName == "Emp_Code" || dtcol.ColumnName == "Name" || dtcol.ColumnName == "DOB" || dtcol.ColumnName == "Designation"
                        || dtcol.ColumnName == "Age as on Date" || dtcol.ColumnName == "Senior Citizen" || dtcol.ColumnName == "Age Criteria"
                        || dtcol.ColumnName == "From Financial Year" || dtcol.ColumnName == "Others Details")
                    {
                    }
                    else
                    {
                        ActualAmount = dt.Rows[i][dtcol.ColumnName].ToString();
                        RuleText = dtcol.ColumnName.Replace("(u", "[u").Replace("E)", "E]").Replace("#", ".").Replace("D)", "D]")
                            .Replace("C)", "C]").Replace("contribution to National", "contribution to National Pension Scheme u/s 80CCD(1B)").Replace("))", ")]");

                        SqlCommand cmdBindHeaderwiseRules = new SqlCommand("ManageHeaderwiseRules", con, trans);
                        cmdBindHeaderwiseRules.CommandType = CommandType.StoredProcedure;
                        cmdBindHeaderwiseRules.Parameters.AddWithValue("@RuleText", RuleText);
                        cmdBindHeaderwiseRules.Parameters.AddWithValue("@User", null);
                        cmdBindHeaderwiseRules.Parameters.AddWithValue("@HeaderID", 0);
                        cmdBindHeaderwiseRules.Parameters.AddWithValue("@Type", "GetRecords");
                        DataTable dtBindHeaderwiseRules = new DataTable();
                        SqlDataAdapter daBindHeaderwiseRules = new SqlDataAdapter(cmdBindHeaderwiseRules);
                        daBindHeaderwiseRules.Fill(dtBindHeaderwiseRules);

                        if (dtBindHeaderwiseRules.Rows.Count <= 0)
                        {
                            HeaderWiseRuleCount = 0;
                            break;
                        }
                        else
                        {
                            HeaderID = dtBindHeaderwiseRules.Rows[0]["HeaderID"].ToString();
                            RuleID = dtBindHeaderwiseRules.Rows[0]["RuleID"].ToString();
                            MaxAmount = dtBindHeaderwiseRules.Rows[0]["MaxAmount"].ToString();

                            cmd = new SqlCommand("ManageTDSIDF", con, trans);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ProfileID", dt.Rows[i]["ProfileID"].ToString());
                            cmd.Parameters.AddWithValue("@FromYear", dt.Rows[i]["From Financial Year"].ToString());
                            cmd.Parameters.AddWithValue("@ToYear", ToYear);
                            cmd.Parameters.AddWithValue("@SeniorCitizen", SeniorCitizen);
                            cmd.Parameters.AddWithValue("@AgeCriteria", AgeCriteria);
                            cmd.Parameters.AddWithValue("@HeaderID", HeaderID);
                            cmd.Parameters.AddWithValue("@RuleID", RuleID);
                            cmd.Parameters.AddWithValue("@MaxAmount", MaxAmount);
                            cmd.Parameters.AddWithValue("@OthersDetails", OthersDetails);
                            cmd.Parameters.AddWithValue("@Place", Place);
                            cmd.Parameters.AddWithValue("@Signature", Signature);
                            cmd.Parameters.AddWithValue("@Date", Date);
                            cmd.Parameters.AddWithValue("@User", User);

                            if (RuleText == "Name & Add. of the Landlord")
                            {
                                if (EffectedFrom > 0)
                                {
                                    if (EffectedFrom > 3)
                                    {
                                        for (int k = EffectedFrom; k <= MaxNoOfMonths; k++)
                                        {
                                            CountRentAmount++;
                                        }

                                        CountRentAmount = CountRentAmount + 3;
                                    }
                                    else
                                    {
                                        MaxNoOfMonths = 3;

                                        for (int k = EffectedFrom; k <= MaxNoOfMonths; k++)
                                        {
                                            CountRentAmount++;
                                        }
                                    }

                                    TotalRentAmountPerAnnum = RentAmountPM * CountRentAmount;
                                }
                                else
                                {
                                    TotalRentAmountPerAnnum = 0;
                                    RentAmountPM = 0;
                                }

                                cmd.Parameters.AddWithValue("@NameAndAddress", NameAddLandlord);
                                cmd.Parameters.AddWithValue("@AddressOfAccommodation", AddOfAcc);
                                cmd.Parameters.AddWithValue("@CityName", CityName);
                                cmd.Parameters.AddWithValue("@PanNoOfOwner", PanNo);
                                cmd.Parameters.AddWithValue("@RentAmountPM", RentAmountPM);
                                cmd.Parameters.AddWithValue("@EffectedFrom", EffectedFrom);
                                cmd.Parameters.AddWithValue("@RentAmountPerAnnum", TotalRentAmountPerAnnum);
                                cmd.Parameters.AddWithValue("@Type", "SaveClaimingHouseRent");
                                Count = cmd.ExecuteNonQuery();
                                if (Count <= 0)
                                {
                                    Count = 0;
                                    HeaderWiseRuleCount = 0;
                                }
                                break;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ActualAmount", ActualAmount == "" ? "0" : ActualAmount);
                                cmd.Parameters.AddWithValue("@Type", "SaveIDF");
                                Count = cmd.ExecuteNonQuery();
                                if (Count <= 0)
                                {
                                    Count = 0;
                                    HeaderWiseRuleCount = 0;
                                    break;
                                }
                            }
                        }
                    }
                }


                if (HeaderWiseRuleCount > 0 && Count > 0)
                {
                    i++;
                    continue;
                }
                else
                {
                    Count = -1;
                    break;
                }
            }

            if (Count <= 0)
            {
                HasSuccess = 0;
            }
            else
            {
                HasSuccess = HasSuccess + 1;
            }

            if (HasSuccess > 0)
            {
                trans.Commit();
                con.Close();
                Clear();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
            }
            else
            {
                trans.Rollback();
                con.Close();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + msg + "');", true);
            }

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            con.Close();
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Column Name Mismatch.');", true);
        }
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

    private void ExportGridToExcel()
    {
        try
        {
            string ToYear = (Convert.ToInt32(ddlFromFinancialYear.SelectedItem.Text) + 1).ToString();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", ddlEmployeeStatus.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Fill_IDF_" + ddlFromFinancialYear.SelectedItem.Text + Session["SchoolPrefix"].ToString() + ".xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;
                int Count = 0;
                string Age = string.Empty;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    if (dtcol.ColumnName == "ProfileID" || dtcol.ColumnName == "Emp_Code" || dtcol.ColumnName == "Name" || dtcol.ColumnName == "DOB" || dtcol.ColumnName == "Designation")
                    {
                        Response.Write(str + dtcol.ColumnName);
                        str = "\t";
                        Count++;
                    }
                    else
                    {
                        if (Count == 5)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                Response.Write(str + "Age as on Date");
                Response.Write(str + "Senior Citizen");
                Response.Write(str + "Age Criteria");
                Response.Write(str + "From Financial Year");

                for (int m = 1; m <= 5; m++)
                {
                    SqlCommand cmdBindHeaderwiseRules = new SqlCommand("ManageHeaderwiseRules", con);
                    cmdBindHeaderwiseRules.CommandType = CommandType.StoredProcedure;
                    cmdBindHeaderwiseRules.Parameters.AddWithValue("@RuleText", null);
                    cmdBindHeaderwiseRules.Parameters.AddWithValue("@User", null);
                    cmdBindHeaderwiseRules.Parameters.AddWithValue("@HeaderID", m);
                    cmdBindHeaderwiseRules.Parameters.AddWithValue("@Type", "GetRecords");
                    DataTable dtBindHeaderwiseRules = new DataTable();
                    SqlDataAdapter daBindHeaderwiseRules = new SqlDataAdapter(cmdBindHeaderwiseRules);
                    daBindHeaderwiseRules.Fill(dtBindHeaderwiseRules);

                    if (dtBindHeaderwiseRules.Rows.Count > 0)
                    {
                        for (int a = 0; a < dtBindHeaderwiseRules.Rows.Count; a++)
                        {
                            if (dtBindHeaderwiseRules.Rows[a]["RuleText"].ToString() == "Total Rent Amount / Annum" || dtBindHeaderwiseRules.Rows[a]["RuleText"].ToString() == "Total Rent Amount/Annum")
                            {
                            }
                            else
                            {
                                Response.Write(str + dtBindHeaderwiseRules.Rows[a]["RuleText"].ToString());

                                if (dtBindHeaderwiseRules.Rows[a]["RuleText"].ToString().Contains("Others"))
                                {
                                    Response.Write(str + "Others Details");
                                }
                            }
                        }
                    }
                }

                str = "\t";
                Response.Write("\n");
                int Row = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 0 || j == 2 || j == 5 || j == 6 || j == 8)
                        {
                            Response.Write(str + Convert.ToString(dr[j]));
                            str = "\t";
                        }

                        if (j == 8)
                        {
                            Age = CalculateYourAge(Convert.ToDateTime(dr[6]));
                            Response.Write(str + Age);

                            string[] EmpAgeText = Age.Split(' ');
                            int Years = 0;
                            int Days = 0;
                            Years = Convert.ToInt32(EmpAgeText[0]);
                            Days = Convert.ToInt32(EmpAgeText[4]);

                            if (Years > 80 || (Years == 80 && Days > 0))
                            {
                                Response.Write(str + "Yes");
                                Response.Write(str + "80+");
                            }
                            else if (Years > 60 || (Years == 60 && Days > 0))
                            {
                                Response.Write(str + "Yes");
                                Response.Write(str + "60+");
                            }
                            else if (Years < 60)
                            {
                                Response.Write(str + "No");
                                Response.Write(str + string.Empty);
                            }
                        }
                    }

                    Response.Write(str + ddlFromFinancialYear.SelectedItem.Text);

                    for (int i = 1; i <= 4; i++)
                    {
                        SqlCommand cmdHeaderwiseRules = new SqlCommand("ManageHeaderwiseRules", con);
                        cmdHeaderwiseRules.CommandType = CommandType.StoredProcedure;
                        cmdHeaderwiseRules.Parameters.AddWithValue("@RuleText", null);
                        cmdHeaderwiseRules.Parameters.AddWithValue("@User", null);
                        cmdHeaderwiseRules.Parameters.AddWithValue("@HeaderID", i);
                        cmdHeaderwiseRules.Parameters.AddWithValue("@Type", "GetRecords");
                        DataTable dtHeaderwiseRules = new DataTable();
                        SqlDataAdapter daHeaderwiseRules = new SqlDataAdapter(cmdHeaderwiseRules);
                        daHeaderwiseRules.Fill(dtHeaderwiseRules);

                        if (dtHeaderwiseRules.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtHeaderwiseRules.Rows.Count; k++)
                            {
                                SqlCommand cmdManageTDSIDF = new SqlCommand("ManageTDSIDF", con);
                                cmdManageTDSIDF.CommandType = CommandType.StoredProcedure;
                                cmdManageTDSIDF.Parameters.AddWithValue("@ProfileID", dt.Rows[Row]["ProfileID"].ToString());
                                cmdManageTDSIDF.Parameters.AddWithValue("@FromYear", ddlFromFinancialYear.SelectedItem.Text);
                                cmdManageTDSIDF.Parameters.AddWithValue("@ToYear", ToYear);
                                cmdManageTDSIDF.Parameters.AddWithValue("@HeaderID", i);
                                cmdManageTDSIDF.Parameters.AddWithValue("@RuleID", dtHeaderwiseRules.Rows[k]["RuleID"].ToString());
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

                                if (dtManageTDSIDF.Rows.Count > 0)
                                {
                                    Response.Write(str + dtManageTDSIDF.Rows[0]["ActualAmount"].ToString());

                                    if (dtHeaderwiseRules.Rows[k]["RuleText"].ToString().Contains("Others"))
                                    {
                                        Response.Write(str + dtManageTDSIDF.Rows[0]["OthersDetails"].ToString().Replace("\n", " "));
                                    }
                                }
                            }
                        }
                    }

                    SqlCommand cmdHouseRent = new SqlCommand("ManageTDSIDF", con);
                    cmdHouseRent.CommandType = CommandType.StoredProcedure;
                    cmdHouseRent.Parameters.AddWithValue("@ProfileID", dt.Rows[Row]["ProfileID"].ToString());
                    cmdHouseRent.Parameters.AddWithValue("@FromYear", ddlFromFinancialYear.SelectedItem.Text);
                    cmdHouseRent.Parameters.AddWithValue("@ToYear", ToYear);
                    cmdHouseRent.Parameters.AddWithValue("@SeniorCitizen", null);
                    cmdHouseRent.Parameters.AddWithValue("@AgeCriteria", null);
                    cmdHouseRent.Parameters.AddWithValue("@OthersDetails", null);
                    cmdHouseRent.Parameters.AddWithValue("@Place", null);
                    cmdHouseRent.Parameters.AddWithValue("@Signature", null);
                    cmdHouseRent.Parameters.AddWithValue("@Date", null);
                    cmdHouseRent.Parameters.AddWithValue("@User", null);
                    cmdHouseRent.Parameters.AddWithValue("@NameAndAddress", null);
                    cmdHouseRent.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmdHouseRent.Parameters.AddWithValue("@CityName", null);
                    cmdHouseRent.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmdHouseRent.Parameters.AddWithValue("@Type", "GetRecords");

                    DataTable dtHouseRent = new DataTable();
                    SqlDataAdapter daHouseRent = new SqlDataAdapter(cmdHouseRent);
                    daHouseRent.Fill(dtHouseRent);

                    if (dtHouseRent.Rows.Count > 0)
                    {
                        Response.Write(str + dtHouseRent.Rows[0]["NameAndAddress"].ToString().Replace("\n", " "));
                        Response.Write(str + dtHouseRent.Rows[0]["AddressOfAccommodation"].ToString().Replace("\n", " "));
                        Response.Write(str + dtHouseRent.Rows[0]["CityName"].ToString().Replace("\n", " "));
                        Response.Write(str + dtHouseRent.Rows[0]["PanNoOfOwner"].ToString().Replace("\n", " "));
                        Response.Write(str + dtHouseRent.Rows[0]["RentAmountPM"].ToString());
                        Response.Write(str + dtHouseRent.Rows[0]["EffectedFrom"].ToString());
                    }

                    con.Close();
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
            if (flIDFImport.HasFile && Convert.ToInt32(ddlFromFinancialYear.SelectedValue) > 0)
            {
                string FileName = ddlFromFinancialYear.SelectedItem.Text + "_" + Path.GetFileName(flIDFImport.PostedFile.FileName);
                if (FileName.Contains(Session["SchoolPrefix"].ToString()))
                {
                    string Extension = Path.GetExtension(flIDFImport.PostedFile.FileName);
                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    string FilePath = Server.MapPath(FolderPath + FileName);
                    flIDFImport.SaveAs(FilePath);
                    Import_To_Grid(FilePath, Extension);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Selected file does not belong to this school, please select correct file to save the IDF data.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please select both - File and From Financal Year.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}