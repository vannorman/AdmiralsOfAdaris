using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AoA
{

    public class TeamIdentifier : MonoBehaviour
    {

        public Constants.TeamColor color;
        public GameObject[] redIdentifierObjects;
        public GameObject[] blueIdentifierObjects;
        private void Start()
        {
            foreach (var ti in GetComponentsInChildren<TeamIdentifier>())
            {
                ti.color = color;
            }

            if (color == Constants.TeamColor.Red)
            {
                foreach (var o in redIdentifierObjects)
                {
                    o.SetActive(true);
                }
            }
            else if (color == Constants.TeamColor.Blue)
            {
                foreach (var o in blueIdentifierObjects)
                {
                    o.SetActive(true);
                }
            }
        }
    }
}
