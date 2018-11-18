using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;

using NAudio.Wave;

using FFTW.NET;

namespace FFT_Testing {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        BufferedWaveProvider bufferedWaveProvider = null;       //Thing that provides the sound for me to use
        byte[] buffer = new byte[8192];     //Buffer to store the sound in

        private void btnAudioInputStart_Click(object sender, EventArgs e) {
            WaveInEvent waveIn = new WaveInEvent();

            waveIn.DeviceNumber = 0;            //The divice I want to use as the source for the sound (Device 0 is the default device)
            waveIn.WaveFormat = new WaveFormat(44100, 1);       //Format of the source
            waveIn.DataAvailable += WaveIn_DataAvailable;       //IDK what it does, it seems to add all the audio bits together to make it work

            //This thingy is the thingy that stores the data
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

            // begin record
            waveIn.StartRecording();


            //Start and stop the timer
            if (tmr1.Enabled)
                tmr1.Enabled = false;
            else
                tmr1.Enabled = true;

        }

        //Idk what this does. It seems to make it work tho
        void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
            if (bufferedWaveProvider != null) {
                bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                bufferedWaveProvider.DiscardOnBufferOverflow = true;
            }
        }


        /// <summary>
        /// Draw the graph thingy
        /// </summary>
        /// <param name="inBuffer"></param>
        void drawTimeGraph(byte[] inBuffer) { 


            //picGraph.Image = null; //Clear the image that is on the thingy
            
            using (Graphics g = picGraph.CreateGraphics()) {
            
                Pen plotter = new Pen(Brushes.Black);
                int valueToPlot;
                for (int i = 1; i < inBuffer.Length + 1; i += 2) {      //Half the value are loud, half are the ones we want, so ignore half of them
                    valueToPlot = Convert.ToInt32(inBuffer[i]);
                    if (valueToPlot < 128)
                        valueToPlot = (valueToPlot + 127) * 2;
                    else
                        valueToPlot = 2 * (valueToPlot - 128);
                    g.DrawEllipse(plotter, i, valueToPlot, 1, 1);
                }
            }


        }




        int counter = 0;
        short[] outBufferTest = new short[8192/2];

        private void tmr1_Tick(object sender, EventArgs e) {
            counter++;
            bufferedWaveProvider.Read(buffer, 0, buffer.Length);
            bufferedWaveProvider.ClearBuffer();

            //New drawing thing
            short[] shortBuff = byteToShort(buffer);
            drawAudio(shortBuff);

            //drawTimeGraph(buffer);


            if (counter == 10) {
                //Convert buffer to short
                for (int i = 1; i < buffer.Length - 2; i += 2) {
                    outBufferTest[i / 2] = (short)((Convert.ToUInt16((buffer[i]) | buffer[i + 1] << 8)));
                }
                //Write values to txt box
                string output = "";
                for (int i = 0; i < outBufferTest.Length; i++) {
                    output += outBufferTest[i].ToString() + ",";
                }
                txtSimOutput.Text = output;

            }
        }








        private void fft(byte[] inputBuffer) {
            tmr1.Enabled = false;

            System.Numerics.Complex[] input = new System.Numerics.Complex[4096];
            System.Numerics.Complex[] output = new System.Numerics.Complex[input.Length];

            double biggestMag = 0;


            //Get rid of the loud values
            for (int i = 1; i < inputBuffer.Length-1; i += 2) {
                input[i/2] = (short)((Convert.ToUInt16((inputBuffer[i] << 8) | inputBuffer[i + 1])));
            }


            /*for (int i = 0; i < input.Length / 2; i++) {
                using (Graphics g = picGraph.CreateGraphics()) {
                    g.FillEllipse(Brushes.Red, i / 2, 500 - Convert.ToSingle(input[i].Magnitude) * (500 / Convert.ToSingle(60000)), 4, 4);
                }
            }*/

            //for (int i = 1; i < input.Length-1; i+=2) {
            //    input[i / 2] = inputBuffer[i];
            //}
            //

            //Centre the values around 0
            //for (int i = 0; i < input.Length; i++) {
            //    //If the value is bigger than 128, then it is the weaird top half of the graph and should have it taken away to put it below the line
            //    if (input[i].Real > 128)
            //        input[i] =  input[i].Real - 256;
            //}
            //


            //Do the transformation
            using (var pinIn = new PinnedArray<System.Numerics.Complex>(input))
            using (var pinOut = new PinnedArray<System.Numerics.Complex>(output)) {
                DFT.FFT(pinIn, pinOut);
            }


            //find the biggest magitude
            foreach (var currentNum in output) {
                if (currentNum.Magnitude > biggestMag)
                    biggestMag = currentNum.Magnitude;
            }


            //Draw the output
            for (int i = 0; i < output.Length/2; i++) {
                using (Graphics g = picGraph.CreateGraphics()) {
                    g.FillEllipse(Brushes.Red, i/2, 500-Convert.ToSingle(output[i].Magnitude)*(500/Convert.ToSingle(biggestMag)), 4, 4);
                }
            }


            



            /*
            for (int i = 0; i < output.Length; i++) {
                using (Graphics g = picGraph.CreateGraphics()) {
                    g.FillEllipse(Brushes.Red, (Convert.ToSingle(output[i].Real) * (1024 / Convert.ToSingle(biggestMag))) + 512, (500 - Convert.ToSingle(output[i].Imaginary) * (500 / Convert.ToSingle(biggestMag)))/2, 4, 4);
                }
            }*/

        }

        private void btnFFT_Click(object sender, EventArgs e) {
            fft(buffer);
        }

        private void btnTestFFT_Click(object sender, EventArgs e) {
            System.Numerics.Complex[] input = new System.Numerics.Complex[4096];
            System.Numerics.Complex[] output = new System.Numerics.Complex[input.Length];
            double biggestMag = 0;

            //Generate an array of sine values
            for (int i = 0; i < input.Length; i++)
                input[i] = Math.Sin(i * 2 * Math.PI * 128 / input.Length) + Math.Sin(i * 7 * Math.PI * 128 / input.Length);

            //Run the Transfoamtion
            using (var pinIn = new PinnedArray<System.Numerics.Complex>(input))
            using (var pinOut = new PinnedArray<System.Numerics.Complex>(output)) {
                DFT.FFT(pinIn, pinOut);
            }


            



            //Draw the output
            for (int i = 0; i < input.Length; i++) {
                using (Graphics g = picGraph.CreateGraphics()) {
                    g.DrawEllipse(Pens.Black, i, Convert.ToSingle(output[i].Magnitude), 4, 4);
                }
            }


        }

        private void btnClear_Click(object sender, EventArgs e) {
            picGraph.Image = null;
            lblBiggestPitch.Text = "";
        }

        private void btnFindPitch_Click(object sender, EventArgs e) {
            System.Numerics.Complex[] input = new System.Numerics.Complex[4096];
            System.Numerics.Complex[] output = new System.Numerics.Complex[input.Length];
            double biggestPitch = 0;
            int biggestPitchLocation = 0;
            byte[] bufferCopy = buffer;

            //Get rid of the loud values
            for (int i = 1; i < bufferCopy.Length - 1; i += 2) {
                input[i / 2] = (short)(Convert.ToUInt16((bufferCopy[i]) | bufferCopy[i + 1] << 8));
            }
            

            /*Centre the values around 0
            for (int i = 0; i < input.Length; i++) {
                //If the value is bigger than 128, then it is the weaird top half of the graph and should have it taken away to put it below the line
                if (input[i].Real > 128)
                    input[i] = input[i].Real - 256;
            }*/


            //Do the transformation
            using (var pinIn = new PinnedArray<System.Numerics.Complex>(input))
            using (var pinOut = new PinnedArray<System.Numerics.Complex>(output)) {
                DFT.FFT(pinIn, pinOut);
            }

            /*//miss half of the values cuz they too loud
            for (int i = 0; i < output.Length/2; i++) {
                if(output[i].Magnitude > biggestPitch) {
                    biggestPitch = output[i].Magnitude;
                    biggestPitchLocation = i;
                }
            }*/

            lblBiggestPitch.Text = biggestPitchLocation.ToString();

            fft(bufferCopy);


            double biggestMag = 0;

            foreach (var currentNum in output) {
                if (currentNum.Magnitude > biggestMag)
                    biggestMag = currentNum.Magnitude;
            }
            
            using (Graphics g = picGraph.CreateGraphics()) {
                g.DrawLine(Pens.Blue,biggestPitchLocation/2+2, 500,biggestPitchLocation/2+2, 500-Convert.ToSingle(biggestPitch * 500/biggestMag));
            }

        }

        private void btnSimFreq_Click(object sender, EventArgs e) {

            for (int i = 0; i < buffer.Length -1 ; i+=2) {
                buffer[i] = Convert.ToByte(((Math.Sin(Math.PI*((44100 + i) / (44100 / Double.Parse(txtFreqToSim.Text)))))+1) * 127.5);

                buffer[i + 1] = buffer[i];  

                if(buffer[i] > 128) {
                    buffer[i] = Convert.ToByte(buffer[i]-128);
                    buffer[i+1] = buffer[i];
                } else {
                    buffer[i] = Convert.ToByte(buffer[i]+127);
                    buffer[i + 1] = buffer[i];
                }
            }
        }

        private void btnSimulateList_Click(object sender, EventArgs e) {
            //List<double> freqsToSim = new List<double>();
            string[] inputs = txtFreqList.Text.Split(',');
            List<int> outputs = new List<int>();

            /*//Convert all the strings to a list of doubles
            foreach (var strFreq in inputs) {
                freqsToSim.Add(double.Parse(strFreq));
            }
            */

            foreach (string freqToSim in inputs) {
                //Set the text box to have the frequency to simulate
                txtFreqToSim.Text = freqToSim;

                //Run the sub that would be run if the button was pressed
                btnSimFreq_Click(btnSimFreq,new EventArgs());

                //Add the returned value to the output list
                //outputs.Add(int.Parse(lblBiggestPitch.Text));

                //Run the sub as if it was clicked
                btnFindPitch_Click(btnFindPitch, new EventArgs());


                txtSimOutput.Text += lblBiggestPitch.Text + ",";
            }



        }


        Dictionary<int, String> notes = new Dictionary<int, string>();

        private void btnFindNote_Click(object sender, EventArgs e) {

        }

        private void btnLoadDict_Click(object sender, EventArgs e) {
            using (System.IO.StreamReader dictFile = new StreamReader("C:\\Users\\Thomas\\source\\repos\\FFT Testing\\NoteDictonary.txt")) {
                string[] inputStrings = dictFile.ReadToEnd().Split(',');



                for (int i = 0; i < inputStrings.Length; i+=2) {
                    notes.Add(int.Parse(inputStrings[i]), inputStrings[i + 1]);                    
                }
            }
        }


        private void drawAudio(short[] inAudio) {
            //Bitmap drawnPic = new Bitmap(picOutput.Width, picOutput.Height);
            using (Bitmap drawnPic = new Bitmap(picGraph.Width, picGraph.Height)) {

                //Draw the pixel black
                for (int i = 0; i <inAudio.Length; i++) {
                    short val = inAudio[i];
                    //drawnPic.SetPixel((i/(inAudio.Length/drawnPic.Width)), (drawnPic.Height / 2) - (inAudio[i]/(65550 / (drawnPic.Height)))-1, Color.Black);
                    drawnPic.SetPixel((i / (inAudio.Length / drawnPic.Width)), (drawnPic.Height / 2) - (inAudio[i]>>8)*2 - 1, Color.Black);
                }
                drawnPic.SetPixel(512, 256, Color.Black);
                picGraph.Image = drawnPic.Clone(new Rectangle(0, 0, drawnPic.Width, drawnPic.Height), System.Drawing.Imaging.PixelFormat.DontCare);
            }
        }


        /// <summary>
        /// Function to convert a byte array of values into a signed short array of half the values.
        /// </summary>
        /// <param name="inArray">The byte array to be converted</param>
        /// <returns>A signed short array made of the input array with the most signinficat byte first</returns>
        private short[] byteToShort(byte[] inArray) {
            short[] outputArr = new short[inArray.Length / 2];      //Output Array will be half as long as input array

            for (int i = 0; i < inArray.Length - 2; i += 2) {
                //The final 16 bit value will be made of the first byte bitshifted 8 left, or-ed with the second byte
                outputArr[i / 2] = (short)((inArray[i+1] << 8) | (inArray[i]));
            }

            return outputArr;
        }

    }
}
