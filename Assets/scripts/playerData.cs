using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct playerData :IEquatable<playerData>, INetworkSerializable
{
    public ulong clientId;
    public int skinId;

    public bool Equals(playerData other)
    {
        return clientId == other.clientId && skinId == other.skinId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref skinId);
    }
}
