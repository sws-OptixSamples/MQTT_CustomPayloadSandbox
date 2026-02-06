#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.MQTTClient;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.NativeUI;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
#endregion

public class MqttClientSetupLogic : BaseNetLogic
{
    [ExportMethod]
    public void ConfigMqttClient()
    {
        var epochTimeSec = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var mqttClient = Project.Current.Get<MQTTClient>("MQTT/MQTTClient1");
        mqttClient.ClientId = $"swsClient_{epochTimeSec}";
        Log.Info($"MQTT client ID set to: {mqttClient.ClientId}");
        mqttClient.BrokerAddress = "test.mosquitto.org";
        Log.Info($"MQTT broker address set to: {mqttClient.BrokerAddress}");

        var pubTopic0 = $"swsTopic0_{epochTimeSec}";
        var pubTopic1 = $"swsTopic1_{epochTimeSec}";
        var pubTopic2 = $"swsTopic2_{epochTimeSec}";
        var pubTopic3 = $"swsTopic3_{epochTimeSec}";

        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p0/Topic").Value = pubTopic0;
        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p1/Topic").Value = pubTopic1;
        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p2/Topic").Value = pubTopic2;
        Project.Current.GetVariable("NetLogic/MqttSandbox/P3_Topic").Value = pubTopic3;
        Log.Info($"MQTT client configured seccessfully");

    }

    [ExportMethod]
    public void ClearValues()
    {
        var mqttClient = Project.Current.Get<MQTTClient>("MQTT/MQTTClient1");
        mqttClient.ClientId = $"not updated - run method ConfigMqttClient";
        mqttClient.BrokerAddress = "";
        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p0/Topic").Value = "";
        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p1/Topic").Value = "";
        Project.Current.GetVariable("MQTT/MQTTClient1/MQTTPublisher_p2/Topic").Value = "";
        Project.Current.GetVariable("NetLogic/MqttSandbox/P3_Topic").Value = "";
        Log.Info($"MQTT client config values cleared");

    }
}
