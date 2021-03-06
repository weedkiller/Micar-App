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
    public partial class frmDailyCheckoutReport : System.Web.UI.Page
    {
        public string varPageName;
        public string connectionString = "";
        string varName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //connectionString = WebConfigurationManager.ConnectionStrings["FleetConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(connectionString);

            try
            {
                HttpCookie cookie = Request.Cookies["UserName"];

           
            if (cookie != null)
            {
                varName = cookie["varUserName"];
                txtUserLabel.Text = varName;
            }
            else
            {
                txtUserLabel.Text = "No cookies found";
            }


            varPageName = "Daily Transport Checkout Report";
            //**********************************************************
            // Create the Command.
            AXcontrol DB = new AXcontrol();

            //**********************************************************
            // Create the Command.
            //string insertSQL = "select * from tb_RoleUser where userid=@userid";

            //SqlCommand cmd = new SqlCommand(insertSQL, con);
            //cmd.Parameters.AddWithValue("@userid", txtUserLabel.Text);
            //SqlDataReader reader;
            //con.Open();
            //reader = cmd.ExecuteReader();
            //reader.Read();
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
                ds = DB.FindRoleMenus(varRole, varPageName);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblResults.Text = "No permissions found for role " + varRole;
                }
                else
                {
                    string varMenu = "";
                    //string varMenu = DB.FindRoleMenusCreate(varRole, varPageName);
                    //if (varMenu == "false")
                    //{
                    //    cmdAdd.Visible = false;
                    //}
                    //else
                    //{
                    //    cmdAdd.Visible = true;
                    //}
                    varMenu = DB.FindRoleMenusRead(varRole, varPageName);
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
                log.Source = "Milorry Transport Management System";
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
            clsDailyCheckoutReport DB = new clsDailyCheckoutReport();
            try
            {

                if (cboRegno1.Text == null)
                {
                    lblResults.Text = "Please select the first registration number";
                    return;
                }
                if (cboRegno2.Text == null)
                {
                    lblResults.Text = "Please select the second registration number";
                    return;
                }
                if (cboDate1.Text == null)
                {
                    lblResults.Text = "Please select the first date";
                    return;
                }
                if (cboDate2.Text == null)
                {
                    lblResults.Text = "Please select second date";
                    return;
                }
                bool varResponse = DB.Find_rec(cboRegno1.Text, cboRegno2.Text, DateTime.Parse(cboDate1.Text), DateTime.Parse(cboDate2.Text));
                if (varResponse == true)
                {
                    Response.Redirect("frmDisplayDailyCheckoutReport.aspx");
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
                log.Source = "Milorry Transport Management System";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
            }
        }
        }
    }
