using System;
using SceneHandling.SoundSystem.Scripts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SoundInfo))]
    public class CustomSoundDrawer : PropertyDrawer
    {
        private SerializedProperty soundImplementationName;
        private SerializedProperty action;
        private SerializedProperty eventReference;
        
        private SerializedProperty instanceVariant;
        
        private SerializedProperty locality;
        private SerializedProperty parameterName;
        private SerializedProperty parameterValue;
        
        private SerializedProperty locationVariant;
        private SerializedProperty locationTransform;

        private SerializedProperty stopMode;

        private int linesSizeBase;
        private int lineSizeEventRef;
        private int lineSizeInstanceVariant;
        private int lineSizePlay;
        private int lineSizeLocationRelated;
        private int lineSizeRemove;
        private int lineSizeParameter;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FindAllProperties(property);
            
            int amountOfLines = 1;
            
            //Fold-outs
            if (property.isExpanded)
            {
                //Todo sizing depending on different types of functionality. EventReference is needed for all when used for example. Shouldn't be recreated nor counted as for multiple lines
                amountOfLines += linesSizeBase;
                
                SoundInfo.SoundAction soundAction = (SoundInfo.SoundAction) action.enumValueFlag;
                if (soundAction != 0)
                {
                    amountOfLines += lineSizeEventRef;
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.Create))
                {
                    amountOfLines += lineSizeInstanceVariant;
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.Location))
                {
                    amountOfLines += lineSizeLocationRelated;
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.ChangeParameter))
                {
                    amountOfLines += lineSizeParameter;
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.Play))
                {
                    amountOfLines += lineSizePlay;
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.Stop))
                {
                    amountOfLines += lineSizeRemove;
                }
            }
            
            return EditorGUIUtility.singleLineHeight * amountOfLines;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Reset of implemented
            //implementedFlagProperties = 0;
            int currentAmountOfLines = 1;
            
            FindAllProperties(property);
            
            EditorGUI.BeginProperty(position, label, property);
            
            Rect foldOutBox = new Rect(position.xMin, position.yMin, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

            if (property.isExpanded)
            {
                DrawBaseRelated(position, ref currentAmountOfLines);
                
                SoundInfo.SoundAction soundAction = (SoundInfo.SoundAction) action.enumValueFlag;
                if (soundAction != 0)
                {
                    DrawInstanceRelated(position, ref currentAmountOfLines);
                    DrawEventReferenceRelated(position, ref currentAmountOfLines);
                    
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.ChangeParameter))
                {
                    DrawParameterRelated(position, ref currentAmountOfLines);
                }
                if (soundAction.HasFlag(SoundInfo.SoundAction.Location))
                {
                    DrawLocationRelated(position, ref currentAmountOfLines);
                }
                /*
                if (soundAction.HasFlag(SoundInfo.SoundAction.Play))
                {
                    DrawPlayRelated(position, ref currentAmountOfLines);
                }*/
                if (soundAction.HasFlag(SoundInfo.SoundAction.Stop))
                {
                    DrawRemoveRelated(position, ref currentAmountOfLines);
                }
                
                
            }
            
            EditorGUI.EndProperty();
        }

        private void FindAllProperties(SerializedProperty property)
        {
            soundImplementationName = property.FindPropertyRelative("soundImplementationName");
            if (soundImplementationName == null) Debug.LogWarning("soundImplementationName is null in PropertyDrawer.");

            action = property.FindPropertyRelative("action");
            if (action == null) Debug.LogWarning("action is null in PropertyDrawer.");

            eventReference = property.FindPropertyRelative("eventReference");
            if (eventReference == null) Debug.LogWarning("eventReference is null in PropertyDrawer.");
            
            instanceVariant = property.FindPropertyRelative("instanceVariant");
            if (eventReference == null) Debug.LogWarning("instanceVariant is null in PropertyDrawer.");

            locality = property.FindPropertyRelative("locality");
            if (locality == null) Debug.LogWarning("locality is null in PropertyDrawer.");

            parameterName = property.FindPropertyRelative("parameterName");
            if (parameterName == null) Debug.LogWarning("parameterName is null in PropertyDrawer.");

            parameterValue = property.FindPropertyRelative("parameterValue");
            if (parameterValue == null) Debug.LogWarning("parameterValue is null in PropertyDrawer.");

            /*playVariant = property.FindPropertyRelative("playVariant");
            if (playVariant == null) Debug.LogWarning("playVariant is null in PropertyDrawer.");*/

            locationVariant = property.FindPropertyRelative("locationVariant");
            if (locationVariant == null) Debug.LogWarning("locationVariant is null in PropertyDrawer.");

            locationTransform = property.FindPropertyRelative("locationTransform");
            if (locationTransform == null) Debug.LogWarning("locationTransform is null in PropertyDrawer.");

            stopMode = property.FindPropertyRelative("stopMode");
            if (stopMode == null) Debug.LogWarning("stopMode is null in PropertyDrawer.");
            
            
        }
        
        private void DrawBaseRelated(Rect position, ref int startLineIndex)
        {
            linesSizeBase = 0;
            int sectionAmountOfLines = 0;
            
            float xPos = position.xMin;
            float yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight;
            Rect drawArea = new Rect(xPos, yPos, width, height);
            
            EditorGUI.PropertyField(drawArea, soundImplementationName,new GUIContent("Implementation-Name"));
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(xPos, yPos, width, height);
            EditorGUI.PropertyField(drawArea, action,new GUIContent("Action"));
            //Extra padding, 1+1
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            linesSizeBase = sectionAmountOfLines;
        }
        
        private void DrawInstanceRelated(Rect position, ref int startLineIndex)
        {
            lineSizeInstanceVariant = 0;
            int sectionAmountOfLines = 0;
            
            float xPos = position.xMin;
            float yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight;
            Rect drawArea = new Rect(xPos, yPos, width, height);
            
            
            //Header
            EditorGUI.LabelField(drawArea,"Create", EditorStyles.boldLabel);
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(xPos, yPos, width, height);
            EditorGUI.PropertyField(drawArea, instanceVariant ,new GUIContent("Instance-Variant"));
            //Extra padding, 1+1
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            lineSizeInstanceVariant = sectionAmountOfLines;
        }
        
        private void DrawLocationRelated(Rect position, ref int startLineIndex)
        {
            lineSizeLocationRelated = 0;
            int sectionAmountOfLines = 0;
            
            //Type of location-handling for audioHandler
            float xPos = position.xMin;
            float yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight;
            Rect drawArea = new Rect(xPos, yPos, width, height);
            
            //Header
            EditorGUI.LabelField(drawArea,"Location", EditorStyles.boldLabel);
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            //Enum
            yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(xPos, yPos, width, height);
            EditorGUI.PropertyField(drawArea, locationVariant,new GUIContent("LocationTypes"));
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            //Transform-property
            yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(xPos, yPos, width, height);
            EditorGUI.PropertyField(drawArea, locationTransform,new GUIContent("Location Transform"));
            //1+1 for extra padding
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            lineSizeLocationRelated = sectionAmountOfLines;
        }
        
        private void DrawRemoveRelated(Rect position, ref int startLineIndex)
        {
            lineSizeRemove = 0;
            int sectionAmountOfLines = 0;
            
            float xPos = position.xMin;
            float yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight;
            Rect drawArea = new Rect(xPos, yPos, width, height);
            
            
            //Header
            EditorGUI.LabelField(drawArea,"Remove", EditorStyles.boldLabel);
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            yPos = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(xPos, yPos, width, height);
            EditorGUI.PropertyField(drawArea, stopMode ,new GUIContent("Stop-Mode"));
            //Extra padding, 1+1
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            lineSizeRemove = sectionAmountOfLines;
        }
        
        private void DrawEventReferenceRelated(Rect position,ref int startLineIndex)
        {
            lineSizeEventRef = 0;
            int sectionAmountOfLines = 0;
            
            float x = position.xMin;
            float y = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight * 2;
            Rect drawArea = new Rect(x, y, width, height);
            
            EditorGUI.PropertyField(drawArea, eventReference,new GUIContent("EventReference"));
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            lineSizeEventRef = sectionAmountOfLines;
        }
        
        /// <summary>
        /// Name, valueChange (float), global or not
        /// </summary>
        /// <param name="position"></param>
        private void DrawParameterRelated(Rect position, ref int startLineIndex)
        {
            lineSizeParameter = 0;
            int sectionAmountOfLines = 0;
            
            float x = position.xMin;
            float y = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            float width = position.size.x;
            float height = EditorGUIUtility.singleLineHeight;
            Rect drawArea = new Rect(x, y, width, height);
            
            //Header
            EditorGUI.LabelField(drawArea, "Parameters", EditorStyles.boldLabel);
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            //Locality
            y = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(x, y, width, height);
            EditorGUI.PropertyField(drawArea, locality,new GUIContent("Locality"));
            startLineIndex += 1;
            sectionAmountOfLines += 1;
            
            //same line
            //ParameterName
            x = position.xMin;
            y = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(x, y, width/2f, height);
            EditorGUIUtility.labelWidth = 50;
            EditorGUI.PropertyField(drawArea, parameterName,new GUIContent("Name"));
            
            //ParameterValue
            x = width/2f;
            y = position.yMin + EditorGUIUtility.singleLineHeight * startLineIndex;
            drawArea = new Rect(x, y, width/2f, height);
            EditorGUI.PropertyField(drawArea, parameterValue,new GUIContent("Value"));
            EditorGUIUtility.labelWidth = default;
            //Extra padding 1+1
            startLineIndex += 2;
            sectionAmountOfLines += 2;
            lineSizeParameter = sectionAmountOfLines;

        }
        
        
        
    }
}
