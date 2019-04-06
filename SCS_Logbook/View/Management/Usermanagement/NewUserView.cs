﻿using log4net;
using SCS_Logbook.MySql;
using SCS_Logbook.Objects;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace SCS_Logbook.View.Management.Usermanagement
{
    public partial class NewUserView : AddView<User>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NewUserView()
        {
            InitializeComponent();
        }

        private void tb_username_TextChanged(object sender, EventArgs e)
        {
            if(!MySqlConnector.Instance.GetDbContext().Users.Where(user => user.Username.Equals(tb_username.Text)).Any())
            {
                tb_username.BackColor = Color.LightGreen;
                btn_create.Enabled = true;
            }
            else
            {
                tb_username.BackColor = Color.Red;
                btn_create.Enabled = false;
            }
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            MySqlConnector.Instance.BeginTransaction();
            try { 
                User newUser = new User
                {
                    Username = tb_username.Text,
                    Password = tb_password.Text
                };

                MySqlConnector.Instance.GetDbContext().Users.Add(newUser);
                MySqlConnector.Instance.GetDbContext().SaveChanges();
                MySqlConnector.Instance.EndTransaction();   
                Logbook.Instance.closeView(GetType());
            }
            catch(Exception ex)
            {
                log.Error("Could not create a new user.", ex);
                MySqlConnector.Instance.RollbackTransaction();
            }            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Logbook.Instance.closeView(GetType());
        }
    }
}
