using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
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
            connection.On<string>("succes", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On<string>("failed", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On<string>("updatestatus", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On("start", () =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"Het spel start nu!";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On<string, string, dynamic>("newQuestion", (catName, question, Awnsers) => 
            {
                this.Dispatcher.Invoke(() => {
                    var newMessage = $"vraag in de catogorie {catName}, de vraag is {question}!";
                    messagesList.Items.Add(newMessage);
                    messagesList.Items.Add(Awnsers);
                });
            });

            connection.On<char>("answerQuestion", (awnser) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"Het antwoord dat jij koos was {awnser}";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On<bool>("result", (result) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (result)
                    {
                        messagesList.Items.Add("Dat was het goede antwoord!");
                    }
                    else 
                    {
                        messagesList.Items.Add("Je hebt niet het goede antwoord gegeven");
                    }
                });
            });

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("answerQuestion", messageTextBox.Text);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void joinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("join", userTextBox.Text);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void joinButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                massClient mockClient = new massClient();
                await mockClient.join(i.ToString());
            }
        }
    }
}
