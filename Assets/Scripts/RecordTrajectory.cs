using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordTrajectory : MonoBehaviour
{
    public string participant_number;

    string PATH;
    float t = 0;
    StreamWriter sw;

    // Start is called before the first frame update
    void Start()
    {
        PATH = @"D:\1 Research\2_HDBE_Model\_Trajectory_data\trajectory_" + participant_number + ".csv";
        sw = File.AppendText(PATH);
        sw.WriteLine("x_position" + "," + "y_position" + "," + "z_position" + "," + "timestamp");
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time == 0.0f || Time.time - t > 0.5f)
        {
            float x = this.transform.position.x;
            float height = this.transform.position.y;
            float z = this.transform.position.z;
            StreamWriter sw = File.AppendText(PATH);
            sw.WriteLine(x + "," + height + "," + z + "," + Time.time);
            sw.Close();
            t = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StreamWriter sw = File.AppendText(PATH);
            sw.WriteLine("Start time:" + Time.time);
            sw.Close();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StreamWriter sw = File.AppendText(PATH);
            sw.WriteLine("Finish time: " + Time.time);
            sw.Close();
        }
    }
}
