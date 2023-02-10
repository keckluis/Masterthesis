volatile long temp, counter = 0;
    
void setup() 
{
  Serial.begin(9600);  

  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(2), ai0, RISING);
  attachInterrupt(digitalPinToInterrupt(3), ai1, RISING);
}

void loop() 
{
  if (counter != temp)
  {
    Serial.print("H");
    Serial.println(counter);
    temp = counter;
  }

  delay(100);
}
   
void ai0() 
{
  if (digitalRead(3) == LOW) 
  {
    counter++;
  }
  else
  {
    counter--;
  }
}
  
void ai1() 
{
  if (digitalRead(2) == LOW) 
  {
    counter--;
  }
  else
  {
    counter++;
  }
}
