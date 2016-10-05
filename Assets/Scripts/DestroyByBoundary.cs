using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
    [SerializeField]
    int _levelToLoad;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Application.LoadLevel(_levelToLoad);
        }
    }

}
