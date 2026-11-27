using UnityEngine;
using System.Collections;

public class ProblemsManager : MonoBehaviour
{
    [field: SerializeField] public GameObject[] problems { get; protected set; }
    [field: SerializeField] public float baseOdds { get; set; } // Base factor in percentage of the probability of the problem to appear
    [field: SerializeField] public float[] odds { get; protected set; } // Indivitual multiplyer of the probability of the problem to appear
    public float[] occurrences { get; protected set; } // Number of times the problem has appeared in the current run
    [field: SerializeField] public float[] maxOccurrences { get; protected set; } // Maximum number of times the problem can appear in a run
    [field: SerializeField] public float problemCooldown { get; protected set; } // Cooldown time in seconds before any problem can be triggered again
    private ProblemWarningIcons problemWarningIcons;

    private void OnEnable()
    {
        Timer.Tick += OnTick;
    }

    private void OnDisable()
    {
        Timer.Tick -= OnTick;
    }

    private void Start()
    {
        problemWarningIcons = this.gameObject.GetComponent<ProblemWarningIcons>();
        foreach (var problem in problems) // Ensure all problems start inactive
        {
            problem.SetActive(false);
        }

        occurrences = new float[problems.Length];
    }

    private void OnTick()
    {
        for (int i = 0; i < problems.Length; i++)
        {
            CheckAndTriggerProblem(i);
        }
    }

    private void CheckAndTriggerProblem(int index)
    {
        float occurrenceChance = baseOdds * odds[index];
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < occurrenceChance)
        {
            if(occurrences[index] >= maxOccurrences[index]) return;
            if(problems[index].activeInHierarchy) return;

            TriggerProblem(index);
        }
    }

    public void TriggerProblem(int index)
    {
        Debug.Log($"Problem {problems[index].name} triggered!");
        problems[index].SetActive(true);
        occurrences[index]++;
        problemWarningIcons.AddProblem(index);
        StartCoroutine(ProblemCooldown());
    }

    private IEnumerator ProblemCooldown()
    {
        float frozenOdds = baseOdds;
        baseOdds = 0f; 
        yield return new WaitForSeconds(problemCooldown); // Cooldown time before any problem can be triggered again
        baseOdds = frozenOdds;
    }    
}