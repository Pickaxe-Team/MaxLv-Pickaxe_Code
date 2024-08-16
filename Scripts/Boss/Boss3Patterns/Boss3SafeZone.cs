using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3SafeZone : MonoBehaviour
{
    private void OnEnable()
    {
        // 특정 범위 내에서 랜덤 위치 계산
        float randomX = Random.Range(-0.285f, 0.285f);
        float randomY = Random.Range(-0.38f, 0.38f);

        // 랜덤 위치로 설정
        transform.localPosition = new Vector2(randomX, randomY);

    }
}
