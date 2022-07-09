using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BizzManWebErp
{
    public partial class wfAdminLogin : System.Web.UI.Page
    {     
        clsMain objMain;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            objMain = new clsMain();
        }
        protected void btn_login_Click(object sender, EventArgs e)
        {
            
            messagelbl.Text = "";
            string strSql;
            strSql = "SELECT * FROM tblUserMaster WHERE UserName='" + txtuser.Text + "' AND Password ='" + txtpassword.Text + "'";
            if(objMain.blSearchDataHO(strSql))
            {
                messagelbl.Visible = true;
                Session["Id"] = txtuser.Text;
                messagelbl.Text = "Login Successfully !";
                messagelbl.ForeColor = System.Drawing.Color.Green;
                Response.Redirect("wfErpMain.aspx");
            }
            else
            {
                messagelbl.Visible = true;
                messagelbl.Text = "Wrong Email or Password!";

            }

        }
    }
}