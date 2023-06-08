using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestModuleSO")]
public class TestModuleSO : ScriptableObject
{
    public GameObject prefab;
    public int Id;
    public int[] connections;
}
