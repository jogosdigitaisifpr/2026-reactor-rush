using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	private enum VolumeType{
		MASTER,
		MUSIC,
		AMBIENCE,
		SFX,
	}

	[Header("Type")]
	[SerializeField] private VolumeType volumeType;

	private Slider volumeSlider;

	private void Awake()
	{
		volumeSlider = GetComponentInChildren<Slider>();

		switch(volumeType)
		{
			case VolumeType.MASTER:
				volumeSlider.value = AudioManager._instance.masterVolume;
				break;
			case VolumeType.MUSIC:
				volumeSlider.value = AudioManager._instance.musicVolume;
				break;
			case VolumeType.AMBIENCE:
				volumeSlider.value = AudioManager._instance.ambienceVolume;
				break;
			case VolumeType.SFX:
				volumeSlider.value = AudioManager._instance.SFXVolume;
				break;
			default:
				Debug.LogWarning("Volume Type not supported: " + volumeType);
				break;
		}
	}

	public void OnSliderValueChanged()
	{
		switch(volumeType)
		{
			case VolumeType.MASTER:
				AudioManager._instance.masterVolume = volumeSlider.value;
				break;
			case VolumeType.MUSIC:
				AudioManager._instance.musicVolume = volumeSlider.value;
				break;
			case VolumeType.AMBIENCE:
				AudioManager._instance.ambienceVolume = volumeSlider.value;
				break;
			case VolumeType.SFX:
				AudioManager._instance.SFXVolume = volumeSlider.value;
				break;
			default:
				Debug.LogWarning("Volume Type not supported: " + volumeType);
				break;
		}
	}
}
