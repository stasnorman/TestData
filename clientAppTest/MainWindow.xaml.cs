using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace clientAppTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static int port = 8000;
        static string ip = "127.0.0.1";

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Подключение к серверу
                serverSocket.Connect(ipPoint);
                string message = TxbTextMessage.Text;

                byte[] data = Encoding.Unicode.GetBytes(message);
                serverSocket.Send(data);

                data = new byte[1024];
                StringBuilder clientBuilder = new StringBuilder();
                int bytes = 0;

                do
                { 
                    bytes = serverSocket.Receive(data, data.Length, 0);
                    clientBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (serverSocket.Available > 0);

                TxtBackMessageFromServer.Visibility = Visibility.Visible;
                TxtBackMessageFromServer.Text = clientBuilder.ToString();

                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(
                    ex.Message.ToString(),
                    "Критическая ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }
    }
}
