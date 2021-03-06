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
using System.Windows.Shapes;
using Telegram.Bot;

namespace exemple_10
{
    /// <summary>
    /// Логика взаимодействия для MyDialog.xaml
    /// </summary>
    public partial class MyDialog : Window
    {
        string selectedUser;
        TelegramBotClient client;
        public MyDialog(string selectedUser, TelegramBotClient client)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            this.client = client;
        }
        /// <summary>
        /// отправить сообщение выбранному пользователю.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = answerUser.Text;
            client.SendTextMessageAsync(selectedUser, text);
        }
    }
}
