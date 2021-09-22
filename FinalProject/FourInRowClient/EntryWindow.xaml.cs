using Grpc.Core;
using GrpcFIRService;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Collections.Generic;
using Grpc.Net.Client;

namespace FourInRowClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {

        public GrpcChannel Channel { get; set; }
        public FourInRow.FourInRowClient Client { get; set; }

        public AsyncServerStreamingCall<ChatMessage> Listener { get; set; }
        public string UserName { get; set; }

        public Func<IAsyncStreamReader<ChatMessage>, CancellationToken, Task> ListenerDelegate;

        //Each game only have one serialnumber
        private int serialnumber;
        //Each player can only send one request to another player to play with until they respond
        private bool playerResponse;
        //constructor to initilize chanel client and fields
        public EntryWindow(UserModel userId, GrpcChannel _Channel, FourInRow.FourInRowClient _Client)
        {
            InitializeComponent();

            Channel = _Channel;
            Client = _Client;

            AsyncServerStreamingCall<ChatMessage> Listener = Client.Connect(userId);          
            UserName = userId.UserName;
            lblName.Content = "Welcome " + UserName + " to four in row";
            playerResponse = true;

            ListenAsync(Listener.ResponseStream, new CancellationTokenSource().Token);
        }
        //---------------------------------------------------------------------------------
        //listener to recive messages from ohter users
        private async Task ListenAsync(IAsyncStreamReader<ChatMessage> stream, CancellationToken token)
        {
            await foreach (ChatMessage info in stream.ReadAllAsync(token))
            {
                //updated messages
                if (info.Type == MessageType.Update)
                {
                    await UpdateUsersListAsync();
                }

                //receive a approved message to play 
                else if (info.Type == MessageType.Approved)
                {
                    //INIT GAME 1
                    string[] splitMessage = info.Message.Split(" ");
                    serialnumber = int.Parse(splitMessage[splitMessage.Length - 1]);
                    
                    MessageBox.Show(info.FromUser+" accepted your request");

                    playerResponse = true;                   

                    GameWindow game = new GameWindow(UserName, info.FromUser, true, serialnumber,Channel,Client);                    
                    //The player UserName now in played list in the server
                    await Client.InsertUser2PlayedAsync(new UserModel { UserName = info.FromUser.ToString() });
                    
                    game.ShowDialog();
                }
                //receive a reject play message 
                else if (info.Type == MessageType.Reject)
                {
                    playerResponse = true;
                    MessageBox.Show(info.Message);
                }
                else // equal to info.Type == MessageType.Message
                {
                    //Query window to get status if UserName want to paly with info.FromUser
                    string message = $"{UserName}: {info.Message}";
                    string title = "Close Window";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, buttons);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        serialnumber = (await Client.GetNextSerialNumberAsync(new Empty())).Serialnumber;
                        ChatMessage startGameMessage = new ChatMessage
                        {
                            Type = MessageType.Approved,
                            FromUser = UserName,
                            ToUser = info.FromUser,
                            Message = UserName + " accepted your request " + serialnumber.ToString()
                        };

                        await Client.SendMessageAsync(startGameMessage);
                        //INIT GAME 2
                        GameWindow game = new GameWindow(UserName, info.FromUser, false, serialnumber, Channel, Client);
                        await Client.InsertUser2PlayedAsync(new UserModel { UserName = info.FromUser.ToString() });                       
                        game.ShowDialog();
                    }
                    else
                    {
                        await Client.SendMessageAsync(new ChatMessage
                        {
                            Type = MessageType.Reject,
                            FromUser = UserName,
                            ToUser = info.FromUser,
                            Message = UserName + " rejected your request"
                        });
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------
        //update the online users in the server
        private async Task UpdateUsersListAsync()
        {
            var users = (await Client.UpdateUsersAsync(new Empty())).UserNames;
            users.Remove(UserName);
            lbUsers.ItemsSource = users;
        }
        //---------------------------------------------------------------------------------
        //when window is closed also we close all the environment
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //remove the UserName from played list on the server if exist           
            await Client.DisconnectAsync(new UserModel { UserName = UserName });
            //System.Windows.Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }
        //---------------------------------------------------------------------------------
        //when user select other user to play with 
        private async void lbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbUsers.SelectedItem == null)
            {
                lbUsers.UnselectAll();
                lbUsers.SelectedIndex = -1;
                return;
            }

            if (playerResponse == false)
            {
                lbUsers.UnselectAll();
                lbUsers.SelectedIndex = -1;
                MessageBox.Show("A request has been sent, please wait for a response");
                return;
            }

            //Send request to the selected player 
            string receiver = lbUsers.SelectedItem as string;
            lbUsers.UnselectAll();
            lbUsers.SelectedIndex = -1;

            if ((await Client.CheckUsertInPlayedAsync(new UserModel { UserName = UserName })).Istrue)
            {

                MessageBox.Show("You are in a game, finish the current game or exit");
                return;
            }

            if ((await Client.CheckUsertInPlayedAsync(new UserModel { UserName = receiver })).Istrue)
            {

                MessageBox.Show("This player in a game please select onther player or wait");
                return;
            }

            try
            {
                playerResponse = false;
                await Client.SendMessageAsync(new ChatMessage
                {
                    Type = MessageType.Message,
                    FromUser = UserName,
                    ToUser = receiver,
                    Message = UserName + " Want to play wth you"
                });
                
            }
            catch (RpcException ex)
            {
                MessageBox.Show($"{ex.StatusCode}: {ex.Message}");
            }

        }
        //---------------------------------------------------------------------------------
        //search button opens a search window
        private void Searchbtn_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow sw = new SearchWindow();
            sw.Show();
        }      
    }
}