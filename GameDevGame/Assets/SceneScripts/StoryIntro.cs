using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryIntroManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public float typingSpeed = 0.05f;
    public string[] storyLines;
    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (!isTyping && Input.anyKeyDown)
        {
            currentLine++;
            if (currentLine < storyLines.Length)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        storyText.text = "";
        foreach (char c in storyLines[currentLine])
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}
