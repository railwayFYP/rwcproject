using UnityEngine;
using System.Collections;

public class ObjData : MonoBehaviour {
    public int              posX = 0;
    public int              posY = 0;
    
    public bool             isTrain = false;
    public bool             isBuilding = false;
    public bool             isTrack = false;

    public Building         type;
    public TrainType        trainType;
    public Track            trackType;
}
