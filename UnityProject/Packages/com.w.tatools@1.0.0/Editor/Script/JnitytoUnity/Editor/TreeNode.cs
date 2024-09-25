using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public long localId;
    public int typeId;
    public string nodeInfo;
    public string objectName;

    public TreeNode(long localId, int typeId, string nodeInfo)
    {
        this.localId = localId;
        this.typeId = typeId;
        this.nodeInfo = nodeInfo;
    }
}
