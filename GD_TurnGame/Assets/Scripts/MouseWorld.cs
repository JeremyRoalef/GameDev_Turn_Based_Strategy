using Unity.VisualScripting;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(Physics.Raycast(ray));
    }
}
