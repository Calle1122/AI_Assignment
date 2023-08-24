using AI;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carl_Lindstedt
{
    public class Unit_Carl_Lindstedt : Unit
    {
        #region Properties

        public new Team_Carl_Lindstedt Team => base.Team as Team_Carl_Lindstedt;

        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            // pick a random target!
            return enemiesInRange != null && enemiesInRange.Count > 0 ? 
                enemiesInRange[Random.Range(0, enemiesInRange.Count)] : null;
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(StupidLogic());
        }

        IEnumerator StupidLogic()
        {
            while (true)
            {
                // wait (or take cover)
                TargetNode = null;
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

                // move randomly
                TargetNode = Battlefield.Instance.GetRandomNode();
                yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));
            }
        }
    }
}