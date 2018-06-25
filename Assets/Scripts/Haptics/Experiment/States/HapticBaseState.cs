﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HapticBaseState : MonoBehaviour {

    public HapticBaseState nextState;

    private float lastTimeButton;

    AudioSource audioSource;
    protected float timeRepeat = 1f; //to stop sounds from playing too fast - needs fixing

    protected String stateName;

    //objects this state is going to use 
    //and therefore needs to activate (in Activate method)
    public GameObject[] neededObjects;

    //private Settings settings;

    void Start()
    {
        //settings = GetComponent<Settings>();
    }

    protected virtual void Update()
    {
        //Using only one of the buttons on one stylus because the others don't
        //seem to function.
        if ((PluginImport.GetButtonState(1, 1) || PluginImport.GetButtonState(1, 2))
            && Time.time - lastTimeButton > .5f)
        {
            triggerPressed();
            lastTimeButton = Time.time;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePressed();
        }
    }

    protected virtual void mousePressed()
    {
    }

    protected abstract void triggerPressed();

    //Disable all objects other
    //than the ones the state needs
    public virtual void OnEnable()
    {
        //disable all objects tagged "useful"
        //this is so that we don't disable everything
        //gameobjects holding networking scripts, OpenVR, HMD's etc.
        //"useful" was chosen for lack of a better tag.
        GameObject[] objects = GameObject.FindGameObjectsWithTag("useful");
        foreach (GameObject gameObject in objects)
        {
            gameObject.SetActive(false);
        }

        //enable only Objects needed by current state
        foreach (GameObject gameObject in neededObjects)
        {
            gameObject.SetActive(true);
        }

        audioSource = GetComponent<AudioSource>();
        AudioClip clip = (AudioClip)Resources.Load("Start");
        audioSource.PlayOneShot(clip);
        //Debug.Log("State: " + this.stateName);
    }

    public virtual void advanceState()
    {
        //disable this state (i.e. update will stop being called)
        enabled = false;

        //tried to enable next state in list but did not work because it was not enabled
        //ExperimentState[] states = GetComponents<ExperimentState>(true);
        //for (int i = 0; i < states.Length; i++)
        //{
        //    if (states[i].Equals(this))
        //    {
        //        this.enabled = false;
        //        if (states[i + 1] != null)
        //            states[i + 1].enabled = true;
        //    }
        //    break;
        //}

        //enable the next one
        if (nextState != null)
            nextState.enabled = true;
    }

    protected void playSound(String filename)
    {
        AudioClip clip = (AudioClip)Resources.Load(filename);
        if (!audioSource.isPlaying)
        {
            if (timeRepeat < 0)
            {
                audioSource.PlayOneShot(clip);
                timeRepeat = .5f;
            }
        }
    }
}