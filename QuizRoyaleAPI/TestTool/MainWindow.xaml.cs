using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        IList<MassClient> holder;
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
            holder = new List<MassClient>();
            for (int j = 0; j < playersToAdd; j++)
            {
                MassClient mockClient = new MassClient(playersToAdd);
                await mockClient.Join((j % 10).ToString());
                holder.Add(mockClient);
                Thread.Sleep(100);
            }
        }

        private async void joinButton_specific_Click(object sender, RoutedEventArgs e)
        {
            int playersToAdd = (int)amountSlider.Value;
            holder = new List<MassClient>();
            for (int j = 0; j < playersToAdd; j++)
            {
                MassClient mockClient = new MassClient(playersToAdd);
                await mockClient.Join((j % 10).ToString());
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
