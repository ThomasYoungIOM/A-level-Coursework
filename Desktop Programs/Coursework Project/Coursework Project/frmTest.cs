using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Numerics;

using FFTW.NET;     //This is the libary that provides the Fast Fouier Transfomation (FFT)
using NAudio.Wave;  //This is the libary that allows me to manipulate and write to wave files


namespace Coursework_Project {
    public partial class frmTest : Form {

        static WaveFileWriter waveWriter;   //This object will handle the writing to the wav file
        static readonly WaveFormat waveFileFormat = new WaveFormat(44100,16,1);     //The format of the wave files that the program will be dealing with
        const int inBufferLen = 4096;       //The length of the audio buffer that will be have the FFT Performed on it
        const string waveFilePath = @"C:\Users\Thomas\source\repos\A-level Coursework\Desktop Programs\Coursework Project\Coursework Project\bin\Debug\TempWaveFileToMark.wav";
        static WaveInEvent waveIn;          //This is the event that will trigger the sub that will record the current buffer to the temperary file
        static midiFile inputMidi;

        #region Form Triggered Methods

        public frmTest(midiFile _inputMidi) {
            InitializeComponent();
            inputMidi = new midiFile(_inputMidi.devision, _inputMidi.tempo, _inputMidi.timeSig, _inputMidi.keySig, _inputMidi.listOfNotes);
        }


        /// <summary>
        /// Starts the recoding of the audio file
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e) {
            waveIn = new WaveInEvent();     //This is the object that will handle whenever there is some data sored in the audio buffer to be used
            waveIn.WaveFormat = waveFileFormat;
            waveIn.DeviceNumber = 0;

            waveWriter = new WaveFileWriter(waveFilePath, waveFileFormat);  //Instantiate the wave writer class

            waveIn.DataAvailable += WaveInDataAvailable;        //Add the subroutine to the events list of the wave in so it is run whenever there is some new data
            waveIn.StartRecording();

            btnStop.Enabled = true;
            btnStart.Enabled = false;



        }


