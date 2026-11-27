using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ProblemWarningIcons : MonoBehaviour
{
    [SerializeField] private GameObject[] problemObjects;
    private Image[] problemIcons;
    [SerializeField] private Sprite[] problemSprites;
    private List<int> activeProblems = new List<int>();

    private void Start()
    {
        problemIcons = new Image[problemObjects.Length];
        for (int i = 0; i < problemObjects.Length; i++)
        {
            problemIcons[i] = problemObjects[i].GetComponent<Image>();
            problemIcons[i].gameObject.SetActive(false);
        }

        UpdateIcons();
    }

    private void OnDisable()
    {
        foreach (var icon in problemObjects)
        {
            icon.SetActive(false);
        }
    }

    public void AddProblem(int problemIndex)
    {
        if (!activeProblems.Contains(problemIndex))
        {
            activeProblems.Add(problemIndex);
            UpdateIcons();
        }
    }

    public void RemoveProblem(int problemIndex)
    {
        if (activeProblems.Contains(problemIndex))
        {
            activeProblems.Remove(problemIndex);
            UpdateIcons();
        }
    }

    private void UpdateIcons()
    {
        for (int i = 0; i < problemIcons.Length; i++)
        {
            if (i < activeProblems.Count)
            {
                problemObjects[i].SetActive(true);
                problemIcons[i].sprite = problemSprites[activeProblems[i]];
            }
            else
            {
                problemObjects[i].SetActive(false);
            }
        }
    }
}
