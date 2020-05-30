using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Ally
{
    [Header("Ranger Attack Indicator Settings")]
    public float rotateSpeed;
    public float attackRotateSpeed;
    public float rotateSpeedUpPeriod;
    private float _initialRotateSpeed;

    // Core color when attacking
    public Color attackCoreColor;
    private Color _initialCoreColor;

    public Vector3 attackPosition;
    

    private List<LineRenderer> _lineRenderers = new List<LineRenderer>();
    private GameObject _core;

    private AllyProjectileController _projectileController;

    protected override void Start() {
        base.Start();
        
        _initialRotateSpeed = rotateSpeed;

        // Set the start position of the attack indicator
        _core = transform.Find("Core").gameObject;

        _initialCoreColor = _core.GetComponent<Renderer>().material.color;

        _projectileController = gameObject.GetComponent<AllyProjectileController>();
    }

    protected override void Update() {
        base.Update();

        rotate();
        updateAttackIndicators();
    }

    // Rotate when idle. rotate faster when attack
    private void rotate() {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public override void updateAttackTarget() {
        base.updateAttackTarget();

        if (_attackTargets.Count != 0) {
            // speed up rotation for rotate speed up period
            rotateSpeed = attackRotateSpeed;
            CancelInvoke("resetRotateSpeed");
            Invoke("resetRotateSpeed", rotateSpeedUpPeriod);
        }
    }

    public override void attack() {
        if (_projectileController == null) {
            Debug.LogWarning("No projectile controller attached");
            base.attack();
        } else {
            foreach(GameObject target in _attackTargets) {
                if (target == null) {
                    continue;
                }

                GameObject obj = _projectileController.spawnAllyProjectile(
                    transform.TransformPoint(attackPosition), Quaternion.identity, gameObject, target);
                obj.transform.LookAt(target.transform.TransformPoint(target.GetComponent<Unit>().center));
            }
            _attackCoolDown = attackData.attackCoolDown;
        }
        

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

    private void addAttackIndicators(int number) {
        for (int i = 0; i < number; i ++) {
            GameObject indicator = Instantiate(attackIndicator, _core.transform.position,
                transform.rotation, transform);
            LineRenderer lr = indicator.GetComponent<LineRenderer>();
            lr.SetPosition(0, _core.transform.position);
            lr.SetPosition(1, _core.transform.position);
            _lineRenderers.Add(lr);
        }
    }

    private void updateAttackIndicators() {
        if (_lineRenderers.Count < attackData.attackNumber) {
            addAttackIndicators(attackData.attackNumber - _lineRenderers.Count);
        }

        // clear all attack indicators
        foreach (LineRenderer lr in _lineRenderers) {
            lr.SetPosition(1, _core.transform.position);
        }

        for(int i = 0; i < _attackTargets.Count; i ++) {
            if (_attackTargets[i] != null) {
                _lineRenderers[i].SetPosition(1, 
                    _attackTargets[i].transform.TransformPoint(
                    _attackTargets[i].GetComponent<Destroyable>().center));
            }
        }
    }

}
