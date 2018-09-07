using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plx : MonoBehaviour {
    private static GameObject[] layers;

    //private List<Rigidbody2D> _rigidbody;
    private Rigidbody2D[] _rigidbody;
    private Rigidbody2D rigidbodyPlayer;
    private float lastX;
    private static float playerCurX;
    private static float VEL_TEST;
    private static float[] speeds;

    public float[] speedSrcs;
    // public float speed = 1;
    private float TMP_CONST_L = 0f; //34.121709
    private float TMP_CONST_R = 0f;


    // Use this for initialization
    void Start () {
        //this._rigidbody = new List<Rigidbody2D>();


        this.lastX = PlayerController.sharedInstance.transform.position.x;
        this.rigidbodyPlayer = PlayerController.sharedInstance.GetComponent<Rigidbody2D>();

        GameObject[] list = GameObject.FindGameObjectsWithTag("bg");
        Debug.Log("Found " + list.Length + " bg layers" );

        this._rigidbody = new Rigidbody2D[list.Length];
        speeds = new float[list.Length];
        var ct = 0;
        foreach(GameObject i in list) {
            // Debug.Log(i.GetType() + " Tag => " + i.tag +" =>" + i.name + " possition => " + i.transform.position.x );
            Rigidbody2D rb = i.GetComponent<Rigidbody2D>();
            //Debug.Log(rb.GetType() + "  " + rb.transform.position.x);
            this._rigidbody[ct] = rb;
            speeds[ct] = calculateLayerSpeed(i.name);
            ct++;
            if ( i.transform.position.x > 10 ) {
                TMP_CONST_L = -i.transform.position.x;
                TMP_CONST_R = i.transform.position.x;

            }

            //Debug.Log("TMP_CONST_L => " + TMP_CONST_L);
        }
    }

    private float calculateLayerSpeed(string strName) {
        float _s = 0;
        if (strName.IndexOf("00_",System.StringComparison.OrdinalIgnoreCase ) > -1 ) {
            _s = speedSrcs[0];
        } else if (strName.IndexOf ("01_", System.StringComparison.OrdinalIgnoreCase) > -1) {
            _s = speedSrcs[1];
        } else if (strName.IndexOf("02_", System.StringComparison.OrdinalIgnoreCase) > -1) {
            _s = speedSrcs[2];
        }
        return _s;
    }

    private void Update()
    {
         //playerCurX = PlayerController.sharedInstance.transform.position.x;
         //VEL_TEST = this.rigidbodyPlayer.velocity.x >= 0 ? 1 : -1; // this.rigidbodyPlayer.velocity.x;//2.5f; //Mathf.Abs(this.rigidbodyPlayer.velocity.x);
    }


    private void FixedUpdate()
    {
        playerCurX = PlayerController.sharedInstance.transform.position.x;
        VEL_TEST = this.rigidbodyPlayer.velocity.x >= 0 ? 1 : -1; // this.rigidbodyPlayer.velocity.x;//2.5f; //Mathf.Abs(this.rigidbodyPlayer.velocity.x);

        for (int ct = 0; ct < _rigidbody.Length; ct++)
        {
            Rigidbody2D layer = _rigidbody[ct];
            float parentPossition = this.transform.position.x;

            // Check if Player is not moving
            layer.velocity = (Mathf.Approximately(playerCurX, lastX)) ? layer.velocity = new Vector2(0, 0) : layer.velocity = new Vector2(VEL_TEST * speeds[ct], 0);
            if (layer.transform.position.x - parentPossition < TMP_CONST_L)
            {
                float diff = (Mathf.Abs((layer.transform.position.x - parentPossition) - TMP_CONST_L));
                layer.transform.position = new Vector3( (parentPossition - diff ) - TMP_CONST_L , layer.transform.position.y, layer.transform.position.z);
            } else if (layer.transform.position.x - parentPossition > TMP_CONST_R) {
                float diff = (Mathf.Abs((layer.transform.position.x - parentPossition) - TMP_CONST_R));
                layer.transform.position = new Vector3( (parentPossition + diff ) - TMP_CONST_R, layer.transform.position.y, layer.transform.position.z);
            }
        }

        lastX = playerCurX;

    }
}
