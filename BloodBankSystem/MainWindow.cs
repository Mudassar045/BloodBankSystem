using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using MetroFramework;
using System.Drawing;
using System.Data;
using System.Threading;

namespace BloodBankSystem
{

    public partial class MainWindow : Form
    {
        private int mIsPressedLoginWindowButton = 0;
        private int mIsPressedRegisterWindowButton = 0;
        // Donor Menu Buttons
        private int mIsPressedQSearchDonorButton = 0;
        private int mIsPressedAddDonorButton = 0;
        private int mIsPressedViewDonorButton = 0;
        // Patient Menu Buttons
        private int mIsPressedViewPatientButton = 0;
        private int mIsPressedAddPatientButton = 0;
        private int mIsPressedQSearchPatientButton = 0;
        // Blood Stock Menu Button
        private int mIsPressedSearchBloodButton = 0;
        private int mIsPressedAddBloodBagButton = 0;
        private int mIsPressedBloodStatusButton = 0;
        // Setting Menu Button
        private int mIsPressedSettingsButton = 0;

        private bool mIsAuthorizedUser = false;
        bool mIsSearched = false; // Used in _SearchTextButtonPressed
        // Setting Menu Window
        private bool IS_OPENED_SETTINGS_WINDOW = false;
        // Donor Menu Windows
        private bool IS_OPENED_ADD_DONOR_WINDOW = false;
        private bool IS_OPENED_VIEW_DONOR_WINDOW = false;
        private bool IS_OPENED_REMOVE_DONOR_WINDOW = false;
        private bool IS_OPENED_QSEARCH_DONOR_WINDOW = false;
        //Patient Menu Windows
        private bool IS_OPENED_VIEW_PATIENT_WINDOW = false;
        private bool IS_OPENED_ADD_PATIENT_WINDOW = false;
        private bool IS_OPENED_QSEARCH_PATIENT_WINDOW = false;
        //BloodStock Menu Windows
        private bool IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
        private bool IS_OPENED_ADD_BLOODBAG_WINDOW = false;
        private bool IS_OPENED_BLOODGROUP_STATUS_WINDOW = false;
        //Search Windows Panel
        private bool IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
        private bool IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;

