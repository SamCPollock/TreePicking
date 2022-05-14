using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    //****Static properties for physics objects (fruits).****//
    public static Vector3 gravity = new Vector3(0, -0.004f, 0);
    public static float floorLevel = -4.1f;
    public static float screenWidth = 8.5f;
}
