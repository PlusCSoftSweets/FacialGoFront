using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem {
    public string user_id { get; set; }
    public string nickname { get; set; }
    public string avatar { get; set; }
    public int exp { get; set; }
    public int diamand { get; set; }
}

static public class GlobalUserInfo {

    static public UserItem userInfo = new UserItem();

    static public void setInstance(LoginSceneManagerController.UserItem user) {
        if (user.avatar == null) user.avatar = "";
        userInfo.avatar = user.avatar;
        userInfo.diamand = user.diamand;
        userInfo.exp = user.exp;
        userInfo.nickname = user.nickname;
        userInfo.user_id = user.user_id;
    }
}

