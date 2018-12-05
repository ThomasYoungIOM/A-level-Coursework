using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Project {
    class midiFile {
        List<notes> p_listOfNotes;  //List of notes
        uint? p_timeSig;            //Time sig. Format: nn dd cc dd     nn/2^dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1/32 notes per 24 MIDI clocks (8 normally)
        ushort? p_devision;         //Stores the tempo of the peice. 
        ushort? p_keySig;           //key  sig. Format: sf mi           sf - Number of Sharps or flats |     mi - Major (0) or minor (1)


        #region Get Sets
        public List<notes> listOfNotes {
            get {
                return p_listOfNotes;
            }
        }

        public uint timeSig {
            get {
                return p_timeSig ?? (uint)(0x04022408);     //If the value is null, then return the default MIDI time signature
            }
        }


        public ushort keySig {
            get {
                return p_keySig ?? (ushort)(0x0000);        //If the value is null, then return the key sig to be C major
            }
        }

        public ushort devision {
            get {
                return p_devision ?? (ushort)(120);         //If the value is null, then return the default tempo which is 120 bpm
            }
        }
            
        #endregion
        
        
    }

    struct notes {
        //delta Time
        int deltaTime;
        //Note
        byte note;
        //Length
        int length;

        public notes(int inDeltaTime, byte inNote, byte inLength) {
            deltaTime = inDeltaTime;
            note = inNote;
            length = inLength;
        }
    }
}
