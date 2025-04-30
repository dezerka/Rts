using System.Collections.Generic;
using UnityEngine;
public enum Team
{
    None,
    Player,
    Enemy,
    Neutral
}
public class Squad : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public bool isSelected;
    public Team team = Team.None;

    public GameObject squadIconPrefab;
    private GameObject iconInstance;
    private void Awake()
    {
        // Automatically assign team to all units
        foreach (var unit in units)
        {
            unit.AssignSquad(this);
        }
    }
    private void Start()
    {
        if (squadIconPrefab != null && units.Count > 0)
        {
            iconInstance = Instantiate(squadIconPrefab);
            var iconScript = iconInstance.GetComponent<SquadIcon>();
            iconScript.target = GetSquadCenter(); // можна вибрати юніта або розрахувати середину
        }
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
        float spacing = 1.5f;
        int rows = Mathf.CeilToInt(Mathf.Sqrt(units.Count));
        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i];
            var agent = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                int row = i / rows;
                int col = i % rows;
                Vector3 offset = new Vector3(col * spacing, 0, row * spacing);
                agent.SetDestination(position + offset);
            }
        }
    }
    Transform GetSquadCenter()
    {
        Vector3 average = Vector3.zero;
        foreach (var unit in units)
        {
            average += unit.transform.position;
        }
        average /= units.Count;

        GameObject temp = new GameObject("SquadCenter");
        temp.transform.position = average;
        temp.transform.SetParent(this.transform);
        return temp.transform;
    }



}
