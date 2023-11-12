using System.Collections.Generic;
using UnityEngine;

namespace RPG.VisualNovel
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/Visual Novel/New Story Scene")]
    [System.Serializable]
    public class StoryScene : GameScene
    {
        public List<Sentence> sentences;
        public Sprite background;
        public GameScene nextScene;

        [System.Serializable]
        public struct Sentence
        {
            public string text;
            public Speaker speaker;
        }
    }
}