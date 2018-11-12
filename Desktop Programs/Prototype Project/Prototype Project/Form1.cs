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
        byte[] timeSig;
        byte[] keySig;
        List<chunkClass> p_trackChunks;      //List to store the track chunks found in the MIDI file

        public void parseStream(Stream midiStream) {
            byte[] ChunkType = new byte[4];
            uint chunkLength;
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


            //Find Head Chunk
            //Get the current Chunk Type
            midiStream.Read(ChunkType, 0, 4); 
            chunkLength = midiStream.
            

        }


    }

    /// <summary>
    /// Class to store the channel and global commands that can be found in a class
    /// </summary>
    public class chunkClass {
        List<channelClass> p_channels;
    }

    /// <summary>
    /// Class to store all of the messages that will be sent to a particular channel
    /// </summary>
    public class channelClass { 
        Queue<Queue<byte>> p_commands;      //List to store each of the commads that will be sent to that channel

    }

    #endregion
}
