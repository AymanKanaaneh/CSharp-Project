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
   
    public partial class SortWindow : Window
    {

        private GrpcChannel channel;
        private FourInRow.FourInRowClient client;
        private Google.Protobuf.Collections.RepeatedField<string> users;
        //-------------------------------------------------------------------
        //initilize the chanel and the client
        public SortWindow()
        {
            InitializeComponent();
            channel = GrpcChannel.ForAddress("https://localhost:5001/");
            client = new FourInRow.FourInRowClient(channel);
            UpdateUsersListAsync();
        }
        //-------------------------------------------------------------------
        //update the users list to show in listbox
        private async Task UpdateUsersListAsync()
        {
            users =(await client.GetShowedUsersAsync(new Empty())).Usershow;           
            lbusers.ItemsSource = users;
        }
        //-------------------------------------------------------------------
        //reorder the data by username on the click
        private void btn_usernameSort_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string>(users.ToList());
            names.Sort();
            var oc = new ObservableCollection<string>(names);
            lbusers.ItemsSource = oc;

        }
        //-------------------------------------------------------------------
        //reorder the data by number of game on the click
        private void btn_gamesnumberSort_Click(object sender, RoutedEventArgs e)
        {
            var winnersort = new List<string>(users.ToList());
            string[] variable;
            List<string[]> sorted = new List<string[]>();
            foreach(var item in winnersort)
            {
                variable = item.Split(' ');
                sorted.Add(variable);
            }
            sorted = new List<string[]>(sorted.OrderBy(sorted => int.Parse(sorted[3])));           
            winnersort = new List<string>();
            foreach (var v in sorted)
            {
                winnersort.Add(string.Join(" ",v));
            }
            
            var oc = new ObservableCollection<string>(winnersort);
            lbusers.ItemsSource = oc;
            
        }
        //-------------------------------------------------------------------
        //reorder the data by loses on the click
        private void btn_losesSort_Click(object sender, RoutedEventArgs e)
        {
            var winnersort = new List<string>(users.ToList());
            string[] variable;
            List<string[]> sorted = new List<string[]>();
            foreach (var item in winnersort)
            {
                variable = item.Split(' ');
                sorted.Add(variable);
            }
            sorted = new List<string[]>(sorted.OrderBy(sorted => int.Parse(sorted[7])));
            winnersort = new List<string>();
            foreach (var v in sorted)
            {
                winnersort.Add(string.Join(" ", v));
            }
            var oc = new ObservableCollection<string>(winnersort);
            lbusers.ItemsSource = oc;
        }
        //-------------------------------------------------------------------
        //reorder the data by number of points on the click
        private void btn_points_Sort_Click(object sender, RoutedEventArgs e)
        {
            var winnersort = new List<string>(users.ToList());
            string[] variable;
            List<string[]> sorted = new List<string[]>();
            foreach (var item in winnersort)
            {
                variable = item.Split(' ');
                sorted.Add(variable);
            }
            sorted = new List<string[]>(sorted.OrderBy(sorted => int.Parse(sorted[9])));
            winnersort = new List<string>();
            foreach (var v in sorted)
            {
                winnersort.Add(string.Join(" ", v));
            }
            var oc = new ObservableCollection<string>(winnersort);
            lbusers.ItemsSource = oc;
        }
        //-------------------------------------------------------------------
        //reorder the data by number of wins on the click
        private void btn_winnsSort_Click(object sender, RoutedEventArgs e)
        {
            var winnersort = new List<string>(users.ToList());
            string[] variable;
            List<string[]> sorted = new List<string[]>();
            foreach (var item in winnersort)
            {
                variable = item.Split(' ');
                sorted.Add(variable);
            }
            sorted = new List<string[]>(sorted.OrderBy(sorted => int.Parse(sorted[5])));
            winnersort = new List<string>();
            foreach (var v in sorted)
            {
                winnersort.Add(string.Join(" ", v));
            }
            var oc = new ObservableCollection<string>(winnersort);
            lbusers.ItemsSource = oc;
        }
    }
}
