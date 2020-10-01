using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Login_Registration
{
    /* Rory Stouder
     * Assignment 8
     * 10/11/17
     * Password Validation
     */

    public struct Account
    {
        public string name;
        public string password;


        public override string ToString()
        {
            return name + ", " + password;
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Global
        const string ACCOUNT_FILE = "accounts.txt";
        const string BLANK_ERROR = "field must not be blank!";
        const int MIN_LENGTH = 8;          // Password's minimum length

        List<Account> allAccounts = new List<Account>();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader accountFile = new StreamReader(ACCOUNT_FILE);

                while (!accountFile.EndOfStream)
                {
                    String entireline = accountFile.ReadLine();
                    String[] tokens = entireline.Split(',');

                    Account newAccount = new Account();

                    newAccount.name = tokens[0];
                    newAccount.password = tokens[1];

                    allAccounts.Add(newAccount);
                }
                accountFile.Close();
                txtRegisterName.Focus();
            }
            catch
            {
                lblStatus.Text = "file load did not happen!";
            }
        }

        // The NumberUpperCase method accepts a string argument
        // and returns the number of uppercase letters it contains.
        private int NumberUpperCase(string str)
        {
            int upperCase = 0;  // The number of uppercase letters

            // Count the uppercase characters in str.
            foreach (char ch in str)
            {
                if (char.IsUpper(ch))
                {
                    upperCase++;
                }
            }

            // Return the number of uppercase characters.
            return upperCase;
        }

        // The NumberLowerCase method accepts a string argument
        // and returns the number of lowercase letters it contains.
        private int NumberLowerCase(string str)
        {
            int lowerCase = 0;  // The number of lowercase letters

            // Count the lowercase characters in str.
            foreach (char ch in str)
            {
                if (char.IsLower(ch))
                {
                    lowerCase++;
                }
            }

            // Return the number of lowercase characters.
            return lowerCase;
        }

        // The NumberDigits method accepts a string argument
        // and returns the number of numeric digits it contains.
        private int NumberDigits(string str)
        {
            int digits = 0;  // The number of digits

            // Count the digits in str.
            foreach (char ch in str)
            {
                if (char.IsDigit(ch))
                {
                    digits++;
                }
            }

            // Return the number of digits.
            return digits;
        }

        // The Symbol method accepts a string argument
        // and returns the number of symbols it contains.
        private int NumberPunctuation(string str)
        {
            int symbols = 0; // the number of symbols

            // count the symbols in str.
            foreach(char ch in str)
            {
                if (char.IsPunctuation(ch))
                {
                    symbols++;
                }
            }
            // Return the number of symbols
            return symbols;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Get the User Name from the TextBox.
            string userName = txtRegisterName.Text;

            // Get the password from the TextBox.
            string password = txtRegisterPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (userName.Length == 0 &&
                password.Length == 0 &&
                confirmPassword.Length == 0)
            {
                lblStatus.Text = ("the User Name " + BLANK_ERROR);
                txtRegisterName.Focus();
                return;
            }
            else if (userName.Contains(',') &&
                password.Contains(',') &&
                confirmPassword.Contains(','))
            {
                lblStatus.Text = "You can not use a comma in any field!";
                txtRegisterName.Focus();
                return;
            }

            // Validate the password.
            if (password.Length >= MIN_LENGTH &&
                NumberUpperCase(password) >= 1 &&
                NumberLowerCase(password) >= 1 &&
                NumberDigits(password) >= 1 &&
                NumberPunctuation(password) >= 1)
            {
                if (password.Equals(txtConfirmPassword.Text))
                {
                    Account newAccount = new Account();

                    newAccount.name = txtRegisterName.Text;
                    newAccount.password = txtRegisterPassword.Text;

                    if (allAccounts.Contains(newAccount))
                    {
                        lblStatus.Text = "That username has already taken, pick another.";
                        txtRegisterName.Focus();
                        return;
                    }
                    else
                    {
                        // save to list
                        allAccounts.Add(newAccount);
                        lblStatus.Text = "New Account Confirmed, Welcome to C#";

                        // append file.
                        StreamWriter outputFile;

                        saveFile.FileName = ACCOUNT_FILE;
                        saveFile.Filter = "Plain Text (*.txt)| *.txt | Word (*.doc)| *.doc | Open Text (*.odt)| *.odt | Rich Text (*)| *.rtf ";

                        try
                        {
                            outputFile = File.CreateText(saveFile.FileName);

                            foreach (Account obj in allAccounts)
                            {
                                outputFile.WriteLine((obj.name) + "," + (obj.password));
                            }
                            outputFile.Close();
                        }
                        catch
                        {
                            lblStatus.Text = "file write did not happen!";
                        }

                        // Clear all fields
                        txtRegisterName.Text = string.Empty;
                        txtRegisterPassword.Text = string.Empty;
                        txtConfirmPassword.Text = string.Empty;
                    }
                }
            }
            else
            {
                lblStatus.Text = ("The password does not meet " +
                    "the requirements.");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            foreach (Account account in allAccounts)
            {
                if (account.name == txtLoginName.Text &&
                    account.password == txtLoginPassword.Text)
                {
                    lblStatus.Text = "Welcome to the system, " + account.name;
                    txtLoginName.Text = string.Empty;
                    txtLoginPassword.Text = string.Empty;
                    return;
                }                 
            }

            lblStatus.Text = "Login credentials are not correct";

            txtLoginPassword.Text = string.Empty;
            txtLoginName.Text = string.Empty;
        }
    }
}
