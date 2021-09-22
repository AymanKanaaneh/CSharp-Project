using GrpcFIRService;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

//FourInRowClient.
namespace FourInRowClient {   
    public partial class RegisterWindow : Window {
        public Func<UserModel, Task> AddUser { get; internal set; }
        string namePattern = @"^[A-Za-z]+[1-9]*$";
        private bool clicked;
        //initilize
        public RegisterWindow() {
            InitializeComponent();
            clicked = false;
        }
        //----------------------------------------------------------------------
        //handle the btn click and save the username to the database
        private async void btnSignUp(object sender, RoutedEventArgs e) {

            if (clicked) {
                MessageBox.Show("Please wait");
                return;
            }

            if (!AllTextBoxesFilled()) {
                MessageBox.Show("You must enter all data");
                clicked = false;
                return;
            }

            if (!vallidName()) {
                MessageBox.Show("User name not in correct syntex, it should be contain first letters after that zero or more numbers");
                clicked = false;
                return;
            }

            if (!vallidPassword()) {
                MessageBox.Show("Password should contain at least one symbol");
                clicked = false;
                return;
            }

            clicked = true;
            await AddUser(new UserModel
            {
                UserName = tbUserName.Text.Trim(),
                HashedPassword = HashValue(tbPassword.Password),
                NumOfGames = 0,
                NumOFWins = 0,
               
            });
            clicked = false;
        }
        //----------------------------------------------------------------------
        //check if all fields(username and password) are filled
        private bool AllTextBoxesFilled() {
            if (tbUserName.Text != null && tbPassword.Password != null)
                return true;
            return false;
        }
        //----------------------------------------------------------------------
        //hash value
        private string HashValue(string password) {
            using (SHA256 hashObject = SHA256.Create()) {
                byte[] hashBytes = hashObject.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes) {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        //----------------------------------------------------------------------
        //check if the username is legal and return true else return false
        private bool vallidName() {
            try {
                if (!Regex.Match(tbUserName.Text, namePattern).Success) {
                    throw new Exception();
                }
            }
            catch (Exception ex) {
                return false;
            }
            
            return true;
        }
        //----------------------------------------------------------------------
        //check if the password is legal and return true else return false
        private bool vallidPassword() {
            if (tbPassword.Password != "") { 
                return true;
            }
            return false;
        }
    }
}