        /***************************************************************************
         ****************** DATABASE CONNECTION AND DATA GRID TOOLS*****************/
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB
        ;AttachDbFilename=|DataDirectory|\bbmsdatabase.mdf;Integrated Security=True
        ;Connect Timeout=30");
        SqlDataAdapter adap = new SqlDataAdapter();
        DataSet mDataSet = new DataSet();
        DataSet mSearchDataSet = new DataSet();
        DataSet mSearchBGDataSet = new DataSet();
        BindingSource mBindingSource = new BindingSource();
        private int mIndex;
        private string mSearchByText;
        private string mSearchQuery;
        private void StartDatabase()
        {
            con.Open();
            string str = con.State.ToString();
            con.Close();
        }
        // Class_Constractor
        public MainWindow()
        {
            InitializeComponent();
            StartDatabase();
        }
        /**************************************************************
        *************** MAIN FORM CONTROLS IMPLEMENTATION *************
        ***************************************************************/
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x20000;

                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                return cp;
            }
        }


        private void _CloseButton_Click(object sender, EventArgs e)
        {
            /* if (MetroMessageBox.Show(this, "Do you want to exit application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, 120) == DialogResult.Yes)
             {
                 Application.Exit();
             }*/
            Application.Exit();
        }
        private void _MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void _MakeFormMoveable(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        // FORM_LOAD_EVENT
        private void MainWindow_Load(object sender, EventArgs e)
        {
            _LoginWindowPanel.Hide();
            _RegisterWindowPanel.Hide();
            _HomeWindowPanel.Hide();
            //Home Page Panel 
            _AddDonorWindowPanel.Hide();
            _AddPatientWindowPanel.Hide();
            _UpdateWindowPanel.Hide();
            _QuickSearchWindowPanel.Hide();
            _SearchBloodGroupWindowPanel.Hide();
            _AddBloodWindowPanel.Hide();
            _BloodStatusWindowPanel.Hide();
            _SettingsWindowPanel.Hide();

            //Panel on Quic Search Panel
            _SearchBySelectWindowPanel.Hide();

            tbUserName.Text = "Mudi045";
            tbPassword.Text = "Mentor045";
            CustomDataGridView(); // private function
            _CustomMenuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomMenuStripColor());
        }
        /*********************************************************************************************/

        /***********************************************************
         ********** LOGIN CONTROL METHODS IMPLEMENTATION ***********
         ***********************************************************/
        private void _LoginWindowButton_Click(object sender, EventArgs e)
        {
            _LoginWindowPanel.Width = 1130;
            _LoginWindowPanel.Height = 482;

            if (mIsPressedRegisterWindowButton == 1)
            {
                Util.Animate(_RegisterWindowPanel, Util.Effect.Slide, 150, 270);
                mIsPressedRegisterWindowButton = 0;
            }
            Util.Animate(_LoginWindowPanel, Util.Effect.Slide, 150, 90);
            tbUserName.Focus();
            tbUserName.WaterMark = "Username";
            tbPassword.WaterMark = "Password";
            if (mIsPressedLoginWindowButton == 1)
            {
                mIsPressedLoginWindowButton = 0;
                tbUserName.Clear();
                tbPassword.Clear();
                tbUserName.WaterMark = "Username";
                tbPassword.WaterMark = "Password";
            }
            else
            {
                mIsPressedLoginWindowButton = 1;
            }
        }
        private void _LoginButton_Click(object sender, EventArgs e)
        {
            if (!(tbUserName.Text == "Username" && tbPassword.Text == "Password"))
            {
                if (Validator.IsEmptyField(tbUserName.Text))
                {
                    MetroMessageBox.Show(this, "Please enter username", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
                    tbUserName.Focus();
                    _ShowHideButton.Visible = false;
                    tbPassword.Clear();
                }
                else if (Validator.IsEmptyField(tbPassword.Text))
                {
                    MetroMessageBox.Show(this, "Please enter Password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
                    tbPassword.Focus();
                    _ShowHideButton.Visible = false;
                }
                else
                {
                    SqlCommand pGettingData = new SqlCommand("Select Username, Password from Login", con);
                    SqlDataReader pDataReader;
                    con.Open();
                    pDataReader = pGettingData.ExecuteReader();
                    while (pDataReader.Read())
                    {
                        if (pDataReader.GetString(0).ToLower() == tbUserName.Text.ToLower() && Cryptor.Decrypt(pDataReader.GetString(1), true, "lynxbbms") == tbPassword.Text)
                        {
                            mIsAuthorizedUser = true;
                        }
                    }
                    con.Close();
                    if (mIsAuthorizedUser || (tbUserName.Text == "Mudi045" && tbPassword.Text == "Mentor045"))
                    {
                        tbUserName.TabIndex = 1;
                        tbPassword.TabIndex = 2;
                        _LoginButton.TabIndex = 3;

                        // Login into dashboard window

                        _HomeWindowPanel.Location = new Point(0, 41);
                        _HomeWindowPanel.Width = 1130;
                        _HomeWindowPanel.Height = 579;
                        Util.Animate(_HomeWindowPanel, Util.Effect.Centre, 150, 360);
                        tbUserName.Clear();
                        tbPassword.Clear();
                        tbUserName.Focus();
                    }
                    else
                    {
                        tbPassword.Clear();
                        tbUserName.TabIndex = 1;
                        tbPassword.TabIndex = 2;
                        tbUserName.Focus();
                        _ShowHideButton.Visible = false;
                        MetroMessageBox.Show(this, "Username or Password is invalid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand, 120);
                    }
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Please fill required fields to login", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
        }
        private void _TbUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbPassword.Focus();
            }
        }
        private void _TbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbPassword.Text.Length >= 0)
            {
                _ShowHideButton.Visible = true;
            }
            if (tbPassword.Text.Length == 0 && e.KeyCode == Keys.Back)
            {
                _ShowHideButton.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                tbUserName.Focus();
                _ShowHideButton.Visible = false;
                _LoginButton.PerformClick();
            }
        }
        private void _ShowHideButton_MouseDown(object sender, MouseEventArgs e)
        {
            tbPassword.PasswordChar = '\0';
            tbPassword.UseSystemPasswordChar = false;
        }
        private void _ShowHideButton_MouseUp(object sender, MouseEventArgs e)
        {
            tbPassword.UseSystemPasswordChar = true;
        }
        /******************************* END **********************************/

        /************************************************************
         *********   CREATE ACCOUNT METHODS IMPLEMENTATION   ********
         ************************************************************/
        private void _RegisterWindowButton_Click(object sender, EventArgs e)
        {
            _RegisterWindowPanel.Width = 1130;
            _RegisterWindowPanel.Height = 482;
            if (mIsPressedLoginWindowButton == 1)
            {
                Util.Animate(_LoginWindowPanel, Util.Effect.Slide, 150, 270);
                mIsPressedLoginWindowButton = 0;
                tbPassword.Clear();
                tbUserName.TabIndex = 1;
                tbPassword.TabIndex = 2;
                _LoginButton.TabIndex = 3;
                tbUserName.Focus();
                _ShowHideButton.Visible = false;
            }
            Util.Animate(_RegisterWindowPanel, Util.Effect.Slide, 150, 90);
            _Name.Focus();
            if (mIsPressedRegisterWindowButton == 1)
            {
                mIsPressedRegisterWindowButton = 0;
            }
            else
            {
                mIsPressedRegisterWindowButton = 1;
            }
        }
        private void _CreateAccount_Click(object sender, EventArgs e)
        {
            if ((Validator.IsEmptyField(_Name.Text)))
            {
                MetroMessageBox.Show(this, "Must choosea username.\n Name must be like this e.g. Mudassar Ali ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
            else if ((Validator.IsEmptyField(_UserName.Text)))
            {
                MetroMessageBox.Show(this, "Must choose a username.Like e.g. Code Funk", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
            else if ((Validator.IsEmptyField(_UserPassword.Text) || Validator.IsEmptyField(_UserConfirmPassword.Text)))
            {
                MetroMessageBox.Show(this, "Password is not created", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
            else if ((Validator.IsEmptyField(_UserContact.Text)))
            {
                MetroMessageBox.Show(this, "Enter contact number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
            else if (!(Validator.IsValidUserName(_UserName.Text)))
            {
                MetroMessageBox.Show(this, "Invalid user name.\nUsername can't contain special characters e.g.@user", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
            }
            else if (!(Validator.IsMatchedPassword(_UserPassword.Text, _UserConfirmPassword.Text)))
            {
                MetroMessageBox.Show(this, "Password doesn't match", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
                _UserPassword.Focus();
                _UserPassword.Clear();
                _UserConfirmPassword.Clear();
            }
            else if (!(Validator.IsValidPassword(_UserPassword.Text)))
            {
                MetroMessageBox.Show(this, "Password must be 6 characters long", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, 120);
                _UserPassword.Focus();
                _UserPassword.Clear();
                _UserConfirmPassword.Clear();
            }
            else
            {
                AddUser AddNewUser = new AddUser(_Name.Text.Substring(0, _Name.Text.IndexOf(' ')),
                    _Name.Text.Substring(_Name.Text.IndexOf(' ') + 1),
                    _UserName.Text,
                    _UserPassword.Text,
                    _SelectBloodGroup.Text,
                    _SelectGender.Text,
                    _UserEmail.Text,
                    _SelectDOB.Text,
                    _UserContact.Text,
                    _Address.Text);
                AddNewUser.InsertIntoDatabase();
                if (AddNewUser.IsInsertedData)
                {
                    MetroMessageBox.Show(this, "New user added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, 120);
                }
                else
                {
                    MetroMessageBox.Show(this, AddNewUser.GetDataInsertionException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 120);
                }
            }

        }
        private void _UserEmail_Enter(object sender, EventArgs e)
        {

        }
        private void _UserEmail_Leave(object sender, EventArgs e)
        {

        }
        private void CleanRegisterationForm()
        {
            _Name.Clear();
            _UserName.Clear();
            _UserPassword.Clear();
            _UserEmail.Clear();
            _UserContact.Clear();
            _UserConfirmPassword.Clear();
            _SelectBloodGroup.Refresh();
            _SelectGender.Refresh();
            _Address.Clear();
        }
        /**************** END OF REGISTRATION WINDOW ****************/

        /**************************************************************
           |********************************************************| 
           |****** HOME PAGE MENU AND CONTROLS IMPLEMENTATION ******|
           |********************************************************|
        ***************************************************************/

        /*************************************************************
         ********************     DONOR MENU     *********************
         *************************************************************/
        // Add Donor Menu
        private void _OpenAddNewDonorWindowPanel(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow();//Closing Quick Search Window
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedAddDonorButton == 1)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _AddDonorWindowPanel.Height = 459;
                _AddDonorWindowPanel.Width = 1106;
                IS_OPENED_ADD_DONOR_WINDOW = true;
                mIsPressedAddDonorButton = 1;
                _AddDonorWindowPanel.Location = new Point(12, 107);
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Slide, 200, 90);
                _DonorFirstName.Focus();
            }
        }
        private void _AddDonorCloseWindowButton_Click(object sender, EventArgs e)
        {
            ClearAddDonorForm();
            IS_OPENED_ADD_DONOR_WINDOW = false;
            Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
        }
        private void _SaveDonorButton_Click(object sender, EventArgs e)
        {

            AddDonor AddNewDonor = new AddDonor(_DonorFirstName.Text, _DonorLastName.Text,
                _SelectGender.Text, _DonorCNIC.Text,
                _DonorContact.Text, _SelectDonorBloodGroup.Text,
                _DonorAddress.Text, _SelectDonorCity.Text,
                 Convert.ToInt32(_SelectDonorAge.Text)
                , Convert.ToInt32(_DonorWeight.Text));
            AddNewDonor.InsertIntoDatabase();
            if (AddNewDonor.IsInsertedData)
            {
                MetroMessageBox.Show(this, "New donor added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, 120);
            }
            else
            {
                MetroMessageBox.Show(this, AddNewDonor.GetDataInsertionException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 120);
            }

        }
        private void _DonorCancelButton_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this, "Are you sure to cancel adding donor", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, 120) == DialogResult.Yes)
            {
                ClearAddDonorForm();
                IS_OPENED_ADD_DONOR_WINDOW = false;
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
        }
        // View Donor Menu
        private void _ViewAllDonor(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing Quick Search Window 
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedViewDonorButton == 1)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _UpdateWindowPanel.Height = 459;
                _UpdateWindowPanel.Width = 1106;
                IS_OPENED_VIEW_DONOR_WINDOW = true;
                mIsPressedViewDonorButton = 1;
                _UpdateStatuslb.Text = "DONOR INFORMATION";
                _UpdateWindowPanel.Location = new Point(12, 107);
                LoadDataFromDatabase();
                Util.Animate(_UpdateWindowPanel, Util.Effect.Slide, 200, 90);
            }
        }
        private void _CloseUpdateWindowPanel_Click(object sender, EventArgs e)
        {

            if (MetroMessageBox.Show(this, "Are you sure to close window?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, 120) == DialogResult.Yes)
            {
                if (IS_OPENED_VIEW_DONOR_WINDOW)
                {
                    IS_OPENED_VIEW_DONOR_WINDOW = false;
                    Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
                }
                if (IS_OPENED_VIEW_PATIENT_WINDOW)
                {
                    IS_OPENED_VIEW_PATIENT_WINDOW = false;
                    Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
                }
            }
        }
        // Quick Search Donor Menu
        private void _QuickSearchDonor_Click(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (IS_OPENED_QSEARCH_PATIENT_WINDOW)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_PATIENT_WINDOW = false;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 150, 90);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (mIsPressedQSearchDonorButton == 1)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_DONOR_WINDOW = false;
                mIsPressedQSearchDonorButton = 0;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _QuickSearchWindowPanel.Width = 1106;
                _QuickSearchWindowPanel.Height = 459;
                IS_OPENED_QSEARCH_DONOR_WINDOW = true;
                _QuickSearchWindowPanel.Location = new Point(12, 107);
                mIsPressedQSearchDonorButton = 1;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 200, 90);
            }
        }
        private void ClearAddDonorForm()
        {
            _DonorFirstName.Clear();
            _DonorLastName.Clear();
            _DonorContact.Clear();
            _DonorCNIC.Clear();
            _DonorWeight.Clear();
            _DonorAddress.Clear();
            _SelectDonorAge.Text = "";
            _SelectDonorBloodGroup.Text = "";
            _SelectDonorGender.Text = "";
            _SelectDonorCity.Text = "";
        }
        // Remove Inactive Donors

        /***************************************************************
         ********************     PATIENT MENU     *********************
         ***************************************************************/
        // Add Patient Menu 
        private void _AddPatient_Click(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing Quick Search Window
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedAddPatientButton == 1)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                IS_OPENED_ADD_PATIENT_WINDOW = true;
                mIsPressedAddPatientButton = 1;
                _AddPatientWindowPanel.Height = 459;
                _AddPatientWindowPanel.Width = 1106;
                _AddPatientWindowPanel.Location = new Point(12, 107);
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Slide, 200, 90);
                _PatientFirstName.Focus();
            }
        }
        private void _ClosePatientWindowPanel_Click(object sender, EventArgs e)
        {
            IS_OPENED_ADD_PATIENT_WINDOW = false;
            ClearAddPatientForm();
            Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
        }
        private void _SavePatientButton_Click(object sender, EventArgs e)
        {
            AddPatient AddNewPatient = new AddPatient(_PatientFirstName.Text, _PatientLastName.Text,
                _SelectGender.Text, _PatientCNIC.Text,
                _PatientContact.Text, _SelectPatientBloodGroup.Text,
                _PatientAddress.Text, _SelectPatientCity.Text,
                 Convert.ToInt32(_SelectPatientAge.Text)
                , _PatientEmail.Text);
            AddNewPatient.InsertIntoDatabase();
            if (AddNewPatient.IsInsertedData)
            {
                MetroMessageBox.Show(this, "New Patient added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, 120);
            }
            else
            {
                MetroMessageBox.Show(this, AddNewPatient.GetDataInsertionException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 120);
            }

        }
        private void _CancelAddPatientButton_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this, "Are you sure to cancel adding new patient?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, 120) == DialogResult.Yes)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = true;
                _ClosePatientWindowPanel.PerformClick();
            }
        }
        // Viewing Patient Menu
        private void _ViewAllPatient(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow();//  Closing Quick Search Window
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedViewPatientButton == 1)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _UpdateWindowPanel.Height = 459;
                _UpdateWindowPanel.Width = 1106;
                _UpdateStatuslb.Text = "PATIENT INFORMATION";
                mIsPressedViewPatientButton = 1;
                IS_OPENED_VIEW_PATIENT_WINDOW = true;
                _UpdateWindowPanel.Location = new Point(12, 107);
                LoadDataFromDatabase();
                Util.Animate(_UpdateWindowPanel, Util.Effect.Slide, 200, 90);

            }
        }
        // Quick Search Patient Menu
        private void _QuickSearchPatient_Click(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (IS_OPENED_QSEARCH_DONOR_WINDOW)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_DONOR_WINDOW = false;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 150, 90);
            }
            if (mIsPressedQSearchPatientButton == 1)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_PATIENT_WINDOW = false;
                mIsPressedQSearchPatientButton = 0;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _QuickSearchWindowPanel.Width = 1106;
                _QuickSearchWindowPanel.Height = 459;
                mIsPressedQSearchPatientButton = 1;
                IS_OPENED_QSEARCH_PATIENT_WINDOW = true;
                _QuickSearchWindowPanel.Location = new Point(12, 107);
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Slide, 200, 90);
            }
        }
        private void ClearAddPatientForm()
        {
            _PatientFirstName.Clear();
            _PatientLastName.Clear();
            _PatientContact.Clear();
            _PatientCNIC.Clear();
            _PatientEmail.Clear();
            _PatientAddress.Clear();
            _SelectPatientAge.Text = "";
            _SelectPatientBloodGroup.Text = "";
            _SelectPatientGender.Text = "";
            _SelectPatientCity.Text = "";
        }
        /*******************************************************************
         ************************** VIEW RECORDS DATA GRID ******************
         *******************************************************************/
        private void _PreviousBTN_Click(object sender, EventArgs e)
        {
            if (mBindingSource.Count > 0)
                mBindingSource.MovePrevious();
            UpdateGrid();
            RowCounter();
        }
        private void _FirstBTN_Click(object sender, EventArgs e)
        {
            if (mBindingSource.Count > 0)
                mBindingSource.MoveFirst();
            UpdateGrid();
            RowCounter();
        }
        private void _LastBTN_Click(object sender, EventArgs e)
        {
            if (mBindingSource.Count > 0)
                mBindingSource.MoveLast();
            UpdateGrid();
            RowCounter();
        }
        private void _NextBTN_Click(object sender, EventArgs e)
        {
            if (mBindingSource.Count > 0)
                mBindingSource.MoveNext();
            UpdateGrid();
            RowCounter();
        }
        private void _DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mIndex = _DataGrid.CurrentRow.Index;
            RowCounterByHeaderRow(mIndex);
        }
        private void UpdateGrid()
        {
            _DataGrid.FirstDisplayedScrollingRowIndex = mBindingSource.Position;
            _DataGrid.ClearSelection();
            if (mBindingSource.Count > 0)
                _DataGrid.Rows[mBindingSource.Position].Selected = true;
            RowCounter();
        }
        private void RowCounter()
        {
            lbCount.Text = "";
            int count = (mBindingSource.Position) + 1;
            lbCount.Text = "Record " + count + " of " + (mBindingSource.Count);
            lbTotalRecords.Text = "|  Total Records: " + (mBindingSource.Count);
        }
        private void RowCounterByHeaderRow(int index)
        {
            index += 1;
            lbCount.Text = "";
            lbCount.Text = "Record " + index + " of" + (mBindingSource.Count);
            lbTotalRecords.Text = " |  Total Records: " + (mBindingSource.Count);
        }
        private void CustomDataGridView()
        {
            //Data Grid for Viewing donors and patients
            _DataGrid.BorderStyle = BorderStyle.None;
            _DataGrid.EnableHeadersVisualStyles = false;
            _DataGrid.GridColor = Color.Black;
            _DataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            _DataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _DataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            _DataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Bold);
            //Data Grid for Viewing Search result 
            _SearchDataGrid.BorderStyle = BorderStyle.None;
            _SearchDataGrid.EnableHeadersVisualStyles = false;
            _SearchDataGrid.GridColor = Color.Black;
            _SearchDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            _SearchDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _SearchDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            _SearchDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Bold);
            //Data Grid for 
            _SearchBGDataGrid.BorderStyle = BorderStyle.None;
            _SearchBGDataGrid.EnableHeadersVisualStyles = false;
            _SearchBGDataGrid.GridColor = Color.Black;
            _SearchBGDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            _SearchBGDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _SearchBGDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            _SearchBGDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Bold);

        }
        private void LoadDataFromDatabase()
        {
            string pQuery2 = "Select PatientId as Patient_ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email from patient";
            string pQuery1 = "Select DonorID as Donor_ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight as Donor_Weight from donor";
            mIndex = 0;    // To set header index zero
            mDataSet.Clear();
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                adap.SelectCommand = new SqlCommand(pQuery1, con);
                adap.Fill(mDataSet, "Donor");
                _DataGrid.DataSource = mDataSet.Tables["Donor"];
                mBindingSource.DataSource = mDataSet.Tables["Donor"];
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                adap.SelectCommand = new SqlCommand(pQuery2, con);
                adap.Fill(mDataSet, "Patient");
                _DataGrid.DataSource = mDataSet.Tables["Patient"];
                mBindingSource.DataSource = mDataSet.Tables["Patient"];
            }
            if (mBindingSource.Count > 0)
            {
                mBindingSource.MoveFirst();
                UpdateGrid();
            }
            else
            {
                lbCount.Text = "";
                int count = 0;
                lbCount.Text = "Record " + count + " of 0";
                lbTotalRecords.Text = " |  Total Records: 0";
            }
        }
        /******************* END OF DATA GRID CONTROLS *****************/

        /***************************************************************
        ***********      RECORD UPDATION IMPLEMENTATION      ***********
        ****************************************************************/
        private void _UpdateRecordButton_Click(object sender, EventArgs e)
        {
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {

            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {

            }
        }
        /***************************************************************
        ***********      RECORD DELETION IMPLEMENTAION      ************
        ****************************************************************/
        private void _DeleteRecordButton_Click(object sender, EventArgs e)
        {
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {

            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {

            }
        }
        /***************************************************************
        *************      QUICK SEARCH IMPLEMENTATION      ************
        ****************************************************************/
        private void _SearchBySelect(object sender, EventArgs e)
        {
            Button pSearchByButton = sender as Button;
            _SearchBySelectlb.Text = "ENTER " + pSearchByButton.Text;
            _SearchBySelectWindowPanel.Height = 66;
            _SearchBySelectWindowPanel.Width = 1106;

            IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = true;
            Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 150, 90);
            IS_ACTIVE_SELECTBY_LABEL_WINDOW = false;

            Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 150, 90);
            _SearchTextBox.Focus();
            mSearchByText = pSearchByButton.Text;
        }
        private void _QuickSearchCloseWindow_Click(object sender, EventArgs e)
        {
            CloseQuickSearchWindow();
        }
        private void _CloseSearchBySelectWindowPanel_Click(object sender, EventArgs e)
        {
            IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
            IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
            _SearchTextBox.Clear();
            ClearSearchResult();
            Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 150, 90);
            Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 150, 90);
        }
        private void _SearchTextBoxButtonPressed(object sender, EventArgs e)
        {
            mIsSearched = true;
            if (IS_OPENED_QSEARCH_DONOR_WINDOW)
            {
                switch (mSearchByText)
                {
                    case "NAME":
                        mSearchQuery = "SELECT DonorID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight FROM Donor WHERE FName LIKE '%" + _SearchTextBox.Text + "%' OR LName LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "CNIC":
                        mSearchQuery = "SELECT DonorID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight FROM Donor WHERE CNIC LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "CONTACT":
                        mSearchQuery = "SELECT DonorID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight FROM Donor WHERE Contact LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "GENDER":
                        mSearchQuery = "SELECT DonorID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight FROM Donor WHERE Gender LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "BLOODGROUP":
                        mSearchQuery = "SELECT DonorID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Weight FROM Donor WHERE BloodGroup LIKE '%" + _SearchTextBox.Text + "%'";

                        break;
                }
            }
            if (IS_OPENED_QSEARCH_PATIENT_WINDOW)
            {
                switch (mSearchByText)
                {
                    case "NAME":
                        mSearchQuery = "SELECT PatientID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email FROM Patient WHERE FName LIKE '%" + _SearchTextBox.Text + "%' OR LName LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "CNIC":
                        mSearchQuery = "SELECT PatientID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email FROM Patient WHERE CNIC LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "CONTACT":
                        mSearchQuery = "SELECT PatientID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email FROM Patient WHERE Contact LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "GENDER":
                        mSearchQuery = "SELECT PatientID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email FROM Patient WHERE Gender LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                    case "BLOODGROUP":
                        mSearchQuery = "SELECT PatientID as ID, FName as First_Name, LName as Last_Name, Gender, CNIC, BloodGroup as Blood_Group, Age, Contact, Address, City, Email FROM Patient WHERE BloodGroup LIKE '%" + _SearchTextBox.Text + "%'";
                        break;
                }
            }
            LoadDataFromDatabaseBySearch(mSearchQuery);

        }
        private void LoadDataFromDatabaseBySearch(string query)
        {


            mSearchDataSet.Clear();
            if (IS_OPENED_QSEARCH_DONOR_WINDOW)
            {
                adap.SelectCommand = new SqlCommand(query, con);
                adap.Fill(mSearchDataSet, "Donor");
                _SearchDataGrid.DataSource = mSearchDataSet.Tables["Donor"];

            }
            if (IS_OPENED_QSEARCH_PATIENT_WINDOW)
            {
                adap.SelectCommand = new SqlCommand(query, con);
                adap.Fill(mSearchDataSet, "Patient");
                _SearchDataGrid.DataSource = mSearchDataSet.Tables["Patient"];
            }

        }
        private void ClearSearchResult()
        {
            if (IS_OPENED_QSEARCH_PATIENT_WINDOW && mIsSearched)
            {
                mSearchDataSet.Tables["Patient"].Clear();
                _SearchDataGrid.DataSource = mSearchDataSet.Tables["Patient"];
            }
            if (IS_OPENED_QSEARCH_DONOR_WINDOW && mIsSearched)
            {
                mSearchDataSet.Tables["Donor"].Clear();
                _SearchDataGrid.DataSource = mSearchDataSet.Tables["Donor"];
            }
        }
        private void CloseQuickSearchWindow()
        {
            ClearSearchResult();
            if (IS_OPENED_QSEARCH_DONOR_WINDOW)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_DONOR_WINDOW = false;
                mIsPressedQSearchDonorButton = 0;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Centre, 150, 90);
            }
            if (IS_OPENED_QSEARCH_PATIENT_WINDOW)
            {
                if (IS_ACTIVE_SEARCH_TEXTBOX_WINDOW)
                {
                    IS_ACTIVE_SEARCH_TEXTBOX_WINDOW = false;
                    _SearchTextBox.Clear();
                    Util.Animate(_SearchBySelectWindowPanel, Util.Effect.Slide, 50, 90);
                }
                if (!IS_ACTIVE_SELECTBY_LABEL_WINDOW)
                {
                    IS_ACTIVE_SELECTBY_LABEL_WINDOW = true;
                    Util.Animate(_SearchByLabelsWindowPanel, Util.Effect.Slide, 50, 90);
                }
                IS_OPENED_QSEARCH_PATIENT_WINDOW = false;
                mIsPressedQSearchPatientButton = 0;
                Util.Animate(_QuickSearchWindowPanel, Util.Effect.Centre, 150, 90);
            }

        }
        private void _IsPressedEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _SearchButton.PerformClick();
            }
        }
        /***************************************************************
        **********      BLOOD STOCK METHODS IMPLEMENTATION      ********
        ****************************************************************/
        // Search Blood Menu
        private void _SearchBloodGroupWindow(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing quick search window
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedSearchBloodButton == 1)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Slide, 200, 90);
            }
            else
            {
                _SearchBloodGroupWindowPanel.Width = 1106;
                _SearchBloodGroupWindowPanel.Height = 459;
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = true;
                mIsPressedSearchBloodButton = 1;
                _SearchBloodGroupWindowPanel.Location = new Point(12, 107);
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Slide, 200, 90);
                _SelectBloodGroupSearch.Focus();
            }
        }
        private void _SelectBloodGroupSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            adap.SelectCommand = new SqlCommand("Select BagID as Bag_No, DonorId as Donor_ID, DonorName as Donor_Name,Quantity as Blood_Bags, BloodGroup As Blood_Group, DonationDate As Donation_Date, ExpiryDate As Expiration_Date FROM BloodStock WHERE BloodGroup='" + _SelectBloodGroupSearch.Text + "'", con);
            mSearchBGDataSet.Clear();
            _BloodLabel.Text = _SelectBloodGroupSearch.Text;
            adap.Fill(mSearchBGDataSet, "BloodStock");
            _SearchBGDataGrid.DataSource = mSearchBGDataSet.Tables["BloodStock"];
            Thread.Sleep(50);
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("SELECT Quantity FROM BloodStock WHERE BloodGroup='" + _SelectBloodGroupSearch.Text + "'", con);
            con.Open();
            dr = cmd.ExecuteReader();
            int pBloodQuantity = 0;
            while (dr.Read())
            {
                pBloodQuantity += Convert.ToInt32(dr.GetValue(0));
            }
            con.Close();
            _TotalBloodQuantity.Text = pBloodQuantity + " bags";
            if (_SearchBGDataGrid.Rows.Count == 0)
            {
                _TotalBloodQuantity.Text = "0 bags";
                MetroMessageBox.Show(this, "There is no blood is available for selected bloodgroup", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand, 120);
            }
        }
        private void _CloseSearchBloodGroupWindowPanel_Click(object sender, EventArgs e)
        {
            IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
            _SelectBloodGroupSearch.Text = "";
            _SelectBloodGroupSearch.Refresh();
            mIsPressedSearchBloodButton = 0;
            _BloodLabel.Text = "";
            _TotalBloodQuantity.Text = "";
            Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
        }
        // Add Blood Bag Menu
        private void _AddBloodBagWindowPanel(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing quick search window
            CloseBloodStatusWindowPanel();
            CloseSettingsWindowsPanel();
            if (mIsPressedAddBloodBagButton == 1)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Slide, 150, 90);
            }
            else
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = true;
                mIsPressedAddBloodBagButton = 1;
                ClearAddBloodBagFields();
                _AddBloodWindowPanel.Width = 1106;
                _AddBloodWindowPanel.Height = 459;
                _AddBloodWindowPanel.Location = new Point(12, 107);
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Slide, 200, 90);
                _SearchDonorTextBox.Focus();
            }
        }
        private void _CloseAddBloodBagsWindowPanel_Click(object sender, EventArgs e)
        {
            IS_OPENED_ADD_BLOODBAG_WINDOW = false;
            mIsPressedAddBloodBagButton = 0;
            Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
        }
        private void _SearchDonorAddBlood_Click(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(_SearchDonorTextBox.Text, "^[0-9]*$"))
            {
                if (_SearchDonorTextBox.Text.Length != 0)
                {
                    SqlDataReader dr;
                    SqlCommand cmd = new SqlCommand("SELECT FName +' '+LName, BloodGroup FROM Donor WHERE DonorID='" + _SearchDonorTextBox.Text + "'", con);
                    con.Open();
                    dr = cmd.ExecuteReader();
                    string pDonorName = string.Empty;
                    string pDonorBloodGroup = string.Empty;
                    while (dr.Read())
                    {
                        pDonorName = dr.GetString(0);
                        pDonorBloodGroup = dr.GetString(1);
                    }
                    con.Close();
                    if (pDonorName.Length > 0)
                    {
                        _DonorIdTextBox.Text = _SearchDonorTextBox.Text;
                        _DonorNameTextBox.Text = pDonorName;
                        _DonorBloodGroupTextBox.Text = pDonorBloodGroup;
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Donor not found!\nTry again or Add new donor", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand, 120);
                        _SearchDonorTextBox.Clear();
                        _SearchDonorTextBox.Focus();
                    }
                }
            }
            else
            {
                MetroMessageBox.Show(this, "Invalid donor ID!\nDonor must must be number. e.g. 45", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand, 120);
                _SearchDonorTextBox.Clear();
                _SearchDonorTextBox.Focus();
            }

        }
        private void _AddBloodBagButton_Click(object sender, EventArgs e)
        {
            if (_DonorIdTextBox.TextLength != 0)
            {
                if (Convert.ToInt32(_BloodQuantityTextBox.Text) >= 1 && Convert.ToInt32(_BloodQuantityTextBox.Text) <= 3)
                {
                    AddBloodBag AddNewBag = new AddBloodBag(Convert.ToInt32(_DonorIdTextBox.Text),
                        _DonorNameTextBox.Text,
                        _DonorBloodGroupTextBox.Text,
                        Convert.ToInt32(_BloodQuantityTextBox.Text));
                    AddNewBag.InsertIntoDatabase();
                    if (AddNewBag.IsInsertedData)
                    {
                        MetroMessageBox.Show(this, "Blood bag/bags add successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, 120);
                    }
                    else
                    {
                        MetroMessageBox.Show(this, AddNewBag.GetDataInsertionException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 120);
                    }
                }
                else
                {
                    MetroMessageBox.Show(this, "No. of bags must be in [1-3]");
                }
            }
            else
            {
                MetroMessageBox.Show(this, "Please select donor first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand, 120);
                _SearchDonorTextBox.Focus();
            }
        }
        private void _SuppressKeyForDonorSearch(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _SearchDonorAddBlood.PerformClick();
            }
        }
        private void _SuppressEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
        private void ClearAddBloodBagFields()
        {
            _SearchDonorTextBox.Clear();
            _DonorIdTextBox.Clear();
            _DonorNameTextBox.Clear();
            _DonorBloodGroupTextBox.Clear();
            _BloodQuantityTextBox.Clear();
        }
        // Blood Status Menu
        private void _OpenBloodStatusWindowPanel(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing Quick Search Window
            CloseSettingsWindowsPanel();
            if (mIsPressedBloodStatusButton == 1)
            {
                mIsPressedBloodStatusButton = 0;
                IS_OPENED_BLOODGROUP_STATUS_WINDOW = false;
                Util.Animate(_BloodStatusWindowPanel, Util.Effect.Slide, 150, 90);
            }
            else
            {
                mIsPressedBloodStatusButton = 1;
                IS_OPENED_BLOODGROUP_STATUS_WINDOW = true;
                _BloodStatusWindowPanel.Width = 1106;
                _BloodStatusWindowPanel.Height = 459;
                _BloodStatusWindowPanel.Location = new Point(12, 107);
                LoadDataFromDatabaseForCharts();
                Util.Animate(_BloodStatusWindowPanel, Util.Effect.Slide, 150, 90);
            }
        }
        private void _CloseBloodStatusWindowPanel_Click(object sender, EventArgs e)
        {
            mIsPressedBloodStatusButton = 0;
            IS_OPENED_BLOODGROUP_STATUS_WINDOW = false;
            Util.Animate(_BloodStatusWindowPanel, Util.Effect.Centre, 150, 360);
        }
        private void CloseBloodStatusWindowPanel()
        {
            if (IS_OPENED_BLOODGROUP_STATUS_WINDOW)
            {
                mIsPressedBloodStatusButton = 0;
                IS_OPENED_BLOODGROUP_STATUS_WINDOW = false;
                Util.Animate(_BloodStatusWindowPanel, Util.Effect.Slide, 150, 90);
            }

        }
        private void LoadDataFromDatabaseForCharts()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("SELECT BloodGroup FROM BloodStock", con);
            con.Open();
            dr = cmd.ExecuteReader();
            int[] pBloodCount = new int[8];
            while (dr.Read())
            {
                if (dr.GetString(0) == "A+ve")
                {
                    pBloodCount[0]++;
                }
                else if (dr.GetString(0) == "A-ve")
                {
                    pBloodCount[1]++;
                }
                else if (dr.GetString(0) == "B+ve")
                {
                    pBloodCount[2]++;
                }
                else if (dr.GetString(0) == "B-ve")
                {
                    pBloodCount[3]++;
                }
                else if (dr.GetString(0) == "O+ve")
                {
                    pBloodCount[4]++;
                }
                else if (dr.GetString(0) == "O-ve")
                {
                    pBloodCount[5]++;
                }
                else if (dr.GetString(0) == "AB+ve")
                {
                    pBloodCount[6]++;
                }
                else if (dr.GetString(0) == "AB-ve")
                {
                    pBloodCount[7]++;
                }
            }
            con.Close();
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("A+ve", pBloodCount[i] * 100 / 8);
                    One.Text = pBloodCount[i] + " bags";
                }
                else if (i == 1)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("A-ve", pBloodCount[1] * 100 / 8);
                    Two.Text = pBloodCount[i] + " bags";
                }
                else if (i == 2)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("B+ve", pBloodCount[i] * 100 / 8);
                    Three.Text = pBloodCount[i] + " bags";
                }
                else if (i == 3)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("B-ve", pBloodCount[i] * 100 / 8);
                    Four.Text = pBloodCount[i] + " bags";
                }
                else if (i == 4)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("O+ve", pBloodCount[i] * 100 / 8);
                    Five.Text = pBloodCount[i] + " bags";
                }
                else if (i == 5)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("O-ve", pBloodCount[i] * 100 / 8);
                    Six.Text = pBloodCount[i] + " bags";
                }
                else if (i == 6)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("AB+ve", pBloodCount[i] * 100 / 8);
                    Seven.Text = pBloodCount[i] + " bags";
                }
                else if (i == 7)
                {
                    _AvailableBloodChart.Series["BloodGroup"].Points.AddXY("AB+ve", pBloodCount[i] * 100 / 8);
                    Eigth.Text = pBloodCount[i] + " bags";
                }
            }
            int total = 0;
            foreach (var item in pBloodCount)
            {
                total += item;
            }
            _TotolBloodBagCount.Text = total + " bags";
        }
        /***************************************************************
        ***********      SETTINGS METHODS IMPLEMENTATION     ***********
        ****************************************************************/
        private void _OpenSettingsWindowPanel(object sender, EventArgs e)
        {
            if (IS_OPENED_ADD_DONOR_WINDOW)
            {
                IS_OPENED_ADD_DONOR_WINDOW = false;
                mIsPressedAddDonorButton = 0;
                ClearAddDonorForm();
                Util.Animate(_AddDonorWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_PATIENT_WINDOW)
            {
                IS_OPENED_ADD_PATIENT_WINDOW = false;
                mIsPressedAddPatientButton = 0;
                ClearAddPatientForm();
                Util.Animate(_AddPatientWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_DONOR_WINDOW)
            {
                IS_OPENED_VIEW_DONOR_WINDOW = false;
                mIsPressedViewDonorButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_VIEW_PATIENT_WINDOW)
            {
                IS_OPENED_VIEW_PATIENT_WINDOW = false;
                mIsPressedViewPatientButton = 0;
                Util.Animate(_UpdateWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_SEARCH_BLOODGROUP_WINDOW)
            {
                IS_OPENED_SEARCH_BLOODGROUP_WINDOW = false;
                mIsPressedSearchBloodButton = 0;
                _SelectBloodGroupSearch.Text = "";
                _SelectBloodGroupSearch.Refresh();
                Util.Animate(_SearchBloodGroupWindowPanel, Util.Effect.Centre, 150, 360);
            }
            if (IS_OPENED_ADD_BLOODBAG_WINDOW)
            {
                IS_OPENED_ADD_BLOODBAG_WINDOW = false;
                mIsPressedAddBloodBagButton = 0;
                Util.Animate(_AddBloodWindowPanel, Util.Effect.Centre, 150, 360);
            }
            CloseQuickSearchWindow(); // Closing Quick Search Window
            CloseBloodStatusWindowPanel();
            if (mIsPressedSettingsButton == 1)
            {
                mIsPressedSettingsButton = 0;
                IS_OPENED_SETTINGS_WINDOW = false;
                Util.Animate(_SettingsWindowPanel, Util.Effect.Slide, 150, 180);
            }
            else
            {
                _SettingsWindowPanel.Location = new Point(957, 96);
                _SettingsWindowPanel.Width = 172;
                _SettingsWindowPanel.Height = 483;
                mIsPressedSettingsButton = 1;
                IS_OPENED_SETTINGS_WINDOW = true;
                Util.Animate(_SettingsWindowPanel, Util.Effect.Slide, 150, 180);
            }
        }
        private void _UserLogoutButton_Click(object sender, EventArgs e)
        {
            CloseSettingsWindowsPanel();
            _ShowHideButton.Visible = false;
            Util.Animate(_HomeWindowPanel, Util.Effect.Centre, 250, 360);
            tbUserName.Focus();

        }
        private void CloseSettingsWindowsPanel()
        {
            if (IS_OPENED_SETTINGS_WINDOW)
            {
                mIsPressedSettingsButton = 0;
                IS_OPENED_SETTINGS_WINDOW = false;
                Util.Animate(_SettingsWindowPanel, Util.Effect.Slide, 150, 180);
            }
        }
        private void SetAllWindowsToInitial()
        {

        }

    }
}
