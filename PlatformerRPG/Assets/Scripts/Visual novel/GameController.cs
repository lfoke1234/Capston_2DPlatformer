using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.VisualNovel
{
    public class GameController : MonoBehaviour
    {
        public StoryScene currentScene;
        public DialogueBoxController bottonBar;
        public BackgroundController backgroundController;

        void Start()
        {
            bottonBar.PlayScene(currentScene);
            backgroundController.SetImage(currentScene.background);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
            {
                if (bottonBar.IsCompleted())
                {
                    if (bottonBar.IsLastScentence())
                    {
                        currentScene = currentScene.nextScene;
                        bottonBar.PlayScene(currentScene);
                        backgroundController.SwitchImage(currentScene.background);
                    }
                    else
                    {
                        bottonBar.PlayNextSentence();
                    }
                }
            }
        }
    }
}