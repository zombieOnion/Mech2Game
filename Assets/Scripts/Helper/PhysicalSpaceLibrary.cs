using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class PhysicalSpaceLibrary {

    public static Collider[] CheckForOverlap(Transform transform, LayerMask layer) => Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, layer);
    public static Collider[] CheckForOverlap(Vector3 point, Transform transform, LayerMask layer) => Physics.OverlapBox(point, transform.localScale / 2, Quaternion.identity, layer);
}
