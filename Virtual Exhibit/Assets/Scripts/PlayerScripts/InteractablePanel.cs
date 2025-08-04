using UnityEngine;

public class InteractablePanel : MonoBehaviour
{
    public GameObject panelObject;

    private bool panelOpen = false;

    public void TogglePanel()
    {
        if (panelOpen == false)
        {
            panelObject.SetActive(true);
            Time.timeScale = 0f;
            panelOpen = true;
        }
        else
        {
            panelObject.SetActive(false);
            Time.timeScale = 1f;
            panelOpen = false;
        }
    }
}
