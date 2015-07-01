using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoyHoodApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Token _token;
        
        public MainWindow()
        {
            InitializeComponent();
            Authorize();
        }

        private void Authorize()
        {
            WebBrowser1.Navigate(String.Format(Constants.ATHORIZE_URL, Constants.APP_ID, Constants.SCOPE));
        }

        private void WebBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri.ToString().IndexOf("access_token", StringComparison.Ordinal) != -1)
            {
                string accessToken = String.Empty;
                int userId = 0;
                var regex = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match match in regex.Matches(e.Uri.ToString()))
                {
                    if (match.Groups["name"].Value == "access_token")
                    {
                        accessToken = match.Groups["value"].Value;
                    }
                    else if (match.Groups["name"].Value == "user_id")
                    {
                        userId = Convert.ToInt32(match.Groups["value"].Value);
                    }
                }
                _token = new Token {AccessToken = accessToken, UserId = userId};
                var groupWindow = new GroupWindow();
                groupWindow.AccessToken.Content = _token.AccessToken;
                groupWindow.UserID.Content = _token.UserId;
                groupWindow.Show();
                this.Close();
            }
        }
    }
}
