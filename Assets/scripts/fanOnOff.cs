using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class fanOnOff : NetworkBehaviour
{
    public Animator anim;
    public AreaEffector2D areaEffector2D;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        areaEffector2D = GetComponent<AreaEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void On()
    {
        anim.SetBool("ifActivated", true);
        areaEffector2D.forceMagnitude = 15;
    }
    public void Off()
    {
        anim.SetBool("ifActivated", false);
        areaEffector2D.forceMagnitude = 0;
    }
}
