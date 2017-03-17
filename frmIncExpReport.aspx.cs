﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using Transport;
using System.Configuration.Assemblies;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;


namespace MiCar
{
    public partial class frmIncExpReport : System.Web.UI.Page
    {
        public string varPageName1;
        public string connectionString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //connectionString = WebConfigurationManager.ConnectionStrings["FleetConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(connectionString);

            try
            {
                HttpCookie cookie = Request.Cookies["UserName"];

                string varName = "";
                if (cookie != null)
                {
                    varName = cookie["varUserName"];
                    txtUserLabel.Text = varName;
                }
                else
                {
                    txtUserLabel.Text = "No cookies found";
                }


                varPageName1 = "Produce Income and Expense Report";
                //**********************************************************
                // Create the Command.
                AXcontrol DB = new AXcontrol();
                string varUserPresence = DB.FindUserPresence(txtUserLabel.Text);

                if (varUserPresence.Length > 0)
                {
                    string varRole = varUserPresence.ToString();  // (string)reader["role"];

                    //string query = "SELECT * FROM operator_permission where role=@role and menus=@menus";
                    //SqlConnection con1 = new SqlConnection(connectionString1);
                    //SqlCommand cmd1 = new SqlCommand(query, con1);
                    //cmd1.Parameters.AddWithValue("@role", varRole);
                    //cmd1.Parameters.AddWithValue("@menus", varPageName1);
                    //SqlDataReader readerPermissions;
                    //con1.Open();
                    //readerPermissions = cmd1.ExecuteReader();
                    //readerPermissions.Read();


                    DataSet ds = new DataSet();
                    ds = DB.FindRoleMenus(varRole, varPageName1);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        lblResults.Text = "No permissions found for role " + varRole;
                    }
                    else
                    {
                        //string varMenu = DB.FindRoleMenusCreate(varRole, varPageName);
                        //if (varMenu == "false")
                        //{
                        //    cmdAdd.Visible = false;
                        //}
                        //else
                        //{
                        //    cmdAdd.Visible = true;
                        //}
                        string varMenu = DB.FindRoleMenusRead(varRole, varPageName1);
                        if (varMenu == "false")
                        {
                            cmdReport.Visible = false;
                        }
                        else
                        {
                            cmdReport.Visible = true;
                        }
                        //varMenu = DB.FindRoleMenusUpdate(varRole, varPageName);
                        //if (varMenu == "false")
                        //{
                        //    cmdEdit.Visible = false;
                        //}
                        //else
                        //{
                        //    cmdEdit.Visible = true;
                        //}
                        //varMenu = DB.FindRoleMenusDelete(varRole, varPageName);
                        //if (varMenu == "false")
                        //{
                        //    cmdDelete.Visible = false;
                        //}
                        //else
                        //{
                        //    cmdDelete.Visible = true;
                        //}
                    }
                }
            }
            catch (Exception err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
                return;
            }
            finally
            {
                //con.Close();
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            clsIncExpReport DB = new clsIncExpReport();

            try
            {

                if (cboRegNo.Text == null)
                {
                    lblResults.Text = "Please select the registration number";
                    return;
                }
                if (cboStartDate.GetType()!=typeof(DateTime))
                {
                    lblResults.Text = "Please select the first date";
                    return;
                }
                if (cboEndDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Please select the second date";
                    return;
                }
                bool varResponse = DB.Find_rec(cboRegNo.Text, DateTime.Parse(cboStartDate.Text),DateTime.Parse(cboEndDate.Text) );
                if (varResponse == true)
                {
                    Response.Redirect("frmDisplayIncExpReport.aspx");
                }
                else
                {
                    lblResults.Text = "No transactions found";
                }
                return;
            }
            catch (FormatException err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
            }
        }
    }
}