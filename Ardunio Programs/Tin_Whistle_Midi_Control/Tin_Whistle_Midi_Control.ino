/*  Ardunio program that interperates the midi signal,
 *  moves the servos, and controls the air flow for the tin whistle
 *  
 *  
 * 
 */

//Varibles
byte incommingByte = 0;   //Stores the byte received by the ardunio
byte channelNumber = 0;   //Stores the channel that the ardunio is currently listening to
byte currentNote = 0;     //Stores the note that the ardunio is currently playing. (For this application, 0 means that the ardnuio is not currently playing a note)


void setup() {
  //Set up the serial to receive midi
  Serial.begin(31500);
  
  //Set the servo objects

  
  
}

//Main bit of the program that will be looping.
void loop() {
  
  while (Serial.available()) {    //While there are some bytes in the buffer to read
    incommingByte = Serial.read();  //Receive byte

    
    //What does the byte do? 
    if(incommingByte >= 0x80) {     //If the byte is bigger than or equal to 0x80, then it is a status byte
      if(incommingByte  < 0xF0) {     //If the byte is less then 0xF0, then it is not a system control byte and the seconc nibble of the byte is the channel.
        if(incommingByte && 0x0F == channelNumber){      //Get only the LS nibble on its own. If this nibble matches the channel num, then listen to the command.    
          switch(incommingByte >> 4){         //Bit Shift the first byte over because the first byte tells you what it does.
            
            case 0x8:   //Note off
              noteOff
              break;
      
          }
        }
        
      } else {    //Else, if the byte is greater that 0xF0, then it is a system control byte, which means the second nibble is not the channel for the midi message
        
      }
    } else {    //Else, if the byte is less than 0x80, then the byte is a databyte, and it will need to use the status carry.
      
    }
    
    
    
  //Is the byte for this channel?
  
  
  
  }


  
}
