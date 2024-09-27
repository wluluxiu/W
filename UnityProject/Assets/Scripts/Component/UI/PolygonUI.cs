using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolygonUI : MaskableGraphic
{
    public bool fill = true;
    public float thickness = 5;
    [Range(3, 360)]
    public int sides = 3;
    [Range(0, 360)]
    public float rotation = 0;
    [Range(0, 1)]
    public float[] VerticesDistances = new float[3];
    private float size = 0;

    public List<UIVertex[]> vbos = new List<UIVertex[]>();
    public List<GameObject> GameObjects = new List<GameObject>();

    protected override void OnEnable()
    {
        base.OnEnable();
        GameObjects.Clear();
    }

    void Update()
    {
        size = rectTransform.rect.width;
        if (rectTransform.rect.width > rectTransform.rect.height)
            size = rectTransform.rect.height;
        else
            size = rectTransform.rect.width;
        thickness = (float)Mathf.Clamp(thickness, 0, size / 2);
        
        if(GameObjects.Count != 0)
            return;
        for (int i = 0; i < vbos.Count; i++)
        {
            var gameObject = new GameObject();
            gameObject.transform.position = vbos[i][0].position;
            gameObject.transform.SetParent(transform,false);
            GameObjects.Add(gameObject);
        }
    }
 
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
 
        Vector2 prevX = Vector2.zero;
        Vector2 prevY = Vector2.zero;
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;
        float degrees = 360f / sides;
        int vertices = sides + 1;
        if (VerticesDistances.Length != vertices)
        {
            VerticesDistances = new float[vertices];
            for (int i = 0; i < vertices - 1; i++) VerticesDistances[i] = 1;
        }
  
        VerticesDistances[vertices - 1] = VerticesDistances[0];
        vbos.Clear();
        for (int i = 0; i < vertices; i++)
        {
            float outer = -rectTransform.pivot.x * size * VerticesDistances[i];
            float inner = -rectTransform.pivot.x * size * VerticesDistances[i] + thickness;
            float rad = Mathf.Deg2Rad * (i * degrees + rotation);
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            uv0 = new Vector2(0, 1);
            uv1 = new Vector2(1, 1);
            uv2 = new Vector2(1, 0);
            uv3 = new Vector2(0, 0);
            pos0 = prevX;
            pos1 = new Vector2(outer * c, outer * s);
            if (fill)
            {
                pos2 = Vector2.zero;
                pos3 = Vector2.zero;
            }
            else
            {
                pos2 = new Vector2(inner * c, inner * s);
                pos3 = prevY;
            }
            prevX = pos1;
            prevY = pos2;
            
            UIVertex[] vbo = new UIVertex[4];
            var vs = new[] { pos0, pos1, pos2, pos3 };
            var uvs = new[] { uv0, uv1, uv2, uv3 };
            for (int j = 0; j < vs.Length; j++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vs[j];
                vert.uv0 = uvs[j];
                vbo[j] = vert;
            }
            vh.AddUIVertexQuad(vbo);
            vbos.Add(vbo);
        }
    }
}