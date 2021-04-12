using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class SalaryModule_TransportRecoverySetUp : System.Web.UI.Page
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
                    Bindgrid();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Bindgrid();
            }
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
            cmd = new SqlCommand("ShowTransportRecoverySetup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            grdrecord.DataSource = dt;
            grdrecord.DataBind();
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
            txtRecAmount.Text = string.Empty;
            ViewState["RecoveryID"] = null;
            pnlData.Visible = false;
            pnlDetail.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
   
    public enum Status
    {
        Active = 1,
        Deactive = 2,
        All = 0
    }

    public enum BusUser
    {
        Yes = 1,
        No = 2,
        All = 0
    }

    public enum NatureOfEmp
    {
        Confirm = 1,
        Probation = 2,
        Contract = 3,
        All = 0
    }

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlTransaction trans = con.BeginTransaction();

        try
        {
            int HasUpdateHRA = 0;
            int HasUpdateProfile = 0;
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];

            cmd = new SqlCommand("SaveTransportRecoverySetup", con, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RecoveryAmount", txtRecAmount.Text);
            cmd.Parameters.AddWithValue("@RecoveryID", Convert.ToInt32(ViewState["RecoveryID"]));
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@Type", "Save");
            HasUpdateHRA = cmd.ExecuteNonQuery();

            if (HasUpdateHRA > 0)
            {
                decimal RecoveryAmount = Convert.ToDecimal(txtRecAmount.Text);
                int i = 0;

                SqlCommand cmdProfile = new SqlCommand("ShowEmpSalaryProfile", con, trans);
                cmdProfile.Parameters.AddWithValue("@IsActive", Status.Active);
                cmdProfile.Parameters.AddWithValue("@EmpNature", NatureOfEmp.Contract);
                cmdProfile.Parameters.AddWithValue("@BusUser", BusUser.Yes);
                cmdProfile.CommandType = CommandType.StoredProcedure;
                DataTable dtProfile = new DataTable();
                SqlDataAdapter daProfile = new SqlDataAdapter(cmdProfile);
                daProfile.Fill(dtProfile);

                if (dtProfile.Rows.Count > 0)
                {
                    foreach (DataRow row in dtProfile.Rows)
                    {
                        decimal BasicScale = 0;
                        decimal DaValue = 0;
                        decimal TransportValue = 0;
                        decimal PFValue = 0;
                        decimal ESIValue = 0;
                        decimal TransportRecovery = 0;
                        decimal GrossAllowance = 0;
                        decimal GrossDeduction = 0;
                        decimal GrossTotal = 0;
                        decimal NetSalary = 0;
                        string ProfileID = "0";
                        decimal HRAValue = 0;
                        decimal MedicalValue = 0;
                        decimal ExGratiaValue = 0;
                        decimal WashingValue = 0;
                        decimal GISValue = 0;

                        ProfileID = dtProfile.Rows[i]["ProfileID"].ToString();
                        BasicScale = Convert.ToDecimal(dtProfile.Rows[i]["BasicScale"].ToString());
                        DaValue = Convert.ToDecimal(dtProfile.Rows[i]["DaValue"].ToString());
                        HRAValue = Convert.ToDecimal(dtProfile.Rows[i]["HRAValue"].ToString());
                        MedicalValue = Convert.ToDecimal(dtProfile.Rows[i]["MedicalValue"].ToString());
                        TransportValue = Convert.ToDecimal(dtProfile.Rows[i]["TransportValue"].ToString());
                        WashingValue = Convert.ToDecimal(dtProfile.Rows[i]["WashingValue"].ToString());
                        ExGratiaValue = Convert.ToDecimal(dtProfile.Rows[i]["ExGratiaValue"].ToString());

                        GrossAllowance = Converter(DaValue + HRAValue + MedicalValue + TransportValue + WashingValue + ExGratiaValue);
                        GrossTotal = Converter(BasicScale + DaValue + HRAValue + MedicalValue + TransportValue + WashingValue + ExGratiaValue);

                        PFValue = Convert.ToDecimal(dtProfile.Rows[i]["PFValue"].ToString());
                        ESIValue = Convert.ToDecimal(dtProfile.Rows[i]["EsiValue"].ToString());
                        GISValue = Convert.ToDecimal(dtProfile.Rows[i]["GisValue"].ToString());
                        TransportRecovery = RecoveryAmount;

                        GrossDeduction = Converter(PFValue + ESIValue + GISValue + TransportRecovery);
                        NetSalary = Converter(GrossTotal - GrossDeduction);

                        SqlCommand cmdSaveProfile = new SqlCommand("SaveTransportRecoverySetup", con, trans);
                        cmdSaveProfile.CommandType = CommandType.StoredProcedure;
                        cmdSaveProfile.Parameters.AddWithValue("@ProfileID", ProfileID);
                        cmdSaveProfile.Parameters.AddWithValue("@RecoveryAmount", TransportRecovery);
                        cmdSaveProfile.Parameters.AddWithValue("@GrossDeduction", GrossDeduction);
                        cmdSaveProfile.Parameters.AddWithValue("@NetSalary", NetSalary);
                        cmdSaveProfile.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                        cmdSaveProfile.Parameters.AddWithValue("@Type", "Update");
                        HasUpdateProfile = cmdSaveProfile.ExecuteNonQuery();
                        i++;
                    }
                }

                if (HasUpdateHRA > 0 || HasUpdateProfile > 0)
                {
                    trans.Commit();
                    con.Close();
                    Bindgrid();
                    Clear();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Update Sucessfully.');", true);
                }
                else
                {
                    trans.Rollback();
                    con.Close();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Saving Data.');", true);
                }
            }
            else
            {
                trans.Rollback();
                con.Close();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Modification Appear, Please do some changes to Update, Else Cancel');", true);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            con.Close();
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkEdit")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowTransportRecoverySetup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecoveryID", e.CommandArgument);
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                txtRecAmount.Text = dt.Rows[0]["RecoveryAmount"].ToString();
                ViewState["RecoveryID"] = e.CommandArgument;
                pnlData.Visible = true;
                pnlDetail.Visible = false;
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
            Bindgrid();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

}