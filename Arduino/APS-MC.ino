/*Libraries for the REAST API*/
#include <SPI.h>
#include <Ethernet.h>
#include <aREST.h>
#include <avr/wdt.h>

/*Library for the Sensor*/
#include <dht11.h>

/*Setting Board MAC Address*/
byte MAC[] = { 0x90, 0xA2, 0xDA, 0x0E, 0xFE, 0x39 };

/*Static IP Address in case DHCP Fails*/
IPAddress IP(192, 168, 0, 166);

/*Creating Ethernet Server*/
EthernetServer server(80);

/*Creating the REST instance*/
aREST rest = aREST();

/*Functions to be exposed to the API*/
int restTemperature(String command);
int restHumidity(String command);
int restRead(String command);

int letsGoTemp(String command);
int letsGoHumid(String command);

/*Creating the DHT11 instance*/
dht11 DHT11;

const int buttonLeft = 2;
const int buttonMiddle = 4;
const int buttonRight = 7;

/*Defining Sensors Pins and iterables*/
const int sensorsNumber = 4;

int sensorIndex = 0;
int sensorAction = 0;

int sensors[sensorsNumber] = { 22, 24, 26, 28 };
int sensorsLeds[sensorsNumber] = { 23, 25, 27, 29 };

/*Iterable for LEDs configurations*/
int ledsConfigurationsIndex = 0;

/*Default delay for pressed button*/
int buttonDelay = 500;

/*Setting Default Pin for Buzzer*/
int buzzerPin = 13;

/*Setting up seven segments displays*/
int displaysSetup[3] = { HIGH, LOW, LOW };
int displays[3][7] = { { 30, 31, 32, 33, 34, 35, 36 }, { 37, 38, 39, 40, 41, 42, 43 }, { 44, 45, 46, 47, 48, 49, 50 } };

int LEDs[8] = {23, 14, 15, 16, 17, 25, 27, 29};

void setup()
{
  /*Starting Serial Communication at Baud Rate 115200*/
  Serial.begin(115200);

  /*Setting PinMode to the buttons*/
  pinMode(buttonLeft, INPUT);
  pinMode(buttonMiddle, INPUT);
  pinMode(buttonRight, INPUT);

  /*Setting PinMode for sensors and their respective LEDs*/
  for(int index = 0; index < sensorsNumber; index++)
  {
    pinMode(sensors[index], INPUT);
    pinMode(sensorsLeds[index], OUTPUT);
  }

  pinMode(buzzerPin, OUTPUT);

  for (int indexDisplay = 0; indexDisplay < 3; indexDisplay++)
    for (int indexPort = 0; indexPort < 7; indexPort++)
      pinMode(displays[indexDisplay][indexPort], OUTPUT);

  /*Starting LED on initial Sensor*/
  digitalWrite(sensorsLeds[0], HIGH);

  /*Exposing functions*/
  rest.function("sensors/temperature", restTemperature);
  rest.function("sensors/humidity", restHumidity);
  rest.function("ports", restRead);
  rest.function("temp", letsGoTemp);
  rest.function("humid", letsGoHumid);
  
  /*Setting ID and Name for the REST API*/
  rest.set_id("1");
  rest.set_name("APS MC");

  Serial.println("Connecting to network...");

  /*Starting Ethernet connection*/
  if (Ethernet.begin(MAC) == 0)
  {
    /*Since DHCP failed, try to configure with pre-defined IP Address*/
    Ethernet.begin(MAC, IP);

    Serial.println("DHCP failed. Using default static IP Addresss");
  }

  server.begin();
  Serial.print("Server running at ");
  Serial.println(Ethernet.localIP());

  /*Starting the WatchDog*/
  wdt_enable(WDTO_4S);
}

void loop()
{
  /*Handle the REST API*/
  EthernetClient client = server.available();
  rest.handle(client);
  wdt_reset();

  /*Alternate Sensor on pressed button*/
  checkAlternateSensor();

  /*Alternate reading configuration on pressed button*/
  checkSensorReadingConfiguration();

  /*Alternate leds configuration*/
  checkLEDsConfiguration();

  displaySensor();

  checkSensors();
}

void checkAlternateSensor()
{
  if (digitalRead(buttonLeft) == 1)
  {
    digitalWrite(sensorsLeds[sensorIndex], LOW);
    sensorIndex = (sensorIndex + 1) % sensorsNumber;
    digitalWrite(sensorsLeds[sensorIndex], HIGH);

    delay(buttonDelay);
  }
}

void checkSensorReadingConfiguration()
{
  if (digitalRead(buttonMiddle) == 1)
  {
    sensorAction = (sensorAction + 1) % 2;
  
    delay(buttonDelay);
  }
}

