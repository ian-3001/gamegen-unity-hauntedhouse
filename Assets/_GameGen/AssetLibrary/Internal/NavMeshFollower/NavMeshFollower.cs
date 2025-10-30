using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollower : MonoBehaviour
{
    [SerializeField] private Transform _toFollow;
    [SerializeField] private NavMeshAgent _agent;

    private Vector3 _lastPosition = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 goalPosition = _toFollow.position;
        float distance = Vector3.Distance(_lastPosition, goalPosition);

        //don't update if the follow hasn't moved
        if (distance < 0.5f) { return; }

        //make sure the position we're looking for is a valid spot on the navmesh (within maxDistance meters)
        float maxDistance = 3;
        int areaMask = NavMesh.AllAreas;
        bool foundGoal = NavMesh.SamplePosition(goalPosition, out NavMeshHit hit, maxDistance, areaMask);

        if (foundGoal)
        {
            _lastPosition = goalPosition;
            _agent.SetDestination(hit.position);
        }
    }
}
