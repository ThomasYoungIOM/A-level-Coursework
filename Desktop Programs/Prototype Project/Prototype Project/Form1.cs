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

namespace Prototype_Project {
    public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent()
        }


        private void btnOpenFile_Click(object sender, EventArgs e) {
            //Show the open file dialog so the user can select the file they whish to open in the program
            ofdOpenMidi.ShowDialog();

            #region temp
            Queue<Queue<byte>> p_commands;      //List to store each of the commads that will be sent to that channel

            p_commands = new Queue<Queue<byte>>();

            p_commands.Dequeue();

            #endregion






        }

        private void ofdOpenMidi_FileOk(object sender, CancelEventArgs e) {
            //
        }







    }


    #region Classes

    //Class to store all of the data in the currently loaded MIDI file
    public class midiClass {
        byte format;
        ushort timeSig;
        byte[] keySig;
        ushort numberOfTrack;
        List<chunkClass> trackChunks;      //List to store the track chunks found in the MIDI file

        public void parseStream(Stream midiStream) {
            //uint currentChunkOffset = 0;         //Stores at what point in the midi stream that the current Midi Chunk starts at
            byte[] ChunkType = new byte[4];
            uint chunkLength;
            byte[] currentChunk;            //Stores the current chunk in a byte array


            #region FileNotes
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
            #endregion

            //Find Head Chunk
            //Get the current Chunk Type
            midiStream.Read(ChunkType, 0, 4); 
            chunkLength = (uint)(midiStream.ReadByte()<<24 + midiStream.ReadByte()<<16 + midiStream.ReadByte()<<8 + midiStream.ReadByte());     //The chunk length is made of 4 bytes. These should be bit shifted to make the most sugnificant byte come first and then add them together

            //Check to see if the head chunk has been found. If it has been, then parse the headchunk
            if (ChunkType.Equals(new byte['M', 'T', 'h', 'd'])) {
                midiStream.Read(currentChunk = new byte[chunkLength], 0, Convert.ToInt32(chunkLength));     //Read the current chunk and save it to an array                            ////////////////Add error checking here///////////////
                parseHeadChunk(new Queue<byte>(currentChunk));      //Pass the current chunk to the parse head chunk method
            } else {
                /*//If the chunk type is not recognised, then skip the chunk.
                 *midiStream.Read(new byte[chunkLength], 0, Convert.ToInt32(chunkLength));        ////////////////////Add error checking here//////////////////
                 */

                //If the head chunk isn't found, throw an exeption to help debugging.
                throw new ArgumentException("Head Chunk not found");
            }


            //Read track chunks
            midiStream.Read(ChunkType, 0, 4);
            chunkLength = (uint)(midiStream.ReadByte() << 24 + midiStream.ReadByte() << 16 + midiStream.ReadByte() << 8 + midiStream.ReadByte());     //The chunk length is made of 4 bytes. These should be bit shifted to make the most sugnificant byte come first and then add them together

            //Check to see if the track chunk has been found
            if(ChunkType.Equals(new byte['M', 'T', 'r', 'k'])) {
                midiStream.Read(currentChunk = new byte[chunkLength], 0, Convert.ToInt32(chunkLength));     //Read the current chunk and save it to an array                            ////////////////Add error checking here///////////////
                trackChunks.Add(new chunkClass(new Queue<byte>(currentChunk), chunkLength));      //Pass the current chunk to the parse head chunk method
            } else {
                //If the head chunk isn't found, throw an exeption to help debugging.
                throw new ArgumentException("Track Chunk not found");
            }
        }

        private void parseHeadChunk(Queue<byte> inputQueue) {
            //Find the format of the rest of the MIDI File
            inputQueue.Dequeue();           //Since there are only 3 formats of MIDI file, this byte will be empty
            format = inputQueue.Dequeue();  //This is the byte that will store the type of MIDI file being delt with

            //Find the Number of tracks
            numberOfTrack = (ushort)(inputQueue.Dequeue() << 8 + inputQueue.Dequeue());     //Not strictly necerssary since queues will be used and they don't need to know the length

            //Find the time signature
            timeSig = (ushort)(inputQueue.Dequeue() << 8 + inputQueue.Dequeue());       //Read 2 bytes from the queue and add them together after bitshifting to make sure everything has the right values
        }




    }

    /// <summary>
    /// Class to store the channel and global commands that can be found in a class
    /// </summary>
    public class chunkClass {
        List<channelClass> p_channels;

        /// <summary>
        /// Parse the incomming data into the class
        /// </summary>
        /// <param name="chunkData">The data in the chunk that should be parsed</param>
        public void parseChunk(Queue<byte> chunkData, uint chunkLength) {
            int deltaTime;          //The max delta time allowed in a MIDI File is 0FFFFFFF, which means it can be stored by a Signed Integer
            byte currentByte;
            byte statusCarry = 0;

            #region Find DeltaTime
            //get the next byte to be read
            //currentByte = chunkData.Dequeue();
            deltaTime = 0;
            while (chunkData.Peek() >= 0x80) {   //While bit 8 of the byte is 1 which means the variable length is not over
                deltaTime = (deltaTime << 7) + (chunkData.Dequeue() - 0x80);      //Bit shift the time byte left 7, then add the current byte after removing bit 8
            }
            deltaTime = (deltaTime << 7) + (chunkData.Dequeue());                 //Bit shift the time data left 7, then add the current byte, knowing that bit 8 will be 0 cuz it's the last byte

            #region Find next message
            if(chunkData.Peek() >= 0x80) {
                //Interprate Status Byte 
                interprateStatusByte(deltaTime, chunkData.Dequeue(), chunkData.Dequeue(), ref chunkData, ref statusCarry);     //Pass the nessercary data to the interprate status byte.
            } else {
                //Interprate Status byte using status carry
            }

            #endregion
            #endregion
        }

        /// <summary>
        /// Method to interprate the status byte and act upon it
        /// </summary>
        /// <param name="controlByte">The control byte in the message</param>
        /// <param name="firstDataByte">The first databyte in the stream</param>
        private void interprateStatusByte(int deltaTime, byte controlByte, byte firstDataByte, ref Queue<byte> incommingData, ref byte statusCarry) {
            switch ((controlByte & 0xF0) >> 4) {        //Use the and bitwise operator to only select the left most nibble, then bitshift right to get left mot nibble only
                case 0x8:
                    //Note off (2 data bytes)
                    //lsvMidi.Items.Add($"Time: {deltaTime} Note off Ch: {controlByte & 0x0F} Note:{firstDataByte}  Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                    statusCarry = controlByte;
                    break;

                case 0x9:
                    //Note on   (2data bytes)
                    //lsvMidi.Items.Add($"Time: {deltaTime} Note on Ch: {controlByte & 0x0F} Note:{firstDataByte} Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                    statusCarry = controlByte;
                    break;

                case 0xa:
                    //"Polly phonic after touch" (2 data bytes)
                    //lsvMidi.Items.Add($"Time: {deltaTime} PollyPhonic Ch: {controlByte & 0x0F} Note:{firstDataByte} Velocity: {Convert.ToByte(midiStream.ReadByte())}");
                    statusCarry = controlByte;
                    break;

                case 0xb:
                    //Chan mode control thingys.    (2data bytes)
                    //midiStream.ReadByte();
                    statusCarry = controlByte;
                    break;

                case 0xc:
                    //Chan Program Change           (1 data byte)
                    //lsvMidi.Items.Add($"Time: {deltaTime} ChanProgram change Ch: {controlByte & 0x0F} New instument: {firstDataByte}");
                    statusCarry = controlByte;
                    break;

                case 0xd:
                    //Chanel after touch    (1 data byte)
                    //lsvMidi.Items.Add($"Time: {deltaTime} Channel aftertouch Ch: {controlByte & 0x0F} Data (volume, idk): {firstDataByte}");
                    statusCarry = controlByte;
                    break;

                case 0xe:
                    //Pitch bend (2 data bytes)
                    //lsvMidi.Items.Add($"Time: {deltaTime} PitchBend Ch: {controlByte & 0x0F} Data1:{firstDataByte} Data2: {Convert.ToByte(midiStream.ReadByte())}");
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

    /// <summary>
    /// Class to store all of the messages that will be sent to a particular channel
    /// </summary>
    public class channelClass { 
        Queue<messageStruct> p_commands;      //List to store each of the commads that will be sent to that channel
    }
    
    /// <summary>
    /// Struct to store each message
    /// </summary>
    public struct messageStruct {
        public int deltaTime;
        public Queue<byte> message;
    }

    #endregion
}
