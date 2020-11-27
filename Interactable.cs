using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float radius = 3f;

    private GameObject player;
    private UIManager uiManager;
    private bool isInteractable;

    private void Awake()
    {
        player = FindObjectOfType<MoveController>().gameObject;
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        SetInteractable(false);
    }

    private void Update()
    {
        // inside radius
        if (Vector3.Distance(transform.position, player.transform.position) <= radius)
        {
            // inside viewport
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPosition.z >= 0 && 0 <= viewportPosition.x && viewportPosition.x <= 1 && 0 <= viewportPosition.y && viewportPosition.y <= 1)
            {
                Debug.Log("interactable");
                uiManager.ShowInteractButton();
                SetInteractable(true);
            }
            else
            {
                uiManager.HideInteractButton();
                SetInteractable(false);
            }
        }

        // adsf
        //if (Input.GetButton("e"))
        //{
        //    if (isInteractable)
        //    {
        //        Debug.Log("interact");
        //    }
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }
}
