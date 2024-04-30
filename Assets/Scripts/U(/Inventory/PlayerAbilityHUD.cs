using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityHUD : MonoBehaviour
{
    [SerializeField]
    protected Image _inventoryUIImage;
    [SerializeField]
    protected Image _inventoryUICooldown;

    public void Setup(Sprite mainImage)
    {
        _inventoryUIImage.sprite = mainImage;
    }
    public IEnumerator UILerpFill(float abilityActiveTime)
    {
        float duration = Time.time + abilityActiveTime;
        while (Time.time <= duration)
        {
            float timeRemaining = duration - Time.time;
            _inventoryUICooldown.fillAmount = Mathf.Lerp(1, 0, timeRemaining / abilityActiveTime);
            yield return null;
        }
        _inventoryUICooldown.fillAmount = 0f;
    }
}
