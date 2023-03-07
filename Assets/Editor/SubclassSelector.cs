using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SubclassSelector
{
    private Type type;
    private int currentIndex = -1;

    public List<Type> MatchingTypes { get; private set; }
    public string[] MatchingTypesNames => MatchingTypes.Select(t => t.Name).ToArray(); 

    public SubclassSelector(Type type)
    {
        this.type = type;
        GetAllMatchingTypes();
    }

    private void GetAllMatchingTypes()
    {
        MatchingTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.IsSubclassOf(type) || type == t).ToList();
    }

    public void DrawDropdown(Rect position, SerializedProperty property)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        currentIndex = GetCurrentPropertyTypeIndex(property);
        var selectedIndex = EditorGUI.Popup(position, currentIndex, MatchingTypesNames);
        if (currentIndex != selectedIndex)
        {
            currentIndex = selectedIndex;
            var selectedType = MatchingTypes[currentIndex];
            property.managedReferenceValue = SubclassSelector.CreateInstance(selectedType);
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    private int GetCurrentPropertyTypeIndex(SerializedProperty property)
    {
        if (property.managedReferenceValue == null)
        {
            return -1;
        }
        else
        {
            var currentType = property.managedReferenceValue.GetType();
            return MatchingTypes.IndexOf(currentType);
        }
    }

    public static object CreateInstance<T>(T type) where T: Type
    {
        return Activator.CreateInstance(type);
    }
}