using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlider : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    private IStoreSliderUpdate _sliderUpdatee;
    public void Setup(IStoreSliderUpdate sliderUpdatee)
    {
        _sliderUpdatee = sliderUpdatee;
        _slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    public void MaxMinSliderValues(int maxSellable, int maxBuyable)
    {
        _slider.maxValue = 0 + maxBuyable;
        _slider.minValue = 0 - maxSellable;
        _slider.value = 0;
    }

    private void OnSliderValueChanged()
    {
        int value = (int)_slider.value;
        _sliderUpdatee.StoreSliderUpdateUI(value);
    }
}
