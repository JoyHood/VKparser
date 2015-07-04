using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json;

namespace JoyHoodApp
{
    public class VK
    {
        public static string AddFriend(int userId, string accessToken)
        {
            var api = new VKAPI(accessToken);
            var queryIsMyfriend = new NameValueCollection();
            queryIsMyfriend["user_ids"] = userId.ToString();
            queryIsMyfriend["need_sign"] = "1";
            var isMyfriend = api.ExecuteCommand(Constants.IS_MY_FRIEND, queryIsMyfriend);
            var data = JsonConvert.DeserializeObject<RootJson>(isMyfriend);
            if (data.response != null)
            {
                // check: this user isn't my friend yet
                var response = data.response.FirstOrDefault();
                if (response != null && response.friend_status == "0")
                {
                    var queryString = new NameValueCollection();
                    queryString["user_id"] = userId.ToString();
                    queryString["text"] = Constants.ADDFRIEND_TEXT;

                    var addToFriends = api.ExecuteCommand(Constants.ADD_TO_MYFRIENDS, queryString);
                    return addToFriends;
                }
            }
            return "-1";
        }
    }

    public class VkFriendProxy
    {
        public string uid { get; set; }
        public string friend_status { get; set; }
        public string sign { get; set; }
    }

    public class RootJson
    {
        public VkFriendProxy[] response { get; set; }
    }
}
