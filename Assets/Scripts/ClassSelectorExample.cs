using UnityEngine;

public class ClassSelectorExample : MonoBehaviour
{
    [SerializeReference, ClassSelector]
    SerializedTestClass testSingle;

    [Space(20)]
    [SerializeReference, ClassSelector]
    SerializedTestClass[] testList;
}

[System.Serializable]
public class SerializedTestClass
{
    public string baseString;
}

[System.Serializable]
public class DerivedSerializedClass : SerializedTestClass
{
    public float derivedFloat;
}
