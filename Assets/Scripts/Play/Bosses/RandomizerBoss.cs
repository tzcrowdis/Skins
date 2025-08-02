using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RandomizerBoss : Boss
{
    [Header("Eyes")]
    public Transform eyeL;
    public Transform eyeR;
    public float spinSpeed;

    [Header("Randomize")]
    public float skinSwitchTime;
    float t = 0f;

    [Header("Background Particles")]
    public ParticleSystem backgroundParticles;
    public float transitionSpeed;
    [HideInInspector] public bool transition = false;
    float startSimSpeed;
    float endSimSpeed;
    float transitionTime;

    [Header("Audio Sources")]
    public AudioSource hoverSource;
    public AudioSource clickSource;

    [HideInInspector] public bool lockSkin = false;

    protected override void Start()
    {
        base.Start();

        playedSkin.hoverSource = hoverSource;
        playedSkin.clickSource = clickSource;
    }

    void Update()
    {
        if (!lockSkin) RandomizeSkin();
        RotateEyes();
    }

    public override void ReactToPlayerSkin(Skin playerSkin)
    {
        dialogue.text = "I don't care what you pick, either way you're nothing to me.";
        dialogueCanvas.gameObject.SetActive(true);
    }

    public override void ReactToChosenSkin()
    {
        dialogue.text = "I'll be seeing you"; // TODO better reaction
        dialogueCanvas.gameObject.SetActive(true);
    }

    void RandomizeSkin()
    {
        t += Time.deltaTime;
        if (t > skinSwitchTime)
        {
            // NOTE should probably make a custom function for this use case (load all skins only once)
            SetSkin(ItemDatabase.instance.RandomSkinRandomCollection());
            playedSkin.SetSkin(skin, true);
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
        Gradient gradient1 = new Gradient();
        gradient1.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.cyan, 0f), new GradientColorKey(Color.red, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        Gradient gradient2 = new Gradient();
        gradient2.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0f), new GradientColorKey(Color.magenta, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        var mainModule = backgroundParticles.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(gradient1, gradient2);

        startSimSpeed = mainModule.simulationSpeed;
        endSimSpeed = 5f;
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
