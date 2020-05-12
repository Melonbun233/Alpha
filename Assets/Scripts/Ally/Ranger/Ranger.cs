using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Ally
{
    [Header("Attack Indicator Settings")]
    public float rotateSpeed;
    public float attackRotateSpeed;
    public float rotateSpeedUpPeriod;
    private float _initialRotateSpeed;

    // Core color when attacking
    public Color attackCoreColor;
    private Color _initialCoreColor;
    

    private LineRenderer _attackIndicator;
    private GameObject _core;

    protected override void Start() {
        base.Start();
        
        _initialRotateSpeed = rotateSpeed;

        // Set the start position of the attack indicator
        _core = transform.Find("Core").gameObject;
        _attackIndicator = GetComponent<LineRenderer>();
        _attackIndicator.SetPosition(0, _core.transform.position);

        _initialCoreColor = _core.GetComponent<Renderer>().material.color;
    }

    protected override void Update() {
        base.Update();

        rotate();
        updateAttackIndicator();
    }

    // Rotate when idle. rotate faster when attack
    private void rotate() {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public override void updateAttackTarget() {
        base.updateAttackTarget();

        if (_attackTarget != null) {
            // speed up rotation for rotate speed up period
            rotateSpeed = attackRotateSpeed;
            CancelInvoke("resetRotateSpeed");
            Invoke("resetRotateSpeed", rotateSpeedUpPeriod);
        }
    }

    public override void attack() {
        base.attack();

        // Change the color of core for 0.1 second
        _core.GetComponent<Renderer>().material.SetColor("_BaseColor", attackCoreColor);
        // No need to cancel invoke as attack speed is slower than this
        Invoke("resetCoreColor", 0.1f);
    }

    private void resetRotateSpeed() {
        rotateSpeed = _initialRotateSpeed;
    }

    private void resetCoreColor() {
        _core.GetComponent<Renderer>().material.SetColor("_BaseColor", _initialCoreColor);
    }

    private void updateAttackIndicator() {
        // check if the ranger has a target
        if (_attackTarget == null) {
            _attackIndicator.GetComponent<LineRenderer>().SetPosition(1, _core.transform.position);
        } else {
            _attackIndicator.GetComponent<LineRenderer>().SetPosition(1, 
                _attackTarget.transform.TransformPoint(_attackTarget.GetComponent<Destroyable>().center));
        }
    }

}
