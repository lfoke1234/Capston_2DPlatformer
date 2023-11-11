using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

namespace RPG.VisualNovel
{
    public class DialogueBoxController : MonoBehaviour
    {
        public TextMeshProUGUI barText;
        public TextMeshProUGUI personNameText;

        private int sentenceIndex = 0;
        public StoryScene currentScene;
        private State state = State.Completed;


        private enum State
        {
            Playing,
            Completed,
        }
        
        public void PlayScene(StoryScene scene)
        {
            currentScene = scene;
            sentenceIndex = -1;
            PlayNextSentence();
        }

        public void PlayNextSentence()
        {
            StartCoroutine(TypeText(currentScene.sentence[++sentenceIndex].text));
            personNameText.text = currentScene.sentence[sentenceIndex].speaker.speakerName;
            personNameText.color = currentScene.sentence[sentenceIndex].speaker.textColor;
        }

        public bool IsLastScentence()
        {
            return sentenceIndex + 1 == currentScene.sentence.Count;
        }

        public bool IsCompleted()
        {
            return state == State.Completed;
        }

        private IEnumerator TypeText(string text)
        {
            barText.text = "";
            state = State.Playing;
            int wordIndex = 0;

            while (state != State.Completed)
            {
                barText.text += text[wordIndex];
                yield return new WaitForSeconds(0.05f);
                if (++wordIndex == text.Length)
                {
                    state = State.Completed;
                    break;
                }
            }
        }
    }
}
