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
using System.Collections.ObjectModel;
using Telegram.Bot;
using System.IO;
using Newtonsoft.Json;

namespace exemple_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string token = "1917004999:AAEm-Kj33jy6sBlBpu4TNnLCjCk-uEkvho4";
        static TelegramBotClient client;
        string selectedUser;

        ObservableCollection<string> users = new ObservableCollection<string>();
        ObservableCollection<string> userMessages = new ObservableCollection<string>();
        Dictionary<string, List<string>> allMessages = new Dictionary<string, List<string>>();

        public MainWindow()
        {
            
            InitializeComponent();
            InitUsers();
            UserList.ItemsSource = users;
            client = new TelegramBotClient(token);
            client.OnMessage += Client_OnMessage;
            client.StartReceiving();
        
        }
        /// <summary>
        /// получение данных из телеграмм бота
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string userName = string.Join(" ", e.Message.From.FirstName, e.Message.From.LastName, e.Message.From.Id);
            if (allMessages.ContainsKey(userName))
            {
                if(userName==selectedUser)
                {
                    userMessages.Add(e.Message.Text);
                }
            }
            else
            {
                allMessages[userName] = new List<string>();
                Dispatcher.Invoke(() => users.Add(userName));
            }
            allMessages[userName].Add(e.Message.Text);
        }
        /// <summary>
        /// функция выбора пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = (string)UserList.SelectedItem;
            userMessages = new ObservableCollection<string>(allMessages[selectedUser]);
            MessageList.ItemsSource = userMessages;
        }
        /// <summary>
        /// вызов нового окна для написания и отправки сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Click(object sender, RoutedEventArgs e)
        {
            string[] arr = selectedUser.Split(' ');
            new MyDialog(arr[2], client).Show();
        }
        /// <summary>
        /// Сериализация
        /// </summary>
        public void Serialize()
        {
            using (StreamWriter file = new StreamWriter("Data.json"))
            {
                string json = JsonConvert.SerializeObject(allMessages);
                file.Write(json);
            }
            MessageBox.Show("Сериализация успешна!");
        }
        /// <summary>
        /// Сериализация
        /// </summary>
        public void DeSerialize()
        {
            using (StreamReader file = new StreamReader("Data.json"))
            {
                string json;
                json = file.ReadToEnd();
                allMessages = JsonConvert.DeserializeObject< Dictionary<string, List<string>>> (json);
            }
        }

        /// <summary>
        /// десериализация и запись пользователей в список 
        /// </summary>
        public void InitUsers()
        {
            DeSerialize();
            users = new ObservableCollection<string>(allMessages.Keys);
        }
        /// <summary>
        /// Функция сохранения в json файл по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Serialize();
        }
    }
}
