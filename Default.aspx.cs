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
using System.Net;
using System.Text;
using System.Security.Cryptography;

public partial class _Default : System.Web.UI.Page
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

    public DataTable CheckUser(string Username, string Password)
    {
        SqlConnection con = new SqlConnection(constr);
        cmd = new SqlCommand("GetLoginDetails", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserName", Username);
        cmd.Parameters.AddWithValue("@Password", Password);
        con.Open();
        DataTable dt = new DataTable();
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();
        return dt;
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

    protected void ValidateCaptcha(object sender, ServerValidateEventArgs e)
    {
        try
        {
           // Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());
           // e.IsValid = Captcha1.UserValidated;
           // if (e.IsValid)
            //{
                DataTable dt = new DataTable();
                dt = CheckUser(txt_login.Text.Trim(), Encrypt(txt_pass.Text.Trim()));
                if (dt.Rows.Count > 0)
                {
                    hash.Add("Name", dt.Rows[0]["UserName"].ToString());
                    Session["User"] = hash;
                    Session["LoginID"] = dt.Rows[0]["LoginID"].ToString();
                    Session["LastLogin"] = dt.Rows[0]["LastLogin"].ToString();
                    Session["SchoolName"] = "K. R. Mangalam World School, Faridabad ";
                    Session["SchoolPrefix"] = "_FDB";
                    Response.Redirect("Home/Home.aspx", false);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Invalid User, Please Try Again')", true);
                    txtCaptcha.Text = string.Empty;
                }
           // }
           // else
           // {
           //     txtCaptcha.Text = string.Empty;
           //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}