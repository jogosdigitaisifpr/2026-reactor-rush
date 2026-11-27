using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Instância Singleton para acesso global
    public static DialogueManager Instance { get; private set; }

    [Header("Referências de UI")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject nextButton;

    [Header("Configurações")]
    [SerializeField] private float typingDelay = 0.02f;

    private List<string> sentences = new List<string>();
    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private Coroutine typingRoutine;

    private void Awake()
    {
        // Configuração do Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Inicia uma nova conversa a partir de um array de strings enviado por outro script.
    /// </summary>
    public void StartDialogue(string[] newSentences)
    {
        if (newSentences == null || newSentences.Length == 0) return;

        sentences.Clear();
        sentences.AddRange(newSentences);
        currentSentenceIndex = 0;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        DisplayNextSentence();
    }

    /// <summary>
    /// Método para ser vinculado ao evento OnClick() do botão "Avançar".
    /// </summary>
    public void OnNextButtonClicked()
    {
        // Se ainda está digitando o texto atual, completa instantaneamente ao clicar
        if (isTyping)
        {
            StopCoroutine(typingRoutine);
            dialogueText.text = sentences[currentSentenceIndex - 1];
            isTyping = false;
            if (nextButton != null) nextButton.SetActive(true);
            return;
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        // Se chegou ao fim de todas as frases, fecha a janela
        if (currentSentenceIndex >= sentences.Count)
        {
            CloseDialogue();
            return;
        }

        // Inicia o efeito de digitação da frase atual
        typingRoutine = StartCoroutine(TypeTextRoutine(sentences[currentSentenceIndex]));
        currentSentenceIndex++;
    }

    private IEnumerator TypeTextRoutine(string sentence)
    {
        isTyping = true;
        if (nextButton != null) nextButton.SetActive(false);
        dialogueText.text = "";

        for (int i = 0; i <= sentence.Length; i++)
        {
            dialogueText.text = sentence.Substring(0, i);
            yield return new WaitForSeconds(typingDelay);
        }

        isTyping = false;
        if (nextButton != null) nextButton.SetActive(true);
    }

    private void CloseDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        sentences.Clear();
        currentSentenceIndex = 0;
    }
}