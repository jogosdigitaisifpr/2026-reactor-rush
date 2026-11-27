using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeFX : MonoBehaviour
{
    [Header("Configurações")]
    public float delay = 0.01f;
    [TextArea] public List<string> sentences;

    [Header("Referências")]
    [SerializeField] private TMP_Text textMeshElement;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject windowToClose;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingRoutine;

    private void Awake()
    {
        if (textMeshElement == null)
            textMeshElement = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (sentences.Count > 0)
        {
            AdvanceText();
        }
    }

    // Função que será vinculada ao OnClick() do Botão
    public void AdvanceText()
    {
        if (isTyping)
        {
            StopCoroutine(typingRoutine);
            textMeshElement.text = sentences[currentIndex - 1];
            isTyping = false;
            if (nextButton != null) nextButton.SetActive(true);
            return;
        }

        if (currentIndex >= sentences.Count)
        {
            if (windowToClose != null)
            {
                windowToClose.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
            return;
        }

        typingRoutine = StartCoroutine(ShowText(sentences[currentIndex]));
        currentIndex++;
    }

    private IEnumerator ShowText(string textToType)
    {
        isTyping = true;
        if (nextButton != null) nextButton.SetActive(false);

        for (int i = 0; i <= textToType.Length; i++)
        {
            textMeshElement.text = textToType.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        if (nextButton != null) nextButton.SetActive(true);
    }
}