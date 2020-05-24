using UnityEngine;
using Random = System.Random;


namespace DLLTest {

    public class MyUtilities {
    
        private float c;

        public void AddValues(float a, float b) {
            c = a + b;  
        }
    
        public static int GenerateRandom(int min, int max) {
            var rand = new Random();
            return rand.Next(min, max);
        }
        
        public float GetValue()
        {
            return c + Camera.VerticalToHorizontalFieldOfView(1, 1);
        }
    }
}