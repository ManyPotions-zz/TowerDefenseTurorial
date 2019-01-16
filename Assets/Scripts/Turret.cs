using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private Transform target;
    private Enemy targetEnemy;


    [Header("General")]

    public float range = 15f; //range du weapon

    [Header("Use Bullets (default)")]

    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f; //Fire Rate div by FireCountdown 

    [Header("Use Lazer")]

    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowAmount = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
   


    [Header("Unity Setup")] // should not be change by the User.


    public string enemyTag = "Enemy";

    public Transform partTorRotate;
    public float turnSpeed = 10f;

    
    public Transform firePoints; // From where our bullet is fire.

       
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);   //ca va caller la UpdateTarget
    }

    void UpdateTarget () // check for the closet target and check if the closet target is in range.
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //Cherche tout les enemeni du level un cherchan par Tag.
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); //distence between our Enemy and or positions.

            if (distanceToEnemy < shortesDistance)
            {
                shortesDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortesDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>(); // Je get mes information de ma target ici pour ne pas a avoir a regarder a chaque frame.
        } else //jai besoin d'un else pour que la  turret perde ca target quand elle n'est plus en range.
        {
            target = null;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (target == null) //check la turret a perdu ca target
        {
            if (useLaser) // check si la turret utilisait un laser
            {
                if (lineRenderer.enabled) // si oui on diable le laser et l'animaition d'impact du lsaser.
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }

            return;
        }
           

        //Firest thing to do Aquire target.
        LockOnTarget();


        //Check if  the turret use a laser
        if (useLaser)
        {
            Laser();
        }
        else //then that mean its bullet time.
        {
            //Check if its time to shoot for the fireRate stuff
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }



    }

    void LockOnTarget()
    {
        //----------------
        //Target lock on
        //----------------

        //to get the direction to A to B, you get the END direction in this case "B" minus "A".
        Vector3 dir = target.position - transform.position; //dir is for direction
        //Quaternion is Unity way dealing w/rotaions
        Quaternion lookRotaion = Quaternion.LookRotation(dir);
        //Les angles d'Euler sont un ensemble de trois angles pour décrire l'orientation d'un solide ou celle d'un référentiel par rapport à un trièdre cartésien de référence.
        //se sont les Angles entre  de X,Y,Z de la tourelle par apport a X,Y,Z de la scene.
        //le parametre "lerp" sert a smoother une transition entre 2 state. position, color, etc..
        Vector3 rotation = Quaternion.Lerp(partTorRotate.rotation, lookRotaion, Time.deltaTime * turnSpeed).eulerAngles;
        //we only want to rotate on the Y axe
        partTorRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        //before the graphyc stuff
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        //slowing the target
        targetEnemy.Slow(slowAmount);
        
        

        //si on avais deja une target et que le lineRenderer a ete desactiver (le laser) on le reenamble.
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
            

        //Set position 0 et 1 sont les point de debut et fin du laser. aka lineRenderer.
        lineRenderer.SetPosition(0, firePoints.position);
        lineRenderer.SetPosition(1, target.position);

        //on a besoin de la direction de la turret par apport a la target Target-Turret = position
        Vector3 dir = firePoints.position - target.position;
        // fait suivre l'effect du laser au bou du laser aka sur la target.
        // le "+ dir.normalized * 0.5f" cest pour deplacer un peux ver l'exterieur de notre monstre.
        impactEffect.transform.position = target.position + dir.normalized;
        //apres faut lui fair fair un 180
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);



    }

    void Shoot()
    {
        //Debug.Log("SHOOT!");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoints.position, firePoints.rotation); //disable for debug 1/12/19-9:33am
        Bullet bullet = bulletGO.GetComponent<Bullet>(); //disable for debug 1/12/19-9:33am
                                                         //Instantiate(bulletPrefab, firePoints.position, firePoints.rotation);

        if (bullet != null)
            bullet.Seek(target);
        
    }

    void OnDrawGizmosSelected () //fonctions de debug pour tracer le range du weapon.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range); //range du Gizmos cest le meme range que le weapon.
    }
}
