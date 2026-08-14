using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// The master interact class, universal interact functionality can be added here
public abstract class InteractParent : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private bool inRange = false;
    
    [SerializeField]
    [HideInInspector]
    private Outline outline; // Reference to the outline around the object

    [SerializeField]
    [HideInInspector]
    private string id; // The GUID of the object

    public string ID => id;

    private void OnValidate()
    {
        if(string.IsNullOrEmpty(id))
        {
            GenerateId();
        }
    }

    // Generates a GUID that corresponds to the object
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
        // Generates a new ID in case we are a clone of another prefab, preventing mismatches
        // or more than one object having the same ID
        if(name.Contains("(Clone)"))
        {
            GenerateId();
        }

        // This will check if we have an outline already on the object, if we don't it will stay null and be added on overlap.
        // This prevents unneeded interactable from getting an outline.
        TryGetComponent(out outline);
    }
    
    public void CopyGuidToClipboard()
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

    // Will assign a 
    protected virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player") && !GameManager.gameData.interactFlag)
        {
            inRange = true;
            // Creates an outline if we don't already have one
            if(!outline)
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
        // Will change with a save system, but this will prevent old data from persisting after PIE
        GameManager.gameData.interactFlag = false;
        GameManager.gameData.newDoorGUID = "";
        GameManager.gameData.doorExits.Clear();
    }

    // The main interact method that should be overriden in every single interact object
    protected abstract void Interact();
}
