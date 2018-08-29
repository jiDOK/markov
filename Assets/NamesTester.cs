using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class NamesTester : MonoBehaviour
{
    public string[] names;
    //public string[] namesFromFile;
    public List<string> namesFromAsset;
    public int order;
    public int minLenght;
    MarkovNameGenerator generator;

    void Start()
    {
        //namesFromFile = File.ReadAllLines("./Assets/Resources/Names.txt");// funktioniert so nicht im Build
        TextAsset nameAsset = Resources.Load("Names") as TextAsset;
        namesFromAsset = new List<string>(nameAsset.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
        generator = new MarkovNameGenerator(namesFromAsset, order, minLenght);
        for (int i = 0; i < 10; i++)
        {
            var name = generator.NextName;
            Debug.Log(name);
        }
    }
}
