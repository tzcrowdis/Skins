# Skins
A game about leveling up a battlepass, purchasing loot crates, and buying skins from a store just to show them off and compare them against the skins of NPCs.

This project started as a joke I thought I could complete in a long weekend but turned into a 2-3 month process. 
I consider this my first complete game due to substantial effort put into friends playtesting with bug fixes and design tweaks bringing it to a fairly stable state.
While it is complete, I wouldn't consider it good.
If I were to improve it, I would add more unique audio feedback and more dialogue chirps from the NPCs.

For screenshots check out: [Skins on Itch.io](https://tzcrowdis.itch.io/skins)

#### IDE and Engine Version
Unity: 2022.3.24f1 <br/>
Visual Studio 2022: 17.13.6 

## "Good" Code:
A modifier, heavily if not absolutely, based on Jokers in Balatro affects the player's xp post match dependent on specific conditions.
Breaking down the modifier implementation into this simple inheritance structure made scripting new modifiers easy.

Parent class handling basic administrative tasks of a modifier:
[Modifier Code](Assets/Scripts/Modifiers/Modifier.cs)

Child class inheriting from the modifier class:
[Cannibal Modifier Code](Assets/Scripts/Modifiers/Custom%20Modifiers/Cannibal.cs)

The Cannibal modifier eats/deletes the skin the player used in that match and accumulates a xp multiplier based on the rarity of the skin it consumed.

```csharp
public class Cannibal : Modifier
{
    [Header("Cannibal")]
    public TMP_Text multText;
    public float multDelta = 0.1f;

    
    public override bool ModifierEffect()
    {
        try 
        {
            if (Player.instance.skin == null)
            {
                modifierExpDescription = "there was no skin to consume";
                return false;
            }
            
            switch (Player.instance.skin.rarity)
            {
                case Skin.Rarity.VeryCommon:
                    mult += multDelta;
                    break;
                case Skin.Rarity.Common:
                    mult += multDelta * 2f;
                    break;
                case Skin.Rarity.Rare:
                    mult += multDelta * 3f;
                    break;
                case Skin.Rarity.Legendary:
                    mult += multDelta * 4f;
                    break;
            }

            float expGain = Play.instance.expGain;
            Play.instance.expGain = (int)(expGain * mult);

            multText.text = $"{mult}x";
            modifierExpDescription = $"ate {Player.instance.skin.itemName}. multiplied xp by {mult}x.";

            Collection.instance.DeleteCollectionItem(Player.instance.skin);
            Player.instance.skin = null;

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "there was no skin to consume";
            return false;
        }
    }

    /*
     * DRAG
     */
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        multText.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        multText.gameObject.SetActive(true);
    }
}
```

## Bad Code:
All of the following code is contained in the update function. 
It's an xp system where the player gained a certain amount and it updates the leveling progress bar. 
If I were to do this again I would consider passing in set points to coroutines to better modularize this process. 
This was a large source of trouble during development.

Process:
1. Get Base XP from the skin you played that match (baseExpPhase).
2. Update the position of the xp bar based on a preset time. Can be positive or negative requiring limits at the max level and min level.
3. Run through each modifier the player has purchased and apply their effects (modifierExpPhase).
4. Update the xp bar position for each modifier.

Redeeming qualities might be the early exits

For more context see the whole script: [Post Match Summary Code](Assets/Scripts/Play/Play.cs)
```csharp
void Update()
{
    if (!skip && displayCurrentLevel == Player.instance.maxLevel)
        SkipToEndPostMatchSummary();
    
    if (skip)
        return;
    
    if (!postMatchSummary)
        return;

    if (baseExpPhase) // doesn't wait to populate
    {
        expGain = 0;
        int posExpGain = Player.instance.skin.GetSkinExp();
        int negExpGain = EnemyController.instance.GetNegativeExp();
        expGain += posExpGain + negExpGain;

        if (EnemyController.instance.bossFight)
            expGainText.text = $"+{expGain}xp / {EnemyController.instance.GetBossExpThreshold()}xp";
        else
            expGainText.text = $"+{expGain}xp";

        GameObject src = Instantiate(expSourceText, expContent);
        src.GetComponent<TMP_Text>().text = $"+{posExpGain}xp from skin rarity {Home.instance.SplitCamelCase(Player.instance.GetSkinRarity().ToString())} and {negExpGain}xp from enemies";

        if (Player.instance.modifiers.Count > 0)
        {
            GameObject tempText = Instantiate(expSourceText, expContent);
            tempText.GetComponent<TMP_Text>().text = "...";
        }

        baseExpPhase = false;
        modifierExpPhase = true;
        modifierExpPhaseIndex = 0;
        expSpeed = expGain / (stepT - delayBtwnSteps);
        prevExpGain = expGain;
        t = 0f;

        expSFXSource.Play();
        expSFXSource.loop = true;
    }
    else if (modifierExpPhase)
    {
        // early exit if no modifiers
        Modifier mod = null;
        if (Player.instance.modifiers.Count > 0)
            mod = Player.instance.modifiers[modifierExpPhaseIndex];
        else
            modifierExpPhaseIndex = 100;

        if (mod)
        {
            if (mod.modifierType != Modifier.Type.ExpMult & mod.modifierType != Modifier.Type.ExpAdd)
                t = stepT + 1f;

            if (!expSFXSource.isPlaying)
            {
                expSFXSource.Play();
                expSFXSource.loop = true;

                if (expSpeed > 0)
                    expSFXSource.pitch = expPosPitch;
                else
                    expSFXSource.pitch = expNegPitch;
            }   

            // update exp gain
            t += Time.deltaTime;
            if (t >= stepT)
            {
                if (mod.modifierType == Modifier.Type.ExpMult | mod.modifierType == Modifier.Type.ExpAdd)
                {
                    bool success = mod.ModifierEffect();
                    if (success)
                    {
                        if (EnemyController.instance.bossFight)
                            expGainText.text = $"+{expGain}xp / {EnemyController.instance.GetBossExpThreshold()}xp";
                        else
                            expGainText.text = $"+{expGain}xp";

                        GameObject src = expContent.GetChild(expContent.childCount - 1).gameObject;
                        src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: {mod.ModifierExpDescription()}";

                        expSpeed = (expGain - prevExpGain) / (stepT - delayBtwnSteps);
                        prevExpGain = expGain;

                        if (expSpeed > 0)
                            expSFXSource.pitch = expPosPitch;
                        else
                            expSFXSource.pitch = expNegPitch;
                    }
                    else
                    {
                        GameObject src = expContent.GetChild(expContent.childCount - 1).gameObject;
                        src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: {mod.ModifierExpDescription()}";
                        expSFXSource.Stop();
                    }

                    if (Player.instance.modifiers.Count - 1 > modifierExpPhaseIndex)
                    {
                        GameObject tempText = Instantiate(expSourceText, expContent);
                        tempText.GetComponent<TMP_Text>().text = $"...";
                    }
                }

                modifierExpPhaseIndex++;
                t = 0f;
            }
        }

        // final exit
        if (modifierExpPhaseIndex >= Player.instance.modifiers.Count)
        {
            modifierExpPhase = false;
            modifierExpPhaseIndex = 0;
            t = 0f;
        }
    }


    // update exp progress bar
    // default exit
    if ((currentNewExp >= expGain & expSpeed >= 0) | (currentNewExp <= expGain & expSpeed < 0))
    {
        if (!baseExpPhase & !modifierExpPhase)
        {
            skipHomeButton.onClick.RemoveAllListeners();
            skipHomeButton.onClick.AddListener(ReturnHome);
            skipHomeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "HOME";
        }

        if (expSFXSource.isPlaying)
            expSFXSource.Stop();

        return;
    }

    // max level exit
    if (Player.instance.level == Player.instance.maxLevel)
    {
        currentLevel.text = $"{displayCurrentLevel - 1}";
        nextLevel.text = "MAX";

        expCurrentProgressBar.anchorMax = new Vector2(1f, 0.5f);
        expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);

        if (expSFXSource.isPlaying)
            expSFXSource.Stop();

        t = stepT + 1f; // exit modifier phase early

        return;
    }

    displayCurrentExp += expSpeed * Time.deltaTime;
    currentNewExp += expSpeed * Time.deltaTime;

    // adjust for xp going below 0xp lvl0
    if (displayCurrentExp <= 0 & displayCurrentLevel <= 0 & expSpeed < 0)
    {
        displayCurrentExp = 0;
        displayCurrentLevel = 0;
        currentLevel.text = $"{displayCurrentLevel}";
        nextLevel.text = $"{displayCurrentLevel + 1}";
        t = stepT + 1f; // exit modifier phase early
        currentNewExp = expGain - 1f; // exit final xp count early
        return;
    }

    // increment/decrement level
    if (expSpeed > 0 && displayCurrentExp >= currentLevelCap)
    {
        if (displayCurrentLevel + 1 == Player.instance.maxLevel)
        {
            nextLevel.text = "MAX";
            expCurrentProgressBar.anchorMax = new Vector2(1f, 0.5f);
            expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);
            t = stepT + 1f;
            return;
        }

        displayCurrentExp = 0f;
        displayCurrentLevel++;
        currentLevel.text = $"{displayCurrentLevel}";
        nextLevel.text = $"{displayCurrentLevel + 1}";
        currentLevelCap = Player.instance.CalculateLevelCap(displayCurrentLevel);
    }
    else if (displayCurrentExp < 0)
    {
        displayCurrentLevel--;
        currentLevelCap = Player.instance.CalculateLevelCap(displayCurrentLevel);
        displayCurrentExp = currentLevelCap;
        currentLevel.text = $"{displayCurrentLevel}";
        nextLevel.text = $"{displayCurrentLevel + 1}";
    }

    expCurrentProgressBar.anchorMax = new Vector2(displayCurrentExp / (float)currentLevelCap, 0.5f);
    expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);
}
```
