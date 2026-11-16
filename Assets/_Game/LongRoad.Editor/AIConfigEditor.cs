using LongRoad.Core.PersonAI.Abstractions;
using LongRoad.Core.Scriptables;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LongRoad.Editor
{
    [CustomEditor(typeof(PersonAIConfig))]
    public class AIConfigEditor : UnityEditor.Editor
    {
        public PersonAIConfig Current => (PersonAIConfig)target;

        private string[] AIClassNames = Array.Empty<string>();

        private void OnEnable()
        {
            var personAIbase = typeof(PersonAIBase);

            AIClassNames = typeof(GameManager).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && personAIbase.IsAssignableFrom(type))
                .Select(type => type.Name)
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            Current.Bravery = EditorGUILayout.Slider("Храбрость", Current.Bravery, 0f, 1f);
            Current.Generosity = EditorGUILayout.Slider("Щедрость", Current.Generosity, 0f, 1f);
            Current.Kindness = EditorGUILayout.Slider("Доброта", Current.Kindness, 0f, 1f);
            Current.Patience = EditorGUILayout.Slider("Терпение", Current.Patience, 0f, 1f);

            var currentType = AIClassNames.ToList().FindIndex(i => i == Current.AITypeName);
            currentType = currentType == -1 ? 0 : currentType;
            Current.AITypeName = AIClassNames[EditorGUILayout.Popup("Тип интелекта", currentType, AIClassNames)];

            if (GUI.changed)
            {
                EditorUtility.SetDirty(Current);
            }
        }
    }
}