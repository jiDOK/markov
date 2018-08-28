using UnityEngine;

public class NamesTester : MonoBehaviour
{
    public string[] names;
    public int order;
    public int minLenght;
    MarkovNameGenerator generator;

    void Start()
    {
        generator = new MarkovNameGenerator(names, order, minLenght);
        for (int i = 0; i < 10; i++)
        {
            var name = generator.NextName;
            Debug.Log(name);
        }
    }
}
