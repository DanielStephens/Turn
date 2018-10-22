using UnityEngine;
using System.Collections.Generic;
using System;

public class ExampleRotation : MonoBehaviour {

    Routine<Transform, Void> verticalRotator;
    Routine<Transform, Void> horizontalRotator;

    Queue<string> queueDescription = new Queue<string>();
    string futureDescription;
    string description = "DOING NOTHING";

    Queue<Routine<Transform, Void>> queuedTasks = new Queue<Routine<Transform, Void>>();
    Future<Transform, Void> future;

    bool paused = false;
    
    void Start () {
        verticalRotator = RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.up, 90, 1);
        horizontalRotator = RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.right, 90, 1);
    }
	
    void Update()
    {
        processPause();
        processActions();
        processQueue();

        description = generateDescription();
    }

    private string generateDescription()
    {
        string description = futureDescription == null ? "DOING NOTHING" : paused ? "PAUSED \t" + futureDescription :  "GOING  \t" + futureDescription;

        foreach(string d in queueDescription)
        {
            description += Environment.NewLine + "\t" + d;
        }

        return description;
    }

    private void processQueue()
    {
        if(queuedTasks.Count == 0)
        {
            if(future == null || future.IsDone())
            {
                futureDescription = null;
            }
            return;
        }

        if(future != null && !future.IsDone())
        {
            return;
        }

        futureDescription = queueDescription.Dequeue();
        future = queuedTasks.Dequeue().Start();
        if (paused)
        {
            future.Pause();
        }
    }

    private void processActions()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queueDescription.Enqueue("UP");
            queuedTasks.Enqueue(RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.right, 90, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queueDescription.Enqueue("DOWN");
            queuedTasks.Enqueue(RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.right, -90, 1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queueDescription.Enqueue("RIGHT");
            queuedTasks.Enqueue(RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.up, -90, 1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queueDescription.Enqueue("LEFT");
            queuedTasks.Enqueue(RotationUtils.RotateAround(transform, t => transform.position, t => Vector3.up, 90, 1));
        }
    }


    private void processPause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (future != null)
            {
                if (paused)
                {
                    future.Resume();
                }
                else
                {
                    future.Pause();
                }
            }
            paused = !paused;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width, 40), "Try spamming some of the arrow keys to start rotating the cube. Press the space key to pause rotation.");
        GUI.Label(new Rect(10, 60, Screen.width, 20), "ACTION QUEUE:");
        GUI.Label(new Rect(10, 90, Screen.width, Screen.height-70), description);
    }

}
