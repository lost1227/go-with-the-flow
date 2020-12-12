﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameState : MonoBehaviour
{
    
    private Vector2 minpos = new Vector2(0, 0);
    private Vector2 maxpos = new Vector2(0, 0);
    private List<StateTrackedObject> water = new List<StateTrackedObject>();
    private List<StateTrackedObject> stillPlatforms = new List<StateTrackedObject>();
    private List<StateTrackedObject> movPlatforms = new List<StateTrackedObject>();


    private void updateBounds(Transform trans) {
        minpos.x = Math.Min(trans.position.x, minpos.x);
        minpos.y = Math.Min(trans.position.y, minpos.y);
        maxpos.x = Math.Max(trans.position.x, maxpos.x);
        maxpos.y = Math.Max(trans.position.y, maxpos.y);
    }

    public void register(StateTrackedObject obj) {
        if(obj is WaterBehavior) {
            WaterBehavior water = obj.GetComponent<WaterBehavior>();
            Assert.IsNotNull(water);
            this.water.Add(obj);
        } else if(obj is MovPlatformBehavior) {
            MovPlatformBehavior platform = obj.GetComponent<MovPlatformBehavior>();
            Assert.IsNotNull(platform);
            this.movPlatforms.Add(obj);
        } else if(obj is StillPlatformBehavior) {
            StillPlatformBehavior platform = obj.GetComponent<StillPlatformBehavior>();
            Assert.IsNotNull(platform);
            this.stillPlatforms.Add(obj);
        } else {
            // Should never be reached
            Assert.IsTrue(false);
        }
        updateBounds(obj.transform);
    }

    void Start()
    {
        
    }

    

    public StateTrackedObject[,] buildState()
    {
        Assert.IsTrue(minpos.x < maxpos.x);
        Assert.IsTrue(minpos.y < maxpos.y);
        StateTrackedObject[,] state = new StateTrackedObject[((int)maxpos.x - (int)minpos.x) + 1, ((int)maxpos.y - (int)minpos.y) + 1];
        foreach(StateTrackedObject item in water)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsNull(curr);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }
        foreach (StateTrackedObject item in stillPlatforms)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsNull(curr);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }
        foreach (StateTrackedObject item in movPlatforms)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsTrue(curr == null || curr is WaterBehavior);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }

        return state;
    }

    public StateTrackedObject[,] buildStaticState()
    {
        Assert.IsTrue(minpos.x < maxpos.x);
        Assert.IsTrue(minpos.y < maxpos.y);
        StateTrackedObject[,] state = new StateTrackedObject[((int)maxpos.x - (int)minpos.x) + 1, ((int)maxpos.y - (int)minpos.y) + 1];
        foreach (StateTrackedObject item in water)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsNull(curr);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }
        foreach (StateTrackedObject item in stillPlatforms)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsNull(curr);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }

        return state;
    }

    public StateTrackedObject[,] buildDynamicState()
    {
        Assert.IsTrue(minpos.x < maxpos.x);
        Assert.IsTrue(minpos.y < maxpos.y);
        StateTrackedObject[,] state = new StateTrackedObject[((int)maxpos.x - (int)minpos.x) + 1, ((int)maxpos.y - (int)minpos.y) + 1];
        foreach (StateTrackedObject item in movPlatforms)
        {
            StateTrackedObject curr = state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)];
            Assert.IsTrue(curr == null || curr is WaterBehavior);
            state[(int)(item.transform.position.x - minpos.x), (int)(item.transform.position.y - minpos.y)] = item;
        }

        return state;
    }

    public Vector2 gridToGlobal(Vector2 grid)
    {
        return grid + minpos;
    }

    public Vector2 globalToGrid(Vector2 global)
    {
        Vector2 snapped = new Vector2(Mathf.Floor(global.x), Mathf.Floor(global.y));
        return snapped - minpos;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