void checkLEDsConfiguration()
{
  if (digitalRead(buttonRight) == 1)
  {
    handleLEDsConfiguration();

    delay(buttonDelay);
  }
}

void handleLEDsConfiguration()
{
  delay(1000);
  
  int executionTime = 1;
  int configurationMode = 1;

  bool enterPressed = false;

  while (!enterPressed)
  {
    if (digitalRead(buttonLeft) == 1)
    {
      if (executionTime > 1)
        executionTime--;

      delay(buttonDelay);
    }

    else if (digitalRead(buttonMiddle) == 1)
    {
      if (executionTime < 99)
        executionTime++;

      delay(buttonDelay);
    }

    else if (digitalRead(buttonRight) == 1)
    {
      enterPressed = !enterPressed;
      delay(buttonDelay);
    }

    String executionTimeString = "";

    if (executionTime < 10)
      executionTimeString = "0" + String(executionTime);

    else
      executionTimeString = String(executionTime);

    showDisplay(0, executionTimeString[0]);
    showDisplay(1, executionTimeString[1]);
  }

  enterPressed = !enterPressed;

  while(!enterPressed)
  {
    if (digitalRead(buttonLeft) == 1)
    {
      if (configurationMode > 1)
        configurationMode--;

      delay(buttonDelay);
    }

    else if (digitalRead(buttonMiddle) == 1)
    {
      if (configurationMode < 3)
        configurationMode++;

      delay(buttonDelay);
    }

    else if (digitalRead(buttonRight) == 1)
    {
      enterPressed = !enterPressed;
      delay(buttonDelay);
    }

    showDisplay(0, '0');
    showDisplay(1, String(configurationMode)[0]);
  }

  applyLEDsConfiguration(configurationMode, executionTime * 1000);
}

void applyLEDsConfiguration(int configurationMode, int executionTime)
{
  int timeCount = 0;
  int indexLED = 0;
  int lastIndex = 7;
  
  switch(configurationMode)
  {
    case 1:
      for(timeCount = 0; timeCount <= executionTime; timeCount+=1000)
      {
        digitalWrite(LEDs[0], HIGH);
        digitalWrite(LEDs[1], HIGH);
        digitalWrite(LEDs[2], HIGH);
        digitalWrite(LEDs[3], HIGH);
        digitalWrite(LEDs[4], HIGH);
        digitalWrite(LEDs[5], HIGH);
        digitalWrite(LEDs[6], HIGH);
        digitalWrite(LEDs[7], HIGH);
        delay(500);
        
        digitalWrite(LEDs[0], LOW);
        digitalWrite(LEDs[1], LOW);
        digitalWrite(LEDs[2], LOW);
        digitalWrite(LEDs[3], LOW);
        digitalWrite(LEDs[4], LOW);
        digitalWrite(LEDs[5], LOW);
        digitalWrite(LEDs[6], LOW);
        digitalWrite(LEDs[7], LOW);
        delay(500);
      }
    break;

    case 2:
      timeCount = 500;
      
      digitalWrite(LEDs[0], LOW);
      delay(500);
      indexLED++;
      
      while(timeCount <= executionTime)
      {
        if(indexLED < lastIndex)
        {
          digitalWrite(LEDs[indexLED-1], LOW);
          delay(500);
          timeCount+=500;
          
          digitalWrite(LEDs[indexLED], HIGH);
          delay(500);
          timeCount+=500;

          indexLED++;
        }
        else
        {
          digitalWrite(LEDs[indexLED-1], LOW);
          delay(500);
          timeCount+=500;
          
          digitalWrite(LEDs[indexLED], HIGH);
          delay(500);
          timeCount+=500;

          digitalWrite(LEDs[indexLED], LOW);
          delay(500);
          timeCount+=500;
          
          indexLED = 0;
        }  
      }
      
      digitalWrite(indexLED, LOW);
    break;

    case 3:
      timeCount = 1000;

      digitalWrite(LEDs[0], HIGH);
      digitalWrite(LEDs[lastIndex], HIGH);
      delay(1000);
      indexLED++;
      lastIndex--;
      
      while(timeCount <= executionTime)
      {
        if(indexLED != 7 && lastIndex != 0)
        {
          digitalWrite(LEDs[indexLED-1], LOW);
          digitalWrite(LEDs[lastIndex+1], LOW);
        
          digitalWrite(LEDs[indexLED], HIGH);
          digitalWrite(LEDs[lastIndex], HIGH);
          delay(1000);
          timeCount += 1000;  
        }
        else
        {
          digitalWrite(LEDs[indexLED], LOW);
          digitalWrite(LEDs[lastIndex], LOW);
          delay(1000);
          timeCount += 1000;

          indexLED = 1;
          lastIndex = 6;
        }
      }

      digitalWrite(LEDs[indexLED], LOW);
      digitalWrite(LEDs[lastIndex], LOW);  
    break;
  }    
}

