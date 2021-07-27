using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RadarHitList {
    private int NextToAddIndex { get; set; } = 0;
    private Transform[] RadarHits;
    private int size;

    public RadarHitList(int size) {
        this.size = size;
        RadarHits = new Transform[size];
    }

    public void Add(Transform newTransform) {
        if(NextToAddIndex==size-1) {
            NextToAddIndex = 0;
        }
        RadarHits[NextToAddIndex] = newTransform;
        NextToAddIndex++;
    }

    public Transform GetCurrent() => NextToAddIndex == 0 ? RadarHits[size - 1] : RadarHits[NextToAddIndex-1];
    public List<Transform> GetLast(int amount) {
        List<Transform> returnList = new List<Transform>(amount);
        if(0 >= NextToAddIndex - amount) {
            for(int i = NextToAddIndex-1; i >= 0; i--) {
                returnList.Add(RadarHits[i]);
            }
            int amountLeft = System.Math.Abs(NextToAddIndex - amount);
            for(int i = size-1; i > size-1-amountLeft; i--) {
                returnList.Add(RadarHits[i]);
            }
        }
        else {
            for(int i = NextToAddIndex-1; i > NextToAddIndex-amount; i--) {
                returnList.Add(RadarHits[i]);
            }
        }
        return returnList;
    }
}

