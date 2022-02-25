﻿using UnityEngine;

public enum Clip { water_pour2, ContainerFinish };

public class SFXManager : MonoBehaviour {
	public static SFXManager instance;

	private AudioSource[] sfx;

	// Use this for initialization
	void Start () {
		instance = GetComponent<SFXManager>();
		sfx = GetComponents<AudioSource>();
    }

	public void PlaySFX(Clip audioClip) {
		sfx[(int)audioClip].Play();
	}
}
