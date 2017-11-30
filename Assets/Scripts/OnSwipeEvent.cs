using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwipeEvent : MonoBehaviour {

    public void OnSwipe(SwipeGesture gesture)
    {
        // 完整的滑动数据
        Vector2 move = gesture.Move;

        // 滑动的速度
        // float velocity = gesture.Velocity;

        // 大概的滑动方向
        FingerGestures.SwipeDirection direction = gesture.Direction;

        // 控制player行动
        HAHAController player = HAHAController.getHaHaInstance();
        player.Move(direction.ToString());

        // Debug
        // Debug.Log("OnSwipe,move=" + move.ToString() + ",velocity=" + velocity + ",direction=" + direction.ToString());
    }
}
