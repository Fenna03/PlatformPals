using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class fanOnOff : onOffScript
{
    public AreaEffector2D areaEffector2D;

    // Start is called before the first frame update
    public override void Start()
    {
        anim = GetComponent<Animator>();
        areaEffector2D = GetComponent<AreaEffector2D>();
    }

    public override void On()
    {
        anim.SetBool("ifActivated", true);
        areaEffector2D.forceMagnitude = 15;
    }
    public override void Off()
    {
        anim.SetBool("ifActivated", false);
        areaEffector2D.forceMagnitude = 0;
    }
}
