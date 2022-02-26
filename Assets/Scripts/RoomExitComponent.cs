using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExitComponent : MonoBehaviour
{
    public const string targetRoomPrefsKey = "targetEntry";
    public string thisEntryName;
    public string targetScene;
    public string targetEntryName;
    public Transform entryOffset;
    void Start()
    {
    }

    void Update()
    {
        
    }

    public Vector3 GetSpawnPosition() {
        return entryOffset.position;
    }

    // -1 facing left
    // 1 facing right
    // to be multiplied with scale.x
    public float GetFacingDirection() {
        return Mathf.Sign(entryOffset.position.x - transform.position.x);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player") {
            PlayerPrefs.SetString(targetRoomPrefsKey, targetEntryName);
            SceneManager.LoadSceneAsync(targetScene);
        }
    }
}
