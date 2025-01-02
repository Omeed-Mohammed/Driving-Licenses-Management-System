﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_License.Licenses.Local_Licenses
{
    public partial class frmShowLicenseInfo : Form
    {
        private int _licenseID;
        public frmShowLicenseInfo(int licenseID)
        {
            InitializeComponent();
            _licenseID = licenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadInfo(_licenseID);
        }
    }
}