        /// <summary>
        /// Stops the audio recording and saves it to a file
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e) {
            waveIn.DataAvailable -= WaveInDataAvailable;        //Remove the sub from the wave in avaible event to stop it being called
            waveIn.StopRecording();
            waveWriter.Close();

            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }


        /// <summary>
        /// Load in the audio file and then mark it
        /// </summary>
        private void btnMark_Click(object sender, EventArgs e) {
            byte[] currentByteBuffer;   //Unforunatly, the method that reads the data from the wav file does not allow me to pass the byte array straight to another function to convert it to a short array, so I need to use this temp array
            short[] currentBuffer;
            int currentBufferNum;
            const int audioSectionSize = 32;   //Stores the size of the buffer that is used to work out if any audio is being played
            const int audioThresh = 655;        //Stores the thresh used to deterimin if any audio can be heard
            WaveFileReader waveReader;
            string errorString;
            Complex[] fftCompOutput = new Complex[inBufferLen / 2];      //The FFT gives out half the values that are fed into it so the array can be half the length
            List<int> maxValues = new List<int>();

            bool aboveThresh = false;       //Stores whether the last thing was audio above the minumum threshold or not. False if the last thing analysed was silence, else, true

            List<bool> audioDetected = new List<bool>();      //Stores weather any audio was found at which points in the recording
            List<bool> currentAudioDetected = new List<bool>(); //Stores the current buffer's audio

            //Load Wave File in
            if (!File.Exists(waveFilePath)) {
                MessageBox.Show("Sorry, the file could not be found. Make sure you've recorded yourself playing first!");
                return;
            }


            try {
                waveReader = new WaveFileReader(waveFilePath);
            } catch (Exception ex) {
                MessageBox.Show("Sorry, the wav file could not be opened");
                return;
            }


            //Loop through the file
            for (currentBufferNum = 0; (currentBufferNum + 1) * inBufferLen * 2 < waveReader.Length; currentBufferNum++) {
                waveReader.Read(currentByteBuffer = new byte[inBufferLen * 2], 0, inBufferLen * 2);

                if (!ByteToShort(currentByteBuffer, out currentBuffer, out errorString)) {
                    MessageBox.Show("Sorry, something has gone very wrong " + errorString);
                    return;
                }


                //Fill a list with weather or note audio was being played at that specific time
                if (!DetectAudio(currentBuffer, audioThresh, audioSectionSize, out currentAudioDetected, out errorString)) { 
                    MessageBox.Show(errorString);
                    return;
                }

                audioDetected.AddRange(currentAudioDetected);   //Adds the values from the just read buffer to the previously collected values


                //Perform the FFT on the buffer and copy only the first half to the array
                Array.Copy(FFT(currentBuffer),fftCompOutput, fftCompOutput.Length);


                //Find the index of the maximum value of that buffer and add it to the list
                maxValues.Add(Array.IndexOf(fftCompOutput.Select(x => x.Magnitude).ToArray(), fftCompOutput.Select(x => x.Magnitude).Max()));

                

            }

            //TEMP THINGS @@
            //drawGraph(Array.ConvertAll(audioDetected.ToArray(), x => Convert.ToInt32(x)).ToList());
            drawGraph(maxValues);




            //Analyse how well they matched the midi file
            AnalysePerformance(inputMidi.listOfNotes, audioDetected, maxValues, ((float)inputMidi.tempo  / 1000000f) / (float)inputMidi.devision, (float)audioSectionSize / (float)waveFileFormat.SampleRate, (float)inBufferLen / (float)waveFileFormat.SampleRate, out float timingScore, out float noteScore, out errorString);



        }

        #endregion

        #region Called Subs

        /// <summary>
        /// This Event triggered method is called whenever there is data in the wave in buffer to be read to the file
        /// </summary>
        private void WaveInDataAvailable(object sender, WaveInEventArgs e) {
            waveWriter.Write(e.Buffer, 0, e.Buffer.Length);
        }


        /// <summary>
        /// Function to convert a byte array of values into a signed short array of half the values.
        /// </summary>
        /// <param name="inArray">The byte array to be converted</param>
        /// <param name="outputArr">A signed short array made of the input array with the most signinficat byte first</param>
        /// <returns>Wether it was sucessful or not</returns>
        private bool ByteToShort(byte[] inArray, out short[] outputArr, out string errorString) {
            errorString = "";
            outputArr = new short[inArray.Length / 2];      //Output Array will be half as long as input array

            //If the array inputted is not an even number length long, then there will be an error
            if (inArray.Length % 2 == 1) {
                errorString = "Odd number of bytes";
                return false;
            }


            for (int i = 0; i <= inArray.Length - 2; i += 2) {
                //The final 16 bit value will be made of the 2nd byte bitshifted 8 left, or-ed with the second byte since the format is least significant byte first
                outputArr[i / 2] = (short)((inArray[i + 1] << 8) | (inArray[i]));
            }

            return true;
        }

        /// <summary>
        /// With a given buffer input, outs a float list of when the audio changes occured
        /// </summary>
        /// <param name="inBuffer">The buffer of audio to analyse</param>
        /// <param name="currentBufferNum">The number of buffers that have previously passed</param>
        /// <param name="aboveThresh">Weather the last thing analysed was above the audio threshold or not</param>
        /// <param name="audioDetected">The list of floats that signal when the audio was last heard</param>
        /// <param name="errorString">Returns any error messages that may have been encounted</param>
        /// <returns>Whether the thing was sucessful or not</returns>
        private bool DetectAudio(short[] inBuffer,int thresh, int bufferSectionSize, out List<bool> audioDetected, out string errorString) {
            //const int thresh = 655;             //The treshhold that seperates audio being played and no audio being played
            //const int bufferSectionSize = 32;   //The size of each sample that will have the Root Mean Squared value calculated
            audioDetected = new List<bool>();   //Stores weather audio was being played or not
            errorString = "";
            short[] bufferSection = new short[bufferSectionSize];
            int rms;

            if(inBuffer.Length % bufferSectionSize != 0) {
                errorString = "Buffer Subsection size does not divide into the buffer length";
                return false;
            }


            for (int i = 0; i < inBuffer.Length - bufferSectionSize; i += bufferSectionSize) {
                Array.ConstrainedCopy(inBuffer, i, bufferSection, 0, bufferSectionSize);    //Get a section of the buffer array

                rms = Convert.ToInt32(RootMeanSquare(bufferSection));

                if (rms > thresh)
                    audioDetected.Add(true);       //If the volume is greater than the thresh, then save the fact that there was audio here to the array           
                else
                    audioDetected.Add(false);      //Else, then there was no note being played

            }



            return true;
        }

        /// <summary>
        /// returns the RMS value of a short array
        /// </summary>
        /// <param name="inArr">the array to calculate the rms off</param>
        /// <returns>The RMS value</returns>
        private static double RootMeanSquare(short[] inArr) {
            double[] doubleArr = Array.ConvertAll(inArr, x => (double)x);   //Have to covert to double first so that the sqrt works and so that we don't get an overflow error using shorts

            return Math.Sqrt(doubleArr.Sum(n => n * n) / inArr.Length);        //The Lambda expression sums up all the squares, which is then devided by the number of items in the array before being sqarerooted
        }



        private Complex[] FFT(short[] inArray) {
            Complex[] inputDouble = new Complex[inArray.Length];
            Complex[] outComp = new Complex[inArray.Length];

            //Convert the input array to the complex array
            inputDouble = Array.ConvertAll(inArray, x => (Complex)x);
            

            //The Libary required pinned arrays to be used. This measn that the arrays position in the RAM is fixed so short cuts can be made to make the Fourier Transfom run faster
            using (PinnedArray<Complex> inPinnedComp = new PinnedArray<Complex>(inputDouble))
            using (PinnedArray<Complex> outPinnedComp = new PinnedArray<Complex>(outComp)) {
                DFT.FFT(inPinnedComp, outPinnedComp);
            }

            //Return the output from the transformation
            return outComp;
        }


        /// <summary>
        /// Analyses how well the user has played
        /// </summary>
        /// <param name="listOfNotes">The list of MIDI notes that should have been played</param>
        /// <param name="timingList">The list that contains the timing information about what the user played</param>
        /// <param name="pitchList">The list of pitches that have been returned from the FFT</param>
        /// <param name="secondsPerTick">The seconds per MIDI tick</param>
        /// <param name="audioBufferLengthSec">The length of each timing Sub Buffer in seconds</param>
        /// <param name="pitchBufferLengthSec">The length of each pitch buffer in seconds</param>
        /// <param name="timingScore">The proportion of times where the user was playing at the right time</param>
        /// <param name="noteScore">The proportion of times where the user was playing the right notes at the right time</param>
        /// <param name="errorString">Stores the error message incase an error is hit</param>
        /// <returns>Whether the function was sucessfull or not</returns>
        private bool AnalysePerformance(List<note> listOfNotes, List<bool> timingList, List<int> pitchList, float secondsPerTick, float audioBufferLengthSec, float pitchBufferLengthSec, out float timingScore, out float noteScore, out string errorString) {
            float noteStartSec;      //Stores the start time of the note currently being processed in seconds
            float noteLengthSec;     //Stores the length of the note currently being processed in seconds
            int audioIndex;          //Stores the index of the current note in the timing list
            int pitchIndex;          //Stores the index of the current note in the pitch list
            int audioIndexLength;    //Stores the number of indexs that the current note takes up in the timing list
            int pitchIndexLength;    //Stores the number of indexs that the current note takes up in the pitch list

            int audioSamplesCorrect = 0;    //Stores the number of audio samples that were correct (Eg, there was audio playing/not playing at the right time)
            int pitchSamplesCorrect = 0;    //Stores the number of pitches that were correct (Eg, the right note was being played at that time)


            timingScore = 0;
            noteScore = 0;
            errorString = "";

            //Query database about what bin ids that each note makes
            //Create dictionary


            for (int i = 0; i < listOfNotes.Count; i++) {

                noteStartSec = listOfNotes[i].absoluteTime * secondsPerTick;    //Get the start time of the note in seconds
                noteLengthSec = listOfNotes[i].length * secondsPerTick;         //Get the length of the note in seconds

                audioIndex = Convert.ToInt32(noteStartSec / audioBufferLengthSec);               //Get the index of the note in the audio list
                audioIndexLength = Convert.ToInt32(noteLengthSec / audioBufferLengthSec);        //Get the length of indexs that the current note takes up in the audio list

                pitchIndex = Convert.ToInt32(noteStartSec * pitchBufferLengthSec);          //Get the index of the note in the pitch list
                pitchIndexLength = Convert.ToInt32(noteLengthSec * pitchBufferLengthSec);   //Get the length of indexs that the current note takes up in the pitch list

                
                //If the Note length would go on longer than the audio file was, then there is an error
                if(audioIndex + audioIndexLength > timingList.Count || pitchIndex + pitchIndexLength > pitchList.Count) {
                    errorString = "Midi File longer than the recorded audio file";
                    return false;
                }


                //If the note number is not 0, then it is an actual note and so should be treated as such, else, it is a rest and so should be analysed as such
                if (listOfNotes[i].noteNum != 0) {                    
                    audioSamplesCorrect += timingList.GetRange(audioIndex, audioIndexLength).Count(x => x == true);     //Count all the times that audio was playing in the specified range

                } else {
                    audioSamplesCorrect += timingList.GetRange(audioIndex, audioIndexLength).Count(x => x == true);     //Count all the times that audio was NOT playing in the specified range
                }
            }

            //Calculate the overall scores the user got
            timingScore = (float)audioSamplesCorrect / (float)timingList.Count;
            noteScore = (float)pitchSamplesCorrect / (float)pitchList.Count;

            return true;
        }
        #endregion

        #region Temp Testing

        private void drawGraph(List<int> pitchList) {
            bool isRed = false;
            Bitmap drawnGraph = new Bitmap(pitchList.Count, pitchList.Max() + 1);
            using (Graphics g = Graphics.FromImage(drawnGraph)) {




                for (int i = 0; i < pitchList.Count; i++) {
                    drawnGraph.SetPixel(i, pitchList[i], isRed ? Color.Red : Color.Black);

                    //Swap the colour every 0.5 sec
                    if (i % 5 == 0)
                        isRed = isRed ? false : true;
                }

                picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
                picDisplay.Image = drawnGraph;
            }
        }





        #endregion


    }
}
