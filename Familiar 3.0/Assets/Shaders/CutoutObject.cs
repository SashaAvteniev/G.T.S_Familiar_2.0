using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private bool showCutout = true;
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    
    [SerializeField] private float fadeInDuration = 0.5f;
    
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
        Debug.DrawLine(transform.position, targetObject.position, Color.red);
        UpdateMaterials(centerRaycastHits);
        
        // Left Raycast
        RaycastHit[] leftRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,0,-5), offset.magnitude, wallMask);
        Debug.DrawRay(transform.position, offset + new Vector3(0,0,-5), Color.red);
        UpdateMaterials(leftRaycastHits);
        
        // Right Raycast
        RaycastHit[] rightRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,0,5), offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, targetObject.position + new Vector3(0,0,5), Color.red);
        UpdateMaterials(rightRaycastHits);
        
        // Upper Raycast
        RaycastHit[] upperRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,5,0), offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, targetObject.position + new Vector3(0,5,0), Color.red);
        UpdateMaterials(upperRaycastHits);
        
        // Lower Raycast
        RaycastHit[] lowerRaycastHits = Physics.RaycastAll(transform.position, offset + new Vector3(0,-5,0), offset.magnitude, wallMask);
        Debug.DrawLine(transform.position, targetObject.position + new Vector3(0,-5,0), Color.red);
        UpdateMaterials(lowerRaycastHits);
    }

    void UpdateMaterials(RaycastHit[] hits)
    {
        if (hits.Length == 0)
        {
            /*materials[m].SetVector("_CutoutPos", cutoutPos);
            materials[m].SetFloat("_CutoutSize", cutoutSize);
            materials[m].SetFloat("_FalloffSize", falloffSize);
            materials[m].SetFloat("_ShowCutout", showCutout ? 1f : 0f);*/
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
