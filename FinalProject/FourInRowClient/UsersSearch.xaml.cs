using Grpc.Net.Client;
using GrpcFIRService;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FourInRowClient {
   
    public partial class UsersSearch : Window {
        ObservableCollection<string> selectedusers = new ObservableCollection<string>();
        ObservableCollection<string> gamesselected = new ObservableCollection<string>();
        private string FirstPlayer;
        private string FirstPlayerUserName;
        private string SecondPlayer;
        private string SecondPlayerUserName;
        private GrpcChannel channel;
        private FourInRow.FourInRowClient client;
        //------------------------------------------------------------------
        //initilize the chanel and client
        public UsersSearch() {
            InitializeComponent();
            channel = GrpcChannel.ForAddress("https://localhost:5001/");
            client = new FourInRow.FourInRowClient(channel);
            FirstPlayer = null;
            SecondPlayer = null;
            lbShow.ItemsSource = selectedusers;
            initAsynk();
        }
        //-------------------------------------------------------------------
        //initilize the users from the server
        private async Task initAsynk() {
            var users = await client.GetAllUsersAsync(new Empty());
            lbUsersSearch.ItemsSource = users.Usershow;
        }
        //-------------------------------------------------------------------
        //search the games between the selected users
        private void btn_search_Click(object sender, RoutedEventArgs e) {
            if (FirstPlayer == null || SecondPlayer == null) {
                MessageBox.Show("must select 2 players");
                return;
            }
            showdata();

        }
        //show the data between the 2 selected users
        private async void showdata() {
            var fp = await client.GetUserByUserNameAsync(new NameString { Name = FirstPlayerUserName });
            var sp = await client.GetUserByUserNameAsync(new NameString { Name = SecondPlayerUserName });
            UsersModel usersModel = new UsersModel
            {
                User1 = fp,
                User2 = sp
            };
            var show = await client.GetShowedGamesBetween2UsersAsync(usersModel);
            if (show.Gamesnumber == 0) {
                string x = "No matches between " + fp.UserName + " and " + sp.UserName;
                gamesselected = new ObservableCollection<string>();
                gamesselected.Add(x);
                lbgames.ItemsSource = gamesselected;
                return;
            }
            gamesselected = new ObservableCollection<string>(show.Gameshow);
            double p1 = (show.P1Wins * 100 / show.Gamesnumber);
            double p2 = (show.P2Wins * 100 / show.Gamesnumber);
            var user1percent = fp.UserName + " won " + p1.ToString() + "%";
            var user2percent = sp.UserName + " won " + p2.ToString() + "%";
            gamesselected.Add(user1percent);
            gamesselected.Add(user2percent);
            lbgames.ItemsSource = gamesselected;

        }
        //-------------------------------------------------------------------
        //show the games, percent of winning and number of winnings when the user selected 
        private void lbUsersSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            string receiver = lbUsersSearch.SelectedItem as string;
            if (FirstPlayer != null && SecondPlayer != null) {
                selectedusers.Remove(FirstPlayerUserName);
                selectedusers.Add(receiver);
                FirstPlayer = SecondPlayer;
                FirstPlayerUserName = SecondPlayerUserName;
                initSecondPlayer(receiver);
            }
            else if (FirstPlayer != null && SecondPlayer == null) {
                selectedusers.Add(receiver);
                initSecondPlayer(receiver);
            }
            else {
                selectedusers.Add(receiver);
                initFirstPlayer(receiver);
            }
        }
        //-------------------------------------------------------------------
        //initilize the first player ,get it from the server 
        private async void initFirstPlayer(string receiver) {
            FirstPlayerUserName = receiver;
            FirstPlayer = receiver;
            NameString ns = new NameString
            {
                Name = receiver
            };
            var player = await client.GetUserByUserNameAsync(ns);
            double percent = 0;
            if (player.NumOfGames != 0) {
                percent = player.NumOFWins * 100 / player.NumOfGames;
                FirstPlayer = "played:" + player.NumOfGames + " ,won:" + player.NumOFWins + " " +
                percent.ToString() + "% " + " ,points: " + player.Points;
            }
            else {
                FirstPlayer = "he/she didn't play.";
            }
            MessageBox.Show(FirstPlayer);
        }
        //-------------------------------------------------------------------
        //initilize the second player ,get it from the server 
        private async void initSecondPlayer(string receiver) {
            SecondPlayer = receiver;
            SecondPlayerUserName = receiver;
            NameString ns = new NameString
            {
                Name = receiver
            };
            var player = await client.GetUserByUserNameAsync(ns);
            double percent = 0;
            if (player.NumOfGames != 0) {
                percent = (player.NumOFWins * 100 / player.NumOfGames);
                SecondPlayer = "played:" + player.NumOfGames + " ,won:" + player.NumOFWins + " " +
                percent.ToString() + "% " + " ,points: " + player.Points;
            }
            else {
                SecondPlayer = "he/she didn't play.";
            }
            MessageBox.Show(SecondPlayer);
        }
    }
}
