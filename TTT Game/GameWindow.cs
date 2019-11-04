using System;
using System.Drawing;
using System.Windows.Forms;

namespace TTT_Game
{
    public partial class GameWindow : Form
    {
        static int _Xscore = 0;
        static int _Oscore = 0;
        bool x = false;
        bool y = false;
        public GameWindow()
        {
            InitializeComponent();
        }

        /**/
        /** Making form moveable from control bar
        /**/
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void panel9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

        }
        // Game Close Method
        private void _BtnClose_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Exit Game?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, 120) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        // Game Minimize Method
        private void _BtnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            _Col1Panel.Hide();
            _Col2Panel.Hide();
            _Col3Panel.Hide();
            _Row1Panel.Hide();
            _Row2Panel.Hide();
            _Row3Panel.Hide();
        }
        //Variable to store player, 0 is X, 1 is O.
        int counter = 0;
        int b1 = 0, b2 = 0, b3 = 0, b4 = 0, b5 = 0, b6 = 0, b7 = 0, b8 = 0, b9 = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (b1 == 0)
            {
                //Check who's turn it is
                if (counter == 0)
                {
                    button1.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button1.Text = "O";
                    counter--;
                }
                b1++;
                check();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (b2 == 0)
            {
                if (counter == 0)
                {
                    button2.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button2.Text = "O";
                    counter--;
                }
                b2++;
                check();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (b3 == 0)
            {
                //Check who's turn it is
                if (counter == 0)
                {
                    button3.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button3.Text = "O";
                    counter--;
                }
                b3++;
                check();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (b4 == 0)
            {
                if (counter == 0)
                {
                    button4.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button4.Text = "O";
                    counter--;
                }
                b4++;
                check();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (b5 == 0)
            {
                if (counter == 0)
                {
                    button5.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button5.Text = "O";
                    counter--;
                }
                b5++;
                check();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (b6 == 0)
            {
                if (counter == 0)
                {
                    button6.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button6.Text = "O";
                    counter--;
                }
                b6++;
                check();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (b7 == 0)
            {
                if (counter == 0)
                {
                    button7.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button7.Text = "O";
                    counter--;
                }
                b7++;
                check();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (b8 == 0)
            {
                if (counter == 0)
                {
                    button8.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button8.Text = "O";
                    counter--;
                }
                b8++;
                check();
            }
        }
        
        private void button9_Click(object sender, EventArgs e)
        {
            if (b9 == 0)
            {
                if (counter == 0)
                {
                    button9.Text = "X";
                    counter++;
                }
                else if (counter == 1)
                {
                    button9.Text = "O";
                    counter--;
                }
                b9++;
                check();
            }
        }
        void check()
        {
            _StartLabel.Visible = false;
            _GameStatus.Visible = true;
            if (counter == 0)
            {
                _GameStatus.Text = "X turn";
                _XSelectPanel.BackColor = Color.MediumAquamarine;
                _OSelectPanel.BackColor = Color.Transparent;
            }
            else
            {
                _XSelectPanel.BackColor = Color.Transparent;
                _OSelectPanel.BackColor = Color.MediumAquamarine;
                _GameStatus.Text = "O turn";
            }
            if (button1.Text != "" && button2.Text != "" && button3.Text != "" &&
                button4.Text != "" && button5.Text != "" && button6.Text != "" &&
                button7.Text != "" && button8.Text != "" && button9.Text != "")
            {
                if (MetroFramework.MetroMessageBox.Show(this, "DRAW!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _EnableBoard();
                }
            }

            //Check diagonal for X
            if (button1.Text == "X" && button5.Text == "X" && button9.Text == "X")
            {
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1; 
                    _EnableBoard();
                }
            }
            if (button3.Text == "X" && button5.Text == "X" && button7.Text == "X")
            {
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            //Check rows for X
            if (button1.Text == "X" && button2.Text == "X" && button3.Text == "X")
            {
                Effects.Animate(_Row1Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            if (button4.Text == "X" && button5.Text == "X" && button6.Text == "X")
            {
                Effects.Animate(_Row2Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            if (button7.Text == "X" && button8.Text == "X" && button9.Text == "X")
            {
                Effects.Animate(_Row3Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            //Check columns for X
            if (button1.Text == "X" && button4.Text == "X" && button7.Text == "X")
            {
                Effects.Animate(_Col1Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            if (button2.Text == "X" && button5.Text == "X" && button8.Text == "X")
            {
                Effects.Animate(_Col2Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }
            if (button3.Text == "X" && button6.Text == "X" && button9.Text == "X")
            {
                Effects.Animate(_Col3Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "X WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Xscore += 1;
                    _EnableBoard();
                }
            }

            //Check diagonal for O
            if (button1.Text == "O" && button5.Text == "O" && button9.Text == "O")
            {
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            if (button3.Text == "O" && button5.Text == "O" && button7.Text == "O")
            {
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            //Check rows for O
            if (button1.Text == "O" && button2.Text == "O" && button3.Text == "O")
            {
                Effects.Animate(_Row1Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            if (button4.Text == "O" && button5.Text == "O" && button6.Text == "O")
            {
                Effects.Animate(_Row2Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            if (button7.Text == "O" && button8.Text == "O" && button9.Text == "O")
            {
                Effects.Animate(_Row3Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            //Check columns for O
            if (button1.Text == "O" && button4.Text == "O" && button7.Text == "O")
            {
                Effects.Animate(_Col1Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            if (button2.Text == "O" && button5.Text == "O" && button8.Text == "O")
            {
                Effects.Animate(_Col2Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
            if (button3.Text == "O" && button6.Text == "O" && button9.Text == "O")
            {
                Effects.Animate(_Col3Panel, Effects.Effect.Centre, 150, 360);
                if (MetroFramework.MetroMessageBox.Show(this, "O WINNER!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information, 90) == DialogResult.OK)
                {
                    _Oscore += 1;
                    _EnableBoard();
                }
            }
        }
        /*
        * Method to restart the game 
        */
        private void _RestartGame_Click(object sender, EventArgs e)
        {
            _EnableBoard();
        }
      //  Selecting player
        private void _OPanelScore_Click(object sender, EventArgs e)
        {
            counter = 1;
            _OSelectPanel.BackColor = Color.MediumAquamarine;
            _XSelectPanel.BackColor = Color.Transparent;
            _StartLabel.Visible = false;
            _GameStatus.Visible = true;
            _GameStatus.Text = "O turn";
        }

        private void _XPanelScore_Click(object sender, EventArgs e)
        {
             counter = 0;
            _OSelectPanel.BackColor = Color.Transparent;
            _XSelectPanel.BackColor = Color.MediumAquamarine;
            _StartLabel.Visible = false;
            _GameStatus.Visible = true;
            _GameStatus.Text = "X turn";
        }
        private void _EnableBoard()
        {
            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";
            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = 0;
            counter = 0;
            _StartLabel.Visible = true;
            _GameStatus.Visible = false;
            _GameStatus.Text = "";
            _OSelectPanel.BackColor = Color.Transparent;
            _XSelectPanel.BackColor = Color.Transparent;
            _XScoreLabel.Text = _Xscore.ToString();
            _OScoreLabel.Text = _Oscore.ToString();
            _Col1Panel.Hide();
            _Col2Panel.Hide();
            _Col3Panel.Hide();
            _Row1Panel.Hide();
            _Row2Panel.Hide();
            _Row3Panel.Hide();
        }
    }
}
