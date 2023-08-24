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
        private int m_formationNumber;
        
        #region Properties

        public new Team_Carl_Lindstedt Team => base.Team as Team_Carl_Lindstedt;

        public int FormationNumber
        {
            get => m_formationNumber;

            set
            {
                m_formationNumber = value;
            }
        }
        
        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            // pick a random target!
            return enemiesInRange != null && enemiesInRange.Count > 0 ? 
                enemiesInRange[Random.Range(0, enemiesInRange.Count)] : null;
        }

        protected override GraphUtils.Path GetPathToTarget()
        {
            return Team.GetShortestPath(CurrentNode, TargetNode);
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(PathfindingLogic());
        }

        IEnumerator PathfindingLogic()
        {
            while (true)
            {
                //if you are the squad leader... pathfind towards target
                if (FormationNumber == 0 && Team.formationFollow)
                {
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, Team.squadLeaderTargetPoint.transform.position);
                    yield return new WaitForSeconds(.1f);
                }

                else
                {
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, Team.targetPoints[FormationNumber].transform.position);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }
}