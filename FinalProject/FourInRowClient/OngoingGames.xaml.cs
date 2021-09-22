using Grpc.Net.Client;
using GrpcFIRService;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FourInRowClient
{
    /// <summary>
    /// Interaction logic for OngoingGames.xaml
    /// </summary>
    public partial class OngoingGames : Window
    {
        private GrpcChannel channel;
        private FourInRow.FourInRowClient client;
        //initilize the client and the chanel
        public OngoingGames()
        {
            InitializeComponent();
            channel = GrpcChannel.ForAddress("https://localhost:5001/");
            client = new FourInRow.FourInRowClient(channel);
            initAsynk();
        }
        //get the ongoing games from the server
        private async Task initAsynk()
        {
            var games = (await client.GetShowedOngoingGamesAsync(new Empty())).Gameshow;
            lbOngoingGames.ItemsSource = games;
        }
    }
}
