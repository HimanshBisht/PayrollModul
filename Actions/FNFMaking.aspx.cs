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

public partial class SalaryModule_FNFMaking : System.Web.UI.Page
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

    public void Cancel()
    {
        try
        {
            btnDeactivate.Visible = false;
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            ddlemployee.Enabled = true;
            lblEmpName.Text = string.Empty;
            lblEmpCode.Text = string.Empty;
            lblDOJ.Text = string.Empty;
            lblDesignation.Text = string.Empty;
            lblStaffType.Text = string.Empty;
            lblNature.Text = string.Empty;
            lblResignDate.Text = string.Empty;
            lblActualLWD.Text = string.Empty;
            txtAsPerNormsLWD.Text = string.Empty;
            lblTotalWorking.Text = string.Empty;
            lblRecoveryDays.Text = string.Empty;
            pnlDetails.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlGrid.Visible = false;
            btnCalculateRecovery.Text = string.Empty;
            grdCalculateRecovery.DataSource = null;
            grdCalculateRecovery.DataBind();
            pnlCalculateRecoveryButton.Visible = false;
            pnlCalculateRecovery.Visible = false;
            pnlCalculateFNFButton.Visible = false;
            ClearCalculateFNF();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearCalculateFNF()
    {
        try
        {
            txtAsPerNormsLWD.ReadOnly = false;
            lblGrandTotal.Text = string.Empty;
            lblFNFRecoveryDays.Text = string.Empty;
            lblRecoveryAmount.Text = string.Empty;
            txtOtherAdjustment.Text = "0";
            txtOtherAdjustment.ReadOnly = false;
            btnAdd.Enabled = true;
            btnDeduct.Enabled = true;
            txtFNFRemarks.Text = string.Empty;
            lblNetFNFPayable.Text = string.Empty;
            pnlCalculateFNF.Visible = false;
            lblNetFNFText.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearTable()
    {
        try
        {
            lblEmpName.Text = string.Empty;
            lblEmpCode.Text = string.Empty;
            lblDOJ.Text = string.Empty;
            lblDesignation.Text = string.Empty;
            lblStaffType.Text = string.Empty;
            lblNature.Text = string.Empty;
            lblResignDate.Text = string.Empty;
            lblActualLWD.Text = string.Empty;
            txtAsPerNormsLWD.Text = string.Empty;
            lblTotalWorking.Text = string.Empty;
            lblRecoveryDays.Text = string.Empty;
            pnlDetails.Visible = false;
            pnlCalculateFNFButton.Visible = false;
            pnlCalculateRecoveryButton.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public string CalculateTotalWorking(DateTime Doj)
    {
        DateTime Now = Convert.ToDateTime(lblActualLWD.Text);
        int Years = new DateTime(Convert.ToDateTime(lblActualLWD.Text).Subtract(Doj).Ticks).Year - 1;
        DateTime PastYearDate = Doj.AddYears(Years);
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

    public void CalculateRecoveryDays()
    {
        try
        {
            decimal RecoveryDays = 0;

            if (txtAsPerNormsLWD.Text.Length > 0 && lblActualLWD.Text.Length > 0)
            {
                if (Convert.ToDateTime(lblActualLWD.Text) < Convert.ToDateTime(lblResignDate.Text))
                {
                    RecoveryDays = Convert.ToDecimal((Convert.ToDateTime(txtAsPerNormsLWD.Text) - Convert.ToDateTime(lblResignDate.Text)).TotalDays) + 1;
                }
                else if (Convert.ToDateTime(lblActualLWD.Text) > Convert.ToDateTime(lblResignDate.Text))
                {
                    RecoveryDays = Convert.ToDecimal((Convert.ToDateTime(txtAsPerNormsLWD.Text) - Convert.ToDateTime(lblActualLWD.Text)).TotalDays);
                }
                else if (Convert.ToDateTime(lblResignDate.Text) == Convert.ToDateTime(lblActualLWD.Text))
                {
                    RecoveryDays = Convert.ToDecimal((Convert.ToDateTime(txtAsPerNormsLWD.Text) - Convert.ToDateTime(lblResignDate.Text)).TotalDays);
                }

                if (RecoveryDays > 0)
                {
                    lblRecoveryDays.Text = RecoveryDays.ToString();
                    pnlCalculateRecoveryButton.Visible = true;
                    btnCalculateRecovery.Text = "Calculate Recovery For " + lblRecoveryDays.Text + " Days";
                    pnlCalculateFNFButton.Visible = false;
                }
                else
                {
                    lblRecoveryDays.Text = "0";
                    pnlCalculateFNFButton.Visible = true;
                    pnlCalculateRecoveryButton.Visible = false;
                    btnCalculateRecovery.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Please Enter a Valid Date.');", true);
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
            SqlCommand cmdExist = new SqlCommand("ManageFNFMaking", con);
            cmdExist.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmdExist.Parameters.AddWithValue("@IsActive", Status.Active);
            cmdExist.Parameters.AddWithValue("@Type", "GetRecord");
            cmdExist.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dtexist = new DataTable();
            SqlDataAdapter daExist = new SqlDataAdapter(cmdExist);
            daExist.Fill(dtexist);
            if (dtexist.Rows.Count > 0)
            {
                btnDeactivate.Visible = true;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : " + ddlemployee.SelectedItem.Text + " FNF is Already Exist, In Case if you want to change something in existing FNF then please Deactivate that record first.');", true);
            }
            else
            {
                btnDeactivate.Visible = false;
                lblEmpName.Text = string.Empty;
                lblEmpCode.Text = string.Empty;
                lblDOJ.Text = string.Empty;
                lblDesignation.Text = string.Empty;
                lblStaffType.Text = string.Empty;
                lblNature.Text = string.Empty;
                lblResignDate.Text = string.Empty;
                lblActualLWD.Text = string.Empty;
                txtAsPerNormsLWD.Text = string.Empty;
                lblTotalWorking.Text = string.Empty;
                lblRecoveryDays.Text = string.Empty;
                pnlDetails.Visible = false;
                grdrecord.DataSource = null;
                grdrecord.DataBind();
                pnlGrid.Visible = false;
                btnCalculateRecovery.Text = string.Empty;
                grdCalculateRecovery.DataSource = null;
                grdCalculateRecovery.DataBind();
                pnlCalculateRecoveryButton.Visible = false;
                pnlCalculateRecovery.Visible = false;
                pnlCalculateFNFButton.Visible = false;
                ClearCalculateFNF();

                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0 && dt.Rows[0]["ResignationDate"].ToString().Length > 0 && dt.Rows[0]["LWD"].ToString().Length > 0)
                {
                    int FNFMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                    int FNFYear = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    int ActualLWDMonth = Convert.ToDateTime(dt.Rows[0]["LWD"].ToString()).Month;
                    int ActualLWDYear = Convert.ToDateTime(dt.Rows[0]["LWD"].ToString()).Year;

                    if ((FNFMonth >= ActualLWDMonth && FNFYear >= ActualLWDYear) || (FNFYear > ActualLWDYear))
                    {
                        lblEmpName.Text = dt.Rows[0]["Name"].ToString();
                        lblEmpCode.Text = dt.Rows[0]["Emp_Code"].ToString();
                        lblDOJ.Text = dt.Rows[0]["DOJ"].ToString();
                        lblDesignation.Text = dt.Rows[0]["DesignationText"].ToString();
                        lblStaffType.Text = dt.Rows[0]["StaffTypeText"].ToString();
                        lblNature.Text = dt.Rows[0]["NatureOfEmpText"].ToString();
                        lblResignDate.Text = dt.Rows[0]["ResignationDate"].ToString();
                        lblActualLWD.Text = dt.Rows[0]["LWD"].ToString();
                        DateTime DOJ = Convert.ToDateTime(lblDOJ.Text);
                        lblTotalWorking.Text = CalculateTotalWorking(DOJ);

                        SqlCommand cmd1 = new SqlCommand("ManageNoticePeriodDays", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@User", null);
                        cmd1.Parameters.AddWithValue("@NatureID", dt.Rows[0]["NatureOfEmp"].ToString());
                        cmd1.Parameters.AddWithValue("@Type", "GetRecords");
                        DataTable dtNoticeDays = new DataTable();
                        da = new SqlDataAdapter(cmd1);
                        da.Fill(dtNoticeDays);
                        pnlDetails.Visible = true;
                        if (dtNoticeDays.Rows.Count > 0)
                        {
                            txtAsPerNormsLWD.Text = (Convert.ToDateTime(dt.Rows[0]["ResignationDate"].ToString()).AddDays(Convert.ToDouble(dtNoticeDays.Rows[0]["NoticeDays"]) - 1)).ToString("dd MMM yyyy");
                            CalculateRecoveryDays();
                            Bindgrid();
                            ddlemployee.Enabled = false;
                        }
                        else
                        {
                            ddlemployee.Enabled = true;
                            ClearTable();
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Please Define the Notice Days. ');", true);
                        }
                    }
                    else
                    {
                        ClearTable();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Invalid Request, Actual Last Working Date should always be less or Equal to the Selected FNF Month and Year.');", true);
                    }
                }
                else
                {
                    ClearTable();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Please Update Resignation Date and Last Working Date (LWD) in Profile. ');", true);
                }
            }
            con.Close();
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
            Cancel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtAsPerNormsLWD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateRecoveryDays();
            grdCalculateRecovery.DataSource = null;
            grdCalculateRecovery.DataBind();
            pnlCalculateRecovery.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public enum SalaryStatus
    {
        All = 0,
        Release = 1,
        Hold = 2
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
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@SalaryMakeStatus", SalaryStatus.Hold);
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
            }

            pnlGrid.Visible = true;
            grdrecord.DataSource = dt;
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

    protected void btnCalculateFNF_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCalculateFNF();
            txtAsPerNormsLWD.ReadOnly = true;
            pnlCalculateFNF.Visible = true;
            GridViewRow Footerrow = grdrecord.FooterRow;
            decimal FooterGrandTotal = 0;
            if (grdrecord.Rows.Count > 0)
            {
                FooterGrandTotal = Convert.ToDecimal(((Label)Footerrow.FindControl("lblGrandTotalGrossTotalSalary")).Text);
            }
            decimal NetFNFPayable = 0;
            lblGrandTotal.Text = FooterGrandTotal.ToString();
            lblFNFRecoveryDays.Text = lblRecoveryDays.Text;
            if (Convert.ToDecimal(lblFNFRecoveryDays.Text) == 0)
            {
                lblRecoveryAmount.Text = "0";
            }
            else
            {
                foreach (GridViewRow row in grdCalculateRecovery.Rows)
                {
                    decimal GrossTotalRecovery = Convert.ToDecimal(((Label)row.FindControl("lblGrossTotalRecovery")).Text);
                    lblRecoveryAmount.Text = GrossTotalRecovery.ToString();
                }
            }
            NetFNFPayable = (Convert.ToDecimal(lblGrandTotal.Text) - Convert.ToDecimal(lblRecoveryAmount.Text));
            lblNetFNFPayable.Text = NetFNFPayable.ToString();
            if (NetFNFPayable >= 0)
            {
                lblNetFNFText.Text = "Net FNF Payable";
            }
            else
            {
                lblNetFNFText.Text = "Net FNF Recovery";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            decimal FNFNetPayable = 0;
            decimal OtherAdjustment = 0;

            FNFNetPayable = Convert.ToDecimal(lblNetFNFPayable.Text);
            OtherAdjustment = Convert.ToDecimal(txtOtherAdjustment.Text);
            FNFNetPayable = FNFNetPayable + OtherAdjustment;
            lblNetFNFPayable.Text = FNFNetPayable.ToString();
            txtOtherAdjustment.ReadOnly = true;
            btnAdd.Enabled = false;
            btnDeduct.Enabled = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnDeduct_Click(object sender, EventArgs e)
    {
        try
        {
            decimal FNFNetPayable = 0;
            decimal OtherAdjustment = 0;

            FNFNetPayable = Convert.ToDecimal(lblNetFNFPayable.Text);
            OtherAdjustment = Convert.ToDecimal(txtOtherAdjustment.Text);
            FNFNetPayable = FNFNetPayable - OtherAdjustment;
            lblNetFNFPayable.Text = FNFNetPayable.ToString();
            txtOtherAdjustment.ReadOnly = true;
            btnAdd.Enabled = false;
            btnDeduct.Enabled = false;
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

    public void Reset()
    {
        try
        {
            lblNetFNFPayable.Text = (Convert.ToDecimal(lblGrandTotal.Text) - Convert.ToDecimal(lblRecoveryAmount.Text)).ToString();
            txtOtherAdjustment.Text = "0";
            txtOtherAdjustment.ReadOnly = false;
            btnAdd.Enabled = true;
            btnDeduct.Enabled = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSaveFNF_Click(object sender, EventArgs e)
    {
        try
        {
            decimal OtherAdjustment = 0;

            if (txtOtherAdjustment.Text.Length > 0)
            {
                OtherAdjustment = Convert.ToDecimal(txtOtherAdjustment.Text);
            }

            if (OtherAdjustment > 0 && txtFNFRemarks.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Mention Some Remarks For any other Adjustment.');", true);
            }
            else
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageFNFMaking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@Name", lblEmpName.Text);
                cmd.Parameters.AddWithValue("@Emp_Code", lblEmpCode.Text);
                cmd.Parameters.AddWithValue("@DOJ", Convert.ToDateTime(lblDOJ.Text));
                cmd.Parameters.AddWithValue("@ResignDate", Convert.ToDateTime(lblResignDate.Text));
                cmd.Parameters.AddWithValue("@ActualLWD", Convert.ToDateTime(lblActualLWD.Text));
                cmd.Parameters.AddWithValue("@AsPerNormsLWD", Convert.ToDateTime(txtAsPerNormsLWD.Text));
                cmd.Parameters.AddWithValue("@TotalWorking", lblTotalWorking.Text);
                cmd.Parameters.AddWithValue("@GrandHoldTotal", lblGrandTotal.Text);
                cmd.Parameters.AddWithValue("@RecoveryDays", lblFNFRecoveryDays.Text);
                cmd.Parameters.AddWithValue("@RecoveryAmount", lblRecoveryAmount.Text);
                cmd.Parameters.AddWithValue("@OtherAdjustment", OtherAdjustment);
                cmd.Parameters.AddWithValue("@Remarks", txtFNFRemarks.Text);
                cmd.Parameters.AddWithValue("@NetFNFPayableORRecovery", lblNetFNFPayable.Text);
                cmd.Parameters.AddWithValue("@FNFMonthID", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@FNFYearID", ddlYear.Text);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@Type", "Save");
                con.Open();
                int HasRow = cmd.ExecuteNonQuery();
                con.Close();
                if (HasRow > 0)
                {
                    Cancel();
                    ClearTable();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('FNF Record Saved Sucessfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : " + ddlemployee.SelectedItem.Text + " Already Exist.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCancelFNF_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCalculateFNF();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCalculateRecovery_Click(object sender, EventArgs e)
    {
        try
        {
            BindRecoverygrid();
            CalculateRecovery();
            pnlCalculateFNFButton.Visible = true;
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
                int AsPerNormsYear = Convert.ToDateTime(txtAsPerNormsLWD.Text).Year;
                int AsPerNormsMonth = Convert.ToDateTime(txtAsPerNormsLWD.Text).Month;
                // MonthDays = System.DateTime.DaysInMonth(AsPerNormsYear, AsPerNormsMonth);

                // Set As Standard Month Days For Calculating Recovery.
                MonthDays = 30;

                foreach (GridViewRow row in grdCalculateRecovery.Rows)
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

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    public void GetImportAllData()
    {
        try
        {
            foreach (GridViewRow row in grdCalculateRecovery.Rows)
            {
                Label lblDeduction = (Label)row.FindControl("lblDeduction");

                int MonthID = Convert.ToDateTime(txtAsPerNormsLWD.Text).Year;
                int YearID = Convert.ToDateTime(txtAsPerNormsLWD.Text).Month;

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetImportDataDetails", con);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@AssignEmpCode", null);
                cmd.Parameters.AddWithValue("@MonthID", MonthID);
                cmd.Parameters.AddWithValue("@YearID", YearID);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                if (ds.Tables[2].Rows.Count > 0)
                {
                    lblDeduction.Text = ds.Tables[2].Rows[0]["DeductionValue"].ToString();
                }
                else
                {
                    lblDeduction.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateRecovery()
    {
        try
        {
            int i = 0;

            foreach (GridViewRow row in grdCalculateRecovery.Rows)
            {
                string Name = ((Label)row.FindControl("lblName")).Text;
                string Emp_Code = ((Label)row.FindControl("lblEmp_Code")).Text;
                Label lblProfileID = (Label)row.FindControl("lblProfileID");
                Label lblFinalRecoveryDays = (Label)row.FindControl("lblFinalRecoveryDays");
                Label lblPayDrawnBasic = (Label)row.FindControl("lblPayDrawnBasic");
                Label lblDaOnBasic = (Label)row.FindControl("lblDaOnBasic");
                Label lblHraOnBasic = (Label)row.FindControl("lblHraOnBasic");
                Label lblTransportOnBasic = (Label)row.FindControl("lblTransportOnBasic");
                Label lblMedicalOnBasic = (Label)row.FindControl("lblMedicalOnBasic");
                Label lblWashingOnBasic = (Label)row.FindControl("lblWashingOnBasic");
                Label lblGrossRevisedRecovery = (Label)row.FindControl("lblGrossRevisedRecovery");
                Label lblExGratiaOnBasic = (Label)row.FindControl("lblExGratiaOnBasic");
                Label lblGrossTotal = (Label)row.FindControl("lblGrossTotal");
                Label lblDeduction = (Label)row.FindControl("lblDeduction");
                Label lblGrossTotalRecovery = (Label)row.FindControl("lblGrossTotalRecovery");


                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@SalaryStatus", Status.Active);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.Parameters.AddWithValue("@ProfileID", lblProfileID.Text);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                decimal HRA = 0;
                if (dt.Rows.Count > 0)
                {
                    decimal BasicSalary = Convert.ToDecimal(dt.Rows[i]["BasicScale"].ToString());
                    decimal MonthDays = Convert.ToDecimal(((Label)row.FindControl("lblMonthDays")).Text);
                    decimal SalaryRecoveryDays = Convert.ToDecimal(lblRecoveryDays.Text);
                    lblFinalRecoveryDays.Text = SalaryRecoveryDays.ToString();
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
                    decimal Deduction = Convert.ToDecimal(((Label)row.FindControl("lblDeduction")).Text);
                    decimal SalaryPerDay = 0;
                    decimal FinalSalaryPerDay = 0;
                    decimal TotalPaidBasicSalary = 0;
                    decimal TotalDA = 0;
                    decimal TotalHRA = 0;
                    decimal TotalTransport = 0;
                    decimal TotalMedical = 0;
                    decimal TotalWashing = 0;
                    decimal TotalExGratia = 0;
                    decimal GrossRevisedRecovery = 0;
                    decimal GrossTotal = 0;
                    decimal GrossTotalRecoveryAfterDeduction = 0;
                    decimal Total = 0;
                    decimal EffectedGrossTotalRecoveryAfterDeduction = 0;
                    decimal EffectedTotal = 0;
                    string ChangeScale = dt.Rows[i]["ChangeScaleText"].ToString();
                    decimal HRAPerDay = 0;

                    // Calculate Salary On Basic with Total salary Paid Days start here.

                    if (BasicSalary > 0 && SalaryRecoveryDays > 0)
                    {

                        SalaryPerDay = (BasicSalary / MonthDays);

                        TotalPaidBasicSalary = Converter((SalaryPerDay * SalaryRecoveryDays));

                        TotalDA = Converter((TotalPaidBasicSalary * DA) / 100);

                        if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                        {
                            TotalHRA = Converter((TotalPaidBasicSalary * HRA) / 100);
                        }
                        else
                        {
                            HRAPerDay = HRA / MonthDays;
                            TotalHRA = Converter(HRAPerDay * SalaryRecoveryDays);
                        }

                        TotalTransport = Converter((Transport / MonthDays) * SalaryRecoveryDays);

                        TotalMedical = Converter((Medical / MonthDays) * SalaryRecoveryDays);

                        TotalWashing = Converter((Washing / MonthDays) * SalaryRecoveryDays);

                        GrossRevisedRecovery = Converter((TotalPaidBasicSalary + TotalDA + TotalHRA + TotalTransport + TotalMedical + TotalWashing));

                        TotalExGratia = Converter((ExGratia / MonthDays) * SalaryRecoveryDays);

                        GrossTotal = Converter((GrossRevisedRecovery + TotalExGratia + ArearAdjust));

                        GrossTotalRecoveryAfterDeduction = Converter((GrossTotal - Deduction));

                        Total = (GrossRevisedRecovery + TotalExGratia);
                        FinalSalaryPerDay = Converter((Total / SalaryRecoveryDays));

                        // Calculate Salary On Basic with Total salary Paid Days End here.

                        // Calculate Salary On Effected Basic with Total salary Paid Days start here.

                        if (ChangeScale == "Yes" && SalaryRecoveryDays > 0)
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
                            decimal EffectedGrossRevisedRecovery = 0;
                            decimal EffectedGrossTotal = 0;
                            decimal EffectedHRAPerDay = 0;

                            EffectedSalaryPerDay = (Convert.ToDecimal(EffectedScale) / MonthDays);
                            EffectedTotalPaidBasicSalary = Converter((EffectedSalaryPerDay * SalaryRecoveryDays));

                            EffectedTotalDA = Converter((EffectedTotalPaidBasicSalary * DA) / 100);

                            if (dt.Rows[i]["SelectHRA"].ToString().Contains("%"))
                            {
                                EffectedTotalHRA = Converter((EffectedTotalPaidBasicSalary * HRA) / 100);
                            }
                            else
                            {
                                EffectedHRAPerDay = HRA / MonthDays;
                                EffectedTotalHRA = Converter(EffectedHRAPerDay * SalaryRecoveryDays);
                            }

                            EffectedTotalTransport = Converter((Transport / MonthDays) * SalaryRecoveryDays);

                            EffectedTotalMedical = Converter((Medical / MonthDays) * SalaryRecoveryDays);

                            EffectedTotalWashing = Converter((Washing / MonthDays) * SalaryRecoveryDays);

                            EffectedGrossRevisedRecovery = Converter((EffectedTotalPaidBasicSalary + EffectedTotalDA + EffectedTotalHRA + EffectedTotalTransport + EffectedTotalMedical + EffectedTotalWashing));

                            EffectedTotalExGratia = Converter((ExGratia / MonthDays) * SalaryRecoveryDays);

                            EffectedGrossTotal = Converter((EffectedGrossRevisedRecovery + EffectedTotalExGratia + ArearAdjust));

                            EffectedGrossTotalRecoveryAfterDeduction = Converter((EffectedGrossTotal - Deduction));

                            EffectedTotal = (EffectedGrossRevisedRecovery + EffectedTotalExGratia);

                            decimal TotalDifference = (Total - EffectedTotal);

                            Deduction = Deduction + TotalDifference;

                            GrossTotalRecoveryAfterDeduction = Converter((GrossTotal - Deduction));

                            // Again Calculate Salary for Effected Scale users on the basis of original Scale with New Salary Paid Days End here.
                        }

                        // Calculate Salary On Effected Basic with Total salary Paid Days End here.            

                        if (GrossTotal >= Deduction)
                        {
                            lblPayDrawnBasic.Text = TotalPaidBasicSalary.ToString("0.00");
                            lblDaOnBasic.Text = TotalDA.ToString("0.00");
                            lblHraOnBasic.Text = TotalHRA.ToString("0.00");
                            lblTransportOnBasic.Text = TotalTransport.ToString("0.00");
                            lblMedicalOnBasic.Text = TotalMedical.ToString("0.00");
                            lblWashingOnBasic.Text = TotalWashing.ToString("0.00");
                            lblGrossRevisedRecovery.Text = GrossRevisedRecovery.ToString("0.00");
                            lblExGratiaOnBasic.Text = TotalExGratia.ToString("0.00");
                            lblGrossTotal.Text = GrossTotal.ToString("0.00");
                            lblDeduction.Text = Deduction.ToString("0.00");
                            lblGrossTotalRecovery.Text = GrossTotalRecoveryAfterDeduction.ToString("0.00");
                        }
                        else
                        {

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

    public void BindRecoverygrid()
    {
        try
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
            grdCalculateRecovery.DataSource = dt;
            grdCalculateRecovery.DataBind();
            if (dt.Rows.Count > 0)
            {
                pnlCalculateRecovery.Visible = true;
                CalculateDaysInMonth();
                GetImportAllData();
            }
            else
            {
                pnlCalculateRecovery.Visible = false;
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
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageFNFMaking", con);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@Type", "Deactivate");
            cmd.Parameters.AddWithValue("@Name", null);
            cmd.Parameters.AddWithValue("@Emp_Code", null);
            cmd.Parameters.AddWithValue("@DOJ", null);
            cmd.Parameters.AddWithValue("@ResignDate", null);
            cmd.Parameters.AddWithValue("@ActualLWD", null);
            cmd.Parameters.AddWithValue("@AsPerNormsLWD", null);
            cmd.Parameters.AddWithValue("@TotalWorking", null);
            cmd.Parameters.AddWithValue("@Remarks", null);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            int HasRow = cmd.ExecuteNonQuery();
            con.Close();
            if (HasRow > 0)
            {
                Cancel();
                ClearTable();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('FNF Record Deactivate Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Deactivating Record.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}