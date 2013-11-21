using UnityEngine;
using System.Collections;
 
public class ControllerBogieCar : MonoBehaviour {
       
        private bool leftBrake = false;
        private bool rightBrake = false;
       
        // Use this for initialization
        void Start () {
       
        }
       
        // Update is called once per frame
        void Update () {
       
        }
       
        [RPC]
        void pressRightBrake() {
                rightBrake = true;     
                Debug.Log("right brake is now "+rightBrake);
        }      
       
        [RPC]
        void releaseRightBrake() {
                rightBrake = false;                    
        }      
       
        [RPC]
        void pressLeftBrake() {
                leftBrake = true;
        }      
       
        [RPC]
        void releaseLeftBrake() {
                leftBrake = false;
        }
}