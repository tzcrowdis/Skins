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
    public Button changeSkinButton;
    bool skinSelected = false;
    public GameObject bossWarningContainer;
    public TMP_Text bossWarning;
    Skin selectedSkin;

    [Header("Skin Selection")]
    public GameObject skinSelectionScrollView;
    public Transform skinSelectionContent;
    public GameObject skinSelectionButtonPrefab;

    [Header("Collection")]
    public Transform collectionContent;


    public static ReadyUp instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        startButton.onClick.AddListener(PlayOrSelectSkin);
        changeSkinButton.onClick.AddListener(OpenSelectSkin);
        if (skinSelected)
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "Start";
            changeSkinButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "Equip Skin";
            changeSkinButton.gameObject.SetActive(false);
        }

        skinSelectionScrollView.SetActive(false);

        bossWarningContainer.SetActive(false);
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
            changeSkinButton.gameObject.SetActive(false);

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

    void OpenSelectSkin()
    {
        if (!skinSelectionScrollView.activeSelf)
        {
            UpdateSkinSelectionContent();
            readyUpPanel.transform.position -= new Vector3(350f, 0f, 0f);
            skinSelectionScrollView.SetActive(true);
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
        changeSkinButton.gameObject.SetActive(true);
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
