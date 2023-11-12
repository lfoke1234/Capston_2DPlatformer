using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.VisualNovel
{
    [CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/Visual Novel/New Choose Scene")]
    [System.Serializable]
    public class ChooseScene : GameScene
    {
        public List<ChooseLabel> labels;

        [System.Serializable]
        public struct ChooseLabel
        {
            public string text;
            public StoryScene nextScene;
        }
    }
}