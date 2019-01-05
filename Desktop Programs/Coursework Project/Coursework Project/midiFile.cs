using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Project {
    public class midiFile {
        #region Exercise Varibles
        string p_instrument;        //Stores the instrument that the exercise is to be played on
        string p_exerciseName;      //Stores the name of the exercise
        int p_exerciseId;           //Stores the exercise ID
        byte p_difficulty;          //Stores the difficulty of the exercise
        #endregion

        #region Midi varibles
        List<note> p_listOfNotes;   //List of notes
        ushort? p_devision;         //Stores the tempo of the peice. 
        uint? p_tempo;              //Stores the tempo of the peice in the format tt tt tt where the value is the number of micro seconds per Crochet. Default is 500000 (120bpm)
        uint? p_timeSig;            //Time sig. Format: nn dd cc dd     nn/2^dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1/32 notes per 24 MIDI clocks (8 normally)
        ushort? p_keySig;           //key  sig. Format: sf mi           sf - Number of Sharps or flats |     mi - Major (0) or minor (1)
        #endregion


        /// <summary>
        /// Instantiats the midi file class with all of the important values that are required
        /// </summary>
        /// <param name="timeSig">The time signalture of the peice</param>
        /// <param name="devision">The tempo of the peice</param>
        /// <param name="keySig">The key sig of the peice</param>
        /// <param name="listOfNotes">The list of notes that makes up the peice</param>
        public midiFile( ushort? devision, uint? tempo, uint? timeSig, ushort? keySig, List<note> listOfNotes) {
            p_devision = devision;
            p_tempo = tempo;
            p_timeSig = timeSig;
            p_keySig = keySig;
            p_listOfNotes = listOfNotes;
        }

        #region Get Sets
        public List<note> listOfNotes {
            get {
                return p_listOfNotes;
            }
        }

        public ushort devision {
            get {
                return p_devision ?? (ushort)(120);         //If the value is null, then return the default tempo which is 120 bpm
            }
        }


        public uint tempo {
            get {
                return p_tempo ?? (uint)(500000);       //If the value is null, then return the default value of 500000 (120bpm)
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

            
        #endregion
        
        
    }


    /// <summary>
    /// Struct to store all the neccerary information about a note event. (The Absolute time it starts, the note to be played and the length)
    /// </summary>
    public struct note {
        //delta Time
        public readonly int absoluteTime;
        //Note
        public readonly byte noteNum;
        //Length
        public 
            readonly int length;

        public note(int inAbsoluteTime, byte inNote, int inLength) {
            absoluteTime = inAbsoluteTime;
            noteNum = inNote;
            length = inLength;
        }
    }
}
