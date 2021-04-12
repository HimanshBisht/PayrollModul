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

public partial class SalaryModule_ImportBasicScale : System.Web.UI.Page
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
                    BindStaffType();
                    BindEmpNature();
                    BindAppointment();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                BindStaffType();
                BindEmpNature();
                BindAppointment();
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
            ddlStaffType.Items.Insert(0, new ListItem("All Staff Type", "0"));
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
            ddlNatureOfEmp.Items.Insert(0, new ListItem("All Nature Type", "0"));
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
            ddlAppointment.Items.Insert(0, new ListItem("All Appointment Type", "0"));
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
            ddlAppointment.ClearSelection();
            ddlNatureOfEmp.ClearSelection();
            ddlStaffType.ClearSelection();
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

    private void ExportGridToExcel()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Bulk_Update_Basic_Scale_FDB.xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;
                int Count = 0;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    if (dtcol.ColumnName == "ProfileID" || dtcol.ColumnName == "Emp_Code" || dtcol.ColumnName == "AssignEmpCode" || dtcol.ColumnName == "Name" || dtcol.ColumnName == "Designation" || dtcol.ColumnName == "NatureOfEmp" || dtcol.ColumnName == "Appointment" || dtcol.ColumnName == "StaffType" || dtcol.ColumnName == "BasicScale")
                    {
                        Response.Write(str + dtcol.ColumnName);
                        str = "\t";
                        Count++;
                    }
                    else
                    {
                        if (Count == 9)
                        {
                            break;
                        }
                        else
                        {
                            continue;
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
                        if (j == 0 || j == 2 || j == 4 || j == 5 || j == 8 || j == 16 || j == 20 || j == 14 || j == 22)
                        {
                            Response.Write(str + Convert.ToString(dr[j]));
                            str = "\t";
                        }
                    }

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
            if (flImportBasicScale.HasFile)
            {
                string FileName = "Bulk_Update_Basic_Scale_FDB" + Path.GetFileName(flImportBasicScale.PostedFile.FileName);
                string Extension = Path.GetExtension(flImportBasicScale.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = Server.MapPath(FolderPath + FileName);
                flImportBasicScale.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select File.');", true);
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

    private void Import_To_Grid(string FilePath, string Extension)
    {
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
            int HasRow = 0;

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];

            foreach (DataRow row in dt.Rows)
            {
                decimal BasicScale = 0;
                string ProfileID = dt.Rows[i]["ProfileID"].ToString();

                if (dt.Rows[i]["BasicScale"].ToString().Length > 0)
                {
                    BasicScale = Convert.ToDecimal(dt.Rows[i]["BasicScale"].ToString());
                }

                SqlCommand cmdProfile = new SqlCommand("ShowEmpSalaryProfile", con);
                cmdProfile.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmdProfile.Parameters.AddWithValue("@IsActive", Status.Active);
                cmdProfile.CommandType = CommandType.StoredProcedure;

                DataTable dtProfile = new DataTable();
                SqlDataAdapter daProfile = new SqlDataAdapter(cmdProfile);
                daProfile.Fill(dtProfile);

                if (dtProfile.Rows.Count > 0)
                {
                    string DaAllow = "0";
                    decimal DA = 0;
                    decimal DaValue = 0;
                    string HraAllow = "0";
                    decimal HRA = 0;
                    decimal HRAValue = 0;
                    string TransportAllow = "0";
                    decimal GradePay = 0;
                    decimal TransportValue = 0;
                    decimal TotalTransport = 0;
                    string PFDeduct = "0";
                    decimal PFValue = 0;
                    string ESIDeduct = "0";
                    decimal ESIValue = 0;
                    decimal TransportRecovery = 0;
                    decimal GrossAllowance = 0;
                    decimal GrossDeduction = 0;
                    decimal GrossTotal = 0;
                    decimal NetSalary = 0;
                    decimal MedicalValue = 0;
                    decimal ExGratiaValue = 0;
                    decimal WashingValue = 0;
                    string BusUser = "0";
                    string NatureOfEmp = "0";
                    decimal GISValue = 0;
                    decimal GetPFValue = 0;
                    decimal GetPFMaxRange = 0;
                    decimal GetBasicPlusDAForPF = 0;
                    decimal GetESIValue = 0;

                    DaAllow = dtProfile.Rows[0]["DaAllow"].ToString();

                    if (DaAllow == "1")
                    {
                        DA = Convert.ToDecimal(dtProfile.Rows[0]["DA"].ToString());
                        DaValue = Converter((BasicScale * DA) / 100);
                    }

                    HraAllow = dtProfile.Rows[0]["HraAllow"].ToString();

                    if (HraAllow == "1")
                    {
                        HRA = Convert.ToDecimal(dtProfile.Rows[0]["HRA"].ToString());
                        HRAValue = Converter((BasicScale * HRA) / 100);
                    }
                    else if (HraAllow == "3")
                    {
                        HRAValue = Convert.ToDecimal(dtProfile.Rows[0]["HraValue"].ToString());
                    }

                    GradePay = Convert.ToDecimal(dtProfile.Rows[0]["GradePay"].ToString());
                    TransportAllow = dtProfile.Rows[0]["TransportAllow"].ToString();

                    SqlCommand cmdGetValues = new SqlCommand("GetSetUpDetails", con);
                    cmdGetValues.Parameters.AddWithValue("@GradePay", GradePay);
                    cmdGetValues.CommandType = CommandType.StoredProcedure;
                    DataSet dsGetValues = new DataSet();
                    SqlDataAdapter daGetValues = new SqlDataAdapter(cmdGetValues);
                    daGetValues.Fill(dsGetValues);

                    if (TransportAllow == "1")
                    {
                        if (GradePay > 0)
                        {
                            TransportValue = Convert.ToDecimal(dsGetValues.Tables[3].Rows[0]["TransportValue"]);
                            TotalTransport = (TransportValue + ((TransportValue * DA) / 100));
                        }
                    }

                    MedicalValue = Convert.ToDecimal(dtProfile.Rows[0]["MedicalValue"].ToString());
                    PFDeduct = dtProfile.Rows[0]["PFDeduct"].ToString();

                    if (PFDeduct == "1" || PFDeduct == "3")
                    {
                        GetBasicPlusDAForPF = BasicScale + DaValue;
                        GetPFMaxRange = Convert.ToDecimal(dsGetValues.Tables[5].Rows[0]["MaxRange"].ToString());
                        GetPFValue = Convert.ToDecimal(dsGetValues.Tables[5].Rows[0]["PF"].ToString());

                        if (PFDeduct == "1")
                        {
                            if (GetBasicPlusDAForPF >= GetPFMaxRange)
                            {
                                PFValue = Converter((GetPFMaxRange * GetPFValue) / 100);
                            }
                            else if (GetBasicPlusDAForPF < GetPFMaxRange)
                            {
                                PFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                            }
                        }
                        else if (PFDeduct == "3")
                        {
                            PFValue = Converter((GetBasicPlusDAForPF * GetPFValue) / 100);
                        }
                    }

                    ESIDeduct = dtProfile.Rows[0]["ESIDeduct"].ToString();

                    if (ESIDeduct == "1")
                    {
                        GetESIValue = Convert.ToDecimal(dsGetValues.Tables[4].Rows[0]["ESI"].ToString());
                        ESIValue = Math.Ceiling((GrossTotal * GetESIValue) / 100);
                    }

                    WashingValue = Convert.ToDecimal(dtProfile.Rows[0]["WashingValue"].ToString());
                    ExGratiaValue = Convert.ToDecimal(dtProfile.Rows[0]["ExGratiaValue"].ToString());

                    GrossAllowance = Converter(DaValue + HRAValue + MedicalValue + TotalTransport + WashingValue + ExGratiaValue);
                    GrossTotal = Converter(BasicScale + DaValue + HRAValue + MedicalValue + TotalTransport + WashingValue + ExGratiaValue);

                    BusUser = dtProfile.Rows[0]["BusUser"].ToString();
                    NatureOfEmp = dtProfile.Rows[0]["NatureOfEmp"].ToString();

                    if (BusUser == "2" || BusUser == "3")
                    {
                        TransportRecovery = 0;
                    }
                    else if (BusUser == "1" && NatureOfEmp == "3")
                    {
                        TransportRecovery = Convert.ToDecimal(dsGetValues.Tables[6].Rows[0]["RecoveryAmount"].ToString());
                    }
                    else if (BusUser == "1" && (NatureOfEmp == "1" || NatureOfEmp == "2"))
                    {
                        TransportRecovery = TotalTransport;
                    }

                    GISValue = Convert.ToDecimal(dtProfile.Rows[0]["GisValue"].ToString());

                    GrossDeduction = Converter(PFValue + ESIValue + GISValue + TransportRecovery);
                    NetSalary = Converter(GrossTotal - GrossDeduction);

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
                    cmd.Parameters.AddWithValue("@BasicScale", BasicScale);
                    cmd.Parameters.AddWithValue("@DaValue", DaValue);
                    cmd.Parameters.AddWithValue("@HraValue", HRAValue);
                    cmd.Parameters.AddWithValue("@TransportValue", TotalTransport);
                    cmd.Parameters.AddWithValue("@PFValue", PFValue);
                    cmd.Parameters.AddWithValue("@EsiValue", ESIValue);
                    cmd.Parameters.AddWithValue("@TransportRecovery", TransportRecovery);
                    cmd.Parameters.AddWithValue("@GrossAllowance", GrossAllowance);
                    cmd.Parameters.AddWithValue("@GrossTotal", GrossTotal);
                    cmd.Parameters.AddWithValue("@GrossDeduction", GrossDeduction);
                    cmd.Parameters.AddWithValue("@NetSalary", NetSalary);
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                    cmd.Parameters.AddWithValue("@Type", "BulkUpdateSalary");
                    HasRow = HasRow + cmd.ExecuteNonQuery();
                }

                i++;
            }

            con.Close();

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            if (HasRow > 0)
            {
                Clear();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Update Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found to Update.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}