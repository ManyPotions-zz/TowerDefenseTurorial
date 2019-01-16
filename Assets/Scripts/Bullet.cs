using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;

    public float speed = 70f;

    public int damage = 50;

    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public void Seek( Transform _target) //or Chase the target
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //we need to find the direction of the bullet need to point in order to look directly to the target.
        //to get the direction to A to B, you get the END direction in this case "B" minus "A".
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        //dir is our bullet direction and Magnitude is the distence between our bullet and the target.
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

       // we normalise beacose no matter are close we are the the target we need to move a the same speed.
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);


    }

    void HitTarget()
    {
        //Debug.Log("We hit something!");
        
        GameObject effectIns =   (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        
        Destroy(gameObject);
    }

    void Explode()
    {
        //pour savoir ce qui ce fait hit par explosion. la grosseur de la sphere est determiner par l'esplosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        //Verifier si e a bien un Component
        if (e != null)
        {
            e.TakeDamage(damage);
        }

        

     
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
