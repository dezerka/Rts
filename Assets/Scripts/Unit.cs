using UnityEngine;

public class Unit : MonoBehaviour
{
    public Squad squad;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent agent;
    public Squad squadOwner;
    void Start()
    {
        if (squad == null)
        {
            squad = GetComponentInParent<Squad>();
        }
        animator.speed = Random.Range(0.9f, 1.1f);
    }

    void Update()
    {
        if (agent != null && animator != null)
        {
            float speed = agent.velocity.magnitude;
            Debug.Log("Speed: " + speed); // <-- додай це
            animator.SetFloat("Speed", speed);
        }
    }
    public Team GetTeam()
    {
        if (squadOwner != null)
        {
            return squadOwner.team;
        }
        return Team.None;
    }
    public void AssignSquad(Squad squad)
    {
        squadOwner = squad;
    }


}
