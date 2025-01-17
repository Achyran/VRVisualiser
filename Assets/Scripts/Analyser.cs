using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Audio;


public class Analyser : MonoBehaviour
{
    public static Analyser current;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private LoopbackAudio loopback;

    public float[] samplesLeft { get; private set; }
    public float[] samplesRight { get; private set; }
    public float[] freqBand { get; private set; }
    public float[] bandBuffer { get; private set; }
    private float [] bufferDecrese = new float[8];

    public float [] audioBand { get; private set; }
    public float [] audioBandBuffer { get; private set; }
    public float[] highestFriq { get; private set; }

    public float amplitude { get; private set; }
    public float amplitudeBuffer { get; private set; }
    float amplitudeHighest;
    float lastamp = 0.5f;
    public float amplitudeDiffenece { get; private set; }

    public float[] bandDifference { get; private set; }
    private float[] lastBand;

    [SerializeField]
    private float audioProfile;
    private enum chanelSelect { Sterio, Left, Right }
    [SerializeField]
    private chanelSelect chanel;

    [SerializeField]
    [Range(1,2)]
    private float decresspeed;
    

    private void Awake()
    {
    
        if(Analyser.current == null)
        {
            Analyser.current = this;
        }
        else
        {
            Debug.LogWarning("AudioPeer exists");
            Destroy(this);
        }

       // if (LoopbackAudio.current != null) audioSource = null;
        InitArrays();

        AudioProfile(audioProfile);

        if (loopback != null) loopback.SpectrumSize = 512;
       
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreatAudioBands();
        GetAmplitude();
        UpdateApmDiffernce();
        UpdateBandDifference();
    }
    private void UpdateApmDiffernce()
    {
        amplitudeDiffenece = amplitude - lastamp;
        lastamp = amplitude;
        
    }
    private void UpdateBandDifference()
    {
        for (int i = 0; i < 8; i++)
        {
            bandDifference[i] = audioBandBuffer[i] - lastBand[i];
            lastBand[i] = audioBandBuffer[i];
        }
        
    }


    private void InitArrays()
    {
        samplesLeft = new float[512];
        samplesRight = new float[512];
        freqBand = new float[8];
        bandBuffer = new float[8];

        audioBand = new float[8];
        audioBandBuffer = new float[8];
        highestFriq = new float[8];
        lastBand = new float[8];
        bandDifference = new float[8];
    }

   

    private void GetAmplitude()
    {
        float currentAmp = 0;
        float currentAmpBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmp += audioBand[i];
            currentAmpBuffer += audioBandBuffer[i];
        }
        if (currentAmp > amplitudeHighest) amplitudeHighest = currentAmp;
        amplitude = currentAmp / amplitudeHighest;
        amplitudeBuffer = currentAmpBuffer / amplitudeBuffer;
        
    }
    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            highestFriq[i] = audioProfile;
        }
    }

    private void GetSpectrumAudioSource()
    {
        if (audioSource != null)
        {
            audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
            audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
        }
        else
        {
            samplesLeft = LoopbackAudio.current.GetAllSpectrumData(AudioVisualizationStrategy.PostScaled);
            samplesRight = LoopbackAudio.current.GetAllSpectrumData(AudioVisualizationStrategy.PostScaled);
        }

    }
    
    private void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrese[i] = 0.005f;
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrese[i];
                bufferDecrese[i] *= decresspeed;
            }
        }
    }

    private void CreatAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > highestFriq[i]) highestFriq[i] = freqBand[i];

            audioBand[i] = (freqBand[i] / highestFriq[i]);
            audioBandBuffer[i] = (bandBuffer[i] / highestFriq[i]);
        }
        
    }
    private void MakeFrequencyBands()
    {
        /*
         * 22050/512 = 43hertz per sample
         * 
         * 20   - 60    hertz
         * 60   - 250   hertz
         * 250  - 500   hertz
         * 500  - 2000  hertz
         * 2000 - 4000  hertz
         * 4000 - 6000  hertz
         * 6000 - 20000 hertz
         * 
         * 
         * 
         * To Do: Make it so you can have X amounts of Bands
         * 
         */

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            
            float avrage = 0;

            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount ; j++)
            {
                switch (chanel)
                {
                    case chanelSelect.Sterio:
                        avrage += samplesLeft[count] + samplesRight[count] * (count + 1);
                        break;
                    case chanelSelect.Left:
                        avrage += samplesLeft[count] * (count + 1);
                        break;
                    case chanelSelect.Right:
                        avrage += samplesRight[count] * (count + 1);
                        break;
                    default:
                        break;
                }
                avrage += samplesLeft[count] * (count + 1);
                count++;
            }
            avrage /= count;
            freqBand[i] = avrage ;
        }
        
    }

}
