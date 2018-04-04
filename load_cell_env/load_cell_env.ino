/*-----( Import needed libraries )-----*/
#include <Wire.h>  

#include <Hx711.h>
Hx711 scale(A2, A3);



/*-----( Declare Variables )-----*/
//NONE
float val;


void setup()   /*----( SETUP: RUNS ONCE )----*/
{
 
  Serial.begin(9600);  // Used to type in characters
 
  
  delay(1000);   
}

////////////////////////////////////////////////////////////////////////////////
void loop()   
{
  
 
            
            Serial.println(scale.getGram(), 1);
            
             val=(scale.getGram());
             delay(1000);
  

}


