using UnityEngine;

public class calculateMeasure: MonoBehaviour
{
    public GameObject prefab; // Drag and drop your prefab here in the inspector.

    void Start()
    {
        // Instantiate the prefab temporarily
        GameObject tempInstance = Instantiate(prefab);

        // Get Renderer bounds (if the prefab has a Renderer)
        Renderer renderer = tempInstance.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Vector3 dimensions = renderer.bounds.size;
            Debug.Log($"Prefab Dimensions (Renderer): {dimensions}");
        }

        // Get Collider bounds (if the prefab has a Collider)
        Collider collider = tempInstance.GetComponentInChildren<Collider>();
        if (collider != null)
        {
            Vector3 colliderDimensions = collider.bounds.size;
            Debug.Log($"Prefab Dimensions (Collider): {colliderDimensions}");
        }

        // Destroy the temporary instance
        Destroy(tempInstance);
    }
}
