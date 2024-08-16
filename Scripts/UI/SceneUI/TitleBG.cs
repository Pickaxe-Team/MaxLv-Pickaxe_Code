using UnityEngine;

public class TitleBG : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Transform BackgroundA;
    [SerializeField] private Transform BackgroundB;
    private float _scrollSpeed = 1f;
    private float _backgroundWidth;

    private void Start()
    {
        InitBackground();
    }

    private void Update()
    {
        ScrollBackground();
    }

    #region 배경 스크롤
    private void InitBackground()
    {
        _backgroundWidth = BackgroundA.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        BackgroundB.position = new Vector2(BackgroundA.position.x + _backgroundWidth, BackgroundA.position.y);
    }

    private void ScrollBackground()
    {
        BackgroundA.Translate(_scrollSpeed * Time.deltaTime * Vector2.left);
        BackgroundB.Translate(_scrollSpeed * Time.deltaTime * Vector2.left);

        if (BackgroundA.position.x <= -_backgroundWidth)
        {
            BackgroundA.position = new Vector2(BackgroundB.position.x + _backgroundWidth, BackgroundA.position.y);
        }

        if (BackgroundB.position.x <= -_backgroundWidth)
        {
            BackgroundB.position = new Vector2(BackgroundA.position.x + _backgroundWidth, BackgroundB.position.y);
        }
    }
    #endregion
}