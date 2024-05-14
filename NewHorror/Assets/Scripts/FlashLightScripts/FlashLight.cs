using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    private PlayerControls input;
    private new Light light;
    public BatteryManager batteryManager;

    private float maxIntensity = 5f;
    public float lightBattery;
    public float maxBattery;
    private float batteryDrainRate;
    private float rechargeRate;
    private bool isLightOn = false;
    private float lightCutOff;

    private void Awake()
    {
        input = new PlayerControls();
    }

    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;

        //Sets default stats to the flashlight
        maxBattery = 20f;
        lightBattery = maxBattery;
        batteryDrainRate = 1f;
        rechargeRate = 2f;
        lightCutOff = maxBattery / 5f;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    void Update()
    {
        if (isLightOn)
        {
            DrainBattery();
            if(lightBattery <= 0)
            {
                isLightOn = false;
                light.enabled = false;
            }
        }
        else
        {
            RechargeBattery();
        }
        DoLight();
    }

    private void DoLight()
    {
        if(input.Player.Light.triggered)
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        isLightOn = !isLightOn;
        light.enabled = isLightOn;
    }

    private void DrainBattery()
    {
        lightBattery -= batteryDrainRate * Time.deltaTime;
        batteryManager.UseBattery();
        if(lightBattery <= maxBattery/lightCutOff)
        {
            light.intensity = Mathf.Lerp(0f, maxIntensity, lightBattery * lightCutOff / maxBattery);
        }
    }

    private void RechargeBattery()
    {
        lightBattery = Mathf.Min(maxBattery, lightBattery + rechargeRate * Time.deltaTime);
        light.intensity = Mathf.Lerp(0f, maxIntensity, lightBattery / lightCutOff);
        batteryManager.FillBattery();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
