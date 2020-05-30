
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyProjectileThunder : AllyProjectile
{
    public override string[] defaultPaths {get;} = new string[3];
    public override string[] prefabPaths {get; set;} = new string[3];


    public override string[] modifyNamesLevel1 {get; set;} = {};
    public override Action<GameObject, int, int, Color> modifyLevel1Action {get; set;}

    public override string[] modifyNamesLevel2 {get; set;} = {};
    public override Action<GameObject, int, int, Color> modifyLevel2Action {get; set;}

    public override string[] modifyNamesLevel3 {get; set;} = {};
    public override Action<GameObject, int, int, Color> modifyLevel3Action {get; set;}

    protected override void Start() {
        base.Start();

        modifyLevel1Action = new Action<GameObject, int, int, Color>(defaultModifyAction);
        modifyLevel2Action = new Action<GameObject, int, int, Color>(defaultModifyAction);
        modifyLevel3Action = new Action<GameObject, int, int, Color>(defaultModifyAction);
    }
}
