using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public bool isInteractable = false;

    public bool isFocus = false;

    private Material objectMaterial;
    
    Transform player;
    Shader shader;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehavior>().transform;
        //objectMaterial = gameObject.GetComponent<Renderer>().material;
        //Debug.Log("Material " + objectMaterial);
        //objectMaterial.SetColor("_EmissionMap", Color.black);

    }

    private void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Update()
    {
        Debug.Log("Update");
        if (isFocus && isInteractable)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                Interact();
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interact");
    }
}
