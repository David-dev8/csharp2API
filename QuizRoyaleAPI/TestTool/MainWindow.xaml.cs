using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        IList<massClient> holder;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5264/GameHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection succesfully started");
                connectButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void joinButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            int playersToAdd = int.Parse(((Button)sender).Tag.ToString());
            holder = new List<massClient>();
            for (int j = 0; j < playersToAdd; j++)
            {
                massClient mockClient = new massClient(playersToAdd);
                await mockClient.join((j % 10).ToString());
                holder.Add(mockClient);
                Thread.Sleep(100);
            }
        }

        private async void joinButton_specific_Click(object sender, RoutedEventArgs e)
        {
            int playersToAdd = (int)amountSlider.Value;
            holder = new List<massClient>();
            for (int j = 0; j < playersToAdd; j++)
            {
                massClient mockClient = new massClient(playersToAdd);
                await mockClient.join((j % 10).ToString());
                holder.Add(mockClient);
                Thread.Sleep(100);
            }
        }

        private void amountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            joinButton_specific.Content = "Add " + e.NewValue + " bots";
        }
    }
}
