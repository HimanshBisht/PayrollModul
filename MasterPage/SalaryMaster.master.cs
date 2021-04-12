using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using System.Web.Script.Serialization;

public partial class SalaryMaster : System.Web.UI.MasterPage
{
    string constr = "";
    SqlConnection con;
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
            if (Session["User"] != null || Session["LastLogin"] != null || Session["SchoolName"] != null)
            {
                if (!IsPostBack)
                {
                    hash = (Hashtable)Session["User"];
                    lblUser.Text = "Welcome - " + Convert.ToString(hash["Name"].ToString());
                    if (Session["LastLogin"].ToString().Length > 0)
                    {
                        lblLastLogin.Text = "Your Last Login - " + Convert.ToDateTime(Session["LastLogin"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss tt");
                    }
                    else
                    {
                        lblLastLogin.Text = string.Empty;
                    }

                    ToggleDisplayPages();
                }
            }
            else
            {
                Response.Redirect("../Default.aspx");
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

    public void ToggleDisplayPages()
    {
        try
        {
            int AppointmentTypeSetup = 0;
            int BankSetup = 0;
            int ChangePassword = 0;
            int DaSetup = 0;
            int DesignationSetup = 0;
            int EmployeeNatureSetup = 0;
            int ESISetup = 0;
            int GISSetup = 0;
            int HraSetUp = 0;
            int MakeUser = 0;
            int MedicalSetup = 0;
            int PFSetup = 0;
            int StaffTypeSetup = 0;
            int SubjectsSetup = 0;
            int TransportRecoverySetUp = 0;
            int TransportSetUp = 0;
            int YearSetup = 0;
            // int ImportArear = 0;
            int ImportPaidDays = 0;
            int ImportTDS = 0;
            int EmployeeSalaryProfile = 0;
            int AdvanceMaking = 0;
            int GenerateSalarySlip = 0;
            int ReimbursementMaking = 0;
            int SalaryMaking = 0;
            int DaSetupReport = 0;
            int ESISetupReport = 0;
            int GISSetupReport = 0;
            int HraSetUpReport = 0;
            int MedicalSetupReport = 0;
            int BankTransferReport = 0;
            int MonthlyESIReport = 0;
            int MonthlyPFReport = 0;
            int MonthlyReimbursementReport = 0;
            int CashEmpSalaryReport = 0;
            int ChequeEmpSalaryReport = 0;
            int MonthlySalaryReport = 0;
            int PFSetupReport = 0;
            int EmployeeProfileReport = 0;
            int TransportRecoverySetUpReport = 0;
            int TransportSetUpReport = 0;
            int ProfileModificationReport = 0;
            int FNFMaking = 0;
            int FNFReport = 0;
            int AssignNoticeDays = 0;
            int SalaryApproval = 0;
            int EmpWiseSalaryReport = 0;
            int ImportBasicScale = 0;
            int UpdateEmpProfile = 0;
            // int DeductionMaking = 0;
            int TDSAdditionalAmountCaption = 0;
            int TDSAdditionalAmount = 0;
            int TDSForm16 = 0;
            int TDSHeaders = 0;
            int TDSHeaderwiseRules = 0;
            int TDSHeaderwiseFooter = 0;
            int TDSInvestmentDeclarationForm = 0;
            int MidMonthIncrementSalaryMaking = 0;
            int AccommodationCriteria = 0;
            int StandardDeduction = 0;
            int PendingIDFReport = 0;
            int IncomeTaxRules = 0;
            int SurchargeRules = 0;
            int CalculateTax = 0;
            int EmpWiseTDSReport = 0;
            int IDFApproval = 0;
            int IDFDisapproval = 0;
            int CalculateArrear = 0;
            int ArrearReport = 0;
            int CalculateRecovery = 0;
            int RecoveryReport = 0;
            int ManageNoticePeriod = 0;
            int NoticePeriodReport = 0;
            int TDSForm16AllEmp = 0;
            int ImportIDF = 0;
            int DownloadIDF = 0;

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
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    if (row["PageName"].ToString() == "AppointmentSetup.aspx")
                    {
                        AppointmentTypeSetup = AppointmentTypeSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "BankSetup.aspx")
                    {
                        BankSetup = BankSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "AssignNoticeDays.aspx")
                    {
                        AssignNoticeDays = AssignNoticeDays + 1;
                    }
                    else if (row["PageName"].ToString() == "ChangePassword.aspx")
                    {
                        ChangePassword = ChangePassword + 1;
                    }
                    else if (row["PageName"].ToString() == "DaSetup.aspx")
                    {
                        DaSetup = DaSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "DesignationSetup.aspx")
                    {
                        DesignationSetup = DesignationSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "EmpNatureSetup.aspx")
                    {
                        EmployeeNatureSetup = EmployeeNatureSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "ESISetup.aspx")
                    {
                        ESISetup = ESISetup + 1;
                    }
                    else if (row["PageName"].ToString() == "GISSetup.aspx")
                    {
                        GISSetup = GISSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "HraSetUp.aspx")
                    {
                        HraSetUp = HraSetUp + 1;
                    }
                    else if (row["PageName"].ToString() == "MakeUser.aspx")
                    {
                        MakeUser = MakeUser + 1;
                    }
                    else if (row["PageName"].ToString() == "MedicalSetup.aspx")
                    {
                        MedicalSetup = MedicalSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "PFSetup.aspx")
                    {
                        PFSetup = PFSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "StaffTypeSetup.aspx")
                    {
                        StaffTypeSetup = StaffTypeSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "SubjectsSetup.aspx")
                    {
                        SubjectsSetup = SubjectsSetup + 1;
                    }
                    else if (row["PageName"].ToString() == "TransportRecoverySetUp.aspx")
                    {
                        TransportRecoverySetUp = TransportRecoverySetUp + 1;
                    }
                    else if (row["PageName"].ToString() == "TransportSetUp.aspx")
                    {
                        TransportSetUp = TransportSetUp + 1;
                    }
                    else if (row["PageName"].ToString() == "YearSetup.aspx")
                    {
                        YearSetup = YearSetup + 1;
                    }
                    //else if (row["PageName"].ToString() == "ImportArear.aspx")
                    //{
                    //    ImportArear = ImportArear + 1;
                    //}
                    else if (row["PageName"].ToString() == "ImportPaidDays.aspx")
                    {
                        ImportPaidDays = ImportPaidDays + 1;
                    }
                    else if (row["PageName"].ToString() == "ImportTDS.aspx")
                    {
                        ImportTDS = ImportTDS + 1;
                    }
                    else if (row["PageName"].ToString() == "EmpSalaryProfile.aspx")
                    {
                        EmployeeSalaryProfile = EmployeeSalaryProfile + 1;
                    }
                    else if (row["PageName"].ToString() == "AdvanceMaking.aspx")
                    {
                        AdvanceMaking = AdvanceMaking + 1;
                    }
                    else if (row["PageName"].ToString() == "GenerateSalarySlip.aspx")
                    {
                        GenerateSalarySlip = GenerateSalarySlip + 1;
                    }
                    else if (row["PageName"].ToString() == "ReimbursementMaking.aspx")
                    {
                        ReimbursementMaking = ReimbursementMaking + 1;
                    }
                    else if (row["PageName"].ToString() == "SalaryMaking.aspx")
                    {
                        SalaryMaking = SalaryMaking + 1;
                    }
                    else if (row["PageName"].ToString() == "FNFMaking.aspx")
                    {
                        FNFMaking = FNFMaking + 1;
                    }
                    else if (row["PageName"].ToString() == "FNFReport.aspx")
                    {
                        FNFReport = FNFReport + 1;
                    }
                    else if (row["PageName"].ToString() == "DaSetupReport.aspx")
                    {
                        DaSetupReport = DaSetupReport + 1;
                    }
                    else if (row["PageName"].ToString() == "ESISetupReport.aspx")
                    {
                        ESISetupReport = ESISetupReport + 1;
                    }
                    else if (row["PageName"].ToString() == "GISSetupReport.aspx")
                    {
                        GISSetupReport = GISSetupReport + 1;
                    }
                    else if (row["PageName"].ToString() == "HraSetUpReport.aspx")
                    {
                        HraSetUpReport = HraSetUpReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MedicalSetupReport.aspx")
                    {
                        MedicalSetupReport = MedicalSetupReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlyBankTransferReport.aspx")
                    {
                        BankTransferReport = BankTransferReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlyESIReport.aspx")
                    {
                        MonthlyESIReport = MonthlyESIReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlyPFReport.aspx")
                    {
                        MonthlyPFReport = MonthlyPFReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlyReimbursementReport.aspx")
                    {
                        MonthlyReimbursementReport = MonthlyReimbursementReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlySalaryCashEmpReport.aspx")
                    {
                        CashEmpSalaryReport = CashEmpSalaryReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlySalaryChequeEmpReport.aspx")
                    {
                        ChequeEmpSalaryReport = ChequeEmpSalaryReport + 1;
                    }
                    else if (row["PageName"].ToString() == "MonthlySalaryReport.aspx")
                    {
                        MonthlySalaryReport = MonthlySalaryReport + 1;
                    }
                    else if (row["PageName"].ToString() == "PFSetupReport.aspx")
                    {
                        PFSetupReport = PFSetupReport + 1;
                    }
                    else if (row["PageName"].ToString() == "ProfileReport.aspx")
                    {
                        EmployeeProfileReport = EmployeeProfileReport + 1;
                    }
                    else if (row["PageName"].ToString() == "TransportRecoverySetUpReport.aspx")
                    {
                        TransportRecoverySetUpReport = TransportRecoverySetUpReport + 1;
                    }
                    else if (row["PageName"].ToString() == "TransportSetUpReport.aspx")
                    {
                        TransportSetUpReport = TransportSetUpReport + 1;
                    }
                    else if (row["PageName"].ToString() == "ProfileHistoryReport.aspx")
                    {
                        ProfileModificationReport = ProfileModificationReport + 1;
                    }
                    else if (row["PageName"].ToString() == "EmpWiseSalaryReport.aspx")
                    {
                        EmpWiseSalaryReport = EmpWiseSalaryReport + 1;
                    }
                    else if (row["PageName"].ToString() == "SalaryApproval.aspx")
                    {
                        SalaryApproval = SalaryApproval + 1;
                    }
                    else if (row["PageName"].ToString() == "ImportBasicScale.aspx")
                    {
                        ImportBasicScale = ImportBasicScale + 1;
                    }
                    else if (row["PageName"].ToString() == "UpdateEmpProfile.aspx")
                    {
                        UpdateEmpProfile = UpdateEmpProfile + 1;
                    }
                    //else if (row["PageName"].ToString() == "DeductionMaking.aspx")
                    //{
                    //    DeductionMaking = DeductionMaking + 1;
                    //}
                    else if (row["PageName"].ToString() == "TDSAdditionalAmountCaption.aspx")
                    {
                        TDSAdditionalAmountCaption = TDSAdditionalAmountCaption + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSAdditionalAmount.aspx")
                    {
                        TDSAdditionalAmount = TDSAdditionalAmount + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSForm16.aspx")
                    {
                        TDSForm16 = TDSForm16 + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSHeaders.aspx")
                    {
                        TDSHeaders = TDSHeaders + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSHeaderwiseRules.aspx")
                    {
                        TDSHeaderwiseRules = TDSHeaderwiseRules + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSHeaderwiseFooter.aspx")
                    {
                        TDSHeaderwiseFooter = TDSHeaderwiseFooter + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSInvestmentDeclarationForm.aspx")
                    {
                        TDSInvestmentDeclarationForm = TDSInvestmentDeclarationForm + 1;
                    }
                    else if (row["PageName"].ToString() == "MidMonthIncrementSalaryMaking.aspx")
                    {
                        MidMonthIncrementSalaryMaking = MidMonthIncrementSalaryMaking + 1;
                    }
                    else if (row["PageName"].ToString() == "AccommodationCriteria.aspx")
                    {
                        AccommodationCriteria = AccommodationCriteria + 1;
                    }
                    else if (row["PageName"].ToString() == "StandardDeduction.aspx")
                    {
                        StandardDeduction = StandardDeduction + 1;
                    }
                    else if (row["PageName"].ToString() == "PendingIDFReport.aspx")
                    {
                        PendingIDFReport = PendingIDFReport + 1;
                    }
                    else if (row["PageName"].ToString() == "IncomeTaxRules.aspx")
                    {
                        IncomeTaxRules = IncomeTaxRules + 1;
                    }
                    else if (row["PageName"].ToString() == "SurchargeRules.aspx")
                    {
                        SurchargeRules = SurchargeRules + 1;
                    }
                    else if (row["PageName"].ToString() == "CalculateTax.aspx")
                    {
                        CalculateTax = CalculateTax + 1;
                    }
                    else if (row["PageName"].ToString() == "EmpWiseTDSReport.aspx")
                    {
                        EmpWiseTDSReport = EmpWiseTDSReport + 1;
                    }
                    else if (row["PageName"].ToString() == "IDFApproval.aspx")
                    {
                        IDFApproval = IDFApproval + 1;
                    }
                    else if (row["PageName"].ToString() == "IDFDisapproval.aspx")
                    {
                        IDFDisapproval = IDFDisapproval + 1;
                    }
                    else if (row["PageName"].ToString() == "CalculateArrear.aspx")
                    {
                        CalculateArrear = CalculateArrear + 1;
                    }
                    else if (row["PageName"].ToString() == "ArrearReport.aspx")
                    {
                        ArrearReport = ArrearReport + 1;
                    }
                    else if (row["PageName"].ToString() == "CalculateRecovery.aspx")
                    {
                        CalculateRecovery = CalculateRecovery + 1;
                    }
                    else if (row["PageName"].ToString() == "RecoveryReport.aspx")
                    {
                        RecoveryReport = RecoveryReport + 1;
                    }
                    else if (row["PageName"].ToString() == "ManageNoticePeriod.aspx")
                    {
                        ManageNoticePeriod = ManageNoticePeriod + 1;
                    }
                    else if (row["PageName"].ToString() == "NoticePeriodReport.aspx")
                    {
                        NoticePeriodReport = NoticePeriodReport + 1;
                    }
                    else if (row["PageName"].ToString() == "TDSForm16AllEmp.aspx")
                    {
                        TDSForm16AllEmp = TDSForm16AllEmp + 1;
                    }
                    else if (row["PageName"].ToString() == "ImportIDF.aspx")
                    {
                        ImportIDF = ImportIDF + 1;
                    }
                    else if (row["PageName"].ToString() == "DownloadIDF.aspx")
                    {
                        DownloadIDF = DownloadIDF + 1;
                    }
                }

                if (AppointmentTypeSetup > 0)
                {
                    liAppointmentSetup.Visible = true;
                }
                else
                {
                    liAppointmentSetup.Visible = false;
                }
                if (AssignNoticeDays > 0)
                {
                    liAssignNoticeDays.Visible = true;
                }
                else
                {
                    liAssignNoticeDays.Visible = false;
                }
                if (BankSetup > 0)
                {
                    liBankSetup.Visible = true;
                }
                else
                {
                    liBankSetup.Visible = false;
                }
                if (ChangePassword > 0)
                {
                    liChangePassword.Visible = true;
                }
                else
                {
                    liChangePassword.Visible = false;
                }
                if (DaSetup > 0)
                {
                    liDaSetup.Visible = true;
                }
                else
                {
                    liDaSetup.Visible = false;
                }
                if (DesignationSetup > 0)
                {
                    liDesignationSetup.Visible = true;
                }
                else
                {
                    liDesignationSetup.Visible = false;
                }
                if (EmployeeNatureSetup > 0)
                {
                    liEmpNatureSetup.Visible = true;
                }
                else
                {
                    liEmpNatureSetup.Visible = false;
                }
                if (ESISetup > 0)
                {
                    liESISetup.Visible = true;
                }
                else
                {
                    liESISetup.Visible = false;
                }
                //if (GISSetup > 0)
                //{
                //    liGISSetup.Visible = true;
                //}
                //else
                //{
                //    liGISSetup.Visible = false;
                //}
                if (HraSetUp > 0)
                {
                    liHraSetUp.Visible = true;
                }
                else
                {
                    liHraSetUp.Visible = false;
                }
                if (MakeUser > 0)
                {
                    liMakeUser.Visible = true;
                }
                else
                {
                    liMakeUser.Visible = false;
                }
                if (MedicalSetup > 0)
                {
                    liMedicalSetup.Visible = true;
                }
                else
                {
                    liMedicalSetup.Visible = false;
                }
                if (PFSetup > 0)
                {
                    liPFSetup.Visible = true;
                }
                else
                {
                    liPFSetup.Visible = false;
                }
                if (StaffTypeSetup > 0)
                {
                    liStaffTypeSetup.Visible = true;
                }
                else
                {
                    liStaffTypeSetup.Visible = false;
                }
                if (SubjectsSetup > 0)
                {
                    liSubjectsSetup.Visible = true;
                }
                else
                {
                    liSubjectsSetup.Visible = false;
                }
                if (TransportRecoverySetUp > 0)
                {
                    liTransportRecoverySetUp.Visible = true;
                }
                else
                {
                    liTransportRecoverySetUp.Visible = false;
                }
                if (TransportSetUp > 0)
                {
                    liTransportSetUp.Visible = true;
                }
                else
                {
                    liTransportSetUp.Visible = false;
                }
                if (YearSetup > 0)
                {
                    liYearSetup.Visible = true;
                }
                else
                {
                    liYearSetup.Visible = false;
                }
                if (AdvanceMaking > 0)
                {
                    liAdvanceMaking.Visible = true;
                }
                else
                {
                    liAdvanceMaking.Visible = false;
                }
                //if (ImportArear > 0)
                //{
                //    liImportArrear.Visible = true;
                //}
                //else
                //{
                //    liImportArrear.Visible = false;
                //}
                if (ImportPaidDays > 0)
                {
                    liImportPaidDays.Visible = true;
                }
                else
                {
                    liImportPaidDays.Visible = false;
                }
                if (ImportTDS > 0)
                {
                    liImportTDS.Visible = true;
                }
                else
                {
                    liImportTDS.Visible = false;
                }
                if (EmployeeSalaryProfile > 0)
                {
                    liEmpSalaryProfile.Visible = true;
                }
                else
                {
                    liEmpSalaryProfile.Visible = false;
                }
                if (GenerateSalarySlip > 0)
                {
                    liGenerateSalarySlip.Visible = true;
                }
                else
                {
                    liGenerateSalarySlip.Visible = false;
                }
                if (ReimbursementMaking > 0)
                {
                    liReimbursementMaking.Visible = true;
                }
                else
                {
                    liReimbursementMaking.Visible = false;
                }
                if (SalaryMaking > 0)
                {
                    liSalaryMaking.Visible = true;
                }
                else
                {
                    liSalaryMaking.Visible = false;
                }
                if (FNFMaking > 0)
                {
                    liFNFMaking.Visible = true;
                }
                else
                {
                    liFNFMaking.Visible = false;
                }
                if (FNFReport > 0)
                {
                    liFNFReport.Visible = true;
                }
                else
                {
                    liFNFReport.Visible = false;
                }
                if (DaSetupReport > 0)
                {
                    liDaSetupReport.Visible = true;
                }
                else
                {
                    liDaSetupReport.Visible = false;
                }
                if (ESISetupReport > 0)
                {
                    liESISetupReport.Visible = true;
                }
                else
                {
                    liESISetupReport.Visible = false;
                }
                //if (GISSetupReport > 0)
                //{
                //    liGISSetupReport.Visible = true;
                //}
                //else
                //{
                //    liGISSetupReport.Visible = false;
                //}
                if (HraSetUpReport > 0)
                {
                    liHraSetUpReport.Visible = true;
                }
                else
                {
                    liHraSetUpReport.Visible = false;
                }
                if (MedicalSetupReport > 0)
                {
                    liMedicalSetupReport.Visible = true;
                }
                else
                {
                    liMedicalSetupReport.Visible = false;
                }
                if (BankTransferReport > 0)
                {
                    liMonthlyBankTransferReport.Visible = true;
                }
                else
                {
                    liMonthlyBankTransferReport.Visible = false;
                }
                if (MonthlyESIReport > 0)
                {
                    liMonthlyESIReport.Visible = true;
                }
                else
                {
                    liMonthlyESIReport.Visible = false;
                }
                if (MonthlyPFReport > 0)
                {
                    liMonthlyPFReport.Visible = true;
                }
                else
                {
                    liMonthlyPFReport.Visible = false;
                }
                if (MonthlyReimbursementReport > 0)
                {
                    liMonthlyReimbursementReport.Visible = true;
                }
                else
                {
                    liMonthlyReimbursementReport.Visible = false;
                }
                if (CashEmpSalaryReport > 0)
                {
                    liMonthlySalaryCashEmpReport.Visible = true;
                }
                else
                {
                    liMonthlySalaryCashEmpReport.Visible = false;
                }
                if (ChequeEmpSalaryReport > 0)
                {
                    liMonthlySalaryChequeEmpReport.Visible = true;
                }
                else
                {
                    liMonthlySalaryChequeEmpReport.Visible = false;
                }
                if (MonthlySalaryReport > 0)
                {
                    liMonthlySalaryReport.Visible = true;
                }
                else
                {
                    liMonthlySalaryReport.Visible = false;
                }
                if (PFSetupReport > 0)
                {
                    liPFSetupReport.Visible = true;
                }
                else
                {
                    liPFSetupReport.Visible = false;
                }
                if (EmployeeProfileReport > 0)
                {
                    liProfileReport.Visible = true;
                }
                else
                {
                    liProfileReport.Visible = false;
                }
                if (TransportRecoverySetUpReport > 0)
                {
                    liTransportRecoverySetUpReport.Visible = true;
                }
                else
                {
                    liTransportRecoverySetUpReport.Visible = false;
                }
                if (TransportSetUpReport > 0)
                {
                    liTransportSetUpReport.Visible = true;
                }
                else
                {
                    liTransportSetUpReport.Visible = false;
                }
                if (ProfileModificationReport > 0)
                {
                    liProfileReportHistory.Visible = true;
                }
                else
                {
                    liProfileReportHistory.Visible = false;
                }

                if (EmpWiseSalaryReport > 0)
                {
                    liEmpWiseSalaryReport.Visible = true;
                }
                else
                {
                    liEmpWiseSalaryReport.Visible = false;
                }
                if (SalaryApproval > 0)
                {
                    liSalaryApproval.Visible = true;
                }
                else
                {
                    liSalaryApproval.Visible = false;
                }
                if (ImportBasicScale > 0)
                {
                    liImportBasicScale.Visible = true;
                }
                else
                {
                    liImportBasicScale.Visible = false;
                }
                if (UpdateEmpProfile > 0)
                {
                    liUpdateEmpProfile.Visible = true;
                }
                else
                {
                    liUpdateEmpProfile.Visible = false;
                }
                //if (DeductionMaking > 0)
                //{
                //    liDeductionMaking.Visible = true;
                //}
                //else
                //{
                //    liDeductionMaking.Visible = false;
                //}
                if (TDSAdditionalAmountCaption > 0)
                {
                    liTDSAdditionalAmountCaption.Visible = true;
                }
                else
                {
                    liTDSAdditionalAmountCaption.Visible = false;
                }
                if (TDSAdditionalAmount > 0)
                {
                    liTDSAdditionalAmount.Visible = true;
                }
                else
                {
                    liTDSAdditionalAmount.Visible = false;
                }
                if (TDSForm16 > 0)
                {
                    liTDSForm16.Visible = true;
                }
                else
                {
                    liTDSForm16.Visible = false;
                }
                if (TDSHeaders > 0)
                {
                    liTDSHeader.Visible = true;
                }
                else
                {
                    liTDSHeader.Visible = false;
                }
                if (TDSHeaderwiseRules > 0)
                {
                    liTDSHeaderwiseRules.Visible = true;
                }
                else
                {
                    liTDSHeaderwiseRules.Visible = false;
                }
                if (TDSHeaderwiseFooter > 0)
                {
                    liTDSHeaderwiseFooter.Visible = true;
                }
                else
                {
                    liTDSHeaderwiseFooter.Visible = false;
                }
                if (TDSInvestmentDeclarationForm > 0)
                {
                    liTDSInvestmentDeclarationForm.Visible = true;
                }
                else
                {
                    liTDSInvestmentDeclarationForm.Visible = false;
                }
                if (MidMonthIncrementSalaryMaking > 0)
                {
                    liMidMonthIncrementSalaryMaking.Visible = true;
                }
                else
                {
                    liMidMonthIncrementSalaryMaking.Visible = false;
                }
                if (AccommodationCriteria > 0)
                {
                    liAccomodationCriteria.Visible = true;
                }
                else
                {
                    liAccomodationCriteria.Visible = false;
                }
                if (StandardDeduction > 0)
                {
                    liStandardDeduction.Visible = true;
                }
                else
                {
                    liStandardDeduction.Visible = false;
                }
                if (PendingIDFReport > 0)
                {
                    liPendingIDFReport.Visible = true;
                }
                else
                {
                    liPendingIDFReport.Visible = false;
                }
                if (IncomeTaxRules > 0)
                {
                    liIncomeTaxRules.Visible = true;
                }
                else
                {
                    liIncomeTaxRules.Visible = false;
                }
                if (SurchargeRules > 0)
                {
                    liSurchargeRules.Visible = true;
                }
                else
                {
                    liSurchargeRules.Visible = false;
                }
                if (CalculateTax > 0)
                {
                    liCalculateTax.Visible = true;
                }
                else
                {
                    liCalculateTax.Visible = false;
                }
                if (EmpWiseTDSReport > 0)
                {
                    liEmpWiseTDSReport.Visible = true;
                }
                else
                {
                    liEmpWiseTDSReport.Visible = false;
                }
                if (IDFApproval > 0)
                {
                    liIDFApproval.Visible = true;
                }
                else
                {
                    liIDFApproval.Visible = false;
                }
                if (IDFDisapproval > 0)
                {
                    liIDFDisapproval.Visible = true;
                }
                else
                {
                    liIDFDisapproval.Visible = false;
                }
                if (CalculateArrear > 0)
                {
                    liCalculateArrear.Visible = true;
                }
                else
                {
                    liCalculateArrear.Visible = false;
                }
                if (ArrearReport > 0)
                {
                    liArrearReport.Visible = true;
                }
                else
                {
                    liArrearReport.Visible = false;
                }
                if (CalculateRecovery > 0)
                {
                    liCalculateRecovery.Visible = true;
                }
                else
                {
                    liCalculateRecovery.Visible = false;
                }
                if (RecoveryReport > 0)
                {
                    liRecoveryReport.Visible = true;
                }
                else
                {
                    liRecoveryReport.Visible = false;
                }
                if (ManageNoticePeriod > 0)
                {
                    liManageNoticePeriod.Visible = true;
                }
                else
                {
                    liManageNoticePeriod.Visible = false;
                }
                if (NoticePeriodReport > 0)
                {
                    liNoticePeriodReport.Visible = true;
                }
                else
                {
                    liNoticePeriodReport.Visible = false;
                }
                if (TDSForm16AllEmp > 0)
                {
                    liTDSForm16AllEmp.Visible = true;
                }
                else
                {
                    liTDSForm16AllEmp.Visible = false;
                }
                if (ImportIDF > 0)
                {
                    liImportIDF.Visible = true;
                }
                else
                {
                    liImportIDF.Visible = false;
                }
                if (DownloadIDF > 0)
                {
                    liDownloadIDF.Visible = true;
                }
                else
                {
                    liDownloadIDF.Visible = false;
                }

                if (AppointmentTypeSetup > 0 || AssignNoticeDays > 0 || BankSetup > 0 || ChangePassword > 0 || DaSetup > 0 || DesignationSetup > 0 || EmployeeNatureSetup > 0 || ESISetup > 0 || GISSetup > 0 || HraSetUp > 0 || MakeUser > 0 || MedicalSetup > 0 || PFSetup > 0 || StaffTypeSetup > 0 || SubjectsSetup > 0 || TransportRecoverySetUp > 0 || TransportSetUp > 0 || YearSetup > 0)
                {
                    liSetUp.Visible = true;

                    if (AppointmentTypeSetup > 0 || AssignNoticeDays > 0 || BankSetup > 0 || ChangePassword > 0 || DesignationSetup > 0 || EmployeeNatureSetup > 0 || MakeUser > 0 || StaffTypeSetup > 0 || SubjectsSetup > 0 || YearSetup > 0)
                    {
                        liBasicSetUp.Visible = true;
                    }
                    else
                    {
                        liBasicSetUp.Visible = false;
                    }

                    if (DaSetup > 0 || HraSetUp > 0 || MedicalSetup > 0 || TransportSetUp > 0)
                    {
                        liAllowanceSetUp.Visible = true;
                    }
                    else
                    {
                        liAllowanceSetUp.Visible = false;
                    }

                    if (ESISetup > 0 || GISSetup > 0 || PFSetup > 0 || TransportRecoverySetUp > 0)
                    {
                        liDeductionsSetUp.Visible = true;
                    }
                    else
                    {
                        liDeductionsSetUp.Visible = false;
                    }
                }
                else
                {
                    liSetUp.Visible = false;
                }

                if (ImportPaidDays > 0 || ImportTDS > 0 || ImportBasicScale > 0 || ImportIDF > 0)
                {
                    liImportData.Visible = true;
                }
                else
                {
                    liImportData.Visible = false;
                }

                if (EmployeeSalaryProfile > 0 || UpdateEmpProfile > 0 || AdvanceMaking > 0 || GenerateSalarySlip > 0 || ReimbursementMaking > 0 || SalaryMaking > 0
                    || MidMonthIncrementSalaryMaking > 0 || SalaryApproval > 0 || FNFMaking > 0 || CalculateArrear > 0 || CalculateRecovery > 0 || ManageNoticePeriod > 0)
                {
                    liActions.Visible = true;

                    if (SalaryMaking > 0 || MidMonthIncrementSalaryMaking > 0 || SalaryApproval > 0)
                    {
                        liSalaryMakingMenu.Visible = true;
                    }
                    else
                    {
                        liSalaryMakingMenu.Visible = false;
                    }

                    if (EmployeeSalaryProfile > 0 || UpdateEmpProfile > 0)
                    {
                        liProfileMenu.Visible = true;
                    }
                    else
                    {
                        liProfileMenu.Visible = false;
                    }

                    if (AdvanceMaking > 0 || CalculateArrear > 0 || CalculateRecovery > 0 || ReimbursementMaking > 0)
                    {
                        liOtherAdjustments.Visible = true;
                    }
                    else
                    {
                        liOtherAdjustments.Visible = false;
                    }

                    if (FNFMaking > 0 || ManageNoticePeriod > 0)
                    {
                        liFNFAndNotice.Visible = true;
                    }
                    else
                    {
                        liFNFAndNotice.Visible = false;
                    }
                }
                else
                {
                    liActions.Visible = false;
                }

                if (DaSetupReport > 0 || ESISetupReport > 0 || GISSetupReport > 0 || HraSetUpReport > 0 || MedicalSetupReport > 0 || BankTransferReport > 0
                    || MonthlyESIReport > 0 || MonthlyPFReport > 0 || MonthlyReimbursementReport > 0 || CashEmpSalaryReport > 0 || ChequeEmpSalaryReport > 0
                    || MonthlySalaryReport > 0 || PFSetupReport > 0 || EmployeeProfileReport > 0 || TransportRecoverySetUpReport > 0 || TransportSetUpReport > 0
                    || ProfileModificationReport > 0 || EmpWiseSalaryReport > 0 || FNFReport > 0 || EmpWiseTDSReport > 0 || ArrearReport > 0 || RecoveryReport > 0 || NoticePeriodReport > 0)
                {
                    liReports.Visible = true;

                    if (DaSetupReport > 0 || HraSetUpReport > 0 || MedicalSetupReport > 0 || TransportSetUpReport > 0)
                    {
                        liAllowanceReport.Visible = true;
                    }
                    else
                    {
                        liAllowanceReport.Visible = false;
                    }

                    if (ESISetupReport > 0 || GISSetupReport > 0 || PFSetupReport > 0 || TransportRecoverySetUpReport > 0)
                    {
                        liDeductionsReports.Visible = true;
                    }
                    else
                    {
                        liDeductionsReports.Visible = false;
                    }

                    if (DaSetupReport > 0 || HraSetUpReport > 0 || MedicalSetupReport > 0 || TransportSetUpReport > 0 || ESISetupReport > 0 || GISSetupReport > 0 || PFSetupReport > 0 || TransportRecoverySetUpReport > 0)
                    {
                        liSetUpReports.Visible = true;
                    }
                    else
                    {
                        liSetUpReports.Visible = false;
                    }

                    if (BankTransferReport > 0 || MonthlyESIReport > 0 || MonthlyPFReport > 0 || MonthlyReimbursementReport > 0 || CashEmpSalaryReport > 0
                        || ChequeEmpSalaryReport > 0 || MonthlySalaryReport > 0 || EmployeeProfileReport > 0 || ProfileModificationReport > 0 || EmpWiseSalaryReport > 0
                        || FNFReport > 0 || EmpWiseTDSReport > 0 || ArrearReport > 0 || RecoveryReport > 0 || NoticePeriodReport > 0)
                    {
                        liActionsReports.Visible = true;

                        if (EmployeeProfileReport > 0 || ProfileModificationReport > 0)
                        {
                            liProfileMenuReport.Visible = true;
                        }
                        else
                        {
                            liProfileMenuReport.Visible = false;
                        }

                        if (BankTransferReport > 0 || MonthlyESIReport > 0 || MonthlyPFReport > 0 || MonthlyReimbursementReport > 0 || CashEmpSalaryReport > 0
                        || ChequeEmpSalaryReport > 0 || MonthlySalaryReport > 0 || ArrearReport > 0 || RecoveryReport > 0)
                        {
                            liMonthlyReports.Visible = true;
                        }
                        else
                        {
                            liMonthlyReports.Visible = false;
                        }

                        if (FNFReport > 0 || NoticePeriodReport > 0)
                        {
                            liFNFAndNoticeReports.Visible = true;
                        }
                        else
                        {
                            liFNFAndNoticeReports.Visible = false;
                        }

                        if (EmpWiseSalaryReport > 0 || EmpWiseTDSReport > 0)
                        {
                            liOtherReports.Visible = true;
                        }
                        else
                        {
                            liOtherReports.Visible = false;
                        }
                    }
                    else
                    {
                        liActionsReports.Visible = false;
                    }
                }
                else
                {
                    liReports.Visible = false;
                }

                if (TDSAdditionalAmountCaption > 0 || TDSAdditionalAmount > 0 || TDSForm16 > 0 || TDSHeaders > 0 || TDSHeaderwiseRules > 0 || TDSHeaderwiseFooter > 0
                    || TDSInvestmentDeclarationForm > 0 || PendingIDFReport > 0 || IncomeTaxRules > 0 || SurchargeRules > 0 || CalculateTax > 0 || IDFApproval > 0
                    || IDFDisapproval > 0 || TDSForm16AllEmp > 0 || DownloadIDF > 0)
                {
                    liTDS.Visible = true;

                    if (TDSAdditionalAmountCaption > 0 || TDSAdditionalAmount > 0 || TDSForm16 > 0 || TDSForm16AllEmp > 0)
                    {
                        liForm16.Visible = true;
                    }
                    else
                    {
                        liForm16.Visible = false;
                    }

                    if (TDSHeaders > 0 || TDSHeaderwiseRules > 0 || TDSHeaderwiseFooter > 0 || TDSInvestmentDeclarationForm > 0 || PendingIDFReport > 0 ||
                        IDFApproval > 0 || IDFDisapproval > 0 || DownloadIDF > 0)
                    {
                        liIDFForm.Visible = true;
                    }
                    else
                    {
                        liIDFForm.Visible = true;
                    }

                    if (AccommodationCriteria > 0 || StandardDeduction > 0 || IncomeTaxRules > 0 || SurchargeRules > 0)
                    {
                        liTDSSetUp.Visible = true;
                    }
                    else
                    {
                        liTDSSetUp.Visible = false;
                    }
                }
                else
                {
                    liTDS.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void lnklogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Response.Redirect("../Default.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}