using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData : MonoBehaviour {
    // The level of the mission
    public int Level = 0;

    // Tracks that are given on the level
    public int VertTrack    = 0;
    public int HoriTrack    = 0;
    public int URTrack      = 0;
    public int ULTrack      = 0;
    public int DRTrack      = 0;
    public int DLTrack      = 0;

    // Position for train to start
    public int sGridX = 0;
    public int sGridY = 0;

    // Position for track to reach
    public int eGridX = 0;
    public int eGridY = 0;

    // Map size
    public int mSizeX = 5;
    public int mSizeY = 5;

    public List<ObjData> vObjects = new List<ObjData>();

    // Mission Mode Levels
    public void loadGameLevel(int _level)
    {
        switch (_level)
        {
            case 1:
                {
                    VertTrack = 1;
                    HoriTrack = 4;
                    URTrack   = 1;
                    ULTrack   = 1;
                    DRTrack   = 2;
                    DLTrack   = 1;

                    sGridX = 1;
                    sGridY = 0;

                    eGridX = 4;
                    eGridY = 4;

                    mSizeX = 5;
                    mSizeY = 5;

                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 0;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 0;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 3;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Horizontal;
                    temp.posX = eGridX;
                    temp.posY = eGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                }
            case 2:
                {
                    VertTrack = 2;
                    HoriTrack = 3;
                    URTrack = 2;
                    ULTrack = 1;
                    DRTrack = 1;
                    DLTrack = 2;

                    sGridX = 3;
                    sGridY = 0;

                    eGridX = 1;
                    eGridY = 6;

                    mSizeX = 7;
                    mSizeY = 7;

                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 2;
                    temp.posY = 1;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill2;
                    temp.posX = 2;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Mountain;
                    temp.posX = 0;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Depot;
                    temp.posX = eGridX;
                    temp.posY = eGridY;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                }
            case 3:
                {
                    VertTrack = 2;
                    HoriTrack = 3;
                    URTrack = 3;
                    ULTrack = 1;
                    DRTrack = 1;
                    DLTrack = 4;

                    sGridX = 6;
                    sGridY = 0;

                    eGridX = 0;
                    eGridY = 6;

                    mSizeX = 9;
                    mSizeY = 9;

                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 2;
                    temp.posY = 0;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 1;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 3;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 8;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 0;
                    temp.posY = 5;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 2;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 8;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Horizontal;
                    temp.posX = eGridX;
                    temp.posY = eGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                }
        }
    }

    // Time Attack Levels
    public void loadTimeLevel(int _level)
    {
        switch (_level)
        {
            case 1:
                {
                    sGridX = 4;
                    sGridY = 0;

                    eGridX = 4;
                    eGridY = 4;

                    mSizeX = 9;
                    mSizeY = 9;

                    // First Row
                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 1;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 2;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 7;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Second Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 0;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 1;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 2;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 6;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill;
                    temp.posX = 7;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Windmill2;
                    temp.posX = 8;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Third Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 1;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 2;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 7;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Fourth Row
                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 0;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Mountain;
                    temp.posX = 1;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 2;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 6;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Mountain;
                    temp.posX = 7;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Mountain;
                    temp.posX = 8;
                    temp.posY = 8;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                }
            case 2:
                {
                    sGridX = 4;
                    sGridY = 0;

                    eGridX = 4;
                    eGridY = 4;

                    mSizeX = 9;
                    mSizeY = 9;

                    // First Row
                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 1;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 2;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 7;
                    temp.posY = 2;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Second Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 1;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 7;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Third Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 1;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 7;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Fourth Row
                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 1;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();
                    temp.type = Building.Mountain;
                    temp.posX = 2;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 6;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Mountain;
                    temp.posX = 7;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);
                
                    // Starting Point

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                }
            case 3:
                {
                    sGridX = 4;
                    sGridY = 0;

                    eGridX = 4;
                    eGridY = 4;

                    mSizeX = 9;
                    mSizeY = 9;

                    // First Row
                    ObjData temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 3;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 5;
                    temp.posY = 3;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Second Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 2;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 4;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Third Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 2;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 6;
                    temp.posY = 6;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Fourth Row
                    temp = new ObjData();

                    temp.type = Building.Windmill2;
                    temp.posX = 3;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    temp = new ObjData();

                    temp.type = Building.Windmill;
                    temp.posX = 5;
                    temp.posY = 7;
                    temp.isBuilding = true;

                    vObjects.Add(temp);

                    // Starting Point

                    temp = new ObjData();
                    temp.trackType = Track.Vertical;
                    temp.posX = sGridX;
                    temp.posY = sGridY;
                    temp.isTrack = true;

                    vObjects.Add(temp);
                    break;
                    break;
                }

            default: break;

        }
    }
}
