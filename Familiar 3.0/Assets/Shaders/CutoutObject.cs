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
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            for (int m = 0; m < materials.Length; ++m)
            {
                Debug.Log($"Setting cutout for {hitObjects[i].transform.name} material {m}");
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", cutoutSize);
                materials[m].SetFloat("_FalloffSize", falloffSize);
                materials[m].SetFloat("_ShowCutout", showCutout ? 1f : 0f);
            }
        }
    }
}
