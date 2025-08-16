using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModifierDeleteButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public Modifier modifier;

    [Header("Progress Bar")]
    public float deleteSpeed;
    public RectTransform deleteProgressBar;
    public RectTransform deleteProgressBackground;
    public RectTransform deleteRevealText;
    bool heldDown = false;

    void Start()
    {
        deleteProgressBar.sizeDelta = new Vector2(0f, deleteProgressBackground.sizeDelta.y);
    }

    void Update()
    {
        if (heldDown)
        {
            deleteProgressBar.anchorMax += new Vector2(deleteSpeed * Time.deltaTime, 0f);
            deleteProgressBar.sizeDelta = new Vector2(0, deleteProgressBar.sizeDelta.y);

            if (deleteProgressBar.anchorMax.x >= 1f)
            {
                modifier.DeleteModifier();
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        modifier.buttonHover.PlayOneShot(modifier.hoverSound);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        heldDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        heldDown = false;
        deleteProgressBar.anchorMax = new Vector2(0f, 0.5f);
        deleteProgressBar.sizeDelta = new Vector2(0, deleteProgressBar.sizeDelta.y);
    }
}
