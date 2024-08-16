using UnityEngine;
using UnityEngine.EventSystems;

public class SFXSliderPointer : MonoBehaviour, IPointerUpHandler
{
    private GameManager GM => GameManager.Instance;

    public void OnPointerUp(PointerEventData eventData)
    {
        GM.PlaySFX(SFX.Click);
    }
}
