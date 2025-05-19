using UnityEngine;

public class MoveMarke : MonoBehaviour
{
    [SerializeField] private float lifeTIme = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTIme);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
