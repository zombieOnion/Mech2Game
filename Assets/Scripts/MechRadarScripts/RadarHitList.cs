using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RadarHitList <T> {
    protected int NextToAddIndex { get; set; } = 0;
    public int Size { get => size; private set => size = value; }

    protected T[] RadarHits;
    private int size;

    public RadarHitList(int size) {
        this.Size = size;
        RadarHits = new T[size];
    }

    public void Add(T newTransform)
    {
        RadarHits[NextToAddIndex] = newTransform;
        NextToAddIndex++;
        if (NextToAddIndex==Size) {
            NextToAddIndex = 0;
        }
    }

    public T GetCurrent() => NextToAddIndex == 0 ? RadarHits[Size - 1] : RadarHits[NextToAddIndex-1];

    public T AdvanceNext()
    {
        var current = GetCurrent();
        NextToAddIndex++;
        if (NextToAddIndex == Size)
        {
            NextToAddIndex = 0;
        }
        return current;
    }
    public List<T> GetLast(int amount) {
        if(amount > RadarHits.Length)
            amount = RadarHits.Length;
        //Debug.Log($"amount: {amount} NextToAddIndex: {NextToAddIndex} RadarHits: {RadarHits.Length}");
        List<T> returnList = new List<T>(amount);
        if(0 >= NextToAddIndex - amount) {
            for(int i = NextToAddIndex-1; i >= 0; i--) {
                returnList.Add(RadarHits[i]);
            }
            int amountLeft = System.Math.Abs(NextToAddIndex - amount);
            for(int i = Size-1; i > Size-1-amountLeft; i--) {
                returnList.Add(RadarHits[i]);
            }
        }
        else {
            for(int i = NextToAddIndex-1; i >= NextToAddIndex-amount; i--) {
                returnList.Add(RadarHits[i]);
            }
        }
        return returnList;
    }
}

