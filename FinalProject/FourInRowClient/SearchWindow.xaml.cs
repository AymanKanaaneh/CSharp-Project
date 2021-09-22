using Grpc.Net.Client;
using GrpcFIRService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace FourInRowClient
{
    public partial class SearchWindow : Window
    {
        public  SearchWindow()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------
        //open games window (games list)
        private void games_Click(object sender, RoutedEventArgs e)
        {
            Games games = new Games();
            games.Show();
        }
        //-------------------------------------------------------------------
        //open the ongoing games window
        private void ongoing_games_Click(object sender, RoutedEventArgs e)
        {
            OngoingGames ongoing = new OngoingGames();
            ongoing.Show();
        }
        //-------------------------------------------------------------------
        //open the users window
        private void users_Click(object sender, RoutedEventArgs e)
        {
            UsersSearch usersSearch = new UsersSearch();
            usersSearch.Show();
        }
        //-------------------------------------------------------------------
        //open the sort window
        private void sort_Click(object sender, RoutedEventArgs e)
        {

            SortWindow sortWindow = new SortWindow();
            sortWindow.Show();
        }
    }
}
