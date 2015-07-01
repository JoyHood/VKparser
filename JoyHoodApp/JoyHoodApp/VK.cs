using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace JoyHoodApp
{
    public class VK
    {
        public static string AddFriend(string accessToken)
        {
            var api = new VKAPI(accessToken);
            var queryString = new NameValueCollection();

            string someId = "308491007";

            queryString["user_id"] = someId;
            queryString["text"] = "hello bro";

            var addToFriends = api.ExecuteCommand(Constants.ADD_TO_MYFRIENDS, queryString);
            return addToFriends;
        }
    }
}
