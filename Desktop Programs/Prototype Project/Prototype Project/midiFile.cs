using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Project {
    class midiFile {
        byte fileType;      //There are 3 types of MIDI files. My program needs to cope with all of them
        byte numOfChunks;   //Number of chunks. (Will be 2 for type 0 or 1 files, or any number for type 2)
        ushort devision;    //Stores the "Time Signature". This will be in one of 2 formats depending on the value of the 16th bit (MSB)
        
    }

    //Class to represent the chunks found in the midi file
    class chunk {
        bool isHeader;      //Saves whether the chunk is a header chunk or not
        uint length;        //Saves the length of the chunk


    }
}
