using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwipeEvent : MonoBehaviour {

    public static GameObject swipeEvent;

    void Start()
    {
        if (swipeEvent == null) {
            swipeEvent = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void OnSwipe(SwipeGesture gesture)
    {
        // 完整的滑动数据
        Vector2 move = gesture.Move;

        // 大概的滑动方向
        FingerGestures.SwipeDirection direction = gesture.Direction;

        // 控制player行动
        HAHAController player = HAHAController.GetHaHaInstance();
        player.Move(direction.ToString());

        // Debug
        // Debug.Log("OnSwipe,move=" + move.ToString() + ",velocity=" + ",direction=" + direction.ToString());
    }
}
