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
using System.IO.Ports;
                                                                      
namespace Midi_Serial_sender                                          
{                                                                     
    public partial class Form1 : Form                                 
    {                                                                 
        public Form1() {                                                             
            InitializeComponent();
            
        }

        SerialPort ardunioPort = new SerialPort("COM5", 31250);     //Save and configure the serial port to talk to the ardunio
                                                                      
        private void btnSend_Click(object sender, EventArgs e) {
            ardunioPort.Open();         //Open the ardunio port
            List<string> inputStrings = new List<string>();     //The list that will store the data that will be sent to the ardunio in string form
            List<byte> inputBytes = new List<byte>();           //The list that will store the data that will be sent to the ardunio in byte form

            inputStrings = txtData.Text.Split(',').ToList<string>();        //Split the input string into the list
            foreach (string byteToSend in inputStrings) {       //Loop through the string list and convert it all to bytes and add it to the byte list
                inputBytes.Add(byte.Parse(byteToSend));         
            }

            
            ardunioPort.Write(inputBytes.ToArray(),0,inputBytes.Count);     //Write the bytes to the ardunio down the serial port
            ardunioPort.Close();            //Close the serial port
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //byte currentByte = 0;
            Stream testStream = openFileDialog1.OpenFile();
            List<byte> midiBytes = new List<byte>();


            for (int i = 0; i < testStream.Length; i++) {
                midiBytes.Add(Convert.ToByte(testStream.ReadByte()));
            }

            
                txtMidiFileOutput.Text = BitConverter.ToString(midiBytes.ToArray()).Replace('-', ' ');
            




        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btnLsvTest_Click(object sender, EventArgs e) {

            lsvMidi.Items.Add("hello");
            


        }

        private void btnMidiToLSV_Click(object sender, EventArgs e) {

            //Variables
            Stream midiStream; //Stream with the midi values in
            List<byte> midiBytes = new List<byte>();        //List for the bytes from the file

            //Open the file
            openFileDialog1.ShowDialog();
            midiStream = openFileDialog1.OpenFile();

            ////Get all the bytes from the stream and add them to the list
            //for (int i = 0; i < midiStream.Length; i++) {
            //    midiBytes.Add(Convert.ToByte(midiStream.ReadByte()));
            //}

            int currentChunkLength; //The length of the current chunk
            byte statusCarry = 0;   //Last status byte that may be used for a status carry
            byte currentByte;   //Value of the byte currently being processed
            byte eventLength;   //Used to store the length of commands that are not a set length eg SysEx messages or Meta Events
            //bool error = false; //
            byte trackFormat;   //Either 0, 1 or 2
            int numOfTracks;    //Number of track chunks in the file
            int ticksPerNote;   //Speed of the thingy
            int deltaTime = 0;
            bool end = false;           //Has the end been reached?

            //Header Chunk
            //MThd  (4 bytes)
            //Length (4 bytes)
            //Format (2 bytes)
            //number of track chunks (2 bytes)
            //Devision (2 bytes) (if byte1 > 80, then note notes per second)

            //Track Chunk
            //MTtk (4 bytes)
            //Length (4 bytes)
            //Data  (Length bytes)


            //Header Chunk
            //MThd  (4 bytes)


            //Read bytes until the first byte of the MThd is found
            while (midiStream.ReadByte() != 0x4D) { };

            #region HeadChunk
            //Make sure that the rest of the byte match up
            if (midiStream.ReadByte() == 0x54 && midiStream.ReadByte() == 0x68 && midiStream.ReadByte() == 0x64) {
                //Length (4 bytes) (In Head chunk should be 6, but apparently midi should be able to handle any length here)
                currentChunkLength = midiStream.ReadByte() * 0x1000000 + midiStream.ReadByte() * 0x10000 + midiStream.ReadByte() * 0x100 + midiStream.ReadByte();       //Convert the 4 bytes into 1 integer. First byte times by 16^6 plus the second byte times 16^4 and so on

                //Format (2 bytes)
                midiStream.ReadByte();  //First byte is not used
                trackFormat = Convert.ToByte(midiStream.ReadByte());

                //number of track chunks (2 bytes)
                numOfTracks = midiStream.ReadByte() * 0x100 + midiStream.ReadByte();    //Convert the bytes to int

                //Devision (2 bytes) (if byte1 > 0x80, then note notes per second)
                currentByte = Convert.ToByte(midiStream.ReadByte());

                //If bit 15 is a 0, then the subsequent bits store the number of ticks per note
                if (currentByte < 0x80) {
                    ticksPerNote = currentByte * 0x100 + midiStream.ReadByte();            //Take of the bit number 15, then add the bytes together to get the int
                } else {    //If bit 15 is not a 1, then the subsequent bits store the number of frames per second (bits 14 - 8) and the number of delta time units per frame (bits 7 - 0)
                    //I'm gonna do this later
                }

                #endregion



                #region TrackChunk
                //Track Chunk
                //MTtk (4 bytes)
                if (midiStream.ReadByte() == 0x4d && midiStream.ReadByte() == 0x54 && midiStream.ReadByte() == 0x72 && midiStream.ReadByte() == 0x6b) {
                    //Length (4 bytes)
                    currentChunkLength = midiStream.ReadByte() * 0x1000000 + midiStream.ReadByte() * 0x10000 + midiStream.ReadByte() * 0x100 + midiStream.ReadByte();

                    //Data  (Length bytes)
                    do {


                        //Find the delta time byte
                        currentByte = Convert.ToByte(midiStream.ReadByte());
                        deltaTime = 0;
                        while (currentByte >= 0x80) {   //While bit 8 of the byte is 1 which means the variable length is not over
                            deltaTime = (deltaTime << 7) + (currentByte - 0x80);      //Bit shift the time byte left 7, then add the current byte after removing bit 8
                            currentByte = Convert.ToByte(midiStream.ReadByte());
                        }


                        deltaTime = (deltaTime << 7) + (currentByte);                 //Bit shift the time data left 7, then add the current byte, knowing that bit 8 will be 0 cuz it's the last byte



                        //Status byte
                        currentByte = Convert.ToByte(midiStream.ReadByte());

                        //If the Current byte is greater than or equal to 0x80, then it is a control byte
                        if (currentByte >= 0x80) {
                            interpretStatusByte(currentByte, Convert.ToByte(midiStream.ReadByte()));
                        } else {        //Else, it is a databyte and therefore status carry should be used
                            interpretStatusByte(statusCarry, currentByte);
                        }



                    } while (!end);
                }

                #endregion

            }

            void interpretStatusByte(byte controlByte, byte firstDataByte) {

                switch ((controlByte & 0xF0) >> 4) {        //Use the and bitwise operator to only select the left most nibble, then bitshift right to get left most nibble only
                    case 0x8:
                        //Note off (2 data bytes)
                        lsvMidi.Items.Add($"Time: {deltaTime} Note off Ch: {controlByte & 0x0F} Note:{firstDataByte}  Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                        statusCarry = controlByte;
                        break;

                    case 0x9:
                        //Note on   (2data bytes)
                        lsvMidi.Items.Add($"Time: {deltaTime} Note on Ch: {controlByte & 0x0F} Note:{firstDataByte} Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                        statusCarry = controlByte;
                        break;

                    case 0xa:
                        //"Polly phonic after touch" (2 data bytes)
                        lsvMidi.Items.Add($"Time: {deltaTime} PollyPhonic Ch: {controlByte & 0x0F} Note:{firstDataByte} Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                        statusCarry = controlByte;
                        break;

                    case 0xb:
                        //Chan mode control thingys.    (2data bytes)
                        midiStream.ReadByte();
                        statusCarry = controlByte;
                        break;

                    case 0xc:
                        //Chan Program Change           (1 data byte)
                        lsvMidi.Items.Add($"Time: {deltaTime} ChanProgram change Ch: {controlByte & 0x0F} New instument: {firstDataByte}");
                        statusCarry = controlByte;
                        break;

                    case 0xd:
                        //Chanel after touch    (1 data byte)
                        lsvMidi.Items.Add($"Time: {deltaTime} Channel aftertouch Ch: {controlByte & 0x0F} Data (volume, idk): {firstDataByte}");
                        statusCarry = controlByte;
                        break;

                    case 0xe:
                        //Pitch bend (2 data bytes)
                        lsvMidi.Items.Add($"Time: {deltaTime} PitchBend Ch: {controlByte & 0x0F} Data1:{firstDataByte} Data2: {Convert.ToByte(midiStream.ReadByte())}");
                        statusCarry = controlByte;
                        break;

                    case 0xf:
                        //Sytem control bytes, (varying databytes)
                        switch (controlByte & 0x0f) {
                            case 0x0f:
                                //If it's a FF byte, it's a meta event and so it'll follow the format: FF <type> <length> <bytes> 
                                //(Found on page 136 of Referance 1, Complete midi specifation)


                                currentByte = firstDataByte;
                                eventLength = Convert.ToByte(midiStream.ReadByte());

                                //Read the next byte that will include the type of system control byte that's being dealt with
                                switch (currentByte) {
                                    //End of file message
                                    case 0x2f:
                                        end = true;
                                        break;

                                    //Meta-event not included
                                    default:
                                        //If the meta event is not used, then just read to the end of it based on the length of the message
                                        for (int i = 0; i < eventLength; i++) {
                                            midiStream.ReadByte();
                                        }
                                        break;
                                }

                                break;

                            default:
                                break;
                        }
                        break;
                }
            }




        }
        
    }                                                                 
}                                                                     
