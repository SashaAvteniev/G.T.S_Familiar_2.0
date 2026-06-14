using UnityEngine;
using System.Collections.Generic;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private bool showCutout = true;
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    [SerializeField, Range(0f, 0.5f)] private float viewportRaySpread = 1f;

    private Camera mainCamera;
    private readonly HashSet<Renderer> affectedRenderers = new HashSet<Renderer>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null || mainCamera == null)
        {
            return;
        }

        Vector3 playerViewportPos = mainCamera.WorldToViewportPoint(targetObject.position);
        if (playerViewportPos.z <= 0f)
        {
            return;
        }
        
        List<RaycastHit> allHits = new List<RaycastHit>(16);
        AddHits(playerViewportPos, Vector2.zero, Color.red, "Center", allHits);
        AddHits(playerViewportPos, Vector2.left * viewportRaySpread, Color.blue, "Left", allHits);
        AddHits(playerViewportPos, Vector2.right * viewportRaySpread, Color.green, "Right", allHits);
        AddHits(playerViewportPos, Vector2.up * viewportRaySpread, Color.magenta, "Upper", allHits);
        AddHits(playerViewportPos, Vector2.down * viewportRaySpread, Color.yellow, "Lower", allHits);

        UpdateMaterials(allHits.ToArray());
    }

    private void AddHits(Vector3 centerViewportPoint, Vector2 viewportOffset, Color debugColor, string label, List<RaycastHit> allHits)
    {
        Vector3 sampleViewportPoint = centerViewportPoint + new Vector3(viewportOffset.x, viewportOffset.y, 0f);
        sampleViewportPoint.z = centerViewportPoint.z;
        Vector3 rayEndPoint = GetClampedRayEndPoint(sampleViewportPoint);
        Vector3 rayToEnd = rayEndPoint - transform.position;
        Vector3 rayDirection = rayToEnd.normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, rayDirection, rayToEnd.magnitude, wallMask);
        Debug.DrawRay(transform.position, rayDirection * rayToEnd.magnitude, debugColor);
        Debug.Log(label + ": " + hits.Length);
        allHits.AddRange(hits);
    }

    private Vector3 GetClampedRayEndPoint(Vector3 viewportPoint)
    {
        Vector3 targetPoint = mainCamera.ViewportToWorldPoint(viewportPoint);
        Vector3 origin = transform.position;
        float playerX = targetObject.position.x;

        float xDelta = targetPoint.x - origin.x;
        if (Mathf.Abs(xDelta) < 0.0001f)
        {
            return targetPoint;
        }

        float distanceToPlayerX = (playerX - origin.x) / xDelta;
        distanceToPlayerX = Mathf.Clamp01(distanceToPlayerX);

        return origin + (targetPoint - origin) * distanceToPlayerX;
    }

    void UpdateMaterials(RaycastHit[] hits)
    {
        ClearAffectedRenderers();

        Shader.SetGlobalFloat("_ShowCutout", showCutout ? 1f : 0f);

        if (hits.Length == 0)
        {
            Debug.Log("No Hits");
            return;
        }
        
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        for (int i = 0; i < hits.Length; i++)
        {
            Renderer hitRenderer = hits[i].transform.GetComponent<Renderer>();
            if (hitRenderer == null)
            {
                continue;
            }

            affectedRenderers.Add(hitRenderer);

            Material[] materials = hitRenderer.materials;
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

    private void ClearAffectedRenderers()
    {
        foreach (Renderer affectedRenderer in affectedRenderers)
        {
            if (affectedRenderer == null)
            {
                continue;
            }

            Material[] materials = affectedRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_ShowCutout", 0f);
            }
        }

        affectedRenderers.Clear();
    }
}
