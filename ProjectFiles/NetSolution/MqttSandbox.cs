#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.MQTTClient;
using System.Collections.Generic;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
#endregion

public class MqttSandbox : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    // example publish method with a single data item
    [ExportMethod]
    public void PublishFromNetLogic_p3_1()
    {
        // folder with data items and values
        var dataFolder = Project.Current.Get<Folder>("Model/MQTT_VarFolder3");
        var dataItem1 = dataFolder.Get("PartData301");

        // create a data item list from one of the custom data item classes
        var dataItems = new List<MqttDataItem2>
        {
            // add new data item for each item to publish
            new MqttDataItem2
            {
                Name = (string)dataItem1.GetVariable("PartNumber").Value,
                Value = (string)dataItem1.GetVariable("Count").Value,
                Type = "String",
                Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz")
            }
        };

        // create config object with NetLogic variables
        var config = new MqttConfig
        {
            Topic = (string)LogicObject.GetVariable("P3_Topic").Value,
            QoS = (int)LogicObject.GetVariable("P3_QOS").Value,
            Retain = (bool)LogicObject.GetVariable("P3_Retain").Value,
        };

        // create payload object from one of the custom payload classes
        var payload = new MqttPayload3<MqttDataItem2>
        {
            // what time format and/or UTC offset does your application require?
            PublishTimestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
            PublishCount = dataItems.Count,
            ObjectArrayName = dataItems
        };

        // serialize the config and payload objects to JSON strings
        string configJson = JsonSerializer.Serialize(config);
        string payloadJson = JsonSerializer.Serialize(payload);

        // publish the message using the MQTTClient instance in the project
        var mqttClient = Project.Current.Get<MQTTClient>("MQTT/MQTTClient1");
        mqttClient.Publish(configJson, payloadJson);

    }

    // example publish method with a single data item
    [ExportMethod]
    public void PublishFromNetLogic_p3_3()
    {
        // folder with data items and values
        var dataFolder = Project.Current.Get<Folder>("Model/MQTT_VarFolder3");
        var dataItem1 = dataFolder.Get("PartData300");
        var dataItem2 = dataFolder.Get("PartData301");
        var dataItem3 = dataFolder.Get("PartData302");

        // create a data item list from one of the custom data item classes
        var dataItems = new List<MqttDataItem4>
        {
            // add new data item for each item to publish
            new MqttDataItem4
            {
                Count = (string)dataItem1.GetVariable("Count").Value,
                PartNumber = (string)dataItem1.GetVariable("PartNumber").Value,
                OtherVariables = (string)dataItem1.GetVariable("PartNumber").Value
            },
            new MqttDataItem4
            {
                Count = (string)dataItem2.GetVariable("Count").Value,
                PartNumber = (string)dataItem2.GetVariable("PartNumber").Value,
                OtherVariables = (string)dataItem2.GetVariable("PartNumber").Value
            },
            new MqttDataItem4
            {
                Count = (string)dataItem3.GetVariable("Count").Value,
                PartNumber = (string)dataItem3.GetVariable("PartNumber").Value,
                OtherVariables = (string)dataItem3.GetVariable("PartNumber").Value
            }

        };

        // create config object with NetLogic variables
        var config = new MqttConfig
        {
            Topic = (string)LogicObject.GetVariable("P3_Topic").Value,
            QoS = (int)LogicObject.GetVariable("P3_QOS").Value,
            Retain = (bool)LogicObject.GetVariable("P3_Retain").Value,
        };

        // create payload object from one of the custom payload classes
        var payload = new MqttPayload1<MqttDataItem4>
        {
            // what time format and/or UTC offset does your application require?
            Version = "1.0.3",
            Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
            ProcessedCount = dataItems
        };

        // serialize the config and payload objects to JSON strings
        string configJson = JsonSerializer.Serialize(config);
        string payloadJson = JsonSerializer.Serialize(payload);

        // publish the message using the MQTTClient instance in the project
        var mqttClient = Project.Current.Get<MQTTClient>("MQTT/MQTTClient1");
        mqttClient.Publish(configJson, payloadJson);

    }

    // these are a few classes created to clean up the code and make it a bit more reusable.
    // as your payload or data item requirements change, you can create new classes or modify these ones as needed.

    public class MqttConfig
    {
        public string Topic { get; set; }
        public int QoS { get; set; }
        public bool Retain { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    // custom payload 01
    // adjust the initial payload as needed
    // the TDataItem will be specified when creating the class instance
    public class MqttPayload1<TDataItem>
    {
        public string Version { get; set; }
        public string Timestamp { get; set; }
        // The data list is now a List of whatever type TDataItem is specified
        public List<TDataItem> ProcessedCount { get; set; }

        // This generic type is still fully serializable!
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    // custom payload 02
    // adjust the initial payload as needed
    // the TDataItem will be specified when creating the class instance
    public class MqttPayload2<TDataItem>
    {
        public string Timestamp { get; set; }
        // The data list is now a List of whatever type TDataItem is specified
        public List<TDataItem> ObjectArrayName { get; set; }

        // This generic type is still fully serializable!
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    // custom payload 03
    // adjust the initial payload as needed
    // the TDataItem will be specified when creating the class instance
    public class MqttPayload3<TDataItem>
    {
        public string PublishTimestamp { get; set; }
        public int PublishCount { get; set; }
        // The data list is now a List of whatever type TDataItem is specified
        public List<TDataItem> ObjectArrayName { get; set; }

        // This generic type is still fully serializable!
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    // custom data item 01
    // adjust as needed
    // will be specified when creating payload instance
    public class MqttDataItem1
    {
        public string Count { get; set; }
        public string PartNumber { get; set; }
    }

    // custom data item 02
    // adjust as needed
    // will be specified when creating payload instance
    public class MqttDataItem2
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Timestamp { get; set; }
    }

    // custom data item 03
    // adjust as needed
    // will be specified when creating payload instance
    public class MqttDataItem3
    {
        public string V { get; set; }
        public string T { get; set; }
        public string Q { get; set; }
    }

    // custom data item 04
    // adjust as needed
    // will be specified when creating payload instance
    public class MqttDataItem4
    {
        public string Count { get; set; }
        public string PartNumber { get; set; }
        public string OtherVariables { get; set; }
    }

}
