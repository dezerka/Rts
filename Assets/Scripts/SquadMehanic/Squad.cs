using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public bool isSelected;

    private void Start()
    {
        units.Clear();
        foreach (Transform child in transform)
        {
            Unit unit = child.GetComponent<Unit>();
            if (unit != null)
            {
                unit.squad = this;
                units.Add(unit);
            }
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        foreach (var unit in units)
        {
            // Visual feedback (e.g., highlight or enable outline)
            Renderer r = unit.GetComponent<Renderer>();
            if (r != null)
                r.material.color = selected ? Color.green : Color.white;
        }
    }

    public void MoveTo(Vector3 position)
    {
        // Spread units slightly to avoid overlap
        float spacing = 1.5f;
        int rows = Mathf.CeilToInt(Mathf.Sqrt(units.Count));
        for (int i = 0; i < units.Count; i++)
        {
            int row = i / rows;
            int col = i % rows;
            Vector3 offset = new Vector3(col * spacing, 0, row * spacing);
            units[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(position + offset);
        }
    }
}
