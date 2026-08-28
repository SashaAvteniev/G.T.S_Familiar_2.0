using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    public bool EnableBloom = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await Task.Delay(100);
        GameManager.timekeeper.postProcessVolume.profile.TryGet(out Bloom bloom);
        bloom.active = EnableBloom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
