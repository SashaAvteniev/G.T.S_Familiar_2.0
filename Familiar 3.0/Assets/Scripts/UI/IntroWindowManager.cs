using UnityEngine;

[CreateAssetMenu(fileName = "IntroWindowManager", menuName = "Scriptable Objects/IntroWindowManager")]
public class IntroWindowManager : ScriptableObject
{
    //stores if the player has already scene the intro playtest window
    public bool hasBeenShown = false;
}
