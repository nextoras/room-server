  #include <Adafruit_GFX.h>
  #include <Adafruit_PCD8544.h>
  
  #include "DHT.h"
  
  #define DHTPIN 2 // номер пина, к которому подсоединен датчик
  int ledPin = 3;
  byte incomingByte;
  

  DHT dht(DHTPIN, DHT22);
  
  
  void setup() {

  pinMode(ledPin, OUTPUT);
  
  Serial.begin(9600);
  
  dht.begin();
  
  }
  
  void loop() {
  
  // Задержка 2 секунды между измерениями
  
  delay(2000);
  
  //Считываем влажность
  
  float h = dht.readHumidity();
  
  // Считываем температуру
  
  float t = dht.readTemperature();
  
  // Проверка удачно прошло ли считывание.
  
  if (isnan(h) || isnan(t)) {
  
  Serial.println("Не удается считать показания");
  
  return;
  
  }
  
  Serial.print(" ");
  Serial.print( h );
  Serial.print(" ");
  //Serial.print("Temperture: ");
  Serial.print(t);
  //Serial.print(" *C ");
  Serial.print(" \n");
  //Serial.print(" \n");


  if (Serial.available() > 0) {
    incomingByte = Serial.read();

    if(incomingByte == '0'){
      digitalWrite(ledPin, HIGH);

    }
    else if (incomingByte == '1'){
      digitalWrite(ledPin, LOW); 
    }
    
      Serial.print("I received: ");
      Serial.println(incomingByte, DEC);
  }
  }