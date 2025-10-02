using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    [System.Serializable]
    private struct GenerateObject
    {
        public GameObject generateObject;
        public int generateNum;
    }

    [SerializeField]
    private List<GenerateObject> sGenerateObjects;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
