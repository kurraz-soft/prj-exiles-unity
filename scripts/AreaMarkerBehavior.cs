using UnityEngine;

public class AreaMarkerBehavior : MonoBehaviour
{
    public float radius;
    public Color color = Color.yellow;

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnRenderObject()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);

        GL.Color(color);

        float x1, y1, x2, y2;
        const float step = 0.2f;

        for (x1 = 0; x1 <= radius; x1 += step)
        {
            x2 = x1 + step;

            var t1 = radius*radius - x1*x1;
            var t2 = radius*radius - x2*x2;

            y1 = t1 > 0 ? Mathf.Sqrt(t1) : 0;
            y2 = t2 > 0 ? Mathf.Sqrt(t2) : 0;

            GL.Vertex3(x1,0.1f,y1);
            GL.Vertex3(x2, 0.1f, y2);
            
            GL.Vertex3(x1,0.1f,-y1);
            GL.Vertex3(x2, 0.1f, -y2);

            GL.Vertex3(-x1, 0.1f, y1);
            GL.Vertex3(-x2, 0.1f, y2);
            
            GL.Vertex3(-x1, 0.1f, -y1);
            GL.Vertex3(-x2, 0.1f, -y2);
        }

        GL.End();
        GL.PopMatrix();
    }
}

