using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace JoyHoodApp
{
    /// <summary>
    /// Логика взаимодействия для GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        private int count;
        private IEnumerable<int> _invitedUsers; 


        public GroupWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string accessToken = AccessToken.Content.ToString();
            var members = GetGroupMembers(GroupId.Text, accessToken);
            WriteMembers(members);
        }

        private List<int> GetGroupMembers(string groupId, string accessToken)
        {
            var take = 1000;
            var offset = 0;
            var items = new List<int>();
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();
            queryString["group_id"] = groupId;
            queryString["count"] = "1";
            string jsonString = api.ExecuteCommand(Constants.GET_MEMBERS, queryString);
            var json = JObject.Parse(jsonString);
            count = (int)json["response"]["count"];
            while (offset <= count)
            {
                Thread.Sleep(500);
                queryString["count"] = take.ToString();
                queryString["offset"] = offset.ToString();
                jsonString = api.ExecuteCommand(Constants.GET_MEMBERS, queryString);
                json = JObject.Parse(jsonString);
                items.AddRange(((JArray)json["response"]["users"]).Select(item => (int)item));
                offset = offset + take+1;
            }
            return items;
        }

        private void WriteMembers(List<int> members)
        {
            try
            {
                var limit = Math.Floor((double) members.Count/50);
                for (int i = 0; i < limit; i++)
                {
                    using (var writer = new StreamWriter(String.Format(Constants.FILE_MEMBERS, i), false))
                    {
                        for(int j = 0; j<50; j++)
                        {
                            writer.WriteLine(members[i+j]);
                        }
                    }
                }

                using (var writer = new StreamWriter(String.Format(Constants.FILE_MEMBERS, limit), false))
                {
                    for (int i = 0; i < members.Count - ((int)limit * 50); i++)
                    {
                        writer.WriteLine((int)limit * 50 + i);
                    }
                }
                MessageBox.Show("All users from group was written in file");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Some error {0}", ex.Message));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string accessToken = AccessToken.Content.ToString();
            string groupId = GroupForInvitationId.Text;
            GetInvitedUsers(groupId, accessToken);
            var friends = GetFriends(accessToken);
            foreach (var friend in friends)
            {
                if (ShouldBeAdded(friend, groupId, accessToken))
                {
                    Invite(friend, groupId, accessToken);
                }
            }
        }

        private IEnumerable<int> GetFriends(string accessToken)
        {
            var friends = new List<int>();
            var api = new VKAPI(accessToken);
            string jsonString = api.ExecuteCommand(Constants.GET_FRIENDS, null);
            var json = JObject.Parse(jsonString);
            friends.AddRange(((JArray)json["response"]).Select(friend => (int)friend));
            return friends;
        }

        private void GetInvitedUsers(string groupId, string accessToken)
        {
            var take = 1000;
            var offset = 0;
            var users = new List<int>();
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();
            queryString["group_id"] = groupId;
            queryString["count"] = "1";
            string jsonString = api.ExecuteCommand(Constants.GET_INVITED_USERS, queryString);
            var json = JObject.Parse(jsonString);
            count = (int)json["response"][0];
            while (offset <= count)
            {
                Thread.Sleep(500);
                queryString["count"] = take.ToString();
                queryString["offset"] = offset.ToString();
                jsonString = api.ExecuteCommand(Constants.GET_INVITED_USERS, queryString);
                json = JObject.Parse(jsonString);
                for (int i = 1; i <= count; i++)
                {
                    users.Add((int)json["response"][i]["uid"]);
                }
                offset = offset + take + 1;
            }
            _invitedUsers = users;
        }

        private bool IsMember(int userId, string groupId, string accessToken)
        {
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();
            queryString["group_id"] = groupId;
            queryString["user_id"] = userId.ToString();
            string jsonString = api.ExecuteCommand(Constants.IS_MEMBER, queryString);
            var json = JObject.Parse(jsonString);
            return (bool)json["response"];
        }

        private bool ShouldBeAdded(int userId, string groupId, string accessToken)
        {
            return !IsMember(userId, groupId, accessToken) && !_invitedUsers.Contains(userId);
        }

        private void Invite(int userId, string groupId, string accessToken)
        {
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();
            queryString["group_id"] = groupId;
            queryString["user_id"] = userId.ToString();
            api.ExecuteCommand(Constants.INVITE_FRIEND, queryString);
        }

        private void AddToFriend_Click(object sender, RoutedEventArgs e)
        {
            // Макс. заявок в друзья в сутки – 50 заявок
            try
            {
                var result = MessageBox.Show("The daily limit for adding to friends in the VC is 50 ! Run once a day! Continue?", 
                    "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    int counter = 0;
                    string accessToken = AccessToken.Content.ToString();
                    using (var sr = new StreamReader(tbFile.Text))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null && counter < 50)
                        {
                            int userId = 0;
                            int.TryParse(line, out userId);
                            if (userId > 0)
                            {
                                VK.AddFriend(userId, accessToken);
                                ++counter;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Some error {0}", ex.Message));
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt", 
                Filter = "TXT Files (*.txt)|*.txt"
            };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbFile.Text = filename;
            }
        }
    }
}
