using UnityEngine;
using System.Collections;
using PitchDetector;

public class SoundDetector : MonoBehaviour
{
	private Detector pitchDetector;                     //Pitch detector object

	private int minFreq, maxFreq;                       //Max and min frequencies window
	public string selectedDevice { get; private set; }  //Mic selected
	private bool micSelected = false;                   //Mic flag

	float[] data;                                       //Sound samples data
	public int cumulativeDetections = 5;                //Number of consecutive detections used to determine current note
	int[] detectionsMade;                               //Detections buffer
	private int maxDetectionsAllowed = 50;              //Max buffer size
	private int detectionPointer = 0;                       //Current buffer pointer
	public int pitchTimeInterval = 100;                     //Millisecons needed to detect tone

	public bool listening = true;                     //Flag for listening
	public int midi = 0;							//range in 48 to 74?
	void Awake()
	{
		pitchDetector = new Detector();
		pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
	}


	void Start()
	{
		selectedDevice = Microphone.devices[0].ToString();
		micSelected = true;
		GetMicCaps();

		//Estimates bufer len, based on pitchTimeInterval value
		int bufferLen = (int)Mathf.Round(AudioSettings.outputSampleRate * pitchTimeInterval / 1000f);
		data = new float[bufferLen];

		detectionsMade = new int[maxDetectionsAllowed]; //Allocates detection buffer

		setUptMic();
	}


	void Update()
	{
		if (listening)
		{
			GetComponent<AudioSource>().GetOutputData(data, 0);
			pitchDetector.DetectPitch(data);
			detectionsMade[detectionPointer++] = pitchDetector.lastMidiNote();
			detectionPointer %= cumulativeDetections;
			midi = findMode();
			//Debug.Log("midi: " + midi);
		}
	}

	void setUptMic()
	{
		GetComponent<AudioSource>().volume = 1f;
		GetComponent<AudioSource>().clip = null;
		GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
		StartMicrophone();
	}

	public void GetMicCaps()
	{
		Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)
			maxFreq = 44100;
	}

	public void StartMicrophone()
	{
		GetComponent<AudioSource>().clip = Microphone.Start(selectedDevice, true, 10, maxFreq);//Starts recording
		while (!(Microphone.GetPosition(selectedDevice) > 0)) { } // Wait until the recording has started
		GetComponent<AudioSource>().Play(); // Play the audio source!
	}

	public void StopMicrophone()
	{
		GetComponent<AudioSource>().Stop();//Stops the audio
		Microphone.End(selectedDevice);//Stops the recording of the device	
	}

	int repetitions(int element)
	{
		int rep = 0;
		int tester = detectionsMade[element];
		for (int i = 0; i < cumulativeDetections; i++)
		{
			if (detectionsMade[i] == tester)
				rep++;
		}
		return rep;
	}

	public int findMode()
	{
		cumulativeDetections = (cumulativeDetections >= maxDetectionsAllowed) ? maxDetectionsAllowed : cumulativeDetections;
		int moda = 0;
		int veces = 0;
		for (int i = 0; i < cumulativeDetections; i++)
		{
			if (repetitions(i) > veces)
				moda = detectionsMade[i];
		}
		return moda;
	}
}
