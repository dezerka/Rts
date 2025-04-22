using UnityEngine;
using UnityEngine.AI;

public class SelectableUnit : MonoBehaviour
{
    public bool isSelected = false;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.color = isSelected ? Color.cyan : Color.white; // Підсвітка
    }

    public void MoveTo(Vector3 position)
    {
        if (isSelected)
        {
            GetComponent<NavMeshAgent>().SetDestination(position);
        }
    }
}
