using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
	[Header("Volume")]
	[Range(0,1)]
	public float masterVolume = 1;
	[Range(0,1)]
	public float musicVolume = 1;
	[Range(0,1)]
	public float ambienceVolume = 1;
	[Range(0,1)]
	public float SFXVolume = 1;

	private Bus masterBus;
	private Bus musicBus;
	private Bus ambienceBus;
	private Bus sfxBus;

	public static AudioManager _instance {get; private set;}

	private void Awake()
	{
		if(_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		
		masterBus = RuntimeManager.GetBus("bus:/");
		musicBus = RuntimeManager.GetBus("bus:/Music");
		ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
		sfxBus = RuntimeManager.GetBus("bus:/SFX");
	}

	private void Update()
	{
		masterBus.setVolume(masterVolume);
		musicBus.setVolume(musicVolume);
		ambienceBus.setVolume(ambienceVolume);
		sfxBus.setVolume(SFXVolume);
	}
}
