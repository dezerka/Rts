using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Squad squad;

    void Start()
    {
        if (squad == null)
        {
            squad = GetComponentInParent<Squad>();
        }
    }
}
