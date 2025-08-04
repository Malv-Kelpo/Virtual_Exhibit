using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 8f;
    public LayerMask interactableLayer;
    private KeyCode interactKey = KeyCode.E;

    private InteractablePanel currentOpenPanel;

    void Update()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange, Color.red);
        if (Input.GetKeyDown(interactKey))
        {
            // If a panel is already open, close it
            if (currentOpenPanel != null)
            {
                currentOpenPanel.TogglePanel(); // This will close it
                currentOpenPanel = null;
                return;
            }

            // Otherwise, raycast to see if we hit a painting
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                InteractablePanel panel = hit.collider.GetComponent<InteractablePanel>();
                if (panel != null)
                {
                    panel.TogglePanel();            // This will open it
                    currentOpenPanel = panel;
                }
            }
        }
    }
}
