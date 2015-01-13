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
    public int sGrixY = 0;

    // Position for track to reach
    public int eGridX = 0;
    public int eGridY = 0;

    // Map size
    public int mSizeX = 5;
    public int mSizeY = 5;

    public List<BuildingData> vBuildings = new List<BuildingData>();

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
                    sGrixY = 0;

                    eGridX = 4;
                    eGridY = 4;

                    mSizeX = 5;
                    mSizeY = 5;

                    BuildingData temp = new BuildingData();

                    temp.type = Building.Windmill;
                    temp.posX = 4;
                    temp.posY = 0;

                    vBuildings.Add(temp);

                    temp = new BuildingData();

                    temp.type = Building.Windmill;
                    temp.posX = 0;
                    temp.posY = 2;

                    vBuildings.Add(temp);

                    temp = new BuildingData();
                    temp.type = Building.Windmill;
                    temp.posX = 3;
                    temp.posY = 3;

                    vBuildings.Add(temp);

                    temp = new BuildingData();
                    temp.type = Building.Depot;
                    temp.posX = sGridX;
                    temp.posY = sGrixY;

                    vBuildings.Add(temp);
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }
        }
    }
}
