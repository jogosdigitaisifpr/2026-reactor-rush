using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject infoProblem;
    
    [Header("Diálogos")]
    [TextArea] [SerializeField] private string[] firstInteractionDialogue;
    [TextArea] [SerializeField] private string[] secondInteractionDialogue;

    private bool visited;
    private float lastInteractionTime;

    public void FixInfo()
    {
        if(PlayerStatus._instance.infoCrashProblem == false)return;
        // PRIMEIRA INTERAÇÃO
        if (!visited && PlayerStatus._instance.infoCrashProblem)
        {
            visited = true;
            lastInteractionTime = Time.time;

            // Inicia o primeiro diálogo
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(firstInteractionDialogue);
            }

            return;
        }

        // SEGUNDA INTERAÇÃO (Somente após 10 segundos da primeira)
        if (visited && Time.time - lastInteractionTime >= 10f)
        {
            if (infoProblem != null)
            {
                infoProblem.SetActive(false);
            }

            // Inicia o segundo diálogo
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(secondInteractionDialogue);
            }

            visited = false; 
        }
    }
}