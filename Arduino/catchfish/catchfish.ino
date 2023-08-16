#include <MPU6050_tockn.h>

MPU6050 mpu6050(Wire);

#include <SoftwareSerial.h>

int blueRx=3;
int blueTx=2;
SoftwareSerial mySerial(blueRx, blueTx);
int baudRateNum = 9600;

long prev_x = 0;
long prev_y = 0;
long prev_z = 0;
long curr_x, curr_y, curr_z;

const int yPin = 0;

int minVal = 265;
int maxVal = 402;

double curr;
double prev = 0;




void setup() {
  Serial.begin(baudRateNum);
  mySerial.begin(baudRateNum);
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}

void loop() {
  mpu6050.update();
  curr_x = mpu6050.getAngleX();
  curr_y = mpu6050.getAngleY();
  curr_z = mpu6050.getAngleZ();

  curr = analogRead(yPin);

  if (abs(curr_x - prev_x) > 20 || abs(curr_y - prev_y) > 20 || abs(curr_z - prev_z) > 20) {
    if ((curr - prev) > 130) {
      mySerial.println(7);
    } else {
      mySerial.println(5);
    }
  } else {
    if ((curr - prev) > 100) {
      mySerial.println(3);
    } else {
      mySerial.println(1);
    }
  }

  prev_x = curr_x;
  prev_y = curr_y;
  prev_z = curr_z;

  prev = curr;

  delay(200);  
}
