using System;
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
            public List<Action> actions;

            [System.Serializable]
            public struct Action
            {
                public Speaker speaker;
                public int spriteIndex;
                public Type actionType;
                public Vector2 coords;
                public float moveSpeed;

                [System.Serializable]
                public enum Type
                {
                    None,
                    Appear,
                    Move,
                    Disappear,
                }
            }
        }
    }
}