using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoyHoodApp
{
    public static class Constants
    {
        public const int APP_ID = 4977268;
        public const int SCOPE = (int) (Scope.Groups | Scope.Friends);
        public const string ATHORIZE_URL = @"https://oauth.vk.com/authorize?client_id={0}&scope={1}&display=popup&response_type=token";
        public const string REQUEST_URL_PARAMS = @"https://api.vkontakte.ru/method/{0}?access_token={1}&{2}";
        public const string REQUEST_URL = @"https://api.vkontakte.ru/method/{0}?access_token={1}";
        public const string GET_MEMBERS = "groups.getMembers";
        public const string FILE_MEMBERS = @"C:\Users\Zhenya\Documents\Visual Studio 2012\Projects\members.txt";
        public const string GET_FRIENDS = "friends.get";
        public const string GET_INVITED_USERS = "groups.getInvitedUsers";
        public const string INVITE_FRIEND = "groups.invite";
        public const string IS_MEMBER = "groups.isMember";
        public const string ADD_TO_MYFRIENDS = "friends.add";
    }
}
