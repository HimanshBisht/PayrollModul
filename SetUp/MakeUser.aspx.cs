using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public partial class SalaryModule_MakeUser : System.Web.UI.Page
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

    public enum Menu
    {
        SetUp = 1,
        ImportData = 2,
        Actions = 3,
        Reports = 4,
        TDS = 5
    }

    public enum MenuType
    {
        All = 0,
        SetUp = 1,
        ImportData = 2,
        Actions = 3,
        Reports = 4,
        TDS = 5
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
                    SetUpPages();
                    ImportDataPages();
                    ActionsPages();
                    ReportsPages();
                    TDSPages();
                    BindUsers();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                SetUpPages();
                ImportDataPages();
                ActionsPages();
                ReportsPages();
                TDSPages();
                BindUsers();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void SetUpPages()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowPages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageID", 0);
            cmd.Parameters.AddWithValue("@MenuID", Menu.SetUp);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            chkbxSetupPageList.DataSource = dt;
            chkbxSetupPageList.DataTextField = "PageTitle";
            chkbxSetupPageList.DataValueField = "PageID";
            chkbxSetupPageList.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ImportDataPages()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowPages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageID", 0);
            cmd.Parameters.AddWithValue("@MenuID", Menu.ImportData);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            chkbxImportDataPageList.DataSource = dt;
            chkbxImportDataPageList.DataTextField = "PageTitle";
            chkbxImportDataPageList.DataValueField = "PageID";
            chkbxImportDataPageList.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ActionsPages()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowPages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageID", 0);
            cmd.Parameters.AddWithValue("@MenuID", Menu.Actions);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            chkbxActionsPageList.DataSource = dt;
            chkbxActionsPageList.DataTextField = "PageTitle";
            chkbxActionsPageList.DataValueField = "PageID";
            chkbxActionsPageList.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ReportsPages()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowPages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageID", 0);
            cmd.Parameters.AddWithValue("@MenuID", Menu.Reports);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            chkbxReportsPageList.DataSource = dt;
            chkbxReportsPageList.DataTextField = "PageTitle";
            chkbxReportsPageList.DataValueField = "PageID";
            chkbxReportsPageList.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void TDSPages()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowPages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageID", 0);
            cmd.Parameters.AddWithValue("@MenuID", Menu.TDS);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            chkbxTDSPageList.DataSource = dt;
            chkbxTDSPageList.DataTextField = "PageTitle";
            chkbxTDSPageList.DataValueField = "PageID";
            chkbxTDSPageList.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void BindUsers()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetLoginDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", null);
            cmd.Parameters.AddWithValue("@Password", null);
            cmd.Parameters.AddWithValue("@ParentID", Session["LoginID"]);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlUsers.DataSource = dt;
            ddlUsers.DataTextField = "UserName";
            ddlUsers.DataValueField = "LoginID";
            ddlUsers.DataBind();
            ddlUsers.Items.Insert(0, new ListItem("Select User", "0"));
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
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            chkSetupAll.Checked = false;
            chkbxSetupPageList.ClearSelection();
            chkImportDataAll.Checked = false;
            chkbxImportDataPageList.ClearSelection();
            chkActionsAll.Checked = false;
            chkbxActionsPageList.ClearSelection();
            chkReportsAll.Checked = false;
            chkbxReportsPageList.ClearSelection();
            chkTDSAll.Checked = false;
            chkbxTDSPageList.ClearSelection();
            btnSave.Text = "Save User";
            ddlUsers.ClearSelection();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int CountSetupPageList = 0;
            foreach (ListItem item in chkbxSetupPageList.Items)
            {
                if (item.Selected == true)
                {
                    CountSetupPageList++;
                }
            }

            int CountImportDataPageList = 0;
            foreach (ListItem item in chkbxImportDataPageList.Items)
            {
                if (item.Selected == true)
                {
                    CountImportDataPageList++;
                }
            }

            int CountActionsPageList = 0;
            foreach (ListItem item in chkbxActionsPageList.Items)
            {
                if (item.Selected == true)
                {
                    CountActionsPageList++;
                }
            }

            int CountReportsPageList = 0;
            foreach (ListItem item in chkbxReportsPageList.Items)
            {
                if (item.Selected == true)
                {
                    CountReportsPageList++;
                }
            }

            int CountTDSPageList = 0;
            foreach (ListItem item in chkbxTDSPageList.Items)
            {
                if (item.Selected == true)
                {
                    CountTDSPageList++;
                }
            }

            if (CountSetupPageList > 0 || CountImportDataPageList > 0 || CountActionsPageList > 0 || CountReportsPageList > 0 || CountTDSPageList > 0)
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("SaveUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", Encrypt(txtPassword.Text.Trim()));
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (btnSave.Text == "Save User")
                {
                    cmd.Parameters.AddWithValue("@ParentID", Session["LoginID"]);
                    cmd.Parameters.AddWithValue("@LoginID", 0);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LoginID", ddlUsers.SelectedValue);
                    cmd.Parameters.AddWithValue("@Type", "Update");
                }
                con.Open();
                int Count = (int)cmd.ExecuteScalar();
                con.Close();

                if (btnSave.Text == "Save User")
                {
                    string id = cmd.Parameters["@id"].Value.ToString();

                    foreach (ListItem item in chkbxSetupPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.SetUp);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "SavePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxImportDataPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.ImportData);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "SavePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxActionsPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.Actions);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "SavePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxReportsPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.Reports);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "SavePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxTDSPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.TDS);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "SavePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                }
                else
                {
                    cmd = new SqlCommand("SaveUsers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", null);
                    cmd.Parameters.AddWithValue("@Password", null);
                    cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                    cmd.Parameters.AddWithValue("@LoginID", 0);
                    cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                    cmd.Parameters.AddWithValue("@MenuID", MenuType.All);
                    cmd.Parameters.AddWithValue("@PageID", 0);
                    cmd.Parameters.AddWithValue("@Type", "DeleteOldRecord");
                    con.Open();
                    Count = (int)cmd.ExecuteScalar();
                    con.Close();

                    foreach (ListItem item in chkbxSetupPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.SetUp);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "UpdatePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxImportDataPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.ImportData);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "UpdatePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxActionsPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.Actions);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "UpdatePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxReportsPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.Reports);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "UpdatePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }

                    foreach (ListItem item in chkbxTDSPageList.Items)
                    {
                        if (item.Selected == true)
                        {
                            cmd = new SqlCommand("SaveUsers", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserName", null);
                            cmd.Parameters.AddWithValue("@Password", null);
                            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                            cmd.Parameters.AddWithValue("@LoginID", 0);
                            cmd.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                            cmd.Parameters.AddWithValue("@MenuID", MenuType.TDS);
                            cmd.Parameters.AddWithValue("@PageID", item.Value);
                            cmd.Parameters.AddWithValue("@Type", "UpdatePages");
                            con.Open();
                            Count = (int)cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                }

                if (Count == 0)
                {
                    Clear();
                    BindUsers();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                }
                else if (Count == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + txtUserName.Text + " Already Exist , Please Try Again.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Please Select at least one page from the below page list.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlUsers.SelectedValue) > 0)
            {
                chkSetupAll.Checked = false;
                chkbxSetupPageList.ClearSelection();
                chkImportDataAll.Checked = false;
                chkbxImportDataPageList.ClearSelection();
                chkActionsAll.Checked = false;
                chkbxActionsPageList.ClearSelection();
                chkReportsAll.Checked = false;
                chkbxReportsPageList.ClearSelection();
                chkTDSAll.Checked = false;
                chkbxTDSPageList.ClearSelection();

                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("GetLoginDetails", con);
                cmd.Parameters.AddWithValue("@LoginID", ddlUsers.SelectedValue);
                cmd.Parameters.AddWithValue("@UserName", null);
                cmd.Parameters.AddWithValue("@Password", null);
                cmd.Parameters.AddWithValue("@MenuID", MenuType.All);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataSet ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtUserName.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                    txtPassword.Text = Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                    btnSave.Text = "Update User";
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (ListItem item in chkbxSetupPageList.Items)
                    {
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (item.Value == ds.Tables[1].Rows[i]["PageID"].ToString())
                            {
                                item.Selected = true;
                            }
                            i++;
                        }
                    }

                    foreach (ListItem item in chkbxImportDataPageList.Items)
                    {
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (item.Value == ds.Tables[1].Rows[i]["PageID"].ToString())
                            {
                                item.Selected = true;
                            }
                            i++;
                        }
                    }

                    foreach (ListItem item in chkbxActionsPageList.Items)
                    {
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (item.Value == ds.Tables[1].Rows[i]["PageID"].ToString())
                            {
                                item.Selected = true;
                            }
                            i++;
                        }
                    }

                    foreach (ListItem item in chkbxReportsPageList.Items)
                    {
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (item.Value == ds.Tables[1].Rows[i]["PageID"].ToString())
                            {
                                item.Selected = true;
                            }
                            i++;
                        }
                    }

                    foreach (ListItem item in chkbxTDSPageList.Items)
                    {
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (item.Value == ds.Tables[1].Rows[i]["PageID"].ToString())
                            {
                                item.Selected = true;
                            }
                            i++;
                        }
                    }
                }
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

    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("SaveUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", null);
            cmd.Parameters.AddWithValue("@Password", null);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@LoginID", ddlUsers.SelectedValue);
            cmd.Parameters.AddWithValue("@id", 0);
            cmd.Parameters.AddWithValue("@Type", "Deactivate");
            con.Open();
            int Count = (int)cmd.ExecuteScalar();
            con.Close();
            if (Count == 0)
            {
                Clear();
                BindUsers();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivated Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Deactivating.');", true);
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
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkSetupAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSetupAll.Checked == true)
            {
                foreach (ListItem item in chkbxSetupPageList.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkbxSetupPageList.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkbxSetupPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Count = 0;
            foreach (ListItem item in chkbxSetupPageList.Items)
            {
                if (item.Selected == false)
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                chkSetupAll.Checked = false;
            }
            else
            {
                chkSetupAll.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkImportDataAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkImportDataAll.Checked == true)
            {
                foreach (ListItem item in chkbxImportDataPageList.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkbxImportDataPageList.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkbxImportDataPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Count = 0;
            foreach (ListItem item in chkbxImportDataPageList.Items)
            {
                if (item.Selected == false)
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                chkImportDataAll.Checked = false;
            }
            else
            {
                chkImportDataAll.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkActionsAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkActionsAll.Checked == true)
            {
                foreach (ListItem item in chkbxActionsPageList.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkbxActionsPageList.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkbxActionsPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Count = 0;
            foreach (ListItem item in chkbxActionsPageList.Items)
            {
                if (item.Selected == false)
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                chkActionsAll.Checked = false;
            }
            else
            {
                chkActionsAll.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkReportsAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkReportsAll.Checked == true)
            {
                foreach (ListItem item in chkbxReportsPageList.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkbxReportsPageList.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkbxReportsPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Count = 0;
            foreach (ListItem item in chkbxReportsPageList.Items)
            {
                if (item.Selected == false)
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                chkReportsAll.Checked = false;
            }
            else
            {
                chkReportsAll.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkTDSAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkTDSAll.Checked == true)
            {
                foreach (ListItem item in chkbxTDSPageList.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chkbxTDSPageList.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void chkbxTDSPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Count = 0;
            foreach (ListItem item in chkbxTDSPageList.Items)
            {
                if (item.Selected == false)
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                chkTDSAll.Checked = false;
            }
            else
            {
                chkTDSAll.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}