using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private RectTransform BackgroundA;
    [SerializeField] private RectTransform BackgroundB;
    private float _scrollSpeed = 150f;
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
        _backgroundWidth = BackgroundA.rect.width;

        BackgroundB.localPosition = new Vector2(BackgroundA.localPosition.x + _backgroundWidth, BackgroundA.localPosition.y);
    }

    private void ScrollBackground()
    {
        BackgroundA.localPosition += _scrollSpeed * Time.deltaTime * Vector3.left;
        BackgroundB.localPosition += _scrollSpeed * Time.deltaTime * Vector3.left;

        if (BackgroundA.localPosition.x <= -_backgroundWidth)
        {
            BackgroundA.localPosition = new Vector2(BackgroundB.localPosition.x + _backgroundWidth, BackgroundA.localPosition.y);
        }

        if (BackgroundB.localPosition.x <= -_backgroundWidth)
        {
            BackgroundB.localPosition = new Vector2(BackgroundA.localPosition.x + _backgroundWidth, BackgroundB.localPosition.y);
        }
    }
    #endregion
}