void checkSensors()
{
  for(int index = 0; index < sensorsNumber; index++)
  {
    float humidity = readHumiditySensor(sensors[index]);
    float temperature = readTemperatureSensor(sensors[index]);

    if (humidity < 30)
      alarmHumidity(sensorsLeds[index]);

    if (temperature > 25)
      alarmTemperature();
  }
}

int readDHT11(int pinPort)
{
  int check = DHT11.read(pinPort);
}

float readHumiditySensor(int pinPort)
{
  float humidity = -1;

  readDHT11(pinPort);
  humidity = (float)DHT11.humidity;

  return humidity;
}

float readTemperatureSensor(int pinPort)
{
  float temperature = -1;
  
  readDHT11(pinPort);
  temperature = (float)DHT11.temperature;

  return temperature;
}

float soundBuzzer(int pinPort, int wait)
{
  pinMode(pinPort, OUTPUT);
  
  digitalWrite(pinPort, HIGH);
  delay(wait);
  digitalWrite(pinPort, LOW);
}

void switchLight(int pinPort)
{
  pinMode(pinPort, OUTPUT);

  digitalWrite(pinPort, (digitalRead(pinPort) + 1) % 2);
}

void alarmHumidity(int pinPort)
{
  for (int index = 0; index < 3; index++)
  {
    switchLight(pinPort);
    soundBuzzer(buzzerPin, 250);
  }
}

void alarmTemperature()
{
  showDisplay(0, 'H');
  showDisplay(1, 'O');
  showDisplay(2, 'T');

  delay(1000);

  displayOff(0);
  displayOff(1);
  displayOff(2);
}

void displaySensor()
{
  float value = 0;
  char last = '0';

  if (sensorAction == 1)
  {
    value = readHumiditySensor(sensors[sensorIndex]);
    last = 'C';
  }
  
  else
  {
    value = readTemperatureSensor(sensors[sensorIndex]);
    last = 'T';
  }

   String valueString = "";

    if (value < 10)
      valueString= "0" + String(value);

    else
      valueString = String(value);

    showDisplay(0, valueString[0]);
    showDisplay(1, valueString[1]);
    showDisplay(2, last);

    Serial.print("Value: ");
    Serial.print(valueString);
    Serial.print(" ");
    Serial.println(last);
}

void displayOff(int indexDisplay)
{
  int low = (displaysSetup[indexDisplay] + 1) % 2;
  
  digitalWrite(displays[indexDisplay][0], low);
  digitalWrite(displays[indexDisplay][1], low);
  digitalWrite(displays[indexDisplay][2], low);
  digitalWrite(displays[indexDisplay][3], low);
  digitalWrite(displays[indexDisplay][4], low);
  digitalWrite(displays[indexDisplay][5], low);
  digitalWrite(displays[indexDisplay][6], low);
}

void showDisplay(int indexDisplay, char value)
{
  int high = displaysSetup[indexDisplay];
  int low = (high + 1) % 2;
  
  switch (value)
  {
    case '0':
      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], low);

      break;

    case '1':
      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], low);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], low);

      break;

    case '2':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], low);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '3':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '4':

      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], low);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '5':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], low);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '6':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], low);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '7':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], low);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], low);

      break;

    case '8':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case '9':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case 'H':

      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], low);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case 'O':

      digitalWrite(displays[indexDisplay][0], high);
      digitalWrite(displays[indexDisplay][1], high);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], low);

      break;

    case 'T':

      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], low);
      digitalWrite(displays[indexDisplay][2], low);
      digitalWrite(displays[indexDisplay][3], low);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], high);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case 'C':

      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], low);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], low);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], high);

      break;

    case 'U':

      digitalWrite(displays[indexDisplay][0], low);
      digitalWrite(displays[indexDisplay][1], low);
      digitalWrite(displays[indexDisplay][2], high);
      digitalWrite(displays[indexDisplay][3], high);
      digitalWrite(displays[indexDisplay][4], high);
      digitalWrite(displays[indexDisplay][5], low);
      digitalWrite(displays[indexDisplay][6], low);

      break;
  }
}

int restHumidity(String command)
{
  return readHumiditySensor(command.toInt()) * 100;
}

int restTemperature(String command)
{
  return readTemperatureSensor(command.toInt()) * 100;
}

int restRead(String command)
{
  Serial.println(command);
  Serial.println(digitalRead(command.toInt()));
  return digitalRead(command.toInt());
}

int letsGoTemp(String command)
{
  alarmTemperature();

  return 1;
}

int letsGoHumid(String command)
{
  alarmHumidity(2);

  return 1;
}
