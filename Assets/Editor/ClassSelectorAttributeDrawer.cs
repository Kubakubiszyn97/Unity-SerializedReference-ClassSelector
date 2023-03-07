using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ClassSelectorAttribute))]
public class ClassSelectorAttributeDrawer : PropertyDrawer
{
    private Dictionary<System.Type, SubclassSelector> selectorsLookup = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference) return;
        
        var classType = GetFieldInfoType();
        if (classType == null) return;

        SubclassSelector selector;
        if (!selectorsLookup.TryGetValue(classType, out selector))
        {
            selector = new SubclassSelector(classType);
            selectorsLookup.Add(classType, selector);
        }

        var fieldPosition = EditorGUI.PrefixLabel(position, label);
        selector.DrawDropdown(fieldPosition, property);
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);

    private Type GetFieldInfoType()
    {
        var propertyType = fieldInfo.FieldType;
        if (propertyType.IsGenericType && typeof(IList).IsAssignableFrom(propertyType))
        {
           return propertyType.GenericTypeArguments[0];
        }
        else if (propertyType.IsArray)
        {
            return propertyType.GetElementType();
        }
        else
        {
           return propertyType;
        }
    }
}