using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private UserData NowUserData => GameManager.Instance.NowUserData;

    // 마지막 수집 시간 반환
    public DateTime GetLastCollectionTime(ItemID itemID)
    {
        if (NowUserData.LastCollectionTimes.TryGetValue(itemID, out DateTime lastTime))
        {
            return lastTime;
        }
        else
        {
            return DateTime.Now; // 초기화되지 않은 경우 현재 시간 반환
        }
    }

    public float GetElapsedTime(ItemID id)
    {
        if (NowUserData.LastCollectionTimes.TryGetValue(id, out DateTime lastTime))
        {
            return (float)(DateTime.Now - lastTime).TotalSeconds;
        }
        else
        {
            return 0f;
        }
    }
}
