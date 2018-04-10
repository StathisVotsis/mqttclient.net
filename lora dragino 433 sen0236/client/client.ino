#include <OneWire.h>
#include <SPI.h>
#include <RH_RF95.h>
#include <DFRobot_BME280.h>
#include "HX711.h"


RH_RF95 rf95;
#define DOUT 5
#define CLK  6

// Data wire is plugged into port 3 on the Arduino
#define ONE_WIRE_BUS 3
#define SEA_LEVEL_PRESSURE  1013.25f
#define BME_CS 10

HX711 scale(DOUT, CLK);

float calibration_factor = 45; //worked for 100kg load cell
float units;
float ounces;

DFRobot_BME280 bme; //I2C

float temp, pa, hum, alt;

// Setup a oneWire instance to communicate with any OneWire devices (not just Maxim/Dallas temperature ICs)
OneWire oneWire(ONE_WIRE_BUS);

// Pass our oneWire reference to Dallas Temperature. 
float data;
float data2;
String datastring="";
String datastring2="";
String datastring3="";
char databuf[10];
char databuf2[10];
char databuf3[30];
uint8_t dataoutgoing[10];
uint8_t dataoutgoing2[10];
uint8_t dataoutgoing3[30];
void setup() 
{
  Serial.begin(9600);
  if (!rf95.init())
    Serial.println("init failed");
   // Defaults after init are 434.0MHz, 13dBm, Bw = 125 kHz, Cr = 4/5, Sf = 128chips/symbol, CRC on
   // Need to change to 868.0Mhz in RH_RF95.cpp 
   
   
   if (!bme.begin(0x77)) {
        Serial.println("No sensor device found, check line or address!");
        while (1);
    }

    scale.set_scale();
    scale.tare();  //Reset the scale to 0
}

void loop()
{
  ///////////////////////////////////////
  scale.set_scale(calibration_factor); //Adjust to this calibration factor

  Serial.print("Reading: ");
  units = scale.get_units(), 10;
  if (units < 0) {
    units = 0.00;
  }
  ounces = units * 0.035274;
  Serial.print(units);
  Serial.print(" grams"); 
  Serial.print(" calibration_factor: ");
  Serial.print(calibration_factor);
  Serial.println();
  
  //////////////////////////////////////
  temp = bme.temperatureValue();
  pa = bme.pressureValue();
  hum = bme.humidityValue();
  alt = bme.altitudeValue(SEA_LEVEL_PRESSURE);
  // Print Sending to rf95_server
  Serial.println("Sending to rf95_server");
  
  
  data = temp;
  data2 = units;
  datastring +=dtostrf(data, 4, 2, databuf);
  datastring2 +=dtostrf(data2, 4, 2, databuf2);

  for (int i=0; i <= 5; i++){
      databuf3[i] = databuf[i];
      delay(10);
   }

   for (int i=0; i <= 9; i++){
      databuf3[i+5] = databuf2[i];
      delay(10);
   }

   for (int i=0; i <= 30; i++){
      Serial.println(databuf3[i]);
      delay(10);
   }
   
  strcpy((char *)dataoutgoing3,databuf3);
  
  Serial.println(databuf3);
  
  rf95.send(dataoutgoing3, sizeof(dataoutgoing3));
  
  rf95.waitPacketSent();
  // Now wait for a reply
  uint8_t indatabuf[RH_RF95_MAX_MESSAGE_LEN];
  uint8_t len = sizeof(indatabuf);

  if (rf95.waitAvailableTimeout(10000))
  { 
    // Should be a reply message for us now   
    if (rf95.recv(indatabuf, &len))
   {
      // Serial print "got reply:" and the reply message from the server
      Serial.print("got reply: ");
      Serial.println((char*)indatabuf);
   }
     else
     {
      //
      Serial.println("recv failed");
     }
  }
  else
  {
    // Serial print "No reply, is rf95_server running?" if don't get the reply .
    Serial.println("No reply, is rf95_server running?");
  }
  delay(5000);
}
