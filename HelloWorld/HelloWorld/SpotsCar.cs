using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    public class SportsCar : Car
    {
        public string GetPetName()
        {
            //throw new System.NotImplementedException();
            petName = "SportCar";
            return petName;
        }
    }
}