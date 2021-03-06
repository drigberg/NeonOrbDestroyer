﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] public MenuButtonController menuButtonController;
    public bool disableOnce;

    void PlaySound(AudioClip whichSound){
        if (!disableOnce && !GameSettings.muted){
            menuButtonController.audioSource.PlayOneShot (whichSound);
        } else{
            disableOnce = false;
        }
    }
}
