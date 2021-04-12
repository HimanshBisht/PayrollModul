using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class EmpSalaryProfile : System.Web.UI.Page
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
                    CalDOB.EndDate = DateTime.Now.Date;
                    CalDOJ.EndDate = DateTime.Now.Date;
                    CalFromDt.EndDate = DateTime.Now.Date;
                    Employee();
                    DeactivateEmployee();
                    BindDesignation();
                    Bank();
                    BindStaffType();
                    BindEmpNature();
                    BindAppointment();
                    BindSubjects();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                CalDOB.EndDate = DateTime.Now.Date;
                CalDOJ.EndDate = DateTime.Now.Date;
                CalFromDt.EndDate = DateTime.Now.Date;
                Employee();
                DeactivateEmployee();
                BindDesignation();
                Bank();
                BindStaffType();
                BindEmpNature();
                BindAppointment();
                BindSubjects();
            }
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
            ddlemployee.Items.Insert(0, new ListItem("Select Active Employee", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void DeactivateEmployee()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", Status.Deactive);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "Name ASC";
            ddlDeactivateEmployee.DataSource = dv;
            ddlDeactivateEmployee.DataTextField = "DropText";
            ddlDeactivateEmployee.DataValueField = "ProfileID";
            ddlDeactivateEmployee.DataBind();
            ddlDeactivateEmployee.Items.Insert(0, new ListItem("Select Deactive Employee", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void Bank()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageBanks", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BankCode", null);
            cmd.Parameters.AddWithValue("@BankName", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankId";
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("Select Bank", "0"));
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
            ddlDesignation.Items.Insert(0, new ListItem("Select Designation", "0"));

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
            ddlStaffType.Items.Insert(0, new ListItem("Select Staff Type", "0"));

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
            ddlNatureOfEmp.Items.Insert(0, new ListItem("Select Nature", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindAppointment()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppointmentType", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlAppointment.DataSource = dt;
            ddlAppointment.DataTextField = "AppointmentType";
            ddlAppointment.DataValueField = "AppointmentID";
            ddlAppointment.DataBind();
            ddlAppointment.Items.Insert(0, new ListItem("Select Appointment", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindSubjects()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageSubjects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubjectName", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlSubjects.DataSource = dt;
            ddlSubjects.DataTextField = "SubjectName";
            ddlSubjects.DataValueField = "SubjectID";
            ddlSubjects.DataBind();
            ddlSubjects.Items.Insert(0, new ListItem("Select Subject", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void rdoModeOfSalary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoModeOfSalary.SelectedValue == "2")
            {
                pnlBankDetails.Visible = true;
            }
            else
            {
                pnlBankDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateGrossTotal()
    {
        try
        {
            decimal BasicScale = 0;
            decimal EffectedBasicScale = 0;
            decimal DA = 0;
            decimal HRA = 0;
            decimal MedicalAllow = 0;
            decimal TransportAllow = 0;
            decimal WashingAllow = 0;
            decimal ExGratia = 0;
            decimal GrossTotal = 0;
            decimal EffectiveGrossTotal = 0;
            decimal GrossAllowance = 0;

            if (ddlChangeScale.SelectedValue == "1" && txtEffectedScale.Text.Length > 0)
            {
                EffectedBasicScale = Convert.ToDecimal(txtEffectedScale.Text);
            }

            if (lblTotalBasicScale.Text.Length > 0)
            {
                BasicScale = Convert.ToDecimal(lblTotalBasicScale.Text);
            }

            if (txtTotalDaValue.Text.Length > 0)
            {
                DA = Convert.ToDecimal(txtTotalDaValue.Text);
            }

            if (txtTotalHRAValue.Text.Length > 0)
            {
                HRA = Convert.ToDecimal(txtTotalHRAValue.Text);
            }

            if (txtTotalMedicalAllow.Text.Length > 0)
            {
                MedicalAllow = Convert.ToDecimal(txtTotalMedicalAllow.Text);
            }

            if (txtTotalTransportAllow.Text.Length > 0)
            {
                TransportAllow = Convert.ToDecimal(txtTotalTransportAllow.Text);
            }

            if (txtTotalWashingAllow.Text.Length > 0)
            {
                WashingAllow = Convert.ToDecimal(txtTotalWashingAllow.Text);
            }

            if (txtTotalExGratia.Text.Length > 0)
            {
                ExGratia = Convert.ToDecimal(txtTotalExGratia.Text);
            }

            GrossTotal = Converter(BasicScale + DA + HRA + MedicalAllow + TransportAllow + WashingAllow + ExGratia);
            EffectiveGrossTotal = Converter(EffectedBasicScale + ExGratia);
            GrossAllowance = Converter(DA + HRA + MedicalAllow + TransportAllow + WashingAllow + ExGratia);
            lblGrossTotal.Text = GrossTotal.ToString();
            lblGrossAllowance.Text = GrossAllowance.ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateGrossDeduction()
    {
        try
        {
            decimal PF = 0;
            decimal ESI = 0;
            decimal EffectedESI = 0;
            decimal GIS = 0;
            decimal TransportRecovery = 0;

            if (txtTotalPF.Text.Length > 0)
            {
                PF = Convert.ToDecimal(txtTotalPF.Text);
            }

            if (txtTotalESI.Text.Length > 0)
            {
                ESI = Convert.ToDecimal(txtTotalESI.Text);
            }

            if (txtTotalGIS.Text.Length > 0)
            {
                GIS = Convert.ToDecimal(txtTotalGIS.Text);
            }

            if (txtTotalTPTRec.Text.Length > 0)
            {
                TransportRecovery = Convert.ToDecimal(txtTotalTPTRec.Text);
            }

            decimal GrossDeductionTotal = Converter(PF + ESI + GIS + TransportRecovery);
            lblGrossDeduction.Text = GrossDeductionTotal.ToString();

            decimal EffectedGrossDeductionTotal = EffectedESI;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculateNetSalary()
    {
        try
        {
            decimal GrossTotal = 0;
            decimal GrossDeductionTotal = 0;
            decimal NetSalary = 0;

            GrossTotal = Convert.ToDecimal(lblGrossTotal.Text);
            GrossDeductionTotal = Convert.ToDecimal(lblGrossDeduction.Text);
            NetSalary = Converter(GrossTotal - GrossDeductionTotal);
            lblNetSalary.Text = NetSalary.ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            string Name = txtName.Text;
            decimal GradePay = Convert.ToDecimal(txtGradePay.Text);
            decimal BasicScale = Convert.ToDecimal(txtScale.Text);
            string DOJ = txtDOJ.Text;
            string DOB = txtDOB.Text;
            string NatureOfEmp = ddlNatureOfEmp.SelectedValue;
            string BusUser = ddlBusUser.SelectedValue;
            string DA = ddlDA.SelectedValue;
            string HRA = ddlHRA.SelectedValue;
            string Medical = ddlMedicalAllow.SelectedValue;
            string Transport = ddlTransportAllow.SelectedValue;
            string Washing = ddlWashingAllow.SelectedValue;
            string ModeOfSalary = rdoModeOfSalary.SelectedItem.Text;
            string ExGratia = ddlExGratia.SelectedValue;
            string BankName = ddlBank.SelectedItem.Text;
            string AccountNo = txtAccountNo.Text;
            string IFSCCode = txtIFSCCode.Text;
            string PF = ddlPFDeduct.SelectedValue;
            string ESI = ddlESIDeduct.SelectedValue;
            string GIS = ddlGISDeduct.SelectedValue;
            pnlData.Visible = false;
            pnlDetail.Visible = true;
            lblEmpName.Text = Name;
            lblDOB.Text = DOB;
            lblDOJ.Text = DOJ;
            lblTotalGradePay.Text = GradePay.ToString();
            lblTotalBasicScale.Text = BasicScale.ToString();
            lblModeOfSalary.Text = ModeOfSalary;

            if (rdoModeOfSalary.SelectedValue == "2")
            {
                lblBankName.Text = BankName;
                lblAccountNo.Text = AccountNo;
                lblIFSCCode.Text = IFSCCode;
                pnlAccountInfo.Visible = true;
            }
            else
            {
                lblBankName.Text = string.Empty;
                lblAccountNo.Text = string.Empty;
                lblIFSCCode.Text = string.Empty;
                pnlAccountInfo.Visible = false;
            }

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetSetUpDetails", con);
            cmd.Parameters.AddWithValue("@GradePay", GradePay);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            // Allowance Start Here.

            if (DA == "1")
            {
                pnlDAAllow.Visible = true;
                txtTotalDA.Text = ds.Tables[0].Rows[0]["DA"].ToString();
                txtTotalDaValue.Text = Converter((BasicScale * Convert.ToDecimal(txtTotalDA.Text)) / 100).ToString();
            }
            else
            {
                txtTotalDA.Text = "0";
                txtTotalDaValue.Text = "0";
                pnlDAAllow.Visible = false;
            }

            if (HRA == "1")
            {
                pnlHRAAllow.Visible = true;
                txtTotalHRA.Text = ds.Tables[1].Rows[0]["HRA"].ToString();
                txtTotalHRAValue.Text = Converter((BasicScale * Convert.ToDecimal(txtTotalHRA.Text)) / 100).ToString();
            }
            else if (HRA == "3")
            {
                pnlHRAAllow.Visible = true;
                txtTotalHRA.ReadOnly = true;
                txtTotalHRAValue.ReadOnly = false;
                if (btnSubmit.Text == "Submit")
                {
                    txtTotalHRA.Text = "0.00";
                    txtTotalHRA.ReadOnly = true;
                    txtTotalHRAValue.Text = "0.00";
                    txtTotalHRAValue.ReadOnly = false;
                }
            }
            else
            {
                txtTotalHRA.Text = "0";
                txtTotalHRAValue.Text = "0";
                pnlHRAAllow.Visible = false;
            }

            if (Medical == "1")
            {
                pnlMedicalAllow.Visible = true;
                txtTotalMedicalAllow.Text = ds.Tables[2].Rows[0]["Medical"].ToString();
            }
            else
            {
                txtTotalMedicalAllow.Text = "0";
                pnlMedicalAllow.Visible = false;
            }

            if (Transport == "1")
            {
                pnlTransportAllow.Visible = true;
                if (HRA == "1")
                {
                    if (txtGradePay.Text.Length > 0)
                    {
                        decimal DAPercent = Convert.ToDecimal(txtTotalDA.Text);
                        decimal TransportValue = Convert.ToDecimal(ds.Tables[3].Rows[0]["TransportValue"]);
                        decimal TotalTransport = (TransportValue + ((TransportValue * DAPercent) / 100));
                        txtTotalTransportAllow.Text = TotalTransport.ToString("0.00");
                    }
                    else
                    {
                        txtTotalTransportAllow.Text = "0";
                    }
                }
            }

            else
            {
                txtTotalTransportAllow.Text = "0";
                pnlTransportAllow.Visible = false;
            }

            // Get Transport Recovery here.

            if (btnSubmit.Text == "Submit")
            {
                if (BusUser == "2" || BusUser == "3")
                {
                    txtTotalTPTRec.Text = "0";
                }
                else if (BusUser == "1" && NatureOfEmp == "3")
                {
                    txtTotalTPTRec.Text = ds.Tables[6].Rows[0]["RecoveryAmount"].ToString();
                }
                else if (BusUser == "1" && (NatureOfEmp == "1" || NatureOfEmp == "2"))
                {
                    txtTotalTPTRec.Text = txtTotalTransportAllow.Text;
                }
            }

            if (Washing == "1")
            {
                pnlWashingAllow.Visible = true;
                if (btnSubmit.Text == "Submit")
                {
                    txtTotalWashingAllow.Text = "0";
                }
            }
            else
            {
                txtTotalWashingAllow.Text = "0";
                pnlWashingAllow.Visible = false;

            }

            if (ExGratia == "1")
            {
                pnlExGratiaAllow.Visible = true;
                if (btnSubmit.Text == "Submit")
                {
                    txtTotalExGratia.Text = "0";
                }
            }
            else
            {
                txtTotalExGratia.Text = "0";
                pnlExGratiaAllow.Visible = false;
            }

            // Allowance End Here.

            // Calculate Gross Total Start Here.

            CalculateGrossTotal();

            // Calculate Gross Total End Here.

            // Deductions Start Here.

            if (GIS == "1")
            {
                pnlGIS.Visible = true;
                txtTotalGIS.Text = ddlSelectGIS.SelectedValue;
            }
            else
            {
                txtTotalGIS.Text = "0";
                pnlGIS.Visible = false;
            }

            CalculateESI();
            CalculatePF();
            CalculateGrossDeduction();
            CalculateNetSalary();

            // Deductions End Here.

            // Display Reimbursement Start Here.

            DisplayReimbursementAndCalculateTotalReimbursement();

            // Display Reimbursement End Here.

            if (btnSubmit.Text == "Submit")
            {
                btnDeactivate.Visible = false;
            }
            else
            {
                btnDeactivate.Visible = true;
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

    public void DisplayReimbursementAndCalculateTotalReimbursement()
    {
        try
        {
            if (rdoreimbursement.SelectedValue == "1")
            {
                pnlShowReimbursement.Visible = true;

                string ReimbursementFor1 = txtReimbursementFor1.Text;
                decimal ReimbursementValue1 = 0;

                string ReimbursementFor2 = txtReimbursementFor2.Text;
                decimal ReimbursementValue2 = 0;

                string ReimbursementFor3 = txtReimbursementFor3.Text;
                decimal ReimbursementValue3 = 0;

                string ReimbursementFor4 = txtReimbursementFor4.Text;
                decimal ReimbursementValue4 = 0;

                string ReimbursementFor5 = txtReimbursementFor5.Text;
                decimal ReimbursementValue5 = 0;

                if (txtReimbursementFor1.Text.Length > 0 && txtReimbursementValue1.Text.Length > 0)
                {
                    pnlReimbursement1.Visible = true;
                    lblTotalReimbursement1.Text = ReimbursementFor1;
                    ReimbursementValue1 = Convert.ToDecimal(txtReimbursementValue1.Text);
                    lblTotalReimbursementValue1.Text = ReimbursementValue1.ToString();
                }
                else
                {
                    pnlReimbursement1.Visible = false;
                    lblTotalReimbursement1.Text = string.Empty;
                    lblTotalReimbursementValue1.Text = "0";
                }

                if (txtReimbursementFor2.Text.Length > 0 && txtReimbursementValue2.Text.Length > 0)
                {
                    pnlReimbursement2.Visible = true;
                    lblTotalReimbursement2.Text = ReimbursementFor2;
                    ReimbursementValue2 = Convert.ToDecimal(txtReimbursementValue2.Text);
                    lblTotalReimbursementValue2.Text = ReimbursementValue2.ToString();
                }
                else
                {
                    pnlReimbursement2.Visible = false;
                    lblTotalReimbursement2.Text = string.Empty;
                    lblTotalReimbursementValue2.Text = "0";
                }

                if (txtReimbursementFor3.Text.Length > 0 && txtReimbursementValue3.Text.Length > 0)
                {
                    pnlReimbursement3.Visible = true;
                    lblTotalReimbursement3.Text = ReimbursementFor3;
                    ReimbursementValue3 = Convert.ToDecimal(txtReimbursementValue3.Text);
                    lblTotalReimbursementValue3.Text = ReimbursementValue3.ToString();
                }
                else
                {
                    pnlReimbursement3.Visible = false;
                    lblTotalReimbursement3.Text = string.Empty;
                    lblTotalReimbursementValue3.Text = "0";
                }

                if (txtReimbursementFor4.Text.Length > 0 && txtReimbursementValue4.Text.Length > 0)
                {
                    pnlReimbursement4.Visible = true;
                    lblTotalReimbursement4.Text = ReimbursementFor4;
                    ReimbursementValue4 = Convert.ToDecimal(txtReimbursementValue4.Text);
                    lblTotalReimbursementValue4.Text = ReimbursementValue4.ToString();
                }
                else
                {
                    pnlReimbursement4.Visible = false;
                    lblTotalReimbursement4.Text = string.Empty;
                    lblTotalReimbursementValue4.Text = "0";
                }

                if (txtReimbursementFor5.Text.Length > 0 && txtReimbursementValue5.Text.Length > 0)
                {
                    pnlReimbursement5.Visible = true;
                    lblTotalReimbursement5.Text = ReimbursementFor5;
                    ReimbursementValue5 = Convert.ToDecimal(txtReimbursementValue5.Text);
                    lblTotalReimbursementValue5.Text = ReimbursementValue5.ToString();
                }
                else
                {
                    pnlReimbursement5.Visible = false;
                    lblTotalReimbursement5.Text = string.Empty;
                    lblTotalReimbursementValue5.Text = "0";
                }

                decimal TotalReimbursement = 0;
                TotalReimbursement = ReimbursementValue1 + ReimbursementValue2 + ReimbursementValue3 + ReimbursementValue4 + ReimbursementValue5;
                lblGrossReimbursement.Text = TotalReimbursement.ToString();
            }
            else
            {
                lblTotalReimbursement1.Text = string.Empty;
                lblTotalReimbursementValue1.Text = "0";
                pnlReimbursement1.Visible = false;
                lblTotalReimbursement2.Text = string.Empty;
                lblTotalReimbursementValue2.Text = "0";
                pnlReimbursement2.Visible = false;
                lblTotalReimbursement3.Text = string.Empty;
                lblTotalReimbursementValue3.Text = "0";
                pnlReimbursement3.Visible = false;
                lblTotalReimbursement4.Text = string.Empty;
                lblTotalReimbursementValue4.Text = "0";
                pnlReimbursement4.Visible = false;
                lblTotalReimbursement5.Text = string.Empty;
                lblTotalReimbursementValue5.Text = "0";
                pnlReimbursement5.Visible = false;
                pnlShowReimbursement.Visible = false;
                lblGrossReimbursement.Text = "0";
            }
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
            decimal GradePay = Convert.ToDecimal(txtGradePay.Text);
            decimal BasicScale = Convert.ToDecimal(txtScale.Text);
            decimal EffectedBasicScale = 0;
            string ESI = ddlESIDeduct.SelectedValue;

            if (ddlChangeScale.SelectedValue == "1" && txtEffectedScale.Text.Length > 0)
            {
                EffectedBasicScale = Convert.ToDecimal(txtEffectedScale.Text);
            }

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetSetUpDetails", con);
            cmd.Parameters.AddWithValue("@GradePay", GradePay);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            decimal GetGrossTotal = Convert.ToDecimal(lblGrossTotal.Text);
            decimal GetESIMaxRange = Convert.ToDecimal(ds.Tables[4].Rows[0]["MaxRange"].ToString());
            decimal GetESIValue = Convert.ToDecimal(ds.Tables[4].Rows[0]["ESI"].ToString());
            decimal AssignESIValue = 0;

            if (ESI == "1" && GetGrossTotal <= GetESIMaxRange)
            {
                pnlESI.Visible = true;
                AssignESIValue = Math.Ceiling((GetGrossTotal * GetESIValue) / 100);
                txtTotalESI.Text = AssignESIValue.ToString();
            }

            else if (ESI == "1" && GetGrossTotal > GetESIMaxRange)
            {
                pnlESI.Visible = true;
                AssignESIValue = Math.Ceiling((GetGrossTotal * GetESIValue) / 100);
                txtTotalESI.Text = AssignESIValue.ToString();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('GrossTotal " + GetGrossTotal + " is greater than ESI Value " + GetESIMaxRange + " , So According to our Protocol ESI is not Applicable for this Employee, If want to change, please go back and set ESI=NO , ELSE Continue.');", true);
            }

            else if (ESI == "2" && GetGrossTotal <= GetESIMaxRange)
            {
                txtTotalESI.Text = "0";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('GrossTotal " + GetGrossTotal + " is less than ESI Value " + GetESIMaxRange + " , So According to our Protocol ESI is Applicable for this Employee, If want to change, please go back and set ESI=YES , ELSE Continue.');", true);
            }

            else
            {
                txtTotalESI.Text = "0";
                pnlESI.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void CalculatePF()
    {
        try
        {
            decimal GradePay = Convert.ToDecimal(txtGradePay.Text);
            decimal BasicScale = Convert.ToDecimal(txtScale.Text);
            decimal DAValue = 0;
            string PF = ddlPFDeduct.SelectedValue;
            decimal AssignPFValue = 0;
            decimal GetPFMaxRange = 0;
            decimal GetPFValue = 0;

            if (txtTotalDaValue.Text.Length > 0)
            {
                DAValue = Convert.ToDecimal(txtTotalDaValue.Text);
            }
            else
            {
                DAValue = 0;
            }
            decimal GetBasicPlusDAForPF = BasicScale + DAValue;

            if (PF == "1" || PF == "3")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", GradePay);
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
                        pnlPF.Visible = true;
                        AssignPFValue = Converter((GetPFMaxRange * GetPFValue) / 100);
                        txtTotalPF.Text = AssignPFValue.ToString();
                    }
                    else if (GetBasicPlusDAForPF < GetPFMaxRange)
                    {
                        pnlPF.Visible = true;
                        AssignPFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                        txtTotalPF.Text = AssignPFValue.ToString();
                    }
                }
                else if (PF == "3")
                {
                    pnlPF.Visible = true;
                    AssignPFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                    txtTotalPF.Text = AssignPFValue.ToString();
                }
            }
            else
            {
                txtTotalPF.Text = AssignPFValue.ToString();
                pnlPF.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            ClearPnlDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalDA_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtTotalDaValue.Text = Convert.ToString((Convert.ToDecimal(lblTotalBasicScale.Text) * Convert.ToDecimal(txtTotalDA.Text)) / 100);
            CalculateGrossTotal();
            CalculatePF();
            CalculateESI();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalHRA_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtTotalHRAValue.Text = Convert.ToString((Convert.ToDecimal(lblTotalBasicScale.Text) * Convert.ToDecimal(txtTotalHRA.Text)) / 100);
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalMedicalAllow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalTransportAllow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalWashingAllow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalExGratia_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void rdoMaternityLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoMaternityLeave.SelectedValue == "1")
            {
                pnlMaternityLeave.Visible = true;
            }
            else
            {
                txtFromMaternity.Text = string.Empty;
                txtToMaternity.Text = string.Empty;
                pnlMaternityLeave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearPnlData()
    {
        try
        {
            pnlData.Visible = true;
            pnlDetail.Visible = false;
            ddlemployee.ClearSelection();
            txtName.Text = string.Empty;
            txtEmployeeID.Text = string.Empty;
            txtEmp_Code.Text = string.Empty;
            txtSystemNumber.Text = string.Empty;
            txtEmpCode.Text = string.Empty;
            //txtDOB.Text = string.Empty;
            txtEmpAge.Text = string.Empty;
            txtDOJ.Text = string.Empty;
            ddlDesignation.ClearSelection();
            ddlSubjects.ClearSelection();
            pnlSubjectFor.Visible = false;
            ddlStaffType.ClearSelection();
            ddlNatureOfEmp.ClearSelection();
            txtFromDt.Text = string.Empty;
            txtToDate.Text = string.Empty;
            ddlAppointment.ClearSelection();
            txtGradePay.Text = string.Empty;
            txtScale.Text = string.Empty;
            txtPfNo.Text = string.Empty;
            txtEsiNo.Text = string.Empty;
            txtConfirmationDate.Text = string.Empty;
            txtProbationDate.Text = string.Empty;
            txtResignDate.Text = string.Empty;
            ddlChangeScale.ClearSelection();
            txtEffectedScale.Text = string.Empty;
            txtPANCardNo.Text = string.Empty;
            txtAadharCardNo.Text = string.Empty;
            pnlEffectedScale.Visible = false;
            ddlBusUser.ClearSelection();
            ddlGovAcc.ClearSelection();
            ddlDA.ClearSelection();
            ddlHRA.ClearSelection();
            ddlTransportAllow.ClearSelection();
            ddlMedicalAllow.ClearSelection();
            //ddlWashingAllow.ClearSelection();
            ddlExGratia.ClearSelection();
            ddlESIDeduct.ClearSelection();
            ddlPFDeduct.ClearSelection();
            //ddlGISDeduct.ClearSelection();
            //ddlSelectGIS.ClearSelection();
            //pnlSelectGIS.Visible = false;
            rdoModeOfSalary.SelectedValue = "1";
            ddlBank.ClearSelection();
            txtAccountNo.Text = string.Empty;
            txtIFSCCode.Text = string.Empty;
            txtAddress.Text = string.Empty;
            pnlBankDetails.Visible = false;
            rdoMaternityLeave.SelectedValue = "2";
            txtFromMaternity.Text = string.Empty;
            txtToMaternity.Text = string.Empty;
            pnlMaternityLeave.Visible = false;
            rdoreimbursement.SelectedValue = "2";
            txtReimbursementFor1.Text = string.Empty;
            txtReimbursementValue1.Text = string.Empty;
            txtReimbursementFor2.Text = string.Empty;
            txtReimbursementValue2.Text = string.Empty;
            txtReimbursementFor3.Text = string.Empty;
            txtReimbursementValue3.Text = string.Empty;
            txtReimbursementFor4.Text = string.Empty;
            txtReimbursementValue4.Text = string.Empty;
            txtReimbursementFor5.Text = string.Empty;
            txtReimbursementValue5.Text = string.Empty;
            pnlReimbursement.Visible = false;
            ViewState["ProfileID"] = null;
            btnSubmit.Text = "Submit";
            rfvPFNo.ValidationGroup = "Cancel";
            rfvESINo.ValidationGroup = "Cancel";
            txtFatherORHusbandName.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtMobileNo.Text = string.Empty;
            ddlGender.ClearSelection();
            txtContractDate.Text = string.Empty;
            txtLWD.Text = string.Empty;
            txtHomeAddress.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            ddlTransferType.ClearSelection();
            txtUANNo.Text = string.Empty;
            txtDOB.Text = string.Empty;
            PnlActivateButtons.Visible = false;
            ddlemployee.Enabled = true;
            pnlUpdateButtons.Visible = true;
            ddlDeactivateEmployee.ClearSelection();
            ddlDeactivateEmployee.Enabled = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearPnlDetail()
    {
        try
        {
            pnlData.Visible = true;
            pnlDetail.Visible = false;
            if (btnSubmit.Text == "Submit")
            {
                lblEmpName.Text = string.Empty;
                lblDOB.Text = string.Empty;
                lblDOJ.Text = string.Empty;
                lblTotalGradePay.Text = string.Empty;
                lblTotalBasicScale.Text = string.Empty;
                lblModeOfSalary.Text = string.Empty;
                lblBankName.Text = string.Empty;
                lblAccountNo.Text = string.Empty;
                lblIFSCCode.Text = string.Empty;
                pnlAccountInfo.Visible = false;
                txtTotalDA.Text = string.Empty;
                txtTotalDaValue.Text = string.Empty;
                pnlDAAllow.Visible = false;
                txtTotalHRA.Text = string.Empty;
                txtTotalHRAValue.Text = string.Empty;
                pnlHRAAllow.Visible = false;
                txtTotalHRA.ReadOnly = false;
                txtTotalHRAValue.ReadOnly = true;
                txtTotalMedicalAllow.Text = string.Empty;
                pnlMedicalAllow.Visible = false;
                txtTotalTransportAllow.Text = string.Empty;
                pnlTransportAllow.Visible = false;
                txtTotalWashingAllow.Text = string.Empty;
                pnlWashingAllow.Visible = false;
                txtTotalExGratia.Text = string.Empty;
                pnlExGratiaAllow.Visible = false;
                lblGrossAllowance.Text = "0";
                lblGrossTotal.Text = "0";
                txtTotalPF.Text = string.Empty;
                pnlPF.Visible = false;
                txtTotalESI.Text = string.Empty;
                pnlESI.Visible = false;
                txtTotalGIS.Text = string.Empty;
                pnlGIS.Visible = false;
                txtTotalTPTRec.Text = string.Empty;
                lblGrossDeduction.Text = "0";
                lblNetSalary.Text = "0";
                lblTotalReimbursement1.Text = string.Empty;
                lblTotalReimbursementValue1.Text = string.Empty;
                pnlReimbursement1.Visible = false;
                lblTotalReimbursement2.Text = string.Empty;
                lblTotalReimbursementValue2.Text = string.Empty;
                pnlReimbursement2.Visible = false;
                lblTotalReimbursement3.Text = string.Empty;
                lblTotalReimbursementValue3.Text = string.Empty;
                pnlReimbursement3.Visible = false;
                lblTotalReimbursement4.Text = string.Empty;
                lblTotalReimbursementValue4.Text = string.Empty;
                pnlReimbursement4.Visible = false;
                lblTotalReimbursement5.Text = string.Empty;
                lblTotalReimbursementValue5.Text = string.Empty;
                lblGrossReimbursement.Text = "0";
                pnlReimbursement5.Visible = false;
                pnlShowReimbursement.Visible = false;
                ddlSalaryStatus.ClearSelection();
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
            ClearPnlData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtDOB_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDOB.Text.Length > 0)
            {
                DateTime dob = Convert.ToDateTime(txtDOB.Text);
                string text = CalculateYourAge(dob);
                txtEmpAge.Text = text;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
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

    protected void txtFromDt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtFromDt.Text.Length > 0)
            {
                txtToDate.Text = string.Empty;
                CalToDt.StartDate = Convert.ToDateTime(txtFromDt.Text);
            }
            else
            {
                txtToDate.Text = string.Empty;
                CalToDt.StartDate = DateTime.Now.Date;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtFromMaternity_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtFromMaternity.Text.Length > 0)
            {
                txtToMaternity.Text = string.Empty;
                CalToMaternity.StartDate = Convert.ToDateTime(txtFromMaternity.Text);
            }
            else
            {
                txtToMaternity.Text = string.Empty;
                CalToMaternity.StartDate = DateTime.Now.Date;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlGISDeduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGISDeduct.SelectedValue == "1")
            {
                pnlSelectGIS.Visible = true;
                BindGIS();
            }
            else
            {
                pnlSelectGIS.Visible = false;
                ddlSelectGIS.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindGIS()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowGISSetUp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlSelectGIS.DataSource = dt;
            ddlSelectGIS.DataTextField = "GISName";
            ddlSelectGIS.DataValueField = "GISValue";
            ddlSelectGIS.DataBind();
            ddlSelectGIS.Items.Insert(0, new ListItem("Select GIS", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void rdoreimbursement_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoreimbursement.SelectedValue == "1")
            {
                pnlReimbursement.Visible = true;
            }
            else
            {
                txtReimbursementFor1.Text = string.Empty;
                txtReimbursementValue1.Text = string.Empty;
                txtReimbursementFor2.Text = string.Empty;
                txtReimbursementValue2.Text = string.Empty;
                txtReimbursementFor3.Text = string.Empty;
                txtReimbursementValue3.Text = string.Empty;
                txtReimbursementFor4.Text = string.Empty;
                txtReimbursementValue4.Text = string.Empty;
                txtReimbursementFor5.Text = string.Empty;
                txtReimbursementValue5.Text = string.Empty;
                pnlReimbursement.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalPF_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossDeduction();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalESI_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossDeduction();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalGIS_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossDeduction();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlPFDeduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPFDeduct.SelectedValue == "1" || ddlPFDeduct.SelectedValue == "3")
            {
                rfvPFNo.ValidationGroup = "Save";
                rfvUANNo.ValidationGroup = "Save";
            }
            else
            {
                rfvPFNo.ValidationGroup = string.Empty;
                rfvUANNo.ValidationGroup = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlESIDeduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlESIDeduct.SelectedValue == "1")
            {
                rfvESINo.ValidationGroup = "Save";
            }
            else
            {
                rfvESINo.ValidationGroup = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlChangeScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlChangeScale.SelectedValue == "1")
            {
                pnlEffectedScale.Visible = true;
            }
            else
            {
                pnlEffectedScale.Visible = false;
                txtEffectedScale.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("SaveEmpSalaryProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
            cmd.Parameters.AddWithValue("@Emp_Code", txtEmp_Code.Text);
            cmd.Parameters.AddWithValue("@SystemNumber", txtSystemNumber.Text);
            cmd.Parameters.AddWithValue("@AssignEmpCode", txtEmpCode.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            if (txtDOB.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@DOB", txtDOB.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DOB", null);
            }

            if (txtDOJ.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@DOJ", txtDOJ.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DOJ", null);
            }

            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@SubjectID", ddlSubjects.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@NatureOfEmp", ddlNatureOfEmp.SelectedValue);

            if (txtFromDt.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@FromDate", txtFromDt.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FromDate", null);
            }

            if (txtToDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToDate", null);
            }
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@GradePay", txtGradePay.Text);
            cmd.Parameters.AddWithValue("@BasicScale", txtScale.Text);
            if (txtPfNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@PFNo", txtPfNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PFNo", null);
            }

            if (txtEsiNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ESINo", txtEsiNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ESINo", null);
            }

            if (txtProbationDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ProbationDate", txtProbationDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProbationDate", null);
            }

            if (txtConfirmationDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ConfirmationDate", txtConfirmationDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ConfirmationDate", null);
            }

            if (txtResignDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ResignationDate", txtResignDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ResignationDate", null);
            }

            cmd.Parameters.AddWithValue("@ChangeScale", ddlChangeScale.SelectedValue);
            if (ddlChangeScale.SelectedValue == "1" && txtEffectedScale.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@EffectedScale", txtEffectedScale.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EffectedScale", 0);
            }
            cmd.Parameters.AddWithValue("@PanCardNo", txtPANCardNo.Text);
            cmd.Parameters.AddWithValue("@AadharCardNo", txtAadharCardNo.Text);
            cmd.Parameters.AddWithValue("@UANNo", txtUANNo.Text);
            cmd.Parameters.AddWithValue("@BusUser", ddlBusUser.SelectedValue);
            cmd.Parameters.AddWithValue("@GovAcc", ddlGovAcc.SelectedValue);
            cmd.Parameters.AddWithValue("@DaAllow", ddlDA.SelectedValue);
            cmd.Parameters.AddWithValue("@DA", txtTotalDA.Text);
            cmd.Parameters.AddWithValue("@DaValue", txtTotalDaValue.Text);
            cmd.Parameters.AddWithValue("@HraAllow", ddlHRA.SelectedValue);
            cmd.Parameters.AddWithValue("@HRA", txtTotalHRA.Text);
            cmd.Parameters.AddWithValue("@HraValue", txtTotalHRAValue.Text);
            cmd.Parameters.AddWithValue("@TransportAllow", ddlTransportAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@TransportValue", txtTotalTransportAllow.Text);
            cmd.Parameters.AddWithValue("@MedicalAllow", ddlMedicalAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@MedicalValue", txtTotalMedicalAllow.Text);
            cmd.Parameters.AddWithValue("@WashingAllow", ddlWashingAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@WashingValue", txtTotalWashingAllow.Text);
            cmd.Parameters.AddWithValue("@ExGratiaAllow", ddlExGratia.SelectedValue);
            cmd.Parameters.AddWithValue("@ExGratiaValue", txtTotalExGratia.Text);
            cmd.Parameters.AddWithValue("@PFDeduct", ddlPFDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@PFValue", txtTotalPF.Text);
            cmd.Parameters.AddWithValue("@ESIDeduct", ddlESIDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@EsiValue", txtTotalESI.Text);
            cmd.Parameters.AddWithValue("@GISDeduct", ddlGISDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@GisValue", txtTotalGIS.Text);
            cmd.Parameters.AddWithValue("@TransportRecovery", txtTotalTPTRec.Text);
            cmd.Parameters.AddWithValue("@ModeOfSalary", rdoModeOfSalary.SelectedValue);
            cmd.Parameters.AddWithValue("@BankID", ddlBank.SelectedValue);

            if (txtAccountNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@AccountNo", txtAccountNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AccountNo", null);
            }

            if (txtIFSCCode.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@IFSCCODE", txtIFSCCode.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@IFSCCODE", null);
            }

            if (txtAddress.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@BranchAddress", txtAddress.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchAddress", null);
            }

            cmd.Parameters.AddWithValue("@MaternityLeave", rdoMaternityLeave.SelectedValue);

            if (txtFromMaternity.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@FromMaternity", txtFromMaternity.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FromMaternity", null);
            }

            if (txtToMaternity.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ToMaternity", txtToMaternity.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToMaternity", null);
            }

            cmd.Parameters.AddWithValue("@Reimbursement", rdoreimbursement.SelectedValue);
            if (lblTotalReimbursement1.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor1", lblTotalReimbursement1.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor1", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue1", lblTotalReimbursementValue1.Text);

            if (lblTotalReimbursement2.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor2", lblTotalReimbursement2.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor2", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue2", lblTotalReimbursementValue2.Text);

            if (lblTotalReimbursement3.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor3", lblTotalReimbursement3.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor3", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue3", lblTotalReimbursementValue3.Text);

            if (lblTotalReimbursement4.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor4", lblTotalReimbursement4.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor4", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue4", lblTotalReimbursementValue4.Text);

            if (lblTotalReimbursement5.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor5", lblTotalReimbursement5.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor5", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue5", lblTotalReimbursementValue5.Text);
            cmd.Parameters.AddWithValue("@TotalReimbursement", lblGrossReimbursement.Text);
            cmd.Parameters.AddWithValue("@GrossAllowance", lblGrossAllowance.Text);
            cmd.Parameters.AddWithValue("@GrossTotal", lblGrossTotal.Text);
            cmd.Parameters.AddWithValue("@GrossDeduction", lblGrossDeduction.Text);
            cmd.Parameters.AddWithValue("@NetSalary", lblNetSalary.Text);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@SalaryStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@FatherORHusband", txtFatherORHusbandName.Text);
            cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
            cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);

            if (txtContractDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ContractDate", txtContractDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ContractDate", null);
            }

            if (txtLWD.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@LWD", txtLWD.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@LWD", null);
            }

            cmd.Parameters.AddWithValue("@Address", txtHomeAddress.Text);
            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
            cmd.Parameters.AddWithValue("@BankType", ddlTransferType.SelectedValue);

            if (btnSubmit.Text == "Submit")
            {
                cmd.Parameters.AddWithValue("@ProfileID", 0);
                cmd.Parameters.AddWithValue("@Type", "Save");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", "Update");
            }
            con.Open();
            int Count = (int)cmd.ExecuteScalar();
            con.Close();
            if (Count == 0)
            {
                ClearPnlData();
                ClearPnlDetail();
                Employee();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
            }

            else if (Count == 1)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Already Exist for this (Emp_Code or AssignEmpCode), These all Fields are Must be Unique, Please Try Again.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
            }
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
                    ddlDeactivateEmployee.ClearSelection();
                    ddlDeactivateEmployee.Enabled = false;
                    txtEmployeeID.Text = dt.Rows[0]["EmployeeID"].ToString();
                    txtEmp_Code.Text = dt.Rows[0]["Emp_Code"].ToString();
                    txtSystemNumber.Text = dt.Rows[0]["SystemNumber"].ToString();
                    txtEmpCode.Text = dt.Rows[0]["AssignEmpCode"].ToString();
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtDOB.Text = dt.Rows[0]["DOB"].ToString();
                    txtDOB_TextChanged(sender, e);
                    txtDOJ.Text = dt.Rows[0]["DOJ"].ToString();
                    ddlDesignation.SelectedValue = dt.Rows[0]["Designation"].ToString();
                    ddlStaffType.SelectedValue = dt.Rows[0]["StaffType"].ToString();
                    if (ddlStaffType.SelectedValue == "1")
                    {
                        pnlSubjectFor.Visible = true;
                    }
                    else
                    {
                        pnlSubjectFor.Visible = false;
                    }
                    ddlSubjects.SelectedValue = dt.Rows[0]["SubjectID"].ToString();
                    ddlNatureOfEmp.SelectedValue = dt.Rows[0]["NatureOfEmp"].ToString();
                    txtFromDt.Text = dt.Rows[0]["FromDate"].ToString();
                    txtToDate.Text = dt.Rows[0]["Todate"].ToString();
                    ddlAppointment.SelectedValue = dt.Rows[0]["Appointment"].ToString();
                    txtGradePay.Text = dt.Rows[0]["GradePay"].ToString();
                    txtScale.Text = dt.Rows[0]["BasicScale"].ToString();
                    txtPfNo.Text = dt.Rows[0]["PFNo"].ToString();
                    txtEsiNo.Text = dt.Rows[0]["ESINo"].ToString();
                    txtConfirmationDate.Text = dt.Rows[0]["ConfirmationDate"].ToString();
                    txtProbationDate.Text = dt.Rows[0]["ProbationDate"].ToString();
                    txtResignDate.Text = dt.Rows[0]["ResignationDate"].ToString();
                    ddlChangeScale.SelectedValue = dt.Rows[0]["ChangeScale"].ToString();
                    if (ddlChangeScale.SelectedValue == "1")
                    {
                        pnlEffectedScale.Visible = true;
                    }
                    else
                    {
                        pnlEffectedScale.Visible = false;
                    }
                    txtEffectedScale.Text = dt.Rows[0]["EffectedScale"].ToString();
                    txtPANCardNo.Text = dt.Rows[0]["PanCardNo"].ToString();
                    txtAadharCardNo.Text = dt.Rows[0]["AadharCardNo"].ToString();
                    txtUANNo.Text = dt.Rows[0]["UANNo"].ToString();
                    txtFatherORHusbandName.Text = dt.Rows[0]["Father/Husband"].ToString();
                    txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                    if (dt.Rows[0]["Gender"].ToString().Length > 0)
                    {
                        ddlGender.SelectedValue = dt.Rows[0]["Gender"].ToString();
                    }
                    else
                    {
                        ddlGender.SelectedValue = "0";
                    }
                    txtContractDate.Text = dt.Rows[0]["ContractDate"].ToString();
                    txtLWD.Text = dt.Rows[0]["LWD"].ToString();
                    txtHomeAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                    if (dt.Rows[0]["BankType"].ToString().Length > 0)
                    {
                        ddlTransferType.Text = dt.Rows[0]["BankType"].ToString();
                    }
                    else
                    {
                        ddlTransferType.Text = "0";
                    }

                    ddlBusUser.SelectedValue = dt.Rows[0]["BusUser"].ToString();
                    ddlGovAcc.SelectedValue = dt.Rows[0]["GovAcc"].ToString();
                    ddlDA.SelectedValue = dt.Rows[0]["DaAllow"].ToString();
                    if (ddlDA.SelectedValue == "1")
                    {
                        txtTotalDA.Text = dt.Rows[0]["DA"].ToString();
                    }
                    else
                    {
                        txtTotalDA.Text = "0";
                    }
                    ddlHRA.SelectedValue = dt.Rows[0]["HraAllow"].ToString();
                    if (ddlHRA.SelectedValue == "1" || ddlHRA.SelectedValue == "3")
                    {
                        txtTotalHRA.Text = dt.Rows[0]["HRA"].ToString();
                        txtTotalHRAValue.Text = dt.Rows[0]["HraValue"].ToString();
                    }
                    else
                    {
                        txtTotalHRA.Text = "0";
                    }
                    ddlTransportAllow.SelectedValue = dt.Rows[0]["TransportAllow"].ToString();
                    if (ddlTransportAllow.SelectedValue == "1")
                    {
                        txtTotalTransportAllow.Text = dt.Rows[0]["TransportValue"].ToString();
                    }
                    else
                    {
                        txtTotalTransportAllow.Text = "0";
                    }
                    ddlMedicalAllow.SelectedValue = dt.Rows[0]["MedicalAllow"].ToString();
                    if (ddlMedicalAllow.SelectedValue == "1")
                    {
                        txtTotalMedicalAllow.Text = dt.Rows[0]["MedicalValue"].ToString();
                    }
                    else
                    {
                        txtTotalMedicalAllow.Text = "0";
                    }
                    ddlWashingAllow.SelectedValue = dt.Rows[0]["WashingAllow"].ToString();
                    if (ddlWashingAllow.SelectedValue == "1")
                    {
                        txtTotalWashingAllow.Text = dt.Rows[0]["WashingValue"].ToString();
                    }
                    else
                    {
                        txtTotalWashingAllow.Text = "0";
                    }
                    ddlExGratia.SelectedValue = dt.Rows[0]["ExGratiaAllow"].ToString();
                    if (ddlExGratia.SelectedValue == "1")
                    {
                        txtTotalExGratia.Text = dt.Rows[0]["ExGratiaValue"].ToString();
                    }
                    else
                    {
                        txtTotalExGratia.Text = "0";
                    }
                    ddlPFDeduct.SelectedValue = dt.Rows[0]["PFDeduct"].ToString();
                    txtTotalPF.Text = dt.Rows[0]["PFValue"].ToString();
                    if (ddlPFDeduct.SelectedValue == "1")
                    {
                        rfvPFNo.ValidationGroup = "Save";
                    }
                    else
                    {
                        rfvPFNo.ValidationGroup = "Cancel";
                    }
                    ddlESIDeduct.SelectedValue = dt.Rows[0]["ESIDeduct"].ToString();
                    txtTotalESI.Text = dt.Rows[0]["ESIValue"].ToString();
                    if (ddlESIDeduct.SelectedValue == "1")
                    {
                        rfvESINo.ValidationGroup = "Save";
                    }
                    else
                    {
                        rfvESINo.ValidationGroup = "Cancel";
                    }
                    ddlGISDeduct.SelectedValue = dt.Rows[0]["GISDeduct"].ToString();
                    txtTotalGIS.Text = dt.Rows[0]["GISValue"].ToString();
                    if (ddlGISDeduct.SelectedValue == "1")
                    {
                        pnlSelectGIS.Visible = true;
                        BindGIS();
                        ddlSelectGIS.SelectedValue = dt.Rows[0]["GISValue"].ToString();
                    }
                    else
                    {
                        pnlSelectGIS.Visible = false;
                        ddlSelectGIS.ClearSelection();
                    }
                    txtTotalTPTRec.Text = dt.Rows[0]["TransportRecovery"].ToString();
                    rdoModeOfSalary.SelectedValue = dt.Rows[0]["ModeOfSalary"].ToString();
                    if (rdoModeOfSalary.SelectedValue == "2")
                    {
                        pnlBankDetails.Visible = true;
                        ddlBank.SelectedValue = dt.Rows[0]["BankID"].ToString();
                        txtAccountNo.Text = dt.Rows[0]["AccountNo"].ToString();
                        txtIFSCCode.Text = dt.Rows[0]["IFSCCODE"].ToString();
                        txtAddress.Text = dt.Rows[0]["BranchAddress"].ToString();
                    }
                    else
                    {
                        pnlBankDetails.Visible = false;
                        ddlBank.ClearSelection();
                        txtAccountNo.Text = string.Empty;
                        txtIFSCCode.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                    }
                    rdoMaternityLeave.SelectedValue = dt.Rows[0]["MaternityLeave"].ToString();
                    if (rdoMaternityLeave.SelectedValue == "1")
                    {
                        pnlMaternityLeave.Visible = true;
                        txtFromMaternity.Text = dt.Rows[0]["FromMaternity"].ToString();
                        txtToMaternity.Text = dt.Rows[0]["ToMaternity"].ToString();
                    }
                    else
                    {
                        pnlMaternityLeave.Visible = false;
                        txtFromMaternity.Text = string.Empty;
                        txtToMaternity.Text = string.Empty;
                    }
                    rdoreimbursement.SelectedValue = dt.Rows[0]["Reimbursement"].ToString();
                    if (rdoreimbursement.SelectedValue == "1")
                    {
                        pnlReimbursement.Visible = true;
                        txtReimbursementFor1.Text = dt.Rows[0]["ReimbursementFor1"].ToString();
                        txtReimbursementValue1.Text = dt.Rows[0]["ReimbursementValue1"].ToString();
                        txtReimbursementFor2.Text = dt.Rows[0]["ReimbursementFor2"].ToString();
                        txtReimbursementValue2.Text = dt.Rows[0]["ReimbursementValue2"].ToString();
                        txtReimbursementFor3.Text = dt.Rows[0]["ReimbursementFor3"].ToString();
                        txtReimbursementValue3.Text = dt.Rows[0]["ReimbursementValue3"].ToString();
                        txtReimbursementFor4.Text = dt.Rows[0]["ReimbursementFor4"].ToString();
                        txtReimbursementValue4.Text = dt.Rows[0]["ReimbursementValue4"].ToString();
                        txtReimbursementFor5.Text = dt.Rows[0]["ReimbursementFor5"].ToString();
                        txtReimbursementValue5.Text = dt.Rows[0]["ReimbursementValue5"].ToString();
                    }
                    else
                    {
                        pnlReimbursement.Visible = false;
                        txtReimbursementFor1.Text = string.Empty;
                        txtReimbursementValue1.Text = string.Empty;
                        txtReimbursementFor2.Text = string.Empty;
                        txtReimbursementValue2.Text = string.Empty;
                        txtReimbursementFor3.Text = string.Empty;
                        txtReimbursementValue3.Text = string.Empty;
                        txtReimbursementFor4.Text = string.Empty;
                        txtReimbursementValue4.Text = string.Empty;
                        txtReimbursementFor5.Text = string.Empty;
                        txtReimbursementValue5.Text = string.Empty;
                    }

                    ddlSalaryStatus.SelectedValue = dt.Rows[0]["SalaryStatus"].ToString();
                    btnSubmit.Text = "Update";
                }
                else
                {
                    ClearPnlData();
                }
            }
            else
            {
                ClearPnlData();
                ClearPnlDetail();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalTPTRec_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossDeduction();
            CalculateNetSalary();
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
            cmd = new SqlCommand("SaveEmpSalaryProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
            cmd.Parameters.AddWithValue("@Emp_Code", txtEmp_Code.Text);
            cmd.Parameters.AddWithValue("@SystemNumber", txtSystemNumber.Text);
            cmd.Parameters.AddWithValue("@AssignEmpCode", txtEmpCode.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@DOB", txtDOB.Text);
            cmd.Parameters.AddWithValue("@DOJ", txtDOJ.Text);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@SubjectID", ddlSubjects.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@NatureOfEmp", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", txtFromDt.Text);
            if (txtToDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToDate", null);
            }
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@GradePay", txtGradePay.Text);
            cmd.Parameters.AddWithValue("@BasicScale", txtScale.Text);
            if (txtPfNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@PFNo", txtPfNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PFNo", null);
            }

            if (txtEsiNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ESINo", txtEsiNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ESINo", null);
            }

            if (txtProbationDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ProbationDate", txtProbationDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProbationDate", null);
            }

            if (txtConfirmationDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ConfirmationDate", txtConfirmationDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ConfirmationDate", null);
            }

            if (txtResignDate.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ResignationDate", txtResignDate.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ResignationDate", null);
            }

            cmd.Parameters.AddWithValue("@ChangeScale", ddlChangeScale.SelectedValue);
            if (ddlChangeScale.SelectedValue == "1" && txtEffectedScale.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@EffectedScale", txtEffectedScale.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EffectedScale", 0);
            }
            cmd.Parameters.AddWithValue("@PanCardNo", txtPANCardNo.Text);
            cmd.Parameters.AddWithValue("@AadharCardNo", txtAadharCardNo.Text);
            cmd.Parameters.AddWithValue("@UANNo", txtUANNo.Text);
            cmd.Parameters.AddWithValue("@BusUser", ddlBusUser.SelectedValue);
            cmd.Parameters.AddWithValue("@GovAcc", ddlGovAcc.SelectedValue);
            cmd.Parameters.AddWithValue("@DaAllow", ddlDA.SelectedValue);
            cmd.Parameters.AddWithValue("@DA", txtTotalDA.Text);
            cmd.Parameters.AddWithValue("@DaValue", txtTotalDaValue.Text);
            cmd.Parameters.AddWithValue("@HraAllow", ddlHRA.SelectedValue);
            cmd.Parameters.AddWithValue("@HRA", txtTotalHRA.Text);
            cmd.Parameters.AddWithValue("@HraValue", txtTotalHRAValue.Text);
            cmd.Parameters.AddWithValue("@TransportAllow", ddlTransportAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@TransportValue", txtTotalTransportAllow.Text);
            cmd.Parameters.AddWithValue("@MedicalAllow", ddlMedicalAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@MedicalValue", txtTotalMedicalAllow.Text);
            cmd.Parameters.AddWithValue("@WashingAllow", ddlWashingAllow.SelectedValue);
            cmd.Parameters.AddWithValue("@WashingValue", txtTotalWashingAllow.Text);
            cmd.Parameters.AddWithValue("@ExGratiaAllow", ddlExGratia.SelectedValue);
            cmd.Parameters.AddWithValue("@ExGratiaValue", txtTotalExGratia.Text);
            cmd.Parameters.AddWithValue("@PFDeduct", ddlPFDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@PFValue", txtTotalPF.Text);
            cmd.Parameters.AddWithValue("@ESIDeduct", ddlESIDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@EsiValue", txtTotalESI.Text);
            cmd.Parameters.AddWithValue("@GISDeduct", ddlGISDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@GisValue", txtTotalGIS.Text);
            cmd.Parameters.AddWithValue("@TransportRecovery", txtTotalTPTRec.Text);
            cmd.Parameters.AddWithValue("@ModeOfSalary", rdoModeOfSalary.SelectedValue);
            cmd.Parameters.AddWithValue("@BankID", ddlBank.SelectedValue);

            if (txtAccountNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@AccountNo", txtAccountNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AccountNo", null);
            }

            if (txtIFSCCode.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@IFSCCODE", txtIFSCCode.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@IFSCCODE", null);
            }

            if (txtAddress.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@BranchAddress", txtAddress.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchAddress", null);
            }

            cmd.Parameters.AddWithValue("@MaternityLeave", rdoMaternityLeave.SelectedValue);

            if (txtFromMaternity.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@FromMaternity", txtFromMaternity.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FromMaternity", null);
            }

            if (txtToMaternity.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ToMaternity", txtToMaternity.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToMaternity", null);
            }

            cmd.Parameters.AddWithValue("@Reimbursement", rdoreimbursement.SelectedValue);
            if (lblTotalReimbursement1.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor1", lblTotalReimbursement1.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor1", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue1", lblTotalReimbursementValue1.Text);

            if (lblTotalReimbursement2.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor2", lblTotalReimbursement2.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor2", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue2", lblTotalReimbursementValue2.Text);

            if (lblTotalReimbursement3.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor3", lblTotalReimbursement3.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor3", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue3", lblTotalReimbursementValue3.Text);

            if (lblTotalReimbursement4.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor4", lblTotalReimbursement4.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor4", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue4", lblTotalReimbursementValue4.Text);

            if (lblTotalReimbursement5.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor5", lblTotalReimbursement5.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReimbursementFor5", null);
            }

            cmd.Parameters.AddWithValue("@ReimbursementValue5", lblTotalReimbursementValue5.Text);
            cmd.Parameters.AddWithValue("@TotalReimbursement", lblGrossReimbursement.Text);
            cmd.Parameters.AddWithValue("@GrossAllowance", lblGrossAllowance.Text);
            cmd.Parameters.AddWithValue("@GrossTotal", lblGrossTotal.Text);
            cmd.Parameters.AddWithValue("@GrossDeduction", lblGrossDeduction.Text);
            cmd.Parameters.AddWithValue("@NetSalary", lblNetSalary.Text);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@SalaryStatus", ddlSalaryStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@FatherORHusband", txtFatherORHusbandName.Text);
            cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
            cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
            cmd.Parameters.AddWithValue("@ContractDate", txtContractDate.Text);
            cmd.Parameters.AddWithValue("@LWD", txtLWD.Text);
            cmd.Parameters.AddWithValue("@Address", txtHomeAddress.Text);
            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
            cmd.Parameters.AddWithValue("@BankType", ddlTransferType.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@Type", "Deactivate");

            con.Open();
            int Count = (int)cmd.ExecuteScalar();
            con.Close();
            if (Count == 0)
            {
                ClearPnlData();
                ClearPnlDetail();
                DeactivateEmployee();
                Employee();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
            }
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
            if (ddlStaffType.SelectedValue == "1")
            {
                pnlSubjectFor.Visible = true;
            }
            else
            {
                pnlSubjectFor.Visible = false;
                ddlSubjects.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtTotalHRAValue_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateGrossTotal();
            CalculateNetSalary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlHRA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlHRA.SelectedValue == "1" && txtGradePay.Text.Length > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetSetUpDetails", con);
                cmd.Parameters.AddWithValue("@GradePay", txtGradePay.Text);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                txtTotalHRA.Text = ds.Tables[1].Rows[0]["HRA"].ToString();
                txtTotalHRA.ReadOnly = false;
                txtTotalHRAValue.Text = "0.00";
                txtTotalHRAValue.ReadOnly = true;
            }
            else if (ddlHRA.SelectedValue == "3")
            {
                txtTotalHRA.Text = "0.00";
                txtTotalHRA.ReadOnly = true;
                txtTotalHRAValue.Text = "0.00";
                txtTotalHRAValue.ReadOnly = false;
            }
            else if (ddlHRA.SelectedValue == "2")
            {
                txtTotalHRA.Text = "0";
                txtTotalHRAValue.Text = "0";
                pnlHRAAllow.Visible = false;
            }
            else
            {
                txtGradePay.Focus();
                ddlHRA.ClearSelection();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Enter Grade Pay First');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlDeactivateEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlDeactivateEmployee.SelectedValue) > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@ProfileID", ddlDeactivateEmployee.SelectedValue);
                cmd.Parameters.AddWithValue("@IsActive", Status.Deactive);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    PnlActivateButtons.Visible = true;
                    ddlemployee.ClearSelection();
                    ddlemployee.Enabled = false;
                    pnlUpdateButtons.Visible = false;
                    txtEmployeeID.Text = dt.Rows[0]["EmployeeID"].ToString();
                    txtEmp_Code.Text = dt.Rows[0]["Emp_Code"].ToString();
                    txtSystemNumber.Text = dt.Rows[0]["SystemNumber"].ToString();
                    txtEmpCode.Text = dt.Rows[0]["AssignEmpCode"].ToString();
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtDOB.Text = dt.Rows[0]["DOB"].ToString();
                    txtDOB_TextChanged(sender, e);
                    txtDOJ.Text = dt.Rows[0]["DOJ"].ToString();
                    ddlDesignation.SelectedValue = dt.Rows[0]["Designation"].ToString();
                    ddlStaffType.SelectedValue = dt.Rows[0]["StaffType"].ToString();
                    if (ddlStaffType.SelectedValue == "1")
                    {
                        pnlSubjectFor.Visible = true;
                    }
                    else
                    {
                        pnlSubjectFor.Visible = false;
                    }
                    ddlSubjects.SelectedValue = dt.Rows[0]["SubjectID"].ToString();
                    ddlNatureOfEmp.SelectedValue = dt.Rows[0]["NatureOfEmp"].ToString();
                    txtFromDt.Text = dt.Rows[0]["FromDate"].ToString();
                    txtToDate.Text = dt.Rows[0]["Todate"].ToString();
                    ddlAppointment.SelectedValue = dt.Rows[0]["Appointment"].ToString();
                    txtGradePay.Text = dt.Rows[0]["GradePay"].ToString();
                    txtScale.Text = dt.Rows[0]["BasicScale"].ToString();
                    txtPfNo.Text = dt.Rows[0]["PFNo"].ToString();
                    txtEsiNo.Text = dt.Rows[0]["ESINo"].ToString();
                    txtConfirmationDate.Text = dt.Rows[0]["ConfirmationDate"].ToString();
                    txtProbationDate.Text = dt.Rows[0]["ProbationDate"].ToString();
                    txtResignDate.Text = dt.Rows[0]["ResignationDate"].ToString();
                    ddlChangeScale.SelectedValue = dt.Rows[0]["ChangeScale"].ToString();
                    if (ddlChangeScale.SelectedValue == "1")
                    {
                        pnlEffectedScale.Visible = true;
                    }
                    else
                    {
                        pnlEffectedScale.Visible = false;
                    }
                    txtEffectedScale.Text = dt.Rows[0]["EffectedScale"].ToString();
                    txtPANCardNo.Text = dt.Rows[0]["PanCardNo"].ToString();
                    txtAadharCardNo.Text = dt.Rows[0]["AadharCardNo"].ToString();
                    txtUANNo.Text = dt.Rows[0]["UANNo"].ToString();
                    txtFatherORHusbandName.Text = dt.Rows[0]["Father/Husband"].ToString();
                    txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                    if (dt.Rows[0]["Gender"].ToString().Length > 0)
                    {
                        ddlGender.SelectedValue = dt.Rows[0]["Gender"].ToString();
                    }
                    else
                    {
                        ddlGender.SelectedValue = "0";
                    }
                    txtContractDate.Text = dt.Rows[0]["ContractDate"].ToString();
                    txtLWD.Text = dt.Rows[0]["LWD"].ToString();
                    txtHomeAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                    if (dt.Rows[0]["BankType"].ToString().Length > 0)
                    {
                        ddlTransferType.Text = dt.Rows[0]["BankType"].ToString();
                    }
                    else
                    {
                        ddlTransferType.Text = "0";
                    }

                    ddlBusUser.SelectedValue = dt.Rows[0]["BusUser"].ToString();
                    ddlGovAcc.SelectedValue = dt.Rows[0]["GovAcc"].ToString();
                    ddlDA.SelectedValue = dt.Rows[0]["DaAllow"].ToString();
                    if (ddlDA.SelectedValue == "1")
                    {
                        txtTotalDA.Text = dt.Rows[0]["DA"].ToString();
                    }
                    else
                    {
                        txtTotalDA.Text = "0";
                    }
                    ddlHRA.SelectedValue = dt.Rows[0]["HraAllow"].ToString();
                    if (ddlHRA.SelectedValue == "1" || ddlHRA.SelectedValue == "3")
                    {
                        txtTotalHRA.Text = dt.Rows[0]["HRA"].ToString();
                        txtTotalHRAValue.Text = dt.Rows[0]["HraValue"].ToString();
                    }
                    else
                    {
                        txtTotalHRA.Text = "0";
                    }
                    ddlTransportAllow.SelectedValue = dt.Rows[0]["TransportAllow"].ToString();
                    if (ddlTransportAllow.SelectedValue == "1")
                    {
                        txtTotalTransportAllow.Text = dt.Rows[0]["TransportValue"].ToString();
                    }
                    else
                    {
                        txtTotalTransportAllow.Text = "0";
                    }
                    ddlMedicalAllow.SelectedValue = dt.Rows[0]["MedicalAllow"].ToString();
                    if (ddlMedicalAllow.SelectedValue == "1")
                    {
                        txtTotalMedicalAllow.Text = dt.Rows[0]["MedicalValue"].ToString();
                    }
                    else
                    {
                        txtTotalMedicalAllow.Text = "0";
                    }
                    ddlWashingAllow.SelectedValue = dt.Rows[0]["WashingAllow"].ToString();
                    if (ddlWashingAllow.SelectedValue == "1")
                    {
                        txtTotalWashingAllow.Text = dt.Rows[0]["WashingValue"].ToString();
                    }
                    else
                    {
                        txtTotalWashingAllow.Text = "0";
                    }
                    ddlExGratia.SelectedValue = dt.Rows[0]["ExGratiaAllow"].ToString();
                    if (ddlExGratia.SelectedValue == "1")
                    {
                        txtTotalExGratia.Text = dt.Rows[0]["ExGratiaValue"].ToString();
                    }
                    else
                    {
                        txtTotalExGratia.Text = "0";
                    }
                    ddlPFDeduct.SelectedValue = dt.Rows[0]["PFDeduct"].ToString();
                    txtTotalPF.Text = dt.Rows[0]["PFValue"].ToString();
                    if (ddlPFDeduct.SelectedValue == "1")
                    {
                        rfvPFNo.ValidationGroup = "Save";
                    }
                    else
                    {
                        rfvPFNo.ValidationGroup = "Cancel";
                    }
                    ddlESIDeduct.SelectedValue = dt.Rows[0]["ESIDeduct"].ToString();
                    txtTotalESI.Text = dt.Rows[0]["ESIValue"].ToString();
                    if (ddlESIDeduct.SelectedValue == "1")
                    {
                        rfvESINo.ValidationGroup = "Save";
                    }
                    else
                    {
                        rfvESINo.ValidationGroup = "Cancel";
                    }
                    ddlGISDeduct.SelectedValue = dt.Rows[0]["GISDeduct"].ToString();
                    txtTotalGIS.Text = dt.Rows[0]["GISValue"].ToString();
                    if (ddlGISDeduct.SelectedValue == "1")
                    {
                        pnlSelectGIS.Visible = true;
                        BindGIS();
                        ddlSelectGIS.SelectedValue = dt.Rows[0]["GISValue"].ToString();
                    }
                    else
                    {
                        pnlSelectGIS.Visible = false;
                        ddlSelectGIS.ClearSelection();
                    }
                    txtTotalTPTRec.Text = dt.Rows[0]["TransportRecovery"].ToString();
                    rdoModeOfSalary.SelectedValue = dt.Rows[0]["ModeOfSalary"].ToString();
                    if (rdoModeOfSalary.SelectedValue == "2")
                    {
                        pnlBankDetails.Visible = true;
                        ddlBank.SelectedValue = dt.Rows[0]["BankID"].ToString();
                        txtAccountNo.Text = dt.Rows[0]["AccountNo"].ToString();
                        txtIFSCCode.Text = dt.Rows[0]["IFSCCODE"].ToString();
                        txtAddress.Text = dt.Rows[0]["BranchAddress"].ToString();
                    }
                    else
                    {
                        pnlBankDetails.Visible = false;
                        ddlBank.ClearSelection();
                        txtAccountNo.Text = string.Empty;
                        txtIFSCCode.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                    }
                    rdoMaternityLeave.SelectedValue = dt.Rows[0]["MaternityLeave"].ToString();
                    if (rdoMaternityLeave.SelectedValue == "1")
                    {
                        pnlMaternityLeave.Visible = true;
                        txtFromMaternity.Text = dt.Rows[0]["FromMaternity"].ToString();
                        txtToMaternity.Text = dt.Rows[0]["ToMaternity"].ToString();
                    }
                    else
                    {
                        pnlMaternityLeave.Visible = false;
                        txtFromMaternity.Text = string.Empty;
                        txtToMaternity.Text = string.Empty;
                    }
                    rdoreimbursement.SelectedValue = dt.Rows[0]["Reimbursement"].ToString();
                    if (rdoreimbursement.SelectedValue == "1")
                    {
                        pnlReimbursement.Visible = true;
                        txtReimbursementFor1.Text = dt.Rows[0]["ReimbursementFor1"].ToString();
                        txtReimbursementValue1.Text = dt.Rows[0]["ReimbursementValue1"].ToString();
                        txtReimbursementFor2.Text = dt.Rows[0]["ReimbursementFor2"].ToString();
                        txtReimbursementValue2.Text = dt.Rows[0]["ReimbursementValue2"].ToString();
                        txtReimbursementFor3.Text = dt.Rows[0]["ReimbursementFor3"].ToString();
                        txtReimbursementValue3.Text = dt.Rows[0]["ReimbursementValue3"].ToString();
                        txtReimbursementFor4.Text = dt.Rows[0]["ReimbursementFor4"].ToString();
                        txtReimbursementValue4.Text = dt.Rows[0]["ReimbursementValue4"].ToString();
                        txtReimbursementFor5.Text = dt.Rows[0]["ReimbursementFor5"].ToString();
                        txtReimbursementValue5.Text = dt.Rows[0]["ReimbursementValue5"].ToString();
                    }
                    else
                    {
                        pnlReimbursement.Visible = false;
                        txtReimbursementFor1.Text = string.Empty;
                        txtReimbursementValue1.Text = string.Empty;
                        txtReimbursementFor2.Text = string.Empty;
                        txtReimbursementValue2.Text = string.Empty;
                        txtReimbursementFor3.Text = string.Empty;
                        txtReimbursementValue3.Text = string.Empty;
                        txtReimbursementFor4.Text = string.Empty;
                        txtReimbursementValue4.Text = string.Empty;
                        txtReimbursementFor5.Text = string.Empty;
                        txtReimbursementValue5.Text = string.Empty;
                    }

                    ddlSalaryStatus.SelectedValue = dt.Rows[0]["SalaryStatus"].ToString();
                }
                else
                {
                    ClearPnlData();
                    ClearPnlDetail();
                }
            }
            else
            {
                ClearPnlData();
                ClearPnlDetail();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("SaveEmpSalaryProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Emp_Code", null);
            cmd.Parameters.AddWithValue("@AssignEmpCode", null);
            cmd.Parameters.AddWithValue("@Name", null);
            cmd.Parameters.AddWithValue("@DOB", null);
            cmd.Parameters.AddWithValue("@DOJ", null);
            cmd.Parameters.AddWithValue("@FromDate", null);
            cmd.Parameters.AddWithValue("@ToDate", null);
            cmd.Parameters.AddWithValue("@PFNo", null);
            cmd.Parameters.AddWithValue("@ESINo", null);
            cmd.Parameters.AddWithValue("@ProbationDate", null);
            cmd.Parameters.AddWithValue("@ConfirmationDate", null);
            cmd.Parameters.AddWithValue("@ResignationDate", null);
            cmd.Parameters.AddWithValue("@PanCardNo", null);
            cmd.Parameters.AddWithValue("@AadharCardNo", null);
            cmd.Parameters.AddWithValue("@BankID", null);
            cmd.Parameters.AddWithValue("@IFSCCode", null);
            cmd.Parameters.AddWithValue("@BranchAddress", null);
            cmd.Parameters.AddWithValue("@FromMaternity", null);
            cmd.Parameters.AddWithValue("@ToMaternity", null);
            cmd.Parameters.AddWithValue("@ReimbursementFor1", null);
            cmd.Parameters.AddWithValue("@ReimbursementFor2", null);
            cmd.Parameters.AddWithValue("@ReimbursementFor3", null);
            cmd.Parameters.AddWithValue("@ReimbursementFor4", null);
            cmd.Parameters.AddWithValue("@ReimbursementFor5", null);
            cmd.Parameters.AddWithValue("@FatherORHusband", null);
            cmd.Parameters.AddWithValue("@EmailID", null);
            cmd.Parameters.AddWithValue("@MobileNo", null);
            cmd.Parameters.AddWithValue("@ContractDate", null);
            cmd.Parameters.AddWithValue("@LWD", null);
            cmd.Parameters.AddWithValue("@Address", null);
            cmd.Parameters.AddWithValue("@Remarks", null);
            cmd.Parameters.AddWithValue("@UANNo", null);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@ProfileID", ddlDeactivateEmployee.SelectedValue);
            cmd.Parameters.AddWithValue("@Type", "Activate");
            con.Open();
            int HasRow = cmd.ExecuteNonQuery();
            con.Close();

            if (HasRow > 0)
            {
                ClearPnlData();
                ClearPnlDetail();
                DeactivateEmployee();
                Employee();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Activate Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found to Activate.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCancelActivate_Click(object sender, EventArgs e)
    {
        try
        {
            ClearPnlData();
            ClearPnlDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}