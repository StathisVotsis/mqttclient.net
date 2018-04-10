#include <ESP8266WiFi.h>
#include <PubSubClient.h>
 
const char* ssid = "JARVIS";
const char* password =  "eystbots";
const char* mqttServer = "";
const int mqttPort = 1883;
const char* mqttUser = "evotsis";
const char* mqttPassword = "";
char mystr[30]; //Initialized variable to store recieved data

int resetPin = 7;

WiFiClient espClient;
PubSubClient client(espClient);
 
void setup() {
  digitalWrite(resetPin,HIGH);
  delay(200);
  pinMode(resetPin,OUTPUT);
  Serial.begin(9600);
 
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Connecting to WiFi..");
  }
  Serial.println("Connected to the WiFi network");
 
  client.setServer(mqttServer, mqttPort);
  client.setCallback(callback);
 
  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
 
    if (client.connect("ESP8266Client", mqttUser, mqttPassword )) {
 
      Serial.println("connected");  
 
    } else {
 
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
 
    }
  }
 
  client.publish("test", "Hello from ESP8266");
  client.subscribe("test");
 
}
 
void callback(char* topic, byte* payload, unsigned int length) {
 
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
 
  Serial.print("Message:");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
 
  Serial.println();
  Serial.println("-----------------------");
 
}
 
void loop() {
  if (client.connect("ESP8266Client", mqttUser, mqttPassword )) {
 
      Serial.readBytes(mystr,20); //Read the serial data and store in var
      Serial.println(mystr);
      client.publish("test", mystr);
      delay(2000);  
 
    } 
    
    else { 
      digitalWrite(resetPin,LOW);
      delay(2000);
    } 
}
