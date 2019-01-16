using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;

        // on va chercher le Build position au lieu de position direct pour que le ui spawn pas au centre de la node
        transform.position = target.GetBuildPosition();
    }
}
