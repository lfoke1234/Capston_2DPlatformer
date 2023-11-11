using System.Collections.Generic;
using UnityEngine;

namespace RPG.VisualNovel
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/Visual/New Stroy Scene")]
    public class StoryScene : ScriptableObject
    {
        public List<Sentence> sentence;
        public Sprite background;
        public StoryScene nextScene;

        [System.Serializable]
        public struct Sentence
        {
            public string text;
            public Speaker speaker;
        }
    }

}