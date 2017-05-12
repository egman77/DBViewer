using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;

namespace DBViewer.Model.MySql
{
    public partial class ConnectionConfig : UserControl
    {
        private DBViewerConfig config;

        public DBViewerConfig Config
        {
            get {
                config.Server = this.serverName.Text;
                config.User = this.user.Text;
                config.Password = this.password.Text;
                config.DbName = this.dbname.Text;
                return config; }
            set
            {
                config = value;
                if (config != null)
                {
                    this.serverName.Text = config.Server;
                    this.user.Text = config.User;
                    this.password.Text = config.Password;
                    this.dbname.Text = config.DbName;

                }
            }
        }
        public ConnectionConfig()
        {
            InitializeComponent();
        }
    }
}
