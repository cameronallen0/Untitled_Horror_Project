using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    public Image batteryBar;
    public FlashLight flashLight;

    public void UseBattery()
    {
        batteryBar.fillAmount = flashLight.lightBattery / flashLight.maxBattery;
    }

    public void FillBattery()
    {
        batteryBar.fillAmount = flashLight.lightBattery / flashLight.maxBattery;
    }
}
