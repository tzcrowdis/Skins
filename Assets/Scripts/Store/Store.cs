using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [Header("Store Buttons")]
    [SerializeField]
    public Button[] storeButtons;

    [Header("Sub-Store Buttons")]
    public Button featuredButton;
    public Button crateButton;
    public Button coinButton;
    List<Button> subButtons;

    [Header("Sub-Store Panels")]
    public GameObject featuredPanel;
    public GameObject cratePanel;
    public GameObject coinPanel;
    List<GameObject> subPanels;

    [Header("Purchase Confirmation Panel")]
    public GameObject confirmationPanel;
    public Image itemImage;
    public TMP_Text costText;
    public Button confirmButton;
    public Button cancelButton;
    Button itemButton;
    
    
    void Start()
    {
        foreach (Button button in storeButtons)
        {
            button.onClick.AddListener(delegate { OpenPurchasePanel(button.GetComponent<StoreButton>()); });
        }

        featuredButton.onClick.AddListener(delegate { OpenSubPanel(featuredPanel, featuredButton); });
        crateButton.onClick.AddListener(delegate { OpenSubPanel(cratePanel, crateButton); });
        coinButton.onClick.AddListener(delegate { OpenSubPanel(coinPanel, coinButton); });

        subButtons = new List<Button>();
        subButtons.Add(featuredButton);
        subButtons.Add(crateButton);
        subButtons.Add(coinButton);

        subPanels = new List<GameObject>();
        subPanels.Add(featuredPanel);
        subPanels.Add(cratePanel);
        subPanels.Add(coinPanel);

        foreach (GameObject panel in subPanels)
            panel.SetActive(false);
        featuredPanel.SetActive(true);
        featuredButton.interactable = false;

        confirmButton.onClick.AddListener(ConfirmPurchase);
        cancelButton.onClick.AddListener(CancelPurchase);
        confirmationPanel.SetActive(false);
    }

    public void OpenSubPanel(GameObject panel, Button navBtn)
    {
        foreach (GameObject obj in subPanels)
        {
            if (obj == panel)
                obj.gameObject.SetActive(true);
            else
                obj.gameObject.SetActive(false);
        }

        foreach (Button btn in subButtons)
        {
            if (btn == navBtn)
                btn.interactable = false;
            else
                btn.interactable = true;
        }
    }

    void OpenPurchasePanel(StoreButton storeButton)
    {
        foreach (Transform child in confirmationPanel.transform)
            child.gameObject.SetActive(false);

        itemImage.color = storeButton.image.color;
        itemImage.sprite = storeButton.image.sprite;
        itemImage.material = storeButton.image.material;
        itemImage.gameObject.SetActive(true);

        costText.text = storeButton.cost.text;
        costText.gameObject.SetActive(true);

        itemButton = storeButton.GetComponent<Button>();

        confirmationPanel.SetActive(true);
    }

    void ConfirmPurchase()
    {
        // TODO add to inventory or currency or modifier list

        itemButton.interactable = false;
        confirmationPanel.SetActive(false);
    }

    void CancelPurchase()
    {
        confirmationPanel.SetActive(false);
    }
}
