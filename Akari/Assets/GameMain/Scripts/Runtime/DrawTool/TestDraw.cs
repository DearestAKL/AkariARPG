using UnityEngine;
using System.Collections;

public class TestDraw : MonoBehaviour
{

    void Start()
    {
        //DrawTool.DrawSector(transform, transform.localPosition, 60, 3);

        //DrawTool.DrawCircle(transform, transform.localPosition, 3);

        //DrawTool.DrawRectangle(transform, transform.localPosition, 5, 2);

        //DrawTool.DrawSectorSolid(transform, transform.localPosition, 60, 3);

        //DrawTool.DrawCircleSolid(transform, transform.localPosition, 3);

        DrawTool.DrawRectangleSolid(transform, transform.localPosition, 5, 2);
    }

}
