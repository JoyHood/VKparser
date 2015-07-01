using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
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

        private IEnumerable<int> GetGroupMembers(string groupId, string accessToken)
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

        private void WriteMembers(IEnumerable<int> members)
        {
            using (var writer = new StreamWriter(Constants.FILE_MEMBERS, false))
            {
                foreach (var member in members)
                {
                    writer.WriteLine(member);
                }
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
    }
}
