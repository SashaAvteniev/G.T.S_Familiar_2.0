using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public abstract class InteractParent : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private bool inRange = false;
    
    [SerializeField]
    [HideInInspector]
    private Outline outline;

    [SerializeField]
    [HideInInspector]
    private string id;

    public string ID => id;

    private void OnValidate()
    {
        if(string.IsNullOrEmpty(id))
        {
            GenerateId();
        }
    }

    public void GenerateId()
    {
        // We add the active scene name so we can know which scene to load for the new door
        id = SceneManager.GetActiveScene().name + "_" + System.Guid.NewGuid();
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }

    void Start()
    {
        if(name.Contains("(Clone)"))
        {
            GenerateId();
        }
    }
    
    private void CopyGuidToClipboard()
    {
        if(!string.IsNullOrEmpty(id))
        {
            GUIUtility.systemCopyBuffer = id;
            return;
        }
        Debug.LogError("[id] Can't copy GUID, it is empty");
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame && inRange)
        {
            Interact();
        }
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player") && !GameManager.gameData.interactFlag)
        {
            inRange = true;
            if(!TryGetComponent<Outline>(out outline))
            {
                outline = this.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = 5f;
            }
            outline.enabled = true;
            GameManager.gameData.interactFlag = true;
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            outline.enabled = false;
            GameManager.gameData.interactFlag = false;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        GameManager.gameData.interactFlag = false;
        GameManager.gameData.newDoorGUID = "";
    }

    protected abstract void Interact();
}
