using UnityEngine;
using UnityEngine.InputSystem;

public class DebugPresentationCommands : MonoBehaviour
{

    [SerializeField] private GameObject fireProblem;
    [SerializeField] private GameObject vaporProblem;
    [SerializeField] private GameObject roupaProblem;
    [SerializeField] private GameObject aguaProblem;
    [SerializeField] private GameObject aguaForteProblem;
    [SerializeField] private GameObject comunicacaoProblem;

    private InputAction fogoAction;
    private InputAction roupaAction;
    private InputAction vaporAction;
    private InputAction aguaAction;
    private InputAction aguaForteAction;
    private InputAction comunicacaoAction;
    private InputAction reabAction;
    private InputAction ASSDESTROYERACTION;
    private InputAction ASSSAVIOURACTION;


    private void Awake()
    {
        fogoAction = InputSystem.actions.FindAction("Debug/Fogo");
        roupaAction = InputSystem.actions.FindAction("Debug/Roupa");
        vaporAction = InputSystem.actions.FindAction("Debug/Vapor");
        aguaAction = InputSystem.actions.FindAction("Debug/Agua");
        aguaForteAction = InputSystem.actions.FindAction("Debug/AguaForte");
        comunicacaoAction = InputSystem.actions.FindAction("Debug/Comunicacao");
        reabAction = InputSystem.actions.FindAction("Debug/Reab");
        ASSDESTROYERACTION = InputSystem.actions.FindAction("Debug/AssDestroyer");
        ASSSAVIOURACTION = InputSystem.actions.FindAction("Debug/AssSaviour");


    }

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Debug")?.Enable();
    }

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Debug")?.Disable();
    }

    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        if (fogoAction != null && fogoAction.WasPressedThisFrame())
        {
            if (fireProblem != null) fireProblem.SetActive(!fireProblem.activeSelf);
            return;
        }
        if (roupaAction != null && roupaAction.WasPressedThisFrame())
        {
            if (roupaProblem != null) roupaProblem.SetActive(!roupaProblem.activeSelf);
            return;
        }
        if (vaporAction != null && vaporAction.WasPressedThisFrame())
        {
            if (vaporProblem != null) vaporProblem.SetActive(!vaporProblem.activeSelf);
            return;
        }
        if (aguaAction != null && aguaAction.WasPressedThisFrame())
        {
            if (aguaProblem != null) aguaProblem.SetActive(!aguaProblem.activeSelf);
            return;
        }
        if (aguaForteAction != null && aguaForteAction.WasPressedThisFrame())
        {
            if (aguaForteProblem != null) aguaForteProblem.SetActive(!aguaForteProblem.activeSelf);
            return;
        }
        if (comunicacaoAction != null && comunicacaoAction.WasPressedThisFrame())
        {
            if (comunicacaoProblem != null) comunicacaoProblem.SetActive(!comunicacaoProblem.activeSelf);
            return;
        }
        if (reabAction != null && reabAction.WasPressedThisFrame())
        {
            ReloadController.Instance.needReloading = true;
            return;
        }
        if(ASSDESTROYERACTION != null && ASSDESTROYERACTION.WasPressedThisFrame())
        {
            PlayerStatus._instance.Die();
            return;
        }
        if(ASSSAVIOURACTION != null && ASSSAVIOURACTION.WasPressedThisFrame())
        {
            PlayerStatus._instance.TimeUp();
            return;
        }
    }
}

