using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool enableTutorial = true;

    [SerializeField] private GameObject waterRoomWalls;
    [SerializeField] private GameObject handleRoomWalls;
    [SerializeField] private ProblemsManager problemsManager;
    private float frozenOdds;

    void Start()
    {
        if(!enableTutorial)
        {
            gameObject.SetActive(false);
        } 
        else
        {
            SetupTutorial();
        }
    }

    private void SetupTutorial()
    {
        waterRoomWalls.SetActive(true);
        handleRoomWalls.SetActive(true);
        frozenOdds = problemsManager.baseOdds;
        problemsManager.baseOdds = 0f;
    }

    public void ClearValve() 
    {
        waterRoomWalls.SetActive(false);
    }

    public void ClearHandle() 
    {
        handleRoomWalls.SetActive(false);
        problemsManager.baseOdds = frozenOdds;
    }
}
