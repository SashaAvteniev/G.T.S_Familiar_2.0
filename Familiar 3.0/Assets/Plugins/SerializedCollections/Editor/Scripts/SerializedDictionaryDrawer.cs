using AYellowpaper.SerializedCollections.Editor.Data;
using AYellowpaper.SerializedCollections.Editor.States;
using AYellowpaper.SerializedCollections.KeysGenerators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.Editor
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Editor.DrawerPriority(Sirenix.OdinInspector.Editor.DrawerPriorityLevel.SuperPriority)]
#endif
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        public const string KeyName = nameof(SerializedKeyValuePair<int, int>.Key);
        public const string ValueName = nameof(SerializedKeyValuePair<int, int>.Value);
        public const string SerializedListName = nameof(SerializedDictionary<int, int>._serializedList);
        public const string LookupTableName = nameof(SerializedDictionary<int, int>.LookupTable);

        public const int TopHeaderClipHeight = 20;
        public const int TopHeaderHeight = 19;
        public const int SearchHeaderHeight = 20;
        public const int KeyValueHeaderHeight = 18;
        public const bool KeyFlag = true;
        public const bool ValueFlag = false;
        public static readonly Color BorderColor = new Color(36 / 255f, 36 / 255f, 36 / 255f);
        public static readonly List<int> NoEntriesList = new List<int>();
        internal static GUIContent DisplayTypeToggleContent
        {
            get
            {
                if (_displayTypeToggleContent == null)
                {
                    var texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Plugins/SerializedCollections/Editor/Assets/BurgerMenu@2x.png");
                    _displayTypeToggleContent = new GUIContent(texture, "Toggle to either draw existing editor or draw properties manually.");
                }
                return _displayTypeToggleContent;
            }
        }
        private static GUIContent _displayTypeToggleContent;

        private Dictionary<string, SerializedDictionaryInstanceDrawer> _arrayData = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GetDrawer(property).OnGUI(position, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetDrawer(property).GetPropertyHeight(label);
        }

        private SerializedDictionaryInstanceDrawer GetDrawer(SerializedProperty property)
        {
            string key = GetCacheKey(property);
            if (!_arrayData.TryGetValue(key, out var drawer))
            {
                drawer = new SerializedDictionaryInstanceDrawer(property, fieldInfo);
                _arrayData[key] = drawer;
            }
            else
            {
                drawer.BindProperty(property);
            }

            return drawer;
        }

        private static string GetCacheKey(SerializedProperty property)
        {
            int targetId = property.serializedObject.targetObject != null
                ? property.serializedObject.targetObject.GetInstanceID()
                : 0;
            return $"{targetId}:{property.propertyPath}";
        }
    }
}