﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using NAudio.Wave;
using FFTW.NET;
using System.IO;
using System.Numerics;


namespace FFT_File_Analysis {
    public partial class frmMain : Form {

        bool recording = false;
        WaveFileWriter writer = new WaveFileWriter(@"C:\Users\Thomas\source\repos\A-level-Coursework\Desktop Programs\FFT File Analysis\FFT File Analysis\bin\Debug\Wav Files\recordedWav.wav", new WaveFormat(44100,1));

        const int inBufferLen = 4096;
        WaveFormat inputFormat = new WaveFormat(44100,16,1);        //Format of the file being read in
        WaveFormat convertedFormat = new WaveFormat(8196,16,1);   //The format of the wav file that the insput should be converted to before being sent to the FFT

        byte[] inByteBuffer = new byte[inBufferLen * 2];
        short[] inBuffer = new short[inBufferLen];

        List<List<double>> outputSpec = new List<List<double>>();

        public frmMain() {
            InitializeComponent();
        }
        List<int> largestPitch = new List<int>();


            WaveInEvent waveIn = new WaveInEvent();
        private void btnRec_Click(object sender, EventArgs e) {

            string output1 = "";
            foreach (var item in largestPitch) {
                output1 += item.ToString() + ",";
            }

            int hello = 0;



            waveIn.DeviceNumber = 0;            //The divice I want to use as the source for the sound (Device 0 is the default device)
            waveIn.WaveFormat = new WaveFormat(44100, 1);       //Format of the source
            if (!recording) {
                recording = true;
                waveIn.DataAvailable += WaveIn_DataAvailable;       //Add this sub to the events list of "waveIn". The sub will run whenerver there is data availible in "waveIn"
                waveIn.StartRecording();

                btnRec.BackColor = Color.Red;

            } else {
                recording = false;
                waveIn.StopRecording();
                waveIn.DataAvailable -= WaveIn_DataAvailable;       //Take this sub off the events list of "waveIn" to stop the writing of audio data to a file

                writer.Close();     //Close the writer

                btnRec.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// Sub Run when data is available to be recorded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
               writer.Write(e.Buffer,0,e.Buffer.Length);
        }

        private void btnOpenWav_Click(object sender, EventArgs e) {
            string output = "";
            WaveFileReader wavFile;
            int count = 0;
            double largestMagnitude = 0;
            int largestIndex = 0;
            List<double> outputMags = new List<double>();
            List<Complex> fftCompOutput = new List<Complex>();

            if(ofdWavFile.ShowDialog() == DialogResult.OK) {
                wavFile = new WaveFileReader(ofdWavFile.FileName);
                WaveFormatConversionStream waveFile = new WaveFormatConversionStream(convertedFormat, wavFile);

                //While there is enough data to perform the FFT on
                while (waveFile.Length - count*inBufferLen*2 > inBufferLen * 2) {
                    //Create the new list in the list to hold the values from the FFT of this buffer
                    outputSpec.Add(new List<double>());

                    //Read in the bytes from the wav file, reading twice as many since the array that is passed to the transform will be 16 bit values, not 8 bit
                    wavFile.Read(inByteBuffer, 0, inBufferLen * 2);
                    //Convert the bytes to shorts
                    inBuffer = byteToShort(inByteBuffer);


                    
                    drawAudio(inBuffer);
                    
                    outputMags.Clear();     //Clear the list for the new values
                    
                    fftCompOutput = fft(inBuffer).ToList();

                    //Perform the FFT on the buffer
                    foreach (var currentComp in fftCompOutput) {
                        //outputSpec[count].Add(currentComp.Magnitude);
                        outputMags.Add(currentComp.Magnitude);
                    }

                    for (int i = 0; i < outputMags.Count; i++) {
                        if (outputMags[i] > largestMagnitude) {
                            largestMagnitude = outputMags[i];
                            largestIndex = i;
                        }
                    }

                    largestPitch.Add(largestIndex);

                    largestMagnitude = 0;       //Reset the largest mag counter
                    
                    count++;
                }
            }



           


            output = "";

            foreach (var item in largestPitch) {
                output += item.ToString() + ",";
            }




            int hi = 0;

        }

        /// <summary>
        /// Function to convert a byte array of values into a signed short array of half the values.
        /// </summary>
        /// <param name="inArray">The byte array to be converted</param>
        /// <returns>A signed short array made of the input array with the most signinficat byte first</returns>
        private short[] byteToShort(byte[] inArray) {
            short[] outputArr = new short[inArray.Length / 2];      //Output Array will be half as long as input array

            for (int i = 0; i <= inArray.Length - 2; i+=2) {
                //The final 16 bit value will be made of the 2nd byte bitshifted 8 left, or-ed with the second byte since the format is least significant byte first
                outputArr[i/2] = (short)((inArray[i+1] << 8) | (inArray[i]));
            }

            return outputArr;
        }

        private Complex[] fft(short[] inArray) {
            Complex[] inputDouble = new Complex[inArray.Length];
            Complex[] outComp = new Complex[inArray.Length];

            //Convert the input array to the double array
            for (int i = 0; i < inArray.Length; i++) {
                inputDouble[i] = Convert.ToDouble(inArray[i]);   
            }
            


            //The Libary required pinned arrays to be used. This measn that the arrays position in the RAM is fixed so short cuts can be made to make the thingy run faster
            using (PinnedArray<Complex> inPinnedComp = new PinnedArray<Complex>(inputDouble))
            using (PinnedArray<Complex> outPinnedComp = new PinnedArray<Complex>(outComp)) {
                DFT.FFT(inPinnedComp,outPinnedComp);
            }

            //Return the output from the transformation
            return outComp;
        }


        private void drawAudio(short[] inAudio) {
            //Bitmap drawnPic = new Bitmap(picOutput.Width, picOutput.Height);
            using (Bitmap drawnPic = new Bitmap(picOutput.Width, picOutput.Height)) {

                //Draw the pixel black
                for (int i = 0; i < inAudio.Length; i++) {
                    short val = inAudio[i];
                    drawnPic.SetPixel((i / (inAudio.Length / drawnPic.Width)), (drawnPic.Height / 2) - (inAudio[i] / (65538 / (drawnPic.Height / 2))), Color.Black);
                }
                picOutput.Image = drawnPic.Clone(new Rectangle(0, 0, drawnPic.Width, drawnPic.Height), System.Drawing.Imaging.PixelFormat.DontCare);
            }
        }


    }
}
