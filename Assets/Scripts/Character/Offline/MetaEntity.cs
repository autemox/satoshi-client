using UnityEngine;
using Unity.Netcode;

public class MetaEntity : INetworkSerializable
{
    public string name;
    public string followTarget;
    public string transitionSprite;
    public string defaultSprite;
    public bool connected;
    public Vector3 position;
    public bool changed;

    public MetaEntity() { }

    public MetaEntity(string name, Vector3 position, string sprite)
    {
        this.name = name == "" ? "Undefined" : name;
        this.position = position;
        this.defaultSprite = sprite == "" ? "Default" : sprite;
        this.transitionSprite = sprite == "" ? "Default" : sprite;
        this.followTarget = "Undefined";
        this.connected = false;
        this.changed = false;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref name);
        serializer.SerializeValue(ref followTarget);
        serializer.SerializeValue(ref transitionSprite);
        serializer.SerializeValue(ref defaultSprite);
        serializer.SerializeValue(ref connected);

        // Serialize the patrol position (Vector3)
        serializer.SerializeValue(ref position.x);
        serializer.SerializeValue(ref position.y);
        serializer.SerializeValue(ref position.z);

        serializer.SerializeValue(ref changed);
    }
}