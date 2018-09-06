// Copyright 2014-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BlendModes
{
    [CustomEditor(typeof(ComponentExtension), true)]
    public class ComponentExtensionEditor : Editor
    {
        protected ComponentExtension ComponentExtension { get { return target as ComponentExtension; } }

        private const float headerSpace = 2;
        private const float paddingWidth = 5;
        private const float paddingHeight = 4;

        private static readonly GUIContent nameConent = new GUIContent("Name", "Name of the shader property.");
        private static readonly GUIContent typeConent = new GUIContent("Type", "Type of the shader property.");
        private static readonly GUIContent valueConent = new GUIContent("Value", "Value of the shader property.");

        private ReorderableList propertiesList;

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            var propertyCount = 0;
            var property = serializedObject.GetIterator();
            var expanded = true;
            while (property.NextVisible(expanded))
            {
                expanded = false;
                propertyCount++;

                // First three properties are script, extended component and shader props; don't need to draw them here.
                // Fourth and consequent are the properties declared in the component extension implementation.
                if (propertyCount == 4)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Extension Component Properties", EditorStyles.boldLabel);
                }
                if (propertyCount >= 4) EditorGUILayout.PropertyField(property, true);
            }

            if (ComponentExtension.DefaultShaderProperties.Length > 0)
            {
                // Can't initialize the list at OnEnable(), as it causes some wierd serialization issues.
                if (propertiesList == null) InitilizePropertiesList();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Extension Shader Properties", EditorStyles.boldLabel);
                propertiesList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitilizePropertiesList ()
        {
            var shaderProperties = serializedObject.FindProperty("shaderProperties");
            propertiesList = new ReorderableList(serializedObject, shaderProperties, false, true, false, false);
            propertiesList.drawHeaderCallback = DrawBindListHeader;
            propertiesList.drawElementCallback = DrawBindListElement;
            propertiesList.elementHeightCallback = GetElementHeight;
            propertiesList.footerHeight = 0;
        }

        private void DrawBindListHeader (Rect rect)
        {
            var propertyRect = new Rect(rect.x, rect.y, (rect.width / 3f) - paddingWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(propertyRect, nameConent);
            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.LabelField(propertyRect, typeConent);
            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.LabelField(propertyRect, valueConent);
        }

        private void DrawBindListElement (Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!ShouldDrawElement(index)) return;

            var element = propertiesList.serializedProperty.GetArrayElementAtIndex(index);
            var nameProperty = element.FindPropertyRelative("name");
            var typeProperty = element.FindPropertyRelative("type");

            var propertyRect = new Rect(rect.x, rect.y + headerSpace, (rect.width / 3f) - paddingWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(propertyRect, nameProperty, GUIContent.none);
            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.PropertyField(propertyRect, typeProperty, GUIContent.none);
            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.EndDisabledGroup();
            var type = (ShaderPropertyType)typeProperty.enumValueIndex;
            switch (type)
            {
                case ShaderPropertyType.Color:
                    EditorGUI.PropertyField(propertyRect, element.FindPropertyRelative("colorValue"), GUIContent.none);
                    break;
                case ShaderPropertyType.Vector:
                    var property = element.FindPropertyRelative("vectorValue");
                    property.vector4Value = EditorGUI.Vector4Field(propertyRect, GUIContent.none, property.vector4Value);
                    break;
                case ShaderPropertyType.Float:
                    EditorGUI.PropertyField(propertyRect, element.FindPropertyRelative("floatValue"), GUIContent.none);
                    break;
                case ShaderPropertyType.Texture:
                    EditorGUI.PropertyField(propertyRect, element.FindPropertyRelative("textureValue"), GUIContent.none);
                    break;
            }
        }

        private float GetElementHeight (int index)
        {
            return ShouldDrawElement(index) ? EditorGUIUtility.singleLineHeight + paddingHeight : 0f;
        }

        private bool ShouldDrawElement (int index)
        {
            // Don't draw shader properties that doesn't belong to the current render material.
            // This allows us to define properties for multiple shader families, but draw only the relevant ones.
            var element = propertiesList.serializedProperty.GetArrayElementAtIndex(index);
            var propertyName = element.FindPropertyRelative("name").stringValue;
            return ComponentExtension.MaterialHasProperty(propertyName);
        }
    }
}
