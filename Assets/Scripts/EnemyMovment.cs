
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovment : MonoBehaviour
{
    private Transform target;
    private int wavepointsIndex = 0;

    private Enemy enemy;


    void Start()
    {

        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f) //if we have reach the waypoint go next waypoint
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;

    }

    void GetNextWaypoint()
    {
        if (wavepointsIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        wavepointsIndex++;
        target = Waypoints.points[wavepointsIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }

}