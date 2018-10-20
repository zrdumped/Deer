using UnityEngine;
using System.Collections;
using PitchDetector;

public class sampleUse : MonoBehaviour {
    public GUIText noteText;                            //GUI txt where the note will be printed

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
    private float refValue = 0.1f;                      // RMS value for 0 dB
    public float minVolumeDB = -17f;                        //Min volume in bd needed to start detection

    private int currentDetectedNote = 0;                    //Las note detected (midi number)
    private string currentDetectedNoteName;             //Note name in modern notation (C=Do, D=Re, etc..)

    private bool listening=false;						//Flag for listening

	public GUITexture score;
	public GUITexture noteGraph;
	public GUITexture noteSharpGraph;

	private int top_down_margin=20;						//Top and down margin
	private float ratio=0f;					
	//score positions
	private int startMidiNote=35; 						//Lowst midi printable in score
	private int endMidiNote=86; 						//Lowst midi printable in score
														//Conversion array from notes to positions. Minus indicates sharp note
									//si do  do#  re  re#  mi  fa  fa# sol sol#  la  la#
	int[] notePositions = new int[60] {0,  1,  -1,  2,  -2,  3,  4,  -4, 5,   -5, 6,   -6,
									   7,  8,  -8,  9,  -9, 10, 11, -11, 12, -12, 13, -13,
									  14, 15, -15, 16, -16, 17, 18, -18, 19, -19, 20, -20,
									  21, 22, -22, 23, -23, 24, 25, -25, 26, -26, 27, -27,
									  28, 29, -29, 30, -30, 31, 32, -32, 33, -33, 34, -34};

	void OnGUI() {
		if(!listening) {
			if (GUI.Button (new Rect (40, 20, 200, 60), "Press and start singing!")) {
				listening=true;
				setUptMic();
			}
		}
		else {
			if (GUI.Button (new Rect (40, 20, 200, 60), "Stop detecting notes")) {
				listening=false;
				StopMicrophone();
			}
		}	
	}


    void Awake()
    {
        pitchDetector = new Detector();
        pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
    }


    void Start()
    {
        setUpScorePosition();
        selectedDevice = Microphone.devices[0].ToString();
        micSelected = true;
        GetMicCaps();

        //Estimates bufer len, based on pitchTimeInterval value
        int bufferLen = (int)Mathf.Round(AudioSettings.outputSampleRate * pitchTimeInterval / 1000f);
        Debug.Log("Buffer len: " + bufferLen);
        data = new float[bufferLen];

        detectionsMade = new int[maxDetectionsAllowed]; //Allocates detection buffer

        setUptMic();
    }


    void Update () {
		if (listening) {
			GetComponent<AudioSource>().GetOutputData(data,0);
			float sum = 0f;
			for(int i=0; i<data.Length; i++)
				sum += data[i]*data[i];
			float rmsValue = Mathf.Sqrt(sum/data.Length);
			float dbValue = 20f*Mathf.Log10(rmsValue/refValue);
			if(dbValue<minVolumeDB) {
				noteText.text="Note: <<"+ dbValue;
				hideNotes();
				//return;
			}
			
			pitchDetector.DetectPitch (data);
			int midiant = pitchDetector.lastMidiNote ();
			int midi = findMode ();
			Debug.Log("midiant: " + midiant);
			Debug.Log("midi: " + midi);
			drawNote(midi);
			noteText.text="Note: "+pitchDetector.midiNoteToString(midi);
			detectionsMade [detectionPointer++] = midiant;
			detectionPointer %= cumulativeDetections;
		}
		else {
			noteText.text="Note: -";
		}
	}

	void setUpScorePosition() {
		//Position of score in screen
		float height = Screen.height;
		height-=top_down_margin*2;
		ratio = height / 1024f;
		float left = (Screen.width - (600 * ratio)) / 2;
		Rect inset = new Rect();
		inset.width=600f * ratio;
		inset.height=1024f * ratio;
		inset.x = 0;
		inset.y = 0;
		score.pixelInset=inset;
		Vector3 position=new Vector3();
		position.x = left / Screen.width;
		position.y = (float)top_down_margin / Screen.height;
		position.z = 0f;
		score.transform.position=position;

		//Scale also the notes and send both outside screen
		inset.width=96f * ratio;
		inset.height=192f * ratio;
		inset.x = 0;
		inset.y = 0;
		noteGraph.pixelInset=inset;
		noteSharpGraph.pixelInset=inset;

		position.x = (((400f - 50f) * ratio)+left)/Screen.width;
		position.y = -5f;
		position.z = 1f;
		noteGraph.transform.position=position;
		noteSharpGraph.transform.position=position;
	}

	int notePosition(int note) {
		int arrayIndex = note - startMidiNote;
		if (arrayIndex < 0)
			arrayIndex = 0; //this is a super contrabass man!!!
		if (arrayIndex > (endMidiNote - startMidiNote))
			arrayIndex = (endMidiNote - startMidiNote); //This is a megasoprano girl!!

		return notePositions [arrayIndex];
	}

	void hideNotes() {
		Vector3 position=noteGraph.transform.position;
		position.y = -5f;
		noteGraph.transform.position=position;
		noteSharpGraph.transform.position=position;
	}

	void drawNote(int note) {
		int notePos = notePosition (note);
		bool isSharp = false;
		if (notePos < 0) {
			isSharp=true;	
			notePos*=-1;
		}

		//This is the very center of the note
		float deltaY = (32 * (notePos-1) * ratio) + (float)top_down_margin;
		deltaY += (34f * ratio);

		if(isSharp) {
			Vector3 position=noteGraph.transform.position;
			position.y = deltaY / Screen.height;
			position.z = 1f;
			noteSharpGraph.transform.position=position;
			position.y = -5f;
			noteGraph.transform.position=position;
		}
		else {
			Vector3 position=noteGraph.transform.position;
			position.y = deltaY / Screen.height;
			position.z = 1f;
			noteGraph.transform.position=position;
			position.y = -5f;
			noteSharpGraph.transform.position=position;
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
