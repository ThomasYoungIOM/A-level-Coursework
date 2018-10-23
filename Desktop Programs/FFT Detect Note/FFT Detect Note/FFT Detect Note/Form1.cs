using System;using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Numerics;

using NAudio.Wave;      //Extention that allows for the manipluation and inputting of audio
using FFTW.NET;         //Extention that performs the Fast Fuorier Transformation

namespace FFT_Detect_Note {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e) {
            //Set up the audio stream
            WaveInEvent waveIn = new WaveInEvent();

            waveIn.DeviceNumber = 0;                            //The audio Device that will be listened to to collect audio
            waveIn.WaveFormat = new WaveFormat(44100, 1);       //Sets the file format to be used through hte program
            waveIn.DataAvailable += audioDataAvailable;         //Whenever there is data available, run the audioDataAvailable sub

            waveIn.StartRecording();        //Starts listening to the audio input


        }



        /// <summary>
        /// The sub that is run whenever there is data in the input buffer. This runs all of the main code to actually determin a frequency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioDataAvailable(object sender, WaveInEventArgs e) {

            ushort[] buffer = new ushort[4096];     //Array to store the incomming aduio signal
            Complex[] transformedValues = new Complex[buffer.Length];       //Array to store the complex numbers that have been spat out by the FFT
            double largestMagnitude = 0;        //Stores the largest magnitude that has been found
            int biggestMagIndex;                //Stores the index of the largest magnitude that has been found

            //If the buffer has enough data in, then fill the buffer and perform the FFT
            if (e.BytesRecorded > buffer.Length) {

                //Fill the buffer
                for (int i = 0; i < buffer.Length; i+=2) {
                    buffer[i/2] = Convert.ToUInt16((e.Buffer[i] << 8) | e.Buffer[i + 1]);     //The first byte is the Most significant part of the 16 bit byte that the sound is stored as, so bitshift it to the left and or the next byte to it
                }
                
                


                //Perform FFT on buffer
                transformedValues = fft(buffer);

                //Find Maximum value
                for (int i = 0; i < transformedValues.Length; i++) {            //Loop through the entire array of values
                    if (transformedValues[i].Magnitude > largestMagnitude) {        //If the magnatude of the current number is bigger than the stored highest, then
                        largestMagnitude = transformedValues[i].Magnitude;          //Save the magnitude
                        biggestMagIndex = i;                                        //Save the location of the magnitude
                    }
                }

                //Find Frequency of Max point
                var heelo = 9;

                //Find note of that frequency


                //Display the frequency
                //Display the note
            }
        }

        /// <summary>
        /// Perform the Fast Fourier Transformation on a byte array
        /// </summary>
        /// <param name="inputArr">The uShort array of audio values to be transformed</param>
        /// <returns>Array of complex numbers that are the output fo the transformation</returns>
        private Complex[] fft(ushort[] inputArr) {
            Complex[] inputComp = new Complex[inputArr.Length];     //Array to store the complex numbers that will be inputted to the FFT
            Complex[] outputComp = new Complex[inputArr.Length];    //Array to store the complex numbers that will be outputted from the FFT

            //Loop through all of the items in the array and save them to the complex input array with 0 imaginary part
            for (int i = 0; i < inputArr.Length; i++) {
                inputComp[i] = inputArr[i];
            }



            //The libary used for the Fast Fourier Transformation requires pinned arrays. Using "Using" means that the memory can be returned to the heap once it's done
            using (var pinnedInput = new PinnedArray<Complex>(inputComp))
            using (var pinnedOutput = new PinnedArray<Complex>(outputComp)) {
                DFT.FFT(pinnedInput, pinnedOutput);     //Perform the actual transformation using pinnedInput as the input and saving the output to pinnedOutput
            }

            //Return the complex array from the transformation
            return outputComp;
        }



    }
}
