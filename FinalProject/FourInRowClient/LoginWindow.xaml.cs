using Grpc.Core;
using Grpc.Net.Client;
using GrpcFIRService;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FourInRowClient {
    public partial class MainWindow : Window {
        public GrpcChannel Channel { get; set; }
        public FourInRow.FourInRowClient Client { get; set; }
        private bool clicked;
        //----------------------------------------------------------------------
        //initilize the chanel and client
        public MainWindow() {
            InitializeComponent();
            Channel = GrpcChannel.ForAddress("https://localhost:5001/");
            Client = new FourInRow.FourInRowClient(Channel);

            clicked = false;
        }
        //----------------------------------------------------------------------
        //on clickong on the login button check if the user and password legal and open an entry window
        private async void btnLogin(object sender, RoutedEventArgs e) {
            if (clicked) {
                MessageBox.Show("Please wait");
                return;
            }

            if (!AnyFieldEmpty()) {
                clicked = true;

                try {
                    await Client.UserExistsAsync(new UserModel
                    {

                        UserName = tbUsername.Text.Trim(),
                        HashedPassword = HashValue(tbPassword.Password),
                        NumOfGames = 0,
                        NumOFWins = 0,

                    });
                }
                catch (RpcException ex) {
                    MessageBox.Show(ex.Message);
                    clicked = false;
                    return;
                    
                }

                try {
                    await Client.GetUserByIdAsync(new UserModel
                    {

                        UserName = tbUsername.Text.Trim(),
                        HashedPassword = HashValue(tbPassword.Password),
                        NumOfGames = 0,
                        NumOFWins = 0,

                    });
                    //MessageBox.Show("Welcome");
                }
                catch (RpcException ex) {
                    MessageBox.Show("Incorrect username or password \n"+ ex.Message);
                    clicked = false;
                    return;
                }


                EntryWindow entryWindow = new EntryWindow(new UserModel
                {

                    UserName = tbUsername.Text.Trim(),
                    HashedPassword = HashValue(tbPassword.Password),
                    NumOfGames = 0,
                    NumOFWins = 0,

                }, Channel,Client);


                this.Close();
                entryWindow.Show();
                
            }
        }
        //----------------------------------------------------------------------
        //open a new rigester window with delegate
        private void btnRegister(object sender, RoutedEventArgs e) {

            if (clicked) {
                MessageBox.Show("Please wait");
                return;
            }

            RegisterWindow RW = new RegisterWindow();
            RW.AddUser += AddUser2DB;
            RW.ShowDialog();
        }
        //----------------------------------------------------------------------
        //add a user to the database sends with delegate
        private async Task AddUser2DB(UserModel user2add) {

            try {
                await Client.UserExistsAsync(user2add);
                await Client.InsertUserAsync(user2add);
                MessageBox.Show("User added");
                clicked = false;
            }
            catch (RpcException ex) {
                MessageBox.Show("User alerady exist: "+ex.Message);
                clicked = false;
            }
        }
        //check if all the fields are filled
        private bool AnyFieldEmpty() {
            return String.IsNullOrEmpty(tbUsername.Text.Trim()) || String.IsNullOrEmpty(tbPassword.Password);
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
    }
}
