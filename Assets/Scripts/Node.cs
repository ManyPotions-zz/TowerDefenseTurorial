using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color NotEnoughMoneyColor;
    public Vector3 positionOffset;
    
    [Header("Optional")]
    public GameObject turret;

    private Renderer rend;
    private Color startColor;
    

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        //Empeche de clicer les node si le UI est over la node
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (turret != null)
        {
            buildManager.SelectNode(this);//Si on peux pas build cest parceque ya une turret then, on peux selectioner pour upgrader ou la vendre.
            Debug.Log("Cant' build there! - to do display on screen");
            return;
        }

        if(!buildManager.CanBuild)
            return;


        buildManager.BuildTurretOn(this);


    }

    void OnMouseEnter()
    {
        //Empeche de clicer les node si le UI est over la node
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;
        
        if (buildManager.HasMoney)
        {
            //Change color of our node when we clic on it
            //sans opimisaion
            // GetComponent<Renderer>().material.color = hoverColor;
            //avec optimisations
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = NotEnoughMoneyColor;
        }



    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
