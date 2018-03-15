using UnityEngine;

public class UserInfo : MonoBehaviour {
  public MainSceneMangerController.UserItem userItem;

  public delegate void OnClickHandler(UserInfo info);
  public event OnClickHandler OnClick;

  public void _OnClick() {
    if (OnClick != null) OnClick(this);
  }
}