using System.Collections.Specialized;

namespace JoyHoodApp
{
    public class VK
    {
        public static string AddFriend(int userId, string accessToken)
        {
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();
            queryString["user_id"] = userId.ToString();
            queryString["text"] = Constants.ADDFRIEND_TEXT;

            var addToFriends = api.ExecuteCommand(Constants.ADD_TO_MYFRIENDS, queryString);
            return addToFriends;
        }
    }
}
