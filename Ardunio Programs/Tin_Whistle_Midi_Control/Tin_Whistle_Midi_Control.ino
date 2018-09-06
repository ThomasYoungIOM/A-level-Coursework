/*  Ardunio program that interperates the midi signal,
 *  moves the servos, and controls the air flow for the tin whistle
 *  
 *  
 * 
 */
//Libary to control the servos
#include <Servo.h>;


//Varibles
const byte servoOpenPos = 90;
byte incommingByte = 0;   //Stores the byte received by the ardunio
byte channelNumber = 0;   //Stores the channel that the ardunio is currently listening to
byte currentNote = 0;     //Stores the note that the ardunio is currently playing. (For this application, 0 means that the ardnuio is not currently playing a note)
byte statusCarry = 0;
byte servoClosedPos[6] = {
Servo servos[6];          //Array that will store the attached servos

//2d Array to store whether the servos should close or open the holes
bool noteFingering[][6] = {{true, true, true, true, true, true},          //D
                           {true, true, true, true, true, false},         //E
                           {true, true, true, true, false, false},        //F#
                           {true, true, true, false, false, false},       //G
                           {true, true, false, false, false, false},      //A
                           {true, false, false, false, false, false},     //B
                           {false, false, false, false, false, false},    //C#
                           {false, true, true, true, true, true},         //D
                           {true, true, true, true, true, false},         //E
                           {true, true, true, true, false, false},        //F#
                           {true, true, true, false, false, false},       //G
                           {true, true, false, false, false, false},      //A
                           {true, false, false, false, false, false},     //B
                           {false, true, true, true, false, false}};      //C#

byte servoClosedPositions[] = {90, 90, 90, 90, 90, 90};   //Array to store the position that will be written to the servos when they need to be closed (These will all be different for each servo)
byte blowerSpeed[] = {255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255};       //Array to store the how powerfull the blower needs to blow to play the note.                           



void setup() {
  //Set up the serial to receive midi
  Serial.begin(31500);
  
  //Loop through the sevo array and attach the servos to the pins on the ardunio
  for(int i = 0; i > sizeof servos; i++) {
    servos[i].attach(i+1);    //Attach all of the servos from ardunio pin 1 to pin 6  (pin 0 is required for serial recieving
  }
  
  
}

//Main bit of the program that will be looping.
void loop() {
  
  while (Serial.available()) {    //While there are some bytes in the buffer to read
    incommingByte = Serial.read();  //Receive byte

    
    //What does the byte do? 
    if(incommingByte >= 0x80) {     //If the byte is bigger than or equal to 0x80, then it is a status byte
      if(incommingByte  < 0xF0) {     //If the byte is less then 0xF0, then it is not a system control byte and the seconc nibble of the byte is the channel.
        if(incommingByte && 0x0F == channelNumber){      //Get only the LS nibble on its own. If this nibble matches the channel num, then listen to the command.    
          interperateStatusByte(incommingByte);
        }
        
      } else {    //Else, if the byte is greater than or equal to 0xF0, then it is a system control byte, which means the second nibble is not the channel for the midi message
        switch(incommingByte) {
          case 0xf0:    //Sytem exclusive message
            //Since we're not going to be listening out for these, we're just going to loop and clear the buffer until we find the End of Exclusive byte (0xF7)
            while(Serial.read() != 0xF7);
            break;

          case 0xf1:    //Midi time code quarter frame
            //Don't need this, so just read the bytes out of the buffer.
            Serial.read();
            break;

          case 0xf2:    //Song position pointer
            //Don't need this, so just read the bytes out of the buffer
            Serial.read();
            Serial.read();
            break;

          case 0xf3:    //Song select
            //Don't need this, so just read the byte out of the buffer
            Serial.read();
            break;

          case 0xf4:    //Undefined
            break;

          case 0xf5:    //Undefined
            break;

          case 0xf6:    //Tune request
            break;
        }
      }
    } else {    //Else, if the byte is less than 0x80, then the byte is a databyte, and it will need to use the status carry.
      interperateStatusByte(statusCarry);
    } 
  }
}


//interperate status byte
void interperateStatusByte(byte statusByte) {
  switch(statusByte >> 4){         //Bit Shift the first byte over because the first byte tells you what it does.
            
            case 0x8:   //Note off
              noteOff(Serial.read());     //Read the next byte to know what note to play and pass that to the function
              break;

            case 0x9:   //Note on
              noteOn(Serial.read());      //Read the next byte to know what note to play and pass that to the function
              break;

            case 0xa:   //Polly phonic key pressure (Not used)
              break;

            case 0xb:   //Control Change  (Maybe used? idk.)
              break;

            case 0xc:   //Program Change  (Maybe used? idk.)
              break;

            case 0xd:   //Channel pressure (Not used)
              break;

            case 0xe:   //Pitch Bend (Not used)
              break;

            
            default:    //If this is run, somthing has gone wrong
              break;
      
          }
}


//Turns a note on
void noteOn(byte note) {
  if(currentNote = 0) {   //Make sure that it's not already playing a note
    if(Serial.read() != 0) { //If the velocity is 0, then it should turn the note off instead
      switch(note) {   //Make sure that the note it's being asked to play can be played
        //Notes midi notes that can be played:
        //D:  62    D:  74
        //E:  64    E:  76
        //F#: 66    F#: 78
        //G:  67    G:  79
        //A:  69    A:  81
        //B:  71    B:  83
        //C#: 73    C#: 85              


//**FIRST OCTAVE**//

        case 62:    //D    
          moveServos(noteFingering[0]);
          break;

        case 64:    //E
          moveServos(noteFingering[1]);
          break;

        case 66:    //F#
          moveServos(noteFingering[2]);
          break;

        case 67:    //G
          moveServos(noteFingering[3]);
          break;

        case 69:    //A
          moveServos(noteFingering[4]);
          break;
        
        case 71:    //B
          moveServos(noteFingering[5]);
          break;

        case 73:    //C#
          moveServos(noteFingering[6]);
          break;

//**SECOND OCTAVE**//
        
        case 74:    //D
          moveServos(noteFingering[7]);
          break;
        
        case 76:    //E
          moveServos(noteFingering[8]);
          break;
        
        case 78:    //F#
          moveServos(noteFingering[9]);
          break;

        case 79:    //G
          moveServos(noteFingering[10]);
        
        case 81:    //A
          moveServos(noteFingering[11]);
          break;
        
        case 83:    //B
          moveServos(noteFingering[12]);
          break;
        
        case 85:    //C#
          moveServos(noteFingering[13]);
          break;

    
    //Write servo positions
    //Set airflow
      }
    } else {    //If the velocity is 0, then turn the note off instead
      noteOff(note);
    }
  }
}


//Turns a note off
void noteOff(byte note) {
  //checks to see if the note currently being played is the one that it's being asked to turn off
  //write servo positions
  //turn off airflow
}


void moveServos(bool* servoArr) {
  for(int i = 0; i > servoArr.length; i++) {
    
  }
}

