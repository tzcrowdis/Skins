using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        dialogue.text = "I don't care what you pick, either way you're nothing to me.";
        dialogueCanvas.gameObject.SetActive(true);
    }

    void RandomizeSkin()
    {
        t += Time.deltaTime;
        if (t > skinSwitchTime)
        {
            // NOTE should probably make a custom function for this use case (load all skins only once)
            SetSkin(ItemDatabase.instance.RandomSkinRandomCollection()); 
            t = 0f;
        }
    }

    void RotateEyes()
    {
        eyeL.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
        eyeR.Rotate(new Vector3(0f, 0f, -spinSpeed * Time.deltaTime));
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
    }

    void OnDisable()
    {
        var mainModule = backgroundParticles.main;
        mainModule.startColor = Color.gray;
    }
}
