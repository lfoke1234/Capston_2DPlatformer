using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] GameObject startButton;

    public TMP_Text txtName;
    public TMP_Text txtSentence;

    Queue<string> sentences = new Queue<string>();

    public Animator anim;

    public void Begin(Dialogue info)
    {
        anim.SetBool("isOppen", true);

        sentences.Clear();

        txtName.text = info.name;

        foreach (var sentence in info.sentence) 
        {
            sentences.Enqueue(sentence);
        }
        Next();
    }

    public void Next()
    {
        if (sentences.Count == 0)
        {
            End();
            return;
        }

        // txtSentence.text = sentences.Dequeue();
        txtSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (var item in sentence)
        {
            txtSentence.text += item;
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    private void End()
    {
        txtSentence.text = string.Empty;
        startButton.SetActive(true);
        anim.SetBool("isOppen", false);
    }
}
