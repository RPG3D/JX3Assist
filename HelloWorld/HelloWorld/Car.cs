using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    public class Car
    {
        public string petName { get; set; }
        public int currSpeed { get; set; }
        public int maxSpeed { get; set; } = 100;

        private bool carIsDead;

        public Car() { }
        public Car(string name, int maxSp, int currSp)
        {
            currSpeed = currSp;
            maxSpeed = maxSp;
            petName = name;
        }

        public delegate void CarEngineHandler(string msgForCaller);

        private CarEngineHandler listOfhandlers;

        public void RegisterWithCarEngine(CarEngineHandler methodToCall)
        {
            listOfhandlers = methodToCall;
        }

        public void Accelerate(int delta)
        {
            if(carIsDead)
            {
                if(listOfhandlers != null)
                {
                    listOfhandlers("Sorry, this car is dead");
                }
            }
            else
            {
                currSpeed += delta;

                if(10 == (maxSpeed - currSpeed) && listOfhandlers != null)
                {
                    listOfhandlers("Careful buddy! Gonna blow");
                }
                if(currSpeed >= maxSpeed)
                {
                    carIsDead = true;
                }
                else
                {
                    Console.WriteLine("Current Speed Is {0}", currSpeed);
                }
            }
        }

        public void OnCarEngineEvent(string str)
        {
            Console.WriteLine("=>{0}", str);
        }
    }
}