using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UISync : MonoBehaviour
{
    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            InputSystemUIInputModule pointer = EventSystem.current.currentInputModule as InputSystemUIInputModule;

            if(pointer != null )
            {
                GameObject hovered = pointer.GetLastRaycastResult(Mouse.current.deviceId).gameObject;
                if(hovered != null && hovered != EventSystem.current.currentSelectedGameObject)
                {

                    EventSystem.current.SetSelectedGameObject(hovered);
                }
            }

        }
    }
}
