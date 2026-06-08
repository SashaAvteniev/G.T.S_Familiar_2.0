using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private bool showCutout = true;
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    public Vector3 test;
    
    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = targetObject.position - transform.position;
        // Center Raycast
        RaycastHit[] centerRaycastHits = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, offset, Color.red);
        Debug.Log("Center: " + centerRaycastHits.Length);
        UpdateMaterials(centerRaycastHits);
        
        // Left Raycast
        RaycastHit[] leftRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,0,-5), offset.magnitude, wallMask);
        Debug.DrawRay(transform.position, offset + new Vector3(0,0,-5), Color.blue);
        Debug.Log("Left: " + leftRaycastHits.Length);
        UpdateMaterials(leftRaycastHits);
        
        // Right Raycast
        RaycastHit[] rightRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,0,5), offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, offset + new Vector3(0,0,5), Color.green);
        Debug.Log("Right: " + rightRaycastHits.Length);
        UpdateMaterials(rightRaycastHits);
        
        // Upper Raycast
        RaycastHit[] upperRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,5,0), offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, offset + new Vector3(0,5,0), Color.purple);
        Debug.Log("Upper: " + upperRaycastHits.Length);
        UpdateMaterials(upperRaycastHits);
        
        // Lower Raycast
        RaycastHit[] lowerRaycastHits = Physics.RaycastAll(transform.position, offset + test, offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, offset + test, Color.orange);
        Debug.Log("Lower: " + lowerRaycastHits.Length);
        UpdateMaterials(lowerRaycastHits);
    }

    void UpdateMaterials(RaycastHit[] hits)
    {
        if (hits.Length == 0)
        {
            Debug.Log("No Hits");
            Shader.SetGlobalFloat("ShowCutout", 0f);
        }
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        for (int i = 0; i < hits.Length; i++)
        {
            Material[] materials = hits[i].transform.GetComponent<Renderer>().materials;
            for (int m = 0; m < materials.Length; ++m)
            {
                //Debug.Log($"Setting cutout for {hits[i].transform.name} material {m}");
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", cutoutSize);
                materials[m].SetFloat("_FalloffSize", falloffSize);
                materials[m].SetFloat("_ShowCutout", showCutout ? 1f : 0f);
            }
        }
    }
}
