using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using WrightAngle.Waypoint;

public class Reaction : MonoBehaviour
{
    public static Reaction _instance;

    public bool IsReloaded = true;
    public float waterValveLevel; // 0 to 100 or normalized
    public float rodLevel;        // 0 to 100 (Control rod extraction percentage)
    public float fuelLevel;       
    private float fuelRemaining;

    public float WaterFlow { get; private set; }
    public float Steam { get; private set; }
    public float CurrentEnergyProductionRate { get; private set; }
    public float TotalEnergyAccumulated { get; private set; }

    [field: SerializeField] public float ReactorTemperature { get; private set; }
    [field: SerializeField] public float ReactorPressure { get; private set; }
    [field: SerializeField] public float TurbineTemperature { get; private set; }
    [field: SerializeField] public float TurbinePressure { get; private set; }

    [SerializeField] private WaypointTarget reactorWaypoint;
    [SerializeField] private HUDController hud;

    // Absolute thermodynamic constants
    private const float absoluteZero = 273.15f;
    private const float initialTemperature = 290f; // K (approx 17°C)
    public const float ambientTemperature = 290f; // K
    private const float reactorBaseTemperature = 500f; // K (Base operating temp under minimal system load)
    
    // Limits and critical points
    private const float reactorCriticalTemperature = 621.5f;
    private const float turbineCriticalTemperature = 800f; 
    public const float reactorCriticalPressure = 180f; // Bar
    public const float turbineCriticalPressure = 80f; // Bar
    public const float explosionReactorTemperature = 767f;
    public const float explosionTurbineTemperature = 850f;

    // Rates
    private const float reactorTempChangeRate = 15f;
    private const float reactorPressureChangeRate = 8f;
    private const float turbineTempChangeRate = 12f;
    private const float turbinePressureChangeRate = 6f;
    private const float reloadCoolingRate = 20f;
    private const float waterFlowChangeRate = 15f;

    // Multipliers
    private const float steamProblemMultiplier = 0.5f;
    private const float waterToReactorProblemMultiplier = 0.5f;
    private const float fireReactorHeat = 400f;
    private const float fireTurbineHeat = 300f;
    private const float pressureToEnergyRate = 3.6f;

    public float fuelDuration = 240f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        ReactorTemperature = initialTemperature;
        TurbineTemperature = initialTemperature;
        fuelRemaining = fuelDuration;
    }

    private void OnEnable()
    {
        Timer.Tick += OnTick;
    }

    private void OnDisable()
    {
        Timer.Tick -= OnTick;
    }

    private void Update()
    {
        if (ProblemIndentifier()) return;
        
        GetWaterFlow();
        GetReactorTemperature();
        GetReactorPressure();
        GetTurbinePressure();
        GetTurbineTemperature();
        GetEnergyProduction();
    }

    private bool ProblemIndentifier()
    {
        if (ReactorTemperature >= explosionReactorTemperature || TurbineTemperature >= explosionTurbineTemperature)
        {
            PlayerStatus._instance.Die();
            return true;
        }
        
        if (ReactorPressure >= reactorCriticalPressure)
        {
            PlayerStatus._instance.steamProblem = true;
        }
        return false;
    }

    private void GetWaterFlow()
    {
        float targetFlow;
        // Water flow  bounded by valve input
        if(PlayerStatus._instance.waterProblem)
        {
            targetFlow = waterValveLevel*waterFlowChangeRate;
        }
        else
        {
            targetFlow = waterValveLevel;
        }

        WaterFlow = Mathf.MoveTowards(WaterFlow, targetFlow, waterFlowChangeRate * Time.deltaTime);
    }

    private void GetReactorTemperature()
    {
        if (ReloadController.Instance.needReloading)
        {
            ReactorTemperature = Mathf.MoveTowards(ReactorTemperature, ambientTemperature, reloadCoolingRate * Time.deltaTime);
            return;
        }

        float fissionHeatGeneration = (rodLevel / 100f) * 600f; 
    
        float coolingEfficiency = (WaterFlow / 100f) * 0.8f;
        float coreCooling = coolingEfficiency * (ReactorTemperature - ambientTemperature);

        float targetTemperature = ambientTemperature + fissionHeatGeneration - coreCooling;

        if (PlayerStatus._instance.fireProblem)
        {
            targetTemperature += fireReactorHeat;
        }

        // avoid temperatures below ambient
        targetTemperature = Mathf.Max(targetTemperature, ambientTemperature);
        ReactorTemperature = Mathf.MoveTowards(ReactorTemperature, targetTemperature, reactorTempChangeRate * Time.deltaTime);
    }

    private void GetReactorPressure()
    {
        // PV = nRT
        float thermalDelta = Mathf.Max(0f, ReactorTemperature - ambientTemperature);
        float targetPressure = 1f + (thermalDelta * 0.35f); 

        ReactorPressure = Mathf.MoveTowards(ReactorPressure, targetPressure, reactorPressureChangeRate * Time.deltaTime);
    }

    private void GetTurbinePressure()
    {
        // Turbine pressure is fed directly by the mass flow coming from reactor pressure
        float targetTurbinePressure = ReactorPressure * 0.85f;

        // Severe steam leaks dump pressure straight into the environment rather than building up at the turbine stage
        if (PlayerStatus._instance.steamProblem)
        {
            targetTurbinePressure *= steamProblemMultiplier;
        }

        TurbinePressure = Mathf.MoveTowards(TurbinePressure, targetTurbinePressure, turbinePressureChangeRate * Time.deltaTime);
    }

    private void GetTurbineTemperature()
    {
        //pressure drops convert work/heat to the environment
        // It warms based directly on the steam work
        float targetTemperature = ambientTemperature + (TurbinePressure * 6.5f);

        if (PlayerStatus._instance.fireProblem)
        {
            targetTemperature += fireTurbineHeat;
        }

        TurbineTemperature = Mathf.MoveTowards(TurbineTemperature, targetTemperature, turbineTempChangeRate * Time.deltaTime);
    }

    private void GetEnergyProduction()
    {
        //conversion of fluid pressure into electrical energy units
        CurrentEnergyProductionRate = (TurbinePressure * pressureToEnergyRate) - (0.85f*pressureToEnergyRate);//dont produce energy on zero;
        if(CurrentEnergyProductionRate > 0f)
        {
            TotalEnergyAccumulated += CurrentEnergyProductionRate * Time.deltaTime;
        }
    }

    private void SpendFuel()
    {
        if (fuelRemaining <= 0) return;
        
        fuelRemaining -= 1;
        if (fuelRemaining <= 0)
        {
            Debug.Log("Running out of fuel! Needs reloading. Go to the reloading station");
            if (ReloadController.Instance != null)
            {
                ReloadController.Instance.needReloading = true;
                hud.TooggleReloadAlert();
            } 
            reactorWaypoint?.ActivateWaypoint();

        }
    }

    private void ReloadFuel()
    {
        if (!ReloadController.Instance.needReloading )
        {
            fuelRemaining = fuelDuration;
            reactorWaypoint?.DeactivateWaypoint();
        }
    }

    private void OnTick()
    {
        if (fuelRemaining > 0) SpendFuel();
        else ReloadFuel();
    }
}