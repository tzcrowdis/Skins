using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilRandomizer : Boss
{
    [Header("Eyes")]
    public Transform eyeL;
    public Transform eyeR;
    public float spinSpeed;

    //[Header("Randomize")]
    float skinSwitchTime; 
    float t = 0f;

    [Header("Background Particles")]
    public ParticleSystem backgroundParticles;
    public float transitionSpeed;
    [HideInInspector] public bool transition = false;
    float startSimSpeed;
    float endSimSpeed;
    float transitionTime;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        RandomizeSkin();
        RotateEyes();
    }

    public override void ReactToPlayerSkin(Skin playerSkin)
    {
        dialogue.text = "I don't care what you pick, you'll always be nothing to me.";
        dialogueCanvas.gameObject.SetActive(true);
    }

    void RandomizeSkin()
    {
        t += Time.deltaTime;
        if (t > skinSwitchTime)
        {
            SetSkin(ItemDatabase.instance.RandomSkinRandomCollection());

            // longer for worse skins and shorter for better
            switch (skin.rarity)
            {
                case CollectionItem.Rarity.VeryCommon:
                    skinSwitchTime = 0.1f;
                    break;
                case CollectionItem.Rarity.Common:
                    skinSwitchTime = 0.15f;
                    break;
                case CollectionItem.Rarity.Rare:
                    skinSwitchTime = 0.5f;
                    break;
                case CollectionItem.Rarity.Legendary:
                    skinSwitchTime = 0.75f;
                    break;
            }
            t = 0f;
        }
    }

    void RotateEyes()
    {
        eyeL.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
        eyeR.Rotate(new Vector3(0f, 0f, -spinSpeed * Time.deltaTime));
    }

    public void TransitionParticleSimSpeed()
    {
        var mainModule = backgroundParticles.main;
        transitionTime += Time.deltaTime * transitionSpeed;
        mainModule.simulationSpeed = Mathf.Lerp(startSimSpeed, endSimSpeed, transitionTime);

        if (transitionTime >= 1)
            transition = false;
    }

    public void EnableParticleEffects()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.red, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        var mainModule = backgroundParticles.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(gradient);

        startSimSpeed = mainModule.simulationSpeed;
        endSimSpeed = 10f;
        transition = true;
        transitionTime = 0;
    }

    void OnDisable()
    {
        var mainModule = backgroundParticles.main;
        mainModule.startColor = Color.gray;

        startSimSpeed = mainModule.simulationSpeed;
        endSimSpeed = 1f;
        transition = true;
        transitionTime = 0;
    }
}
