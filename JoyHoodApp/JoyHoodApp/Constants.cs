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
        public const string FILE_MEMBERS = @"D:\test\members.txt";
        public const string DIRECTORY = @"D:\test\";
        public const string GET_FRIENDS = "friends.get";
        public const string GET_INVITED_USERS = "groups.getInvitedUsers";
        public const string INVITE_FRIEND = "groups.invite";
        public const string IS_MEMBER = "groups.isMember";
        public const string ADD_TO_MYFRIENDS = "friends.add";
        public const string IS_MY_FRIEND = "friends.areFriends";
        public const string ADDFRIEND_TEXT = "Привет!" +
                                             "Присоединяйся к нам. " +
                                             "JoyHood - удобная площадка для того, чтобы выгодно купить или продать детские вещи (детскую одежду, обувь, коляски и пр.). У нас есть много классных предложений. " +
                                             "Вещи на любой вкус. Ассортимент постоянно пополняется";

        public const int DAY_LIMIT = 45;
    }
}
