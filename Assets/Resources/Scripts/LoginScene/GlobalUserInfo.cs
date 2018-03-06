using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GlobalUserInfo {
    public class UserItem {
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public int exp { get; set; }
        public int diamand { get; set; }

        public static implicit operator UserItem(LoginSceneManagerController.UserItem v)
        {
            throw new NotImplementedException();
        }
    }
    static public UserItem userInfo;
}