using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;

namespace DBViewer.Model.SQLServer
{
    public partial class ConnectionConfig : UserControl
    {
        private DBViewerConfig config;

        public DBViewerConfig Config
        {
            get {
                config.server = this.serverName.Text;
                config.user = this.user.Text;
                config.password = this.password.Text;
                config.dbname = this.dbname.Text;
                return config; }
            set
            {
                config = value;
                if (config != null)
                {
                    this.serverName.Text = config.server;
                    this.user.Text = config.user;
                    this.password.Text = config.password;
                    this.dbname.Text = config.dbname;

                }
            }
        }
        public ConnectionConfig()
        {
            InitializeComponent();
        }
    }
}
