
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <DallasTemperature.h>

#define ONE_WIRE_BUS_1 2

#define BUFFER_SIZE 100
int aPin = A0;

OneWire oneWire_in(ONE_WIRE_BUS_1);
DallasTemperature sensor_inhouse(&oneWire_in);

const char *ssid =  "Home Wi-fi";  // Имя вайфай точки доступа
const char *pass =  "375291183653"; // Пароль от точки доступа
const char *mqtt_server = "192.168.1.164"; // Имя сервера MQTT
const int mqtt_port = 1884; // Порт для подключения к серверу MQTT
const char *mqtt_user = "User";
const char *mqtt_pass = "u@'xSQjBM&6~nMEd"; // Пароль от сервера
const char *mqtt_topic_SoilMoisture = "soilmoisture/";
const char *mqtt_topic_temperature = "temperature/";
const char *mqtt_led_topic = "led/";

const char *temp_id_sensor = "c684c085-927b-4c76-ba2a-d0084ea76a06";
const char *soil_Moisture_id_sensor = "b14979d1-3854-4b92-beca-b5257d4e6dbd";
const char *led_id = "e065bd8b-257c-4e9a-916a-d6f58a014825";
int releLedPin = 5;
float tempArray[10] = { -127, -127, -127, -127, -127,-127, -127, -127, -127, -127,};
int spoilArray[10] = { -127, -127, -127, -127, -127,-127, -127, -127, -127, -127,};
int counter = 0;
int counter1 = 0;

WiFiClient wclient;
PubSubClient client(wclient, mqtt_server, mqtt_port);

int tm = 3000;

void setup() {
  sensor_inhouse.begin();
  Serial.begin(115200);
  delay(10);
  Serial.println("****************************************");
  Serial.println();
  pinMode(releLedPin, OUTPUT);
  digitalWrite(releLedPin, HIGH);
}

String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = { 0, -1 };
  int maxIndex = data.length() - 1;

  for (int i = 0; i <= maxIndex && found <= index; i++) {
    if (data.charAt(i) == separator || i == maxIndex) {
      found++;
      strIndex[0] = strIndex[1] + 1;
      strIndex[1] = (i == maxIndex) ? i + 1 : i;
    }
  }
  return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}
// Функция получения данных от сервера
void callback(const MQTT::Publish& pub)
{

  Serial.print(pub.topic());   // выводим в сериал порт название топика
  Serial.print(" => ");
  Serial.print(pub.payload_string()); // выводим в сериал порт значение полученных данных
  Serial.println();

  String  payload = pub.payload_string();

  String _topic =  String(mqtt_led_topic) + "publish" + String(led_id);
  if (String(pub.topic()) == _topic) // проверяем из нужного ли нам топика пришли данные
  {
    String command = getValue(payload, '/', 1);
    SetPowerLed(command);
    Serial.println(command);
  }
}

void loop() {
  // подключаемся к wi-fi
  if (WiFi.status() != WL_CONNECTED) {
    Serial.print("Connecting to ");
    Serial.print(ssid);
    Serial.println("...");
    WiFi.begin(ssid, pass);

    if (WiFi.waitForConnectResult() != WL_CONNECTED)
      return;
    Serial.println("WiFi connected");
  }

  // подключаемся к MQTT серверу
  if (WiFi.status() == WL_CONNECTED) {
    if (!client.connected()) {
      Serial.println("Connecting to MQTT server");
      if (client.connect(MQTT::Connect("MqttClient-1").set_auth(mqtt_user, mqtt_pass))) {
        Serial.println("Connected to MQTT server");
        delay(1000);

        String _topic =  String(mqtt_led_topic) + "publish" + String(led_id);
        client.subscribe(_topic);
        client.set_callback(callback);
      } else {
        Serial.println("Could not connect to MQTT server");
      }
    }

    if (client.connected()) {
      client.loop();
      TempSend();
      delay(1000);
      SoilMoistureSensor();
      delay(1000);
      PublishStateLed();
      delay(5000);
    }
  }
} // конец основного цикла


// Функция отправки показаний с термодатчика
void TempSend() {
  sensor_inhouse.requestTemperatures();
  float temp = sensor_inhouse.getTempCByIndex(0);

  if (counter == 10)
    counter = 0;

  tempArray[counter] = temp;
  counter++;
  String arr = "";
  for (int i = 0; i < 10; i++)
  {
    arr +=  String(tempArray[i]) + "/";
  }
  Serial.println(arr);
  Serial.println(temp);
  String _topic =  String(mqtt_topic_temperature) + String(temp_id_sensor);
  client.publish(_topic , String(temp) + "/" + arr); // send

  delay(10);
}

int SoilMoistureSensor() {

  int avalue = analogRead(aPin);
  if (avalue != 0)
  {

    if (counter1 == 10)
      counter1 = 0;

    spoilArray[counter1] = avalue;
    counter1++;
    String arr = "";
    for (int i = 0; i < 10; i++)
    {
      arr +=  String(spoilArray[i]) + "/";
    }
    
    Serial.print("value="); Serial.println(avalue);
    String _topic =  String(mqtt_topic_SoilMoisture) + String(soil_Moisture_id_sensor);
    client.publish(_topic, String(avalue) + "/" + arr);
  }
  delay(10);
}

void SetPowerLed(String command)
{
  if (command == "on")
  {
    digitalWrite(releLedPin, LOW);//power on
    Serial.println("on-client");
    String _topic =  String(mqtt_led_topic) + String(led_id);
    client.publish(_topic, "on");
  }

  if (command == "off")
  {
    digitalWrite(releLedPin, HIGH);
    Serial.println("off-client");
    String _topic =  String(mqtt_led_topic) + String(led_id);
    client.publish(_topic, "off");
  }
  delay(10);
}
void PublishStateLed() {
  int state =   digitalRead(releLedPin);
  String power = "";
  if (state == 0) // реле низкого уровня
    power = "on";
  if (state == 1)
    power = "off";

  String _topic =  String(mqtt_led_topic) + String(led_id);
  Serial.println(power);
  client.publish(_topic, power);
}
