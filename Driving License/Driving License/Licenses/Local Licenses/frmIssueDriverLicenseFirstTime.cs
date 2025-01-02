using Driving_License.Global_Classes;
using Driving_License_Business;
using System;
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
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;


        public frmIssueDriverLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication == null)
            {
                string massage = "No Application with ID = ";
                MessageBox.Show(massage + _LocalDrivingLicenseApplicationID , "Not Allowed", MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(!_LocalDrivingLicenseApplication.PassedAllTests())
            {
                string massage = "Person Should Pass All Tests First.";
                MessageBox.Show(massage, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            if(LicenseID != -1)
            {
                string massage = "Person already has License before with License ID=";
                MessageBox.Show(massage + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            string Notes = txtNotes.Text.Trim();
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirstTime(Notes,clsGlobal.CurrentUser.UserID);
            if (LicenseID != -1) 
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
