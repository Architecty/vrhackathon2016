using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class buildWall : MonoBehaviour
{
    public Material startMaterial;
    public Material intermediateMaterial;
    public Material trackerMaterial;
    public Material wallMaterial;
    public Material floorMaterial;
    public Material ceilingMaterial;
    public Material lineMaterial;

    SteamVR_TrackedObject trackedObj;
    public bool isEnabled = false;
    bool buildingWall = false;

    public LayerMask touchableMask;
    public bool chainWalls = true;

    GameObject wallStart;
    GameObject wallEnd;
    GameObject trackObject = null;

    public List<GameObject> chainedCorners = new List<GameObject>();
    public List<Vector3> chainedPoints = new List<Vector3>();
    GameObject wallLine = null;


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (isEnabled)
        {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                createTrackableObject();
            }
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                addWallPoint();
            }
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {

            }
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {

            }
        }

    }

    void Update()
    {
        if (trackObject != null)
        {
            trackObject.transform.position = roundToNearest(getRaycastPoint(), 0.3048f);
        }
    }

    void createTrackableObject()
    {
        if (trackObject == null)
        {
            Vector3 startPoint = getRaycastPoint();
            if (startPoint != Vector3.zero)
            {
                GameObject starter = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
                starter.transform.position = startPoint;
                starter.transform.localScale = new Vector3(.2f, .2f, .2f);
                starter.GetComponent<Renderer>().material = trackerMaterial;
                trackObject = starter;
            }
        }
    }

    void addWallPoint()
    {
        if (chainedPoints.Count == 0)
        {
            GameObject newPoint = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
            newPoint.transform.position = trackObject.transform.position;
            newPoint.transform.localScale = new Vector3(.2f, .2f, .2f);
            newPoint.GetComponent<Renderer>().material = startMaterial;
            chainedPoints.Add(newPoint.transform.position);
            chainedCorners.Add(newPoint);
        }
        else {
            if (chainWalls)
            {
                if (trackObject.transform.position == chainedPoints[0])
                {
                    endWall();
                }
                else
                {
                    continueWall();
                }
            }
            else
            {
                continueWall();
                endWall();
            }
        }
    }

    //Continue a chained wall
    void continueWall()
    {
        GameObject newPoint = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        newPoint.transform.position = trackObject.transform.position;
        newPoint.transform.localScale = new Vector3(.2f, .2f, .2f);
        newPoint.GetComponent<Renderer>().material = intermediateMaterial;
        chainedPoints.Add(newPoint.transform.position);
        chainedCorners.Add(newPoint);
    }

    //End a looped wall
    void endWall()
    {

        if (chainWalls == true)
        {
            chainWalls = false;
            CreateObject("Exterior Walls", chainedPoints, 3f, wallMaterial);
            createCaps("Caps", chainedPoints, .01f, floorMaterial, false);
            createCaps("Caps", chainedPoints, 3f, ceilingMaterial, true);
        }
        else
        {
            BuildWallBetweenPoints(chainedPoints[chainedPoints.Count - 2], chainedPoints[chainedPoints.Count - 1], 3f, .1016f, wallMaterial);
        }


        for (int i = 0; i < chainedPoints.Count; i++)
        {
            Destroy(chainedCorners[i]);
        }
        chainedCorners = new List<GameObject>();
        chainedPoints = new List<Vector3>();
    }


    void enableWallBuilder()
    {
        transform.GetComponent<SteamVR_LaserPointer>().enabled = true;
        isEnabled = true;
    }

    void disableWallBuilder()
    {
        transform.GetComponent<SteamVR_LaserPointer>().enabled = false;
        isEnabled = false;
    }

    Vector3 getRaycastPoint()
    {

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, 100f, touchableMask);
        if (bHit)
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void BuildWallBetweenPoints(Vector3 startPoint, Vector3 endPoint, float height, float thickness, Material whichMaterial)
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        float angle = getYangle(endPoint, startPoint);
        Vector3 midPoint = Vector3.Lerp(startPoint, endPoint, .5f);

        GameObject newWall = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        newWall.transform.position = new Vector3(midPoint.x, height / 2, midPoint.z);
        newWall.transform.localScale = new Vector3(distance + thickness, height, thickness);


        newWall.transform.localEulerAngles = new Vector3(0f, -1 * angle, 0f);
    }

    float getYangle(Vector3 startPoint, Vector3 endPoint)
    {
        float angle = 0;

        angle = Mathf.Atan(Mathf.Abs((endPoint.z - startPoint.z) / (endPoint.x - startPoint.x))) * Mathf.Rad2Deg;

        float xVal = endPoint.x - startPoint.x;
        float zVal = endPoint.z - startPoint.z;

        if (xVal >= 0f && zVal >= 0f)
        {
            angle = angle + 180;
        }
        else if (xVal < 0f && zVal < 0f)
        {
            angle = angle + 0f;
        }
        else if (xVal >= 0f && zVal < 0f)
        {
            angle = angle * -1;
        }
        else if (xVal < 0f && zVal >= 0f)
        {
            angle = angle * -1;
        }

        return angle;
    }

    Vector3 roundToNearest(Vector3 point, float increment)
    {
        float newX = Mathf.Round(point.x * (1 / increment)) * increment;
        float newZ = Mathf.Round(point.z * (1 / increment)) * increment;


        return new Vector3(newX, point.y, newZ);
    }

    GameObject CreateObject(string name, List<Vector3> points, float depth, Material whichMaterial)
    {

        if (depth < 0)
        { //TODO: Swap this into something that moves the plane down, or reverses triangles or vertices, for negatives
            depth = Mathf.Abs(depth);
        }

        int howManyVerts = points.Count; //Count the number of verts on a flat surface
        Vector3[] AllVerts = new Vector3[howManyVerts * 2]; //Twice as many verst as on a flab surface
        List<int> TriangleList = new List<int>(); //Gotta put all of the triangles somewhere - defined by the index of 3 verts in AllVerts

        for (int i = 0; i < howManyVerts; i++)
        {

            AllVerts[i] = new Vector3(points[i].x, 0, points[i].z); //Convert points to Vector3
            AllVerts[i + howManyVerts] = new Vector3(points[i].x, depth, points[i].z); //Replaces with depth

            if (i < howManyVerts - 1)
            {

                //First triangle
                TriangleList.Add(i + howManyVerts);
                TriangleList.Add(i + 1);
                TriangleList.Add(i);
                //Second triangle
                TriangleList.Add(i + howManyVerts + 1);
                TriangleList.Add(i + 1);
                TriangleList.Add(i + howManyVerts);
            }
            else {//For the final piece of wall
                  //First triangle
                TriangleList.Add(i + howManyVerts);
                TriangleList.Add(0);
                TriangleList.Add(i);
                //Second triangle
                TriangleList.Add(i + howManyVerts);
                TriangleList.Add(howManyVerts);
                TriangleList.Add(0);
            }
        }

        int[] allTriangles = new int[TriangleList.Count];
        for (int i = 0; i < TriangleList.Count; i++)
        {
            allTriangles[i] = TriangleList[i];
        }

        int[] reverseIndices = new int[allTriangles.Length];

        for (int i = 0; i < allTriangles.Length; i++)
        {
            reverseIndices[(allTriangles.Length) - i - 1] = allTriangles[i];
        }

        GameObject newObject = createMesh(name, AllVerts, allTriangles, whichMaterial);
        GameObject newObject2 = createMesh(name + "-reverse", AllVerts, reverseIndices, whichMaterial);

        return newObject;
    }

    GameObject CreateCap(string name, List<Vector3> points, Material whichMaterial, bool putAbove)
    {

        List<int> TriangleList = new List<int>(); //Gotta put all of the triangles somewhere - defined by the index of 3 verts in AllVerts

        //Create Vector2s to feed into triangulator
        Vector2[] TopCapVertices = new Vector2[4];
        Vector3[] TopCapVertices3 = new Vector3[4];
        Vector3[] pointArray = new Vector3[points.Count];

        float leftmost = points[0].x;
        float rightmost = points[0].x;
        float topmost = points[0].z;
        float bottommost = points[0].z;

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].x < leftmost)
                leftmost = points[i].x;
            if (points[i].x > rightmost)
                rightmost = points[i].x;
            if (points[i].z > topmost)
                topmost = points[i].z;
            if (points[i].z < bottommost)
                bottommost = points[i].z;
            //TopCapVertices[i] = new Vector2(points[i].x, points[i].z);
            pointArray[i] = points[i];
        }

        Debug.Log(pointArray);
        TopCapVertices[0] = new Vector2(leftmost, bottommost);
        TopCapVertices[1] = new Vector2(leftmost, topmost);
        TopCapVertices[2] = new Vector2(rightmost, topmost);
        TopCapVertices[3] = new Vector2(rightmost, bottommost);

        TopCapVertices3[0] = new Vector3(leftmost, pointArray[0].y, bottommost);
        TopCapVertices3[1] = new Vector3(leftmost, pointArray[0].y, topmost);
        TopCapVertices3[2] = new Vector3(rightmost, pointArray[0].y, topmost);
        TopCapVertices3[3] = new Vector3(rightmost, pointArray[0].y, bottommost);


        Vector3 centerPoint = new Vector3((leftmost + rightmost) / 2, (pointArray[0].y + .5f), (topmost + bottommost) / 2);
        if (putAbove == false)
        {
            centerPoint.y = pointArray[0].y - .5f;
        }


        float width = Vector2.Distance(new Vector2(leftmost, bottommost), new Vector2(rightmost, bottommost));

        float height = Vector2.Distance(new Vector2(leftmost, topmost), new Vector2(leftmost, bottommost));


        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;

        newObject.transform.position = centerPoint;
        newObject.transform.localScale = new Vector3(width + 1f, 1f, height + 1f);


        Debug.Log(TopCapVertices);

        Triangulator tr = new Triangulator(TopCapVertices);
        int[] indices = tr.Triangulate();

        int[] reverseIndices = new int[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            reverseIndices[(indices.Length) - i - 1] = indices[i];
        }

        // GameObject newObject = createMesh(name, TopCapVertices3, indices, whichMaterial);
        //GameObject newObject2 = createMesh(name + "-reverse", TopCapVertices3, reverseIndices, whichMaterial);

        return newObject;
    }

    GameObject createMesh(string name, Vector3[] vertices, int[] triangles, Material whichMaterial)
    {
        GameObject newObject = new GameObject(name);
        newObject.AddComponent<MeshFilter>();
        newObject.AddComponent<MeshRenderer>();
        Mesh newMesh = new Mesh();

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.Optimize();
        newMesh.RecalculateNormals();

        newObject.GetComponent<MeshFilter>().mesh = newMesh;

        newObject.AddComponent<MeshCollider>();

        //ADD MATERIAL
        newObject.GetComponent<Renderer>().material = whichMaterial;

        return newObject;
    }

    void createCaps(string name, List<Vector3> points, float heightOffset, Material whichMaterial, bool putAbove)
    {
        List<Vector3> allPoints = new List<Vector3>();
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 pointPosition = new Vector3(points[i].x, heightOffset, points[i].z);
            allPoints.Add(pointPosition);
        }

        GameObject topCap = CreateCap(name, allPoints, whichMaterial, putAbove);
    }


}
