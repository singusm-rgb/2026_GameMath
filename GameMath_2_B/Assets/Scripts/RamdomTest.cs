using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomTest : MonoBehaviour
{
    void Start()
    {
        float chance = Random.value;
        int dice = Random.Range (1, 7);

        System.Random sysRand = new System.Random();
        int number = sysRand.Next(1, 7);

        Debug.Log("Unity Random (Random.value): " + chance);
        Debug.Log("Unity Random (Random.Range): " + dice);
        Debug.Log("System.Random (Next): " + number);
    }
}
