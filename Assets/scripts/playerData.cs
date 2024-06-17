using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct playerData :IEquatable<playerData>, INetworkSerializable
{
    public ulong clientId;
    public int skinId;
    public FixedString64Bytes playerId;

    public bool Equals(playerData other)
    {
        return 
            clientId == other.clientId && 
            skinId == other.skinId &&
            playerId == other.playerId;
        
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref skinId);
        serializer.SerializeValue(ref playerId);
    }
}
