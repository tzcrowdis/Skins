using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyUp : MonoBehaviour
{
    [Header("Ready Up")]
    public GameObject readyUpPanel;
    public GameObject skinBorder;
    public Image skinImage;
    public Button startButton;
    bool skinSelected = false;
    Skin selectedSkin;

    [Header("Skin Selection")]
    public GameObject skinSelectionScrollView;
    public Transform skinSelectionContent;
    public GameObject skinSelectionButtonPrefab;
    List<Button> selectableSkins;

    [Header("Collection")]
    public Transform collectionContent;
    

    void Start()
    {
        startButton.onClick.AddListener(PlayOrSelectSkin);
        if (skinSelected)
            startButton.GetComponentInChildren<TMP_Text>().text = "Start";
        else
            startButton.GetComponentInChildren<TMP_Text>().text = "Equip Skin";

        skinSelectionScrollView.SetActive(false);
    }

    void PlayOrSelectSkin()
    {
        if (skinSelected)
        {
            Home.instance.OpenCanvas(Home.instance.playCanvas);
            Home.instance.gameObject.SetActive(false);
            
            Player.instance.DisplayPlayer();
            Player.instance.SetSkin(selectedSkin);
            DeselectSkin();
            startButton.GetComponentInChildren<TMP_Text>().text = "Equip Skin";

            EnemyController.instance.StartPlay();
            gameObject.SetActive(false);
        }
        else
        {
            if (!skinSelectionScrollView.activeSelf)
            {
                UpdateSkinSelectionContent();
                readyUpPanel.transform.position -= new Vector3(350f, 0f, 0f);
                skinSelectionScrollView.SetActive(true);
            }
        }
    }

    void UpdateSkinSelectionContent()
    {
        foreach (Transform child in skinSelectionContent)
            Destroy(child.gameObject);
        
        foreach (Transform child in collectionContent)
        {
            Skin skin = child.GetComponent<Skin>();
            if (skin)
            {
                GameObject skinButton = Instantiate(skinSelectionButtonPrefab, skinSelectionContent);
                skinButton.GetComponent<Button>().onClick.AddListener(delegate { SelectSkin(skin); });

                Image img = skinButton.GetComponent<Image>();
                img.sprite = skin.itemImage.sprite;
                img.color = skin.itemImage.color;
                img.material = skin.itemImage.material;
            }
        }
    }

    void SelectSkin(Skin skin)
    {
        selectedSkin = skin;
        skinSelected = true;

        skinImage.sprite = skin.itemImage.sprite;
        skinImage.color = skin.itemImage.color;
        skinImage.material = skin.itemImage.material;

        startButton.GetComponentInChildren<TMP_Text>().text = "Start";
        readyUpPanel.transform.position += new Vector3(350f, 0f, 0f);
        skinSelectionScrollView.SetActive(false);
    }

    void DeselectSkin()
    {
        selectedSkin = null;
        skinImage.sprite = null;
        skinImage.color = Color.white;
        skinImage.material = null;
        skinSelected = false;
    }
}
